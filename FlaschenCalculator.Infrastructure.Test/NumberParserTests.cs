using FlaschenCalculator.Core;
using FlaschenCalculator.Core.Models;
using FlaschenCalculator.Infrastructure;

namespace FlaschenCalculator.Infrastructure.Test;

/* Test have been AI generated but refined manually
 */

public class NumberParserTests
{
    [Test]
    public void SimpleAddition()
    {
        var expr = NumberParser.Parse("1 + 2");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void AdditionAndSubtraction()
    {
        var expr = NumberParser.Parse("10 + 5 - 3");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(12));
    }

    [Test]
    public void MultiplicationAndDivision()
    {
        var expr = NumberParser.Parse("8 * 3 / 4");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(6));
    }

    [Test]
    public void OperatorPrecedence()
    {
        var expr = NumberParser.Parse("2 + 3 * 4");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(14));
    }

    [Test]
    public void ParenthesesOverridePrecedence()
    {
        var expr = NumberParser.Parse("(2 + 3) * 4");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(20));
    }
    
    [Test]
    public void ParenthesesOverridePrecedence2()
    {
        var expr = NumberParser.Parse("(2)- (2 + 3) * 4");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(-18));
    }

    [Test]
    public void NestedParentheses()
    {
        var expr = NumberParser.Parse("((1 + 2) * (3 + 4)) / (2 + 5)");
        Console.WriteLine(expr.ToString());
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void UnaryMinus()
    {
        var expr = NumberParser.Parse("-5 + 3");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(-2));
    }

    [Test]
    public void DoubleUnaryMinus()
    {
        var expr = NumberParser.Parse("--5");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(5));
    }

    [Test]
    public void NegativeMultiplication()
    {
        var expr = NumberParser.Parse("(-2) * (-4)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(8));
    }

    [Test]
    public void DivisionByZeroThrows()
    {
        Assert.That(() =>
        {
            var expr = NumberParser.Parse("10 / 0");
            expr.Evaluate();
        }, Throws.TypeOf<InvalidOperationDivideByZeroException>());
    }

    [Test]
    public void FloatingPointDivision()
    {
        var expr = NumberParser.Parse("7 / 2");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(3.5));
    }

    [Test]
    public void ClassicRiddle1()
    {
        // 6 / 2 * (1 + 2) = 9
        var expr = NumberParser.Parse("6 / 2 * (1 + 2)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(9));
    }

    [Test]
    public void ClassicRiddle2()
    {
        // 1 + 2 - 3 * 4 / 5 + 6 = 6.6
        var expr = NumberParser.Parse("1 + 2 - 3 * 4 / 5 + 6");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(6.6));
    }

    [Test]
    public void LongExpression()
    {
        var expr = NumberParser.Parse("1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(55));
    }

    [Test]
    public void LargeNumbers()
    {
        var expr = NumberParser.Parse("1000000 * 1000000");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(1_000_000_000_000));
    }

    [Test]
    public void SpacesAndFormatting()
    {
        var expr = NumberParser.Parse("  (  3+4 ) *   2 ");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(14));
    }
    
    [Test]
    public void InvalidCharacterThrows()
    {
        Assert.That(() =>
        {
            var expr = NumberParser.Parse("2 + @");
            expr.Evaluate();
        }, Throws.TypeOf<FormatException>());
    }

    [Test]
    public void EmptyExpressionThrows()
    {
        Assert.That(() =>
        {
            var expr = NumberParser.Parse("");
            expr.Evaluate();
        }, Throws.TypeOf<FormatException>());
    }

    [Test]
    public void UnbalancedParenthesesThrows()
    {
        Assert.That(() =>
        {
            var expr = NumberParser.Parse("(1 + (2 * 3)");
            expr.Evaluate();
        }, Throws.TypeOf<FormatException>());
    }

    [Test]
    public void MissingOperandThrows()
    {
        Assert.That(() =>
        {
            var expr = NumberParser.Parse("1 +");
            expr.Evaluate();
        }, Throws.TypeOf<FormatException>());
    }

    [Test]
    public void MisplacedParenthesesThrows()
    {
        Assert.That(() =>
        {
            var expr = NumberParser.Parse("1 + 2)");
            expr.Evaluate();
        }, Throws.TypeOf<FormatException>());
    }

    [Test]
    public void InvalidDecimalFormatThrows()
    {
        Assert.That(() =>
        {
            var expr = NumberParser.Parse("5..2 + 1");
            expr.Evaluate();
        }, Throws.TypeOf<FormatException>());
    }

    [Test]
    public void CompletelyInvalidExpressionThrows()
    {
        Assert.That(() =>
        {
            var expr = NumberParser.Parse("bananas * 42");
            expr.Evaluate();
        }, Throws.TypeOf<FormatException>());
    }

    [Test]
    public void RandomGarbageInputThrows()
    {
        Assert.That(() =>
        {
            var expr = NumberParser.Parse("👻💥🐍");
            expr.Evaluate();
        }, Throws.TypeOf<FormatException>());
    }
    
    [Test]
    public void ImplicitMultiplication_NumberParenthesis()
    {
        var expr = NumberParser.Parse("2(4)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(8));
    }

    [Test]
    public void ImplicitMultiplication_ComplexExpression()
    {
        // (8+4)*2-2(4) = 12*2-2*4 = 24-8 = 16
        var expr = NumberParser.Parse("(8+4)*2-2(4)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(16));
    }

    [Test]
    public void ImplicitMultiplication_ParenthesisParenthesis()
    {
        // (2+3)(4+5) = 5*9 = 45
        var expr = NumberParser.Parse("(2+3)(4+5)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(45));
    }

    [Test]
    public void ImplicitMultiplication_Multiple()
    {
        // 2(3)(4) = 2*3*4 = 24
        var expr = NumberParser.Parse("2(3)(4)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(24));
    }

    [Test]
    public void ImplicitMultiplication_WithDecimal()
    {
        // 2.5(3) = 2.5*3 = 7.5
        var expr = NumberParser.Parse("2.5(3)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(7.5));
    }

    [Test]
    public void ImplicitMultiplication_WithUnary()
    {
        // -2(4) = -2*4 = -8
        var expr = NumberParser.Parse("-2(4)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(-8));
    }

    [Test]
    public void ImplicitMultiplication_WithSpaces()
    {
        // 2 (4) should also work
        var expr = NumberParser.Parse("2 (4)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(8));
    }

    [Test]
    public void ImplicitMultiplication_NestedComplex()
    {
        // 3(2+4(1+1)) = 3(2+4*2) = 3(2+8) = 3*10 = 30
        var expr = NumberParser.Parse("3(2+4(1+1))");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(30));
    }

    // ===== Power (^) Operation Tests =====
    
    [Test]
    public void Power_BasicOperation()
    {
        var expr = NumberParser.Parse("2^3");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(8));
    }

    [Test]
    public void Power_WithSpaces()
    {
        var expr = NumberParser.Parse("2 ^ 3");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(8));
    }

    [Test]
    public void Power_LargerNumbers()
    {
        var expr = NumberParser.Parse("5^2");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(25));
    }

    [Test]
    public void Power_ZeroExponent()
    {
        var expr = NumberParser.Parse("10^0");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void Power_LargePower()
    {
        var expr = NumberParser.Parse("2^10");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(1024));
    }

    [Test]
    public void Power_RightAssociative_ThreeLevels()
    {
        // 2^3^2 should be evaluated as 2^(3^2) = 2^9 = 512, not (2^3)^2 = 8^2 = 64
        var expr = NumberParser.Parse("2^3^2");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(512));
    }

    [Test]
    public void Power_RightAssociative_AnotherExample()
    {
        // 2^2^3 should be evaluated as 2^(2^3) = 2^8 = 256
        var expr = NumberParser.Parse("2^2^3");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(256));
    }

    [Test]
    public void Power_PrecedenceOverAddition()
    {
        // 2 + 3^2 should be 2 + 9 = 11, not (2+3)^2 = 25
        var expr = NumberParser.Parse("2 + 3^2");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(11));
    }

    [Test]
    public void Power_PrecedenceOverMultiplication()
    {
        // 2 * 3^2 should be 2 * 9 = 18, not (2*3)^2 = 36
        var expr = NumberParser.Parse("2 * 3^2");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(18));
    }

    [Test]
    public void Power_PrecedenceOverDivision()
    {
        // 12 / 2^2 should be 12 / 4 = 3, not (12/2)^2 = 36
        var expr = NumberParser.Parse("12 / 2^2");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void Power_WithParentheses()
    {
        // (2 + 3)^2 = 5^2 = 25
        var expr = NumberParser.Parse("(2 + 3)^2");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(25));
    }

    [Test]
    public void Power_MultiplePowersInExpression()
    {
        // 2^2 * 3^2 = 4 * 9 = 36
        var expr = NumberParser.Parse("2^2 * 3^2");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(36));
    }

    [Test]
    public void Power_WithAddition()
    {
        // 2^3 + 4 = 8 + 4 = 12
        var expr = NumberParser.Parse("2^3 + 4");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(12));
    }

    [Test]
    public void Power_NegativeBase()
    {
        // (-2)^3 = -8
        var expr = NumberParser.Parse("(-2)^3");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(-8));
    }

    [Test]
    public void Power_FractionalExponent()
    {
        // 4^0.5 = 2 (square root)
        var expr = NumberParser.Parse("4^0.5");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(2));
    }

    // ===== Square Root (sqrt) Operation Tests =====

    [Test]
    public void Sqrt_BasicOperation()
    {
        var expr = NumberParser.Parse("sqrt(4)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(2));
    }

    [Test]
    public void Sqrt_LargerNumber()
    {
        var expr = NumberParser.Parse("sqrt(16)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(4));
    }

    [Test]
    public void Sqrt_PerfectSquare()
    {
        var expr = NumberParser.Parse("sqrt(25)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(5));
    }

    [Test]
    public void Sqrt_LargePerfectSquare()
    {
        var expr = NumberParser.Parse("sqrt(100)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(10));
    }

    [Test]
    public void Sqrt_WithExpression()
    {
        // sqrt(9+16) = sqrt(25) = 5
        var expr = NumberParser.Parse("sqrt(9+16)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(5));
    }

    [Test]
    public void Sqrt_WithMultiplication()
    {
        // sqrt(4*9) = sqrt(36) = 6
        var expr = NumberParser.Parse("sqrt(4*9)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(6));
    }

    [Test]
    public void Sqrt_WithPower()
    {
        // sqrt(2^4) = sqrt(16) = 4
        var expr = NumberParser.Parse("sqrt(2^4)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(4));
    }

    [Test]
    public void Sqrt_ImplicitMultiplication_NumberSqrt()
    {
        // 2sqrt(4) = 2 * 2 = 4
        var expr = NumberParser.Parse("2sqrt(4)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(4));
    }

    [Test]
    public void Sqrt_ImplicitMultiplication_AnotherExample()
    {
        // 3sqrt(9) = 3 * 3 = 9
        var expr = NumberParser.Parse("3sqrt(9)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(9));
    }

    [Test]
    public void Sqrt_ImplicitMultiplication_SqrtSqrt()
    {
        // sqrt(4)sqrt(9) = 2 * 3 = 6
        var expr = NumberParser.Parse("sqrt(4)sqrt(9)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(6));
    }

    [Test]
    public void Sqrt_WithAddition()
    {
        // sqrt(16) + 2 = 4 + 2 = 6
        var expr = NumberParser.Parse("sqrt(16) + 2");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(6));
    }

    [Test]
    public void Sqrt_AdditionThenSqrt()
    {
        // 2 + sqrt(9) = 2 + 3 = 5
        var expr = NumberParser.Parse("2 + sqrt(9)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(5));
    }

    [Test]
    public void Sqrt_Multiplication()
    {
        // sqrt(4) * sqrt(9) = 2 * 3 = 6
        var expr = NumberParser.Parse("sqrt(4) * sqrt(9)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(6));
    }

    [Test]
    public void Sqrt_ComplexExpression()
    {
        // sqrt((3+1)*4) = sqrt(16) = 4
        var expr = NumberParser.Parse("sqrt((3+1)*4)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(4));
    }

    [Test]
    public void Sqrt_NestedWithPower()
    {
        // sqrt(3^2 + 4^2) = sqrt(9+16) = sqrt(25) = 5
        var expr = NumberParser.Parse("sqrt(3^2 + 4^2)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(5));
    }

    [Test]
    public void Sqrt_NegativeNumberThrows()
    {
        // sqrt(-4) should throw an exception
        Assert.That(() =>
        {
            var expr = NumberParser.Parse("sqrt(-4)");
            expr.Evaluate();
        }, Throws.TypeOf<InvalidOperationException<UnaryExpression>>());
    }

    [Test]
    public void Sqrt_WithSpaces()
    {
        var expr = NumberParser.Parse("sqrt (9)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void Sqrt_NoParenthesesWithNumber()
    {
        // sqrt 9 should also work (sqrt applied to a factor)
        var expr = NumberParser.Parse("sqrt 9");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(3));
    }

    // ===== Combined Power and Sqrt Tests =====

    [Test]
    public void Combined_PowerAndSqrt()
    {
        // 2^2 + sqrt(16) = 4 + 4 = 8
        var expr = NumberParser.Parse("2^2 + sqrt(16)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(8));
    }

    [Test]
    public void Combined_SqrtOfPower()
    {
        // sqrt(3^2) = sqrt(9) = 3
        var expr = NumberParser.Parse("sqrt(3^2)");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void Combined_PowerOfSqrt()
    {
        // sqrt(4)^2 = 2^2 = 4
        var expr = NumberParser.Parse("sqrt(4)^2");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(4));
    }

    [Test]
    public void Combined_ComplexWithBothOperations()
    {
        // 2sqrt(4) + 3^2 = 2*2 + 9 = 4 + 9 = 13
        var expr = NumberParser.Parse("2sqrt(4) + 3^2");
        var result = expr.Evaluate();
        Assert.That(result, Is.EqualTo(13));
    }
}
