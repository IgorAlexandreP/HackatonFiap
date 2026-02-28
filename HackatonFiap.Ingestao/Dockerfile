FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["HackatonFiap.Ingestao/HackatonFiap.Ingestao.csproj", "HackatonFiap.Ingestao/"]
COPY ["HackatonFiap.Compartilhado/HackatonFiap.Compartilhado.csproj", "HackatonFiap.Compartilhado/"]
RUN dotnet restore "HackatonFiap.Ingestao/HackatonFiap.Ingestao.csproj"
COPY . .
WORKDIR "/src/HackatonFiap.Ingestao"
RUN dotnet build "HackatonFiap.Ingestao.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HackatonFiap.Ingestao.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackatonFiap.Ingestao.dll"]
