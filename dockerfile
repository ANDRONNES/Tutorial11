# Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#downloading an sdk to build our app
WORKDIR /app
#setting up our directory, in which we will store the whole our app in container

#RESTORE
COPY ["Tutorial11/Tutorial11.csproj", "Tutorial11/"]
#copying the most important file from our project to directory Tutorial11(it will be created automatically) 
#The .csproj files store all our dependencies and libraries that we need
RUN dotnet restore 'Tutorial11/Tutorial11.csproj'
#this command will download all our dependencies and libraries that we need to build our app

#BUILD
COPY ["Tutorial11/", "Tutorial11/"]
#copying all the rest files from our project to directory Tutorial11
WORKDIR /app/Tutorial11
#setting up our directory to Tutorial11, because all the rest commands will be run from this directory
RUN dotnet build 'Tutorial11.csproj' -c Development -o /app/build
#this command will build our app in the configuration Development and put the result in the directory /app/build

# Stage 2: Publish Stage
FROM build as publish 
#we are creating a new stage, which is based on the previous stage build
RUN dotnet publish 'Tutorial11.csproj' -c Development -o /app/publish
#this command will publish our app in the configuration Development and put the result in the directory /app/publish

# Stage 3: Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
#pulling the image which is used to run our app only(it is like lightweight sdk)
ENV ASPNETCORE_URLS=http://+:5001
#telling our app to listen on port 5001
EXPOSE 5001
#sharing the information about our port to other developers
ENV ASPNETCORE_ENVIRONMENT=Development 
# Важно, потому что автоматически, по умолчанию стоит Prodduction, а swagger не работает в продакшн
WORKDIR /app
#setting up our directory to /app
COPY --from=publish /app/publish .
#with this command we are copying all the files from the directory /app/publish of the previous stage publish to the current directory (which is /app) of the current stage
#now we will have only dll files
#important: this stages are running in the docker, not in our container, so we will not have all our previous unnecessary files
ENTRYPOINT ["dotnet", "Tutorial11.dll"]
#this command is starting our app, when the container is run
