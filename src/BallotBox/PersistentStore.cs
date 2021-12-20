using RocksDbSharp;

namespace EligereVS.BallotBox
{
    public class PersistentStore
    {
        private string _path;
        private string _ballotboxName;
        private string[] _storeNames;
        private Dictionary<string, RocksDb> _stores;

        public PersistentStore(string path, string ballotboxName, params string[] storeNames)
        {
            _path = path;
            _ballotboxName = ballotboxName;
            _storeNames = storeNames;
            _stores = new Dictionary<string, RocksDb>();
        }

        private RocksDb InitDb(string path)
        {
            var opt = new DbOptions().SetCreateIfMissing(true);
            return RocksDb.Open(opt, path);
        }

        public void Open()
        {
            var basepath = Path.Combine(_path, _ballotboxName);
            if (!Directory.Exists(basepath))
                Directory.CreateDirectory(basepath);

            foreach (var storeName in _storeNames)
            {
                _stores[storeName] = InitDb(Path.Combine(basepath, storeName));
            }
        }

        public void Open(string storeName)
        {
            var basepath = Path.Combine(_path, _ballotboxName);
            if (!_stores.ContainsKey(storeName) || _stores[storeName] == null)
            {
                _stores[storeName] = InitDb(Path.Combine(basepath, storeName));
            }
        }

        public void Close(string storeName)
        {
            if (_stores.ContainsKey(storeName) && _stores[storeName] != null)
            {
                _stores[storeName].Dispose();
                _stores.Remove(storeName);
            }
        }

        public void Close()
        {
            foreach (var store in _stores.Values)
            {
                store.Dispose();
            }
            _stores.Clear();
        }

        public bool IsOpen { get { return _stores.Count == _storeNames.Length; } }

        public bool IsStoreOpen(string storeName)
        { 
            return _stores.ContainsKey(storeName);
        }

        public RocksDb this[string storeName] { get { return _stores[storeName]; } }
    }
}
