namespace DowUmg.Interfaces
{
    public interface IFileLoader<T>
    {
        public T Load(string filePath);
    }
}
