---
title: 'Reverse list elements in C#'
author: eyal
type: post
date: 2020-04-06T22:15:04+00:00
url: /programming/reverse-list-elements-in-c-sharp/
categories:
  - Programming
tags:
  - .netcore
  - 'c#'
  - generics
  - linq
  - list

---
## Background

.NET core provides a generics class List to store a strongly types objects that can be accessed by index.

This class provides us with many methods to add, remove, access, sort or manipulate the objects within the list.

In this sample, I am going to demonstrate the following <code class="EnlighterJSRAW" data-enlighter-language="csharp">Reverse</code> options:

  * Reverse using the System.Collections.Generic List&#8217;s methods.
  * Reverse using Linq method

## Code Samples

### List Initialization

I am going to create a list of integers and set the values using a collection initializer.

<pre class="EnlighterJSRAW" data-enlighter-language="null">var list = new List&lt;int&gt;
{
    1, 5, 6, 7, 9, 10, 99, 777
};</pre>

Note that using a collection initializer as shown above produces the same code as separately using the Add function multiple times:

<pre class="EnlighterJSRAW" data-enlighter-language="null">var list = new List&lt;int&gt;();
list.Add(1);
list.Add(5);
list.Add(6);
list.Add(7);
list.Add(9);
list.Add(10);
list.Add(99);
list.Add(777);</pre>

Printing to console the original list, produces the following output:

<figure id="attachment_187" aria-describedby="caption-attachment-187" style="width: 206px" class="wp-caption alignnone"><img loading="lazy" class="size-full wp-image-187" src="https://gotask.net/wp-content/uploads/2020/04/original-list-int.png" alt="In-place reverse output" width="216" height="228" /><figcaption id="caption-attachment-187" class="wp-caption-text">Original list of items sample output</figcaption></figure>

&nbsp;

### Reverse using List<T> Reverse Methods

The name of the method is self-explanatory &#8211; it reverses the order of the elements in the list.

<span style="text-decoration: underline;">Important note:</span> The Reverse methods are reversing the list in-place, meaning your original List object is being changed.

The Reverse  method has 2 overloads:

  * Reverse(void) &#8211; Reverses the all the elements in the given list
  * Reverse(int, int) &#8211; Reverses the order of the elements in the specified range

#### Full reverse in-place

<code class="EnlighterJSRAW" data-enlighter-language="csharp">list.Reverse();</code>

<figure id="attachment_188" aria-describedby="caption-attachment-188" style="width: 217px" class="wp-caption alignnone"><img loading="lazy" class="size-full wp-image-188" src="https://gotask.net/wp-content/uploads/2020/04/reversed-full-list-int.png" alt="Reverset in place list c#" width="227" height="253" /><figcaption id="caption-attachment-188" class="wp-caption-text">Reverse full list in C#</figcaption></figure>

#### Partial reverse in-place

<code class="EnlighterJSRAW" data-enlighter-language="csharp">list.Reverse(0, 3)</code>

<figure id="attachment_189" aria-describedby="caption-attachment-189" style="width: 284px" class="wp-caption alignnone"><img loading="lazy" class="size-full wp-image-189" src="https://gotask.net/wp-content/uploads/2020/04/reversed-first-3-items-int.png" alt="3 Items reversed c#" width="294" height="223" /><figcaption id="caption-attachment-189" class="wp-caption-text">Reverse first 3 items in C# List</figcaption></figure>

### Reverse using Linq Reverse Method

In case your wish to keep the original list unchanged, the following Linq code will create another list with reversed items:

<code class="EnlighterJSRAW" data-enlighter-language="csharp">list.AsEnumerable().Reverse();</code>

This is also available as query syntax:

<code class="EnlighterJSRAW" data-enlighter-language="csharp">(from i in list select i).Reverse();</code>

Full code:

<pre class="EnlighterJSRAW" data-enlighter-language="null">public class Program
{
  static void Main(string[] args)
  {
    // initialize list.
    var list = new List&lt;int&gt;
    {
      1, 5, 6, 7, 9, 10, 99, 777 
    };

    PrintList("Original List:",  list);

    list.Reverse();
    PrintList("Reversed full:", list);
    list.Reverse(); // reverse back since the list is changed.

    // reverse first 3 items.
    list.Reverse(0, 3);
    PrintList("Reversed first 3 items:", list);
    list.Reverse(0, 3); // reverse back.


    PrintList("Reversed Using LINQ full:", list.AsEnumerable().Reverse());

    PrintList("Reversed Using LINQ Query Syntax:", (from i in list select i).Reverse());
  }

  static void PrintList&lt;T&gt;(string message, IEnumerable&lt;T&gt; list)
  {
    Console.WriteLine($"{message}\r\n{string.Join("\r\n", list)}");
  }
}
</pre>

&nbsp;

## Useful links

  * <a href="https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.reverse?view=netframework-4.8#System_Collections_Generic_List_1_Reverse_System_Int32_System_Int32_" target="_blank" rel="noopener noreferrer">List<T> Reverse</a>

&nbsp;

&nbsp;

&nbsp;

&nbsp;

&nbsp;

&nbsp;