FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OffsiteWorker/OffsiteWorker.csproj", "OffsiteWorker/"]
RUN dotnet restore "OffsiteWorker/OffsiteWorker.csproj"
COPY . .
WORKDIR "/src/OffsiteWorker"
RUN dotnet build "OffsiteWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OffsiteWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OffsiteWorker.dll"]
