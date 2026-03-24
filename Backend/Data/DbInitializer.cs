
using CoastalPharmacyCRUD.Models;
using CoastalPharmacyCRUD.Constants;
using CoastalPharmacyCRUD.Data;
using CoastalPharmacyCRUD.Helpers;

namespace CoastalPharmacyCRUD
{
    public class DbInitializer
    {
        public static void Seed(ApplicationDbContext context, IConfiguration config)
        {
            if (!context.CDL_Identifiers.Any()) 
            {
            // Setting up the app
            // 1. Adding Roles and transactions
            context.CDL_Identifiers.AddRange(

            // Roles
            new CDL_Identifier
            {
                Id = StaticData.RoleIdentifiers.Admin,
                Set = "ROL",
                ElementNumber = 1,
                Code = "ROL1",
                Description = "Administrador",
                Use = "Rol con todos los permisos",
                ParentId = null
            },
            new CDL_Identifier
            {
                Id = StaticData.RoleIdentifiers.User,
                Set = "ROL",
                ElementNumber = 2,
                Code = "ROL2",
                Description = "Usuario",
                Use = "Rol con permisos limitados",
                ParentId = null
            },
            //Transactions
            new CDL_Identifier
            {
                Id = StaticData.TransactionProcesses.CreateProduct,
                Set = "TRA",
                ElementNumber = 1,
                Code = "TRA1",
                Description = "Transacción producto",
                Use = "Creación de producto",
                ParentId = null
            },
            new CDL_Identifier
            {
                Id = StaticData.TransactionProcesses.UpdateProduct,
                Set = "TRA",
                ElementNumber = 2,
                Code = "TRA2",
                Description = "Transacción producto",
                Use = "Actualización de producto",
                ParentId = null
            },
            new CDL_Identifier
            {
                Id = StaticData.TransactionProcesses.DeleteProduct,
                Set = "TRA",
                ElementNumber = 3,
                Code = "TRA3",
                Description = "Transacción producto",
                Use = "Eliminación de producto",
                ParentId = null
            },
            new CDL_Identifier
            {
                Id = StaticData.TransactionProcesses.CreateUser,
                Set = "TRA",
                ElementNumber = 4,
                Code = "TRA4",
                Description = "Transacción usuario",
                Use = "Creación de usuario",
                ParentId = null
            },
            new CDL_Identifier
            {
                Id = StaticData.TransactionProcesses.CreateCategory,
                Set = "TRA",
                ElementNumber = 5,
                Code = "TRA5",
                Description = "Transacción identificador",
                Use = "Creación de Categoría",
                ParentId = null
            },
            new CDL_Identifier
            {
                Id = StaticData.TransactionProcesses.UpdateCategory,
                Set = "TRA",
                ElementNumber = 6,
                Code = "TRA6",
                Description = "Transacción identificador",
                Use = "Actualización de Categoría",
                ParentId = null
            },
            new CDL_Identifier
            {
                Id = StaticData.TransactionProcesses.CreateSubCategory,
                Set = "TRA",
                ElementNumber = 7,
                Code = "TRA7",
                Description = "Transacción identificador",
                Use = "Creación de SubCategoría",
                ParentId = null
            },
            new CDL_Identifier
            {
                Id = StaticData.TransactionProcesses.UpdateSubCategory,
                Set = "TRA",
                ElementNumber = 8,
                Code = "TRA8",
                Description = "Transacción identificador",
                Use = "Actualización de SubCategoría",
                ParentId = null
            },
            new CDL_Identifier
            {
                Id = StaticData.TransactionProcesses.CreateNewIdentifier,
                Set = "TRA",
                ElementNumber = 9,
                Code = "TRA9",
                Description = "Transacción identificador",
                Use = "Creación de Identificador Generico",
                ParentId = null
            },
            new CDL_Identifier
            {
                Id = StaticData.TransactionProcesses.UpdateIdentifier,
                Set = "TRA",
                ElementNumber = 10,
                Code = "TRA10",
                Description = "Transacción identificador",
                Use = "Actualización de Identificador Generico",
                ParentId = null
            });
            context.SaveChanges();
            }

            // 2. Creating the Admin User Default
            if (!context.SYS_Users.Any()) 
            {
                string adminEmail = config.GetValue<string>("InitialAdmin:Email") ?? "admin@cpharmacy.com";
                string adminPassword = config.GetValue<string>("InitialAdmin:Password") ?? "Admin123+";
                int ExpirationTokenDays = config.GetValue("JwtSettings:ExpirationDaysToken", 7);
               
                var token = SecurityHelpers.GenerateSecureToken();

                var admin = new SYS_User 
                {
                    Id = Guid.NewGuid(),
                    Email = adminEmail,
                    Name = "admin",
                    Surname = "sys",
                    PasswordHash = SecurityHelpers.HashPassword(adminPassword),
                    RoleIdentifierId = StaticData.RoleIdentifiers.Admin,
                    Status = 1,
                    RefreshToken = token,
                    TokenCreated = DateTime.UtcNow,
                    TokenExpires = DateTime.UtcNow.AddDays(ExpirationTokenDays)
                };

                context.SYS_Users.Add(admin);
                context.SaveChanges();
            }

        }
    }
}