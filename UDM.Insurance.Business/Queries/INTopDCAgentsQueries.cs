using Embriant.Framework.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDM.Insurance.Business.Objects;

namespace UDM.Insurance.Business.Queries
{
    public static class INTopDCAgentsQueries
    {
        internal static string Fill(INTopDCAgents agentData, ref object[] parameters)
        {
            string query = string.Empty;
            if (agentData != null)
            {
                query = @"
        SELECT [ID], [FKUserID], [CampaignType], [StampUserID], [AcceptedRates], [StampDate] 
        FROM [Insure].[dbo].[INTopDCAgents] 
        WHERE [ID] = @ID";
                parameters = new object[]
                {
            Database.GetParameter("@ID", agentData.ID)
                };
            }
            return query;
        }

        public static string InsertOrUpdate(INTopDCAgents agentData, ref object[] parameters)
        {
            string query = @"
IF EXISTS (SELECT 1 FROM [Insure].[dbo].[INTopDCAgents] WHERE ID = @ID)
BEGIN
    UPDATE [Insure].[dbo].[INTopDCAgents]
    SET FKUserID = @FKUserID,
        CampaignType = @CampaignType,
        StampUserID = @StampUserID,
        AcceptedRates = @AcceptedRates,
        StampDate = @StampDate
    WHERE ID = @ID
END
ELSE
BEGIN
    INSERT INTO [Insure].[dbo].[INTopDCAgents] (FKUserID, CampaignType, StampUserID, AcceptedRates, StampDate)
    VALUES (@FKUserID, @CampaignType, @StampUserID, @AcceptedRates, @StampDate)
END";

            parameters = new object[]
            {
            new SqlParameter("@ID", agentData.ID),
            new SqlParameter("@FKUserID", agentData.FKUserID),
            new SqlParameter("@CampaignType", agentData.CampaignType),
            new SqlParameter("@StampUserID", agentData.StampUserID),
            new SqlParameter("@AcceptedRates", agentData.AcceptedRates),
            new SqlParameter("@StampDate", agentData.StampDate)
            };

            return query;
        }

        public static string Delete(INTopDCAgents agentData, ref object[] parameters)
        {
            string query = "DELETE FROM [Insure].[dbo].[INTopDCAgents] WHERE ID = @ID";

            parameters = new object[]
            {
            new SqlParameter("@ID", agentData.ID)
            };

            return query;
        }

        public static string List()
        {
            string query = "SELECT ID, FKUserID, CampaignType, StampUserID, AcceptedRates, StampDate FROM [Insure].[dbo].[INTopDCAgents]";
            return query;
        }
        public static string ListDeleted()
        {
            // Assuming there is a IsDeleted or similar flag in your schema to mark records as deleted
            string query = "SELECT ID, FKUserID, CampaignType, StampUserID, AcceptedRates, StampDate FROM [Insure].[dbo].[INTopDCAgents] WHERE IsDeleted = 1";
            return query;
        }


        internal static bool RecordExists(INTopDCAgents agentData)
        {
            string query = "SELECT COUNT(1) FROM [Insure].[dbo].[INTopDCAgents] WHERE ID = @ID";
            object[] parameters = new object[] { new SqlParameter("@ID", agentData.ID) };
            parameters[0] = Database.GetParameter("@FKINImportID", agentData.ID);


            int count = Convert.ToInt32(Database.ExecuteScalar(query, parameters));
            return count > 0;

        }
    }

}
