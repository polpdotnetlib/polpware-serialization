using Microsoft.Extensions.Caching.Memory;
using System;

namespace Polpware.IO.Serialization
{
    public interface IThreadSafeMemoryCache
    {
        MemoryCache MemCache { get; }
        Object Mutex { get; }
    }
}
