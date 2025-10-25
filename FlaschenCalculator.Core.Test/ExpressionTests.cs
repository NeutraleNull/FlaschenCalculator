using FlaschenCalculator.Core;
using FlaschenCalculator.Core.Models;

namespace FlaschenCalculator.Core.Test;

/* Test have been AI generated but refined manually
 */


public class ExpressionTests
{
    [Test]
    public void NumberExpression_Evaluate_ReturnsCorrectValue()
    {
        var expr = new NumberExpression(42);
        Assert.That(expr.Evaluate(), Is.EqualTo(42));
    }

    [Test]
    public void BinaryExpression_Addition_ReturnsSum()
    {
        var left = new NumberExpression(5);
        var right = new NumberExpression(3);
        var expr = new BinaryExpression(Operation.Addition, left, right);
        Assert.That(expr.Evaluate(), Is.EqualTo(8));
    }

    [Test]
    public void BinaryExpression_Subtraction_ReturnsDifference()
    {
        var left = new NumberExpression(10);
        var right = new NumberExpression(3);
        var expr = new BinaryExpression(Operation.Subtraction, left, right);
        Assert.That(expr.Evaluate(), Is.EqualTo(7));
    }

    [Test]
    public void BinaryExpression_Multiplication_ReturnsProduct()
    {
        var left = new NumberExpression(4);
        var right = new NumberExpression(5);
        var expr = new BinaryExpression(Operation.Multiplication, left, right);
        Assert.That(expr.Evaluate(), Is.EqualTo(20));
    }

    [Test]
    public void BinaryExpression_Division_ReturnsQuotient()
    {
        var left = new NumberExpression(15);
        var right = new NumberExpression(3);
        var expr = new BinaryExpression(Operation.Division, left, right);
        Assert.That(expr.Evaluate(), Is.EqualTo(5));
    }

    [Test]
    public void BinaryExpression_DivisionByZero_ThrowsException()
    {
        var left = new NumberExpression(10);
        var right = new NumberExpression(0);
        var expr = new BinaryExpression(Operation.Division, left, right);
        Assert.That(() => expr.Evaluate(), Throws.TypeOf<InvalidOperationDivideByZeroException>());
    }

    [Test]
    public void UnaryExpression_Addition_ReturnsPositiveValue()
    {
        var operand = new NumberExpression(5);
        var expr = new UnaryExpression(Operation.Addition, operand);
        Assert.That(expr.Evaluate(), Is.EqualTo(5));
    }

    [Test]
    public void UnaryExpression_Subtraction_ReturnsNegativeValue()
    {
        var operand = new NumberExpression(5);
        var expr = new UnaryExpression(Operation.Subtraction, operand);
        Assert.That(expr.Evaluate(), Is.EqualTo(-5));
    }

    [Test]
    public void UnaryExpression_Multiplication_ThrowsException()
    {
        var operand = new NumberExpression(5);
        var expr = new UnaryExpression(Operation.Multiplication, operand);
        Assert.That(() => expr.Evaluate(), Throws.TypeOf<InvalidOperationException<UnaryExpression>>());
    }

    [Test]
    public void UnaryExpression_Division_ThrowsException()
    {
        var operand = new NumberExpression(5);
        var expr = new UnaryExpression(Operation.Division, operand);
        Assert.That(() => expr.Evaluate(), Throws.TypeOf<InvalidOperationException<UnaryExpression>>());
    }

    [Test]
    public void ComplexExpression_NestedOperations_EvaluatesCorrectly()
    {
        // (5 + 3) * 2 = 16
        var left = new BinaryExpression(Operation.Addition, new NumberExpression(5), new NumberExpression(3));
        var right = new NumberExpression(2);
        var expr = new BinaryExpression(Operation.Multiplication, left, right);
        Assert.That(expr.Evaluate(), Is.EqualTo(16));
    }

    [Test]
    public void ComplexExpression_WithUnary_EvaluatesCorrectly()
    {
        // -(5 + 3) = -8
        var innerExpr = new BinaryExpression(Operation.Addition, new NumberExpression(5), new NumberExpression(3));
        var expr = new UnaryExpression(Operation.Subtraction, innerExpr);
        Assert.That(expr.Evaluate(), Is.EqualTo(-8));
    }

    // ===== Power Operation Tests =====

    [Test]
    public void BinaryExpression_Power_ReturnsCorrectValue()
    {
        // 2^3 = 8
        var left = new NumberExpression(2);
        var right = new NumberExpression(3);
        var expr = new BinaryExpression(Operation.Power, left, right);
        Assert.That(expr.Evaluate(), Is.EqualTo(8));
    }

    [Test]
    public void BinaryExpression_Power_LargerValues()
    {
        // 5^2 = 25
        var left = new NumberExpression(5);
        var right = new NumberExpression(2);
        var expr = new BinaryExpression(Operation.Power, left, right);
        Assert.That(expr.Evaluate(), Is.EqualTo(25));
    }

    [Test]
    public void BinaryExpression_Power_ZeroExponent()
    {
        // 10^0 = 1
        var left = new NumberExpression(10);
        var right = new NumberExpression(0);
        var expr = new BinaryExpression(Operation.Power, left, right);
        Assert.That(expr.Evaluate(), Is.EqualTo(1));
    }

