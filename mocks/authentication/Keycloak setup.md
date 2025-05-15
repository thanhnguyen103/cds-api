# Keycloak setup

This document provides step-by-step instructions for setting up a local Keycloak instance (using Podman) to mock Azure Entra ID's Client Credentials Grant flow. It is intended for developers who need to simulate authentication and authorization scenarios similar to Azure Entra ID, enabling local development and testing of services that rely on OAuth2 and OpenID Connect flows.

**Prerequisites:**

* Podman installed on your system.
* `curl` for testing token acquisition.
* A web browser to access the Keycloak Admin Console.

**Step 1: Start Keycloak in Podman**

First, it's good practice to create a dedicated network for your Keycloak container.

```bash
podman network create keycloak-net
```

Now, run the Keycloak container. For development/mocking purposes, you can use `start-dev` mode which uses an in-memory database. Replace `admin` and `password` with your desired initial admin credentials.

```bash
podman run -d --name keycloak-entra-mock \
  --network keycloak-net \
  -p 8080:8080 \
  -e KEYCLOAK_ADMIN=admin \
  -e KEYCLOAK_ADMIN_PASSWORD=password \
  quay.io/keycloak/keycloak \
  start-dev
```

* `-d`: Run in detached mode.
* `--name keycloak-entra-mock`: Assigns a name to your container.
* `--network keycloak-net`: Connects the container to the created network.
* `-p 8080:8080`: Maps port 8080 on your host to port 8080 in the container.
* `-e KEYCLOAK_ADMIN=admin -e KEYCLOAK_ADMIN_PASSWORD=password`: Sets the initial admin username and password.
* `quay.io/keycloak/keycloak`: The Keycloak image.
* `start-dev`: Starts Keycloak in development mode.

**Step 2: Access Keycloak Admin Console**

Once the container is running (give it a minute to initialize), open your web browser and navigate to:
`http://localhost:8080`

Log in using the admin credentials you set (e.g., `admin`/`password`).

**Step 3: Create a New Realm**

Keycloak uses realms to isolate sets of users, credentials, roles, and clients. It's best to create a new realm for your mock setup rather than using the default `master` realm.

1.  Hover over "Master" in the top-left corner and click "Create Realm".
2.  **Realm name**: Enter a name, for example, `entra-mock-realm`.
3.  Click "Create".
4.  You will be automatically switched to the new realm.

**Step 4: Create a Client for Your Service (The Authenticating Application)**

This client represents the service that will request an access token using its credentials.

1.  In your `entra-mock-realm`, navigate to "Clients" from the left-hand menu.
2.  Click "Create client".
3.  **General Settings**:
    * **Client type**: Keep `OpenID Connect`.
    * **Client ID**: Enter a unique ID for your service, e.g., `my-backend-service`.
    * Click "Next".
4.  **Capability config**:
    * **Client authentication**: Set to `On`.
    * **Authorization**: You can leave this `Off` if you are only using client credentials for authentication and not Keycloak's fine-grained authorization policies for this client directly.
    * **Authentication flow**: Ensure "Standard flow" and "Direct access grants" are checked. While "Client credentials" is a distinct grant type, these settings often enable the necessary infrastructure.
    * Click "Next".
5.  **Login settings**:
    * **Root URL**: Can be left blank or set to a placeholder like `http://localhost/myservice`.
    * **Valid redirect URIs**: For client credentials, this is often not strictly necessary but Keycloak might require a value. `http://localhost/myservice/*` can be used.
    * **Web origins**: Can be left as `+` or set specifically if you have CORS considerations.
    * Click "Save".

**Step 5: Configure the Service Client for Client Credentials Flow**

1.  After saving, you'll be on the `my-backend-service` client's configuration page.
2.  Go to the **Settings** tab.
3.  **Access settings**:
    * **Client authentication**: Should be `On`.
    * **Service accounts roles**: Enable this by toggling it to `On`. This is crucial for Client Credentials flow as it creates a service account for this client.
    * **Authorization**: Can remain `Off` unless you plan to use Keycloak's specific authorization services for this client.
    * Ensure **Grant types** includes `Client credentials`. If it's not explicitly listed or configurable here, it's typically enabled by the "confidential" access type (which is implied by enabling client authentication) and "Service accounts roles". Some Keycloak versions make this more explicit.
4.  Go to the **Credentials** tab.
    * **Client Authenticator**: Should be `Client ID and secret`.
    * Copy the **Client secret** displayed. You will need this for your service to authenticate.
    * Click "Save" if you made any changes.

**Step 6: (Optional but Recommended) Create a Client for Your API Resource**

To better mock Azure Entra ID, where tokens are typically issued *for* a specific API (resource), you can create another client to represent that API. The `aud` (audience) claim in the token will then refer to this API.

1.  In your `entra-mock-realm`, navigate to "Clients" and click "Create client".
2.  **General Settings**:
    * **Client type**: `OpenID Connect`.
    * **Client ID**: e.g., `my-protected-api`. This will be your audience.
    * Click "Next".
3.  **Capability config**:
    * **Client authentication**: Can be `Off` if this client only represents a resource and doesn't authenticate itself.
    * Click "Next", then "Save".

**Step 7: Define Roles (Mocking Azure AD App Roles)**

In Azure AD, applications (APIs) expose "App Roles" which are permissions (e.g., `User.Read`, `Data.Write`). Clients are then granted these roles. You can mock this in Keycloak using client roles on your API client.

1.  Navigate to "Clients" and select your API client (e.g., `my-protected-api`).
2.  Go to the "Roles" tab.
3.  Click "Create role".
    * **Role name**: e.g., `Api.Read`. Click "Save".
4.  Create another role:
    * **Role name**: e.g., `Api.Write`. Click "Save".

