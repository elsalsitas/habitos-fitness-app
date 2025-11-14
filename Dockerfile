FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
<<<<<<< HEAD
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_PRINT_TELEMETRY_MESSAGE=false

# Mejor logging
ENV Logging__Console__FormatterName=Simple

=======
>>>>>>> b3dc4eb5a80c843991550a63cfe6cb986971b2e9
ENTRYPOINT ["dotnet", "MongoDBReports.dll"]
