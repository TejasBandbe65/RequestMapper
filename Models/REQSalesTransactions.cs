using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestMapper.Models
{
    public class REQSalesTransactions
    {
        public DocumentHeader DocumentHeader { get; set; }
        public SalesTransactions SalesTransactions { get; set; }
    }

    public class SalesTransactions
    {
        public Site Site { get; set; }
        public List<SalesTransactionDetails> SalesTransactionDetails { get; set; }
    }

    public class SalesTransactionDetails
    {
        public int SiteId { get; set; } //
        public DateTime SaleTimeStamp { get; set; }
        public string POSId { get; set; }
        public string POSSaleId { get; set; }
        public string? LocationId { get; set; } //
        public string POSUserId { get; set; }
        public string SaleMethod { get; set; }
        public decimal SaleValueInc { get; set; }
        public decimal SaleGSTValue { get; set; }
        public string? OnlineProgramName { get; set; } //
        public DateTime? PostingDate { get; set; } //
        public DateTime CreatedTimestamp { get; set; } //default
        public string CorrelationId { get; set; } //
        public List<Product> Products { get; set; }
        public List<SaleProm> SaleProms { get; set; } //confirm the name from request SaleProm or SaleProms or SalesProm etc
        public List<Tender> Tenders { get; set; }
    }

    public class Product
    {
        public int SiteId { get; set; } //
        public DateTime SaleTimeStamp { get; set; } //
        public string POSId { get; set; } //
        public string POSSaleId { get; set; } //
        public int SaleLineNumber { get; set; }
        public string GTINPLU { get; set; }
        public string ProductName { get; set; }
        public string ProductMSC { get; set; }
        public string DeptId { get; set; }
        public string? MetcashProductCode { get; set; } //nullable
        public decimal BaseSellInc { get; set; }
        public decimal SaleQty { get; set; }
        public string SaleUnit { get; set; }
        public decimal SellValueInc { get; set; }
        public decimal SellValueEx { get; set; }
        public decimal SellGSTRate { get; set; }
        public decimal CostValueEx { get; set; }
        public decimal CostGSTRate { get; set; }
        public decimal? LoyaltyPoints { get; set; }
        public DateTime CreatedTimestamp { get; set; }  //default

        public List<ProductProm>? ProductProms { get; set; }
    }

    public class ProductProm
    {
        public int SiteId { get; set; } //
        public DateTime SaleTimeStamp { get; set; } //
        public string POSId { get; set; } //
        public string POSSaleId { get; set; } //
        public int SaleLineNumber { get; set; } //
        public string ProductPromCode { get; set; }
        public string ProductPromId { get; set; } //this should be GUID, but where can I get this?
        public decimal ProductPromValueInc { get; set; }
        public string? LoyaltyProgramName { get; set; } //nullable
        public DateTime CreatedTimestamp { get; set; } //default
    }

    public class SaleProm
    {
        public int SiteId { get; set; } //
        public DateTime SaleTimeStamp { get; set; } //
        public string POSId { get; set; } //
        public string POSSaleId { get; set; } //
        public int SaleLineNumber { get; set; } //
        public string SalePromCode { get; set; }
        public string? SalePromId { get; set; } //nullable
        public decimal SalePromValueInc { get; set; }
        public string? LoyaltyProgramName { get; set; } //nullable
        public DateTime CreatedTimestamp { get; set; } //default
    }

    public class Tender
    {
        public int SiteId { get; set; } //
        public DateTime SaleTimeStamp { get; set; } //
        public string POSId { get; set; } //
        public string POSSaleId { get; set; } //
        public int SaleLineNumber { get; set; } //this should be Tender sale line number only and not Product's
        public string TenderName { get; set; }
        public decimal? TenderValue { get; set; }
        public string? LoyaltyProgramName { get; set; } //nullable
        public string? TenderCardNumber { get; set; } //nullable
        public string? ShopperMobileNumber { get; set; } //nullable
        public decimal? LoyaltyEarned { get; set; } //nullable
        public decimal? LoyaltyRedeemed { get; set; } //nullable
        public DateTime CreatedTimestamp { get; set; } //default
    }
}
