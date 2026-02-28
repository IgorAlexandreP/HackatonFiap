FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["HackatonFiap.Analise/HackatonFiap.Analise.csproj", "HackatonFiap.Analise/"]
COPY ["HackatonFiap.Compartilhado/HackatonFiap.Compartilhado.csproj", "HackatonFiap.Compartilhado/"]
RUN dotnet restore "HackatonFiap.Analise/HackatonFiap.Analise.csproj"
COPY . .
WORKDIR "/src/HackatonFiap.Analise"
RUN dotnet build "HackatonFiap.Analise.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HackatonFiap.Analise.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackatonFiap.Analise.dll"]
