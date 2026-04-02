# LBProject - Breach Data API

A high-performance **.NET 10 Web API** designed to aggregate and serve "Have I Been Pwned" breach data. This project implements a local **SQL Server caching layer** to ensure low latency and high availability by reducing direct calls to external APIs.

---

## 🚀 Getting Started

### Prerequisites
* **Runtime:** [.NET 10 SDK](https://dotnet.microsoft.com/download)
* **Database:** SQL Server (Express, LocalDB, or Standard)
* **Utilities:** `sqlcmd` (for database scripts) and `PowerShell` (for IIS setup)

---

## 🛠️ Database Setup

The API requires a local SQL Server instance. Follow these steps to initialize the `PwnedDB` database and its schema.

### 1. Create the Database
```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "CREATE DATABASE PwnedDB"
```

### 2. Run the Schema Script
Initialize the tables and relationships:
```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -d PwnedDB -i "Infrastructure\schema.sql"
```

---

## 🏗️ Build and Local Run

For local development and testing, use the .NET CLI:

### 1. Restore & Build
```bash
# Restore NuGet packages
dotnet restore

# Build the project in Release mode
dotnet build --configuration Release
```

### 2. Run the Application
```bash
# Start the API (Default: http://localhost:5018)
dotnet run --configuration Release
```

---

## 🌐 IIS Deployment

To host the API on Windows Server via **Internet Information Services (IIS)**, follow these steps.

### 1. Install Hosting Bundle
Ensure the **.NET 10 Hosting Bundle** is installed on the server to allow IIS to proxy requests to the Kestrel server.

### 2. Publish the Project
Generate the deployment-ready files:
```bash
dotnet publish -c Release -o C:\inetpub\wwwroot\LBProject
```

### 3. Configure IIS (PowerShell)
Run the following script as **Administrator** to automate the site creation:

```powershell
# 1. Ensure the module is loaded with Admin rights
Import-Module WebAdministration

$siteName = "BreachDataAPI"
$path = "C:\inetpub\wwwroot\LBProject"
$port = 5018

# 2. Check if path exists, create if not (failsafe)
if (!(Test-Path $path)) { New-Item -ItemType Directory -Path $path }

# 3. Create the Application Pool
if (!(Test-Path "IIS:\AppPools\$siteName")) {
    New-WebAppPool -Name $siteName
    Set-ItemProperty "IIS:\AppPools\$siteName" -Name "managedRuntimeVersion" -Value ""
}

# 4. Create the Website
if (!(Test-Path "IIS:\Sites\$siteName")) {
    New-WebSite -Name $siteName `
                -Port $port `
                -PhysicalPath $path `
                -ApplicationPool $siteName
    Write-Host "Deployment complete! API is running on http://localhost:$port" -ForegroundColor Green
} else {
    Write-Warning "Site $siteName already exists."
}
```

---

## 📑 API Reference

| Endpoint | Method | Description |
| :--- | :--- | :--- |
| `/api/breaches` | `GET` | Fetches all breaches from the local cache or external provider. |
| `/api/breaches/{id}` | `GET` | Retrieves detailed information for a specific breach ID. |

### Sample Response
```json
{
  "id": "Adobe",
  "title": "Adobe",
  "domain": "adobe.com",
  "breachDate": "2013-10-04T00:00:00",
  "addedDate": "2013-12-04T00:00:00",
  "modifiedDate": "2023-10-01T12:00:00",
  "pwnCount": 152445165,
  "description": "In October 2013, Adobe was breached...",
  "logoPath": "https://images.haveibeenpwned.com/adobe.png",
  "attribution": "Adobe Systems Inc.",
  "disclosureUrl": "http://helpx.adobe.com/customer-care-external.html",
  "dataClasses": [
    "Email addresses",
    "Passwords",
    "Password hints",
    "Usernames"
  ],
  "isVerified": true,
  "isFabricated": false,
  "isSensitive": false,
  "isRetired": false,
  "isSpamList": false,
  "isMalware": false,
  "isSubscriptionFree": true,
  "isStealerLog": false
}
```

---

## 🔧 Configuration
The connection string is located in **`appsettings.json`**. Update this if your SQL Server instance differs from LocalDB.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=PwnedDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```