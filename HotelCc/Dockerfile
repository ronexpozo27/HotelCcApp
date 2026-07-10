# ---------- ETAPA DE COMPILACIÓN ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build

WORKDIR /src

COPY . .

RUN dotnet restore HotelCc.csproj

RUN dotnet publish HotelCc.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ---------- ETAPA DE EJECUCIÓN ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000

EXPOSE 10000

ENTRYPOINT ["dotnet","HotelCc.dll"]
