using System;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace LinqToXML
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Simple
            DoSimpleXML();

            DoOneNamespaceXML();

            DoNestedNamespace();

            DoMultipleNamespace();
        }

        private static void DoSimpleXML()
        {
            Console.WriteLine($"DoSimpleXML");

            var root = XElement.Load("SimpleBooks.xml");
            foreach (var book in root.Descendants("Items").Descendants("Books").Descendants("Book"))
                Console.WriteLine($"Book name: {book.Element("Name").Value}");

            // 2. Convert to anonymous type.
            var books = from book in root.Descendants("Items").Descendants("Books").Descendants("Book")
                        select new
                        {
                            Name = book.Element("Name").Value,
                            ISBN = book.Element("ISBN").Value,
                            Price = book.Element("Price").Value
                        };

            foreach (var book in books)
                Console.WriteLine($"Book name: {book.Name}");
        }

        private static void DoOneNamespaceXML()
        {
            Console.WriteLine($"DoOneNamespaceXML");

            var root = XElement.Load("SimpleBooksOneNamespace.xml");
            XNamespace x = "https://gotask.net";
            foreach (var book in root.Descendants(x + "Items").Descendants(x + "Books").Descendants(x + "Book"))
                Console.WriteLine($"Book name: {book.Element(x + "Name").Value}");
        }

        private static void DoNestedNamespace()
        {
            Console.WriteLine($"DoNestedNamespace");

            var root = XElement.Load("SimpleBooksNestedNamespace.xml");
            XNamespace x = "https://gotask.net";
            XNamespace y = "https://books.net";
            foreach (var book in root.Descendants(x + "Items").Descendants(x + "Books").Descendants(y + "Book"))
                Console.WriteLine($"Book name: {book.Element(y + "Name").Value}");
        }
        private static void DoMultipleNamespace()
        {
            Console.WriteLine($"DoMultipleNamespace");

            var root = XElement.Load("SimpleBooksMultipleNamespace.xml");
            XNamespace x = "https://gotask.net";
            XNamespace b = "https://books.net";
            foreach (var book in root.Descendants(x + "Items").Descendants(x + "Books").Descendants(b + "Book"))
                Console.WriteLine($"Book name: {book.Element(b + "Name").Value}");
            
            foreach (var book in root.Descendants(x + "Items").Descendants(x + "Books").Descendants(x + "Book"))
                Console.WriteLine($"Book name: {book.Element(x + "Name").Value}");
        }
    }
}
