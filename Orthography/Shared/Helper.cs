namespace Orthography.Shared
{
	public static class Helper
	{
        public static int ArrayToBitmask(bool[] array)
        {
            var bitmask = 0;
            for (var i = array.Length - 1; i >= 0; --i)
            {
                bitmask |= array[i] ? 1 : 0;
                if (i > 0) bitmask <<= 1;
            }
            return bitmask;
        }

        public static bool[] BitmaskToArray(int bitmask, int arraySize)
        {
            var res = new bool[arraySize];
            for (var i = 0; i < arraySize; ++i)
            {
                res[i] = bitmask % 2 > 0;
                bitmask >>= 1;
            }
            return res;
        }
    }
}
