# ----- Etapa 1: build -----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY TaskFlow.Api/TaskFlow.Api.csproj TaskFlow.Api/
RUN dotnet restore TaskFlow.Api/TaskFlow.Api.csproj

COPY . .
RUN dotnet publish TaskFlow.Api/TaskFlow.Api.csproj -c Release -o /app /p:UseAppHost=false

# ----- Etapa 2: runtime -----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "TaskFlow.Api.dll"]
