namespace FlaschenCalculator.Core.Models;

public sealed class BinaryExpression(Operation operation, IExpression left, IExpression right) : IExpression
{
    public decimal Evaluate()
    {
        var l = left.Evaluate();
        var r = right.Evaluate();
        return operation switch
        {
            Operation.Addition => l + r,
            Operation.Subtraction => l - r,
            Operation.Multiplication => l * r,
            Operation.Division => r == 0  // division by zero is not allowed
                ? throw new InvalidOperationDivideByZeroException(operation, l, r)
                : l / r,
            Operation.Power => (decimal)Math.Pow((double)l, (double)r),  // NEW
            _ => throw new NotImplementedException()
        };
    }

    public string ToString(string indent = "", bool isRight = false)
    {
        var opSymbol = operation switch
        {
            Operation.Addition => "+",
            Operation.Subtraction => "-",
            Operation.Multiplication => "*",
            Operation.Division => "/",
            Operation.Power => "^",
            _ => "?"
        };

        // Build tree-like output
        var builder = new System.Text.StringBuilder();
        builder.AppendLine($"{indent}{opSymbol}");
        builder.Append(left.ToString(indent + "├─ ", false));
        builder.Append(right.ToString(indent + "└─ ", true));
        return builder.ToString();
    }
}
