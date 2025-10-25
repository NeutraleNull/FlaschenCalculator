using FlaschenCalculator.Infrastructure;

Console.WriteLine("Welcome to the FlaschenCalculator!");
Thread.Sleep(1000);
Console.WriteLine("Please enter a mathematical expression to calculate, allowed operators are +, -, *, /, ^ and sqrt. You can also use parentheses ( ).");
Thread.Sleep(1000);
Console.WriteLine("Type 'exit' to quit the calculator.");
Console.WriteLine();
Console.WriteLine("Example: 2 + 3 * (4 - 1)");
Console.WriteLine();
Thread.Sleep(1000);

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    if (input == "exit")
    {
        Thread.Sleep(1000);
        Console.WriteLine("Goodbye!");
        Thread.Sleep(2000);
        return 0;
    }
    
    try
    {
        var expression = NumberParser.Parse(input);
        var result = expression.Evaluate();
        Console.WriteLine($"Result: {result}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    
}

return 0;
