using System.Numerics;

namespace NumerativeTranslation
{
    public struct BigFraction
    {
        public BigInteger Numerator { get; set; }

        public BigInteger Denominator { get; set; }

        public BigFraction(BigInteger numerator, BigInteger denominator) : this()
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public BigFraction GetAbs()
        {
            return new BigFraction(BigInteger.Abs(Numerator), Denominator);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BigFraction)) return false;

            BigFraction bf1 = this;
            BigFraction bf2 = (BigFraction)obj;

            ToSameDenominator(ref bf1, ref bf2);

            return bf1.Numerator == bf2.Numerator;
        }

        public static BigFraction operator -(BigFraction bf1, BigFraction bf2)
        {
            ToSameDenominator(ref bf1, ref bf2);

            return new BigFraction(bf1.Numerator - bf2.Numerator, bf1.Denominator);
        }

        public static BigFraction operator +(BigFraction bf1, BigFraction bf2)
        {
            ToSameDenominator(ref bf1, ref bf2);

            return new BigFraction(bf1.Numerator + bf2.Numerator, bf1.Denominator);
        }

        public static BigFraction operator *(BigFraction bf1, BigFraction bf2)
        {
            return new BigFraction(bf1.Numerator * bf2.Numerator, bf1.Denominator * bf2.Denominator);
        }

        public static BigFraction operator /(BigFraction bf1, BigFraction bf2)
        {
            return new BigFraction(bf1.Numerator * bf2.Denominator, bf1.Denominator * bf2.Numerator);
        }

        public static bool operator >(BigFraction bf1, BigFraction bf2)
        {
            ToSameDenominator(ref bf1, ref bf2);

            return bf1.Numerator > bf2.Numerator;
        }

        public static bool operator <(BigFraction bf1, BigFraction bf2)
        {
            ToSameDenominator(ref bf1, ref bf2);

            return bf1.Numerator < bf2.Numerator;
        }

        public static bool operator >=(BigFraction bf1, BigFraction bf2)
        {
            ToSameDenominator(ref bf1, ref bf2);

            return bf1.Numerator >= bf2.Numerator;
        }

        public static bool operator <=(BigFraction bf1, BigFraction bf2)
        {
            ToSameDenominator(ref bf1, ref bf2);

            return bf1.Numerator <= bf2.Numerator;
        }

        public static bool operator ==(BigFraction bf1, BigFraction bf2)
        {
            ToSameDenominator(ref bf1, ref bf2);

            return bf1.Numerator == bf2.Numerator;
        }

        public static bool operator !=(BigFraction bf1, BigFraction bf2)
        {
            ToSameDenominator(ref bf1, ref bf2);

            return bf1.Numerator != bf2.Numerator;
        }

        private static void ToSameDenominator(ref BigFraction bf1, ref BigFraction bf2)
        {
            if (bf1.Denominator == bf2.Denominator) return;

            BigInteger denominator1 = bf1.Denominator;
            BigInteger denominator2 = bf2.Denominator;
            BigInteger denominator = bf1.Denominator * bf2.Denominator;

            bf1 = new BigFraction(bf1.Numerator * denominator2, denominator);
            bf2 = new BigFraction(bf2.Numerator * denominator1, denominator);
        }

        public static implicit operator BigFraction(BigExpoFraction bef)
        {
            return new BigFraction(bef.Numerator, bef.Denominator);
        }
    }
}
