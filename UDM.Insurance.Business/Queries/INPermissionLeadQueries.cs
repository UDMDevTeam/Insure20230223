using System;
using System.Text;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using System.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inimportnote objects.
    /// </summary>
    internal abstract partial class INPermissionLeadQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inimportnote from the database.
        /// </summary>
        /// <param name="inimportnote">The inimportnote object to delete.</param>
        /// <returns>A query that can be used to delete the inimportnote from the database.</returns>
        internal static string Delete(INPermissionLead inpermissionlead, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpermissionlead != null)
            {
                query = "INSERT INTO [zHstINPermissionLead] ([ID], [FKImportID], [Title], [Firstname], [Surname], [CellNumber], [AltNumber], [SavedBy], [DateSaved], [DateOfBirth], [Occupation], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [Title], [Firstname], [Surname], [CellNumber], [AltNumber], [SavedBy], [DateSaved], [DateOfBirth], [Occupation], [StampDate], [StampUserID] FROM [INPermissionLead] WHERE [INPermissionLead].[ID] = @ID; ";
                query += "DELETE FROM [INPermissionLead] WHERE [INPermissionLead].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpermissionlead.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inimportnote from the database.
        /// </summary>
        /// <param name="inimportnote">The inimportnote object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inimportnote from the database.</returns>
        internal static string DeleteHistory(INPermissionLead inpermissionlead, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpermissionlead != null)
            {
                query = "DELETE FROM [zHstINPermissionLead] WHERE [zHstINPermissionLead].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpermissionlead.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inimportnote from the database.
        /// </summary>
        /// <param name="inimportnote">The inimportnote object to undelete.</param>
        /// <returns>A query that can be used to undelete the inimportnote from the database.</returns>
        internal static string UnDelete(INPermissionLead inpermissionlead, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpermissionlead != null)
            {
                query = "INSERT INTO [INPermissionLead] ([ID], [FKImportID], [Title], [Firstname], [Surname], [CellNumber], [AltNumber], [SavedBy], [DateSaved], [DateOfBirth], [Occupation], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [Title], [Firstname], [Surname], [CellNumber], [AltNumber], [SavedBy], [DateSaved], [DateOfBirth], [Occupation], [StampDate], [StampUserID] FROM [zHstINPermissionLead] WHERE [zHstINPermissionLead].[ID] = @ID AND [zHstINPermissionLead].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINPermissionLead] WHERE [zHstINPermissionLead].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INPermissionLeaad] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINPermissionLead] WHERE [zHstINPermissionLead].[ID] = @ID AND [zHstINPermissionLead].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINPermissionLead] WHERE [zHstINPermissionLead].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INPermissionLead] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpermissionlead.ID);
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
        internal static string Fill(INPermissionLead inpermissionlead, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpermissionlead != null)
            {
                query = "SELECT [ID], [FKImportID], [Title], [Firstname], [Surname], [CellNumber], [AltNumber], [SavedBy], [DateSaved], [DateOfBirth], [Occupation], [StampUserID] FROM [INPermissionLead] WHERE [INPermissionLead].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpermissionlead.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inimportnote data.
        /// </summary>
        /// <param name="inimportnote">The inimportnote to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inimportnote data.</returns>
        internal static string FillData(INPermissionLead inpermissionlead, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpermissionlead != null)
            {
            query.Append("SELECT [INPermissionLead].[ID], [INPermissionLead].[FKImportID], [INPermissionLead].[Title], [INPermissionLead].[Firstname], [INPermissionLead].[Surname], [INPermissionLead].[CellNumber], [INPermissionLead].[AltNumber], [INPermissionLead].[SavedBy], [INPermissionLead].[DateSaved],  [INPermissionLead].[DateOfBirth], [INPermissionLead].[Occupation], [INPermissionLead].[StampDate], [INPermissionLead].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPermissionLead].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPermissionLead] ");
                query.Append(" WHERE [INPermissionLead].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpermissionlead.ID);
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
        internal static string FillHistory(INPermissionLead inpermissionlead, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpermissionlead != null)
            {
                query = "SELECT [ID], [FKImportID], [Title], [Firstname], [Surname], [CellNumber], [AltNumber], [SavedBy], [DateSaved], [DateOfBirth], [Occupation], [StampDate], [StampUserID] FROM [zHstINPermissionLead] WHERE [zHstINPermissionLead].[ID] = @ID AND [zHstINPermission].[StampUserID] = @StampUserID AND [zHstINPermission].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inpermissionlead.ID);
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
            query.Append("SELECT [INPermission].[ID], [INPermission].[FKImportID], [INPermission].[Title], [INPermission].[Firstname], [INPermission].[Surname], [INPermission].[CellNumber], [INPermission].[AltNumber], [INPermissionLead].[SavedBy], [INPermissionLead].[DateSaved], [INPermissionLead].[DateOfBirth], [INPermissionLead].[Occupation], [INPermission].[StampDate], [INPermission].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPermissionLead].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPermission] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inimportnotes in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inimportnotes in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINPermissionLead].[ID], [zHstINPermissionLead].[FKImportID], [zHstINPermissionLead].[Title], [zHstINPermissionLead].[Firstname], [zHstINPermissionLead].[Surname], [zHstINPermissionLead].[CellNumber], [zHstINPermissionLead].[AltNumber], [zHstINPermissionLead].[SavedBy], [zHstINPermissionLead].[DateSaved], [zHstINPermissionLead].[DateOfBirth], [zHstINPermissionLead].[Occupation], [zHstINPermissionLead].[StampDate], [zHstINPermissionLead].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINPermissionLead].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINPermissionLead] ");
            query.Append("INNER JOIN (SELECT [zHstINPermissionLead].[ID], MAX([zHstINPermissionLead].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINPermissionLead] ");
            query.Append("WHERE [zHstINPermissionLead].[ID] NOT IN (SELECT [INPermissionLead].[ID] FROM [INPermissionLead]) ");
            query.Append("GROUP BY [zHstINPermissionLead].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINPermissionLead].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINPermissionLead].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inimportnote in the database.
        /// </summary>
        /// <param name="inimportnote">The inimportnote object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inimportnote in the database.</returns>
        public static string ListHistory(INPermissionLead inpermissionlead, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpermissionlead != null)
            {
            query.Append("SELECT [zHstINPermissionLead].[ID], [zHstINPermissionLead].[FKImportID], [zHstINPermissionLead].[Title], [zHstINPermissionLead].[Firstname], [zHstINPermissionLead].[Surname], [zHstINPermissionLead].[CellNumber], [zHstINPermissionLead].[AltNumber], [zHstINPermissionLead].[SavedBy], [zHstINPermissionLead].[DateSaved], [zHstINPermissionLead].[DateOfBirth], [zHstINPermissionLead].[Occupation], [zHstINPermissionLead].[StampDate], [zHstINPermissionLead].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINPermissionLead].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINPermissionLead] ");
                query.Append(" WHERE [zHstINPermissionLead].[ID] = @ID");
                query.Append(" ORDER BY [zHstINPermissionLead].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpermissionlead.ID);
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
        internal static string Save(INPermissionLead inpermissionLead, ref object[] parameters)
        {




            StringBuilder query = new StringBuilder();
            if (inpermissionLead != null)
            {
                if (inpermissionLead.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINPermissionLead] ([ID], [FKImportID], [Title], [Firstname], [Surname], [CellNumber], [AltNumber], [SavedBy], [DateSaved], [DateOfBirth], [Occupation], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [Title], [Firstname], [Surname], [CellNumber], [AltNumber], [SavedBy], [DateSaved], [DateOfBirth], [Occupation], [StampDate], [StampUserID] FROM [INPermissionLead] WHERE [INPermissionLead].[ID] = " + inpermissionLead.ID + " ;");
                    query.Append("UPDATE [INPermissionLead]");
                    parameters = new object[11];
                    query.Append(" SET [FKImportID] = @FKImportID");
                    parameters[0] = Database.GetParameter("@FKImportID", inpermissionLead.FKINImportID.HasValue ? (object)inpermissionLead.FKINImportID.Value : DBNull.Value);
                    query.Append(", [Title] = @Title");
                    parameters[1] = Database.GetParameter("@Title", string.IsNullOrEmpty(inpermissionLead.Title) ? DBNull.Value : (object)inpermissionLead.Title);
                    query.Append(", [Firstname] = @Firstname");
                    parameters[2] = Database.GetParameter("@Firstname", string.IsNullOrEmpty(inpermissionLead.Firstname) ? DBNull.Value : (object)inpermissionLead.Firstname);
                    query.Append(", [Surname] = @Surname");
                    parameters[3] = Database.GetParameter("@Surname", string.IsNullOrEmpty(inpermissionLead.Surname) ? DBNull.Value : (object)inpermissionLead.Surname);
                    query.Append(", [CellNumber] = @CellNumber");
                    parameters[4] = Database.GetParameter("@CellNumber", string.IsNullOrEmpty(inpermissionLead.Cellnumber) ? DBNull.Value : (object)inpermissionLead.Cellnumber);
                    query.Append(", [AltNumber] = @AltNumber");
                    parameters[5] = Database.GetParameter("@AltNumber", string.IsNullOrEmpty(inpermissionLead.AltNumber) ? DBNull.Value : (object)inpermissionLead.AltNumber);
                    query.Append(", [SavedBy] = @SavedBy");
                    parameters[6] = Database.GetParameter("@SavedBy", string.IsNullOrEmpty(inpermissionLead.SavedBy) ? DBNull.Value : (object)inpermissionLead.SavedBy);
                    query.Append(", [DateSaved] = @DateSaved");
                    parameters[7] = Database.GetParameter("@DateSaved", inpermissionLead.DateSaved.HasValue ? (object)inpermissionLead.DateSaved.Value : DBNull.Value);
                    query.Append(", [DateOfBirth] = @DateOfBirth");
                    parameters[8] = Database.GetParameter("@DateOfBirth", inpermissionLead.DateOfBirth.HasValue ? (object)inpermissionLead.DateOfBirth.Value : DBNull.Value);
                    query.Append(", [Occupation] = @Occupation");
                    parameters[9] = Database.GetParameter("@Occupation", string.IsNullOrEmpty(inpermissionLead.Occupation) ? DBNull.Value : (object)inpermissionLead.Occupation);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INPermissionLead].[ID] = " + inpermissionLead.ID + ";");
                    parameters[10] = Database.GetParameter("@ID", inpermissionLead.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INPermissionLead] ([FKImportID], [Title], [Firstname], [Surname], [CellNumber], [AltNumber], [SavedBy], [DateSaved], [DateOfBirth], [Occupation], [StampDate], [StampUserID]) VALUES(@FKImportID, @Title, @Firstname, @Surname, @CellNumber, @AltNumber, @SavedBy, @DateSaved, @DateOfBirth, @Occupation, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[10];
                    parameters[0] = Database.GetParameter("@FKImportID", inpermissionLead.FKINImportID.HasValue ? (object)inpermissionLead.FKINImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@Title", string.IsNullOrEmpty(inpermissionLead.Title) ? DBNull.Value : (object)inpermissionLead.Title);
                    parameters[2] = Database.GetParameter("@Firstname", string.IsNullOrEmpty(inpermissionLead.Firstname) ? DBNull.Value : (object)inpermissionLead.Firstname);
                    parameters[3] = Database.GetParameter("@Surname", string.IsNullOrEmpty(inpermissionLead.Surname) ? DBNull.Value : (object)inpermissionLead.Surname);
                    parameters[4] = Database.GetParameter("@CellNumber", string.IsNullOrEmpty(inpermissionLead.Cellnumber) ? DBNull.Value : (object)inpermissionLead.Cellnumber);
                    parameters[5] = Database.GetParameter("@AltNumber", string.IsNullOrEmpty(inpermissionLead.AltNumber) ? DBNull.Value : (object)inpermissionLead.AltNumber);
                    parameters[6] = Database.GetParameter("@SavedBy", string.IsNullOrEmpty(inpermissionLead.SavedBy) ? DBNull.Value : (object)inpermissionLead.SavedBy);
                    parameters[7] = Database.GetParameter("@DateSaved", inpermissionLead.DateSaved.HasValue ? (object)inpermissionLead.DateSaved.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@DateOfBirth", inpermissionLead.DateOfBirth.HasValue ? (object)inpermissionLead.DateOfBirth.Value : DBNull.Value);
                    parameters[9] = Database.GetParameter("@Occupation", string.IsNullOrEmpty(inpermissionLead.Occupation) ? DBNull.Value : (object)inpermissionLead.Occupation);

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
        internal static string Search(long? fkinimportid, string title, string firstname, string surname, string cellnumber, string altnumber, string savedby, DateTime? datesaved, DateTime? dateofbirth, string occupation)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPermissionLead].[FKImportID] = " + fkinimportid + "");
            }
            if (title != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPermissionLead].[Title] = " + title + "");
            }
            if (firstname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPermissionLead].[Firstname] = " + firstname + "");
            }
            if (surname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPermissionLead].[Surname] = '" + surname + "");
            }
            if (cellnumber != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPermissionLead].[CellNumber] LIKE '" + cellnumber + "");
            }
            if (altnumber != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPermissionLead].[AltNumber] = " + altnumber + "");
            }
            if (savedby != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPermissionLead].[SavedBy] = " + savedby + "");
            }
            if (datesaved != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPermissionLead].[DateSaved] = " + datesaved + "");
            }
            if (dateofbirth != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPermissionLead].[DateOfBirth] = " + dateofbirth + "");
            }
            if (occupation != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPermissionLead].[Occupation] = " + occupation + "");
            }

            query.Append("SELECT [INPermissionLead].[ID], [INPermissionLead].[FKImportID], [INPermissionLead].[Title], [INPermissionLead].[Firstname], [INPermissionLead].[Surname], [INPermissionLead].[CellNumber], [INPermissionLead].[AltNumber], [INPermissionLead].[SavedBy], [INPermissionLead].[DateSaved], [INPermissionLead].[DateOfBirth], [INPermissionLead].[Occupation], [INPermissionLead].[StampDate], [INPermissionLead].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPermissionLead].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPermissionLead] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion


    }
}
