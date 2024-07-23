using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RequestMapper.Models
{
    public class REQSalesTotals
    {
        public DocumentHeader DocumentHeader { get; set; }
        public SalesTotals SalesTotals { get; set; }
    }

    public class DocumentHeader
    {
        public DocumentTransaction Transaction { get; set; }
    }

    public class DocumentTransaction
    {
        public string CorrelationId { get; set; }
        public string TransactionID { get; set; }
        public string TransactionIDType { get; set; }
        public DateTime TransactionTimeStamp { get; set; }
        public DateTime CreatedTimestamp { get; set; } //default
    }

    public class SalesTotals
    {
        public Site Site { get; set; }
        public List<Department> Depts { get; set; }
        [JsonProperty("SalesTotals")]
        public List<SalesTotal> SalesTotal { get; set; }
    }

    public class Site
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public string ABN { get; set; }
        public string POSVendorName { get; set; }
        public string POSProductName { get; set; }
        public string POSProductVersion { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveUntil { get; set; }
        public DateTime CreatedTimestamp { get; set; } //default
        public string CorrelationId { get; set; }
    }

    public class Department
    {
        public int SiteId { get; set; }
        public string DeptId { get; set; }
        public string DeptName { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveUntil { get; set; }
        public DateTime CreatedTimestamp { get; set; } //default
        public string CorrelationId { get; set; }
    }

    public class SalesTotal
    {
        public int SiteId { get; set; }
        public string POSId { get; set; }
        public DateTime SalesDate { get; set; }
        public string LocationId { get; set; }
        public decimal SalesValueInc { get; set; }
        public string SiteName { get; set; }
        public string ABN { get; set; }
        public string POSVendorName { get; set; }
        public string POSProductName { get; set; }
        public string POSProductVersion { get; set; }
        public DateTime CreatedTimestamp { get; set; } //default
        public DateTime UpdatedTimestamp { get; set; } //default
        public string CorrelationId { get; set; }
    }
}
