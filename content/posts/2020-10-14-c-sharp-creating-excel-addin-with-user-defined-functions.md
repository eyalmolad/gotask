---
title: 'C# – Creating an Excel Addin with User Defined Functions'
author: eyal
type: post
date: 2020-10-14T20:00:42+00:00
url: /programming/vsto/c-sharp-creating-excel-addin-with-user-defined-functions/
categories:
  - VSTO
tags:
  - .net
  - 'c#'
  - excel
  - excel-dna
  - office-programming
  - ribbon-xml
  - udf

---
## Background

In one of my previous posts, I demonstrated how to create a simple <a href="https://gotask.net/programming/vsto/c-sharp-vsto-addin-sample-for-excel-word-power-point-outlook/" target="_blank" rel="noopener noreferrer">VSTO Addin for Excel</a> that adds a button on the ribbon. In this post, I am going to show how to add a user defined functions using <a href="https://excel-dna.net" target="_blank" rel="noopener noreferrer">Excel-DNA</a> as well as use the ribbon functionality.

## My Stack

<!-- /wp:paragraph -->

<!-- wp:list -->

  * Visual Studio 2019 Community.
  * .NET Framework 4.7.2 / C#
  * Office 365, Desktop Edition.
  * Windows 10 Pro 64-bit (10.0, Build 19041)

&nbsp;

## User Defined Function (UDF)

Excel provides a large set of built in functions, giving a user the ability to perform various calculations and manipulations on the data. But what happens if a user needs a custom calculation, that needs to be used over multiple cells? Starting Excel 2002, Microsoft introduced the User Defined Functions. This capability enables you to wrap some common calculation or string manipulation in a function and call it transparently as any other Excel build-in function.

### Example

Lets say we want to reverse a string in a cell. There are lot of examples of how to do that using the Excel built in functions. One of the examples is using <a href="https://support.microsoft.com/en-us/office/textjoin-function-357b449a-ec91-49d0-80c3-0e8fc845691c" target="_blank" rel="noopener noreferrer">TEXTJOIN</a> function:

<pre class="EnlighterJSRAW" data-enlighter-language="generic">=TEXTJOIN("",1,MID(A1,ABS(ROW(INDIRECT("1:"&LEN(A1)))-(LEN(A1)+1)),1))</pre>

Additional techniques could be found <a href="https://exceljet.net/formula/reverse-text-string" target="_blank" rel="noopener noreferrer">here</a>.

Much cleaner alternative would be creating a UDF that does this in C# and calling the function from the Excel spreadsheet with:

<code class="EnlighterJSRAW" data-enlighter-language="null">=ReverseString(A1)</code>

&nbsp;

## Creating an Excel Addin that supports UDF

From Visual Studio menu, create a new .NET Framework Class Library project.

### Installing dependencies

  1. First we need to reference the Excel-DNA project that enables us to make native XLL addins using C#. In the Visual Studio Package Manager Console type:

<pre class="EnlighterJSRAW" data-enlighter-language="shell">Install-Package ExcelDna.AddIn</pre>

Note: After installing the ExcelDna.Addin package, your project extension will be changed to xll which is a format for an addin that adds UDF functionality. You can read more about the <a href="https://docs.microsoft.com/en-us/office/client-developer/excel/creating-xlls" target="_blank" rel="noopener noreferrer">XLL addins on MSDN</a>.

    2. Since we want to create also some visual components and interact will the Excel elements we need to add the reference to: Microsoft.Office.Interop.Excel.dll. This component is usually located in your Office directory.

### Setting up the control classes

Since we want to combine the ExcelDna.Addin and the Ribbon objects, we can&#8217;t use the regular VSTO, but need to create the control classes manually.

  1. Create a class that implements the <code class="EnlighterJSRAW" data-enlighter-language="csharp">ExcelDna.Integration.IExcelAddIn</code> interface. <pre class="EnlighterJSRAW" data-enlighter-language="csharp">public class ExcelRibbonUDFAddin : IExcelAddIn
{
  public void AutoOpen()
  {
    // startup code
  }

  public void AutoClose()
  {
    // clean up
  }
}</pre>
    
    2. Create the ribbon controller class that derives from <code class="EnlighterJSRAW" data-enlighter-language="csharp">ExcelDna.Integration.CustomUI.ExcelRibbon</code>base class.
    
    <pre class="EnlighterJSRAW" data-enlighter-language="csharp">[ComVisible(true)]
