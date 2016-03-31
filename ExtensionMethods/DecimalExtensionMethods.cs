namespace ExtensionMethods
{
    public static class DecimalExtensionMethods
    {
        public static decimal Negate(this decimal value)
        {
            return -value;
        }

        public static decimal AsRatioOfTotal(this decimal value, decimal total) => 
            (total != 0) ? value / total :0;
              
    }
}
