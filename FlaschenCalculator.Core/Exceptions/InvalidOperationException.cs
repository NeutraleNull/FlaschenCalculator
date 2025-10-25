using FlaschenCalculator.Core.Models;

namespace FlaschenCalculator.Core;

public class InvalidOperationException<T> : Exception where T : IExpression
{
    public InvalidOperationException(Operation operation, T expression) : base(
        $"Operation {operation} is not supported for {expression.GetExpressionType()}.")
    {
    }
}
