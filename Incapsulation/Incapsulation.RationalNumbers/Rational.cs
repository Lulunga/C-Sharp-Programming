using System;

namespace Incapsulation.RationalNumbers
{
    public class Rational
    {
        private readonly int numerator;
        private readonly int denominator;

        public Rational(int numerator, int denominator = 1)
        {
            if (denominator == 0)
            {
                IsNan = true;
                return;
            }

            int gcd = CalculateGCD(Math.Abs(numerator), Math.Abs(denominator));
            this.numerator = numerator / gcd;
            this.denominator = denominator / gcd;

            if (denominator < 0)
            {
                this.numerator = -this.numerator;
                this.denominator = -this.denominator;
            }
        }

        public int Numerator => numerator;

        public int Denominator => denominator;

        public bool IsNan { get; }

        public static implicit operator double(Rational r)
        {
            return r.IsNan ? double.NaN : (double)r.numerator / r.denominator;
        }

        public static implicit operator Rational(int value) => new Rational(value);

        public static explicit operator int(Rational r)
        {
            if (r.denominator != 1)
                throw new ArgumentException("Rational number is not convertible to int.");
            return r.numerator;
        }

        public static Rational operator +(Rational a, Rational b)
        {
            return new Rational(a.numerator * b.denominator + b.numerator *
                a.denominator, a.denominator * b.denominator);
        }

        public static Rational operator -(Rational a, Rational b)
        {
            return new Rational(a.numerator * b.denominator - b.numerator *
                a.denominator, a.denominator * b.denominator);
        }

        public static Rational operator *(Rational a, Rational b)
        {
            return new Rational(a.numerator * b.numerator, a.denominator * b.denominator);
        }

        public static Rational operator /(Rational a, Rational b)
        {
            if (b.numerator == 0)
                return new Rational(0, 0);

            return new Rational(a.numerator * b.denominator, a.denominator * b.numerator);
        }

        public static bool operator ==(Rational a, Rational b) =>
            a.numerator == b.numerator && a.denominator == b.denominator;

        public static bool operator !=(Rational a, Rational b) => !(a == b);

        public override bool Equals(object obj)
        {
            return obj is Rational other && this == other;
        }

        public override int GetHashCode() => Tuple.Create(numerator, denominator).GetHashCode();

        public override string ToString()
        {
            return IsNan ? "NaN" : denominator == 1 ? numerator.ToString() : $"{numerator}/{denominator}";
        }

        private int CalculateGCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}