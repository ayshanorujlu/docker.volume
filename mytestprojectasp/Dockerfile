#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER app
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src/.
#COPY ["mytestprojectasp/mytestprojectasp.csproj", "mytestprojectasp/"]
#RUN dotnet restore "./mytestprojectasp/mytestprojectasp.csproj"
#COPY . .
#WORKDIR "/src/mytestprojectasp"
#RUN dotnet build "./mytestprojectasp.csproj" -c Release -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./mytestprojectasp.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "mytestprojectasp.dll"]
#


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["mytestprojectasp.csproj", "./"]
RUN dotnet restore "./mytestprojectasp.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "mytestprojectasp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "mytestprojectasp.csproj" -c Release -o /app/publish

# Use the runtime image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mytestprojectasp.dll"]

