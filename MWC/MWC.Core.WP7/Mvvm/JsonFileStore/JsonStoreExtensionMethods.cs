using Cirrious.MvvmCross.ExtensionMethods;

namespace MWC.Core.Mvvm.JsonFileStore
{
    public static class JsonStoreExtensionMethods
    {
        public static T LoadFromRepository<T>(this IJsonStoreHelperConsumer consumer, string filePath)
        {
            var helper = consumer.GetService<IJsonStoreHelper>();
            return helper.LoadFromStore<T>(filePath);
        }

        public static void SaveToRepository<T>(this IJsonStoreHelperConsumer consumer, T toSave, string filePath)
                        where T : class
        {
            var helper = consumer.GetService<IJsonStoreHelper>();
            helper.SaveToStore<T>(toSave, filePath);
        }
    }
}