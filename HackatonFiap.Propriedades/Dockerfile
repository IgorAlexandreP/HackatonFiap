FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["HackatonFiap.Propriedades/HackatonFiap.Propriedades.csproj", "HackatonFiap.Propriedades/"]
COPY ["HackatonFiap.Compartilhado/HackatonFiap.Compartilhado.csproj", "HackatonFiap.Compartilhado/"]
RUN dotnet restore "HackatonFiap.Propriedades/HackatonFiap.Propriedades.csproj"
COPY . .
WORKDIR "/src/HackatonFiap.Propriedades"
RUN dotnet build "HackatonFiap.Propriedades.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HackatonFiap.Propriedades.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackatonFiap.Propriedades.dll"]
