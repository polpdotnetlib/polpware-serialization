using System;
using System.ComponentModel;
using System.Xml.Linq;

namespace Polpware.IO.Serialization.XML
{
    public class ProviderBase
    {
        protected readonly IReaderWriter _readerWriter;
        protected readonly string _cacheKey;
        protected readonly string _xmlFile;

        public ProviderBase(IReaderWriter readerWriter, string cacheKey, string xmlFile)
        {
            _readerWriter = readerWriter;
            _cacheKey = cacheKey;
            _xmlFile = xmlFile;
        }

        protected bool InsertObject(string xmlFile, Object entity)
        {
            var doc = _readerWriter.LoadSafely(_cacheKey, _xmlFile);
            doc.Root.Add(entity);
            _readerWriter.SaveSafetly(doc, _cacheKey, _xmlFile);
            return true;
        }

        protected T GetValue<T>(XContainer node, XName name)
        {
            var x = node.Element(name);
            if (x == null)
            {
                return default(T);
            }
            else
            {
                T ret;
                try {
                    var foo = TypeDescriptor.GetConverter(typeof(T));
                    ret = (T)(foo.ConvertFromInvariantString(x.Value));
                }
                catch(Exception)
                {
                    ret = default(T);
                }
                return ret;
            }
        }
    }
}
