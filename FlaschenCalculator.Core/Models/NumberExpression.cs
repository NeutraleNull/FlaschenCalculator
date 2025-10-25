namespace FlaschenCalculator.Core.Models;

public sealed class NumberExpression(decimal value) : IExpression
{
    public decimal Evaluate()
    {
        return value;
    }

    public string ToString(string indent = "", bool isRight = false)
    {
        return $"{indent}{value}\n";
    }
}
