# CoastalPharmacyCRUD

A full-stack CRUD application for pharmacy management, featuring inventory control, transaction logging, and role-based access.  
This project is part of my professional portfolio and demonstrates backend architecture, clean code practices, and REST API design.

---

## Technologies

**Backend**
- .NET 7 (C#)
- Entity Framework Core — Code First
- ASP.NET Core Identity (password hashing)
- SQL Server
- DTO pattern for data transfer
- Swagger / OpenAPI
- **Added**: Middleware Exceptions, Interfaces, Services, SecurityHelpers

**Frontend**
- Angular
- TypeScript

---

## Project Setup

### 1. Clone the repository
git clone https://github.com/your-username/your-repository.git

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

Swagger will be available at: https://localhost:7008/swagger

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
export const environment = {
  apiUrl: 'https://localhost:4200/api'
};
```

### 3. Run development server
Make sure the `src/environments/environment.ts` file points to the address where your .NET API runs. E.g. https://localhost:4200/api.
```cmd
ng serve -o
```

Application will run probably at:
http://localhost:4200/

---

## API Modules

- **Products** — full inventory CRUD
- **Users** — authentication & identity
- **Transactions** — transaction history
- **Roles / Identifiers** — process & role management

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
