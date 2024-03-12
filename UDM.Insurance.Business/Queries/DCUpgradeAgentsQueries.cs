using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to calldata objects.
    /// </summary>
    internal abstract partial class DCUpgradeAgentsQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete.</param>
        /// <returns>A query that can be used to delete the calldata from the database.</returns>
        internal static string Delete(DCUpgradeAgents calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstDCUpgradeAgents] ([ID], [FkUserID], [IsUpgrade], [StampDate], [StampUserID]) SELECT [ID], [FkUserID], [IsUpgrade], [StampDate], [StampUserID] FROM [DCUpgradeAgents] WHERE [DCUpgradeAgents].[ID] = @ID; ";
                query += "DELETE FROM [DCUpgradeAgents] WHERE [DCUpgradeAgents].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the calldata from the database.</returns>
        internal static string DeleteHistory(DCUpgradeAgents calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstDCUpgradeAgents] WHERE [zHstDCUpgradeAgents].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to undelete.</param>
        /// <returns>A query that can be used to undelete the calldata from the database.</returns>
        internal static string UnDelete(DCUpgradeAgents calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [DCUpgradeAgents] ([ID], [FkUserID], [IsUpgrade], [StampDate], [StampUserID]) SELECT [ID], [FkUserID], [IsUpgrade], [StampDate], [StampUserID] FROM [zHstDCUpgradeAgents] WHERE [zHstDCUpgradeAgents].[ID] = @ID AND [zHstDCUpgradeAgents].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstDCUpgradeAgents] WHERE [zHstDCUpgradeAgents].[ID] = @ID) AND (SELECT COUNT(ID) FROM [DCUpgradeAgents] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstDCUpgradeAgents] WHERE [zHstDCUpgradeAgents].[ID] = @ID AND [zHstDCUpgradeAgents].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstDCUpgradeAgents] WHERE [zHstDCUpgradeAgents].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [DCUpgradeAgents] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an calldata object.
        /// </summary>
        /// <param name="calldata">The calldata object to fill.</param>
        /// <returns>A query that can be used to fill the calldata object.</returns>
        internal static string Fill(DCUpgradeAgents calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FkUserID], [IsUpgrade], [StampDate], [StampUserID] FROM [DCUpgradeAgents] WHERE [DCUpgradeAgents].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  calldata data.
        /// </summary>
        /// <param name="calldata">The calldata to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  calldata data.</returns>
        internal static string FillData(DCUpgradeAgents calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [DCUpgradeAgents].[ID], [DCUpgradeAgents].[FkUserID], [DCUpgradeAgents].[IsUpgrade], [DCUpgradeAgents].[StampDate], [DCUpgradeAgents].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [DCUpgradeAgents].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [DCUpgradeAgents] ");
                query.Append(" WHERE [DCUpgradeAgents].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an calldata object from history.
        /// </summary>
        /// <param name="calldata">The calldata object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the calldata object from history.</returns>
        internal static string FillHistory(DCUpgradeAgents calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FkUserID], [IsUpgrade], [StampDate], [StampUserID] FROM [zHstDCUpgradeAgents] WHERE [zHstDCUpgradeAgents].[ID] = @ID AND [zHstDCUpgradeAgents].[StampUserID] = @StampUserID AND [zHstDCUpgradeAgents].[StampDate] = @StampDate";
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
        /// Lists all the calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the calldatas in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [DCUpgradeAgents].[ID], [DCUpgradeAgents].[FkUserID], [DCUpgradeAgents].[IsUpgrade], [DCUpgradeAgents].[StampDate], [DCUpgradeAgents].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [DCUpgradeAgents].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [DCUpgradeAgents] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted calldatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstDCUpgradeAgents].[ID], [zHstDCUpgradeAgents].[FkUserID], [zHstDCUpgradeAgents].[IsUpgrade], [zHstDCUpgradeAgents].[StampDate], [zHstDCUpgradeAgents].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstDCUpgradeAgents].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstDCUpgradeAgents] ");
            query.Append("INNER JOIN (SELECT [zHstDCUpgradeAgents].[ID], MAX([zHstDCUpgradeAgents].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstDCUpgradeAgents] ");
            query.Append("WHERE [zHstDCUpgradeAgents].[ID] NOT IN (SELECT [DCUpgradeAgents].[ID] FROM [DCUpgradeAgents]) ");
            query.Append("GROUP BY [zHstDCUpgradeAgents].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstDCUpgradeAgents].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstDCUpgradeAgents].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) calldata in the database.
        /// </summary>
        /// <param name="calldata">The calldata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) calldata in the database.</returns>
        public static string ListHistory(DCUpgradeAgents calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstDCUpgradeAgents].[ID], [zHstDCUpgradeAgents].[FkUserID], [zHstDCUpgradeAgents].[IsUpgrade], [zHstDCUpgradeAgents].[StampDate], [zHstDCUpgradeAgents].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstDCUpgradeAgents].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstDCUpgradeAgents] ");
                query.Append(" WHERE [zHstDCUpgradeAgents].[ID] = @ID");
                query.Append(" ORDER BY [zHstDCUpgradeAgents].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) calldata to the database.
        /// </summary>
        /// <param name="calldata">The calldata to save.</param>
        /// <returns>A query that can be used to save the calldata to the database.</returns>
        internal static string Save(DCUpgradeAgents calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstDCUpgradeAgents] ([ID], [FkUserID], [IsUpgrade], [StampDate], [StampUserID]) SELECT [ID], [FkUserID], [IsUpgrade], [StampDate], [StampUserID] FROM [DCUpgradeAgents] WHERE [DCUpgradeAgents].[ID] = @ID; ");
                    query.Append("UPDATE [DCUpgradeAgents]");
                    parameters = new object[3];
                    query.Append(" SET [FkUserID] = @FkUserID");
                    parameters[0] = Database.GetParameter("@FkUSerID", calldata.FkUserID.HasValue ? (object)calldata.FkUserID.Value : DBNull.Value);
                    query.Append(", [IsUpgrade] = @IsUpgrade");
                    parameters[1] = Database.GetParameter("@IsUpgrade", string.IsNullOrEmpty(calldata.IsUpgrade) ? DBNull.Value : (object)calldata.IsUpgrade);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [DCUpgradeAgents].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [DCUpgradeAgents] ([FkUserID], [IsUpgrade], [StampDate], [StampUserID]) VALUES(@FkUserID, @IsUpgrade, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FkUserID", calldata.FkUserID.HasValue ? (object)calldata.FkUserID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@IsUpgrade", string.IsNullOrEmpty(calldata.IsUpgrade) ? DBNull.Value : (object)calldata.IsUpgrade);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for calldatas that match the search criteria.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="number">The number search criteria.</param>
        /// <param name="extension">The extension search criteria.</param>
        /// <param name="recref">The recref search criteria.</param>
        /// <returns>A query that can be used to search for calldatas based on the search criteria.</returns>
        internal static string Search(long? fkuserid, string isupgrade)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[DCUpgradeAgents].[FkUserID] = " + fkuserid + "");
            }
            if (isupgrade != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[DCUpgradeAgents].[IsUpgrade] LIKE '" + isupgrade.Replace("'", "''").Replace("*", "%") + "'");
            }

            query.Append("SELECT [DCUpgradeAgents].[ID], [DCUpgradeAgents].[FkUserID], [DCUpgradeAgents].[IsUpgrade], [DCUpgradeAgents].[StampDate], [DCUpgradeAgents].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [DCUpgradeAgents].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [DCUpgradeAgents] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
