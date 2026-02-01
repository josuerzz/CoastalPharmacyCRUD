using CoastalPharmacyCRUD.Data;
using CoastalPharmacyCRUD.Interfaces;
using CoastalPharmacyCRUD.Models;

namespace CoastalPharmacyCRUD.Services
{
    // Service for saving in Transactions
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SaveTransaction(Guid processId, Guid userId, string entityName, Guid entityId, string description, string details)
        {
            if (string.IsNullOrEmpty(entityName)) throw new ArgumentNullException(nameof(entityName));

            var newTransaction = new SYS_Transaction
            {
                Id = Guid.NewGuid(),
                ProcessId = processId,
                UserId = userId,
                EntityName = entityName,
                EntityId = entityId,
                Description = description,
                Details = details,
                Date = DateTime.UtcNow
            };

            _context.SYS_Transactions.Add(newTransaction);
        }
    } 
}
