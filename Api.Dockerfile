FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FamilyBudget.Api/FamilyBudget.Api.csproj", "FamilyBudget.Api/"]
RUN dotnet restore "FamilyBudget.Api/FamilyBudget.Api.csproj"
COPY . .
RUN ls
WORKDIR "/src/FamilyBudget.Api"
RUN dotnet build "FamilyBudget.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FamilyBudget.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FamilyBudget.Api.dll"]