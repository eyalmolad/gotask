---
title: Setup Hugo Site Generator on GitHub Pages with a Custom Domain
author: eyal
type: post
date: 2020-12-13T14:49:18+00:00
url: /tutorials/setup-hugo-on-github-pages-custom-domain/
categories:
  - Tutorials
tags:
  - a-record
  - chocolatey
  - cname-record
  - command-line
  - dns-records
  - domain
  - git
  - git-hub
  - git-hub-pages
  - hugo
  - jamstack
  - package-manager

---
## Background

Recently, I wanted to setup a new blog. I have purchased the domain and started looking for the hosting.

My main goal was to build a clean and a simple site that loads fast. Instead of working with the traditional WordPress platform and trying to configure dozens of plugins to increase the performance, I decided to go with a Jamstack based solution. <a href="https://jamstack.org" target="_blank" rel="noopener noreferrer">Jamstack</a> based sites are pre-generated and served from the static repositories or CDNs, so no pages are generated on the fly when the users access the site.

To pre-generate the site, I chose <a href="https://gohugo.io" target="_blank" rel="noopener noreferrer">Hugo</a>, which is a static site generator optimized for speed and relatively easy to configure. Hugo is an open source project written in Go language.

Since the site is pre-generated and can be stored in the GitHub, I chose the GitHub Pages as a hosting. The deployment on GitHub Pages is very simple (a single git push) command, it can setup a custom domain and it has the built-in support for SSL.

## Requirements

  * <a href="https://git-scm.com/downloads" target="_blank" rel="noopener noreferrer">Git</a> installed on your local machine (including the command line interface)
  * <a href="https://github.com" target="_blank" rel="noopener noreferrer">GitHub</a> account
  * If you wish to setup a &#8216;private&#8217; repository with the custom domain, you will need to purchase one of the GitHub subscriptions. I use the &#8216;Team&#8217; subscription which costs 4$ per month (as of Dec/2020). You may check the <a href="https://github.com/pricing" target="_blank" rel="noopener noreferrer">pricing for more information.</a>
  * A Text editor, I use <a href="https://notepad-plus-plus.org" target="_blank" rel="noopener noreferrer">Notepad ++</a>
  * A basic command line knowledge

## Setting up the repository

### Installing Hugo on the local computer

As described in the <a href="https://github.com/gohugoio/hugo" target="_blank" rel="noopener noreferrer">Hugo Git repository</a>, there are two ways to install the Hugo site generator on the local machine:

  * Install binary files using one of the common Package Managers
  * Build from the sources

For this post we will proceed with the binary files&#8217; installation. Since I am using Windows 10 operating system, I used the <a href="https://chocolatey.org" target="_blank" rel="noopener noreferrer">Chocolatey Package Manager</a>. I installed Chocolatey as an Administrator as described in the <a href="https://chocolatey.org/install" target="_blank" rel="noopener noreferrer">install page</a>.

For Mac/Linux users, I recommend working with <a href="https://brew.sh" target="_blank" rel="noopener noreferrer">Homebrew</a>.

After having the package manager installed, we can use a command line to install the Hugo site generator.

#### Windows

Open the command line prompt as an Administrator and type:

```
choco install hugo -confirm
```

#### Mac/Linux

```
brew install hugo
```

More instructions for the <a href="https://gohugo.io/getting-started/installing/" target="_blank" rel="noopener noreferrer">Hugo installation</a>

### Setting up the repository on GitHub

Since Hugo is pre-generated site and served from a Git repository, we need to create a
new repository on GitHub.

{{<figure src="https://gotask.net/wp-content/uploads/2020/12/git-create-repository-1.png" caption="Create new private repository in GitHub">}}

1. In your web browser, go to <a href="https://github.com/new" target="_blank" rel="noopener noreferrer">https://github.com/new</a>.
2. Choose the name for the repository. For this example, we are going to use: ```myhugoblog```
3. Since we are not creating an open source project, but the repository that will contain my blog, we will set the repository to &#8216;Private&#8217;. This step is not mandatory as you can setup the site and the custom domain with public repositories as well.

 
### Setting up the local directory for the site

  1. Create a local directory that will contain the theme and the pre-generated site. For this example, we will use: ```c:\sites\``` directory.
  2. Open the command line prompt at ```c:\sites\``` and type:

```
git clone https://github.com/<user_name>/<repository_name>.git
```
   
Note: In some cases, you might get an error that repository does not exists. In such case, try to clone with following url (you will be prompted for the password):

```
git clone https://<user_name>@github.com/<user_name>/<repository_name>.git</code>
```

### Generate new Hugo site

At this stage, we have an empty repository connected to our local directory. Now we need to generate the Hugo site.

1. In the command line prompt make sure that the current directory is ```c:\sites\``` and type: 

```
hugo new site myhugoblog --force
```

2. Important note: The root directory of the repository in Git and the root directory of the pre-generated site <span style="text-decoration: underline;">must be the same</span>.

### Choosing the Hugo theme

<a href="https://themes.gohugo.io" target="_blank" rel="noopener noreferrer">Hugo site generator</a> has a few hundreds of free themes available. I decided to go with <a href="https://themes.gohugo.io//theme/bodhi/blog" target="_blank" rel="noopener noreferrer">Bodhi blog theme</a>. It is clean, simple and minimalistic. A perfect solution for my new blog.

  1. Download the zip archive of the theme from the theme repository. In our case, it is https://github.com/rhnvrm/bodhi/archive/master.zip
  2. Extract the zip file to ```c:\sites\<repository_name>\themes```. In our case, it&#8217;s ```c:\sites\myhugoblog```.
  3. Typically, your directory name in the zip file will be: ```<theme_name>\_master```. Remove the ```_master``` suffix from the directory name. In our case, we will have ```c:\sites\myhugoblog\themes\bodhi``` directory.
  4. Open the ```config.toml``` from the site root directory in the text editor and paste the default configuration of the theme as described in your theme page. Please note that different themes might have different configurations options. 
      * Make sure you change the baseUrl property to your custom domain url.
      * Make sure you add/change the default publish directory in the ```config.toml``` to: 
          * ```publishDir = docs```
  5. To run the site locally type the following in your command prompt: 
```
  hugo server
