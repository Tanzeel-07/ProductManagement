using System.Threading.Tasks;

namespace ProductManagement.Services.Interfaces
{
    public interface IStockService
    {
        Task UpdateProductStockByIdAsync(int id, int quantity, bool isIncrement);
    }
}
