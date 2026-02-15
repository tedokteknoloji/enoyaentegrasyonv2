using System.Collections.Generic;
using System.Threading.Tasks;
using ENOYAEntegrasyonV2.Models.Entities;

namespace ENOYAEntegrasyonV2.Repositories.Interfaces
{
    /// <summary>
    /// MALZEME repository interface
    /// </summary>
    public interface IMALZEMERepository
    {
        Task<MALZEME> GetByIdAsync(decimal kod);
        Task<MALZEME> GetByPartNoAsync(string partNo);
        Task<List<MALZEME>> GetAllAsync();
        Task<int> InsertAsync(MALZEME malzeme);
        Task<int> UpdateAsync(MALZEME malzeme);
        Task<int> DeleteAsync(decimal kod);
    }
}

