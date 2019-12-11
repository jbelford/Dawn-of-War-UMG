namespace DowUmg.Interfaces
{
    internal interface IFileLoader<T>
    {
        public T Load(string filePath);
    }
}
