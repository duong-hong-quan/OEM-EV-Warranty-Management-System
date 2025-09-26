# Dockerfile for Backend API
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["BE.API/BE.API.csproj", "BE.API/"]
COPY ["BE.Services/BE.Services.csproj", "BE.Services/"]
COPY ["BE.DAL/BE.DAL.csproj", "BE.DAL/"]
COPY ["BE.Common/BE.Common.csproj", "BE.Common/"]

# Restore dependencies
RUN dotnet restore "BE.API/BE.API.csproj"

# Copy source code
COPY . .

# Build
WORKDIR "/src/BE.API"
RUN dotnet build "BE.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "BE.API.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install Entity Framework tools for migrations
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["dotnet", "BE.API.dll"]
