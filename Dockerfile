

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["Raffle.WebApi/Raffle.WebApi.csproj", "Raffle.WebApi/"]
COPY ["Raffle.Aplication/Raffle.Aplication.csproj", "Raffle.Aplication/"]
COPY ["Raffle.Domain/Raffle.Domain.csproj", "Raffle.Domain/"]
COPY ["Raffle.Infrastructure.Data/Raffle.Infrastructure.Data.csproj", "Raffle.Infrastructure.Data/"]
COPY ["Raffle.Infrastructure.IoC/Raffle.Infrastructure.IoC.csproj", "Raffle.Infrastructure.IoC/"]


RUN dotnet restore "Raffle.WebApi/Raffle.WebApi.csproj"


COPY . .
WORKDIR "/src/Raffle.WebApi"
RUN dotnet build "Raffle.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Raffle.WebApi.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 5000

COPY --from=publish /app/publish .

# Faltou criar o uploads
RUN mkdir -p /app/uploads
# Porta 
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "Raffle.WebApi.dll"]