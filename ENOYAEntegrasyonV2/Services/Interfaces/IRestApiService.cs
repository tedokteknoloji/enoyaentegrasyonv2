using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ENOYAEntegrasyonV2.Models.Entities;

namespace ENOYAEntegrasyonV2.Services.Interfaces
{
    /// <summary>
    /// REST API servisi interface (IFS)
    /// </summary>
    public interface IRestApiService
    {
        /// <summary>
        /// OAuth2 token al
        /// </summary>
        Task<string> GetAccessTokenAsync();

        /// <summary>
        /// İş emri listesini getir
        /// </summary>
        Task<List<IFSPLANLine>> GetShopOrderListAsync(string contract = null, string orderNo = "", string routingAlternative = "*");

        /// <summary>
        /// MALZEME listesini getir
        /// </summary>
        Task<List<MALZEME>> GetMaterialListAsync(string contract = null, string partNo = "", string userId = "", string dateCreated = null);

        /// <summary>
        /// Work order raporu gönder
        /// </summary>
        Task<bool> ReportWorkOrderAsync(string systemId, string messageType, string messageText);
    }
}

