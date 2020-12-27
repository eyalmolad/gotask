---
title: How to set up Google Search Console domain verification in Hostgator
author: eyal
type: post
date: 2020-03-30T17:34:17+00:00
url: /tutorials/how-to-set-up-google-search-console-domain-verification-in-hostgator/
categories:
  - Tutorials
tags:
  - cpanel
  - dns-records
  - google-search-console
  - hostgator

---
Recently, I set up a WordPress blog using HostGator&#8217;s shared hosting.

After writing the first post, I wanted to register the domain in Google Search Console (GSC) (former Google Web Master Tools). This tool provides you with very useful information on how Google&#8217;s search engine sees your site. GSC shows you insights regarding the performance of your site, referral links, sitemap, and some other experimental capabilities.

For this tutorial, I will use the sample domain name &#8216;sampleblog.blog&#8217;.

## Prerequisites

You will need an access to your HostGator cPanel. If you are not familiar with how to access it, please refer to the <a href="https://www.hostgator.com/help/article/how-to-log-into-cpanel" target="_blank" rel="noopener noreferrer"> logging on to cPanel</a> article.

## My Stack

  * HostGator Baby Croc account
  * cPanel version 78.0 (build 47)

## Step 1 &#8211; Registering your domain in GSC

In early 2019, Google announced its &#8216;domain properties&#8217; capability, providing an easy way to verify ownership of a domain in the Google Search Console.

If this is the first domain you are registering, once you open the GSC, you will see the following screen:

<img loading="lazy" class="alignnone size-full wp-image-115" src="https://gotask.net/wp-content/uploads/2020/03/select-property-type-gsc.png" alt="Domain property type in GSC" width="924" height="733" srcset="https://gotask.net/wp-content/uploads/2020/03/select-property-type-gsc.png 924w, https://gotask.net/wp-content/uploads/2020/03/select-property-type-gsc-300x238.png 300w, https://gotask.net/wp-content/uploads/2020/03/select-property-type-gsc-768x609.png 768w" sizes="(max-width: 924px) 100vw, 924px" /> 

  * Chose &#8216;Domain&#8217; option and type your domain name &#8216;sampleblog.blog&#8217;
  * Click on the &#8216;Continue&#8217; button.

## Step 2 &#8211; Verifying the ownership via DNS record

As GSC requires domain ownership proof, in the following screen it requests adding a new TXT record with a specific code.

<figure id="attachment_105" aria-describedby="caption-attachment-105" style="width: 769px" class="wp-caption alignnone"><img loading="lazy" class="size-full wp-image-105" src="https://gotask.net/wp-content/uploads/2020/03/verify-domain-ownership-dns.png" alt="Verify domain ownership DNS" width="779" height="677" srcset="https://gotask.net/wp-content/uploads/2020/03/verify-domain-ownership-dns.png 779w, https://gotask.net/wp-content/uploads/2020/03/verify-domain-ownership-dns-300x261.png 300w, https://gotask.net/wp-content/uploads/2020/03/verify-domain-ownership-dns-768x667.png 768w" sizes="(max-width: 779px) 100vw, 779px" /><figcaption id="caption-attachment-105" class="wp-caption-text">Verifying domain ownership with DNS record.</figcaption></figure>

  * In the &#8216;Instructions for&#8217; dropdown, you can find domain providers like GoDaddy and Name.com, for which Google is able to write the DNS records automatically. Unfortunately, HostGator is not supported in the list, so we will have to write them manually.
  * Make sure &#8216;Any DNS Provider&#8217; is selected.
  * Click on the &#8216;Copy&#8217; button to copy the text to your clipboard. Note this functionality is not fully supported in all browsers. In case the text is not copied, copy it manually using <kbd>CTRL</kbd>+<kbd>C</kbd> on Windows or <kbd>Command</kbd>+<kbd>C</kbd> on Mac.

## Step 3 &#8211; Setting the TXT record in HostGator

