using System;
using System.Collections.Generic;
using System.Linq;

namespace SRP.ControlDigit
{
    public static class Extensions
    {
        public static IEnumerable<int> ToDigits(this long number)
        {
            while (number > 0)
            {
                yield return (int)(number % 10);
                number /= 10;
            }
        }

        public static int SumDigits(this long number, Func<int, int, int> operation)
        {
            return number.ToDigits().Select((digit, index)
                => operation(digit, index)).Sum();
        }

        public static int CalculateIsbn10CheckDigit(this long number)
        {
            return number.ToDigits().Select((digit, index)
                => digit * (index + 2)).Sum();
        }

        public static int ModAndSubtract(this int number, int modulus, int subtractFrom)
        {
            int result = number % modulus;
            return result == 0 ? 0 : subtractFrom - result;
        }

        public static int DoubleForLuhn(this int digit)
        {
            int doubled = digit * 2;
            return doubled > 9 ? doubled - 9 : doubled;
        }
    }

    public static class ControlDigitAlgo
    {
        public static int Upc(long number)
        {
            int sum = Extensions.SumDigits(number, (digit, index) =>
            {
                int factor = index % 2 == 0 ? 3 : 1;
                return factor * digit;
            });

            return sum.ModAndSubtract(10, 10);
        }

        public static char Isbn10(long number)
        {
            var result = (11 - (number.CalculateIsbn10CheckDigit() % 11)) % 11;
            return result == 10 ? 'X' : result.ToString()[0];
        }

        public static int Luhn(long number)
        {
            int sum = number.SumDigits((digit, index) =>

              index % 2 == 0 ? digit.DoubleForLuhn() : digit);

            return sum.ModAndSubtract(10, 10);
        }
    }
}