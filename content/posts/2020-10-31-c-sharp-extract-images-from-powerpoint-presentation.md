---
title: 'C# – VSTO – Extract images from PowerPoint presentation'
author: eyal
type: post
date: 2020-10-31T07:43:24+00:00
url: /programming/vsto/c-sharp-extract-images-from-powerpoint-presentation/
categories:
  - VSTO
tags:
  - .net
  - 'c#'
  - image
  - interop
  - open-xml
  - powerpoint
  - ribbon-xml
  - vsto
  - windows-api-codepack
  - zip

---
## Background

In one of my previous posts, I created a very basic VSTO example that adds a button to the [PowerPoint ribbon][1].

Recently, I had a task I needed to enumerate all the pictures in the Power Point presentation and extract them into a zip file.

Power Point presentation might contain many different shapes, such as rectangles, lines, arrows, textboxes, pictures and more. Each shape might contain a text, but it might also contain a picture. 

In this post, I am going to show how to extract the images using 2 different techniques:

  * PowerPoint COM Interop API
  * Extract directly from ZIP file

## My Stack

<!-- /wp:paragraph -->

<!-- wp:list -->

  * Visual Studio 2019 Community.
  * .NET Framework 4.7.2 / C#
  * Office 365, Desktop Edition.
  * Windows 10 Pro 64-bit (10.0, Build 19041)
  * PowerPoint Interop DLL version 15

## Working with PowerPoint C# Interop version 15.0.0.0

### Step 1 &#8211; Create a button

As shown in the previous example, I am adding a button element to the Ribbon XML. This button will have a callback set in the action attribute.

<pre class="EnlighterJSRAW" data-enlighter-language="xml">&lt;customUI xmlns='http://schemas.microsoft.com/office/2009/07/customui'&gt;
  &lt;ribbon&gt;
     &lt;tabs&gt;
      &lt;tab id='sample_tab' label='GoTask'&gt;
        &lt;group id='sample_group' label='Operations'&gt;
          &lt;button id='extract_images' label='Extract Images' size='large' getImage='OnGetImage' onAction='OnExtractImage'/&gt;
        &lt;/group&gt;
      &lt;/tab&gt;
    &lt;/tabs&gt;
  &lt;/ribbon&gt;
&lt;/customUI&gt;</pre>

###  

### Step 2 &#8211; Collect the images from different shapes

PowerPoint presentation can store the images in a few shapes types. All the different shape types are represented by <a href="https://docs.microsoft.com/en-us/office/vba/api/office.msoshapetype" target="_blank" rel="noopener noreferrer">MsoShapeType</a> enum.  In order to recognize the Shape type, we are going to use <a href="https://docs.microsoft.com/en-us/office/vba/api/powerpoint.shape.type" target="_blank" rel="noopener noreferrer">Shape.Type</a> and <a href="https://docs.microsoft.com/en-us/office/vba/api/powerpoint.shape.placeholderformat" target="_blank" rel="noopener noreferrer">Shape.PlaceholderFormat.ContainedType</a> properties: 

  * Picture &#8211;  MsoShapeType.msoPicture or MsoShapeType.msoLinkedPicture
  * Picture contained in a placeholder MsoShapeType.msoPlaceholder
  * Other shapes that might have a <a href="https://docs.microsoft.com/en-us/office/vba/api/powerpoint.pictureformat" target="_blank" rel="noopener noreferrer">PictureFormat</a> property properly initialized.

In the [sample presentation][2], I&#8217;ve created a few shapes that contain pictures in different formats.

In order to extract the image, I am going to use the PowerPoint Shape <a href="https://docs.microsoft.com/en-us/previous-versions/office/office-12/ff761596(v=office.12)" target="_blank" rel="noopener noreferrer">Export</a> function.

In order to choose a directory for saving the images, I am going to use the CommonOpenFileDialog implemented in <a href="https://www.nuget.org/packages/Microsoft-WindowsAPICodePack-Shell/" target="_blank" rel="noopener noreferrer">Microsoft-WindowsAPICodePack-Shell</a>. Here is the sample implementation of using a directory picker:

<pre class="EnlighterJSRAW" data-enlighter-language="csharp">private string GetSaveDir()
{
  using (var dialog = new CommonOpenFileDialog())
  {
    dialog.IsFolderPicker = true;

    var result = dialog.ShowDialog();

    if (result == CommonFileDialogResult.Ok)
    {
      return dialog.FileName;
    }
  }

  return null;
}</pre>

The code below iterates over all slides in the presentation and extracts the images from the shapes.

