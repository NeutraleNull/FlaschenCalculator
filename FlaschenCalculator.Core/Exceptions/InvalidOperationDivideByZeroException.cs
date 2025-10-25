using FlaschenCalculator.Core.Models;

namespace FlaschenCalculator.Core;

public class InvalidOperationDivideByZeroException : DivideByZeroException
{
    public InvalidOperationDivideByZeroException(Operation operation, decimal left, decimal right) : base(
        $"Division by zero is not allowed for operation {operation} with operands {left} and {right}.")
    {
    }
}
