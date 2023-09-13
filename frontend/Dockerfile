FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src/batch-client
COPY ./batch-client .

WORKDIR /src/web-app
COPY ./web-app/web-app.csproj .
RUN dotnet restore ./web-app.csproj

COPY ./web-app .
RUN dotnet build "web-app.csproj" -c Release -o /app/web-app/build

FROM build AS publish
RUN dotnet publish "web-app.csproj" -c Release -o /app/web-app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
RUN  addgroup --gid 1000  non-root && \
     adduser --gid 1000 --uid 1000 non-root --no-create-home --gecos GECOS --disabled-login

WORKDIR /app/web-app
RUN chown -R non-root:non-root .
#Copy built application to the runtime environment
COPY --from=publish /app/web-app/publish .
#Container running with a created non-root user with ID 1000
USER 1000
EXPOSE 8070
#require env-var ASPNETCORE_URLS=http://+:8070 - set in docker-compose.yaml or in radixconfig.yaml
ENTRYPOINT ["dotnet", "web-app.dll"]
