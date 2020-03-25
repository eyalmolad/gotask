using System.Collections.Generic;
using System.Xml.Serialization;

namespace XMLDeserializeCore
{
    public class Book
    {
        public string ISBN { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }

    [XmlRoot("Books", Namespace = "https://gotask.net")]
    public class Books : List<Book>
    {
    }
}
