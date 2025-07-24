# MadTransfer
This is a lightweight file sharing platform that allows anyone to upload a file and receive a unique download link. You can then share this link with others so they can download the file directly.

## Prerequisite
* Docker Desktop
* Dotnet 9 SDK
* Your favorite IDE (Such as JetBrains Rider)

## Install guide
Run the app using Docker Compose from the root of this repository:
```bash
docker compose --env-file docker-compose.env up --build --force-recreate
```

If you want to log in to the app, follow the instructions in the: [Authentication Server Guide](./docs/authentication-server.md)