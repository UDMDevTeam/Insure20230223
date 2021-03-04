using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to importcmstandardnotes objects.
    /// </summary>
    internal abstract partial class ImportCMStandardNotesQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) importcmstandardnotes from the database.
        /// </summary>
        /// <param name="importcmstandardnotes">The importcmstandardnotes object to delete.</param>
        /// <returns>A query that can be used to delete the importcmstandardnotes from the database.</returns>
        internal static string Delete(ImportCMStandardNotes importcmstandardnotes, ref object[] parameters)
        {
            string query = string.Empty;
            if (importcmstandardnotes != null)
            {
                query = "INSERT INTO [zHstImportCMStandardNotes] ([ID], [FKCallMonitoringStandardNotesID], [FKImportCallMonitoringID]) SELECT [ID], [FKCallMonitoringStandardNotesID], [FKImportCallMonitoringID] FROM [ImportCMStandardNotes] WHERE [ImportCMStandardNotes].[ID] = @ID; ";
                query += "DELETE FROM [ImportCMStandardNotes] WHERE [ImportCMStandardNotes].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", importcmstandardnotes.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) importcmstandardnotes from the database.
        /// </summary>
        /// <param name="importcmstandardnotes">The importcmstandardnotes object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the importcmstandardnotes from the database.</returns>
        internal static string DeleteHistory(ImportCMStandardNotes importcmstandardnotes, ref object[] parameters)
        {
            string query = string.Empty;
            if (importcmstandardnotes != null)
            {
                query = "DELETE FROM [zHstImportCMStandardNotes] WHERE [zHstImportCMStandardNotes].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", importcmstandardnotes.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) importcmstandardnotes from the database.
        /// </summary>
        /// <param name="importcmstandardnotes">The importcmstandardnotes object to undelete.</param>
        /// <returns>A query that can be used to undelete the importcmstandardnotes from the database.</returns>
        internal static string UnDelete(ImportCMStandardNotes importcmstandardnotes, ref object[] parameters)
        {
            string query = string.Empty;
            if (importcmstandardnotes != null)
            {
                query = "INSERT INTO [ImportCMStandardNotes] ([ID], [FKCallMonitoringStandardNotesID], [FKImportCallMonitoringID]) SELECT [ID], [FKCallMonitoringStandardNotesID], [FKImportCallMonitoringID] FROM [zHstImportCMStandardNotes] WHERE [zHstImportCMStandardNotes].[ID] = @ID AND [zHstImportCMStandardNotes].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstImportCMStandardNotes] WHERE [zHstImportCMStandardNotes].[ID] = @ID) AND (SELECT COUNT(ID) FROM [ImportCMStandardNotes] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstImportCMStandardNotes] WHERE [zHstImportCMStandardNotes].[ID] = @ID AND [zHstImportCMStandardNotes].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstImportCMStandardNotes] WHERE [zHstImportCMStandardNotes].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [ImportCMStandardNotes] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", importcmstandardnotes.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an importcmstandardnotes object.
        /// </summary>
        /// <param name="importcmstandardnotes">The importcmstandardnotes object to fill.</param>
        /// <returns>A query that can be used to fill the importcmstandardnotes object.</returns>
        internal static string Fill(ImportCMStandardNotes importcmstandardnotes, ref object[] parameters)
        {
            string query = string.Empty;
            if (importcmstandardnotes != null)
            {
                query = "SELECT [ID], [FKCallMonitoringStandardNotesID], [FKImportCallMonitoringID] FROM [ImportCMStandardNotes] WHERE [ImportCMStandardNotes].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", importcmstandardnotes.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  importcmstandardnotes data.
        /// </summary>
        /// <param name="importcmstandardnotes">The importcmstandardnotes to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  importcmstandardnotes data.</returns>
        internal static string FillData(ImportCMStandardNotes importcmstandardnotes, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (importcmstandardnotes != null)
            {
            query.Append("SELECT [ImportCMStandardNotes].[ID], [ImportCMStandardNotes].[FKCallMonitoringStandardNotesID], [ImportCMStandardNotes].[FKImportCallMonitoringID]");
            query.Append(" FROM [ImportCMStandardNotes] ");
                query.Append(" WHERE [ImportCMStandardNotes].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", importcmstandardnotes.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an importcmstandardnotes object from history.
        /// </summary>
        /// <param name="importcmstandardnotes">The importcmstandardnotes object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the importcmstandardnotes object from history.</returns>
        internal static string FillHistory(ImportCMStandardNotes importcmstandardnotes, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (importcmstandardnotes != null)
            {
                query = "SELECT [ID], [FKCallMonitoringStandardNotesID], [FKImportCallMonitoringID] FROM [zHstImportCMStandardNotes] WHERE [zHstImportCMStandardNotes].[ID] = @ID AND [zHstImportCMStandardNotes].[StampUserID] = @StampUserID AND [zHstImportCMStandardNotes].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", importcmstandardnotes.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the importcmstandardnotess in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the importcmstandardnotess in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [ImportCMStandardNotes].[ID], [ImportCMStandardNotes].[FKCallMonitoringStandardNotesID], [ImportCMStandardNotes].[FKImportCallMonitoringID]");
            query.Append(" FROM [ImportCMStandardNotes] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted importcmstandardnotess in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted importcmstandardnotess in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstImportCMStandardNotes].[ID], [zHstImportCMStandardNotes].[FKCallMonitoringStandardNotesID], [zHstImportCMStandardNotes].[FKImportCallMonitoringID]");
            query.Append(" FROM [zHstImportCMStandardNotes] ");
            query.Append("INNER JOIN (SELECT [zHstImportCMStandardNotes].[ID], MAX([zHstImportCMStandardNotes].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstImportCMStandardNotes] ");
            query.Append("WHERE [zHstImportCMStandardNotes].[ID] NOT IN (SELECT [ImportCMStandardNotes].[ID] FROM [ImportCMStandardNotes]) ");
            query.Append("GROUP BY [zHstImportCMStandardNotes].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstImportCMStandardNotes].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstImportCMStandardNotes].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) importcmstandardnotes in the database.
        /// </summary>
        /// <param name="importcmstandardnotes">The importcmstandardnotes object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) importcmstandardnotes in the database.</returns>
        public static string ListHistory(ImportCMStandardNotes importcmstandardnotes, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (importcmstandardnotes != null)
            {
            query.Append("SELECT [zHstImportCMStandardNotes].[ID], [zHstImportCMStandardNotes].[FKCallMonitoringStandardNotesID], [zHstImportCMStandardNotes].[FKImportCallMonitoringID]");
            query.Append(" FROM [zHstImportCMStandardNotes] ");
                query.Append(" WHERE [zHstImportCMStandardNotes].[ID] = @ID");
                query.Append(" ORDER BY [zHstImportCMStandardNotes].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", importcmstandardnotes.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) importcmstandardnotes to the database.
        /// </summary>
        /// <param name="importcmstandardnotes">The importcmstandardnotes to save.</param>
        /// <returns>A query that can be used to save the importcmstandardnotes to the database.</returns>
        internal static string Save(ImportCMStandardNotes importcmstandardnotes, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (importcmstandardnotes != null)
            {
                if (importcmstandardnotes.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstImportCMStandardNotes] ([ID], [FKCallMonitoringStandardNotesID], [FKImportCallMonitoringID]) SELECT [ID], [FKCallMonitoringStandardNotesID], [FKImportCallMonitoringID] FROM [ImportCMStandardNotes] WHERE [ImportCMStandardNotes].[ID] = @ID; ");
                    query.Append("UPDATE [ImportCMStandardNotes]");
                    parameters = new object[3];
                    query.Append(" SET [FKCallMonitoringStandardNotesID] = @FKCallMonitoringStandardNotesID");
                    parameters[0] = Database.GetParameter("@FKCallMonitoringStandardNotesID", importcmstandardnotes.FKCallMonitoringStandardNotesID.HasValue ? (object)importcmstandardnotes.FKCallMonitoringStandardNotesID.Value : DBNull.Value);
                    query.Append(", [FKImportCallMonitoringID] = @FKImportCallMonitoringID");
                    parameters[1] = Database.GetParameter("@FKImportCallMonitoringID", importcmstandardnotes.FKImportCallMonitoringID.HasValue ? (object)importcmstandardnotes.FKImportCallMonitoringID.Value : DBNull.Value);
                    query.Append(" WHERE [ImportCMStandardNotes].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", importcmstandardnotes.ID);
                }
                else
                {
                    query.Append("INSERT INTO [ImportCMStandardNotes] ([FKCallMonitoringStandardNotesID], [FKImportCallMonitoringID]) VALUES(@FKCallMonitoringStandardNotesID, @FKImportCallMonitoringID);");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKCallMonitoringStandardNotesID", importcmstandardnotes.FKCallMonitoringStandardNotesID.HasValue ? (object)importcmstandardnotes.FKCallMonitoringStandardNotesID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKImportCallMonitoringID", importcmstandardnotes.FKImportCallMonitoringID.HasValue ? (object)importcmstandardnotes.FKImportCallMonitoringID.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for importcmstandardnotess that match the search criteria.
        /// </summary>
        /// <param name="fkcallmonitoringstandardnotesid">The fkcallmonitoringstandardnotesid search criteria.</param>
        /// <param name="fkimportcallmonitoringid">The fkimportcallmonitoringid search criteria.</param>
        /// <returns>A query that can be used to search for importcmstandardnotess based on the search criteria.</returns>
        internal static string Search(long? fkcallmonitoringstandardnotesid, long? fkimportcallmonitoringid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkcallmonitoringstandardnotesid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[ImportCMStandardNotes].[FKCallMonitoringStandardNotesID] = " + fkcallmonitoringstandardnotesid + "");
            }
            if (fkimportcallmonitoringid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[ImportCMStandardNotes].[FKImportCallMonitoringID] = " + fkimportcallmonitoringid + "");
            }
            query.Append("SELECT [ImportCMStandardNotes].[ID], [ImportCMStandardNotes].[FKCallMonitoringStandardNotesID], [ImportCMStandardNotes].[FKImportCallMonitoringID]");
            query.Append(" FROM [ImportCMStandardNotes] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
