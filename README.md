# PiSpy
A cloud-based policy engine for the Internet of Things.

## Deployment Instructions

### Brief Overview of Server Code Structure

The server code is a solution which contains 3 projects: PiSpy.Server, WebRole1, and WorkerRole1. PiSpy.Server is simply an Azure project which coordinates the other two. WebRole1 is the actual website. WorkerRole1 is the “back-end” code which is running all the time. It is also the code that listens on TCP ports and adds blobs and database records.

By deploying the application, it should set up your TCP endpoints for you. However, if there are any issues, the server needs to have TCP ports 27007 and 27008 open for the project to work correctly. Port 27007 collects basic sensor data, port 27008 collects video streams. They can be manually opened in Cloud Services -> Your Cloud Service Name -> Endpoints.


### To Deploy the Server Code

- Open PiSpy.Server.sln through Visual Studio (note that the Azure extension must be installed, look here: https://azure.microsoft.com/en-us/downloads/)
- Create a cloud service, database, and blob storage account on the online Azure portal
- Create a SendGrid account on the online Azure Portal
- Insert proper connection strings and URLs into Web.config, PolicyEngine.cs, BlobHelper.cs, and SqlHelper.cs. You can view these connection strings on your Blob storage dashboard or database dashboard on the online Azure portal. The URL in PolicyEngine.cs should be the URL you are deploying the website to. This URL is hit at a regular interval to prevent the website from falling asleep, which would stop the policy engine from sending notifications.
- In EmailInfo.cs, replace the SendGrid credentials with your own credentials (found in the SendGrid dashboard on the online Azure portal). You will also want to change the MailAddress to be “pispy@your-url.cloudapp.net”.
- In Visual Studio, navigate to View, then click Package Manager Console. In the Package Manager Console, type “update-database” to initialize the database with the required tables (this uses Entity Framework Code First migrations). Hit enter to run the command to prepare the tables for first use.
- Right click on PiSpy.Server in the Solution Explorer, click Publish, log in with your Azure account, and follow the prompts. The first publish could take up to 20 minutes. If there are no further changes to WorkerRole1, WebRole1 can be published by itself in a matter of seconds. For whatever reason, publishing worker roles through Azure takes a very long time.
- Much of the data in the dropdown lists is populated from the database tables, so you will have to fill them out as necessary.

### To Deploy the Client Code

- Install OS on Raspberry Pi (We tested with Raspbian and Arch)
- Copy code for the client to the Raspberry Pi
- Make a FIFO located in /var/pispy/camera/data
- Install build necessities from build
- Unpack and install drivers from the drivers directory
- Compile microPhone.c and DHT.c
- Rename the compiled output from DHT.c to humiture
- Move humiture to /usr/bin
- Run the stream.py as root
- Run cloud_client.py
