# Authentication Server
This folder sets up a local authentication server using **Keycloak** for development and testing purposes.

## KeyCloak (Dev Environment)
### Getting Started
Start the development environment using Docker Compose from the root folder

Once running, access Keycloak Admin UI at:
➡️ http://localhost:5555
Login with:
* Username: admin
* Password: admin

To configure a web client and an API client, follow the official guide:
[Keycloak - Getting Started with Docker](https://www.keycloak.org/getting-started/getting-started-docker)

### Add Audience Attribute (for JWT validation)

To include the correct `aud` (audience) claim in your tokens:

1. Go to **Client Scopes** in the Keycloak Admin UI.
2. Click **Create** to add a new client scope (e.g., `audience-api`).
3. Navigate to the **Mappers** tab of the client scope.
    - Click **Create**.
    - Set **Mapper Type** to `Audience`.
    - Set **Included Client Audience** to your target client ID (e.g., `api-client`).
    - Enable **Add to access token** and **Add to ID token**.
    - Click **Save**.
4. Go to **Clients**, select the client that should include this audience.
5. Open the **Client Scopes** tab.
6. Add your new client scope as either **Default** or **Optional**.

> 💡 This ensures the `aud` field in the JWT token includes the specified client audience (e.g., `api-client`), which is commonly required for API-side JWT validation.

## KeyCloak (Production Environment)
*TODO*