using System.Xml.Linq;

namespace Polpware.IO.Serialization.XML
{
    public interface IReaderWriter
    {
        XDocument Load(string key, string xmlFile);
        XDocument LoadSafely(string key, string xmlFile);
        void Save(XDocument that, string key, string xmlFile);
        void SaveSafetly(XDocument that, string key, string xmlFile);
    }
}
