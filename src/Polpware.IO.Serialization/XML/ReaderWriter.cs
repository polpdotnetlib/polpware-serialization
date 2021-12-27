using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;
using System.Xml.Linq;

namespace Polpware.IO.Serialization.XML
{
    public class ReaderWriter : IReaderWriter
    {
        private readonly IThreadSafeMemoryCache _cache;

        public ReaderWriter(IThreadSafeMemoryCache cache)
        {
            _cache = cache;
        }

        public XDocument Load(string key, string xmlFile)
        {
            XDocument res;
            if (!_cache.MemCache.TryGetValue(key, out res))
            {
                res = new XDocument();

                try
                {
                    using (Stream stream = new FileStream(xmlFile, FileMode.Open))
                    {
                        // File/Stream manipulating code here
                        res = XDocument.Load(stream);
                    }

                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3));
                    _cache.MemCache.Set(key, res, cacheEntryOptions);
                }
                catch (Exception)
                {
                    // TODO:
                    //check here why it failed and ask user to retry if the file is in use.
                }
            }
            return res;
        }

        public XDocument LoadSafely(string key, string xmlFile)
        {
            XDocument res = null;

            lock(_cache.Mutex)
            {
                res = Load(key, xmlFile);
            }
            return res;
        }

        public void Save(XDocument that, string key, string xmlFile)
        {
            // Invalidate cache
            _cache.MemCache.Remove(key);
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3));
            _cache.MemCache.Set(xmlFile, that, cacheEntryOptions);

            try
            {
                using (var ms = new MemoryStream())
                {
                    that.Save(ms);

                    byte[] result = ms.ToArray();
                    File.WriteAllBytes(xmlFile, result);
                }
            }
            catch (Exception)
            {
                // TODO:
            }
        }

        public void SaveSafetly(XDocument that, string key, string xmlFile)
        {
            lock (_cache.Mutex)
            {
                Save(that, key, xmlFile);
            }
        }

    }
}
