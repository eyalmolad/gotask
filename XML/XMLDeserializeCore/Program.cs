using System;
using System.IO;
using System.Xml.Serialization;

namespace XMLDeserializeCore
{
    class Program
    {
        static void Main(string[] args)
        {
            DeserializeSimple();
        }

        static void DeserializeSimple()
        {
            var serializer = new XmlSerializer(typeof(Books));

            // use SimpleBooks.xml or
            // SimpleBooksMultiNs.xml
            using (var reader = new FileStream("SimpleBooksMultiNs.xml", FileMode.Open))
            {
                var books = (Books)serializer.Deserialize(reader);
                Console.WriteLine($"Number of books is {books.Count}");
            }
        }
    }
}
