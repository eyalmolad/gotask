---
title: How to manually setup MySQL server on Windows
author: eyal
type: post
date: 2020-04-12T19:25:09+00:00
url: /tutorials/how-to-manually-setup-mysql-on-windows/
categories:
  - Tutorials
tags:
  - command-line
  - database
  - my-sql
  - windows-service

---
## Background

<figure id="attachment_198" aria-describedby="caption-attachment-198" style="width: 381px" class="wp-caption alignright"><img loading="lazy" class="size-full wp-image-198" src="https://gotask.net/wp-content/uploads/2020/04/my-sql-hangs.png" alt="MySql installer hangs" width="391" height="146" srcset="https://gotask.net/wp-content/uploads/2020/04/my-sql-hangs.png 391w, https://gotask.net/wp-content/uploads/2020/04/my-sql-hangs-300x112.png 300w" sizes="(max-width: 391px) 100vw, 391px" /><figcaption id="caption-attachment-198" class="wp-caption-text">MySql installer hangs</figcaption></figure>

Recently I tried to install MySQL server on my Windows 10 development machine.

First, I tried the MSI installer which is available from <a href="https://dev.mysql.com/downloads/mysql/5.7.html" target="_blank" rel="noopener noreferrer">MySql Community Downloads</a>

as both 32-bit and 64-bit editions.

Unfortunately, after running the installer, it kept hanging in the middle of the setup as shown in the picture.

I could&#8217;t find the solution for this, so I decided to try to manually setup and configure MySql.

&nbsp;

## My Stack

  * Windows 10 Pro 64-bit (10.0, Build 18363)
  * MySql Server Community Edition 5.7.29

## 

## Downloading the binary zipped version

  1. Go to the MySql developers site and download the latest zipped version  &#8211; (Windows (x86, 64-bit), ZIP Archive) 
    <figure id="attachment_199" aria-describedby="caption-attachment-199" style="width: 464px" class="wp-caption alignright"><img loading="lazy" class=" wp-image-199" src="https://gotask.net/wp-content/uploads/2020/04/my-sql-directory.png" alt="My Sql directory" width="474" height="188" srcset="https://gotask.net/wp-content/uploads/2020/04/my-sql-directory.png 827w, https://gotask.net/wp-content/uploads/2020/04/my-sql-directory-300x119.png 300w, https://gotask.net/wp-content/uploads/2020/04/my-sql-directory-768x305.png 768w" sizes="(max-width: 474px) 100vw, 474px" /><figcaption id="caption-attachment-199" class="wp-caption-text">My Sql directory in Windows Explorer</figcaption></figure>
    
      * In case you have a 32-bit operating system, download Windows (x86, 32-bit), ZIP Archive.
  2. Extract the zip archive into a new folder. In this case, I am extracting to c:\MySql.
  3. Pay attention to extract the files into the main c:\MySql directory as shown in the picture.
  4. Looking at the c:\MySql\bin directory, we can find a few important executables 
      * mysqld.exe &#8211; this is the actual database server that will accept connections from clients. It listens by default for requests from port 3306. (the suffix letter &#8216;d&#8217; is for daemon or service).
      * mysql.exe &#8211; this is the command line client that can be used to view/create/edit the databases and the tables.

&nbsp;

## Initializing the database

Before staring the database building process, we need to open the Windows Command Prompt with the Administrator privileges.

1. First thing we need to do is to initialize the MySql database on our machine. It will create the &#8216;data&#8217; folder and setup the system tables.

2. In the command prompt, change the directory to c:\MySql\bin and type the following command:

  * <code class="EnlighterJSRAW" data-enlighter-language="msdos">mysqld.exe --console --initialize</code>

3. If the command succeeded, the new folder &#8216;data&#8217; should be created at the root directory folder. (c:\MySql\Data).

4. Pay attention to the command prompt as the command provided the default username (root@localhost) and the password. I recommend copying the password to a notepad file.

5. Alternatively, you could use the same initialize command without the <code class="EnlighterJSRAW" data-enlighter-language="msdos">--console</code> switch. In this case, the output will be written to the log file located in the data directory (you\_computer\_name.err).

6. Run the server via command line:

<li style="list-style-type: none;">
  <ul>
    <li>
      <code class="EnlighterJSRAW" data-enlighter-language="msdos">mysqld.exe --console</code>
    </li>
  </ul>
</li>

7. The console window should show an output similar to the one shown in the picture.

<li style="list-style-type: none;">
  <ul>
    <li>
      The server should be listening on port 3306.
    </li>
    <li>
      You should see &#8220;mysqld.exe: ready for connections&#8221;.
    </li>
  </ul>
</li>

Important: Don&#8217;t close this console window as it will close the server and we will not be able to connect with clients.

