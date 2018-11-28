FROM plasma999/apidemo
#copy web app source and utility script(s) to image
COPY . /inetpub/wwwroot/APIDemo

#set to base web app dir
WORKDIR /inetpub/wwwroot/APIDemo

#restore packages (which are intentionally not in source control)
#Build (via msbuild)
#Create IIS ASP.NET web application
RUN nuget restore -PackagesDirectory .\packages
RUN powershell /inetpub/wwwroot/web/Run-MsBuild
RUN powershell /inetpub/wwwroot/web/Create-WebApplication -name "APIDemo" -physicalPath "c:\inetpub\wwwroot\APIDemo"