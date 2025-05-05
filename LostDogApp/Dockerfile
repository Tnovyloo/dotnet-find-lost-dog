# Use the SDK image to build your app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy the .csproj file and restore any dependencies (via restore)
COPY LostDogApp/LostDogApp.csproj ./ 
RUN dotnet restore

# Copy the entire application and build it
COPY LostDogApp/. ./LostDogApp/
WORKDIR /src/LostDogApp
RUN dotnet publish -c Release -o /app/publish

# Use the runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

# Copy the build output from the build image
COPY --from=build /app/publish .

# Set the entry point to your app
ENTRYPOINT ["dotnet", "LostDogApp.dll"]
