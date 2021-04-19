# Stage 1 : Define the base image
# This will setup the image that will be used for production(aliased as "base").
FROM mcr.microsoft.com/azure-functions/dotnet:3.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

# Stage 2: Build and publish the code
# Uses an sdk image(aliased as "build"), copies our project code into a working directory,
# restores Nuget packages, builds the code and publishes it to a directory names publish.
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Amdocs.Teams.App.Communicator.Send.Func/Amdocs.Teams.App.Communicator.Send.Func.csproj", "Amdocs.Teams.App.Communicator.Send.Func/"]
COPY ["Amdocs.Teams.App.Communicator.Common/Amdocs.Teams.App.Communicator.Common.csproj", "Amdocs.Teams.App.Communicator.Common/"]
RUN dotnet restore "Amdocs.Teams.App.Communicator.Send.Func/Amdocs.Teams.App.Communicator.Send.Func.csproj"
COPY . .
WORKDIR "/src/Amdocs.Teams.App.Communicator.Send.Func"
RUN dotnet build "Amdocs.Teams.App.Communicator.Send.Func.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Amdocs.Teams.App.Communicator.Send.Func.csproj" -c Release -o /app/publish

# Stage 3: Build and publish the code
# This copies the publish directory into production image's working directory.
FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true