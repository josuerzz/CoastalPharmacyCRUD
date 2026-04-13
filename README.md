# CoastalPharmacyCRUD

A full-stack enterprise-grade CRUD application for pharmacy management. It features inventory control, transaction logging, and robust role-based access control (RBAC).

<img src="/assets/screenshoots/Coastal_Pharmacy_Managment_system.webp" width="800" alt="summary-preview">

---

## Key Features
- **Signals-based State Management:** Leveraging Angular 19 signals for high-performance UI updates.
- **Secure Authentication:** Implementation of JWT via Http-Only Cookies and Refresh Tokens.
- **Global Error Handling:** Centralized Backend Middleware and Frontend Interceptors for seamless UX.
- **Containerized Architecture:** Fully Dockerized for instant deployment.

## Technologies

**Backend**
- **Framework:** .NET 8 (C#).
- **Database:** SQL Server + Entity Framework Core — Code First
- **Security:** Password hashing, JWT, Role-based Authorization
- **Architecture:** Service-Pattern, DTOs, and Exception Middleware.

**Frontend**
- **Framework:** Angular 19
- **UI/UX:** Flexbox Responsive, SweetAlert2, Reactive Forms, Custom Pagination
- **Networking:** Functional Interceptors for 401/403 error management.

---

## Quick start (Docker)
The easiest way to run the entire stack (DB, API, and UI) is using Docker Compose:

### 1. Clone the repository
git clone https://github.com/josuerzz/CoastalPharmacyCRUD

```cmd
cd CoastalPharmacyCRUD
```

### 1.1 Create a `.env` file in the root directory based on the `.env.example` file provided.

...After this

### If you are running the project without Docker, update the connection string in `backend/appsettings.json` to point to your local SQL Server instance.


### 2. Launch the services
```cmd
docker compose up --build
```

### 3. Access the app
- Frontend: http://localhost:4200
- API Swagger: http://localhost:5083/swagger

--- 

## Manual Setup (Withour Docker)

### 1. Clone the repository
git clone https://github.com/josuerzz/CoastalPharmacyCRUD

## Backend Setup (.NET API)

### 2. Configure the database

1. Ensure you have SQL Server running locally.

2. Edit `appsettings.Development.json` and update the `DefaultConnection`.

  *Note: If you are using our custom port, ensure the server is set to localhost,1435.*

3. Apply migrations to create the database schema:
```cmd 
cd backend
dotnet ef update database
```
### 3. Run the API
```cmd
dotnet run
```
Swagger will be available at: https://localhost:5083/swagger

## Frontend Setup (Angular)

Requires:

- Node.js 18+
- Angular CLI

### 1. Install dependencies
```cmd
cd frontend
npm install
```
### 2. Configure API URL

Edit `src/environments/environment.ts` and set the backend URL (ensure the port matches your .NET API, usually 5083):

Set the backend URL like this:
```typescript
export const environment = 
{
  apiUrl: 'https://localhost:5083/api'
};
```

### 3. Run development server

```cmd
ng serve -o
```
The application will run probably at: http://localhost:4200/

[IMPORTANT]
**Database Seeding:** To initialize the system with default categories and subcategories, execute the SQL scripts located in `backend/data/scripts` in the numbered order.

---

## Troubleshooting (Common Issues)

### 1. Port 1433 Conflict (SQL Server)

If you already have **SQL Server installed locally on Windows**, Docker might fail to bind the port because the system is already using it.
* **Symptom:** `Error: Bind for 0.0.0.0:1433 failed`.
* **Solution:** We use port **1435** in the `.env` file to avoid this conflict. Ensure your Connection String or DBeaver points to `localhost,1435`.

### 2. ".env" File Encoding Errors

Docker is strict about file encoding and doesn't like the Windows default (UTF-16).
* **Symptom:** `failed to read .env: unexpected character ""`.
* **Solution:** Open your `.env` file in **VS Code**, check the bottom right corner, and ensure the encoding is set to **UTF-8** (not UTF-16 or UTF-8 with BOM).

### 3. "Login Failed for user 'sa'"

If you changed the password in the `.env` file AFTER the database container was already created for the first time:
* **Symptom:** The API cannot connect even with the new password.
* **Solution:** Docker volumes persist data. To apply a new password to the engine, you must reset the volume:
    ```bash
    docker compose down -v
    docker compose up -d
    ```
    *Warning: This will delete all existing data in the database.*

---

## API Modules

- **Inventory:** Full CRUD for medicine management
- **Auth:** Secure login, registration, and session refresh
- **History:** Automated logging of all stock movements
- **Permissions:** Admin vs. User restricted views and actions.

## License

**MIT License**

You are allowed to:

- Download
- Copy
- Modify
- Run

---

## Author

Project developed by ***Josuerzz***  
For academic and professional purposes.

If you would like to use this project in another context, please contact me.
