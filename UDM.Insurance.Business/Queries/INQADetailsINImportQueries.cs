using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inqadetailsinimport objects.
    /// </summary>
    internal abstract partial class INQADetailsINImportQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inqadetailsinimport from the database.
        /// </summary>
        /// <param name="inqadetailsinimport">The inqadetailsinimport object to delete.</param>
        /// <returns>A query that can be used to delete the inqadetailsinimport from the database.</returns>
        internal static string Delete(INQADetailsINImport inqadetailsinimport, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsinimport != null)
            {
                query = "INSERT INTO [zHstINQADetailsINImport] ([ID], [FKImportID], [FKAssessorID], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKAssessorID], [StampDate], [StampUserID] FROM [INQADetailsINImport] WHERE [INQADetailsINImport].[ID] = @ID; ";
                query += "DELETE FROM [INQADetailsINImport] WHERE [INQADetailsINImport].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsinimport.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inqadetailsinimport from the database.
        /// </summary>
        /// <param name="inqadetailsinimport">The inqadetailsinimport object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inqadetailsinimport from the database.</returns>
        internal static string DeleteHistory(INQADetailsINImport inqadetailsinimport, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsinimport != null)
            {
                query = "DELETE FROM [zHstINQADetailsINImport] WHERE [zHstINQADetailsINImport].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsinimport.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inqadetailsinimport from the database.
        /// </summary>
        /// <param name="inqadetailsinimport">The inqadetailsinimport object to undelete.</param>
        /// <returns>A query that can be used to undelete the inqadetailsinimport from the database.</returns>
        internal static string UnDelete(INQADetailsINImport inqadetailsinimport, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsinimport != null)
            {
                query = "INSERT INTO [INQADetailsINImport] ([ID], [FKImportID], [FKAssessorID], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKAssessorID], [StampDate], [StampUserID] FROM [zHstINQADetailsINImport] WHERE [zHstINQADetailsINImport].[ID] = @ID AND [zHstINQADetailsINImport].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINQADetailsINImport] WHERE [zHstINQADetailsINImport].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INQADetailsINImport] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINQADetailsINImport] WHERE [zHstINQADetailsINImport].[ID] = @ID AND [zHstINQADetailsINImport].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINQADetailsINImport] WHERE [zHstINQADetailsINImport].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INQADetailsINImport] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsinimport.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inqadetailsinimport object.
        /// </summary>
        /// <param name="inqadetailsinimport">The inqadetailsinimport object to fill.</param>
        /// <returns>A query that can be used to fill the inqadetailsinimport object.</returns>
        internal static string Fill(INQADetailsINImport inqadetailsinimport, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsinimport != null)
            {
                query = "SELECT [ID], [FKImportID], [FKAssessorID], [StampDate], [StampUserID] FROM [INQADetailsINImport] WHERE [INQADetailsINImport].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsinimport.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inqadetailsinimport data.
        /// </summary>
        /// <param name="inqadetailsinimport">The inqadetailsinimport to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inqadetailsinimport data.</returns>
        internal static string FillData(INQADetailsINImport inqadetailsinimport, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inqadetailsinimport != null)
            {
            query.Append("SELECT [INQADetailsINImport].[ID], [INQADetailsINImport].[FKImportID], [INQADetailsINImport].[FKAssessorID], [INQADetailsINImport].[StampDate], [INQADetailsINImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INQADetailsINImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INQADetailsINImport] ");
                query.Append(" WHERE [INQADetailsINImport].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsinimport.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inqadetailsinimport object from history.
        /// </summary>
        /// <param name="inqadetailsinimport">The inqadetailsinimport object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inqadetailsinimport object from history.</returns>
        internal static string FillHistory(INQADetailsINImport inqadetailsinimport, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsinimport != null)
            {
                query = "SELECT [ID], [FKImportID], [FKAssessorID], [StampDate], [StampUserID] FROM [zHstINQADetailsINImport] WHERE [zHstINQADetailsINImport].[ID] = @ID AND [zHstINQADetailsINImport].[StampUserID] = @StampUserID AND [zHstINQADetailsINImport].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inqadetailsinimport.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inqadetailsinimports in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inqadetailsinimports in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INQADetailsINImport].[ID], [INQADetailsINImport].[FKImportID], [INQADetailsINImport].[FKAssessorID], [INQADetailsINImport].[StampDate], [INQADetailsINImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INQADetailsINImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INQADetailsINImport] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inqadetailsinimports in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inqadetailsinimports in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINQADetailsINImport].[ID], [zHstINQADetailsINImport].[FKImportID], [zHstINQADetailsINImport].[FKAssessorID], [zHstINQADetailsINImport].[StampDate], [zHstINQADetailsINImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINQADetailsINImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINQADetailsINImport] ");
            query.Append("INNER JOIN (SELECT [zHstINQADetailsINImport].[ID], MAX([zHstINQADetailsINImport].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINQADetailsINImport] ");
            query.Append("WHERE [zHstINQADetailsINImport].[ID] NOT IN (SELECT [INQADetailsINImport].[ID] FROM [INQADetailsINImport]) ");
            query.Append("GROUP BY [zHstINQADetailsINImport].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINQADetailsINImport].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINQADetailsINImport].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inqadetailsinimport in the database.
        /// </summary>
        /// <param name="inqadetailsinimport">The inqadetailsinimport object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inqadetailsinimport in the database.</returns>
        public static string ListHistory(INQADetailsINImport inqadetailsinimport, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inqadetailsinimport != null)
            {
            query.Append("SELECT [zHstINQADetailsINImport].[ID], [zHstINQADetailsINImport].[FKImportID], [zHstINQADetailsINImport].[FKAssessorID], [zHstINQADetailsINImport].[StampDate], [zHstINQADetailsINImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINQADetailsINImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINQADetailsINImport] ");
                query.Append(" WHERE [zHstINQADetailsINImport].[ID] = @ID");
                query.Append(" ORDER BY [zHstINQADetailsINImport].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsinimport.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inqadetailsinimport to the database.
        /// </summary>
        /// <param name="inqadetailsinimport">The inqadetailsinimport to save.</param>
        /// <returns>A query that can be used to save the inqadetailsinimport to the database.</returns>
        internal static string Save(INQADetailsINImport inqadetailsinimport, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inqadetailsinimport != null)
            {
                if (inqadetailsinimport.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINQADetailsINImport] ([ID], [FKImportID], [FKAssessorID], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKAssessorID], [StampDate], [StampUserID] FROM [INQADetailsINImport] WHERE [INQADetailsINImport].[ID] = @ID; ");
                    query.Append("UPDATE [INQADetailsINImport]");
                    parameters = new object[3];
                    query.Append(" SET [FKImportID] = @FKImportID");
                    parameters[0] = Database.GetParameter("@FKImportID", inqadetailsinimport.FKImportID.HasValue ? (object)inqadetailsinimport.FKImportID.Value : DBNull.Value);
                    query.Append(", [FKAssessorID] = @FKAssessorID");
                    parameters[1] = Database.GetParameter("@FKAssessorID", inqadetailsinimport.FKAssessorID.HasValue ? (object)inqadetailsinimport.FKAssessorID.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INQADetailsINImport].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", inqadetailsinimport.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INQADetailsINImport] ([FKImportID], [FKAssessorID], [StampDate], [StampUserID]) VALUES(@FKImportID, @FKAssessorID, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKImportID", inqadetailsinimport.FKImportID.HasValue ? (object)inqadetailsinimport.FKImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKAssessorID", inqadetailsinimport.FKAssessorID.HasValue ? (object)inqadetailsinimport.FKAssessorID.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inqadetailsinimports that match the search criteria.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkassessorid">The fkassessorid search criteria.</param>
        /// <returns>A query that can be used to search for inqadetailsinimports based on the search criteria.</returns>
        internal static string Search(long? fkimportid, long? fkassessorid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsINImport].[FKImportID] = " + fkimportid + "");
            }
            if (fkassessorid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsINImport].[FKAssessorID] = " + fkassessorid + "");
            }
            query.Append("SELECT [INQADetailsINImport].[ID], [INQADetailsINImport].[FKImportID], [INQADetailsINImport].[FKAssessorID], [INQADetailsINImport].[StampDate], [INQADetailsINImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INQADetailsINImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INQADetailsINImport] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
