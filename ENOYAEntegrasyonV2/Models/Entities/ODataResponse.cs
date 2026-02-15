using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Reflection;

namespace ENOYAEntegrasyonV2.Models.Entities
{
    public class ODataResponse<T>
    {
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        [JsonProperty("value")]
        public List<T> Value { get; set; }
    }

    public class ReportOrder
    {
        public string SystemId { get; set; }
        public string MessageType { get; set; }
        public string MessageText { get; set; }
    }


    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class ReportOrderRequest
    {
        [JsonProperty("SystemId")]
        public string SystemId { get; set; }

        [JsonProperty("MessageType")]
        public string MessageType { get; set; }

        [JsonProperty("MessageText")]
        public string MessageText { get; set; }
    }

}
