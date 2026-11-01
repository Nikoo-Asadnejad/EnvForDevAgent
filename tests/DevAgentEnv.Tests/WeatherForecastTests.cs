using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class WeatherForecastTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client;

    public WeatherForecastTests(WebApplicationFactory<Program> factory)
    {
        client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    [Theory]
    [InlineData("Canada", "Canada")]
    [InlineData("United Kingdom", "United Kingdom")]
    [InlineData("Côte d'Ivoire", "Côte d'Ivoire")]
    [InlineData("  Japan  ", "Japan")]
    public async Task CountryRouteReturnsOneForecast(string country, string expectedCountry)
    {
        var earliestDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var response = await client.GetAsync($"/weatherforecast/{Uri.EscapeDataString(country)}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var forecast = document.RootElement;
        Assert.Equal(JsonValueKind.Object, forecast.ValueKind);
        Assert.Equal(expectedCountry, forecast.GetProperty("country").GetString());
        Assert.InRange(forecast.GetProperty("date").Deserialize<DateOnly>(),
            earliestDate, DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
        var temperature = forecast.GetProperty("temperatureC").GetInt32();
        Assert.InRange(temperature, -20, 54);
        Assert.Equal(32 + (int)(temperature / 0.5556), forecast.GetProperty("temperatureF").GetInt32());
        Assert.Contains(forecast.GetProperty("summary").GetString(), new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        });
    }

    [Fact]
    public async Task BlankCountryReturnsBadRequest()
    {
        var response = await client.GetAsync("/weatherforecast/%20%20");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task OriginalRouteStillReturnsFiveForecasts()
    {
        var response = await client.GetAsync("/weatherforecast");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(5, document.RootElement.GetArrayLength());
        foreach (var forecast in document.RootElement.EnumerateArray())
        {
            Assert.False(forecast.TryGetProperty("country", out _));
        }
    }
}
