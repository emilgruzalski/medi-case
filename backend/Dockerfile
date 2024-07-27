FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY MediCaseAPI/MediCaseAPI.csproj MediCaseAPI/
RUN dotnet restore MediCaseAPI/MediCaseAPI.csproj
COPY MediCaseAPI/ MediCaseAPI/
RUN dotnet publish MediCaseAPI/MediCaseAPI.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
# .NET 8 images listen on port 8080 and provide a non-root user.
# The files directory is created here, so a mounted volume is writable for that user.
RUN mkdir -p /home/app/files && chown $APP_UID /home/app/files
ENV FileStorage__RootPath=/home/app/files
USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "MediCaseAPI.dll"]
