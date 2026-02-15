using System.Collections.Generic;
using System.Threading.Tasks;
using ENOYAEntegrasyonV2.Models.Entities;

namespace ENOYAEntegrasyonV2.Repositories.Interfaces
{
    /// <summary>
    /// SEVKIYAT repository interface
    /// </summary>
    public interface ISevkiyatRepository
    {
        Task<SEVKIYAT> GetByIdAsync(int kod);
        Task<List<SEVKIYAT>> GetAllAsync();
        Task<int> InsertAsync(SEVKIYAT sevkiyat);
        Task<int> UpdateAsync(SEVKIYAT sevkiyat);
        Task<int> DeleteAsync(int kod);
        Task<List<SEVKIYAT>> GetByOrderNoAsync(string orderNo);
        Task<List<SEVKIYAT>> GetUnreportedAsync();
    }
}

