# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["InventoryManagement/InventoryManagement.csproj", "InventoryManagement/"]
RUN dotnet restore "InventoryManagement/InventoryManagement.csproj"
COPY . .
RUN dotnet publish "InventoryManagement/InventoryManagement.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InventoryManagement.dll"]
