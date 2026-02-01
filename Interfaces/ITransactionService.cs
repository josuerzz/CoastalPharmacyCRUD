using CoastalPharmacyCRUD.Data;

namespace CoastalPharmacyCRUD.Interfaces
{
    public interface ITransactionService
    {
        void SaveTransaction(Guid processId, Guid userId, string entityName, Guid entityId, string description, string details);

    }
}
