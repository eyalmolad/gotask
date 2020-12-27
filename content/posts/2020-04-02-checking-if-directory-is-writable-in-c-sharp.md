---
title: 'Checking if a directory is writable in C#'
author: eyal
type: post
date: 2020-04-02T21:21:36+00:00
url: /programming/checking-if-directory-is-writable-in-c-sharp/
categories:
  - Programming
tags:
  - .net
  - 'c#'
  - file-io
  - win32

---
Today, I am going to demonstrate a simple way to check if the current executing user has a writing permission for a directory in the Windows file system. I came across this issue in a project when I needed to write a utility program that would do the following: Output a list of all directories that the current user is <span style="text-decoration: underline;">not</span> able to write to.

## Background

Microsoft provides an API for manipulating or viewing security access permission via the <a href="https://docs.microsoft.com/en-us/dotnet/api/system.security.accesscontrol?view=netframework-4.8" target="_blank" rel="noopener noreferrer">System.Security.AccessControl</a> namespace. However, using the AccessControl based solution, requires computing the effective permissions for the user identity running your code. It might not be an easy task, as it involves fetching a security descriptor, an access token, and properly calculating the effective permissions.

As a general solution approach, I am going to try to write a file in a specific directory without any permissions calculations. In case an exception is raised by the operating system, I am going to properly handle it and assume the directory is not writable.

## My Stack

  * Visual Studio 2019 Community Edition (16.5.1)
  * .NET Framework 4.7.2 (C#) &#8211; 32/64 bit.
  * Windows 10 Pro 64-bit (10.0, Build 18363) (18362.19h1_release.190318-1202)

## Implementation

### Setting up the pInvoke imports

  *  So as a first step, I am going to create a utility static class DirectoryUtils that will include the implementation.
  * As I am going to use several Win32 API functions in the sample, so let&#8217;s import the following: 
      * <a href="https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilea" target="_blank" rel="noopener noreferrer">CreateFile</a>
      * <a href="https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-setfiletime" target="_blank" rel="noopener noreferrer">SetFileTime</a>

Note: When importing a Win32 API function into a .NET project, you need to generate the pInvoke signature. For such an operation, I highly recommend you use <a href="https://www.pinvoke.net" target="_blank" rel="noopener noreferrer">pinvoke.net</a>.

<pre class="EnlighterJSRAW" data-enlighter-language="csharp">public static class DirectoryUtils
{
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern SafeFileHandle CreateFile(
            string fileName,
            uint dwDesiredAccess,
            FileShare dwShareMode,
            IntPtr securityAttrs_MustBeZero,
            FileMode dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile_MustBeZero);

        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "SetFileTime", ExactSpelling = true)]
        private static extern bool SetFileTime(
            SafeFileHandle hFile,
            IntPtr lpCreationTimeUnused,
            IntPtr lpLastAccessTimeUnused,
            ref long lpLastWriteTime);

        private const uint FILE_ACCESS_GENERIC_READ = 0x80000000;
        private const uint FILE_ACCESS_GENERIC_WRITE = 0x40000000;

        private const int FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
        private const int OPEN_EXISTING = 3;
}</pre>

