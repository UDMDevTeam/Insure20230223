using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inbatch objects.
    /// </summary>
    internal abstract partial class INBatchQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inbatch from the database.
        /// </summary>
        /// <param name="inbatch">The inbatch object to delete.</param>
        /// <returns>A query that can be used to delete the inbatch from the database.</returns>
        internal static string Delete(INBatch inbatch, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbatch != null)
            {
                query = "INSERT INTO [zHstINBatch] ([ID], [FKINCampaignID], [Code], [UDMCode], [NewLeads], [UpdatedLeads], [ContainsLatentLeads], [Completed], [DateReceived], [IsArchived], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaignID], [Code], [UDMCode], [NewLeads], [UpdatedLeads], [ContainsLatentLeads], [Completed], [DateReceived], [IsArchived], [StampDate], [StampUserID] FROM [INBatch] WHERE [INBatch].[ID] = @ID; ";
                query += "DELETE FROM [INBatch] WHERE [INBatch].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbatch.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inbatch from the database.
        /// </summary>
        /// <param name="inbatch">The inbatch object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inbatch from the database.</returns>
        internal static string DeleteHistory(INBatch inbatch, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbatch != null)
            {
                query = "DELETE FROM [zHstINBatch] WHERE [zHstINBatch].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbatch.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inbatch from the database.
        /// </summary>
        /// <param name="inbatch">The inbatch object to undelete.</param>
        /// <returns>A query that can be used to undelete the inbatch from the database.</returns>
        internal static string UnDelete(INBatch inbatch, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbatch != null)
            {
                query = "INSERT INTO [INBatch] ([ID], [FKINCampaignID], [Code], [UDMCode], [NewLeads], [UpdatedLeads], [ContainsLatentLeads], [Completed], [DateReceived], [IsArchived], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaignID], [Code], [UDMCode], [NewLeads], [UpdatedLeads], [ContainsLatentLeads], [Completed], [DateReceived], [IsArchived], [StampDate], [StampUserID] FROM [zHstINBatch] WHERE [zHstINBatch].[ID] = @ID AND [zHstINBatch].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINBatch] WHERE [zHstINBatch].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INBatch] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINBatch] WHERE [zHstINBatch].[ID] = @ID AND [zHstINBatch].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINBatch] WHERE [zHstINBatch].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INBatch] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbatch.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inbatch object.
        /// </summary>
        /// <param name="inbatch">The inbatch object to fill.</param>
        /// <returns>A query that can be used to fill the inbatch object.</returns>
        internal static string Fill(INBatch inbatch, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbatch != null)
            {
                query = "SELECT [ID], [FKINCampaignID], [Code], [UDMCode], [NewLeads], [UpdatedLeads], [ContainsLatentLeads], [Completed], [DateReceived], [IsArchived], [StampDate], [StampUserID] FROM [INBatch] WHERE [INBatch].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbatch.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inbatch data.
        /// </summary>
        /// <param name="inbatch">The inbatch to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inbatch data.</returns>
        internal static string FillData(INBatch inbatch, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inbatch != null)
            {
            query.Append("SELECT [INBatch].[ID], [INBatch].[FKINCampaignID], [INBatch].[Code], [INBatch].[UDMCode], [INBatch].[NewLeads], [INBatch].[UpdatedLeads], [INBatch].[ContainsLatentLeads], [INBatch].[Completed], [INBatch].[DateReceived], [INBatch].[IsArchived], [INBatch].[StampDate], [INBatch].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INBatch].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INBatch] ");
                query.Append(" WHERE [INBatch].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbatch.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inbatch object from history.
        /// </summary>
        /// <param name="inbatch">The inbatch object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inbatch object from history.</returns>
        internal static string FillHistory(INBatch inbatch, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbatch != null)
            {
                query = "SELECT [ID], [FKINCampaignID], [Code], [UDMCode], [NewLeads], [UpdatedLeads], [ContainsLatentLeads], [Completed], [DateReceived], [IsArchived], [StampDate], [StampUserID] FROM [zHstINBatch] WHERE [zHstINBatch].[ID] = @ID AND [zHstINBatch].[StampUserID] = @StampUserID AND [zHstINBatch].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inbatch.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inbatchs in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inbatchs in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INBatch].[ID], [INBatch].[FKINCampaignID], [INBatch].[Code], [INBatch].[UDMCode], [INBatch].[NewLeads], [INBatch].[UpdatedLeads], [INBatch].[ContainsLatentLeads], [INBatch].[Completed], [INBatch].[DateReceived], [INBatch].[IsArchived], [INBatch].[StampDate], [INBatch].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INBatch].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INBatch] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inbatchs in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inbatchs in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINBatch].[ID], [zHstINBatch].[FKINCampaignID], [zHstINBatch].[Code], [zHstINBatch].[UDMCode], [zHstINBatch].[NewLeads], [zHstINBatch].[UpdatedLeads], [zHstINBatch].[ContainsLatentLeads], [zHstINBatch].[Completed], [zHstINBatch].[DateReceived], [zHstINBatch].[IsArchived], [zHstINBatch].[StampDate], [zHstINBatch].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINBatch].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINBatch] ");
            query.Append("INNER JOIN (SELECT [zHstINBatch].[ID], MAX([zHstINBatch].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINBatch] ");
            query.Append("WHERE [zHstINBatch].[ID] NOT IN (SELECT [INBatch].[ID] FROM [INBatch]) ");
            query.Append("GROUP BY [zHstINBatch].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINBatch].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINBatch].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inbatch in the database.
        /// </summary>
        /// <param name="inbatch">The inbatch object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inbatch in the database.</returns>
        public static string ListHistory(INBatch inbatch, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inbatch != null)
            {
            query.Append("SELECT [zHstINBatch].[ID], [zHstINBatch].[FKINCampaignID], [zHstINBatch].[Code], [zHstINBatch].[UDMCode], [zHstINBatch].[NewLeads], [zHstINBatch].[UpdatedLeads], [zHstINBatch].[ContainsLatentLeads], [zHstINBatch].[Completed], [zHstINBatch].[DateReceived], [zHstINBatch].[IsArchived], [zHstINBatch].[StampDate], [zHstINBatch].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINBatch].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINBatch] ");
                query.Append(" WHERE [zHstINBatch].[ID] = @ID");
                query.Append(" ORDER BY [zHstINBatch].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbatch.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inbatch to the database.
        /// </summary>
        /// <param name="inbatch">The inbatch to save.</param>
        /// <returns>A query that can be used to save the inbatch to the database.</returns>
        internal static string Save(INBatch inbatch, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inbatch != null)
            {
                if (inbatch.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINBatch] ([ID], [FKINCampaignID], [Code], [UDMCode], [NewLeads], [UpdatedLeads], [ContainsLatentLeads], [Completed], [DateReceived], [IsArchived], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaignID], [Code], [UDMCode], [NewLeads], [UpdatedLeads], [ContainsLatentLeads], [Completed], [DateReceived], [IsArchived], [StampDate], [StampUserID] FROM [INBatch] WHERE [INBatch].[ID] = @ID; ");
                    query.Append("UPDATE [INBatch]");
                    parameters = new object[10];
                    query.Append(" SET [FKINCampaignID] = @FKINCampaignID");
                    parameters[0] = Database.GetParameter("@FKINCampaignID", inbatch.FKINCampaignID.HasValue ? (object)inbatch.FKINCampaignID.Value : DBNull.Value);
                    query.Append(", [Code] = @Code");
                    parameters[1] = Database.GetParameter("@Code", string.IsNullOrEmpty(inbatch.Code) ? DBNull.Value : (object)inbatch.Code);
                    query.Append(", [UDMCode] = @UDMCode");
                    parameters[2] = Database.GetParameter("@UDMCode", string.IsNullOrEmpty(inbatch.UDMCode) ? DBNull.Value : (object)inbatch.UDMCode);
                    query.Append(", [NewLeads] = @NewLeads");
                    parameters[3] = Database.GetParameter("@NewLeads", inbatch.NewLeads.HasValue ? (object)inbatch.NewLeads.Value : DBNull.Value);
                    query.Append(", [UpdatedLeads] = @UpdatedLeads");
                    parameters[4] = Database.GetParameter("@UpdatedLeads", inbatch.UpdatedLeads.HasValue ? (object)inbatch.UpdatedLeads.Value : DBNull.Value);
                    query.Append(", [ContainsLatentLeads] = @ContainsLatentLeads");
                    parameters[5] = Database.GetParameter("@ContainsLatentLeads", inbatch.ContainsLatentLeads.HasValue ? (object)inbatch.ContainsLatentLeads.Value : DBNull.Value);
                    query.Append(", [Completed] = @Completed");
                    parameters[6] = Database.GetParameter("@Completed", inbatch.Completed.HasValue ? (object)inbatch.Completed.Value : DBNull.Value);
                    query.Append(", [DateReceived] = @DateReceived");
                    parameters[7] = Database.GetParameter("@DateReceived", inbatch.DateReceived.HasValue ? (object)inbatch.DateReceived.Value : DBNull.Value);
                    query.Append(", [IsArchived] = @IsArchived");
                    parameters[8] = Database.GetParameter("@IsArchived", inbatch.IsArchived.HasValue ? (object)inbatch.IsArchived.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INBatch].[ID] = @ID"); 
                    parameters[9] = Database.GetParameter("@ID", inbatch.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INBatch] ([FKINCampaignID], [Code], [UDMCode], [NewLeads], [UpdatedLeads], [ContainsLatentLeads], [Completed], [DateReceived], [IsArchived], [StampDate], [StampUserID]) VALUES(@FKINCampaignID, @Code, @UDMCode, @NewLeads, @UpdatedLeads, @ContainsLatentLeads, @Completed, @DateReceived, @IsArchived, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[9];
                    parameters[0] = Database.GetParameter("@FKINCampaignID", inbatch.FKINCampaignID.HasValue ? (object)inbatch.FKINCampaignID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@Code", string.IsNullOrEmpty(inbatch.Code) ? DBNull.Value : (object)inbatch.Code);
                    parameters[2] = Database.GetParameter("@UDMCode", string.IsNullOrEmpty(inbatch.UDMCode) ? DBNull.Value : (object)inbatch.UDMCode);
                    parameters[3] = Database.GetParameter("@NewLeads", inbatch.NewLeads.HasValue ? (object)inbatch.NewLeads.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@UpdatedLeads", inbatch.UpdatedLeads.HasValue ? (object)inbatch.UpdatedLeads.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@ContainsLatentLeads", inbatch.ContainsLatentLeads.HasValue ? (object)inbatch.ContainsLatentLeads.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@Completed", inbatch.Completed.HasValue ? (object)inbatch.Completed.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@DateReceived", inbatch.DateReceived.HasValue ? (object)inbatch.DateReceived.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@IsArchived", inbatch.IsArchived.HasValue ? (object)inbatch.IsArchived.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inbatchs that match the search criteria.
        /// </summary>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="code">The code search criteria.</param>
        /// <param name="udmcode">The udmcode search criteria.</param>
        /// <param name="newleads">The newleads search criteria.</param>
        /// <param name="updatedleads">The updatedleads search criteria.</param>
        /// <param name="containslatentleads">The containslatentleads search criteria.</param>
        /// <param name="completed">The completed search criteria.</param>
        /// <param name="datereceived">The datereceived search criteria.</param>
        /// <param name="isarchived">The isarchived search criteria.</param>
        /// <returns>A query that can be used to search for inbatchs based on the search criteria.</returns>
        internal static string Search(long? fkincampaignid, string code, string udmcode, int? newleads, int? updatedleads, bool? containslatentleads, bool? completed, DateTime? datereceived, bool? isarchived)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkincampaignid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBatch].[FKINCampaignID] = " + fkincampaignid + "");
            }
            if (code != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBatch].[Code] LIKE '" + code.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (udmcode != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBatch].[UDMCode] LIKE '" + udmcode.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (newleads != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBatch].[NewLeads] = " + newleads + "");
            }
            if (updatedleads != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBatch].[UpdatedLeads] = " + updatedleads + "");
            }
            if (containslatentleads != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBatch].[ContainsLatentLeads] = " + ((bool)containslatentleads ? "1" : "0"));
            }
            if (completed != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBatch].[Completed] = " + ((bool)completed ? "1" : "0"));
            }
            if (datereceived != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBatch].[DateReceived] = '" + datereceived.Value.ToString(Database.DateFormat) + "'");
            }
            if (isarchived != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBatch].[IsArchived] = " + ((bool)isarchived ? "1" : "0"));
            }
            query.Append("SELECT [INBatch].[ID], [INBatch].[FKINCampaignID], [INBatch].[Code], [INBatch].[UDMCode], [INBatch].[NewLeads], [INBatch].[UpdatedLeads], [INBatch].[ContainsLatentLeads], [INBatch].[Completed], [INBatch].[DateReceived], [INBatch].[IsArchived], [INBatch].[StampDate], [INBatch].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INBatch].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INBatch] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
