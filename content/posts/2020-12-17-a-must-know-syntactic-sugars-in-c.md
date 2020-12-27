---
title: 'A must know syntactic sugars in C#'
author: eyal
type: post
date: 2020-12-17T14:29:29+00:00
draft: true
private: true
url: /programming/a-must-know-syntactic-sugars-in-c/
categories:
  - Programming

---
Replace casting with pattern variable

if (control is Button)  
{  
(control as Button)

}

&nbsp;

if (control is Button button)  
{

}