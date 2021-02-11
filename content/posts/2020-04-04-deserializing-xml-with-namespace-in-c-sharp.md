---
title: 'C# Deserializing XML with namespace in .NET core'
author: eyal
type: post
date: 2020-04-04T15:38:13+00:00
url: /programming/deserializing-xml-with-namespace-in-c-sharp/
categories:
  - Programming
tags:
  - .netcore
  - 'c#'
  - serialization
  - xml
  - xml-namespace

---

## Introduction

I've been seeing a lot of repeated questions of difficulties deserializing XML files that have a namespace.

In this post, I will provide some samples in C# of how to deserialize such XML files with minimal amount of code.

My goal for this article is to provide working examples with minimal amount of code and class attributes.

The full source code available at [GitHub](https://github.com/eyalmolad/gotask/tree/master/XML/XMLDeserializeCore).

## My Stack

* Visual Studio 2019
* .NET Core 3.1
* Notepad++ text editor for XML

## Background

Why do we need namespaces?

There is a lot of similarity between a classes in C# project and XML document. In C#, every class needs to have it's own namespace providing the ability to define classes with a same names across different projects. In case of class, every class is recognizable by a fully qualified name.

For example, ```XmlSerializer``` class that I will use in this sample is actually ```System.Xml.Serialization.XmlSerializer```.

I could easily define a class named ```XmlSerializer``` in my namespace without any conflicts.

When you work alone on a project, class naming conflict might be a rare thing, but when using external library without namespaces, the conflict could be a common thing.

A similar thing happens in XML files. Once you define a namespace, you can create elements with a same name, but different namespace.

A namespace name in XML is usually a URI of organization, but actually it can be any string. Here is a useful article describing the historical reasons for URI usage in [XML namespaces](https://www.xml.com/pub/a/2005/04/13/namespace-uris.html).

XML standard attribute xmlns enables us to define a multiple namespaces for element.

  * ```xmlns="https://somename.org"``` creates a default namespace.
  * In cased we would like to add more namespaces, we need to use a prefix.
    * ```xmlns:gt="https://somename2.org"``` creates a namespace with prefix ```gt```

## Preparing the project

* In Visual Studio, create a new .NET Core Console App.
* Add a ```SimpleBooks.xml``` as shown below. I used a default namespace ```xmlns="https://gotask.net" ``` attribute for Books element.

```XML
<?xml version="1.0" encoding="utf-8" ?>
<Books xmlns="https://gotask.net">
  <Book>
    <ISBN>978-1788478120</ISBN>
    <Name>C# 8.0 and .NET Core 3.0 – Modern Cross-Platform Development: Build applications with C#, .NET Core, Entity Framework Core, ASP.NET Core, and ML.NET using Visual Studio Code, 4th Edition</Name>
    <Price>35.99</Price>
  </Book>
  <Book>
    <ISBN>978-1789133646</ISBN>
    <Name>Hands-On Design Patterns with C# and .NET Core: Write clean and maintainable code by using reusable solutions to common software design problems</Name>
    <Price>34.99</Price>
  </Book>
</Books>
```

* Add 2 classes ```Book``` and ```Books```.
* The only class attribute I will use is ```XmlRoot``` and set the name of the root element and the namespace.

```C#
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
```

## Deserializing

### Simple Case

Once we have the classes and the XML data set properly, the deserialize function is really simple.

In the list we have 2 books now.

```C#
var serializer = new XmlSerializer(typeof(Books));

using (var reader = new FileStream("SimpleBooks.xml", FileMode.Open))
{
  var books = (Books)serializer.Deserialize(reader);
  Console.WriteLine($"Number of books is {books.Count}");
}
```

### Multiple namespaces

I added another namespace to the declaration of the Books element. This namespace has ```gt``` prefix and value ```"https://github.org"```.  Running the code above on this XML will produce the same results (2 books) since there is no ```Book``` element in ```gt``` namespace.

```XML
<?xml version="1.0" encoding="utf-8" ?>
<Books xmlns="https://gotask.net" xmlns:gt="https://github.org">
  <Book>
    <ISBN>978-1788478120</ISBN>
    <Name>C# 8.0 and .NET Core 3.0 – Modern Cross-Platform Development: Build applications with C#, .NET Core, Entity Framework Core, ASP.NET Core, and ML.NET using Visual Studio Code, 4th Edition</Name>
    <Price>35.99</Price>
  </Book>
  <Book>
    <ISBN>978-1789133646</ISBN>
    <Name>Hands-On Design Patterns with C# and .NET Core: Write clean and maintainable code by using reusable solutions to common software design problems</Name>
    <Price>34.99</Price>
  </Book>
</Books>
```

Now lets change the XML and add one of the book elements to gt namespace. The XML would look like:

```XML
<?xml version="1.0" encoding="utf-8" ?>
<Books xmlns="https://gotask.net" xmlns:gt="https://github.org">
  <Book>
    <ISBN>978-1788478120</ISBN>
    <Name>C# 8.0 and .NET Core 3.0 – Modern Cross-Platform Development: Build applications with C#, .NET Core, Entity Framework Core, ASP.NET Core, and ML.NET using Visual Studio Code, 4th Edition</Name>
    <Price>35.99</Price>
  </Book>
  <gt:Book>
    <ISBN>978-1789133646</ISBN>
    <Name>Hands-On Design Patterns with C# and .NET Core: Write clean and maintainable code by using reusable solutions to common software design problems</Name>
    <Price>34.99</Price>
  </gt:Book>
</Books>
```

Now the code above will deserialize only the first book (ISBN 978-1788478120), since it's the only book that exists in the default namespace.

## Useful links

* The full source code available at [GitHub](https://github.com/eyalmolad/gotask/tree/master/XML/XMLDeserializeCore)
* [XML Linq namespaces](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/namespaces-overview-linq-to-xml) overview