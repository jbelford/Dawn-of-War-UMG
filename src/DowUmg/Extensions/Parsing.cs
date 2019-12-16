namespace DowUmg.Extensions
{
    public static class Parsing
    {
        public static string GetAsciiString(in byte[] buffer, int offset)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    return new string((sbyte*)(p + offset));
                }
            }
        }

        public static string GetUnicodeString(in byte[] buffer, int offset)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    return new string((char*)(p + offset));
                }
            }
        }
    }
}
