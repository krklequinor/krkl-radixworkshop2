FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ./job/job.csproj .
RUN dotnet restore ./job.csproj
COPY . .
WORKDIR /src
RUN dotnet build "job.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "job.csproj" -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app
RUN  addgroup --gid 1000  non-root && \
     adduser --gid 1000 --uid 1000 non-root --no-create-home --gecos GECOS --disabled-login
RUN chown -R non-root:non-root .
USER 1000
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "job.dll"]
