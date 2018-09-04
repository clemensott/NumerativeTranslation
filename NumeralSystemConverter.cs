using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NumerativeTranslation
{
    static class NumeralSystemConverter
    {
        private const int defaultBase = 10, defaultRelRoundValue = 20, defaultPlaceRoundValue = -2;

        public static BigExpoFraction Translate(BigExpoFraction value, int toBase, Round round)
        {
            if (!round.IsRelative) return value.ToDenominator(toBase, Math.Max(round.Absolute, 0));

            BigFraction maxRelDelta = new BigExpoFraction(1, 10, round.Relative) * value;
            BigExpoFraction output;
            int expo = -1;

            do
            {
                expo++;
                output = value.ToDenominator(toBase, expo);
            }
            while ((output - value).GetAbs() > maxRelDelta);

            return output.Simplify();
        }

        private static readonly char[] numerals = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C',
            'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public static string Berechnen(string value, int fromBase,
            int toBase, bool doRelRound, int relRoundValue, int placeRoundValue)
        {
            if (value != "") return string.Empty;

            string aus = "";
            bool komma = false, noch = true;
            int na_kom = 0, ver = -1, uber = 0;
            List<int> von_zahl = new List<int>();
            BigInteger grose = BigInteger.Zero, neuer, fac_v, fac_z, genau = BigInteger.Zero, unterschied;


            if (doRelRound) genau = BigInteger.Pow(new BigInteger(10), relRoundValue);

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == ',') komma = true;
                else
                {
                    for (int j = 0; j < fromBase; j++)
                    {
                        if (char.ToUpper(value[i]) != char.ToUpper(numerals[j])) continue;

                        von_zahl.Add(j);
                        na_kom += Convert.ToInt32(komma);
                        break;
                    }
                }
            }

            fac_v = BigInteger.Pow(new BigInteger(fromBase), na_kom);

            for (int i = 1; i <= von_zahl.Count; i++)
            {
                grose += BigInteger.Pow(new BigInteger(fromBase), von_zahl.Count - i) * von_zahl[i - 1];
            }

            while (true)
            {
                ver++;

                fac_z = BigInteger.Pow(new BigInteger(toBase), ver);
                neuer = (fac_z * grose) / fac_v;

                if (doRelRound)
                {
                    BigInteger cache = fac_z * grose - neuer * fac_v;

                    if (1 != BigInteger.Compare(cache, BigInteger.Zero)) break;

                    unterschied = (neuer * fac_v) / cache;
                    noch = 1 == BigInteger.Compare(genau, unterschied);
                }
                else if (ver > -placeRoundValue) break;
            }

            grose = neuer;

            for (int i = 0; 0 < BigInteger.Compare(grose, BigInteger.Zero) || i <= ver; i++)
            {
                if (i == ver && ver > 0)
                    aus = "," + aus;

                int akt = (int)BigInteger.Remainder(grose, new BigInteger(toBase)) + uber;
                uber = akt / toBase;
                akt = akt % toBase;

                aus = numerals[akt] + aus;
                grose = BigInteger.Divide(grose, new BigInteger(toBase));

                if (!doRelRound && i < placeRoundValue + ver)
                {
                    aus = "";
                    uber = Convert.ToInt32(toBase <= akt * 2);
                }
            }

            if (placeRoundValue + ver >= 1 && !doRelRound)
            {
                for (int i = 0; i < placeRoundValue + ver; i++)
                    aus += "0";
            }

            if (placeRoundValue <= 0 && !doRelRound) aus = aus.Remove(aus.Length - 1);
            else if (doRelRound && ver > 0) aus = aus.TrimEnd('0');

            aus = aus.TrimEnd(',');
            aus = aus.TrimStart('0');

            if (aus.Length > 1 && aus.Remove(1) == ",") aus = "0" + aus;
            if (aus == "") aus = "0";

            return aus;
        }
    }
}
