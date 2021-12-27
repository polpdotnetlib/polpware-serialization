using System.Xml.Linq;
using NUnit.Framework;
using System.Linq;

namespace Polpware.IO.Serialization.XML
{
    public class ListProviderTest
    {
        public class Data
        {
            public string Name { get; set; }
        }

        public class ListDataProvider : ListProvider<Data>
        {
            public ListDataProvider() : base(new ReaderWriter(ThreadSafeMemoryCache.SingleInstance),
                "listData", "list-data.xml", "Comment", "Users", "User")
            { }

            protected override bool CompareElement(XElement left, Data right)
            {
                throw new System.NotImplementedException();
            }

            protected override Data ConvertElement(XElement element)
            {
                var name = GetValue<string>(element, "Name");

                return new Data
                {
                    Name = name
                };
            }

            protected override XElement MakeXElement(Data data)
            {
                return new XElement(_elementTag, new XElement("Name", data.Name));
            }

            protected override void UpdateElement(Data source, XElement target)
            {
                throw new System.NotImplementedException();
            }
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InsertData()
        {
            var data = new Data
            {
                Name = "Tom"
            };

            var provider = new ListDataProvider();
            var r1 = provider.InsertElement(data);

            Assert.IsTrue(r1);

            var savedData = provider.Load();

            Assert.IsTrue(savedData.Count() > 0);

            Assert.Pass();
        }
    }
}