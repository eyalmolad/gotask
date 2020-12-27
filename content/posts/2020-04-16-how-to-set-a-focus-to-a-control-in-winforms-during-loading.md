---
title: How to set the focus during loading to a control in WinForms
author: eyal
type: post
date: 2020-04-16T18:13:46+00:00
url: /programming/winforms/how-to-set-a-focus-to-a-control-in-winforms-during-loading/
categories:
  - Winforms
tags:
  - 'c#'
  - combobox
  - textbox

---
## Background

Usually when creating forms with different controls, we would like the form be opened with a specific control in focus. This is usually true for Textboxes, but could be also very relevant for other controls such as ComboBox, Radio control, Listbox, and more. There are several ways to achieve this functionality with a very little of coding.

## My Stack

  * Visual Studio 2019 Community Edition (16.5.1)
  * WinForms application built on .NET Framework 4.7.2 (C#) &#8211; 32/64 bit.
  * Windows 10 Pro 64-bit (10.0, Build 18363) (18362.19h1_release.190318-1202)

## Solutions

<figure id="attachment_216" aria-describedby="caption-attachment-216" style="width: 490px" class="wp-caption alignright"><img loading="lazy" class="wp-image-216 size-full" src="https://gotask.net/wp-content/uploads/2020/04/win-form-sample-1.png" alt="Windows Form in sample controls" width="500" height="303" srcset="https://gotask.net/wp-content/uploads/2020/04/win-form-sample-1.png 500w, https://gotask.net/wp-content/uploads/2020/04/win-form-sample-1-300x182.png 300w" sizes="(max-width: 500px) 100vw, 500px" /><figcaption id="caption-attachment-216" class="wp-caption-text">Windows Form in sample controls</figcaption></figure>

### #1 Default behavior &#8211; the lowest TabIndex

By default, Windows will set the initial focus to the control with the lowest <code class="EnlighterJSRAW" data-enlighter-language="csharp">TabIndex</code> value.

### #2 Setting the active control property

A <code class="EnlighterJSRAW" data-enlighter-language="csharp">Form</code> inherited class contains the inherited property <code class="EnlighterJSRAW" data-enlighter-language="csharp">ActiveControl</code> of type <code class="EnlighterJSRAW" data-enlighter-language="csharp">Control</code>. As all Windows UI elements inherit from a Control, setting this reference to one of our controls in the Load event handler will automatically make it focused once the dialog is first shown.

<pre class="EnlighterJSRAW" data-enlighter-language="null">private void Form1_Load(object sender, EventArgs e)
{
  ActiveControl = textBox1;
}</pre>

&nbsp;

Note that the control must have the following properties set to True value:

  * Visible
  * Enabled

In case the one of the properties above is False, the focused control will be the next control according to the <code class="EnlighterJSRAW" data-enlighter-language="csharp">TabIndex.</code>

### #3 Calling the Focus() member function

I mentioned before that all Windows UI elements are inherited from the Control class. This class provides us the Focus() member function.

We can use this function to capture the focus to a specific control, but unfortunately it will not work in <a href="https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.load?view=netframework-4.8" target="_blank" rel="noopener noreferrer">Load</a> event handler. The reason is that we can not set focus to a control that haven&#8217;t been rendered (shown).

However, WinForms provides us the <a href="https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.shown?view=netframework-4.8" target="_blank" rel="noopener noreferrer">Show</a> event that occurs whenever the form is first displayed. In the event handler, we can call for Focus function as shown in the code below:

<pre class="EnlighterJSRAW" data-enlighter-language="null">private void Form1_Shown(object sender, EventArgs e)
{
  textBox1.Focus();
}</pre>

### #4 Calling the Select() member function

Looking at the <a href="https://referencesource.microsoft.com/#System.Windows.Forms/winforms/Managed/System/WinForms/Control.cs,6c9dc153b2c496ae" target="_blank" rel="noopener noreferrer">source code of Control.cs class</a>, calling the Select function without parameters is similar to setting the ActiveControl.

## 

&nbsp;