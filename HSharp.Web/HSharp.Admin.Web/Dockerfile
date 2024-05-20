FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base 
COPY hsharp / 
WORKDIR / 
EXPOSE 9000
ENV ASPNETCORE_URLS=http://*:9000 
CMD ["dotnet", "HSharp.Admin.Web.dll", "--server.urls", "http://*:9000"]