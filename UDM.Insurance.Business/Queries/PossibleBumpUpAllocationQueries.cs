using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to possiblebumpupallocation objects.
    /// </summary>
    internal abstract partial class PossibleBumpUpAllocationQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) possiblebumpupallocation from the database.
        /// </summary>
        /// <param name="possiblebumpupallocation">The possiblebumpupallocation object to delete.</param>
        /// <returns>A query that can be used to delete the possiblebumpupallocation from the database.</returns>
        internal static string Delete(PossibleBumpUpAllocation possiblebumpupallocation, ref object[] parameters)
        {
            string query = string.Empty;
            if (possiblebumpupallocation != null)
            {
                query = "INSERT INTO [zHstPossibleBumpUpAllocation] ([ID], [FKINImportID], [FKUserID], [CallBackDate], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKUserID], [CallBackDate], [StampDate], [StampUserID] FROM [PossibleBumpUpAllocation] WHERE [PossibleBumpUpAllocation].[ID] = @ID; ";
                query += "DELETE FROM [PossibleBumpUpAllocation] WHERE [PossibleBumpUpAllocation].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", possiblebumpupallocation.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) possiblebumpupallocation from the database.
        /// </summary>
        /// <param name="possiblebumpupallocation">The possiblebumpupallocation object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the possiblebumpupallocation from the database.</returns>
        internal static string DeleteHistory(PossibleBumpUpAllocation possiblebumpupallocation, ref object[] parameters)
        {
            string query = string.Empty;
            if (possiblebumpupallocation != null)
            {
                query = "DELETE FROM [zHstPossibleBumpUpAllocation] WHERE [zHstPossibleBumpUpAllocation].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", possiblebumpupallocation.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) possiblebumpupallocation from the database.
        /// </summary>
        /// <param name="possiblebumpupallocation">The possiblebumpupallocation object to undelete.</param>
        /// <returns>A query that can be used to undelete the possiblebumpupallocation from the database.</returns>
        internal static string UnDelete(PossibleBumpUpAllocation possiblebumpupallocation, ref object[] parameters)
        {
            string query = string.Empty;
            if (possiblebumpupallocation != null)
            {
                query = "INSERT INTO [PossibleBumpUpAllocation] ([ID], [FKINImportID], [FKUserID], [CallBackDate], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKUserID], [CallBackDate], [StampDate], [StampUserID] FROM [zHstPossibleBumpUpAllocation] WHERE [zHstPossibleBumpUpAllocation].[ID] = @ID AND [zHstPossibleBumpUpAllocation].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstPossibleBumpUpAllocation] WHERE [zHstPossibleBumpUpAllocation].[ID] = @ID) AND (SELECT COUNT(ID) FROM [PossibleBumpUpAllocation] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstPossibleBumpUpAllocation] WHERE [zHstPossibleBumpUpAllocation].[ID] = @ID AND [zHstPossibleBumpUpAllocation].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstPossibleBumpUpAllocation] WHERE [zHstPossibleBumpUpAllocation].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [PossibleBumpUpAllocation] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", possiblebumpupallocation.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an possiblebumpupallocation object.
        /// </summary>
        /// <param name="possiblebumpupallocation">The possiblebumpupallocation object to fill.</param>
        /// <returns>A query that can be used to fill the possiblebumpupallocation object.</returns>
        internal static string Fill(PossibleBumpUpAllocation possiblebumpupallocation, ref object[] parameters)
        {
            string query = string.Empty;
            if (possiblebumpupallocation != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKUserID], [CallBackDate], [StampDate], [StampUserID] FROM [PossibleBumpUpAllocation] WHERE [PossibleBumpUpAllocation].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", possiblebumpupallocation.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  possiblebumpupallocation data.
        /// </summary>
        /// <param name="possiblebumpupallocation">The possiblebumpupallocation to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  possiblebumpupallocation data.</returns>
        internal static string FillData(PossibleBumpUpAllocation possiblebumpupallocation, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (possiblebumpupallocation != null)
            {
            query.Append("SELECT [PossibleBumpUpAllocation].[ID], [PossibleBumpUpAllocation].[FKINImportID], [PossibleBumpUpAllocation].[FKUserID], [PossibleBumpUpAllocation].[CallBackDate], [PossibleBumpUpAllocation].[StampDate], [PossibleBumpUpAllocation].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [PossibleBumpUpAllocation].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [PossibleBumpUpAllocation] ");
                query.Append(" WHERE [PossibleBumpUpAllocation].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", possiblebumpupallocation.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an possiblebumpupallocation object from history.
        /// </summary>
        /// <param name="possiblebumpupallocation">The possiblebumpupallocation object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the possiblebumpupallocation object from history.</returns>
        internal static string FillHistory(PossibleBumpUpAllocation possiblebumpupallocation, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (possiblebumpupallocation != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKUserID], [CallBackDate], [StampDate], [StampUserID] FROM [zHstPossibleBumpUpAllocation] WHERE [zHstPossibleBumpUpAllocation].[ID] = @ID AND [zHstPossibleBumpUpAllocation].[StampUserID] = @StampUserID AND [zHstPossibleBumpUpAllocation].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", possiblebumpupallocation.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the possiblebumpupallocations in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the possiblebumpupallocations in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [PossibleBumpUpAllocation].[ID], [PossibleBumpUpAllocation].[FKINImportID], [PossibleBumpUpAllocation].[FKUserID], [PossibleBumpUpAllocation].[CallBackDate], [PossibleBumpUpAllocation].[StampDate], [PossibleBumpUpAllocation].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [PossibleBumpUpAllocation].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [PossibleBumpUpAllocation] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted possiblebumpupallocations in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted possiblebumpupallocations in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstPossibleBumpUpAllocation].[ID], [zHstPossibleBumpUpAllocation].[FKINImportID], [zHstPossibleBumpUpAllocation].[FKUserID], [zHstPossibleBumpUpAllocation].[CallBackDate], [zHstPossibleBumpUpAllocation].[StampDate], [zHstPossibleBumpUpAllocation].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstPossibleBumpUpAllocation].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstPossibleBumpUpAllocation] ");
            query.Append("INNER JOIN (SELECT [zHstPossibleBumpUpAllocation].[ID], MAX([zHstPossibleBumpUpAllocation].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstPossibleBumpUpAllocation] ");
            query.Append("WHERE [zHstPossibleBumpUpAllocation].[ID] NOT IN (SELECT [PossibleBumpUpAllocation].[ID] FROM [PossibleBumpUpAllocation]) ");
            query.Append("GROUP BY [zHstPossibleBumpUpAllocation].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstPossibleBumpUpAllocation].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstPossibleBumpUpAllocation].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) possiblebumpupallocation in the database.
        /// </summary>
        /// <param name="possiblebumpupallocation">The possiblebumpupallocation object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) possiblebumpupallocation in the database.</returns>
        public static string ListHistory(PossibleBumpUpAllocation possiblebumpupallocation, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (possiblebumpupallocation != null)
            {
            query.Append("SELECT [zHstPossibleBumpUpAllocation].[ID], [zHstPossibleBumpUpAllocation].[FKINImportID], [zHstPossibleBumpUpAllocation].[FKUserID], [zHstPossibleBumpUpAllocation].[CallBackDate], [zHstPossibleBumpUpAllocation].[StampDate], [zHstPossibleBumpUpAllocation].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstPossibleBumpUpAllocation].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstPossibleBumpUpAllocation] ");
                query.Append(" WHERE [zHstPossibleBumpUpAllocation].[ID] = @ID");
                query.Append(" ORDER BY [zHstPossibleBumpUpAllocation].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", possiblebumpupallocation.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) possiblebumpupallocation to the database.
        /// </summary>
        /// <param name="possiblebumpupallocation">The possiblebumpupallocation to save.</param>
        /// <returns>A query that can be used to save the possiblebumpupallocation to the database.</returns>
        internal static string Save(PossibleBumpUpAllocation possiblebumpupallocation, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (possiblebumpupallocation != null)
            {
                if (possiblebumpupallocation.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstPossibleBumpUpAllocation] ([ID], [FKINImportID], [FKUserID], [CallBackDate], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKUserID], [CallBackDate], [StampDate], [StampUserID] FROM [PossibleBumpUpAllocation] WHERE [PossibleBumpUpAllocation].[ID] = @ID; ");
                    query.Append("UPDATE [PossibleBumpUpAllocation]");
                    parameters = new object[4];
                    query.Append(" SET [FKINImportID] = @FKINImportID");
                    parameters[0] = Database.GetParameter("@FKINImportID", possiblebumpupallocation.FKINImportID.HasValue ? (object)possiblebumpupallocation.FKINImportID.Value : DBNull.Value);
                    query.Append(", [FKUserID] = @FKUserID");
                    parameters[1] = Database.GetParameter("@FKUserID", possiblebumpupallocation.FKUserID.HasValue ? (object)possiblebumpupallocation.FKUserID.Value : DBNull.Value);
                    query.Append(", [CallBackDate] = @CallBackDate");
                    parameters[2] = Database.GetParameter("@CallBackDate", possiblebumpupallocation.CallBackDate.HasValue ? (object)possiblebumpupallocation.CallBackDate.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [PossibleBumpUpAllocation].[ID] = @ID"); 
                    parameters[3] = Database.GetParameter("@ID", possiblebumpupallocation.ID);
                }
                else
                {
                    query.Append("INSERT INTO [PossibleBumpUpAllocation] ([FKINImportID], [FKUserID], [CallBackDate], [StampDate], [StampUserID]) VALUES(@FKINImportID, @FKUserID, @CallBackDate, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@FKINImportID", possiblebumpupallocation.FKINImportID.HasValue ? (object)possiblebumpupallocation.FKINImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKUserID", possiblebumpupallocation.FKUserID.HasValue ? (object)possiblebumpupallocation.FKUserID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@CallBackDate", possiblebumpupallocation.CallBackDate.HasValue ? (object)possiblebumpupallocation.CallBackDate.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for possiblebumpupallocations that match the search criteria.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="callbackdate">The callbackdate search criteria.</param>
        /// <returns>A query that can be used to search for possiblebumpupallocations based on the search criteria.</returns>
        internal static string Search(long? fkinimportid, long? fkuserid, DateTime? callbackdate)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[PossibleBumpUpAllocation].[FKINImportID] = " + fkinimportid + "");
            }
            if (fkuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[PossibleBumpUpAllocation].[FKUserID] = " + fkuserid + "");
            }
            if (callbackdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[PossibleBumpUpAllocation].[CallBackDate] = '" + callbackdate.Value.ToString(Database.DateFormat) + "'");
            }
            query.Append("SELECT [PossibleBumpUpAllocation].[ID], [PossibleBumpUpAllocation].[FKINImportID], [PossibleBumpUpAllocation].[FKUserID], [PossibleBumpUpAllocation].[CallBackDate], [PossibleBumpUpAllocation].[StampDate], [PossibleBumpUpAllocation].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [PossibleBumpUpAllocation].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [PossibleBumpUpAllocation] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
