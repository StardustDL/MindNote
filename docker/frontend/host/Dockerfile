FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build
WORKDIR /source

COPY *.sln .
COPY ./src ./src
COPY ./test ./test

RUN dotnet restore

RUN cd ./src/Frontend/Server/MindNote.Frontend.Server && dotnet publish -c Release -o /build

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS runtime
RUN apk update \
        && apk upgrade \
        && apk add --no-cache bash \
        && rm -rf /var/cache/apk/*

WORKDIR /app
COPY --from=build /build ./
COPY ./docker/wait-for-it.sh ./
RUN chmod +x ./wait-for-it.sh

EXPOSE 80/tcp

ENTRYPOINT ["dotnet", "MindNote.Frontend.Server.dll"]