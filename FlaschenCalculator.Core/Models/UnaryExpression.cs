namespace FlaschenCalculator.Core.Models;

public sealed class UnaryExpression(Operation operation, IExpression right) : IExpression
{
    public decimal Evaluate()
    {
        var r = right.Evaluate();
        return operation switch
        {
            Operation.Addition => r,
            Operation.Subtraction => -r,
            Operation.Multiplication => throw new InvalidOperationException<UnaryExpression>(operation, this), //* something is not a valid operation without left operand
            Operation.Division => throw new InvalidOperationException<UnaryExpression>(operation, this),       // same goes for division,
            Operation.SquareRoot => r < 0  // NEW
                ? throw new InvalidOperationException<UnaryExpression>(operation, this) // we don't do imaginary numbers here
                : (decimal)Math.Sqrt((double)r),
            _ => throw new NotImplementedException()
        };
    }

    public string ToString(string indent = "", bool isRight = false)
    {
        string opSymbol = operation switch
        {
            Operation.Addition => "+",
            Operation.Subtraction => "-",
            Operation.Multiplication => "*",
            Operation.Division => "/",
            Operation.SquareRoot => "sqrt",
            _ => "?"
        };

        // Build tree-like output
        var builder = new System.Text.StringBuilder();
        builder.AppendLine($"{indent}{opSymbol}");
        builder.Append(right.ToString(indent + "└─ ", true));
        return builder.ToString();
    }
}
