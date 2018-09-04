namespace NumerativeTranslation
{
    public struct Round
    {
        public bool IsRelative { get; set; }

        public int Relative { get; set; }

        public int Absolute { get; set; }

        public Round(bool doRelative, int relative, int absolute) : this()
        {
            IsRelative = doRelative;
            Relative = relative;
            Absolute = absolute;
        }
    }
}
