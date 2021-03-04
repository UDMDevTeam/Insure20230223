using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Data
{
    public class DailySalesFigures
    {
        public List<string> GetUniqueCampaignCodes(DataTable dtSales)
        {
            List<string> Codes = new List<string>();
            DataView view = new DataView(dtSales);
            var table = dtSales.AsEnumerable().GroupBy(row => row.Field<string>("CampaignCode")).Select(group => group.First()).CopyToDataTable();
            foreach (DataRow row in table.Rows)
            {
                Codes.Add(row["CampaignCode"].ToString());
            }
            view.Dispose();
            table.Dispose();
            return Codes;
        }
        public TimeSpan CalculateHoursWorked(string campaignId,long? userId,DateTime saleDate)
        {
            TimeSpan span = new TimeSpan();

            string sql = "SELECT CONVERT(time, DATEADD(s, DATEDIFF(MINUTE, MIN(dbo.UserHours.WorkingDate), MAX(dbo.[User].StampDate)), 144)) AS Expr1 FROM  dbo.INImport INNER JOIN dbo.INCampaign ON dbo.INImport.FKINCampaignID = dbo.INCampaign.ID INNER JOIN dbo.UserHours ON dbo.INCampaign.ID = dbo.UserHours.FKINCampaignID INNER JOIN dbo.[User] ON dbo.UserHours.FKUserID = dbo.[User].ID AND dbo.INImport.FKUserID = dbo.[User].ID WHERE dbo.[User].ID = '" + userId.Value.ToString() + "'  AND dbo.INImport.SaleDate = '" + saleDate.Date.ToString() + "'";
            int month = saleDate.Month;
            int day = saleDate.Day;
            int year = saleDate.Year;

            DataTable dt = Methods.GetTableData(sql);
            if (dt != null && dt.Rows[0][0].ToString().Length > 0)
            {
               span = TimeSpan.Parse(dt.Rows[0][0].ToString());
            }
            return span;
        }
        public double GetTotalSales(string campaignId, long? userId)
        {
            double salesCount = 0.0;




            return salesCount;
        }
    }
}
