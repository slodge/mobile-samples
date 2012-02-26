using System;
using System.Threading;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Services;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Newtonsoft.Json;

namespace MWC.Core.Mvvm.JsonFileStore
{
    public class JsonFileStore<T>
    {
        private readonly IMvxSimpleFileStoreService _fileStoreService;
        private readonly string _filePath;

        public JsonFileStore(string filePath, IMvxSimpleFileStoreService fileStoreService)
        {
            _filePath = filePath;
            _fileStoreService = fileStoreService;
        }
        
        public T LoadOrDefault()
        {
            T model;
            TryLoad(out model);
            return model;
        }

        public bool TryLoad(out T model)
        {
            model = default(T);
            try
            {
                string json;
                if (!_fileStoreService.TryReadTextFile(_filePath, out json))
				{
         			MvxTrace.Trace("Loading repository failed {0}", _filePath);
                    return false;
				}
				
                model = JsonConvert.DeserializeObject<T>(json);
     			MvxTrace.Trace("Loading repository succeeded {0}", _filePath);
                return true;
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                // TODO - need to do *something* here!
     			MvxTrace.Trace("Loading repository failed {0}, error {1}", _filePath, exception.ToLongString());
                return false;
            }
        }

        public bool Store(T model)
        {
            try
            {
				var json = JsonConvert.SerializeObject(model);
                _fileStoreService.WriteFile(_filePath, json);
     			MvxTrace.Trace("Saving repository succeeded {0}", _filePath);
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
     			MvxTrace.Trace("Saving repository failed {0}, error {1}", _filePath, exception.ToLongString());
                return false;
            }
            return true;
        }
    }
}