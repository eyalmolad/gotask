---
title: 'C# Deserialize JSON with fields without name fields'
author: eyal
type: post
date: 2020-03-25T18:34:09+00:00
draft: true
private: true
url: /programming/c-sharp-deserialize-json-with-fields-without-name-fields/
categories:
  - Programming

---
I recently had a situation when I needed to use data in JSON format coming from an external source (REST API). My goal in such cases is to deserialize the data into strongly typed objects with built in deserializers. Unfortunately, in this case, the JSON had the following format (simplified sample):

<pre class="EnlighterJSRAW" data-enlighter-language="json">[
  "2009",
  [
    {
      "Title": "Avatar",
      "Year": "2009",
      "Rated": "PG-13",
      "Released": "18 Dec 2009",
      "Runtime": 162
    }
  ],
  "2007",
  [
    {
      "Title": "I Am Legend",
      "Rated": "PG-13",
      "Released": "14 Dec 2007",
      "Runtime": 101
    }
  ]
]</pre>

The full source code available at <a href="https://github.com/eyalmolad/gotask/tree/master/XML/XMLDeserializeCore" target="_blank" rel="noopener noreferrer">GitHub</a>.

## My Stack

  * Visual Studio 2019
  * .NET Core 3.1
  * Notepad++ text editor for JSON

## Simple JSON Deserialize

sample 2 objects that represent a Movie, grouped by a year, but the year does not have a name field.

Since we could have an unknown number of years, we are going to use a Dictionary object when the key will be the year and the value is going to be the Movie class.

My favorite library for JSON handing in C# is <a href="https://www.newtonsoft.com/json" target="_blank" rel="noopener noreferrer">JSON.NET</a> (Newtonsoft.Json) which I am also going to using in this sample.

&nbsp;

  1. Create a simple C# .NET Core Console application.
  2. In Visual Studio 2019, &#8216;Package Manager Console&#8217; type: <code class="EnlighterJSRAW" data-enlighter-language="shell">Install-Package Newtonsoft.Json</code>
  3. Create a new JSON file with the contents above and add it to the project.
  4. Create a class that represents a Movie. In this case, I wrote the class definition manually, but you could always an online class generator like [quicktype][1].

&nbsp;

 [1]: https://app.quicktype.io/#l=C%23