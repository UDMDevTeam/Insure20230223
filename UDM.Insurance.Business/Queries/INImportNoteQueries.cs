using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inimportnote objects.
    /// </summary>
    internal abstract partial class INImportNoteQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inimportnote from the database.
        /// </summary>
        /// <param name="inimportnote">The inimportnote object to delete.</param>
        /// <returns>A query that can be used to delete the inimportnote from the database.</returns>
        internal static string Delete(INImportNote inimportnote, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportnote != null)
            {
                query = "INSERT INTO [zHstINImportNote] ([ID], [FKINImportID], [FKUserID], [NoteDate], [Note], [Sequence], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKUserID], [NoteDate], [Note], [Sequence], [StampDate], [StampUserID] FROM [INImportNote] WHERE [INImportNote].[ID] = @ID; ";
                query += "DELETE FROM [INImportNote] WHERE [INImportNote].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportnote.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inimportnote from the database.
        /// </summary>
        /// <param name="inimportnote">The inimportnote object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inimportnote from the database.</returns>
        internal static string DeleteHistory(INImportNote inimportnote, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportnote != null)
            {
                query = "DELETE FROM [zHstINImportNote] WHERE [zHstINImportNote].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportnote.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inimportnote from the database.
        /// </summary>
        /// <param name="inimportnote">The inimportnote object to undelete.</param>
        /// <returns>A query that can be used to undelete the inimportnote from the database.</returns>
        internal static string UnDelete(INImportNote inimportnote, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportnote != null)
            {
                query = "INSERT INTO [INImportNote] ([ID], [FKINImportID], [FKUserID], [NoteDate], [Note], [Sequence], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKUserID], [NoteDate], [Note], [Sequence], [StampDate], [StampUserID] FROM [zHstINImportNote] WHERE [zHstINImportNote].[ID] = @ID AND [zHstINImportNote].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportNote] WHERE [zHstINImportNote].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INImportNote] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINImportNote] WHERE [zHstINImportNote].[ID] = @ID AND [zHstINImportNote].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportNote] WHERE [zHstINImportNote].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INImportNote] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportnote.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inimportnote object.
        /// </summary>
        /// <param name="inimportnote">The inimportnote object to fill.</param>
        /// <returns>A query that can be used to fill the inimportnote object.</returns>
        internal static string Fill(INImportNote inimportnote, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportnote != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKUserID], [NoteDate], [Note], [Sequence], [StampDate], [StampUserID] FROM [INImportNote] WHERE [INImportNote].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportnote.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inimportnote data.
        /// </summary>
        /// <param name="inimportnote">The inimportnote to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inimportnote data.</returns>
        internal static string FillData(INImportNote inimportnote, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportnote != null)
            {
            query.Append("SELECT [INImportNote].[ID], [INImportNote].[FKINImportID], [INImportNote].[FKUserID], [INImportNote].[NoteDate], [INImportNote].[Note], [INImportNote].[Sequence], [INImportNote].[StampDate], [INImportNote].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportNote].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportNote] ");
                query.Append(" WHERE [INImportNote].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportnote.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inimportnote object from history.
        /// </summary>
        /// <param name="inimportnote">The inimportnote object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inimportnote object from history.</returns>
        internal static string FillHistory(INImportNote inimportnote, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportnote != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKUserID], [NoteDate], [Note], [Sequence], [StampDate], [StampUserID] FROM [zHstINImportNote] WHERE [zHstINImportNote].[ID] = @ID AND [zHstINImportNote].[StampUserID] = @StampUserID AND [zHstINImportNote].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inimportnote.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportnotes in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inimportnotes in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INImportNote].[ID], [INImportNote].[FKINImportID], [INImportNote].[FKUserID], [INImportNote].[NoteDate], [INImportNote].[Note], [INImportNote].[Sequence], [INImportNote].[StampDate], [INImportNote].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportNote].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportNote] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inimportnotes in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inimportnotes in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINImportNote].[ID], [zHstINImportNote].[FKINImportID], [zHstINImportNote].[FKUserID], [zHstINImportNote].[NoteDate], [zHstINImportNote].[Note], [zHstINImportNote].[Sequence], [zHstINImportNote].[StampDate], [zHstINImportNote].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportNote].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportNote] ");
            query.Append("INNER JOIN (SELECT [zHstINImportNote].[ID], MAX([zHstINImportNote].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINImportNote] ");
            query.Append("WHERE [zHstINImportNote].[ID] NOT IN (SELECT [INImportNote].[ID] FROM [INImportNote]) ");
            query.Append("GROUP BY [zHstINImportNote].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINImportNote].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINImportNote].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inimportnote in the database.
        /// </summary>
        /// <param name="inimportnote">The inimportnote object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inimportnote in the database.</returns>
        public static string ListHistory(INImportNote inimportnote, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportnote != null)
            {
            query.Append("SELECT [zHstINImportNote].[ID], [zHstINImportNote].[FKINImportID], [zHstINImportNote].[FKUserID], [zHstINImportNote].[NoteDate], [zHstINImportNote].[Note], [zHstINImportNote].[Sequence], [zHstINImportNote].[StampDate], [zHstINImportNote].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportNote].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportNote] ");
                query.Append(" WHERE [zHstINImportNote].[ID] = @ID");
                query.Append(" ORDER BY [zHstINImportNote].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportnote.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inimportnote to the database.
        /// </summary>
        /// <param name="inimportnote">The inimportnote to save.</param>
        /// <returns>A query that can be used to save the inimportnote to the database.</returns>
        internal static string Save(INImportNote inimportnote, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportnote != null)
            {
                if (inimportnote.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINImportNote] ([ID], [FKINImportID], [FKUserID], [NoteDate], [Note], [Sequence], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKUserID], [NoteDate], [Note], [Sequence], [StampDate], [StampUserID] FROM [INImportNote] WHERE [INImportNote].[ID] = @ID; ");
                    query.Append("UPDATE [INImportNote]");
                    parameters = new object[6];
                    query.Append(" SET [FKINImportID] = @FKINImportID");
                    parameters[0] = Database.GetParameter("@FKINImportID", inimportnote.FKINImportID.HasValue ? (object)inimportnote.FKINImportID.Value : DBNull.Value);
                    query.Append(", [FKUserID] = @FKUserID");
                    parameters[1] = Database.GetParameter("@FKUserID", inimportnote.FKUserID.HasValue ? (object)inimportnote.FKUserID.Value : DBNull.Value);
                    query.Append(", [NoteDate] = @NoteDate");
                    parameters[2] = Database.GetParameter("@NoteDate", inimportnote.NoteDate.HasValue ? (object)inimportnote.NoteDate.Value : DBNull.Value);
                    query.Append(", [Note] = @Note");
                    parameters[3] = Database.GetParameter("@Note", string.IsNullOrEmpty(inimportnote.Note) ? DBNull.Value : (object)inimportnote.Note);
                    query.Append(", [Sequence] = @Sequence");
                    parameters[4] = Database.GetParameter("@Sequence", inimportnote.Sequence.HasValue ? (object)inimportnote.Sequence.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INImportNote].[ID] = @ID"); 
                    parameters[5] = Database.GetParameter("@ID", inimportnote.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INImportNote] ([FKINImportID], [FKUserID], [NoteDate], [Note], [Sequence], [StampDate], [StampUserID]) VALUES(@FKINImportID, @FKUserID, @NoteDate, @Note, @Sequence, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[5];
                    parameters[0] = Database.GetParameter("@FKINImportID", inimportnote.FKINImportID.HasValue ? (object)inimportnote.FKINImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKUserID", inimportnote.FKUserID.HasValue ? (object)inimportnote.FKUserID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@NoteDate", inimportnote.NoteDate.HasValue ? (object)inimportnote.NoteDate.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@Note", string.IsNullOrEmpty(inimportnote.Note) ? DBNull.Value : (object)inimportnote.Note);
                    parameters[4] = Database.GetParameter("@Sequence", inimportnote.Sequence.HasValue ? (object)inimportnote.Sequence.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportnotes that match the search criteria.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="notedate">The notedate search criteria.</param>
        /// <param name="note">The note search criteria.</param>
        /// <param name="sequence">The sequence search criteria.</param>
        /// <returns>A query that can be used to search for inimportnotes based on the search criteria.</returns>
        internal static string Search(long? fkinimportid, long? fkuserid, DateTime? notedate, string note, int? sequence)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportNote].[FKINImportID] = " + fkinimportid + "");
            }
            if (fkuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportNote].[FKUserID] = " + fkuserid + "");
            }
            if (notedate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportNote].[NoteDate] = '" + notedate.Value.ToString(Database.DateFormat) + "'");
            }
            if (note != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportNote].[Note] LIKE '" + note.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (sequence != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportNote].[Sequence] = " + sequence + "");
            }
            query.Append("SELECT [INImportNote].[ID], [INImportNote].[FKINImportID], [INImportNote].[FKUserID], [INImportNote].[NoteDate], [INImportNote].[Note], [INImportNote].[Sequence], [INImportNote].[StampDate], [INImportNote].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportNote].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportNote] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
