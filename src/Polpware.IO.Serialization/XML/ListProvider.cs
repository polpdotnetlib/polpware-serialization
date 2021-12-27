using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Polpware.IO.Serialization.XML
{
    public abstract class ListProvider<U> : ProviderBase
    {
        protected readonly string _commentTag;
        protected readonly string _rootTag;
        protected readonly string _elementTag;

        public ListProvider(IReaderWriter readerWriter, string cacheKey, string xmlFile,
            string commentTag, string rootTag, string elementTag) :
            base(readerWriter, cacheKey, xmlFile)
        {
            _commentTag = commentTag;
            _rootTag = rootTag;
            _elementTag = elementTag;
        }

        protected abstract U ConvertElement(XElement element);
        protected abstract bool CompareElement(XElement left, U right);
        protected abstract XElement MakeXElement(U data);
        protected abstract void UpdateElement(U source, XElement target);

        public IEnumerable<U> Load()
        {

            //xmlFile is the full path of the xml file
            if (!File.Exists(_xmlFile))
            {
                return new List<U>();
            }

            try
            {
                var doc = _readerWriter.LoadSafely(_cacheKey, _xmlFile);
                var l = doc.Descendants(_elementTag).Select(p =>
                {
                    return ConvertElement(p);
                });
                // TODO: Check if we need to do so or not
                // Can we wait till we need them
                // Force it to load
                return l.ToList();
            }
            catch (Exception ex)
            {
            }

            return new List<U>();
        }

        public bool DeleteElement(U data)
        {
            var ret = true;
            //xmlFile is the full path of the xml file
            if (!File.Exists(_xmlFile))
            {
                return true;
            }

            try
            {
                var doc = _readerWriter.LoadSafely(_cacheKey, _xmlFile);
                    doc.Descendants(_elementTag).Where(p => CompareElement(p, data)).Remove();
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }


        public bool UpdateElemet(U data)
        {
            var ret = true;

            if (!File.Exists(_xmlFile))
            {
                return InsertElement(data);
            }

            try
            {
                var xml = _readerWriter.LoadSafely(_cacheKey, _xmlFile);
                var objProperties = xml.Descendants(_elementTag).Where(p => CompareElement(p, data));
                if (objProperties.Count() == 0)
                {
                    var element = MakeXElement(data);
                    InsertObject(_xmlFile, element);
                }
                else
                {
                    foreach (var property in objProperties)
                    {
                        UpdateElement(data, property);
                    }
                    _readerWriter.SaveSafetly(xml, _cacheKey, _xmlFile);
                }
            }
            catch(Exception ex)
            {
                ret = false;
            }

            return ret;
        }

        public bool InsertElement(U data)
        {
            bool ret = true;

            try
            {
                if (!File.Exists(_xmlFile))
                {
                    var document = new XDocument(
                     new XComment(_commentTag),
                     new XElement(_rootTag));

                    _readerWriter.SaveSafetly(document, _cacheKey, _xmlFile);
                }

                var node = MakeXElement(data);

                ret = InsertObject(_xmlFile, node);
            }
            catch (Exception e)
            {
                ret = false;
            }

            return ret;
        }

    }
}
