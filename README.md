# SignalR.SharePoint

There are a few steps to enable SignalR into a web application. But how do this for a SharePoint based web application? 

The goal of this project is provide you a SharePoint deployment package to  automate such steps for a SharePoint web application.

The only thing you have to do is:

1) Build this sources or direct download [SignalR.SharePoint.wsp](https://onedrive.live.com/redir?resid=ADEB7E738B27F26C!123&authkey=!AB7lL3MQVas4LNw&ithint=file%2cwsp).

2) Install and deploy SignalR distributed assemblies -in the SignalR.SharePoint.wsp package file- to the web application bin directory from a Powershell console:

    >Add-PSSnapin Microsoft.SharePoint.PowerShell
    >Add-SPSolution (Resolve-Path .\SignalR.SharePoint.wsp)
    >Install-SPSolution SignalR.SharePoint.wsp -WebApplication $WebAppUrl -GACDeployment -FullTrustBinDeployment -Force

3) Enable [SignalR SharePoint Configuration Feature] - in the web application scope - in order to modify the web.config for run-time assembly binding redirection, set [legacyCasModel](http://msdn.microsoft.com/en-us/library/vstudio/tkscy493(v=vs.100).aspx) to false (allow dynamic calls) and set the owin:AutomaticAppStartup application setting key to false

    ...
       <trust level="Full" originUrl="" legacyCasModel="false" />
    ...
      <runtime>
        <assemblyBinding>
    ...
          <dependentAssembly>
            <assemblyIdentity name="Microsoft.Owin.Security" publicKeyToken="31bf3856ad364e35" culture="neutral" />
            <bindingRedirect oldVersion="0.0.0.0-2.1.0.0" newVersion="2.1.0.0" />
          </dependentAssembly>
          <dependentAssembly>
            <assemblyIdentity name="Microsoft.Owin" publicKeyToken="31bf3856ad364e35" culture="neutral" />
            <bindingRedirect oldVersion="0.0.0.0-2.1.0.0" newVersion="2.1.0.0" />
          </dependentAssembly>
          <dependentAssembly>
            <assemblyIdentity name="Newtonsoft.Json" publicKeyToken="30ad4fe6b2a6aeed" culture="neutral" />
            <bindingRedirect oldVersion="0.0.0.0-6.0.0.0" newVersion="6.0.0.0" />
          </dependentAssembly>
        </assemblyBinding>
      </runtime>
    ...
      <appSettings>
    ...
        <add key="owin:AutomaticAppStartup" value="false" />
      </appSettings>

4) Create and deploy your hub based assembly into the web application. The easy way to do this is by writing the hub in a SharePoint based project and set the "Assembly Deployment Target" to WebApplication. You can also try with [this](https://onedrive.live.com/redir?resid=ADEB7E738B27F26C!122&authkey=!ABvr991lcrcDUEQ&ithint=file%2czip) simple chat example by deploying it into your web application.

5) Enable the [SignalR SharePoint Enable AutomaticAppStartup Feature] - in the web application scope - in order turn the owin:AutomaticAppStartup application setting key to true.

      <appSettings>
    ...
        <add key="owin:AutomaticAppStartup" value="true" />
      </appSettings>

6) Just open your browser and navigates to the application page. If you deployed the chat example, you can try with %WebApplicationUrl%/_layouts/15/_layouts/15/SignalR.SharePoint.Demo/Chat.aspx application page.