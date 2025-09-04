# Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

#RESTORE
COPY ["Tutorial11/Tutorial11.csproj", "Tutorial11/"]
RUN dotnet restore 'Tutorial11/Tutorial11.csproj'


#BUILD
COPY ["Tutorial11/", "Tutorial11/"]
WORKDIR /app/Tutorial11
RUN dotnet build 'Tutorial11.csproj' -c Development -o /app/build
# Stage 2: Publish Stage
FROM build as publish 
RUN dotnet publish 'Tutorial11.csproj' -c Development -o /app/publish
# Stage 3: Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
ENV ASPNETCORE_URLS=http://+:5001
EXPOSE 5001
ENV ASPNETCORE_ENVIRONMENT=Development 
# Важно, потому что автоматически, по умолчанию стоит Prodduction, а swagger не работает в продакшн
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tutorial11.dll"]
