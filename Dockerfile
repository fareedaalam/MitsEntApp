#Build Stage
# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /source
COPY . .    
# Copy csproj and restore as distinct layers
#COPY API.csproj ./
RUN dotnet restore "./API/API.csproj" --disable-parallel
RUN dotnet publish "./API/API.csproj" --disable-parallel -c release -o /app --no-restore
    

#RUN dotnet publish -c Release -o out

#serve Stage  
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
# Copy everything else and build

WORKDIR /app
COPY --from=build-env /app ./
#COPY --from=build-env /app/out .
EXPOSE 5000
ENTRYPOINT ["dotnet", "API.dll"]