public class RibbonController : ExcelRibbon, IDisposable
{
  private Microsoft.Office.Core.IRibbonUI _ribbonUi;

  private Application App
  {
    get =&gt; (Application)ExcelDnaUtil.Application;
  }            

  public override string GetCustomUI(string ribbonID) =&gt;
      @"&lt;customUI xmlns='http://schemas.microsoft.com/office/2009/07/customui'&gt;
          &lt;ribbon&gt;
             &lt;tabs&gt;
              &lt;tab id='sample_tab' label='GoTask'&gt;
                &lt;group id='sample_group' label='Operations'&gt;                                        
                  &lt;button id='do_reverse_range' label='Reverse' size='large' getImage='OnDoReverseGetImage' onAction='OnDoReverse'/&gt;
                &lt;/group&gt;
              &lt;/tab&gt;
            &lt;/tabs&gt;
          &lt;/ribbon&gt;
        &lt;/customUI&gt;";

  public void OnLoad(Microsoft.Office.Core.IRibbonUI ribbonUI)
  {
    _ribbonUi = ribbonUI;          
  }        
   
  public void Dispose()
  {            
  }
}</pre>
    
    &nbsp;
    
    a) ExcelDnaUtil.Application returns the Excel Application object instance. b) GetCustomUI returns the Ribbon XML string. You can find the full specification regarding the 
    
    <a href="https://docs.microsoft.com/en-us/visualstudio/vsto/ribbon-xml?view=vs-2019" target="_blank" rel="noopener noreferrer">Ribbon XML format on MSDN</a>.

&nbsp;

### Adding the UDF functionality

Create a new static class that will contain the Reverse string function implementation. Make sure that you add <code class="EnlighterJSRAW" data-enlighter-language="null">ExcelFunction</code>attribute to it.

Every time you add <code class="EnlighterJSRAW" data-enlighter-language="null">=ReverseString</code>to any cell in the Excel, this function will be called.

<pre class="EnlighterJSRAW" data-enlighter-language="csharp">public static class CustomFunctions
{
  [ExcelFunction(Description = "Reverse string function")]
  public static string ReverseString(string str)
  {
    var charArray = str.ToCharArray();
    Array.Reverse(charArray);
    return new string(charArray);
  }
}</pre>

### Testing the project

1. Build the project and run it in Debug mode. This should open the Excel application with the addin loaded.<figure id="attachment_249" aria-describedby="caption-attachment-249" style="width: 230px" class="wp-caption alignright">

<img loading="lazy" class="size-full wp-image-249" src="https://gotask.net/wp-content/uploads/2020/10/reverse-string-excel-e1602775342176.png" alt="Reverse string in Excel" width="240" height="228" /> <figcaption id="caption-attachment-249" class="wp-caption-text">Excel functions intellisense</figcaption></figure> 

2. Go to some cell and type <code class="EnlighterJSRAW" data-enlighter-language="null">=ReverseString</code> passing a reference to a cell or hard coded string.

3. Your target cell should contain the reversed string. Since you are running in the debug mode, you can always set a breakpoint in the ReverseString function.

&nbsp;

## Adding the reverse function to an existing range

After we built the basic sample, we can connect it to a button on the ribbon that reverses the string of the selected range and inserts the results to the new column.

For this, we need to implement the ribbon button action function in the Ribbon Controller class:

<pre class="EnlighterJSRAW" data-enlighter-language="csharp">public void OnDoReverse(Microsoft.Office.Core.IRibbonControl control)
{
  var selectedRange = App.Selection;

  if (selectedRange == null) return;

  foreach (Range cell in selectedRange)
  {
    var next = cell.Offset[0, 1];
    next.Formula = $"=ReverseString({cell.Address})";
  }
}</pre>

## Useful resources

  * Source code of this project on <a href="https://github.com/eyalmolad/gotask/tree/master/VSTO/UDF/ExcelRibbonUDF" target="_blank" rel="noopener noreferrer">GitHub</a>

&nbsp;<figure id="attachment_254" aria-describedby="caption-attachment-254" style="width: 290px" class="wp-caption aligncenter">

<img loading="lazy" class="wp-image-254 size-full" src="https://gotask.net/wp-content/uploads/2020/10/image_2020-10-15_184255-e1602780284748.png" alt="Reverse string in Excel" width="300" height="294" /> <figcaption id="caption-attachment-254" class="wp-caption-text">Reverse string in Excel Result</figcaption></figure>