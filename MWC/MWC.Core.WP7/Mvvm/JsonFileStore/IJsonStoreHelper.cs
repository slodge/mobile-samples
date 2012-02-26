namespace MWC.Core.Mvvm.JsonFileStore
{
    public interface IJsonStoreHelper
    {
        T LoadFromStore<T>(string filePath);
        void SaveToStore<T>(T toSave, string filePath) where T : class;
    }
}