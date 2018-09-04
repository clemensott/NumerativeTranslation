using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace NumerativeTranslation
{
    public struct BigExpoFraction
    {
        private const int minDenominatorBase = 2, minDenominatorExponent = 0;

        private static readonly char[] numerals = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C',
            'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        private int denominatorBase;
        private int denominatorExponent;

        public BigInteger Numerator { get; set; }

        public int DenominatorBase
        {
            get
            {
                if (denominatorBase < minDenominatorBase) denominatorBase = minDenominatorBase;

                return denominatorBase;
            }

            set
            {
                if (denominatorBase < minDenominatorBase) denominatorBase = minDenominatorBase;
                else denominatorBase = value;
            }
        }
        public int DenominatorExponent
        {
            get
            {
                if (denominatorExponent < minDenominatorExponent) denominatorExponent = minDenominatorExponent;

                return denominatorExponent;
            }

            set
            {
                if (denominatorExponent < minDenominatorExponent) denominatorExponent = minDenominatorExponent;
                else denominatorExponent = value;
            }
        }

        public BigInteger Denominator { get { return BigInteger.Pow(DenominatorBase, DenominatorExponent); } }

        public BigExpoFraction(BigInteger numerator, int denominatorBase, int denominatorExpo)
        {
            Numerator = numerator;

            this.denominatorBase = denominatorBase;
            this.denominatorExponent = denominatorExpo;
        }

        public BigExpoFraction(string numeralString, int systemBase)
        {
            int decimalPlaceIndex;
            List<int> inputNumerals;
            GetInputNumerals(numeralString, systemBase, out decimalPlaceIndex, out inputNumerals);

            Numerator = GetNumerator(inputNumerals, systemBase);
            denominatorBase = systemBase;
            denominatorExponent = decimalPlaceIndex;
        }

        private static void GetInputNumerals(string numeralString, int systemBase,
            out int decimalPlaceIndex, out List<int> inputNumerals)
        {
            decimalPlaceIndex = -1;
            inputNumerals = new List<int>();

            foreach (char c in numeralString)
            {
                int current;

                if (TryGetValueOfChar(c, out current))
                {
                    if (current >= systemBase) continue;

                    inputNumerals.Add(current);

                    if (decimalPlaceIndex != -1) decimalPlaceIndex++;
                }
                else if (c == ',' && decimalPlaceIndex == -1) decimalPlaceIndex = 0;
            }

            if (decimalPlaceIndex == -1) decimalPlaceIndex = 0;
        }

        private static bool TryGetValueOfChar(char c, out int value)
        {
            c = char.ToUpper(c);

            for (value = 0; value < numerals.Length; value++)
            {
                if (char.ToUpper(numerals[value]) == c) return true;
            }

            value = -1;
            return false;
        }

        private static BigInteger GetNumerator(List<int> inputNumerals, int systemBase)
        {
            BigInteger biAddFactor = BigInteger.One;
            BigInteger biValue = BigInteger.Zero;

            inputNumerals.Reverse();

            foreach (int inputDecimal in inputNumerals)
            {
                biValue += biAddFactor * inputDecimal;
                biAddFactor = biAddFactor * systemBase;
            }

            return biValue;
        }

        public BigExpoFraction ToDenominator(int denominatorBase, int denominatorExpo)
        {
            BigInteger toDenominator = BigInteger.Pow(denominatorBase, denominatorExpo);
            BigInteger numerator = (Numerator * toDenominator) / Denominator;

            return new BigExpoFraction(numerator, denominatorBase, denominatorExpo);
        }

        public BigExpoFraction ToDenominatorExponent(int exponent)
        {
            return ToDenominator(DenominatorBase, exponent);
        }

        public BigExpoFraction GetAbs()
        {
            return new BigExpoFraction(BigInteger.Abs(Numerator), DenominatorBase, DenominatorExponent);
        }

        public BigExpoFraction Simplify()
        {
            BigExpoFraction bef = this;

            while (bef.DenominatorExponent > 0 && bef.Numerator % bef.DenominatorBase == BigInteger.Zero)
            {
                bef.DenominatorExponent--;
                bef.Numerator /= bef.DenominatorBase;
            }

            return bef;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BigFraction)) return false;

            BigFraction bf1 = this;
            BigFraction bf2 = (BigFraction)obj;

            return bf1.Numerator == bf2.Numerator;
        }

        public override string ToString()
        {
            BigExpoFraction fraction = this;
            int carry = 0;
            string text = string.Empty;

            for (int i = 0; fraction.Numerator > BigInteger.Zero || i <= fraction.DenominatorExponent; i++)
            {
                if (i == fraction.DenominatorExponent && fraction.DenominatorExponent > 0) text = "," + text;

                int currentNumeral = (int)BigInteger.Remainder(fraction.Numerator, fraction.DenominatorBase) + carry;

                carry = currentNumeral / fraction.DenominatorBase;
                fraction.Numerator /= fraction.DenominatorBase;

                text = numerals[currentNumeral % fraction.DenominatorBase] + text;
            }

            return FinishString(text);
        }

        private static string FinishString(string text)
        {
            text = text.TrimStart('0');

            if (text.Contains(',')) text = text.TrimEnd('0');
            if (text.Length > 0 && text[0] == ',') text = "0" + text;
            if (text == "") text = "0";

            text = text.TrimEnd(',');

            return text;
        }

        private static string AddSpaces(string text, int every)
        {
            int commaIndex = text.IndexOf(',');

            if (commaIndex == -1) commaIndex = text.Length;

            for (int i = commaIndex + 1; i < text.Length; i += every)
            {
                text = text.Insert(commaIndex, " ");
            }

            for (int i = commaIndex - 1 - every; i >= 0; i -= every)
            {
                text = text.Insert(commaIndex, " ");
            }

            return text;
        }

        public static BigFraction operator -(BigExpoFraction bef1, BigExpoFraction bef2)
        {
            return (BigFraction)bef1 - bef2;
        }

        public static BigFraction operator +(BigExpoFraction bef1, BigExpoFraction bef2)
        {
            return (BigFraction)bef1 + bef2;
        }

        public static BigFraction operator *(BigExpoFraction bef1, BigExpoFraction bef2)
        {
            return (BigFraction)bef1 * bef2;
        }

        public static BigFraction operator /(BigExpoFraction bef1, BigExpoFraction bef2)
        {
            return (BigFraction)bef1 / bef2;
        }

        public static bool operator >(BigExpoFraction bef1, BigExpoFraction bef2)
        {
            return (BigFraction)bef1 > bef2;
        }

        public static bool operator <(BigExpoFraction bef1, BigExpoFraction bef2)
        {
            return (BigFraction)bef1 < bef2;
        }

        public static bool operator >=(BigExpoFraction bef1, BigExpoFraction bef2)
        {
            return (BigFraction)bef1 >= bef2;
        }

        public static bool operator <=(BigExpoFraction bef1, BigExpoFraction bef2)
        {
            return (BigFraction)bef1 <= bef2;
        }

        public static bool operator ==(BigExpoFraction bef1, BigExpoFraction bef2)
        {
            return (BigFraction)bef1 == bef2;
        }

        public static bool operator !=(BigExpoFraction bef1, BigExpoFraction bef2)
        {
            return (BigFraction)bef1 != bef2;
        }

        public static implicit operator BigExpoFraction(double value)
        {
            BigInteger numerator = new BigInteger(Math.Round(value));
            int expo = 0;

            while (value % 1 != 0)
            {
                value = (value * 10) % 10;
                numerator *= 10;
                numerator += new BigInteger(Math.Round(value));
                expo++;
            }

            return new BigExpoFraction(numerator, 10, expo);
        }
    }
}
