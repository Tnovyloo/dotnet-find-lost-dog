# Use the .NET SDK image for development
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development

# Install EF Core tools globally
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Set working directory
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY LostDogApp/LostDogApp.csproj ./LostDogApp/
RUN dotnet restore LostDogApp/LostDogApp.csproj

# Copy the entire application
COPY LostDogApp/. ./LostDogApp/

# Set working directory to the project
WORKDIR /src/LostDogApp

# Expose port for the ASP.NET Core app
EXPOSE 8080

# Run the app with watch for hot reloading
# ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:8080"]