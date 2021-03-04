using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inbumpupoption objects.
    /// </summary>
    internal abstract partial class INBumpUpOptionQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inbumpupoption from the database.
        /// </summary>
        /// <param name="inbumpupoption">The inbumpupoption object to delete.</param>
        /// <returns>A query that can be used to delete the inbumpupoption from the database.</returns>
        internal static string Delete(INBumpUpOption inbumpupoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbumpupoption != null)
            {
                query = "INSERT INTO [zHstINBumpUpOption] ([ID], [Description], [ImportCode], [FKINCampaignTypeID]) SELECT [ID], [Description], [ImportCode], [FKINCampaignTypeID] FROM [INBumpUpOption] WHERE [INBumpUpOption].[ID] = @ID; ";
                query += "DELETE FROM [INBumpUpOption] WHERE [INBumpUpOption].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbumpupoption.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inbumpupoption from the database.
        /// </summary>
        /// <param name="inbumpupoption">The inbumpupoption object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inbumpupoption from the database.</returns>
        internal static string DeleteHistory(INBumpUpOption inbumpupoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbumpupoption != null)
            {
                query = "DELETE FROM [zHstINBumpUpOption] WHERE [zHstINBumpUpOption].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbumpupoption.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inbumpupoption from the database.
        /// </summary>
        /// <param name="inbumpupoption">The inbumpupoption object to undelete.</param>
        /// <returns>A query that can be used to undelete the inbumpupoption from the database.</returns>
        internal static string UnDelete(INBumpUpOption inbumpupoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbumpupoption != null)
            {
                query = "INSERT INTO [INBumpUpOption] ([ID], [Description], [ImportCode], [FKINCampaignTypeID]) SELECT [ID], [Description], [ImportCode], [FKINCampaignTypeID] FROM [zHstINBumpUpOption] WHERE [zHstINBumpUpOption].[ID] = @ID AND [zHstINBumpUpOption].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINBumpUpOption] WHERE [zHstINBumpUpOption].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INBumpUpOption] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINBumpUpOption] WHERE [zHstINBumpUpOption].[ID] = @ID AND [zHstINBumpUpOption].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINBumpUpOption] WHERE [zHstINBumpUpOption].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INBumpUpOption] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbumpupoption.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inbumpupoption object.
        /// </summary>
        /// <param name="inbumpupoption">The inbumpupoption object to fill.</param>
        /// <returns>A query that can be used to fill the inbumpupoption object.</returns>
        internal static string Fill(INBumpUpOption inbumpupoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbumpupoption != null)
            {
                query = "SELECT [ID], [Description], [ImportCode], [FKINCampaignTypeID] FROM [INBumpUpOption] WHERE [INBumpUpOption].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbumpupoption.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inbumpupoption data.
        /// </summary>
        /// <param name="inbumpupoption">The inbumpupoption to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inbumpupoption data.</returns>
        internal static string FillData(INBumpUpOption inbumpupoption, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inbumpupoption != null)
            {
            query.Append("SELECT [INBumpUpOption].[ID], [INBumpUpOption].[Description], [INBumpUpOption].[ImportCode], [INBumpUpOption].[FKINCampaignTypeID]");
            query.Append(" FROM [INBumpUpOption] ");
                query.Append(" WHERE [INBumpUpOption].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbumpupoption.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inbumpupoption object from history.
        /// </summary>
        /// <param name="inbumpupoption">The inbumpupoption object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inbumpupoption object from history.</returns>
        internal static string FillHistory(INBumpUpOption inbumpupoption, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbumpupoption != null)
            {
                query = "SELECT [ID], [Description], [ImportCode], [FKINCampaignTypeID] FROM [zHstINBumpUpOption] WHERE [zHstINBumpUpOption].[ID] = @ID AND [zHstINBumpUpOption].[StampUserID] = @StampUserID AND [zHstINBumpUpOption].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inbumpupoption.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inbumpupoptions in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inbumpupoptions in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INBumpUpOption].[ID], [INBumpUpOption].[Description], [INBumpUpOption].[ImportCode], [INBumpUpOption].[FKINCampaignTypeID]");
            query.Append(" FROM [INBumpUpOption] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inbumpupoptions in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inbumpupoptions in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINBumpUpOption].[ID], [zHstINBumpUpOption].[Description], [zHstINBumpUpOption].[ImportCode], [zHstINBumpUpOption].[FKINCampaignTypeID]");
            query.Append(" FROM [zHstINBumpUpOption] ");
            query.Append("INNER JOIN (SELECT [zHstINBumpUpOption].[ID], MAX([zHstINBumpUpOption].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINBumpUpOption] ");
            query.Append("WHERE [zHstINBumpUpOption].[ID] NOT IN (SELECT [INBumpUpOption].[ID] FROM [INBumpUpOption]) ");
            query.Append("GROUP BY [zHstINBumpUpOption].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINBumpUpOption].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINBumpUpOption].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inbumpupoption in the database.
        /// </summary>
        /// <param name="inbumpupoption">The inbumpupoption object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inbumpupoption in the database.</returns>
        public static string ListHistory(INBumpUpOption inbumpupoption, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inbumpupoption != null)
            {
            query.Append("SELECT [zHstINBumpUpOption].[ID], [zHstINBumpUpOption].[Description], [zHstINBumpUpOption].[ImportCode], [zHstINBumpUpOption].[FKINCampaignTypeID]");
            query.Append(" FROM [zHstINBumpUpOption] ");
                query.Append(" WHERE [zHstINBumpUpOption].[ID] = @ID");
                query.Append(" ORDER BY [zHstINBumpUpOption].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbumpupoption.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inbumpupoption to the database.
        /// </summary>
        /// <param name="inbumpupoption">The inbumpupoption to save.</param>
        /// <returns>A query that can be used to save the inbumpupoption to the database.</returns>
        internal static string Save(INBumpUpOption inbumpupoption, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inbumpupoption != null)
            {
                if (inbumpupoption.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINBumpUpOption] ([ID], [Description], [ImportCode], [FKINCampaignTypeID]) SELECT [ID], [Description], [ImportCode], [FKINCampaignTypeID] FROM [INBumpUpOption] WHERE [INBumpUpOption].[ID] = @ID; ");
                    query.Append("UPDATE [INBumpUpOption]");
                    parameters = new object[4];
                    query.Append(" SET [Description] = @Description");
                    parameters[0] = Database.GetParameter("@Description", string.IsNullOrEmpty(inbumpupoption.Description) ? DBNull.Value : (object)inbumpupoption.Description);
                    query.Append(", [ImportCode] = @ImportCode");
                    parameters[1] = Database.GetParameter("@ImportCode", string.IsNullOrEmpty(inbumpupoption.ImportCode) ? DBNull.Value : (object)inbumpupoption.ImportCode);
                    query.Append(", [FKINCampaignTypeID] = @FKINCampaignTypeID");
                    parameters[2] = Database.GetParameter("@FKINCampaignTypeID", inbumpupoption.FKINCampaignTypeID.HasValue ? (object)inbumpupoption.FKINCampaignTypeID.Value : DBNull.Value);
                    query.Append(" WHERE [INBumpUpOption].[ID] = @ID"); 
                    parameters[3] = Database.GetParameter("@ID", inbumpupoption.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INBumpUpOption] ([Description], [ImportCode], [FKINCampaignTypeID]) VALUES(@Description, @ImportCode, @FKINCampaignTypeID);");
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@Description", string.IsNullOrEmpty(inbumpupoption.Description) ? DBNull.Value : (object)inbumpupoption.Description);
                    parameters[1] = Database.GetParameter("@ImportCode", string.IsNullOrEmpty(inbumpupoption.ImportCode) ? DBNull.Value : (object)inbumpupoption.ImportCode);
                    parameters[2] = Database.GetParameter("@FKINCampaignTypeID", inbumpupoption.FKINCampaignTypeID.HasValue ? (object)inbumpupoption.FKINCampaignTypeID.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inbumpupoptions that match the search criteria.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="importcode">The importcode search criteria.</param>
        /// <param name="fkincampaigntypeid">The fkincampaigntypeid search criteria.</param>
        /// <returns>A query that can be used to search for inbumpupoptions based on the search criteria.</returns>
        internal static string Search(string description, string importcode, long? fkincampaigntypeid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (description != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBumpUpOption].[Description] LIKE '" + description.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (importcode != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBumpUpOption].[ImportCode] LIKE '" + importcode.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkincampaigntypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBumpUpOption].[FKINCampaignTypeID] = " + fkincampaigntypeid + "");
            }
            query.Append("SELECT [INBumpUpOption].[ID], [INBumpUpOption].[Description], [INBumpUpOption].[ImportCode], [INBumpUpOption].[FKINCampaignTypeID]");
            query.Append(" FROM [INBumpUpOption] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
