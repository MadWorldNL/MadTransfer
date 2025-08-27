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

## Test guide
Run the tests from the repository root with Docker running:
```bash
dotnet test
```

## Acknowledge
This project leverages several outstanding open-source tools and platforms. We are grateful to the communities behind these technologies for their continuous innovation and support:
- [Cloud Native PG](https://cloudnative-pg.io/)\
A Kubernetes operator for managing PostgreSQL clusters in a cloud-native way. It provides robust automation, high availability, and native integration with Kubernetes environments.
- [Key Cloak](https://www.keycloak.org/)\
  An open-source identity and access management solution, offering single sign-on (SSO), authentication, authorization, and social login capabilities.
- [Kubernetes](https://kubernetes.io/)\
  The industry-standard platform for automating deployment, scaling, and management of containerized applications.
- [OVH Cloud](https://www.ovhcloud.com/)\
  A global cloud provider offering scalable infrastructure, including public and private cloud services, bare metal servers, and hosted Kubernetes, supporting the deployment and operation of modern cloud-native applications.
- [Postgresql](https://www.postgresql.org/)\
  A powerful, open-source object-relational database system known for its reliability, feature richness, and performance.
- [S3 Ninja](https://s3ninja.net/)\
  A lightweight and easy-to-use mock S3 server for testing S3-compatible applications locally, without needing a connection to AWS.