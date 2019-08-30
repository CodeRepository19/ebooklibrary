# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY EbookStore/EbookUI/obj/Release/netcoreapp2.2/PubTmp/Out/*.* ./
ENTRYPOINT /bin/bash -c "dotnet EbookUI.dll"
