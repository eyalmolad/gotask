---
title: 'C# – How to add or remove items from Windows recent files'
author: eyal
type: post
date: 2020-05-02T23:19:51+00:00
url: /programming/c-sharp-how-to-add-or-remove-items-from-windows-recent-files/
categories:
  - Programming
tags:
  - 'c#'
  - pinvoke
  - shell-api

---
## Background

Starting Windows 7, Microsoft added a capability for displaying recently used files. This usually includes documents, pictures, and movies we&#8217;ve recently accessed. These files can be seen in various Windows components, including:

  * Recent files
  * Recent items
  * Start menu or application&#8217;s Jump List

The management of the listed files is done by the operating system.

In this post, I will show how to programmatically add and remove items from the Recent files list using C#.

## My Stack

  * Visual Studio 2019 Community Edition (16.5.1)
  * Console application built on .NET Framework 4.7.2 (C#) &#8211; 32/64 bit.
  * Windows 10 Pro 64-bit (10.0, Build 18363) (18362.19h1_release.190318-1202)

## Solution

  1. I created a helper class that uses Windows Shell API <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shaddtorecentdocs" target="_blank" rel="noopener noreferrer">SHAddToRecentDocs</a>.
  2. Imported the function using PInvoke.
  3. Added 2 functions: 
      * AddFile -> adds the file to Recent files view.
      * ClearAll -> clears all files from Recent files view.

&nbsp;

<pre class="EnlighterJSRAW" data-enlighter-language="null">public static class RecentDocsHelpers
{
  public enum ShellAddToRecentDocsFlags
  {
    Pidl = 0x001,
    Path = 0x002,
    PathW = 0x003
  }

  [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
  private static extern void SHAddToRecentDocs(ShellAddToRecentDocsFlags flag, string path);

  public static void AddFile(string path)
  {
    SHAddToRecentDocs(ShellAddToRecentDocsFlags.PathW, path);
  }

  public static void ClearAll()
  {
    SHAddToRecentDocs(ShellAddToRecentDocsFlags.Pidl, null);
  }
}</pre>

### Usage

<pre class="EnlighterJSRAW" data-enlighter-language="null">class Program
{
  static void Main(string[] args)
  {
    RecentDocsHelpers.ClearAll();

    // add c:\temp\sample.json to recent files.
    RecentDocsHelpers.AddFile(@"c:\temp\sample.json");
  }
}</pre>

## Limitation

  1. You can not add executable files to Recent files.

## Result

<figure id="attachment_231" aria-describedby="caption-attachment-231" style="width: 790px" class="wp-caption alignnone"><img loading="lazy" class="size-full wp-image-231" src="https://gotask.net/wp-content/uploads/2020/05/windows-recent-files-added-e1588421810539.png" alt="Windows recent files" width="800" height="261" /><figcaption id="caption-attachment-231" class="wp-caption-text">Windows recent files</figcaption></figure>

## Useful resources

  * Source code of this project on [GitHub][1]

 [1]: https://github.com/eyalmolad/gotask/tree/master/Utils/RecentFiles