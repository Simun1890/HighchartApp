# STEP 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Kopiraj .csproj i restore dependencije
COPY *.sln ./
COPY HighchartsApp/*.csproj ./HighchartsApp/
RUN dotnet restore

# Kopiraj ostatak aplikacije
COPY HighchartsApp/. ./HighchartsApp/
WORKDIR /app/HighchartsApp
RUN dotnet publish -c Release -o out

# STEP 2: Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/HighchartsApp/out ./

# Pokreni aplikaciju
ENTRYPOINT ["dotnet", "HighchartsApp.dll"]