Please note the following remarks:

  * The extracted images are in PNG format using the <code class="EnlighterJSRAW" data-enlighter-language="csharp">PpShapeFormat.ppShapeFormatPNG</code> enum. You can specify JPG, BMP or other formats defined in the <code class="EnlighterJSRAW" data-enlighter-language="csharp">PpShapeFormat</code> enum.
  * Pay attention for the <code class="EnlighterJSRAW" data-enlighter-language="csharp">shape.PictureFormat.CropBottom</code> check. Generally, every shape has <code class="EnlighterJSRAW" data-enlighter-language="csharp">PictureFormat</code> set to a non-null value. So we can&#8217;t count on filtering out the shapes that have this property set to null. The trick is to try to access one of the properties (CropBottom or other). If the exception is thrown, we can skip the object (it&#8217;s not a picture).

<pre class="EnlighterJSRAW" data-enlighter-language="null">var i = 1;
foreach (Slide slide in app.ActivePresentation.Slides)
{
  foreach (Shape shape in slide.Shapes)
  {
    var doExport = false;

    if (shape.Type == MsoShapeType.msoPicture ||
      shape.Type == MsoShapeType.msoLinkedPicture)
    {
      doExport = true;
    }
    else if (shape.Type == MsoShapeType.msoPlaceholder)
    {
      if (shape.PlaceholderFormat.ContainedType == MsoShapeType.msoPicture ||
        shape.PlaceholderFormat.ContainedType == MsoShapeType.msoLinkedPicture)
      {
        doExport = true;
      }
    }
    else
    {
      try
      {
        // this is just a dummy code. In case there is no picture in the
        // shape, any attempt to read the CropBottom variable will throw 
        // an exception
        var test = shape.PictureFormat.CropBottom &gt; -1;
        doExport = true;
      }
      catch
      {
        doExport = false;
      }
    }

    if(doExport) 
      shape.Export(Path.Combine(saveDirectory, $"{i++}.png"), PpShapeFormat.ppShapeFormatPNG);
  }
}</pre>

When running this code on the presentation provided with the project, it should export 4 pictures to the chosen directory. (Picture&#8217;s credit: <a href="https://unsplash.com" target="_blank" rel="noopener noreferrer">Unsplash</a>)

## Working with ZIP file to extract the images

The pptx format is actually a zip file with a well formed structure defined in the Open-XML format. You could open the pptx file with any zip file extractor and look at it&#8217;s contents. Fortunately, the pictures are stored in the <code class="EnlighterJSRAW" data-enlighter-language="generic">ppt\media</code> directory within the archive.

All I have to do now it to extract the archive and grab the images.

I am going to use the .NET <a href="https://docs.microsoft.com/en-us/dotnet/api/system.io.compression.zipfile?view=netcore-3.1" target="_blank" rel="noopener noreferrer">ZipFile</a> class located in System.IO.Compression namespace.

  1. Open the pptx file using <code class="EnlighterJSRAW" data-enlighter-language="csharp">ZipFile.Open</code>
  2. Create a temporary <code class="EnlighterJSRAW" data-enlighter-language="generic">temp_zip</code> directory to extract the files to
  3. Copy the media files
  4. Delete the temporary <code class="EnlighterJSRAW" data-enlighter-language="generic">temp_zip</code> directory

<pre class="EnlighterJSRAW" data-enlighter-language="csharp">private void ExtractWithZip(string pptxFile, string directory)
{
  var zipDir = "";

  using (var arh = ZipFile.Open(pptxFile, ZipArchiveMode.Read))
  {
    zipDir = Path.Combine(directory, "temp_zip");
    Directory.CreateDirectory(zipDir);
    arh.ExtractToDirectory(zipDir); // extract

        // iterate over files in the extracted dir.
    foreach (var f in Directory.GetFiles(Path.Combine(zipDir, @"ppt\media")))
      File.Copy(f, Path.Combine(directory, Path.GetFileName(f)));
  }

  // clean up
  try
  {
    var dirToDelete = new DirectoryInfo(zipDir);
    dirToDelete.Delete(true);
  }
  catch
  {
    //
  }
}</pre>

## Useful resources

  * Source code of this project on <a href="https://github.com/eyalmolad/gotask/tree/master/VSTO/PowerPointExtractImages" target="_blank" rel="noopener noreferrer">GitHub</a>

 [1]: https://gotask.net/programming/vsto/c-sharp-vsto-addin-sample-for-excel-word-power-point-outlook/
 [2]: https://github.com/eyalmolad/gotask/blob/master/VSTO/PowerPointExtractImages/SamplePresentation.pptx