    [Test]
    public void BinaryExpression_Power_FractionalExponent()
    {
        // 4^0.5 = 2 (square root)
        var left = new NumberExpression(4);
        var right = new NumberExpression(0.5m);
        var expr = new BinaryExpression(Operation.Power, left, right);
        Assert.That(expr.Evaluate(), Is.EqualTo(2));
    }

    [Test]
    public void BinaryExpression_Power_NegativeBase()
    {
        // (-2)^3 = -8
        var left = new NumberExpression(-2);
        var right = new NumberExpression(3);
        var expr = new BinaryExpression(Operation.Power, left, right);
        Assert.That(expr.Evaluate(), Is.EqualTo(-8));
    }

    // ===== Square Root Operation Tests =====

    [Test]
    public void UnaryExpression_SquareRoot_ReturnsCorrectValue()
    {
        // sqrt(4) = 2
        var operand = new NumberExpression(4);
        var expr = new UnaryExpression(Operation.SquareRoot, operand);
        Assert.That(expr.Evaluate(), Is.EqualTo(2));
    }

    [Test]
    public void UnaryExpression_SquareRoot_LargerValue()
    {
        // sqrt(25) = 5
        var operand = new NumberExpression(25);
        var expr = new UnaryExpression(Operation.SquareRoot, operand);
        Assert.That(expr.Evaluate(), Is.EqualTo(5));
    }

    [Test]
    public void UnaryExpression_SquareRoot_PerfectSquare()
    {
        // sqrt(100) = 10
        var operand = new NumberExpression(100);
        var expr = new UnaryExpression(Operation.SquareRoot, operand);
        Assert.That(expr.Evaluate(), Is.EqualTo(10));
    }

    [Test]
    public void UnaryExpression_SquareRoot_NegativeNumberThrows()
    {
        // sqrt(-4) should throw an exception
        var operand = new NumberExpression(-4);
        var expr = new UnaryExpression(Operation.SquareRoot, operand);
        Assert.That(() => expr.Evaluate(), Throws.TypeOf<InvalidOperationException<UnaryExpression>>());
    }

    [Test]
    public void UnaryExpression_SquareRoot_OfExpression()
    {
        // sqrt(9 + 16) = sqrt(25) = 5
        var innerExpr = new BinaryExpression(Operation.Addition, new NumberExpression(9), new NumberExpression(16));
        var expr = new UnaryExpression(Operation.SquareRoot, innerExpr);
        Assert.That(expr.Evaluate(), Is.EqualTo(5));
    }

    // ===== Complex Expressions with Power and SquareRoot =====

    [Test]
    public void ComplexExpression_PowerAndSquareRoot()
    {
        // 2^2 + sqrt(16) = 4 + 4 = 8
        var powerExpr = new BinaryExpression(Operation.Power, new NumberExpression(2), new NumberExpression(2));
        var sqrtExpr = new UnaryExpression(Operation.SquareRoot, new NumberExpression(16));
        var expr = new BinaryExpression(Operation.Addition, powerExpr, sqrtExpr);
        Assert.That(expr.Evaluate(), Is.EqualTo(8));
    }

    [Test]
    public void ComplexExpression_SquareRootOfPower()
    {
        // sqrt(3^2) = sqrt(9) = 3
        var powerExpr = new BinaryExpression(Operation.Power, new NumberExpression(3), new NumberExpression(2));
        var expr = new UnaryExpression(Operation.SquareRoot, powerExpr);
        Assert.That(expr.Evaluate(), Is.EqualTo(3));
    }

    [Test]
    public void ComplexExpression_PowerOfSquareRoot()
    {
        // sqrt(4)^2 = 2^2 = 4
        var sqrtExpr = new UnaryExpression(Operation.SquareRoot, new NumberExpression(4));
        var expr = new BinaryExpression(Operation.Power, sqrtExpr, new NumberExpression(2));
        Assert.That(expr.Evaluate(), Is.EqualTo(4));
    }

    [Test]
    public void ComplexExpression_NestedPowerOperations()
    {
        // 2^(3^2) = 2^9 = 512
        var innerPower = new BinaryExpression(Operation.Power, new NumberExpression(3), new NumberExpression(2));
        var expr = new BinaryExpression(Operation.Power, new NumberExpression(2), innerPower);
        Assert.That(expr.Evaluate(), Is.EqualTo(512));
    }

    [Test]
    public void ComplexExpression_PythagoreanTheorem()
    {
        // sqrt(3^2 + 4^2) = sqrt(9 + 16) = sqrt(25) = 5
        var power1 = new BinaryExpression(Operation.Power, new NumberExpression(3), new NumberExpression(2));
        var power2 = new BinaryExpression(Operation.Power, new NumberExpression(4), new NumberExpression(2));
        var addition = new BinaryExpression(Operation.Addition, power1, power2);
        var expr = new UnaryExpression(Operation.SquareRoot, addition);
        Assert.That(expr.Evaluate(), Is.EqualTo(5));
    }
}