<figure id="attachment_205" aria-describedby="caption-attachment-205" style="width: 590px" class="wp-caption alignnone"><img loading="lazy" class="wp-image-205 size-full" src="https://gotask.net/wp-content/uploads/2020/04/my-sqld-running-e1586541089671.png" alt="MySql ready for connections" width="600" height="215" /><figcaption id="caption-attachment-205" class="wp-caption-text">MySql server ready for connections</figcaption></figure>

## Connecting to the database

Now that we have the system tables setup and created the root user, we can try connecting to the database.

### Connecting via command line

  1. Open another command line window (don&#8217;t close the mysqld) and change the directory to c:\MySql\bin. 
      * Type <code class="EnlighterJSRAW" data-enlighter-language="msdos">mysql.exe -u root -p</code>
      * Once prompted, type the password you previously saved.
      * You should see mysql command prompt as shown in the picture. 
        <figure id="attachment_204" aria-describedby="caption-attachment-204" style="width: 490px" class="wp-caption alignnone"><img loading="lazy" class="size-full wp-image-204" src="https://gotask.net/wp-content/uploads/2020/04/my-sql-welcome-screen-e1586540996861.png" alt="Mysql welcome screen" width="500" height="308" /><figcaption id="caption-attachment-204" class="wp-caption-text">My sql command line tool welcome screen</figcaption></figure></li> </ul> </li> 
        
          * Now lets try to execute a simple SQL statement like:</ol> 
        
        <li style="list-style-type: none;">
          <ul>
            <li>
              <code class="EnlighterJSRAW" data-enlighter-language="sql">show databases;</code>
            </li>
            <li>
              Here we get a pretty annoying message that we need to change the password before we execute such statements.
            </li>
            <li>
              <em><code class="EnlighterJSRAW" data-enlighter-language="shell">ERROR 1820 (HY000): You must reset your password using ALTER USER statement before executing this statement.</code></em>
            </li>
          </ul>
        </li>
        
        ### Changing the password:
        
          1. In the same command prompt, type (pay attention to the quotes and the semi-colon at the end of the statement): 
              * <code class="EnlighterJSRAW" data-enlighter-language="sql">alter user 'root'@'localhost' identified by '12345';</code>
              * You should get the confirmation message: <code class="EnlighterJSRAW" data-enlighter-language="shell">Query OK, 0 rows affected (0.00 sec)</code>
          2. In order to test the new password, type <code class="EnlighterJSRAW" data-enlighter-language="shell">quit</code> in the mysql prompt to exit to the Windows command prompt. Now repeat the step 1 and login with the new password.
          3. Repeat the step 2 to show the databases. You should get a list of system databases on your server. 
            <figure id="attachment_203" aria-describedby="caption-attachment-203" style="width: 312px" class="wp-caption alignnone"><img loading="lazy" class="size-full wp-image-203" src="https://gotask.net/wp-content/uploads/2020/04/show-databases.png" alt="Show databases on mysql" width="322" height="272" srcset="https://gotask.net/wp-content/uploads/2020/04/show-databases.png 322w, https://gotask.net/wp-content/uploads/2020/04/show-databases-300x253.png 300w" sizes="(max-width: 322px) 100vw, 322px" /><figcaption id="caption-attachment-203" class="wp-caption-text">Show databases on mysql server</figcaption></figure></li> </ol> 
            
            &nbsp;
            
            ## Running the server
            
            Now that we have a database created and the server binaries&#8217; set, we probably want to set the server running in the background when the Windows machine boots.
            
            ### Installing as a Windows Service
            
              1. Quit the mysqld process by pressing CTRL+X in the command prompt window.
              2. In the command line, type: <code class="EnlighterJSRAW" data-enlighter-language="msdos">mysqld.exe --install</code>
              3. You should get a confirmation message: <code class="EnlighterJSRAW" data-enlighter-language="shell">Service successfully installed.</code>
              4. In order to start the service, type: <code class="EnlighterJSRAW" data-enlighter-language="msdos">net start MySql</code>
            
            ### Running as a Console Application
            
            In case you dont want to install MySql as a Windows service, you can run it as a console application in the logon process. Note that MySql will not run until you logon on to the machine.
            
              1. In Windows Files Explorer, type &#8220;C:\Users\%USERNAME%\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup&#8221; in the address bar. This will open the Windows startup folder that includes exe/bat files that will automatically run when the user is logged on.
              2. Create a new bat file, runmysql.bat.
              3. Put the following line in the file: <code class="EnlighterJSRAW" data-enlighter-language="msdos">call c:\MySql\bin\mysqld --console</code>
            
            Note that you can not run both, the server and the console application simultaneously unless you change the MySql listening port.
            
            ## Wrapping up
            
            I found this manual process very easy and intuitive, so I dont see a reason to use the installer for MySql anymore.
            
            In the following posts, I will explain more about MySql configuration, logging, permissions and more.