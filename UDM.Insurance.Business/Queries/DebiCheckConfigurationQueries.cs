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
    internal abstract partial class DebiCheckConfigurationQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete.</param>
        /// <returns>A query that can be used to delete the calldata from the database.</returns>
        internal static string Delete(DebiCheckConfiguration calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstDebiCheckConfiguration] ([ID], [DebiCheckPower], [StampDate], [StampUserID]) SELECT [ID], [DebiCheckPower], [StampDate], [StampUserID] FROM [DebiCheckConfiguration] WHERE [DebiCheckConfiguration].[ID] = @ID; ";
                query += "DELETE FROM [DebiCheckConfiguration] WHERE [DebiCheckConfiguration].[ID] = @ID; ";
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
        internal static string DeleteHistory(DebiCheckConfiguration calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstDebiCheckConfiguration] WHERE [zHstDebiCheckConfiguration].[ID] = @ID";
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
        internal static string UnDelete(DebiCheckConfiguration calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [DebiCheckConfiguration] ([ID], [DebiCheckPower], [StampDate], [StampUserID]) SELECT [ID], [DebiCheckPower], [StampDate], [StampUserID] FROM [zHstDebiCheckConfigurationa] WHERE [zHstDebiCheckConfiguration].[ID] = @ID AND [zHstDebiCheckConfiguration].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstDebiCheckConfiguration] WHERE [zHstDebiCheckConfiguration].[ID] = @ID) AND (SELECT COUNT(ID) FROM [DebiCheckConfiguration] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstDebiCheckConfiguration] WHERE [zHstDebiCheckConfiguration].[ID] = @ID AND [zHstDebiCheckConfiguration].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstDebiCheckConfiguration] WHERE [zHstDebiCheckConfiguration].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [DebiCheckConfiguration] WHERE [ID] = @ID) = 0; ";
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
        internal static string Fill(DebiCheckConfiguration calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [DebiCheckPower], [StampDate], [StampUserID] FROM [DebiCheckConfiguration] WHERE [DebiCheckConfiguration].[ID] = @ID";
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
        internal static string FillData(DebiCheckConfiguration calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [DebiCheckConfiguration].[ID], [DebiCheckConfiguration].[DebiCheckPower], [DebiCheckConfiguration].[StampDate], [DebiCheckConfiguration].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [DebiCheckConfiguration].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [DebiCheckConfiguration] ");
                query.Append(" WHERE [DebiCheckConfiguration].[ID] = @ID");
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
        internal static string FillHistory(DebiCheckConfiguration calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [DebiCheckPower], [StampDate], [StampUserID] FROM [zHstDebiCheckConfiguration] WHERE [zHstDebiCheckConfiguration].[ID] = @ID AND [zHstDebiCheckConfiguration].[StampUserID] = @StampUserID AND [zHstDebiCheckConfiguration].[StampDate] = @StampDate";
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
            query.Append("SELECT [DebiCheckConfiguration].[ID], [DebiCheckConfiguration].[DebiCheckPower], [DebiCheckConfiguration].[StampDate], [DebiCheckConfiguration].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [DebiCheckConfiguration].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [DebiCheckConfiguration] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted calldatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstDebiCheckConfiguration].[ID], [zHstDebiCheckConfiguration].[DebiCheckPower], [zHstDebiCheckConfiguration].[StampDate], [zHstDebiCheckConfiguration].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstDebiCheckConfiguration].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstDebiCheckConfiguration] ");
            query.Append("INNER JOIN (SELECT [zHstDebiCheckConfiguration].[ID], MAX([zHstDebiCheckConfiguration].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstDebiCheckConfiguration] ");
            query.Append("WHERE [zHstDebiCheckConfiguration].[ID] NOT IN (SELECT [DebiCheckConfiguration].[ID] FROM [DebiCheckConfiguration]) ");
            query.Append("GROUP BY [zHstDebiCheckConfiguration].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstDebiCheckConfiguration].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstDebiCheckConfiguration].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) calldata in the database.
        /// </summary>
        /// <param name="calldata">The calldata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) calldata in the database.</returns>
        public static string ListHistory(DebiCheckConfiguration calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstDebiCheckConfiguration].[ID], [zHstDebiCheckConfiguration].[DebiCheckPower], [zHstDebiCheckConfiguration].[StampDate], [zHstDebiCheckConfiguration].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstDebiCheckConfiguration].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstDebiCheckConfiguration] ");
                query.Append(" WHERE [zHstDebiCheckConfiguration].[ID] = @ID");
                query.Append(" ORDER BY [zHstDebiCheckConfiguration].[StampDate] DESC");
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
        internal static string Save(DebiCheckConfiguration calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstDebiCheckConfiguration] ([ID], [DebiCheckPower], [StampDate], [StampUserID]) SELECT [ID], [DebiCheckPower], [StampDate], [StampUserID] FROM [DebiCheckConfiguration] WHERE [DebiCheckConfiguration].[ID] = @ID; ");
                    query.Append("UPDATE [DebiCheckConfiguration]");
                    parameters = new object[2];
                    query.Append(" SET [DebiCheckPower] = @DebiCheckPower");
                    parameters[0] = Database.GetParameter("@DebiCheckPower", calldata.DebiCheckPower.HasValue ? (object)calldata.DebiCheckPower.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [DebiCheckConfiguration].[ID] = @ID"); 
                    parameters[1] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [DebiCheckConfiguration] ([DebiCheckPower],  [StampDate], [StampUserID]) VALUES(@DebiCheckPower, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@DebiCheckPower", calldata.DebiCheckPower.HasValue ? (object)calldata.DebiCheckPower.Value : DBNull.Value);

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
        internal static string Search(int? debicheckpower)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (debicheckpower != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[DebiCheckConfiguration].[DebiCheckPower] = " + debicheckpower + "");
            }

            query.Append("SELECT [DebiCheckConfiguration].[ID], [DebiCheckConfiguration].[DebiCheckPower], [DebiCheckConfiguration].[StampDate], [DebiCheckConfiguration].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [DebiCheckConfiguration].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [DebiCheckConfiguration] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
