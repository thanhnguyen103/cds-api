# Keycloak token lifetime

To increase the token lifetime in Keycloak, you can adjust the **Access Token Lifespan** and other related settings in the Keycloak Admin Console. Here's how to do it:

---

### 1. **Access the Keycloak Admin Console**
- Open your browser and navigate to the Keycloak Admin Console (e.g., `http://localhost:8080`).
- Log in with your admin credentials.

---

### 2. **Navigate to the Realm Settings**
- In the left-hand menu, click on **Realm Settings**.
- Go to the **Tokens** tab.

---

### 3. **Adjust Token Lifetimes**
You can configure the following token lifetimes:

- **Access Token Lifespan**:
  - This controls how long an access token is valid.
  - Default: `5 minutes`.
  - Increase it to your desired value (e.g., `30 minutes` or `1 hour`).

- **Refresh Token Lifespan**:
  - This controls how long a refresh token is valid.
  - Default: `30 minutes`.
  - Increase it if you want tokens to be refreshable for a longer period.

- **Client Session Idle**:
  - This controls how long a session remains idle before it is invalidated.
  - Default: `10 minutes`.

- **Client Session Max**:
  - This controls the maximum duration of a session.
  - Default: `60 minutes`.

---

### 4. **Override Token Lifetimes for Specific Clients (Optional)**
If you want to set different token lifetimes for a specific client:

1. Navigate to **Clients** in the left-hand menu.
2. Select the client you want to configure (e.g., `my-backend-service`).
3. Go to the **Advanced Settings** tab.
4. Adjust the following settings:
   - **Access Token Lifespan**: Override the default realm setting for this client.
   - **Refresh Token Lifespan**: Override the default realm setting for this client.

---

### 5. **Save Changes**
After making the changes, click **Save** to apply them.

---

### 6. **Test the New Token Lifetimes**
- Use the `curl` command or Postman to request a new access token.
- Decode the token (e.g., using [jwt.io](https://jwt.io)) and check the `exp` (expiration) claim to verify the new lifetime.

---

### Notes:
- Be cautious when increasing token lifetimes, as longer lifetimes can increase the risk of token misuse if a token is compromised.
- For production environments, consider using shorter token lifetimes with refresh tokens to maintain security.

Let me know if you need further assistance!