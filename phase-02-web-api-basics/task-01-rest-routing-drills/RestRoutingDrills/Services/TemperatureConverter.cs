using RestRoutingDrills.Interfaces;

namespace RestRoutingDrills.Services;

public class TemperatureConverter : ITemperatureConverter
{
    public double ConvertToFahrenheit(double celsius)
    {
        return (celsius * 9 / 5) + 32;
    }
}
