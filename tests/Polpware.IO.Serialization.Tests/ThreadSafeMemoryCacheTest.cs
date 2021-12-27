using NUnit.Framework;

namespace Polpware.IO.Serialization
{
    public class ThreadSafeMemoryCacheTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CanCreate()
        {
            var m = new ThreadSafeMemoryCache();
            Assert.IsNotNull(m.Mutex);
            Assert.IsNotNull(m.MemCache);
            Assert.Pass();
        }
    }
}