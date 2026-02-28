FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["HackatonFiap.Dashboard/HackatonFiap.Dashboard.csproj", "HackatonFiap.Dashboard/"]
COPY ["HackatonFiap.Compartilhado/HackatonFiap.Compartilhado.csproj", "HackatonFiap.Compartilhado/"]
RUN dotnet restore "HackatonFiap.Dashboard/HackatonFiap.Dashboard.csproj"
COPY . .
WORKDIR "/src/HackatonFiap.Dashboard"
RUN dotnet build "HackatonFiap.Dashboard.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HackatonFiap.Dashboard.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackatonFiap.Dashboard.dll"]
