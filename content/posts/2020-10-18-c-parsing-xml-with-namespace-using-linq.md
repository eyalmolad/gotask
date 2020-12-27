---
title: 'C# Parsing XML with namespace using LINQ'
author: eyal
type: post
date: 2020-10-18T12:58:33+00:00
url: /programming/c-parsing-xml-with-namespace-using-linq/
categories:
  - Programming
tags:
  - .netcore
  - anonymous-type
  - linq
  - linq-to-xml
  - parsing
  - serialization
  - xml
  - xml-namespace

---
## Background

In one of my previous posts, I wrote about <a href="https://gotask.net/programming/deserializing-xml-with-namespace-in-c-sharp/" target="_blank" rel="noopener noreferrer">deserializing XML with namespace</a> using XmlSerializer that requires creating custom model classes in

order to perform the serialization. Today, I am going to cover another powerful method for parsing &#8211; <a href="https://docs.microsoft.com/en-us/dotnet/standard/linq/linq-xml-overview" target="_blank" rel="noopener noreferrer">LINQ to XML</a>.

## My Stack<figure id="attachment_269" aria-describedby="caption-attachment-269" style="width: 277px" class="wp-caption alignright">

<img loading="lazy" class="size-full wp-image-269" src="https://gotask.net/wp-content/uploads/2020/10/xml_logo.png" alt="Xml element tag" width="287" height="65" /> <figcaption id="caption-attachment-269" class="wp-caption-text">Xml element tag</figcaption></figure> <!-- /wp:paragraph -->

<!-- wp:list -->

  * Visual Studio 2019 Community.
  * .NET Core 3.1 / C#
  * Windows 10 Pro 64-bit (10.0, Build 19041)

## Implementation

### Simple LINQ-XML reading

Consider the following XML that contains no namespaces.

<pre class="EnlighterJSRAW" data-enlighter-language="xml">&lt;?xml version="1.0" encoding="utf-8" ?&gt;
&lt;Root&gt;
  &lt;Items&gt;
    &lt;Books&gt;
      &lt;Book&gt;
        &lt;ISBN&gt;978-1788478120&lt;/ISBN&gt;
        &lt;Name&gt;C# 8.0 and .NET Core 3.0 – Modern Cross-Platform Development: Build applications with C#, .NET Core, Entity Framework Core, ASP.NET Core, and ML.NET using Visual Studio Code, 4th Edition&lt;/Name&gt;
        &lt;Price&gt;35.99&lt;/Price&gt;
      &lt;/Book&gt;
      &lt;Book&gt;
        &lt;ISBN&gt;978-1789133646&lt;/ISBN&gt;
        &lt;Name&gt;Hands-On Design Patterns with C# and .NET Core: Write clean and maintainable code by using reusable solutions to common software design problems&lt;/Name&gt;
        &lt;Price&gt;34.99&lt;/Price&gt;
      &lt;/Book&gt;
    &lt;/Books&gt;
  &lt;/Items&gt;
&lt;/Root&gt;</pre>

In order to read the Books elements, we could use the following sample code:

<pre class="EnlighterJSRAW" data-enlighter-language="csharp">var root = XElement.Load("SimpleBooks.xml");

var books = root.Descendants("Items").Descendants("Books").Descendants("Book");

foreach (var book in books)
  Console.WriteLine($"Book name: {book.Element("Name").Value}");</pre>

Another approach could be taking an advantage of the <a href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/anonymous-types" target="_blank" rel="noopener noreferrer">anonymous types</a> in .NET. The sample code below reads all the books into an anonymous type containing the 3 elements from the XML as read only properties.

<pre class="EnlighterJSRAW" data-enlighter-language="csharp">// 2. Convert to anonymous type.
var books = from book in root.Descendants("Items").Descendants("Books").Descendants("Book")
      select new
      {
        Name = book.Element("Name").Value,
        ISBN = book.Element("ISBN").Value,
        Price = book.Element("Price").Value
      };

foreach (var book in books)
  Console.WriteLine($"Book name: {book.Name}");</pre>

### Simple LINQ-XML reading with namespaces

