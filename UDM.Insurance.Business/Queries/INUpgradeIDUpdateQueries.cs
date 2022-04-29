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
    internal abstract partial class INUpgradeIDUpdateQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete.</param>
        /// <returns>A query that can be used to delete the calldata from the database.</returns>
        internal static string Delete(INUpgradeIDUpdate calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstINUpgradeIDUpdate] ([ID], [FKImportID], [IDNumber], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [IDNumber], [StampDate], [StampUserID] FROM [INUpgradeIDUpdate] WHERE [INUpgradeIDUpdate].[ID] = @ID; ";
                query += "DELETE FROM [INUpgradeIDUpdate] WHERE [INUpgradeIDUpdate].[ID] = @ID; ";
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
        internal static string DeleteHistory(INUpgradeIDUpdate calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstINUpgradeIDUpdate] WHERE [zHstINUpgradeIDUpdate].[ID] = @ID";
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
        internal static string UnDelete(INUpgradeIDUpdate calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [INUpgradeIDUpdate] ([ID], [FKImportID], [IDNumber], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [IDNUmber], [StampDate], [StampUserID] FROM [zHstINUpgradeIDUpdate] WHERE [zHstINUpgradeIDUpdate].[ID] = @ID AND [zHstINUpgradeIDUpdate].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINUpgradeIDUpdate] WHERE [zHstINUpgradeIDUpdate].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INUpgradeIDUpdate] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINUpgradeIDUpdate] WHERE [zHstINUpgradeIDUpdate].[ID] = @ID AND [zHstINUpgradeIDUpdate].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINUpgradeIDUpdate] WHERE [zHstINUpgradeIDUpdate].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INUpgradeIDUpdate] WHERE [ID] = @ID) = 0; ";
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
        internal static string Fill(INUpgradeIDUpdate calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKImportID], [IDNumber], [StampDate], [StampUserID] FROM [INUpgradeIDUpdate] WHERE [INUpgradeIDUpdate].[ID] = @ID";
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
        internal static string FillData(INUpgradeIDUpdate calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [INUpgradeIDUpdate].[ID], [INUpgradeIDUpdate].[FKImportID], [INUpgradeIDUpdate].[IDNumber], [INUpgradeIDUpdate].[StampDate], [INUpgradeIDUpdate].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INUpgradeIDUpdate].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INUpgradeIDUpdate] ");
                query.Append(" WHERE [INUpgradeIDUpdate].[ID] = @ID");
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
        internal static string FillHistory(INUpgradeIDUpdate calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKImportID], [IDNumber], [StampDate], [StampUserID] FROM [zHstINUpgradeIDUpdate] WHERE [zHstINUpgradeIDUpdate].[ID] = @ID AND [zHstINUpgradeIDUpdate].[StampUserID] = @StampUserID AND [zHstINUpgradeIDUpdate].[StampDate] = @StampDate";
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
            query.Append("SELECT [INUpgradeIDUpdate].[ID], [INUpgradeIDUpdate].[FKImportID], [INUpgradeIDUpdate].[IDNumber], [INUpgradeIDUpdate].[StampDate], [INUpgradeIDUpdate].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INUpgradeIDUpdate].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INUpgradeIDUpdate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted calldatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINUpgradeIDUpdate].[ID], [zHstINUpgradeIDUpdate].[FKImportID], [zHstINUpgradeIDUpdate].[IDNumber], [zHstINUpgradeIDUpdate].[StampDate], [zHstINUpgradeIDUpdate].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINUpgradeIDUpdate].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINUpgradeIDUpdate] ");
            query.Append("INNER JOIN (SELECT [zHstINUpgradeIDUpdate].[ID], MAX([zHstINUpgradeIDUpdate].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINUpgradeIDUpdate] ");
            query.Append("WHERE [zHstINUpgradeIDUpdate].[ID] NOT IN (SELECT [INUpgradeIDUpdate].[ID] FROM [INUpgradeIDUpdate]) ");
            query.Append("GROUP BY [zHstINUpgradeIDUpdate].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINUpgradeIDUpdate].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINUpgradeIDUpdate].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) calldata in the database.
        /// </summary>
        /// <param name="calldata">The calldata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) calldata in the database.</returns>
        public static string ListHistory(INUpgradeIDUpdate calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstINUpgradeIDUpdate].[ID], [zHstINUpgradeIDUpdate].[FKImportID], [zHstINUpgradeIDUpdate].[IDNumber], [zHstINUpgradeIDUpdate].[StampDate], [zHstINUpgradeIDUpdate].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINUpgradeIDUpdate].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINUpgradeIDUpdate] ");
                query.Append(" WHERE [zHstINUpgradeIDUpdate].[ID] = @ID");
                query.Append(" ORDER BY [zHstINUpgradeIDUpdate].[StampDate] DESC");
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
        internal static string Save(INUpgradeIDUpdate calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINUpgradeIDUpdate] ([ID], [FKImportID], [IDNumber], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [IDNumber], [StampDate], [StampUserID] FROM [INUpgradeIDUpdate] WHERE [INUpgradeIDUpdate].[ID] = @ID; ");
                    query.Append("UPDATE [INUpgradeIDUpdate]");
                    parameters = new object[3];
                    query.Append(" SET [FKImportID] = @FKImportID");
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKImportID.HasValue ? (object)calldata.FKImportID.Value : DBNull.Value);
                    query.Append(", [IDNumber] = @IDNumber");
                    parameters[1] = Database.GetParameter("@IDNumber", string.IsNullOrEmpty(calldata.IDNumber) ? DBNull.Value : (object)calldata.IDNumber);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INUpgradeIDUpdate].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INUpgradeIDUpdate] ([FKImportID], [IDNumber], [StampDate], [StampUserID]) VALUES(@FKImportID, @IDNumber, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKImportID.HasValue ? (object)calldata.FKImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@IDNumber", string.IsNullOrEmpty(calldata.IDNumber) ? DBNull.Value : (object)calldata.IDNumber);
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
        internal static string Search(long? fkimportid, string idnumber)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUpgradeIDUpdate].[FKImportID] = " + fkimportid + "");
            }
            if (idnumber != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUpgradeIDUpdate].[IDNumber] LIKE '" + idnumber.Replace("'", "''").Replace("*", "%") + "'");
            }

            query.Append("SELECT [INUpgradeIDUpdate].[ID], [INUpgradeIDUpdate].[FKImportID], [INUpgradeIDUpdate].[IDNumber], [INUpgradeIDUpdate].[StampDate], [INUpgradeIDUpdate].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INUpgradeIDUpdate].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INUpgradeIDUpdate] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
