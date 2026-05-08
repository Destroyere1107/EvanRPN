using EvanRPN;

// This is a special 'Testing' program that directly exposes the Engine
// as a console app. This is not meant for production use.


var engine = new Engine();

Console.WriteLine("evanRPN Testing Program — type a number to push, or an operator to execute.");
Console.WriteLine("Commands: +, -, *, /, %, ^, neg, abs, sqrt, xroot, 1/x,");
Console.WriteLine("          swap, dup, drop, clear, pi, e, quit");
Console.WriteLine();

while (true)
{
    // Show the stack
    var stack = engine.GetStack().ToArray();
    if (stack.Length == 0)
        Console.WriteLine("  (empty)");
    else
        for (var i = stack.Length - 1; i >= 0; i--)
            Console.WriteLine($"  {(i == 0 ? "►" : " ")} {stack[i]}");

    Console.Write("> ");
    var input = Console.ReadLine();

    if (input is null)
        break;

    input = input.Trim();
    if (input.Length == 0)
        continue;

    if (input.Equals("quit", StringComparison.OrdinalIgnoreCase) ||
        input.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    // Try parsing as a number first
    if (double.TryParse(input, out var number))
    {
        engine.Push(number);
        continue;
    }

    // Otherwise treat it as an operator
    try
    {
        if (!engine.Execute(input))
            Console.WriteLine($"  Unknown command: {input}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  Error: {ex.Message}");
    }
}