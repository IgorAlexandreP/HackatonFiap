FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["HackatonFiap.Identidade/HackatonFiap.Identidade.csproj", "HackatonFiap.Identidade/"]
COPY ["HackatonFiap.Compartilhado/HackatonFiap.Compartilhado.csproj", "HackatonFiap.Compartilhado/"]
RUN dotnet restore "HackatonFiap.Identidade/HackatonFiap.Identidade.csproj"
COPY . .
WORKDIR "/src/HackatonFiap.Identidade"
RUN dotnet build "HackatonFiap.Identidade.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HackatonFiap.Identidade.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackatonFiap.Identidade.dll"]
