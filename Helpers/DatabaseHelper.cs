using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestMapper.Helpers
{
    public class DatabaseHelper
    {
        public static string ExecuteStoredProcedure(string spName, Dictionary<string, object> parameters)
        {
            string SQLConnectionString = KeyVaultHelper.SQLConnectionString;
            DataTable datatable = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(SQLConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(spName, con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        if (parameters != null)
                        {
                            foreach (KeyValuePair<string, object> param in parameters)
                            {
                                command.Parameters.Add(new SqlParameter(param.Key, param.Value));
                            }
                            command.Parameters["@SuccessMessage"].Direction = ParameterDirection.Output;
                            command.Parameters["@SuccessMessage"].Size = 10000; //what does this size do?
                        }
                        con.Open();
                        datatable.Load(command.ExecuteReader());
                        con.Close();
                        return command.Parameters["@SuccessMessage"].Value.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public static DataTable ExecStoredProcedure(string spName, Dictionary<string, object> parameters)
        {
            string SQLConnectionString = KeyVaultHelper.SQLConnectionString;
            DataTable datatable = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(SQLConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(spName, con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        if (parameters != null)
                        {
                            foreach (KeyValuePair<string, object> param in parameters)
                            {
                                command.Parameters.Add(new SqlParameter(param.Key, param.Value));
                            }
                        }
                        con.Open();
                        datatable.Load(command.ExecuteReader());
                        con.Close();
                        return datatable;
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public static Dictionary<string, Object> PrepareSalesTransactionsSPParameters(DataTable dtSalesTransactions,
            DataTable dtProducts, DataTable dtProductProms, DataTable dtSaleProms, DataTable dtTenders,
            string correlationId, string transactionId, string transactionIDType, DateTime transactionTimeStamp)
        {
            try
            {
                Dictionary<string, Object> keyValuePairs = new Dictionary<string, Object>();

                keyValuePairs.Add("@tvpSalesTransactions", dtSalesTransactions);
                keyValuePairs.Add("@tvpProducts", dtProducts);
                keyValuePairs.Add("@tvpProductProms", dtProductProms);
                keyValuePairs.Add("@tvpSaleProms", dtSaleProms);
                keyValuePairs.Add("@tvpTenders", dtTenders);
                keyValuePairs.Add("@CorrelationId", correlationId);
                keyValuePairs.Add("@TransactionId", transactionId);
                keyValuePairs.Add("@TransactionIDType", transactionIDType);
                keyValuePairs.Add("@TransactionTimeStamp", transactionTimeStamp);
                keyValuePairs.Add("@SuccessMessage", "");

                return keyValuePairs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Dictionary<string, Object> PrepareSalesTotalsSPParameters(
            DataTable dtDocumentTransaction, DataTable dtSite, DataTable dtDepartments, DataTable dtSalesTotals)
        {
            try
            {
                Dictionary<string, Object> keyValuePairs = new Dictionary<string, Object>();

                keyValuePairs.Add("@tvpDocumentTransaction", dtDocumentTransaction);
                keyValuePairs.Add("@tvpSite", dtSite);
                keyValuePairs.Add("@tvpDepartments", dtDepartments);
                keyValuePairs.Add("@tvpSalesTotals", dtSalesTotals);
                keyValuePairs.Add("@SuccessMessage", "");

                return keyValuePairs;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static Dictionary<string, Object> PrepareAuditParameters(
            string requestId, string stage, string message)
        {
            try
            {
                Dictionary<string, Object> keyValuePairs = new Dictionary<string, Object>();

                keyValuePairs.Add("@RequestId", requestId);
                keyValuePairs.Add("@Stage", stage);
                keyValuePairs.Add("@Message", message);

                return keyValuePairs;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
