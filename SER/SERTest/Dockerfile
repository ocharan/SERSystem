FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SERTest/SERTest.csproj", "SERTest/"]
RUN dotnet restore "SERTest/SERTest.csproj"
COPY . .
WORKDIR "/src/SERTest"
RUN dotnet build "SERTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SERTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SERTest.dll"]
