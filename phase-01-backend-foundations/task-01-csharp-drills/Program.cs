using CSharpDrills.Drills;

while (true)
{
    Console.WriteLine("\n=== C# Logic Drills ===");
    for (int i = 1; i <= 20; i++)
        Console.WriteLine($"{i,2}. Drill {i:D2}");
    Console.WriteLine(" 0. Exit");
    Console.Write("Choose drill: ");

    if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Invalid input.");
        continue;
    }

    if (choice == 0) break;

    Action? drill = choice switch
    {
        1 => Drill01_TemperatureConverter.Run,
        2 => Drill02_GradeCalculator.Run,
        3 => Drill03_LoginValidator.Run,
        4 => Drill04_EvenOddAnalyzer.Run,
        5 => Drill05_MaxMinFinder.Run,
        6 => Drill06_WordCounter.Run,
        7 => Drill07_NameFormatter.Run,
        8 => Drill08_PasswordStrengthChecker.Run,
        9 => Drill09_ShoppingCartTotal.Run,
        10 => Drill10_SimpleAtmMenu.Run,
        11 => Drill11_DuplicateNumberDetector.Run,
        12 => Drill12_EmailValidator.Run,
        13 => Drill13_PalindromeChecker.Run,
        14 => Drill14_SimpleExpenseTracker.Run,
        15 => Drill15_ArrayRotation.Run,
        16 => Drill16_FrequencyCounter.Run,
        17 => Drill17_SimpleSearchEngine.Run,
        18 => Drill18_NumberStatistics.Run,
        19 => Drill19_TicketPriceCalculator.Run,
        20 => Drill20_MethodRefactoringChallenge.Run,
        _ => null
    };

    if (drill is null)
        Console.WriteLine("Drill not found.");
    else
        drill();
}
