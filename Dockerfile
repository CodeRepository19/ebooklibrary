FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app
RUN EbookStore//EbookUI//EbookUI.csproj new razor
RUN EbookStore//EbookUI//EbookUI.csproj publish -c Release -o out
FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine
WORKDIR /app
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "EbookUI.dll"]
