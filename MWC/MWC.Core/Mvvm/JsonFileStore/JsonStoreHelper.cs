using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Platform;

namespace MWC.Core.Mvvm.JsonFileStore
{
    public class JsonStoreHelper
        : IMvxServiceConsumer<IMvxSimpleFileStoreService>
        , IJsonStoreHelper
    {
        private JsonFileStore<T> CreateRepository<T>(string filePath)
        {
            var fileStoreService = this.GetService<IMvxSimpleFileStoreService>();
            var repository = new JsonFileStore<T>(filePath, fileStoreService);
            return repository;
        }

        public T LoadFromStore<T>(string filePath)
        {
            var repository = CreateRepository<T>(filePath);
            return repository.LoadOrDefault();
        }

        public void SaveToStore<T>(T toSave, string filePath)
            where T : class
        {
            var repository = CreateRepository<T>(filePath);
            repository.Store(toSave);
        }        
    }
}