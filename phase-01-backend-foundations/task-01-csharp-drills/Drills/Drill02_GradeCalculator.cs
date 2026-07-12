namespace CSharpDrills.Drills;

public static class Drill02_GradeCalculator
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 02 - Grade Calculator ===");
        Console.Write("Enter score (0-100): ");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int score))
        {
            Console.WriteLine("Score must be a valid number.");
            return;
        }

        if (score < 0 || score > 100)
        {
            Console.WriteLine("Score must be between 0 and 100.");
            return;
        }

        string grade;
        if (score >= 90) grade = "A";
        else if (score >= 80) grade = "B";
        else if (score >= 70) grade = "C";
        else if (score >= 60) grade = "D";
        else grade = "F";

        Console.WriteLine($"Grade: {grade}");
    }
}
