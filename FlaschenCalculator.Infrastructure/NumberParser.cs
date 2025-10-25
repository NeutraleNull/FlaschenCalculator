using System.Globalization;
using FlaschenCalculator.Core;
using FlaschenCalculator.Core.Models;

namespace FlaschenCalculator.Infrastructure;

public static class NumberParser
{
    public static IExpression Parse(ReadOnlySpan<char> input)
    {
        var parser = new Parser(input);
        var expression = parser.ParseExpression();
        parser.SkipWhitespace();
        return !parser.End ? throw new FormatException($"Unexpected input at position {parser.Position}: '{parser.Remaining.ToString()}'") : expression;
    }
    
    private ref struct Parser(ReadOnlySpan<char> src)
    {
        private readonly ReadOnlySpan<char> _src = src;
        private int _pos = 0;

        public int Position => _pos;
        public bool End => _pos >= _src.Length;
        public ReadOnlySpan<char> Remaining => End ? ReadOnlySpan<char>.Empty : _src[_pos..];

        public IExpression ParseExpression() => ParseAdditionAndSubtraction();

        // expression -> term ( ( "+" | "-" ) term )*
        private IExpression ParseAdditionAndSubtraction()
        {
            var left = ParseMultiplicationAndDivision();
            while (true)
            {
                SkipWhitespace();

                if (Match('+'))
                {
                    var right = ParseMultiplicationAndDivision();
                    left = new BinaryExpression(Operation.Addition, left, right);
                }
                else if (Match('-'))
                {
                    var right = ParseMultiplicationAndDivision();
                    left = new BinaryExpression(Operation.Subtraction, left, right);
                }
                else break;
            }
            return left;
        }

        // term -> exponentiation ( ( "*" | "/" | implicit ) exponentiation )*
        private IExpression ParseMultiplicationAndDivision()
        {
            var left = ParseExponentiation();
            while (true)
            {
                SkipWhitespace();

                if (Match('*'))
                {
                    var right = ParseExponentiation();
                    left = new BinaryExpression(Operation.Multiplication, left, right);
                }
                else if (Match('/'))
                {
                    var right = ParseExponentiation();
                    left = new BinaryExpression(Operation.Division, left, right);
                }
                else if (!End && (_src[_pos] == '(' || IsDigit(_src[_pos]) || IsLetter(_src[_pos])))  // implicit multiplication 3(2+2)=12, 2sqrt(4)=4
                {
                    var right = ParseExponentiation();
                    left = new BinaryExpression(Operation.Multiplication, left, right);
                }
                else break;
            }
            return left;
        }

        // exponentiation -> factor ( "^" exponentiation )?  (right-associative)
        private IExpression ParseExponentiation()
        {
            var left = ParseFactor();
            SkipWhitespace();
            
            if (Match('^'))
            {
                // Right-associative: 2^3^2 = 2^(3^2)
                var right = ParseExponentiation();
                return new BinaryExpression(Operation.Power, left, right);
            }
            
            return left;
        }

        // factor -> NUMBER | "(" expression ")" | ( "+" | "-" ) factor | "sqrt" factor
        private IExpression ParseFactor()
        {
            SkipWhitespace();

            // sqrt keyword
            if (MatchKeyword("sqrt"))
            {
                var right = ParseFactor();
                return new UnaryExpression(Operation.SquareRoot, right);
            }

            // unary +/-
            if (Match('+'))
            {
                var right = ParseFactor();
                return new UnaryExpression(Operation.Addition, right);
            }
            if (Match('-'))
            {
                var right = ParseFactor();
                return new UnaryExpression(Operation.Subtraction, right);
            }

            // parenthesized
            if (Match('('))
            {
                var expr = ParseExpression();
                SkipWhitespace();
                if (!Match(')'))
                    throw new FormatException($"Missing closing parenthesis at position {Position}.");
                return expr;
            }

            // number
            var numberSpan = ReadNumberSpan();
            return !decimal.TryParse(
                numberSpan,
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out var value) ? throw new FormatException($"Invalid number: '{numberSpan.ToString()}'") : new NumberExpression(value);
        }

        // Reads a number without a sign (unary +/- handled in ParseFactor)
        // Accepts forms like "42", "3.14", ".5", "0.", but not just "."
        private ReadOnlySpan<char> ReadNumberSpan()
        {
            SkipWhitespace();
            int start = _pos;

            bool hasInt = false;
            while (!End && IsDigit(_src[_pos]))
            {
                _pos++;
                hasInt = true;
            }

            bool hasFrac = false;
            if (!End && _src[_pos] == '.')
            {
                _pos++; // consume '.'
                int fracStart = _pos;
                while (!End && IsDigit(_src[_pos])) _pos++;
                hasFrac = _pos > fracStart;
            }

            if (!(hasInt || hasFrac))
                throw new FormatException($"Expected number at position {start}.");

            return _src[start.._pos];
        }

        public void SkipWhitespace()
        {
            while (!End && char.IsWhiteSpace(_src[_pos])) _pos++;
        }

        private bool Match(char c)
        {
            if (End || _src[_pos] != c) return false;
            _pos++;
            return true;
        }

        private bool MatchKeyword(string keyword)
        {
            int keywordLen = keyword.Length;
            if (_pos + keywordLen > _src.Length)
                return false;

            // check if the keyword matches
            for (int i = 0; i < keywordLen; i++)
            {
                if (_src[_pos + i] != keyword[i])
                    return false;
            }

            // ensure the keyword is not part of a longer identifier
            // (e.g. "sqrt2" should NOT match "sqrt")
            if (_pos + keywordLen < _src.Length && IsLetter(_src[_pos + keywordLen]))
                return false;

            _pos += keywordLen;
            return true;
        }

        private static bool IsLetter(char c) => c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';
    }

    private static bool IsDigit(char c) => c is >= '0' and <= '9';
}
