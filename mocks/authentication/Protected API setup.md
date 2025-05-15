# Protected API setup

To set up a **protected API** in .NET Core 9 that validates access tokens issued by Keycloak, follow these steps:

---

### 1. **Install Required NuGet Packages**
Add the necessary NuGet packages for JWT authentication:

```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.IdentityModel.Tokens
```

---

### 2. **Update `appsettings.json`**
Add Keycloak configuration to your `appsettings.json` file:

```json
{
  "Keycloak": {
    "Authority": "http://localhost:8080/realms/entra-mock-realm",
    "Audience": "my-protected-api",
    "RequireHttpsMetadata": false
  }
}
```

- **Authority**: The Keycloak realm's issuer URL.
- **Audience**: The client ID of your API (e.g., `my-protected-api`).
- **RequireHttpsMetadata**: Set to `false` for local development if Keycloak is running on HTTP. For production, use HTTPS and set this to `true`.

---

### 3. **Configure Authentication in `Program.cs`**
Update your `Program.cs` file to configure JWT authentication:

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure JWT authentication
var keycloakConfig = builder.Configuration.GetSection("Keycloak");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = keycloakConfig["Authority"];
        options.Audience = keycloakConfig["Audience"];
        options.RequireHttpsMetadata = bool.Parse(keycloakConfig["RequireHttpsMetadata"] ?? "true");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = keycloakConfig["Authority"],
            ValidateAudience = true,
            ValidAudience = keycloakConfig["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // Adjust if needed
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();  // Add authorization middleware

app.MapControllers();

app.Run();
```

---

### 4. **Protect API Endpoints**
Use the `[Authorize]` attribute to protect your API endpoints. For example:

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SampleController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult PublicEndpoint()
    {
        return Ok("This is a public endpoint.");
    }

    [Authorize]
    [HttpGet("protected")]
    public IActionResult ProtectedEndpoint()
    {
        return Ok("This is a protected endpoint.");
    }
}
```

- The `PublicEndpoint` is accessible to everyone.
- The `ProtectedEndpoint` requires a valid JWT token.

---

### 5. **Test the API**
1. **Obtain an Access Token**:
   Use `curl` or Postman to request an access token from Keycloak using the **Client Credentials Grant Flow**:

   ```bash
   curl -X POST \
     http://localhost:8080/realms/entra-mock-realm/protocol/openid-connect/token \
     -H "Content-Type: application/x-www-form-urlencoded" \
     -d "grant_type=client_credentials" \
     -d "client_id=my-backend-service" \
     -d "client_secret=YOUR_CLIENT_SECRET"
   ```

   The response will include an `access_token`.

2. **Call the Protected Endpoint**:
   Use the token to call the protected API endpoint:

   ```bash
   curl -X GET \
     http://localhost:5000/api/sample/protected \
     -H "Authorization: Bearer <access_token>"
   ```

   Replace `<access_token>` with the token obtained in the previous step.

---

### 6. **Validate the Token**
The middleware automatically validates:
- **Signature**: Using Keycloak's public keys (retrieved from the `.well-known/openid-configuration` endpoint).
- **Issuer**: Matches the `Authority` value.
- **Audience**: Matches the `Audience` value.
- **Expiration**: Ensures the token is not expired.

---

### Notes:
- Ensure the `Audience` in your `appsettings.json` matches the client ID of your API in Keycloak.
- For production, always use HTTPS and set `RequireHttpsMetadata` to `true`.
- If you need to validate custom claims (e.g., `roles`), you can extend the `TokenValidationParameters`.

This setup ensures your API is protected and validates incoming requests using Keycloak-issued tokens. Let me know if you need further assistance!