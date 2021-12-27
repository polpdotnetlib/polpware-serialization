using System.Xml.Linq;
using NUnit.Framework;
using System.Linq;
using System.IO;

namespace Polpware.IO.Serialization.DBSettings
{
    public class DataSettingsTest
    {
        [Test]
        public void TestFileLoading()
        {
            var path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory));

            var file = System.IO.Path.Combine(path, "../data/Settings.txt");

            var service = new DataSettingsManager();
            var settings = service.LoadSettings(file);

            Assert.IsNotNull(settings);
            Assert.AreEqual(settings.DataConnectionString, "Data Source=192.168.1.x;Initial Catalog=dbname;Integrated Security=False;Persist Security Info=False;User ID=hello;Password=dd;MultipleActiveResultSets=True");
            Assert.AreEqual(settings.DataProvider, "sqlserver");
        }
    }
}
