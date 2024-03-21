namespace DowUmg.FileFormats
{
    internal interface IFileLoader<T>
    {
        public T Load(string filePath);
    }
}
