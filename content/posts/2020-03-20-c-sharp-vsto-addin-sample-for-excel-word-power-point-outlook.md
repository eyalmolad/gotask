---
title: 'C# VSTO Addin Sample for Excel, Word, Power Point, Outlook'
author: eyal
type: post
date: 2020-03-20T19:27:52+00:00
url: /programming/vsto/c-sharp-vsto-addin-sample-for-excel-word-power-point-outlook/
categories:
  - VSTO
tags:
  - .net
  - 'c#'
  - office-programming
  - visual-studio

---
Microsoft Office provides a variety of component types you could develop to extend its existing functionality or to add more useful features. These components include VBA macros, VSTO components, XLL Addons, and more.

In this sample, I will focus on creating a C# VSTO project that displays a simple ribbon.

## My Stack

  * Visual Studio 2019 Community.
  * .NET Framework 4.7.2 / C#
  * Office 365, Desktop Edition.
  * Windows 10 Pro 64-bit (10.0, Build 18362)

## Minimal requirements

  * Visual Studio 2015
  * Office 2013

The full source code available at <a href="https://github.com/eyalmolad/gotask/tree/master/VSTO/SimpleRibbon" target="_new" rel="noopener noreferrer">GitHub</a>.

Note that the following Ribbon sample can be used for extending these Office applications: Excel, Word, Outlook, and PowerPoint.

In this post, I will use Excel as a hosting application.

## Setting up the environment

  1. Open Visual Studio 2019 and create a new Excel VSTO Addin C# project. <figure id="attachment_22" aria-describedby="caption-attachment-22" style="width: 289px" class="wp-caption alignright"><a href="https://gotask.net/wp-content/uploads/2020/03/visual-studio-setup.png" target="_blank" rel="noopener noreferrer"><img loading="lazy" class="wp-image-22" src="https://gotask.net/wp-content/uploads/2020/03/visual-studio-setup.png" alt="Visual Studio Setup for Office/SharePoint" width="299" height="316" srcset="https://gotask.net/wp-content/uploads/2020/03/visual-studio-setup.png 708w, https://gotask.net/wp-content/uploads/2020/03/visual-studio-setup-284x300.png 284w" sizes="(max-width: 299px) 100vw, 299px" /></a><figcaption id="caption-attachment-22" class="wp-caption-text">Visual Studio Setup for Office/SharePoint</figcaption></figure>  
    In case you cannot find such a project on a Visual Studio templates list, complete the following steps: 
      * Open the Visual Studio 2019 setup from the Windows Control Panel.
      * Make sure &#8220;Office/SharePoint development option&#8221; is selected as shown in the picture.
  2. In the generated project, open the <code class="EnlighterJSRAW" data-enlighter-language="csharp" data-enlighter-theme="git">ThisAddin.cs</code> file: 
      * <code class="EnlighterJSRAW" data-enlighter-language="csharp" data-enlighter-theme="git">ThisAddIn_Startup</code> event handler will be called by the Excel application only once in the hosting application lifetime, during the application startup. This is the recommended place for initialization.
      * <code class="EnlighterJSRAW" data-enlighter-language="csharp" data-enlighter-theme="git">ThisAddIn_Shutdown</code> event handler will be called by the application before it exits. Do your cleanups here.
      * _Note: Outlook no longer raises this event. If you have code that must run when Outlook shuts down, see <a href="https://go.microsoft.com/fwlink/?LinkId=506785" target="_blank" rel="noopener noreferrer">this link</a>._

&nbsp;

<pre class="EnlighterJSRAW" data-enlighter-language="csharp">public partial class ThisAddIn
{
  private void ThisAddIn_Startup(object sender, System.EventArgs e)
  {
  }

  private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
  {
  }

  #region VSTO generated code

  /// &lt;summary&gt;
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// &lt;/summary&gt;
  private void InternalStartup()
  {
    this.Startup += new System.EventHandler(ThisAddIn_Startup);
    this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
  }
  
  #endregion
}</pre>

## Testing the environment

  1. Set a breakpoint in both <code class="EnlighterJSRAW" data-enlighter-language="csharp" data-enlighter-theme="git">ThisAddIn_Startup</code> and <code class="EnlighterJSRAW" data-enlighter-language="csharp" data-enlighter-theme="git">ThisAddIn_Shutdown</code> functions.
  2. Hit F5 to run the project&#8217;s debugger. An Excel splash screen should appear and shortly after <code class="EnlighterJSRAW" data-enlighter-language="csharp" data-enlighter-theme="git">ThisAddIn_Startup</code> breakpoint will hit.
  3. Close the Excel application, <code class="EnlighterJSRAW" data-enlighter-language="csharp" data-enlighter-theme="git">ThisAddIn_Shutdown</code> should hit.

## Adding the Ribbon

When developing a VSTO, I prefer creating the ribbon by manualy creating the XML rather than using the Ribbon Designer, which covers only a subset of the <code class="EnlighterJSRAW" data-enlighter-language="csharp" data-enlighter-theme="git">OfficeFluentUI</code> model.  
You can find more explanations of the difference between using the Ribbon Designer and manually building the XML in these links: <a href="https://stackoverflow.com/questions/22483329/office-ribbon-xml-vs-office-standard-ribbon-designer" target="_blank" rel="noopener noreferrer">xml vs ribbon designer</a> and <a href="https://social.msdn.microsoft.com/Forums/vstudio/en-US/e3a68e06-9e27-4d6c-bd1e-e566ab8b7506/ribbon-xml-vs-ribbon-designer?forum=vsto" target="_blank" rel="noopener noreferrer">xml to ribbon designer comparison</a>.

