#Build stage

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["blogger-clone.csproj", "./"]
RUN dotnet restore "blogger-clone.csproj"

COPY . .
RUN dotnet publish "blogger-clone.csproj" -c Release -o /app/publish /p:UseAppHost=false


#Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet", "blogger-clone.dll" ]