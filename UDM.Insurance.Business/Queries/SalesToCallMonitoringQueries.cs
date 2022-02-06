using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to SalesToCallMonitoring objects.
    /// </summary>
    internal abstract partial class SalesToCallMonitoringQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) SalesToCallMonitoring from the database.
        /// </summary>
        /// <param name="SalesToCallMonitoring">The SalesToCallMonitoring object to delete.</param>
        /// <returns>A query that can be used to delete the SalesToCallMonitoring from the database.</returns>
        internal static string Delete(SalesToCallMonitoring calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstINSalesToCallMonitoring] ([ID], [FKImportID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID] FROM [INSalesToCallMonitoring] WHERE [INSalesToCallMonitoring].[ID] = @ID; ";
                query += "DELETE FROM [INSalesToCallMonitoring] WHERE [INSalesToCallMonitoring].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) SalesToCallMonitoring from the database.
        /// </summary>
        /// <param name="SalesToCallMonitoring">The SalesToCallMonitoring object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the SalesToCallMonitoring from the database.</returns>
        internal static string DeleteHistory(SalesToCallMonitoring calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstINSalesToCallMonitoring] WHERE [zHstINSalesToCallMonitoring].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) SalesToCallMonitoring from the database.
        /// </summary>
        /// <param name="SalesToCallMonitoring">The SalesToCallMonitoring object to undelete.</param>
        /// <returns>A query that can be used to undelete the SalesToCallMonitoring from the database.</returns>
        internal static string UnDelete(SalesToCallMonitoring calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [INSalesToCallMonitoring] ([ID], [FKImportID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID] FROM [zHstINSalesToCallMonitoring] WHERE [zHstINSalesToCallMonitoring].[ID] = @ID AND [zHstINSalesToCallMonitoring].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINSalesToCallMonitoring] WHERE [zHstINSalesToCallMonitoring].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INSalesToCallMonitoring] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINSalesToCallMonitoring] WHERE [zHstINSalesToCallMonitoring].[ID] = @ID AND [zHstINSalesToCallMonitoring].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINSalesToCallMonitoring] WHERE [zHstINSalesToCallMonitoring].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INSalesToCallMonitoring] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an SalesToCallMonitoring object.
        /// </summary>
        /// <param name="SalesToCallMonitoring">The SalesToCallMonitoring object to fill.</param>
        /// <returns>A query that can be used to fill the SalesToCallMonitoring object.</returns>
        internal static string Fill(SalesToCallMonitoring calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKImportID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID] FROM [INSalesToCallMonitoring] WHERE [INSalesToCallMonitoring].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  SalesToCallMonitoring data.
        /// </summary>
        /// <param name="SalesToCallMonitoring">The SalesToCallMonitoring to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  SalesToCallMonitoring data.</returns>
        internal static string FillData(SalesToCallMonitoring calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [INSalesToCallMonitoring].[ID], [INSalesToCallMonitoring].[FKImportID], [INSalesToCallMonitoring].[FKUserID], [INSalesToCallMonitoring].[IsDisplayed], [INSalesToCallMonitoring].[StampDate], [INSalesToCallMonitoring].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INSalesToCallMonitoring].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INSalesToCallMonitoring] ");
                query.Append(" WHERE [INSalesToCallMonitoring].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an SalesToCallMonitoring object from history.
        /// </summary>
        /// <param name="SalesToCallMonitoring">The SalesToCallMonitoring object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the SalesToCallMonitoring object from history.</returns>
        internal static string FillHistory(SalesToCallMonitoring calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKImportID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID] FROM [zHstINSalesToCallMonitoring] WHERE [zHstINSalesToCallMonitoring].[ID] = @ID AND [zHstINSalesToCallMonitoring].[StampUserID] = @StampUserID AND [zHstINSalesToCallMonitoring].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the SalesToCallMonitoring in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the SalesToCallMonitoring in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INSalesToCallMonitoring].[ID], [INSalesToCallMonitoring].[FKImportID], [INSalesToCallMonitoring].[FKUserID], [INSalesToCallMonitoring].[IsDisplayed], [INSalesToCallMonitoring].[StampDate], [INSalesToCallMonitoring].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INSalesToCallMonitoring].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INSalesToCallMonitoring] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted SalesToCallMonitoring in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted SalesToCallMonitoring in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINSalesToCallMonitoring].[ID], [zHstINSalesToCallMonitoring].[FKImportID], [zHstINSalesToCallMonitoring].[FKUserID], [zHstINSalesToCallMonitoring].[IsDisplayed], [zHstINSalesToCallMonitoring].[StampDate], [zHstINSalesToCallMonitoring].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINSalesToCallMonitoring].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINSalesToCallMonitoring] ");
            query.Append("INNER JOIN (SELECT [zHstINSalesToCallMonitoring].[ID], MAX([zHstINSalesToCallMonitoring].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINSalesToCallMonitoring] ");
            query.Append("WHERE [zHstINSalesToCallMonitoring].[ID] NOT IN (SELECT [INSalesToCallMonitoring].[ID] FROM [INSalesToCallMonitoring]) ");
            query.Append("GROUP BY [zHstINSalesToCallMonitoring].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINSalesToCallMonitoring].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINSalesToCallMonitoring].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) SalesToCallMonitoring in the database.
        /// </summary>
        /// <param name="SalesToCallMonitoring">The SalesToCallMonitoring object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) SalesToCallMonitoring in the database.</returns>
        public static string ListHistory(SalesToCallMonitoring calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstINSalesToCallMonitoring].[ID], [zHstINSalesToCallMonitoring].[FKImportID], [zHstINSalesToCallMonitoring].[FKUserID], [zHstINSalesToCallMonitoring].[IsDisplayed], [zHstINSalesToCallMonitoring].[StampDate], [zHstINSalesToCallMonitoring].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINSalesToCallMonitoring].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINSalesToCallMonitoring] ");
                query.Append(" WHERE [zHstINSalesToCallMonitoring].[ID] = @ID");
                query.Append(" ORDER BY [zHstINSalesToCallMonitoring].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) SalesToCallMonitoring to the database.
        /// </summary>
        /// <param name="SalesToCallMonitoring">The SalesToCallMonitoring to save.</param>
        /// <returns>A query that can be used to save the SalesToCallMonitoring to the database.</returns>
        internal static string Save(SalesToCallMonitoring calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINSalesToCallMonitoring] ([ID], [FKImportID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID] FROM [INSalesToCallMonitoring] WHERE [INSalesToCallMonitoring].[ID] = @ID; ");
                    query.Append("UPDATE [INSalesToCallMonitoring]");
                    parameters = new object[4];
                    query.Append(" SET [FKImportID] = @FKImportID");
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKImportID.HasValue ? (object)calldata.FKImportID.Value : DBNull.Value);
                    query.Append(", [FKUserID] = @FKUserID");
                    parameters[1] = Database.GetParameter("@FKUserID", calldata.FKUserID.HasValue ? (object)calldata.FKUserID.Value : DBNull.Value);
                    query.Append(", [IsDisplayed] = @IsDisplayed");
                    parameters[2] = Database.GetParameter("@IsDisplayed", string.IsNullOrEmpty(calldata.IsDisplayed) ? DBNull.Value : (object)calldata.IsDisplayed);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INSalesToCallMonitoring].[ID] = @ID"); 
                    parameters[3] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INSalesToCallMonitoring] ([FKImportID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID]) VALUES(@FKImportID, @FKUserID, @IsDisplayed, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKImportID.HasValue ? (object)calldata.FKImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKUserID", calldata.FKUserID.HasValue ? (object)calldata.FKUserID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@IsDisplayed", string.IsNullOrEmpty(calldata.IsDisplayed) ? DBNull.Value : (object)calldata.IsDisplayed);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for SalesToCallMonitoring that match the search criteria.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="number">The number search criteria.</param>
        /// <param name="extension">The extension search criteria.</param>
        /// <param name="recref">The recref search criteria.</param>
        /// <returns>A query that can be used to search for SalesToCallMonitoring based on the search criteria.</returns>
        internal static string Search(long? fkimportid, long? fkuserid, string isdisplayed)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INSalesToCallMonitoring].[FKImportID] = " + fkimportid + "");
            }
            if (fkuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INSalesToCallMonitoring].[FKUserID] = " + fkuserid + "");
            }
            if (isdisplayed != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INSalesToCallMonitoring].[IsDisplayed] LIKE '" + isdisplayed.Replace("'", "''").Replace("*", "%") + "'");
            }

            query.Append("SELECT [INSalesToCallMonitoring].[ID], [INSalesToCallMonitoring].[FKImportID], [INSalesToCallMonitoring].[FKUserID], [INSalesToCallMonitoring].[IsDislayed], [INSalesToCallMonitoring].[StampDate], [INSalesToCallMonitoring].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INSalesToCallMonitoring].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INSalesToCallMonitoring] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
