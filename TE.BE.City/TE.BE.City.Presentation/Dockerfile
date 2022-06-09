#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS build
WORKDIR /src
COPY ["TE.BE.City.Presentation/TE.BE.City.Api.csproj", "TE.BE.City.Presentation/"]
COPY ["TE.BE.City.Service/TE.BE.City.Service.csproj", "TE.BE.City.Service/"]
COPY ["TE.BE.City.Domain/TE.BE.City.Domain.csproj", "TE.BE.City.Domain/"]
COPY ["TE.BE.City.Infra.CrossCutting/TE.BE.City.Infra.CrossCutting.csproj", "TE.BE.City.Infra.CrossCutting/"]
COPY ["TE.BE.City.Infra.Data/TE.BE.City.Infra.Data.csproj", "TE.BE.City.Infra.Data/"]
RUN dotnet restore "TE.BE.City.Presentation/TE.BE.City.Api.csproj"
COPY . .
WORKDIR "/src/TE.BE.City.Presentation"
RUN dotnet build "TE.BE.City.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TE.BE.City.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TE.BE.City.Api.dll"]