### Implementing the class

  1. In the DirectoryUtils class, create a static function DirectoryUtils.IsWritable that gets a directory path to check and returns bool.
  2. We need to create the temporary file using C# <code class="EnlighterJSRAW" data-enlighter-language="csharp">File.Create</code> with a random generated file name. Note the <code class="EnlighterJSRAW" data-enlighter-language="csharp">FileOptions.DeleteOnClose</code> flag, which ensures the file is deleted once we go out of the using scope.
  3. If the code below throws an exception, we assume the directory is not writable.
  4. <pre class="EnlighterJSRAW" data-enlighter-language="csharp">using (File.Create(Path.Combine(dirPath, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose))
{
}</pre>
    
    So far it looks very easy, but there is a small catch. If the directory is writable, its last write time will change every time we call DirectoryUtils.IsWritable, since we are creating a temporary file. This might look very ugly and unprofessional, especially if we are traversing a long directory tree. All directories will  have the &#8216;Date modified&#8217; changed in Windows Explorer as shown in the picture:
    
    <figure id="attachment_174" aria-describedby="caption-attachment-174" style="width: 835px" class="wp-caption alignnone"><img loading="lazy" class="size-full wp-image-174" src="https://gotask.net/wp-content/uploads/2020/04/windows-explorer.png" alt="Windows File Explorer" width="845" height="273" srcset="https://gotask.net/wp-content/uploads/2020/04/windows-explorer.png 845w, https://gotask.net/wp-content/uploads/2020/04/windows-explorer-300x97.png 300w, https://gotask.net/wp-content/uploads/2020/04/windows-explorer-768x248.png 768w" sizes="(max-width: 845px) 100vw, 845px" /><figcaption id="caption-attachment-174" class="wp-caption-text">Windows File Explorer</figcaption></figure></li> </ol> 
    
    The solution is the following:
    
      1. Save the write time before creating the temporary file by using <code class="EnlighterJSRAW" data-enlighter-language="csharp">Directory.GetLastWriteTimeUtc</code>
      2. Restore the write time after the temporary file is deleted by using <code class="EnlighterJSRAW" data-enlighter-language="csharp">SetFileTime</code> Win32 API.
    
    <pre class="EnlighterJSRAW" data-enlighter-language="csharp">public static bool SetDirectoryLastWriteUtc(string dirPath, DateTime lastWriteDate)
{
  using (var hDir = CreateFile(dirPath, FILE_ACCESS_GENERIC_READ | FILE_ACCESS_GENERIC_WRITE,
    FileShare.ReadWrite, IntPtr.Zero, (FileMode) OPEN_EXISTING,
    FILE_FLAG_BACKUP_SEMANTICS, IntPtr.Zero))
  {
    // put back to the date before checking.
    var lastWriteTime = lastWriteDate.ToFileTime();
    if (!SetFileTime(hDir, IntPtr.Zero, IntPtr.Zero, ref lastWriteTime))
    {
      return false;
    }
  }

  return true;
}

public static bool IsWritable(string dirPath)
{
  try
  {
    // Since there is a temp file that is being created,
    // this will change the modified date of the directory.
    // So if we have successful write operation, we need to
    // revert the last write date.
    var lastWriteDate = Directory.GetLastWriteTimeUtc(dirPath);

    // if this fails -&gt; it raises an exception.
    using (File.Create(Path.Combine(dirPath, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose))
    {
    }

    try
    {
      SetDirectoryLastWriteUtc(dirPath, lastWriteDate);
    }
    catch (Exception)
    {
      // add some log.
    }

    return true;
  }
  catch (UnauthorizedAccessException)
  {
     // add some log.
  }
  catch (Exception)
  {
    // add some log.
  }

  return false;
}</pre>
    
    ## Testing
    
    Running some tests on a development machine:
    
      1. Positive result: I used <code class="EnlighterJSRAW" data-enlighter-language="csharp" data-enlighter-theme="git">Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)</code>. In most cases, this directory is writable for the current non-admin user. Check that the last write date did not change after the function returned a &#8216;true&#8217; value.
      2. Negative result: If you are running the Visual Studio as a non-elevated process, the function should fail if you check the <code class="EnlighterJSRAW" data-enlighter-language="csharp">Environment.SystemDirectory</code>.
    
    <pre class="EnlighterJSRAW" data-enlighter-language="csharp">var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
var result = DirectoryUtils.IsWritable(dir);
Console.Write($"{dir} - result={result}");

dir = Environment.SystemDirectory;
result = DirectoryUtils.IsWritable(dir);
Console.Write($"{dir} - result={result}");</pre>
    
    ## Useful resources
    
      * Source code of this project on [GitHub][1]

 [1]: https://github.com/eyalmolad/gotask/tree/master/IO/DirectoryWritable