using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;

namespace Polpware.IO.Serialization
{
    /// <summary>
    /// Thread-safe memory cache.
    /// This class exposes two members for its clients to consume.
    /// </summary>
    public class ThreadSafeMemoryCache : IThreadSafeMemoryCache
    {
        private readonly MemoryCache _memoryCache;
        private readonly Object _mutex;

        // Single instance
        public static ThreadSafeMemoryCache SingleInstance = new ThreadSafeMemoryCache();

        public MemoryCache MemCache {
            get
            {
                return _memoryCache;
            }
        }

        public Object Mutex
        {
            get
            {
                return _mutex;
            }
        }

        public ThreadSafeMemoryCache()
        {
            _mutex = new object();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public static bool Write(Stream inputStream, string outputFilePath)
        {
            //check if file already exists ????
            // TODO: Why a file may exist ???
            if (File.Exists(outputFilePath))
            {
                return false;
            }
            byte[] result;
            using (var memoryStream = new MemoryStream())
            {
                inputStream.CopyTo(memoryStream);
                result = memoryStream.ToArray();
                File.WriteAllBytes(outputFilePath, result);
            }

            return true;
        }

        public static bool IsFileLocked(string fullpath)
        {
            FileStream stream = null;

            if (!File.Exists(fullpath))
            {
                return false;
            }
            try
            {
                FileInfo file = new FileInfo(fullpath);
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

    }
}
