# Overall Architecture

``` mermaid
graph TD
    subgraph "Client Tier"
        ClientApp1["Client Application 1 (e.g., Web App)"]
        ClientApp2["Client Application 2 (e.g., Mobile App)"]
    end

    subgraph "Security Tier"
        EntraID["Azure Entra ID"]
    end

    subgraph "Application Tier (Azure)"
        APIManagement["Azure API Management (Optional)"]
        AppService[".NET 9 API on Azure App Service"] -- Logs to --> AppInsights
        AppInsights["Azure Application Insights"]
        AppService -- Uses Managed Identity --> SQLDB
    end

    subgraph "Data Tier (Azure)"
        SQLDB["Azure SQL Database (Read-Only Access)"]
    end

    ClientApp1 -- 1.Requests Token --> EntraID
    ClientApp2 -- 1.Requests Token --> EntraID
    EntraID -- 2.Issues JWT Token --> ClientApp1
    EntraID -- 2.Issues JWT Token --> ClientApp2

    ClientApp1 -- 3.Calls API with Token --> APIManagement
    ClientApp2 -- 3.Calls API with Token --> APIManagement
    APIManagement -- 4.Forwards Validated Request --> AppService
    ClientApp1 -- 3a.Calls API directly with Token (if APIM not used) --> AppService
    ClientApp2 -- 3a.Calls API directly with Token (if APIM not used) --> AppService

    AppService -- Authenticates/Authorizes via --> EntraID
    AppService -- Generates OpenAPI Spec --> SwaggerUI["OpenAPI (Swagger) UI"]
    SwaggerUI -- Consumed by --> Developers["Developers/Consumers"]

    AppService -- Reads Data --> SQLDB

    %% Notes %%
    %% note for EntraID "Handles user/application authentication and issues access tokens."
    %% note for AppService "Hosts the .NET 9 API. Validates tokens. Implements business logic for read operations. %% Integrates OpenAPI and App Insights."
    %% note for SQLDB "Stores application data. API has read-only permissions via Managed Identity."
    %% note for AppInsights "Collects telemetry, logs, exceptions for monitoring and diagnostics."
    %% note for APIManagement "Optional: Provides advanced API governance, security, and developer portal."
    %% note for SwaggerUI "Interactive API documentation generated from code."
```
