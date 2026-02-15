using System.Collections.Generic;
using System.Threading.Tasks;
using ENOYAEntegrasyonV2.Models.Entities;

namespace ENOYAEntegrasyonV2.Repositories.Interfaces
{
    /// <summary>
    /// IRSALIYE repository interface
    /// </summary>
    public interface IIrsaliyeRepository
    {
        Task<Irsaliye> GetByIdAsync(decimal kod);
        Task<List<Irsaliye>> GetAllAsync();
        Task<int> InsertAsync(Irsaliye irsaliye);
        Task<int> UpdateAsync(Irsaliye irsaliye);
        Task<int> DeleteAsync(decimal kod);
        Task<List<Irsaliye>> GetByOrderNoAsync(string orderNo);
    }
}

