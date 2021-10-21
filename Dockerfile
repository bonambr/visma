FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ConsoleApp3.csproj", "./"]
RUN dotnet restore "ConsoleApp3.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "ConsoleApp3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp3.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApp3.dll"]
