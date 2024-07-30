
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
COPY ["src/TorinoRestaurant.API/TorinoRestaurant.API.csproj", "src/TorinoRestaurant.API/"]
COPY ["src/TorinoRestaurant.Application/TorinoRestaurant.Application.csproj", "src/TorinoRestaurant.Application/"]
COPY ["src/TorinoRestaurant.Core/TorinoRestaurant.Core.csproj", "src/TorinoRestaurant.Core/"]
COPY ["src/TorinoRestaurant.Hosting/TorinoRestaurant.Hosting.csproj", "src/TorinoRestaurant.Hosting/"]
COPY ["src/TorinoRestaurant.Infrastructure/TorinoRestaurant.Infrastructure.csproj", "src/TorinoRestaurant.Infrastructure/"]
RUN dotnet restore "src/TorinoRestaurant.API/TorinoRestaurant.API.csproj"

# FROM build AS development
# WORKDIR /app/src/API
# RUN dotnet dev-certs https
# RUN dotnet tool install --global dotnet-ef
# ENV PATH="${PATH}:/root/.dotnet/tools/"
# HEALTHCHECK --interval=5s --timeout=10s --retries=5 --start-period=80s CMD curl --fail http://localhost/health
# CMD ["dotnet", "watch", "--no-hot-reload", "run", "--non-interactive"]

FROM build AS publish
COPY . .
WORKDIR /src/TorinoRestaurant.API
RUN dotnet publish "TorinoRestaurant.API.csproj" -c Release -o /src/TorinoRestaurant.API/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /src/TorinoRestaurant.API
COPY --from=publish /src/TorinoRestaurant.API/publish .
CMD [ "ls" ]
ENTRYPOINT ["dotnet", "TorinoRestaurant.API.dll"]