### Ribbon Controller

  1. Add a new class RibbonConroller implementing <code class="EnlighterJSRAW" data-enlighter-language="csharp" data-enlighter-theme="git">Microsoft.Office.Core.IRibbonExtensibility</code> interface.
  2. Make sure to set the <code class="EnlighterJSRAW" data-enlighter-language="csharp" data-enlighter-theme="git">[ComVisible(true)]</code> class attribute.
  3. The only function you should implement is <code class="EnlighterJSRAW" data-enlighter-language="csharp" data-enlighter-theme="git">GetCustomUI</code> that returns the XML representation of the ribbon you wish to create.
  4. For this sample, we will create a simple button that display a message box.

<pre class="EnlighterJSRAW" data-enlighter-language="csharp">[ComVisible(true)]
public class RibbonController : Microsoft.Office.Core.IRibbonExtensibility
{
  private Microsoft.Office.Core.IRibbonUI _ribbonUi;

  public string GetCustomUI(string ribbonID) =&gt;
  @"&lt;customUI xmlns='http://schemas.microsoft.com/office/2009/07/customui'&gt;
    &lt;ribbon&gt;
       &lt;tabs&gt;
        &lt;tab id='sample_tab' label='GoTask'&gt;
          &lt;group id='sample_group' label='Operations'&gt;
            &lt;button id='do_1' label='Do 1' size='large' getImage='OnDo1GetImage' onAction='OnDo1Click'/&gt;
          &lt;/group&gt;
        &lt;/tab&gt;
      &lt;/tabs&gt;
    &lt;/ribbon&gt;
    &lt;/customUI&gt;";

  public void OnLoad(Microsoft.Office.Core.IRibbonUI ribbonUI)
  {
    _ribbonUi = ribbonUI;
  }

  public void OnDo1Click(Microsoft.Office.Core.IRibbonControl control)
  {
    MessageBox.Show(Resources.Do1Action);
  }

  public Bitmap OnDo1GetImage(Microsoft.Office.Core.IRibbonControl control) =&gt; Resources.Do1_128px;
}</pre>

The sample XML above adds the ribbon to the Excel application, which includes:

  1. A new ribbon tab labeled &#8216;GoTask&#8217; containing a new group labeled &#8216;Operations&#8217;.
  2. The group containing a button labeled &#8216;Do 1&#8217; with click handler implemented in OnDo1Click function.
  3. The button displaying an image located in the Resource section.

Notes for <code class="EnlighterJSRAW" data-enlighter-language="csharp" data-enlighter-theme="git">onAction</code> callback function:

  * It must be public and have the following signature: <code class="EnlighterJSRAW" data-enlighter-language="csharp">public void SomeName(Microsoft.Office.Core.IRibbonControl control)</code>
  * You could use any valid name, however it must match the name provided in onAction attribute in XML.
  * For the proper initialization of the Ribbon, add <code class="EnlighterJSRAW" data-enlighter-language="csharp" data-enlighter-theme="git">OnLoad</code> function, which will be called by the hosting application once the ribbon is ready to show.

### Creating an Instance of the Controller class

Once we have the RibbonController class, we need to create the instance of it.  
This is done by overriding the <code class="EnlighterJSRAW" data-enlighter-language="csharp" data-enlighter-theme="git">CreateRibbonExtensibilityObject()</code> function in the ThisAddIn class.

<code class="EnlighterJSRAW" data-enlighter-language="csharp">protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject() =&gt; new RibbonController();</code>

### Running the project

  1. Hit F5 to run the project again.
  2. The Excel Application should show the ribbon with a new button.<figure id="attachment_34" aria-describedby="caption-attachment-34" style="width: 800px" class="wp-caption alignnone">

<a href="https://gotask.net/wp-content/uploads/2020/03/excel-with-ribbon.png" target="_blank" rel="noopener noreferrer"><img loading="lazy" class="wp-image-34 size-large" src="https://gotask.net/wp-content/uploads/2020/03/excel-with-ribbon-1024x185.png" alt="VSTO Excel Sample ribbon with button" width="810" height="146" srcset="https://gotask.net/wp-content/uploads/2020/03/excel-with-ribbon-1024x185.png 1024w, https://gotask.net/wp-content/uploads/2020/03/excel-with-ribbon-300x54.png 300w, https://gotask.net/wp-content/uploads/2020/03/excel-with-ribbon-768x139.png 768w, https://gotask.net/wp-content/uploads/2020/03/excel-with-ribbon.png 1328w" sizes="(max-width: 810px) 100vw, 810px" /></a><figcaption id="caption-attachment-34" class="wp-caption-text">VSTO Excel Sample ribbon with button</figcaption></figure> 

#### Useful resources

  1. Full source code of this post in [GitHub][1]
  2. Full documentation of [Ribbon XML][2]
  3. [Ribbon Designer][3] documentation
  4. Office applications that [supports VSTO Addins][4].

 [1]: https://github.com/eyalmolad/gotask/tree/master/VSTO/SimpleRibbon
 [2]: https://docs.microsoft.com/en-us/visualstudio/vsto/ribbon-xml?view=vs-2019
 [3]: https://docs.microsoft.com/en-us/visualstudio/vsto/ribbon-designer?view=vs-2019
 [4]: https://docs.microsoft.com/en-us/visualstudio/vsto/features-available-by-office-application-and-project-type?view=vs-2019