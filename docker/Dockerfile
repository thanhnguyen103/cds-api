# Use the official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and restore as distinct layers
COPY ./src/CDS-API.Api/CDS-API.Api.csproj ./CDS-API.Api/
COPY ./src/CDS-API.Domain/CDS-API.Domain.csproj ./CDS-API.Domain/
COPY ./src/CDS-API.Application/CDS-API.Application.csproj ./CDS-API.Application/
COPY ./src/CDS-API.Infrastructure/CDS-API.Infrastructure.csproj ./CDS-API.Infrastructure/
RUN dotnet restore ./CDS-API.Api/CDS-API.Api.csproj

# Copy the rest of the source code
COPY ./src/ ./ 

WORKDIR /src/CDS-API.Api
RUN dotnet publish -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (change if your app uses a different port)
EXPOSE 80

# Set environment variables if needed
# ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CDS-API.Api.dll"]