FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FuelPricingLoader/FuelPricingLoader.csproj", "FuelPricingLoader/"]
RUN dotnet restore "FuelPricingLoader/FuelPricingLoader.csproj"
COPY . .
WORKDIR "/src/FuelPricingLoader"
RUN dotnet build "FuelPricingLoader.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FuelPricingLoader.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FuelPricingLoader.dll"]
