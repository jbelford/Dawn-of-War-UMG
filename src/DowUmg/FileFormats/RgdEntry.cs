namespace DowUmg.FileFormats
{
    internal interface IRgdEntry
    {
        public uint Hash { get; }
    }

    internal class RgdEntry<T> : IRgdEntry
    {
        public RgdEntry(uint hash, T value)
        {
            Hash = hash;
            Value = value;
        }

        public T Value { get; }

        public uint Hash { get; }
    }
}
