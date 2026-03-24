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
- **UI/UX:** SweetAlert2, Reactive Forms, Custom Pagination
- **Networking:** Functional Interceptors for 401/403 error management.

---

## Quick start (Docker)
The easiest way to run the entire stack (DB, API, and UI) is using Docker Compose:

### 1. Clone the repository
git clone https://github.com/josuerzz/CoastalPharmacyCRUD

```cmd
cd CoastalPharmacyCRUD
```

### 2. Launch the services
```cmd
docker compose up --build
```

### 3. Access the app
- Frontend: http://localhost:4200
- API Swagger: http://localhost:5083/swagger

--- 

## Manual Setup

### 1. Clone the repository
git clone https://github.com/josuerzz/CoastalPharmacyCRUD

## Backend Setup (.NET API)

### 2. Configure the database

Edit `appsettings.Development.json` and update the `DefaultConnection`.

Create the database tables:
```cmd 
Update-Database
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

npm install

### 2. Configure API URL

Edit `src/environments/environment.ts`

Set the backend URL like this:
```typescript
export const environment = 
{
  apiUrl: 'https://localhost:4200/api'
};
```

### 3. Run development server
Make sure the `src/environments/environment.ts` file points to the address where your .NET API runs. E.g. http://localhost:4200/api
```cmd
ng serve -o
```

Application will run probably at:
http://localhost:4200/

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
