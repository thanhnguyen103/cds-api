# Setting Up a Mock Azure SQL Edge Database with Podman

## 1. Install Podman

Follow the [official Podman installation guide](https://podman.io/getting-started/installation) for your operating system.

## 2. Pull the Azure SQL Edge Image

Azure SQL Edge is a lightweight SQL engine designed for edge deployments and supports both x86_64 and ARM architectures.

```sh
podman pull mcr.microsoft.com/azure-sql-edge:latest
```

## 3. Start the Azure SQL Edge Container

```sh
podman run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=YourStrong!Passw0rd' \
    -p 1433:1433 --name mock-sqledge -d mcr.microsoft.com/azure-sql-edge:latest
```

- Replace `YourStrong!Passw0rd` with your desired strong password.
- This image is intended for development and testing.

## 4. Wait for the Database to Start

Give the container a minute to initialize.

## 5. Load `script.sql` into the Database

Assuming `script.sql` is in the current directory:

```sh
podman cp script.sql mock-sqledge:/script.sql
podman exec -it mock-sqledge /opt/mssql-tools/bin/sqlcmd \
    -S localhost -U SA -P 'YourStrong!Passw0rd' -i /script.sql
```

- Adjust the username and database name in your SQL script if needed.

## 6. Connect to the Database

You can now connect using any SQL client at `localhost:1433` with the credentials you set above.

## 7. Update the Connection String in Your API Project

Update your API project's configuration to use the mock database. For example, in an environment variable or `appsettings.json`:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=YourDatabase;User Id=SA;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
}
```

- Replace `YourDatabase` with the name of your database if needed.
- Ensure the port, username, and password match your container setup.
- `TrustServerCertificate` is often required for local development.
