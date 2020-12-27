---
title: 'C# – Save Excel with values only from VSTO'
author: eyal
type: post
date: 2020-10-14T09:40:42+00:00
draft: true
private: true
url: /programming/vsto/c-sharp-save-excel-with-values-only-vsto/
categories:
  - VSTO
tags:
  - 'c#'
  - office-programming

---
## Background

In my previous post on the <a href="https://gotask.net/programming/vsto/c-sharp-creating-excel-addin-with-user-defined-functions/" target="_blank" rel="noopener noreferrer">UDF functions in Excel</a>, I showed how to create an Excel Addin that has some UI elements and also has adds some UDF functions. I used the excellent [Excel-DNA][1] library to implement the UDF .

As UDF brings us the ability to develop more functions (formulas) that dont exist in Excel, it also adds the big disadvantage. As long as the user has the Addin installed on the computer, there is no issue. But what happens once this Excel file is sent to other user, via email for an example, that does not have the Addin installed? In most cases, once the sheet is calculated, the values calculated by the UDF will become #VALUE.

In this case, the Excel is not really usable unless the other user install the Addin that is responsible for the UDF calculation.

In this article, I will provide a simple technique how to save the Excel with values only before sending it to the user that does not have the Excel Addin.

## My Stack

  * Visual Studio 2019 Community Edition (16.7.4)
  * VSTO using .NET Framework 4.8
  * Office 365 &#8211; 64-bit

## Solution

I am going to use the sample code from my previous post that creates a <a href="https://gotask.net/programming/vsto/c-sharp-creating-excel-addin-with-user-defined-functions/" target="_blank" rel="noopener noreferrer">UDF that reverses the string</a> in a given cell.

### Locating the cells that have UDF

As the first step, we need to find all appearances of the our UDFs used in the Excel across all the sheets.

As our UDF name is ReverseString, we need to perform a search operation using the <a href="https://docs.microsoft.com/en-us/office/vba/api/excel.range.find" target="_blank" rel="noopener noreferrer">Range.Find</a> function across all sheets in the workbook. The find loop code is adapted from <a href="https://docs.microsoft.com/en-us/visualstudio/vsto/how-to-programmatically-search-for-text-in-worksheet-ranges?view=vs-2019" target="_blank" rel="noopener noreferrer">this example in MSDN</a>.

<pre class="EnlighterJSRAW" data-enlighter-language="csharp">var wb = App.ActiveWorkbook;

var functionName = "ReverseString";

foreach (_Worksheet sh in wb.Worksheets)
{
  Range currentFind = null;
  Range firstFind = null;

  currentFind = sh.UsedRange.Find(functionName, Type.Missing,
    XlFindLookIn.xlFormulas, XlLookAt.xlPart,
    XlSearchOrder.xlByRows, XlSearchDirection.xlNext, false, Type.Missing, Type.Missing);

  while (currentFind != null)
  {
    // Keep track of the first range you find. 
    if (firstFind == null)
    {
      firstFind = currentFind;
    }

    // If you didn't move to a new range, you are done.
    else if (currentFind.get_Address(XlReferenceStyle.xlA1)
         == firstFind.get_Address(XlReferenceStyle.xlA1))
    {
      break;
    }

    string f = currentFind.Formula.ToString();

    if (f.IndexOf(functionName, StringComparison.OrdinalIgnoreCase) &gt; -1)
    {
      TryReplaceValue(currentFind);
    }

    currentFind = sh.UsedRange.FindNext(currentFind);
  }
}</pre>

### Replacing the value

Lets take a look at the <code class="EnlighterJSRAW" data-enlighter-language="csharp">TryReplaceValue</code> ff

 [1]: https://excel-dna.net