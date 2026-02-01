namespace CoastalPharmacyCRUD.Constants
{
    /// Provides constant, read-only values ​​for the entire application.
    // Includes unique identifiers (GUIDs) for transaction processes and roles

    public enum ActionType { Delete = 0, Create = 1, Update = 2 }
    public class StaticData
    {
        // Key (2D): (Set, Action) 
        // Value: (Guid ProcessId, string Message)
        public static readonly Dictionary<(string Set, ActionType Action), (Guid Id, string Message)> AuditMatrix = new()
        {
        // Category (CAT)
        { ("CAT", ActionType.Create), (TransactionProcesses.CreateCategory, "A new category was created") },
        { ("CAT", ActionType.Update), (TransactionProcesses.UpdateCategory, "The category details were updated") },

        // Subcat (SAT) 
        { ("SAT", ActionType.Create), (TransactionProcesses.CreateSubCategory, "A new subcategory was added to the catalog") },
        { ("SAT", ActionType.Update), (TransactionProcesses.UpdateSubCategory, "Subcategory information was modified") },

        };

        // Some Transactions Proccess that users can do

        public static class TransactionProcesses
        {
            public static readonly Guid CreateProduct = Guid.Parse("cf51df56-97dc-4335-9029-6cdf976e8726");
            public static readonly Guid UpdateProduct = Guid.Parse("d04252ac-950c-40a9-ae25-f8b8bdd2626b");
            public static readonly Guid DeleteProduct = Guid.Parse("c7d147db-4814-487d-a7a9-a5618b09d564");
            public static readonly Guid CreateUser = Guid.Parse("e04b93d2-7b9d-459d-8f0a-46273ed6b8eb");
            public static readonly Guid CreateCategory = Guid.Parse("0182c74e-37da-4a42-953a-983d8c164e42");
            public static readonly Guid CreateSubCategory = Guid.Parse("a908456c-a96f-4639-9afa-442e68daff6a");
            public static readonly Guid UpdateCategory = Guid.Parse("92c3f37f-4b25-4a84-bcda-761611bfd10e");
            public static readonly Guid UpdateSubCategory = Guid.Parse("4a626e98-2c31-4201-a895-3518a4e605ea");

            public static readonly Guid CreateNewIdentifier = Guid.Parse("7747cbe4-dcd3-4af2-9377-b4767246fe77");
            public static readonly Guid UpdateIdentifier = Guid.Parse("446d84a6-f898-4ee1-a296-fe05f904e585");

        }
        // Rol identifiers that match the records
        public static class RoleIdentifiers
        {
            public static readonly Guid Admin = Guid.Parse("65da9bd8-b01f-4d53-a8d4-f10acd6930a1");
            public static readonly Guid User = Guid.Parse("654a3456-e04f-47a4-acfa-af138d678542");
        }

    }
}