```
  6. Open your browser at ```http://localhost:1313/``` to see the site. Currently it should have only the theme&#8217;s default page. (Note that the port might be different. Check the console window for the exact address).
  7. If you are happy with the site, press ```CTRL + X``` to kill the server.

### Deploying the site to Git repository

Once we have the site running locally, we need to build it (generate the static pages, links, sitemap) and push it to the remote repository.

Open the root directory of the site in the command prompt:

* <figure id="attachment_369" aria-describedby="caption-attachment-369" style="width: 258px" class="wp-caption alignright"><img loading="lazy" class="wp-image-369" src="https://gotask.net/wp-content/uploads/2021/01/hugo-site-build-console.png" alt="Build Hugo site console output" width="268" height="239" /><figcaption id="caption-attachment-369" class="wp-caption-text">Build Hugo site console output</figcaption></figure>
    
To build the site, type:
```
hugo
```
    
* Type the following git command to add the new files to the repository, commit and push.

```
git add .
git commit -m "Added theme"
git push origin
```
        
Note that you need to repeat this process every time you make any change to the site.
        
## Connecting to GitHub Pages

Once we have the repository set, we need to enable the GitHub Pages capability for our repository.
  
<figure id="attachment_355" aria-describedby="caption-attachment-355" style="width: 597px" class="wp-caption alignright"><img loading="lazy" class="size-full wp-image-355" src="https://gotask.net/wp-content/uploads/2020/12/git-set-branch-custom-domain.png" alt="GitHub Pages Settings" width="607" height="285" srcset="https://gotask.net/wp-content/uploads/2020/12/git-set-branch-custom-domain.png 607w, https://gotask.net/wp-content/uploads/2020/12/git-set-branch-custom-domain-300x141.png 300w" sizes="(max-width: 607px) 100vw, 607px" /><figcaption id="caption-attachment-355" class="wp-caption-text">GitHub Pages Settings for Branch and Custom Domain</figcaption></figure>

  1. In your web browser, go the &#8216;Settings&#8217; page of your repository.
  2. At the bottom of the page (make sure you scroll down), you fill find GitHub Pages section.
  3. Choose the &#8216;main&#8217; branch and the &#8216;/docs&#8217; directory.
  4. Click &#8216;Save&#8217;
  5. In the &#8216;Custom Domain&#8217; section, type your domain name (without http/https).
  6. Click &#8216;Save&#8217;
  7. Note that https (ssl) support comes out of the box, but you will need wait up to 24 hours until your certificate is generated. Make sure &#8216;Enforce HTTPS &#8216; option is checked.

### Configuring your DNS

The last but not the least part we need to perform is to setup our domain&#8217;s DNS settings to work with GitHub pages. I registered my domain via NameCheap domain registrar, but this process can be done from any domain registrar.

All we have to do is to setup a few &#8216;A Records&#8217; and &#8216;CNAME record&#8217; to work with GitHub pages.

In your domain management screen, open your Advanced DNS settings screen.

<li style="list-style-type: none;">
  <ol>
    <li>
      Create &#8216;A Record&#8217; for @ host with  185.199.108.153 value.
    </li>
    <li>
      Create &#8216;A Record&#8217; for @ host with  185.199.109.153 value.
    </li>
    <li>
      Create &#8216;A Record&#8217; for @ host with  185.199.110.153 value.
    </li>
    <li>
      Create &#8216;A Record&#8217; for @ host with  185.199.111.153 value.
    </li>
    <li>
      Create &#8216;CNAME Record&#8217; for @ host with <username>.github.io value.
    </li>
  </ol>
</li>

<figure id="attachment_350" aria-describedby="caption-attachment-350" style="width: 715px" class="wp-caption aligncenter"><img loading="lazy" class="size-full wp-image-350" src="https://gotask.net/wp-content/uploads/2020/12/advanced-dns-namecheap-git-hub-1.png" alt="Namecheap Advanced DNS For GitHub Pages" width="725" height="293" srcset="https://gotask.net/wp-content/uploads/2020/12/advanced-dns-namecheap-git-hub-1.png 725w, https://gotask.net/wp-content/uploads/2020/12/advanced-dns-namecheap-git-hub-1-300x121.png 300w" sizes="(max-width: 725px) 100vw, 725px" /><figcaption id="caption-attachment-350" class="wp-caption-text">Namecheap Advanced DNS For GitHub Pages</figcaption></figure>

### Testing the site

Type your domain in the web browser, the site should appear 🙂