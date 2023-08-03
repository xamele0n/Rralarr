FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Rrarr/Rrarr.csproj", "Rrarr/"]
RUN dotnet restore "src/Rrarr/Rrarr.csproj"
COPY . .
WORKDIR "/src/Rrarr"
RUN dotnet build "Rrarr.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rrarr.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rrarr.dll"]
