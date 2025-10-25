namespace FlaschenCalculator.Core;

public interface IExpression
{
    public decimal Evaluate();
    public string ToString(string indent = "", bool isRight = false);
    
    public string GetExpressionType() => this.GetType().Name;
}
