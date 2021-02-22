FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["DynamicsHelper.csproj", "./"]
RUN dotnet restore "DynamicsHelper.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "DynamicsHelper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DynamicsHelper.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DynamicsHelper.dll"]
