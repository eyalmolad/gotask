---
title: 'C# VSTO Addin Sample for Excel, Word, Power Point, Outlook'
author: eyal
type: post
date: 2020-03-20T19:27:52+00:00
url: /programming/vsto/c-sharp-vsto-addin-sample-for-excel-word-power-point-outlook/
toc: true
categories:
  - VSTO
tags:
  - dot-net
  - c-sharp
  - office-programming
  - visual-studio
---

## Background

Microsoft Office provides a variety of component types you could develop to extend its existing functionality or to add more useful features. These components include VBA macros, VSTO components, XLL Addons, and more.

In this sample, I will focus on creating a C# VSTO project that displays a simple ribbon.

## My Stack

* Visual Studio 2019 Community.
* .NET Framework 4.7.2 / C#
* Office 365, Desktop Edition.
* Windows 10 Pro 64-bit (10.0, Build 18362)

### Minimal requirements

* Visual Studio 2015
* Office 2013

The full source code available at [GitHub](https://github.com/eyalmolad/gotask/tree/master/VSTO/SimpleRibbon)

Note that the following Ribbon sample can be used for extending these Office applications: Excel, Word, Outlook, and PowerPoint.

In this post, I will use Excel as a hosting application.

## Setting up the environment

1. Open Visual Studio 2019 and create a new Excel VSTO Addin C# project.

{{<figure width="299" height="316" class="alignright" src="/wp-content/uploads/2020/03/visual-studio-setup.png" caption="Visual Studio Setup for Office/SharePoint">}}

In case you cannot find such a project on a Visual Studio templates list, complete the following steps: 
  * Open the Visual Studio 2019 setup from the Windows Control Panel.
  * Make sure _Office/SharePoint development option_ is selected as shown in the picture.
  
2. In the generated project, open the ```ThisAddin.cs``` file: 

  * ```ThisAddIn_Startup``` event handler will be called by the Excel application only once in the hosting application lifetime, during the application startup. This is the recommended place for initialization.
  * ```ThisAddIn_Shutdown``` event handler will be called by the application before it exits. Do your cleanups here.

  _Note: Outlook no longer raises this event. If you have code that must run when Outlook shuts down, see <a href="https://go.microsoft.com/fwlink/?LinkId=506785" target="_blank" rel="noopener noreferrer">this link</a>._

```C#
public partial class ThisAddIn
{
    private void ThisAddIn_Startup(object sender, System.EventArgs e)
    {
    }

    private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
    {
    }

    #region VSTO generated code
      /// summary
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// /summary
      private void InternalStartup()
      {
        this.Startup += new System.EventHandler(ThisAddIn_Startup);
        this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
      }
    #endregion
}
```

## Testing the environment

  1. Set a breakpoint in both ```C#ThisAddIn_Startup``` and ```ThisAddIn_Shutdown``` functions.
  2. Hit F5 to run the project's debugger. An Excel splash screen should appear and shortly after ```ThisAddIn_Startup``` breakpoint will hit.
  3. Close the Excel application, ```ThisAddIn_Shutdown``` should hit.

## Adding the Ribbon

When developing a VSTO, I prefer creating the ribbon by manually creating the XML rather than using the Ribbon Designer, which covers only a subset of the ```OfficeFluentUI``` model.  
You can find more explanations of the difference between using the Ribbon Designer and manually building the XML in these links: [xml vs ribbon designer](https://stackoverflow.com/questions/22483329/office-ribbon-xml-vs-office-standard-ribbon-designer) and [xml to ribbon designer comparison](https://social.msdn.microsoft.com/Forums/vstudio/en-US/e3a68e06-9e27-4d6c-bd1e-e566ab8b7506/ribbon-xml-vs-ribbon-designer?forum=vsto).

### Ribbon Controller

1. Add a new class RibbonController implementing ```Microsoft.Office.Core.IRibbonExtensibility``` interface.
2. Make sure to set the ```[ComVisible(true)]``` class attribute.
3. The only function you should implement is ```GetCustomUI``` that returns the XML representation of the ribbon you wish to create.
4. For this sample, we will create a simple button that display a message box.

```C#
[ComVisible(true)]
public class RibbonController : Microsoft.Office.Core.IRibbonExtensibility
{
    private Microsoft.Office.Core.IRibbonUI _ribbonUi;

    public string GetCustomUI(string ribbonID) =>
    @"<customUI xmlns='http://schemas.microsoft.com/office/2009/07/customui'>
      <ribbon>
        <tabs>
          <tab id='sample_tab' label='GoTask'>
            <group id='sample_group' label='Operations'>
              <button id='do_1' label='Do 1' size='large' getImage='OnDo1GetImage' onAction='OnDo1Click'/>
            </group>
          </tab>
        </tabs>
      </ribbon>
      </customUI>";

    public void OnLoad(Microsoft.Office.Core.IRibbonUI ribbonUI)
    {
        _ribbonUi = ribbonUI;
    }

    public void OnDo1Click(Microsoft.Office.Core.IRibbonControl control)
    {
        MessageBox.Show(Resources.Do1Action);
    }

    public Bitmap OnDo1GetImage(Microsoft.Office.Core.IRibbonControl control) => Resources.Do1_128px;
}
```

The sample XML above adds the ribbon to the Excel application, which includes:

1. A new ribbon tab labeled ```GoTask``` containing a new group labeled ```Operations```.
2. The group containing a button labeled ```Do 1``` with click handler implemented in ```OnDo1Click``` function.
3. The button displaying an image located in the Resource section.

Notes for ```onAction``` callback function:

* It must be public and have the following signature: ```public void SomeName(Microsoft.Office.Core.IRibbonControl control)```
* You could use any valid name, however it must match the name provided in ```onAction``` attribute in XML.
* For the proper initialization of the Ribbon, add ```OnLoad``` function, which will be called by the hosting application once the ribbon is ready to show.

### Creating an Instance of the Controller class

Once we have the RibbonController class, we need to create the instance of it.  
This is done by overriding the ```CreateRibbonExtensibilityObject()``` function in the ```ThisAddIn``` class.

```C#
protected override IRibbonExtensibility CreateRibbonExtensibilityObject() = new RibbonController();
```

### Running the project

1. Hit F5 to run the project again.
2. The Excel Application should show the ribbon with a new button.
  

{{<figure  width="810" height="146" src="/wp-content/uploads/2020/03/excel-with-ribbon-1024x185.png" caption="VSTO Excel Sample ribbon with button">}}  

#### Useful resources

1. Full source code of this post in [GitHub][1]
2. Full documentation of [Ribbon XML][2]
3. [Ribbon Designer][3] documentation
4. Office applications that [supports VSTO Addins][4].

[1]: https://github.com/eyalmolad/gotask/tree/master/VSTO/SimpleRibbon
[2]: https://docs.microsoft.com/en-us/visualstudio/vsto/ribbon-xml?view=vs-2019
[3]: https://docs.microsoft.com/en-us/visualstudio/vsto/ribbon-designer?view=vs-2019
[4]: https://docs.microsoft.com/en-us/visualstudio/vsto/features-available-by-office-application-and-project-type?view=vs-2019