A quick reminder from the previous article &#8211; why do we need <a href="https://gotask.net/programming/deserializing-xml-with-namespace-in-c-sharp/" target="_blank" rel="noopener noreferrer">namespaces in our XML files</a>?

The short answer would be to prevent any element&#8217;s naming conflicts in the same file. Remember, XML files can be very long and complex written by different people, so naming conflicts might be very common. A comparable example could be names of the classes in the C# code &#8211; once inside namespace the chance for conflict is very low. To create the uniqueness, we usually use URI&#8217;s that we own, but actually the namespace name can be any string. There are more details in this question regarding <a href="https://softwareengineering.stackexchange.com/questions/122002/why-do-we-need-uris-for-xml-namespaces" target="_blank" rel="noopener noreferrer">URI&#8217;s and namespaces</a>.

For the sample, I am going to add a namespace to the Books element of the XML.

<pre class="EnlighterJSRAW" data-enlighter-language="null">xmlns="https://gotask.net"</pre>

So our XML looks like:

<pre class="EnlighterJSRAW" data-enlighter-language="xml">&lt;?xml version="1.0" encoding="utf-8" ?&gt;
&lt;Root&gt;
  &lt;Items xmlns="https://gotask.net"&gt;
    &lt;Books&gt;
      &lt;Book&gt;        
        &lt;ISBN&gt;978-1788478120&lt;/ISBN&gt;
        &lt;Name&gt;C# 8.0 and .NET Core 3.0 – Modern Cross-Platform Development: Build applications with C#, .NET Core, Entity Framework Core, ASP.NET Core, and ML.NET using Visual Studio Code, 4th Edition&lt;/Name&gt;
        &lt;Price&gt;35.99&lt;/Price&gt;
      &lt;/Book&gt;
      &lt;Book&gt;
        &lt;ISBN&gt;978-1789133646&lt;/ISBN&gt;
        &lt;Name&gt;Hands-On Design Patterns with C# and .NET Core: Write clean and maintainable code by using reusable solutions to common software design problems&lt;/Name&gt;
        &lt;Price&gt;34.99&lt;/Price&gt;
      &lt;/Book&gt;
    &lt;/Books&gt;
  &lt;/Items&gt;
&lt;/Root&gt;</pre>

Running the previous code on this code will produce no results. The reason is that each element has it&#8217;s own fully qualified name once we have a namespace &#8211; the element Books is actually https://gotask.net:Books. and our code is searching for <code class="EnlighterJSRAW" data-enlighter-language="csharp">items.Descendants("Books")</code>.

In order to correctly parse the file above, we need to specify the namespace using <a href="https://docs.microsoft.com/en-us/dotnet/api/system.xml.linq.xnamespace?view=netcore-3.1" target="_blank" rel="noopener noreferrer">XNamespace</a> class in every call for Descendants.

<pre class="EnlighterJSRAW" data-enlighter-language="csharp">XNamespace x = "https://gotask.net";

var books = root.Descendants(x + "Items").Descendants(x + "Books").Descendants(x + "Book");

foreach (var book in books)
   Console.WriteLine($"Book name: {book.Element(x + "Name").Value}");</pre>

### Nested namespaces

Consider the following XML, where the Items element is in one namespace, but the Books child element is in other:

<pre class="EnlighterJSRAW" data-enlighter-language="xml">&lt;?xml version="1.0" encoding="utf-8" ?&gt;
&lt;Root&gt;
  &lt;Items xmlns="https://gotask.net"&gt;
    &lt;Books xmlns="https://books.net"&gt;
      &lt;Book&gt;
        &lt;ISBN&gt;978-1788478120&lt;/ISBN&gt;
        &lt;Name&gt;C# 8.0 and .NET Core 3.0 – Modern Cross-Platform Development: Build applications with C#, .NET Core, Entity Framework Core, ASP.NET Core, and ML.NET using Visual Studio Code, 4th Edition&lt;/Name&gt;
        &lt;Price&gt;35.99&lt;/Price&gt;
      &lt;/Book&gt;
      &lt;Book&gt;
        &lt;ISBN&gt;978-1789133646&lt;/ISBN&gt;
        &lt;Name&gt;Hands-On Design Patterns with C# and .NET Core: Write clean and maintainable code by using reusable solutions to common software design problems&lt;/Name&gt;
        &lt;Price&gt;34.99&lt;/Price&gt;
      &lt;/Book&gt;
    &lt;/Books&gt;
  &lt;/Items&gt;
