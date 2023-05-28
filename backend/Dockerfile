FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY MediCaseAPI/MediCaseAPI.csproj MediCaseAPI/
RUN dotnet restore MediCaseAPI/MediCaseAPI.csproj
COPY MediCaseAPI/ MediCaseAPI/
RUN dotnet publish MediCaseAPI/MediCaseAPI.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "MediCaseAPI.dll"]