### Opening &#8216;Advanced Zone Editor&#8217;

  * Open the cPanel main screen
  * Locate the &#8216;DOMAINS&#8217; group
  * Click on &#8216;Advanced Zone Editor&#8217;

<img loading="lazy" class="alignnone size-full wp-image-106" src="https://gotask.net/wp-content/uploads/2020/03/cpanel-advanced-dns-zone.png" alt="cPanel Advanced Zone Editor" width="844" height="545" srcset="https://gotask.net/wp-content/uploads/2020/03/cpanel-advanced-dns-zone.png 844w, https://gotask.net/wp-content/uploads/2020/03/cpanel-advanced-dns-zone-300x194.png 300w, https://gotask.net/wp-content/uploads/2020/03/cpanel-advanced-dns-zone-768x496.png 768w" sizes="(max-width: 844px) 100vw, 844px" /> 

&nbsp;

### Selecting the TXT record type

  * In the following screen, locate the &#8216;Add Record&#8217; section
  * From the &#8216;Type&#8217; dropdown, select &#8216;TXT&#8217;

<figure id="attachment_107" aria-describedby="caption-attachment-107" style="width: 565px" class="wp-caption alignnone"><img loading="lazy" class=" wp-image-107" src="https://gotask.net/wp-content/uploads/2020/03/cpanel-add-txt-record.png" alt="Add TXT record in cPanel" width="575" height="452" srcset="https://gotask.net/wp-content/uploads/2020/03/cpanel-add-txt-record.png 768w, https://gotask.net/wp-content/uploads/2020/03/cpanel-add-txt-record-300x236.png 300w" sizes="(max-width: 575px) 100vw, 575px" /><figcaption id="caption-attachment-107" class="wp-caption-text">Choose TXT in the &#8216;Type&#8217; dropdown</figcaption></figure>

&nbsp;

### Filling the record fields

  * Leave the &#8216;Name&#8217; field blank, as HostGator will set it automatically to your domain name.
  * Set the TTL (Time to live) records to 14400 (meaning 4 hours). This tells the named server resolver how long in seconds to store the value in it&#8217;s cache. Since we are not expected to change this value, 4 hours is reasonable.
  * In the &#8216;TXT Data&#8217; field, paste the text you copied from Google Search Console.
  * Click on &#8216;Add Record&#8217; to finish.
  * Please note that it might take between a few seconds to a few hours until this change takes effect, so don&#8217;t get discouraged if GSC is not able to immediately verify your site.

&nbsp;

<figure id="attachment_109" aria-describedby="caption-attachment-109" style="width: 451px" class="wp-caption alignnone"><img loading="lazy" class=" wp-image-109" src="https://gotask.net/wp-content/uploads/2020/03/cpanel-fill-records-form.png" alt="TXT record form in cPanel" width="461" height="369" srcset="https://gotask.net/wp-content/uploads/2020/03/cpanel-fill-records-form.png 761w, https://gotask.net/wp-content/uploads/2020/03/cpanel-fill-records-form-300x240.png 300w" sizes="(max-width: 461px) 100vw, 461px" /><figcaption id="caption-attachment-109" class="wp-caption-text">Fill TXT records for site verification</figcaption></figure>

&nbsp;

## Step 4 &#8211; Verifying in Google Search Console

Assuming you set the TXT record correctly, the only part left is to let Google verify the record.

  * Go back to Google Search Console
  * Click on the &#8216;Verify&#8217; button
  * This process might take a few seconds. By the end of the process, you should see a message displaying that your domain is verified and added to Google Search Console.

## Troubleshooting

  * In case Google is not able to find the TXT records, it&#8217;s recommended to wait a few minutes/hours until the DNS TXT record is fully updated.

&nbsp;

## Useful Resources

  * Google <a href="https://webmasters.googleblog.com/2019/02/announcing-domain-wide-data-in-search.html" target="_blank" rel="noopener noreferrer">announces &#8216;domain properties&#8217;</a>