**Step 8: Assign Roles to the Service Client's Service Account**

The service account of your `my-backend-service` client needs to be granted the roles defined on `my-protected-api`.

1.  Navigate to "Clients" and select your service client (e.g., `my-backend-service`).
2.  Go to the "Service account roles" tab.
3.  Click "Assign role".
4.  In the filter, you might see "Filter by realm roles". Change this by selecting "Filter by clients".
5.  A "Client roles" dropdown will appear. Select your API client (e.g., `my-protected-api`).
6.  The roles you created (e.g., `Api.Read`, `Api.Write`) should appear. Select them and click "Assign".

**Step 9: Add Mappers to Customize Token Claims (To Mimic Azure AD)**

To make Keycloak's tokens look more like Azure Entra ID tokens, you'll want to add/customize claims like `roles` and `aud`. Configure these mappers on your service client (`my-backend-service`).

1.  Navigate to "Clients" and select `my-backend-service`.
2.  Go to the "Client scopes" tab. Look for a dedicated scope like `my-backend-service-dedicated` or similar. Click on it. (Alternatively, some mappers are added directly under the "Mappers" tab of the client itself if not using dedicated client scopes for this). For simplicity, let's assume adding to a dedicated scope or the client's mappers directly.
3.  Go to the "Mappers" tab within that scope (or the client's main "Mappers" tab).
    * **To add the `roles` claim:**
        1.  Click "Add mapper" -> "By configuration" -> "User Client Roles" (if you defined roles on the API client as suggested).
        2.  **Name**: e.g., `api-roles`.
        3.  **Client ID**: Select your API client (e.g., `my-protected-api`).
        4.  **Token Claim Name**: `roles` (this is a common claim name in Azure AD for app roles).
        5.  **Claim JSON Type**: `String` (Keycloak will handle making it an array if "Multivalued" is on).
        6.  **Multivalued**: `On`.
        7.  **Add to access token**: `On`.
        8.  Click "Save".

    * **To add/confirm the `aud` (audience) claim:**
        By default, for client credentials, the audience might be the client_id itself. To explicitly set it to your API client:
        1.  Click "Add mapper" -> "By configuration" -> "Audience".
        2.  **Name**: e.g., `api-audience`.
        3.  **Included Client Audience**: Select `my-protected-api` (your API client ID).
        4.  **Add to access token**: `On`.
        5.  Click "Save".

    * **(Optional) To mimic `appid` claim (Application ID of the client):**
        Azure AD's `appid` claim typically refers to the client ID of the application making the request. Keycloak's access token for client credentials will have an `azp` (Authorized Party) or `cid` (Client ID) claim containing this value (`my-backend-service`). If you strictly need an `appid` claim:
        1.  Click "Add mapper" -> "By configuration" -> "Hardcoded claim".
        2.  **Name**: `appid-claim`.
        3.  **Token Claim Name**: `appid`.
        4.  **Claim value**: `${client_id}` (This uses a Keycloak variable to insert the client ID of `my-backend-service`. Note: Direct variable substitution like this might vary by Keycloak version or require a "Script Mapper" for more complex logic if `${client_id}` isn't directly supported in "Hardcoded claim". A simpler way if `${client_id}` doesn't work is to hardcode the actual client ID, or use a "User Property" mapper if you can set an attribute on the service account user that holds the client ID, mapping `azp` or `cid` using a Script Mapper is also an option).
        5.  **Claim JSON Type**: `String`.
        6.  **Add to access token**: `On`.
        7.  Click "Save".
        *Alternatively, and often simpler, your services can be adapted to read the `azp` or `cid` claim from the Keycloak token instead of `appid`.*

**Step 10: Request the Access Token using `curl`**

Now, use `curl` to request an access token using the Client Credentials Grant flow. Replace `YOUR_CLIENT_SECRET` with the secret you copied in Step 5.

```bash
curl -X POST \
  http://localhost:8080/realms/entra-mock-realm/protocol/openid-connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=client_credentials" \
  -d "client_id=my-backend-service" \
  -d "client_secret=YOUR_CLIENT_SECRET" \
  -d "scope=openid" # Optional: you can also request specific scopes if defined and needed
```

If you configured the audience mapper for `my-protected-api` and want to explicitly request it (though the mapper should add it by default if configured as "Included Client Audience"):
```bash
curl -X POST \
  http://localhost:8080/realms/entra-mock-realm/protocol/openid-connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=client_credentials" \
  -d "client_id=my-backend-service" \
  -d "client_secret=YOUR_CLIENT_SECRET" \
  -d "audience=my-protected-api" \ # Explicitly requesting audience
  -d "scope=openid"
```

You should receive a JSON response containing an `access_token`.

**Step 11: Validate the Token (Conceptual)**

Your resource server (the API that `my-backend-service` wants to call) would then need to:
1.  Receive the access token as a Bearer token in the `Authorization` header.
2.  Validate the token's signature using Keycloak's public keys (found at `http://localhost:8080/realms/entra-mock-realm/protocol/openid-connect/certs`).
3.  Validate the issuer (`iss` should be `http://localhost:8080/realms/entra-mock-realm`).
4.  Validate the audience (`aud` should contain `my-protected-api` or the identifier of your resource server).
5.  Check the `roles` or `scp` claims for necessary permissions.
6.  Ensure the token is not expired (`exp` claim).

Many standard JWT libraries can help with this validation.

This setup provides a functional mock of Azure Entra ID's Client Credentials Grant flow using Keycloak running in Podman. Remember that while you can mimic claims, the issuer and some structural details of the token will be Keycloak-specific, so your consuming services must be configured to trust and understand tokens from your Keycloak instance.