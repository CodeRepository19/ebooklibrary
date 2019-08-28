#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/aspnetcore-build:2.0

WORKDIR /app

COPY . .

RUN dotnet build -c Release

ENTRYPOINT ["dotnet", "run", "--no-launch-profile" ]
