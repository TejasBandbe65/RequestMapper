using RequestMapper.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestMapper.Helpers
{
    public class MappingHelper
    {
        public static Boolean CreateSalesTransactionsDatatables(ref DataTable dtSalesTransactions, 
            ref DataTable dtProducts, ref DataTable dtProductProms, ref DataTable dtSaleProms, ref DataTable dtTenders)
        {
            try
            {
                dtSalesTransactions.Columns.Add("SiteId", Type.GetType("System.Int32"));
                dtSalesTransactions.Columns.Add("SaleTimeStamp", Type.GetType("System.DateTime"));
                dtSalesTransactions.Columns.Add("POSId", Type.GetType("System.String"));
                dtSalesTransactions.Columns.Add("POSUserId", Type.GetType("System.String"));
                dtSalesTransactions.Columns.Add("POSSaleId", Type.GetType("System.String"));
                dtSalesTransactions.Columns.Add("LocationId", Type.GetType("System.String"));
                dtSalesTransactions.Columns.Add("SaleMethod", Type.GetType("System.String"));
                dtSalesTransactions.Columns.Add("SaleValueInc", Type.GetType("System.Decimal"));
                dtSalesTransactions.Columns.Add("SaleGSTValue", Type.GetType("System.Decimal"));
                dtSalesTransactions.Columns.Add("OnlineProgramName", Type.GetType("System.String"));
                dtSalesTransactions.Columns.Add("PostingDate", Type.GetType("System.DateTime"));
                dtSalesTransactions.Columns.Add("CorrelationId", Type.GetType("System.String"));

                dtProducts.Columns.Add("SiteId", Type.GetType("System.Int32"));
                dtProducts.Columns.Add("SaleTimeStamp", Type.GetType("System.DateTime"));
                dtProducts.Columns.Add("POSId", Type.GetType("System.String"));
                dtProducts.Columns.Add("POSSaleId", Type.GetType("System.String"));
                dtProducts.Columns.Add("SaleLineNumber", Type.GetType("System.Int32"));
                dtProducts.Columns.Add("GTINPLU", Type.GetType("System.String"));
                dtProducts.Columns.Add("ProductName", Type.GetType("System.String"));
                dtProducts.Columns.Add("ProductMSC", Type.GetType("System.String"));
                dtProducts.Columns.Add("DeptId", Type.GetType("System.String"));
                dtProducts.Columns.Add("MetcashProductCode", Type.GetType("System.String"));
                dtProducts.Columns.Add("BaseSellInc", Type.GetType("System.Decimal"));
                dtProducts.Columns.Add("SaleQty", Type.GetType("System.Decimal"));
                dtProducts.Columns.Add("SaleUnit", Type.GetType("System.String"));
                dtProducts.Columns.Add("SellValueInc", Type.GetType("System.Decimal"));
                dtProducts.Columns.Add("SellValueEx", Type.GetType("System.Decimal"));
                dtProducts.Columns.Add("SellGSTRate", Type.GetType("System.Decimal"));
                dtProducts.Columns.Add("CostValueEx", Type.GetType("System.Decimal"));
                dtProducts.Columns.Add("CostGSTRate", Type.GetType("System.Decimal"));
                dtProducts.Columns.Add("LoyaltyPoints", Type.GetType("System.Decimal"));

                dtProductProms.Columns.Add("SiteId", Type.GetType("System.Int32"));
                dtProductProms.Columns.Add("SaleTimeStamp", Type.GetType("System.DateTime"));
                dtProductProms.Columns.Add("POSId", Type.GetType("System.String"));
                dtProductProms.Columns.Add("POSSaleId", Type.GetType("System.String"));
                dtProductProms.Columns.Add("SaleLineNumber", Type.GetType("System.Int32"));
                dtProductProms.Columns.Add("ProductPromCode", Type.GetType("System.String"));
                dtProductProms.Columns.Add("ProductPromId", Type.GetType("System.String"));
                dtProductProms.Columns.Add("ProductPromValueInc", Type.GetType("System.Decimal"));
                dtProductProms.Columns.Add("LoyaltyProgramName", Type.GetType("System.String"));

                dtSaleProms.Columns.Add("SiteId", Type.GetType("System.Int32"));
                dtSaleProms.Columns.Add("SaleTimeStamp", Type.GetType("System.DateTime"));
                dtSaleProms.Columns.Add("POSId", Type.GetType("System.String"));
                dtSaleProms.Columns.Add("POSSaleId", Type.GetType("System.String"));
                dtSaleProms.Columns.Add("SaleLineNumber", Type.GetType("System.Int32"));
                dtSaleProms.Columns.Add("SalePromCode", Type.GetType("System.String"));
                dtSaleProms.Columns.Add("SalePromId", Type.GetType("System.String"));
                dtSaleProms.Columns.Add("SalePromValueInc", Type.GetType("System.Decimal"));
                dtSaleProms.Columns.Add("LoyaltyProgramName", Type.GetType("System.String"));

                dtTenders.Columns.Add("SiteId", Type.GetType("System.Int32"));
                dtTenders.Columns.Add("SaleTimeStamp", Type.GetType("System.DateTime"));
                dtTenders.Columns.Add("POSId", Type.GetType("System.String"));
                dtTenders.Columns.Add("POSSaleId", Type.GetType("System.String"));
                dtTenders.Columns.Add("SaleLineNumber", Type.GetType("System.Int32"));
                dtTenders.Columns.Add("TenderName", Type.GetType("System.String"));
                dtTenders.Columns.Add("TenderValue", Type.GetType("System.Decimal"));
                dtTenders.Columns.Add("LoyaltyProgramName", Type.GetType("System.String"));
                dtTenders.Columns.Add("TenderCardNumber", Type.GetType("System.String"));
                dtTenders.Columns.Add("ShopperMobileNumber", Type.GetType("System.String"));
                dtTenders.Columns.Add("LoyaltyEarned", Type.GetType("System.Decimal"));
                dtTenders.Columns.Add("LoyaltyRedeemed", Type.GetType("System.Decimal"));

                return true;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static Boolean UpdateSalesTransactionsDatatables(ref DataTable dtSalesTransactions,
            ref DataTable dtProducts, ref DataTable dtProductProms, ref DataTable dtSaleProms, 
            ref DataTable dtTenders, REQSalesTransactions requestJson, string productPromId)
        {
            try
            {
                foreach (var tran in requestJson.SalesTransactions.SalesTransactionDetails)
                {
                    DataRow drSalesTransactions = dtSalesTransactions.NewRow();

                    drSalesTransactions["SiteId"] = requestJson.SalesTransactions.Site.SiteId;
                    drSalesTransactions["SaleTimeStamp"] = tran.SaleTimeStamp;
                    drSalesTransactions["POSId"] = tran.POSId;
                    drSalesTransactions["POSUserId"] = tran.POSUserId;
                    drSalesTransactions["POSSaleId"] = tran.POSSaleId;
                    drSalesTransactions["LocationId"] = tran.LocationId;
                    drSalesTransactions["SaleMethod"] = tran.SaleMethod;
                    drSalesTransactions["SaleValueInc"] = tran.SaleValueInc;
                    drSalesTransactions["SaleGSTValue"] = tran.SaleGSTValue;
                    drSalesTransactions["OnlineProgramName"] = tran.OnlineProgramName;
                    //need code review here
                    drSalesTransactions["PostingDate"] = tran.PostingDate ?? (object)DBNull.Value;
                    drSalesTransactions["CorrelationId"] = requestJson.DocumentHeader.Transaction.CorrelationId;
                    dtSalesTransactions.Rows.Add(drSalesTransactions);

                    if(tran.Products != null && tran.Products.Count > 0)
                    {
                        foreach (var prod in tran.Products)
                        {
                            DataRow drProducts = dtProducts.NewRow();
                            drProducts["SiteId"] = requestJson.SalesTransactions.Site.SiteId;
                            drProducts["SaleTimeStamp"] = tran.SaleTimeStamp;
                            drProducts["POSId"] = tran.POSId;
                            drProducts["POSSaleId"] = tran.POSSaleId;
                            drProducts["SaleLineNumber"] = prod.SaleLineNumber;
                            drProducts["GTINPLU"] = prod.GTINPLU;
                            drProducts["ProductName"] = prod.ProductName;
                            drProducts["ProductMSC"] = prod.ProductMSC;
                            drProducts["DeptId"] = prod.DeptId;
                            drProducts["MetcashProductCode"] = prod.MetcashProductCode;
                            drProducts["BaseSellInc"] = prod.BaseSellInc;
                            drProducts["SaleQty"] = prod.SaleQty;
                            drProducts["SaleUnit"] = prod.SaleUnit;
                            drProducts["SellValueInc"] = prod.SellValueInc;
                            drProducts["SellValueEx"] = prod.SellValueEx;
                            drProducts["SellGSTRate"] = prod.SellGSTRate;
                            drProducts["CostValueEx"] = prod.CostValueEx;
                            drProducts["CostGSTRate"] = prod.CostGSTRate;
                            drProducts["LoyaltyPoints"] = prod.LoyaltyPoints ?? (object)DBNull.Value;
                            dtProducts.Rows.Add(drProducts);

                            if (prod.ProductProms != null && prod.ProductProms.Count > 0)
                            {
                                foreach (var prodProm in prod.ProductProms)
                                {
                                    DataRow drProductProms = dtProductProms.NewRow();
                                    drProductProms["SiteId"] = requestJson.SalesTransactions.Site.SiteId;
                                    drProductProms["SaleTimeStamp"] = tran.SaleTimeStamp;
                                    drProductProms["POSId"] = tran.POSId;
                                    drProductProms["POSSaleId"] = tran.POSSaleId;
                                    drProductProms["SaleLineNumber"] = prod.SaleLineNumber;
                                    drProductProms["ProductPromCode"] = prodProm.ProductPromCode;
                                    //drProductProms["ProductPromId"] = prodProm.ProductPromId;
                                    //this productPromId should be present in requestbody - SalesTransactions/SalesTransactionDetails/Products/ProductProms/ProductPromId
                                    //but for few requests, it is not present. so using manually gerenerated id because it is not null field
                                    drProductProms["ProductPromId"] = prodProm.ProductPromId ?? productPromId;
                                    drProductProms["ProductPromValueInc"] = prodProm.ProductPromValueInc;
                                    drProductProms["LoyaltyProgramName"] = prodProm.LoyaltyProgramName;
                                    dtProductProms.Rows.Add(drProductProms);
                                }
                            }
                            else
                            {
                                //
                            }
                        }
                    }
                    else
                    {
                        //
                    }
                    

                    if(tran.SaleProms != null && tran.SaleProms.Count > 0)
                    {
                        foreach (var saleProm in tran.SaleProms)
                        {
                            DataRow drSaleProms = dtSaleProms.NewRow();
                            drSaleProms["SiteId"] = requestJson.SalesTransactions.Site.SiteId;
                            drSaleProms["SaleTimeStamp"] = tran.SaleTimeStamp;
                            drSaleProms["POSId"] = tran.POSId;
                            drSaleProms["POSSaleId"] = tran.POSSaleId;
                            drSaleProms["SaleLineNumber"] = saleProm.SaleLineNumber;
                            drSaleProms["SalePromCode"] = saleProm.SalePromCode;
                            drSaleProms["SalePromId"] = saleProm.SalePromId;
                            drSaleProms["SalePromValueInc"] = saleProm.SalePromValueInc;
                            drSaleProms["LoyaltyProgramName"] = saleProm.LoyaltyProgramName;
                            dtSaleProms.Rows.Add(drSaleProms);
                        }
                    }
                    else
                    {
                        //
                    }

                    if(tran.Tenders != null && tran.Tenders.Count > 0)
                    {
                        foreach (var tender in tran.Tenders)
                        {
                            DataRow drTenders = dtTenders.NewRow();
                            drTenders["SiteId"] = requestJson.SalesTransactions.Site.SiteId;
                            drTenders["SaleTimeStamp"] = tran.SaleTimeStamp;
                            drTenders["POSId"] = tran.POSId;
                            drTenders["POSSaleId"] = tran.POSSaleId;
                            drTenders["SaleLineNumber"] = tender.SaleLineNumber;
                            drTenders["TenderName"] = tender.TenderName;
                            //need code reveiw
                            drTenders["TenderValue"] = tender.TenderValue ?? (object)DBNull.Value;
                            drTenders["LoyaltyProgramName"] = tender.LoyaltyProgramName;
                            drTenders["TenderCardNumber"] = tender.TenderCardNumber;
                            drTenders["ShopperMobileNumber"] = tender.ShopperMobileNumber;
                            //need code review here for next 2 lines
                            drTenders["LoyaltyEarned"] = tender.LoyaltyEarned ?? (object)DBNull.Value;
                            drTenders["LoyaltyRedeemed"] = tender.LoyaltyRedeemed ?? (object)DBNull.Value;
                            dtTenders.Rows.Add(drTenders);
                        }
                    }
                    else
                    {
                        //
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static Boolean CreateSalesTotalsDatatables(ref DataTable dtSite, ref DataTable dtDepartments, ref DataTable dtSalesTotals)
        {
            try
            {
                dtSite.Columns.Add("SiteId", Type.GetType("System.Int32"));
                dtSite.Columns.Add("SiteName", Type.GetType("System.String"));
                dtSite.Columns.Add("ABN", Type.GetType("System.String"));
                dtSite.Columns.Add("POSVendorName", Type.GetType("System.String"));
                dtSite.Columns.Add("POSProductName", Type.GetType("System.String"));
                dtSite.Columns.Add("POSProductVersion", Type.GetType("System.String"));
                dtSite.Columns.Add("EffectiveFrom", Type.GetType("System.DateTime"));
                dtSite.Columns.Add("EffectiveUntil", Type.GetType("System.DateTime"));
                dtSite.Columns.Add("CorrelationId", Type.GetType("System.String"));

                dtDepartments.Columns.Add("SiteId", Type.GetType("System.Int32"));
                dtDepartments.Columns.Add("DeptId", Type.GetType("System.String"));
                dtDepartments.Columns.Add("DeptName", Type.GetType("System.String"));
                dtDepartments.Columns.Add("EffectiveFrom", Type.GetType("System.DateTime"));
                dtDepartments.Columns.Add("EffectiveUntil", Type.GetType("System.DateTime"));
                dtDepartments.Columns.Add("CorrelationId", Type.GetType("System.String"));

                dtSalesTotals.Columns.Add("SiteId", Type.GetType("System.Int32"));
                dtSalesTotals.Columns.Add("POSId", Type.GetType("System.String"));
                dtSalesTotals.Columns.Add("SalesDate", Type.GetType("System.DateTime"));
                dtSalesTotals.Columns.Add("LocationId", Type.GetType("System.String"));
                dtSalesTotals.Columns.Add("SaleValueInc", Type.GetType("System.Decimal"));
                dtSalesTotals.Columns.Add("SiteName", Type.GetType("System.String"));
                dtSalesTotals.Columns.Add("ABN", Type.GetType("System.String"));
                dtSalesTotals.Columns.Add("POSVendorName", Type.GetType("System.String"));
                dtSalesTotals.Columns.Add("POSProductName", Type.GetType("System.String"));
                dtSalesTotals.Columns.Add("POSProductVersion", Type.GetType("System.String"));
                dtSalesTotals.Columns.Add("CorrelationId", Type.GetType("System.String"));

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static Boolean UpdateSalesTotalsDatatables(ref DataTable dtSite, ref DataTable dtDepartments, ref DataTable dtSalesTotals, REQSalesTotals requestJson)
        {
            try
            {
                if (requestJson.SalesTotals.Site != null)
                {
                    DataRow drSite = dtSite.NewRow();
                    drSite["SiteId"] = requestJson.SalesTotals.Site.SiteId;
                    drSite["SiteName"] = requestJson.SalesTotals.Site.SiteName;
                    drSite["ABN"] = requestJson.SalesTotals.Site.ABN;
                    drSite["POSVendorName"] = requestJson.SalesTotals.Site.POSVendorName;
                    drSite["POSProductName"] = requestJson.SalesTotals.Site.POSProductName;
                    drSite["POSProductVersion"] = requestJson.SalesTotals.Site.POSProductVersion;
                    drSite["EffectiveFrom"] = requestJson.DocumentHeader.Transaction.TransactionTimeStamp;
                    drSite["EffectiveUntil"] = new DateTime(2100, 1, 1);
                    drSite["CorrelationId"] = requestJson.DocumentHeader.Transaction.CorrelationId;
                    dtSite.Rows.Add(drSite);
                }
                else
                {
                    //
                }

                if (requestJson.SalesTotals.Depts != null && requestJson.SalesTotals.Depts.Count > 0)
                {
                    foreach (var dept in requestJson.SalesTotals.Depts)
                    {
                        DataRow drDepartments = dtDepartments.NewRow();
                        drDepartments["SiteId"] = requestJson.SalesTotals.Site.SiteId;
                        drDepartments["DeptId"] = dept.DeptId;
                        drDepartments["DeptName"] = dept.DeptName;
                        drDepartments["EffectiveFrom"] = requestJson.DocumentHeader.Transaction.TransactionTimeStamp;
                        drDepartments["EffectiveUntil"] = new DateTime(2100, 1, 1);
                        drDepartments["CorrelationId"] = requestJson.DocumentHeader.Transaction.CorrelationId;
                        dtDepartments.Rows.Add(drDepartments);
                    }
                }
                else
                {
                    //
                }

                if (requestJson.SalesTotals.SalesTotal != null && requestJson.SalesTotals.SalesTotal.Count > 0)
                {
                    foreach (var salesTotal in requestJson.SalesTotals.SalesTotal)
                    {
                        DataRow drSalesTotals = dtSalesTotals.NewRow();
                        drSalesTotals["SiteId"] = requestJson.SalesTotals.Site.SiteId;
                        drSalesTotals["POSId"] = salesTotal.POSId;
                        drSalesTotals["SalesDate"] = salesTotal.SalesDate;
                        drSalesTotals["LocationId"] = salesTotal.LocationId;
                        drSalesTotals["SaleValueInc"] = salesTotal.SalesValueInc;
                        drSalesTotals["SiteName"] = requestJson.SalesTotals.Site.SiteName;
                        drSalesTotals["ABN"] = requestJson.SalesTotals.Site.ABN;
                        drSalesTotals["POSVendorName"] = requestJson.SalesTotals.Site.POSVendorName;
                        drSalesTotals["POSProductName"] = requestJson.SalesTotals.Site.POSProductName;
                        drSalesTotals["POSProductVersion"] = requestJson.SalesTotals.Site.POSProductVersion;
                        drSalesTotals["CorrelationId"] = requestJson.DocumentHeader.Transaction.CorrelationId;
                        dtSalesTotals.Rows.Add(drSalesTotals);
                    }
                }
                else
                {
                    //
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
