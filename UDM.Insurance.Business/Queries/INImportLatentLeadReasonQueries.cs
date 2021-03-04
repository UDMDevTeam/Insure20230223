using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inimportlatentleadreason objects.
    /// </summary>
    internal abstract partial class INImportLatentLeadReasonQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inimportlatentleadreason from the database.
        /// </summary>
        /// <param name="inimportlatentleadreason">The inimportlatentleadreason object to delete.</param>
        /// <returns>A query that can be used to delete the inimportlatentleadreason from the database.</returns>
        internal static string Delete(INImportLatentLeadReason inimportlatentleadreason, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportlatentleadreason != null)
            {
                query = "INSERT INTO [zHstINImportLatentLeadReason] ([ID], [FKINImportID], [FKINLatentLeadReasonID1], [FKINLatentLeadReasonID2], [FKINLatentLeadReasonID3], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKINLatentLeadReasonID1], [FKINLatentLeadReasonID2], [FKINLatentLeadReasonID3], [StampDate], [StampUserID] FROM [INImportLatentLeadReason] WHERE [INImportLatentLeadReason].[ID] = @ID; ";
                query += "DELETE FROM [INImportLatentLeadReason] WHERE [INImportLatentLeadReason].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportlatentleadreason.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inimportlatentleadreason from the database.
        /// </summary>
        /// <param name="inimportlatentleadreason">The inimportlatentleadreason object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inimportlatentleadreason from the database.</returns>
        internal static string DeleteHistory(INImportLatentLeadReason inimportlatentleadreason, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportlatentleadreason != null)
            {
                query = "DELETE FROM [zHstINImportLatentLeadReason] WHERE [zHstINImportLatentLeadReason].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportlatentleadreason.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inimportlatentleadreason from the database.
        /// </summary>
        /// <param name="inimportlatentleadreason">The inimportlatentleadreason object to undelete.</param>
        /// <returns>A query that can be used to undelete the inimportlatentleadreason from the database.</returns>
        internal static string UnDelete(INImportLatentLeadReason inimportlatentleadreason, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportlatentleadreason != null)
            {
                query = "INSERT INTO [INImportLatentLeadReason] ([ID], [FKINImportID], [FKINLatentLeadReasonID1], [FKINLatentLeadReasonID2], [FKINLatentLeadReasonID3], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKINLatentLeadReasonID1], [FKINLatentLeadReasonID2], [FKINLatentLeadReasonID3], [StampDate], [StampUserID] FROM [zHstINImportLatentLeadReason] WHERE [zHstINImportLatentLeadReason].[ID] = @ID AND [zHstINImportLatentLeadReason].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportLatentLeadReason] WHERE [zHstINImportLatentLeadReason].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INImportLatentLeadReason] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINImportLatentLeadReason] WHERE [zHstINImportLatentLeadReason].[ID] = @ID AND [zHstINImportLatentLeadReason].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportLatentLeadReason] WHERE [zHstINImportLatentLeadReason].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INImportLatentLeadReason] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportlatentleadreason.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inimportlatentleadreason object.
        /// </summary>
        /// <param name="inimportlatentleadreason">The inimportlatentleadreason object to fill.</param>
        /// <returns>A query that can be used to fill the inimportlatentleadreason object.</returns>
        internal static string Fill(INImportLatentLeadReason inimportlatentleadreason, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportlatentleadreason != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKINLatentLeadReasonID1], [FKINLatentLeadReasonID2], [FKINLatentLeadReasonID3], [StampDate], [StampUserID] FROM [INImportLatentLeadReason] WHERE [INImportLatentLeadReason].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportlatentleadreason.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inimportlatentleadreason data.
        /// </summary>
        /// <param name="inimportlatentleadreason">The inimportlatentleadreason to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inimportlatentleadreason data.</returns>
        internal static string FillData(INImportLatentLeadReason inimportlatentleadreason, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportlatentleadreason != null)
            {
            query.Append("SELECT [INImportLatentLeadReason].[ID], [INImportLatentLeadReason].[FKINImportID], [INImportLatentLeadReason].[FKINLatentLeadReasonID1], [INImportLatentLeadReason].[FKINLatentLeadReasonID2], [INImportLatentLeadReason].[FKINLatentLeadReasonID3], [INImportLatentLeadReason].[StampDate], [INImportLatentLeadReason].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportLatentLeadReason].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportLatentLeadReason] ");
                query.Append(" WHERE [INImportLatentLeadReason].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportlatentleadreason.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inimportlatentleadreason object from history.
        /// </summary>
        /// <param name="inimportlatentleadreason">The inimportlatentleadreason object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inimportlatentleadreason object from history.</returns>
        internal static string FillHistory(INImportLatentLeadReason inimportlatentleadreason, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportlatentleadreason != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKINLatentLeadReasonID1], [FKINLatentLeadReasonID2], [FKINLatentLeadReasonID3], [StampDate], [StampUserID] FROM [zHstINImportLatentLeadReason] WHERE [zHstINImportLatentLeadReason].[ID] = @ID AND [zHstINImportLatentLeadReason].[StampUserID] = @StampUserID AND [zHstINImportLatentLeadReason].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inimportlatentleadreason.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportlatentleadreasons in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inimportlatentleadreasons in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INImportLatentLeadReason].[ID], [INImportLatentLeadReason].[FKINImportID], [INImportLatentLeadReason].[FKINLatentLeadReasonID1], [INImportLatentLeadReason].[FKINLatentLeadReasonID2], [INImportLatentLeadReason].[FKINLatentLeadReasonID3], [INImportLatentLeadReason].[StampDate], [INImportLatentLeadReason].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportLatentLeadReason].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportLatentLeadReason] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inimportlatentleadreasons in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inimportlatentleadreasons in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINImportLatentLeadReason].[ID], [zHstINImportLatentLeadReason].[FKINImportID], [zHstINImportLatentLeadReason].[FKINLatentLeadReasonID1], [zHstINImportLatentLeadReason].[FKINLatentLeadReasonID2], [zHstINImportLatentLeadReason].[FKINLatentLeadReasonID3], [zHstINImportLatentLeadReason].[StampDate], [zHstINImportLatentLeadReason].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportLatentLeadReason].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportLatentLeadReason] ");
            query.Append("INNER JOIN (SELECT [zHstINImportLatentLeadReason].[ID], MAX([zHstINImportLatentLeadReason].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINImportLatentLeadReason] ");
            query.Append("WHERE [zHstINImportLatentLeadReason].[ID] NOT IN (SELECT [INImportLatentLeadReason].[ID] FROM [INImportLatentLeadReason]) ");
            query.Append("GROUP BY [zHstINImportLatentLeadReason].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINImportLatentLeadReason].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINImportLatentLeadReason].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inimportlatentleadreason in the database.
        /// </summary>
        /// <param name="inimportlatentleadreason">The inimportlatentleadreason object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inimportlatentleadreason in the database.</returns>
        public static string ListHistory(INImportLatentLeadReason inimportlatentleadreason, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportlatentleadreason != null)
            {
            query.Append("SELECT [zHstINImportLatentLeadReason].[ID], [zHstINImportLatentLeadReason].[FKINImportID], [zHstINImportLatentLeadReason].[FKINLatentLeadReasonID1], [zHstINImportLatentLeadReason].[FKINLatentLeadReasonID2], [zHstINImportLatentLeadReason].[FKINLatentLeadReasonID3], [zHstINImportLatentLeadReason].[StampDate], [zHstINImportLatentLeadReason].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportLatentLeadReason].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportLatentLeadReason] ");
                query.Append(" WHERE [zHstINImportLatentLeadReason].[ID] = @ID");
                query.Append(" ORDER BY [zHstINImportLatentLeadReason].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportlatentleadreason.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inimportlatentleadreason to the database.
        /// </summary>
        /// <param name="inimportlatentleadreason">The inimportlatentleadreason to save.</param>
        /// <returns>A query that can be used to save the inimportlatentleadreason to the database.</returns>
        internal static string Save(INImportLatentLeadReason inimportlatentleadreason, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportlatentleadreason != null)
            {
                if (inimportlatentleadreason.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINImportLatentLeadReason] ([ID], [FKINImportID], [FKINLatentLeadReasonID1], [FKINLatentLeadReasonID2], [FKINLatentLeadReasonID3], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKINLatentLeadReasonID1], [FKINLatentLeadReasonID2], [FKINLatentLeadReasonID3], [StampDate], [StampUserID] FROM [INImportLatentLeadReason] WHERE [INImportLatentLeadReason].[ID] = @ID; ");
                    query.Append("UPDATE [INImportLatentLeadReason]");
                    parameters = new object[5];
                    query.Append(" SET [FKINImportID] = @FKINImportID");
                    parameters[0] = Database.GetParameter("@FKINImportID", inimportlatentleadreason.FKINImportID.HasValue ? (object)inimportlatentleadreason.FKINImportID.Value : DBNull.Value);
                    query.Append(", [FKINLatentLeadReasonID1] = @FKINLatentLeadReasonID1");
                    parameters[1] = Database.GetParameter("@FKINLatentLeadReasonID1", inimportlatentleadreason.FKINLatentLeadReasonID1.HasValue ? (object)inimportlatentleadreason.FKINLatentLeadReasonID1.Value : DBNull.Value);
                    query.Append(", [FKINLatentLeadReasonID2] = @FKINLatentLeadReasonID2");
                    parameters[2] = Database.GetParameter("@FKINLatentLeadReasonID2", inimportlatentleadreason.FKINLatentLeadReasonID2.HasValue ? (object)inimportlatentleadreason.FKINLatentLeadReasonID2.Value : DBNull.Value);
                    query.Append(", [FKINLatentLeadReasonID3] = @FKINLatentLeadReasonID3");
                    parameters[3] = Database.GetParameter("@FKINLatentLeadReasonID3", inimportlatentleadreason.FKINLatentLeadReasonID3.HasValue ? (object)inimportlatentleadreason.FKINLatentLeadReasonID3.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INImportLatentLeadReason].[ID] = @ID"); 
                    parameters[4] = Database.GetParameter("@ID", inimportlatentleadreason.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INImportLatentLeadReason] ([FKINImportID], [FKINLatentLeadReasonID1], [FKINLatentLeadReasonID2], [FKINLatentLeadReasonID3], [StampDate], [StampUserID]) VALUES(@FKINImportID, @FKINLatentLeadReasonID1, @FKINLatentLeadReasonID2, @FKINLatentLeadReasonID3, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[4];
                    parameters[0] = Database.GetParameter("@FKINImportID", inimportlatentleadreason.FKINImportID.HasValue ? (object)inimportlatentleadreason.FKINImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKINLatentLeadReasonID1", inimportlatentleadreason.FKINLatentLeadReasonID1.HasValue ? (object)inimportlatentleadreason.FKINLatentLeadReasonID1.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@FKINLatentLeadReasonID2", inimportlatentleadreason.FKINLatentLeadReasonID2.HasValue ? (object)inimportlatentleadreason.FKINLatentLeadReasonID2.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@FKINLatentLeadReasonID3", inimportlatentleadreason.FKINLatentLeadReasonID3.HasValue ? (object)inimportlatentleadreason.FKINLatentLeadReasonID3.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportlatentleadreasons that match the search criteria.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkinlatentleadreasonid1">The fkinlatentleadreasonid1 search criteria.</param>
        /// <param name="fkinlatentleadreasonid2">The fkinlatentleadreasonid2 search criteria.</param>
        /// <param name="fkinlatentleadreasonid3">The fkinlatentleadreasonid3 search criteria.</param>
        /// <returns>A query that can be used to search for inimportlatentleadreasons based on the search criteria.</returns>
        internal static string Search(long? fkinimportid, long? fkinlatentleadreasonid1, long? fkinlatentleadreasonid2, long? fkinlatentleadreasonid3)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportLatentLeadReason].[FKINImportID] = " + fkinimportid + "");
            }
            if (fkinlatentleadreasonid1 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportLatentLeadReason].[FKINLatentLeadReasonID1] = " + fkinlatentleadreasonid1 + "");
            }
            if (fkinlatentleadreasonid2 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportLatentLeadReason].[FKINLatentLeadReasonID2] = " + fkinlatentleadreasonid2 + "");
            }
            if (fkinlatentleadreasonid3 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportLatentLeadReason].[FKINLatentLeadReasonID3] = " + fkinlatentleadreasonid3 + "");
            }
            query.Append("SELECT [INImportLatentLeadReason].[ID], [INImportLatentLeadReason].[FKINImportID], [INImportLatentLeadReason].[FKINLatentLeadReasonID1], [INImportLatentLeadReason].[FKINLatentLeadReasonID2], [INImportLatentLeadReason].[FKINLatentLeadReasonID3], [INImportLatentLeadReason].[StampDate], [INImportLatentLeadReason].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportLatentLeadReason].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportLatentLeadReason] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
