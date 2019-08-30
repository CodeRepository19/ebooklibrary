FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY EbookStore/EbookUI/obj/Release/netcoreapp2.2/PubTmp/Out/*.* ./
ENTRYPOINT ["dotnet", "EbookUI.dll"]