&lt;/Root&gt;</pre>

In the sample code below, we need to specify both namespaces.

<pre class="EnlighterJSRAW" data-enlighter-language="csharp">XNamespace x = "https://gotask.net";

XNamespace y = "https://books.net";

var books = root.Descendants(x + "Items").Descendants(y + "Books").Descendants(y + "Book");

foreach (var book in books)
    Console.WriteLine($"Book is {book.Element(y + "Name").Value}");</pre>

### Multiple namespaces with prefix

<a href="https://www.xml.com/pub/a/2005/04/13/namespace-uris.html" target="_blank" rel="noopener noreferrer">XML standard</a> allows us to define multiple namespaces for the same element. Once we define <code class="EnlighterJSRAW" data-enlighter-language="null">xmlns=https://somename.net</code>, we are actually defining a default namespace without a prefix. In order to define another namespace, we need to specify the prefix <code class="EnlighterJSRAW" data-enlighter-language="null">xmlns:bk=https://books.net</code>.

In order to create child elements that belongs to https://books.net namespace, we need to declare with <code class="EnlighterJSRAW" data-enlighter-language="xml">&lt;bk:book&gt;&lt;/bk:book&gt;</code>. Elements without the prefix will belong to the default namespace.

So lets consider this is our new XML. We have 2 namespaces defined, https://gotask.net is the default one and https://books.net has the bk prefix.

We have one Book element in the bk namespace and the other one in the default.

<pre class="EnlighterJSRAW" data-enlighter-language="xml">&lt;?xml version="1.0" encoding="utf-8" ?&gt;
&lt;Root&gt;
  &lt;Items xmlns="https://gotask.net" xmlns:bk="https://books.net"&gt;
    &lt;Books&gt;
      &lt;bk:Book&gt;
        &lt;bk:ISBN&gt;978-1788478120&lt;/bk:ISBN&gt;
        &lt;bk:Name&gt;C# 8.0 and .NET Core 3.0 – Modern Cross-Platform Development: Build applications with C#, .NET Core, Entity Framework Core, ASP.NET Core, and ML.NET using Visual Studio Code, 4th Edition&lt;/bk:Name&gt;
        &lt;bk:Price&gt;35.99&lt;/bk:Price&gt;
      &lt;/bk:Book&gt;
      &lt;Book&gt;
        &lt;ISBN&gt;978-1789133646&lt;/ISBN&gt;
        &lt;Name&gt;Hands-On Design Patterns with C# and .NET Core: Write clean and maintainable code by using reusable solutions to common software design problems&lt;/Name&gt;
        &lt;Price&gt;34.99&lt;/Price&gt;
      &lt;/Book&gt;
    &lt;/Books&gt;
  &lt;/Items&gt;
&lt;/Root&gt;</pre>

The code below, reads only the Books belonging to the bk namespace.

<pre class="EnlighterJSRAW" data-enlighter-language="csharp">XNamespace x = "https://gotask.net";

XNamespace b = "https://books.net";

var books = root.Descendants(x + "Items").Descendants(x + "Books").Descendants(b + "Book");

foreach (var book in books)
  Console.WriteLine($"Book name: {book.Element(b + "Name").Value}");</pre>

The code below, reads only the Books belonging to the default namespace.

<pre class="EnlighterJSRAW" data-enlighter-language="csharp">XNamespace x = "https://gotask.net";

var books = root.Descendants(x + "Items").Descendants(x + "Books").Descendants(x + "Book");

foreach (var book in books)
  Console.WriteLine($"Book name: {book.Element(x + "Name").Value}");</pre>

## Useful links

  * The full source code available at <a href="https://github.com/eyalmolad/gotask/tree/master/XML/LinqToXML" target="_blank" rel="noopener noreferrer">GitHub</a>.