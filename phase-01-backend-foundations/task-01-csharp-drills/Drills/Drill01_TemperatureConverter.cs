namespace CSharpDrills.Drills;

public static class Drill01_TemperatureConverter
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 01 - Temperature Converter ===");
        Console.Write("Enter temperature in Celsius: ");

        string? input = Console.ReadLine();

        // Read input as string first, then parse safely with TryParse.
        // This prevents crashes on empty or non-numeric values.
        if (!double.TryParse(input, out double celsius))
        {
            Console.WriteLine("Invalid temperature value.");
            return;
        }

        // Conversion formula: Fahrenheit = Celsius * 9 / 5 + 32
        double fahrenheit = (celsius * 9 / 5) + 32;

        // F2 keeps two decimal places as required.
        Console.WriteLine($"{celsius}°C = {fahrenheit:F2}°F");
    }
}
