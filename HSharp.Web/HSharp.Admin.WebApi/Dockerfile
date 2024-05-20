FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base 
COPY hsharpapi / 
WORKDIR / 
EXPOSE 9001
ENV ASPNETCORE_URLS=http://*:9001 
CMD ["dotnet", "HSharp.Admin.WebApi.dll", "--server.urls", "http://*:9001"]