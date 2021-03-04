using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inbeneficiary objects.
    /// </summary>
    internal abstract partial class INBeneficiaryQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inbeneficiary from the database.
        /// </summary>
        /// <param name="inbeneficiary">The inbeneficiary object to delete.</param>
        /// <returns>A query that can be used to delete the inbeneficiary from the database.</returns>
        internal static string Delete(INBeneficiary inbeneficiary, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbeneficiary != null)
            {
                query = "INSERT INTO [zHstINBeneficiary] ([ID], [IDNo], [FKINTitleID], [FKGenderID], [Initials], [FirstName], [Surname], [FKINRelationshipID], [DateOfBirth], [ToDeleteID], [Notes], [TelContact], [StampDate], [StampUserID]) SELECT [ID], [IDNo], [FKINTitleID], [FKGenderID], [Initials], [FirstName], [Surname], [FKINRelationshipID], [DateOfBirth], [ToDeleteID], [Notes], [TelContact], [StampDate], [StampUserID] FROM [INBeneficiary] WHERE [INBeneficiary].[ID] = @ID; ";
                query += "DELETE FROM [INBeneficiary] WHERE [INBeneficiary].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbeneficiary.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inbeneficiary from the database.
        /// </summary>
        /// <param name="inbeneficiary">The inbeneficiary object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inbeneficiary from the database.</returns>
        internal static string DeleteHistory(INBeneficiary inbeneficiary, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbeneficiary != null)
            {
                query = "DELETE FROM [zHstINBeneficiary] WHERE [zHstINBeneficiary].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbeneficiary.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inbeneficiary from the database.
        /// </summary>
        /// <param name="inbeneficiary">The inbeneficiary object to undelete.</param>
        /// <returns>A query that can be used to undelete the inbeneficiary from the database.</returns>
        internal static string UnDelete(INBeneficiary inbeneficiary, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbeneficiary != null)
            {
                query = "INSERT INTO [INBeneficiary] ([ID], [IDNo], [FKINTitleID], [FKGenderID], [Initials], [FirstName], [Surname], [FKINRelationshipID], [DateOfBirth], [ToDeleteID], [Notes], [TelContact], [StampDate], [StampUserID]) SELECT [ID], [IDNo], [FKINTitleID], [FKGenderID], [Initials], [FirstName], [Surname], [FKINRelationshipID], [DateOfBirth], [ToDeleteID], [Notes], [TelContact], [StampDate], [StampUserID] FROM [zHstINBeneficiary] WHERE [zHstINBeneficiary].[ID] = @ID AND [zHstINBeneficiary].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINBeneficiary] WHERE [zHstINBeneficiary].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INBeneficiary] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINBeneficiary] WHERE [zHstINBeneficiary].[ID] = @ID AND [zHstINBeneficiary].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINBeneficiary] WHERE [zHstINBeneficiary].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INBeneficiary] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbeneficiary.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inbeneficiary object.
        /// </summary>
        /// <param name="inbeneficiary">The inbeneficiary object to fill.</param>
        /// <returns>A query that can be used to fill the inbeneficiary object.</returns>
        internal static string Fill(INBeneficiary inbeneficiary, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbeneficiary != null)
            {
                query = "SELECT [ID], [IDNo], [FKINTitleID], [FKGenderID], [Initials], [FirstName], [Surname], [FKINRelationshipID], [DateOfBirth], [ToDeleteID], [Notes], [TelContact], [StampDate], [StampUserID] FROM [INBeneficiary] WHERE [INBeneficiary].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbeneficiary.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inbeneficiary data.
        /// </summary>
        /// <param name="inbeneficiary">The inbeneficiary to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inbeneficiary data.</returns>
        internal static string FillData(INBeneficiary inbeneficiary, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inbeneficiary != null)
            {
            query.Append("SELECT [INBeneficiary].[ID], [INBeneficiary].[IDNo], [INBeneficiary].[FKINTitleID], [INBeneficiary].[FKGenderID], [INBeneficiary].[Initials], [INBeneficiary].[FirstName], [INBeneficiary].[Surname], [INBeneficiary].[FKINRelationshipID], [INBeneficiary].[DateOfBirth], [INBeneficiary].[ToDeleteID], [INBeneficiary].[Notes], [INBeneficiary].[TelContact], [INBeneficiary].[StampDate], [INBeneficiary].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INBeneficiary].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INBeneficiary] ");
                query.Append(" WHERE [INBeneficiary].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbeneficiary.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inbeneficiary object from history.
        /// </summary>
        /// <param name="inbeneficiary">The inbeneficiary object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inbeneficiary object from history.</returns>
        internal static string FillHistory(INBeneficiary inbeneficiary, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbeneficiary != null)
            {
                query = "SELECT [ID], [IDNo], [FKINTitleID], [FKGenderID], [Initials], [FirstName], [Surname], [FKINRelationshipID], [DateOfBirth], [ToDeleteID], [Notes], [TelContact], [StampDate], [StampUserID] FROM [zHstINBeneficiary] WHERE [zHstINBeneficiary].[ID] = @ID AND [zHstINBeneficiary].[StampUserID] = @StampUserID AND [zHstINBeneficiary].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inbeneficiary.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inbeneficiarys in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inbeneficiarys in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INBeneficiary].[ID], [INBeneficiary].[IDNo], [INBeneficiary].[FKINTitleID], [INBeneficiary].[FKGenderID], [INBeneficiary].[Initials], [INBeneficiary].[FirstName], [INBeneficiary].[Surname], [INBeneficiary].[FKINRelationshipID], [INBeneficiary].[DateOfBirth], [INBeneficiary].[ToDeleteID], [INBeneficiary].[Notes], [INBeneficiary].[TelContact], [INBeneficiary].[StampDate], [INBeneficiary].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INBeneficiary].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INBeneficiary] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inbeneficiarys in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inbeneficiarys in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINBeneficiary].[ID], [zHstINBeneficiary].[IDNo], [zHstINBeneficiary].[FKINTitleID], [zHstINBeneficiary].[FKGenderID], [zHstINBeneficiary].[Initials], [zHstINBeneficiary].[FirstName], [zHstINBeneficiary].[Surname], [zHstINBeneficiary].[FKINRelationshipID], [zHstINBeneficiary].[DateOfBirth], [zHstINBeneficiary].[ToDeleteID], [zHstINBeneficiary].[Notes], [zHstINBeneficiary].[TelContact], [zHstINBeneficiary].[StampDate], [zHstINBeneficiary].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINBeneficiary].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINBeneficiary] ");
            query.Append("INNER JOIN (SELECT [zHstINBeneficiary].[ID], MAX([zHstINBeneficiary].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINBeneficiary] ");
            query.Append("WHERE [zHstINBeneficiary].[ID] NOT IN (SELECT [INBeneficiary].[ID] FROM [INBeneficiary]) ");
            query.Append("GROUP BY [zHstINBeneficiary].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINBeneficiary].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINBeneficiary].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inbeneficiary in the database.
        /// </summary>
        /// <param name="inbeneficiary">The inbeneficiary object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inbeneficiary in the database.</returns>
        public static string ListHistory(INBeneficiary inbeneficiary, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inbeneficiary != null)
            {
            query.Append("SELECT [zHstINBeneficiary].[ID], [zHstINBeneficiary].[IDNo], [zHstINBeneficiary].[FKINTitleID], [zHstINBeneficiary].[FKGenderID], [zHstINBeneficiary].[Initials], [zHstINBeneficiary].[FirstName], [zHstINBeneficiary].[Surname], [zHstINBeneficiary].[FKINRelationshipID], [zHstINBeneficiary].[DateOfBirth], [zHstINBeneficiary].[ToDeleteID], [zHstINBeneficiary].[Notes], [zHstINBeneficiary].[TelContact], [zHstINBeneficiary].[StampDate], [zHstINBeneficiary].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINBeneficiary].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINBeneficiary] ");
                query.Append(" WHERE [zHstINBeneficiary].[ID] = @ID");
                query.Append(" ORDER BY [zHstINBeneficiary].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbeneficiary.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inbeneficiary to the database.
        /// </summary>
        /// <param name="inbeneficiary">The inbeneficiary to save.</param>
        /// <returns>A query that can be used to save the inbeneficiary to the database.</returns>
        internal static string Save(INBeneficiary inbeneficiary, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inbeneficiary != null)
            {
                if (inbeneficiary.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINBeneficiary] ([ID], [IDNo], [FKINTitleID], [FKGenderID], [Initials], [FirstName], [Surname], [FKINRelationshipID], [DateOfBirth], [ToDeleteID], [Notes], [TelContact], [StampDate], [StampUserID]) SELECT [ID], [IDNo], [FKINTitleID], [FKGenderID], [Initials], [FirstName], [Surname], [FKINRelationshipID], [DateOfBirth], [ToDeleteID], [Notes], [TelContact], [StampDate], [StampUserID] FROM [INBeneficiary] WHERE [INBeneficiary].[ID] = @ID; ");
                    query.Append("UPDATE [INBeneficiary]");
                    parameters = new object[12];
                    query.Append(" SET [IDNo] = @IDNo");
                    parameters[0] = Database.GetParameter("@IDNo", string.IsNullOrEmpty(inbeneficiary.IDNo) ? DBNull.Value : (object)inbeneficiary.IDNo);
                    query.Append(", [FKINTitleID] = @FKINTitleID");
                    parameters[1] = Database.GetParameter("@FKINTitleID", inbeneficiary.FKINTitleID.HasValue ? (object)inbeneficiary.FKINTitleID.Value : DBNull.Value);
                    query.Append(", [FKGenderID] = @FKGenderID");
                    parameters[2] = Database.GetParameter("@FKGenderID", inbeneficiary.FKGenderID.HasValue ? (object)inbeneficiary.FKGenderID.Value : DBNull.Value);
                    query.Append(", [Initials] = @Initials");
                    parameters[3] = Database.GetParameter("@Initials", string.IsNullOrEmpty(inbeneficiary.Initials) ? DBNull.Value : (object)inbeneficiary.Initials);
                    query.Append(", [FirstName] = @FirstName");
                    parameters[4] = Database.GetParameter("@FirstName", string.IsNullOrEmpty(inbeneficiary.FirstName) ? DBNull.Value : (object)inbeneficiary.FirstName);
                    query.Append(", [Surname] = @Surname");
                    parameters[5] = Database.GetParameter("@Surname", string.IsNullOrEmpty(inbeneficiary.Surname) ? DBNull.Value : (object)inbeneficiary.Surname);
                    query.Append(", [FKINRelationshipID] = @FKINRelationshipID");
                    parameters[6] = Database.GetParameter("@FKINRelationshipID", inbeneficiary.FKINRelationshipID.HasValue ? (object)inbeneficiary.FKINRelationshipID.Value : DBNull.Value);
                    query.Append(", [DateOfBirth] = @DateOfBirth");
                    parameters[7] = Database.GetParameter("@DateOfBirth", inbeneficiary.DateOfBirth.HasValue ? (object)inbeneficiary.DateOfBirth.Value : DBNull.Value);
                    query.Append(", [ToDeleteID] = @ToDeleteID");
                    parameters[8] = Database.GetParameter("@ToDeleteID", inbeneficiary.ToDeleteID.HasValue ? (object)inbeneficiary.ToDeleteID.Value : DBNull.Value);
                    query.Append(", [Notes] = @Notes");
                    parameters[9] = Database.GetParameter("@Notes", string.IsNullOrEmpty(inbeneficiary.Notes) ? DBNull.Value : (object)inbeneficiary.Notes);
                    query.Append(", [TelContact] = @TelContact");
                    parameters[10] = Database.GetParameter("@TelContact", string.IsNullOrEmpty(inbeneficiary.TelContact) ? DBNull.Value : (object)inbeneficiary.TelContact);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INBeneficiary].[ID] = @ID"); 
                    parameters[11] = Database.GetParameter("@ID", inbeneficiary.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INBeneficiary] ([IDNo], [FKINTitleID], [FKGenderID], [Initials], [FirstName], [Surname], [FKINRelationshipID], [DateOfBirth], [ToDeleteID], [Notes], [TelContact], [StampDate], [StampUserID]) VALUES(@IDNo, @FKINTitleID, @FKGenderID, @Initials, @FirstName, @Surname, @FKINRelationshipID, @DateOfBirth, @ToDeleteID, @Notes, @TelContact, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[11];
                    parameters[0] = Database.GetParameter("@IDNo", string.IsNullOrEmpty(inbeneficiary.IDNo) ? DBNull.Value : (object)inbeneficiary.IDNo);
                    parameters[1] = Database.GetParameter("@FKINTitleID", inbeneficiary.FKINTitleID.HasValue ? (object)inbeneficiary.FKINTitleID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@FKGenderID", inbeneficiary.FKGenderID.HasValue ? (object)inbeneficiary.FKGenderID.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@Initials", string.IsNullOrEmpty(inbeneficiary.Initials) ? DBNull.Value : (object)inbeneficiary.Initials);
                    parameters[4] = Database.GetParameter("@FirstName", string.IsNullOrEmpty(inbeneficiary.FirstName) ? DBNull.Value : (object)inbeneficiary.FirstName);
                    parameters[5] = Database.GetParameter("@Surname", string.IsNullOrEmpty(inbeneficiary.Surname) ? DBNull.Value : (object)inbeneficiary.Surname);
                    parameters[6] = Database.GetParameter("@FKINRelationshipID", inbeneficiary.FKINRelationshipID.HasValue ? (object)inbeneficiary.FKINRelationshipID.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@DateOfBirth", inbeneficiary.DateOfBirth.HasValue ? (object)inbeneficiary.DateOfBirth.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@ToDeleteID", inbeneficiary.ToDeleteID.HasValue ? (object)inbeneficiary.ToDeleteID.Value : DBNull.Value);
                    parameters[9] = Database.GetParameter("@Notes", string.IsNullOrEmpty(inbeneficiary.Notes) ? DBNull.Value : (object)inbeneficiary.Notes);
                    parameters[10] = Database.GetParameter("@TelContact", string.IsNullOrEmpty(inbeneficiary.TelContact) ? DBNull.Value : (object)inbeneficiary.TelContact);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inbeneficiarys that match the search criteria.
        /// </summary>
        /// <param name="idno">The idno search criteria.</param>
        /// <param name="fkintitleid">The fkintitleid search criteria.</param>
        /// <param name="fkgenderid">The fkgenderid search criteria.</param>
        /// <param name="initials">The initials search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="fkinrelationshipid">The fkinrelationshipid search criteria.</param>
        /// <param name="dateofbirth">The dateofbirth search criteria.</param>
        /// <param name="todeleteid">The todeleteid search criteria.</param>
        /// <param name="notes">The notes search criteria.</param>
        /// <param name="telcontact">The telcontact search criteria.</param>
        /// <returns>A query that can be used to search for inbeneficiarys based on the search criteria.</returns>
        internal static string Search(string idno, long? fkintitleid, long? fkgenderid, string initials, string firstname, string surname, long? fkinrelationshipid, DateTime? dateofbirth, long? todeleteid, string notes, string telcontact)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (idno != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBeneficiary].[IDNo] LIKE '" + idno.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkintitleid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBeneficiary].[FKINTitleID] = " + fkintitleid + "");
            }
            if (fkgenderid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBeneficiary].[FKGenderID] = " + fkgenderid + "");
            }
            if (initials != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBeneficiary].[Initials] LIKE '" + initials.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (firstname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBeneficiary].[FirstName] LIKE '" + firstname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (surname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBeneficiary].[Surname] LIKE '" + surname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkinrelationshipid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBeneficiary].[FKINRelationshipID] = " + fkinrelationshipid + "");
            }
            if (dateofbirth != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBeneficiary].[DateOfBirth] = '" + dateofbirth.Value.ToString(Database.DateFormat) + "'");
            }
            if (todeleteid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBeneficiary].[ToDeleteID] = " + todeleteid + "");
            }
            if (notes != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBeneficiary].[Notes] LIKE '" + notes.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (telcontact != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBeneficiary].[TelContact] LIKE '" + telcontact.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INBeneficiary].[ID], [INBeneficiary].[IDNo], [INBeneficiary].[FKINTitleID], [INBeneficiary].[FKGenderID], [INBeneficiary].[Initials], [INBeneficiary].[FirstName], [INBeneficiary].[Surname], [INBeneficiary].[FKINRelationshipID], [INBeneficiary].[DateOfBirth], [INBeneficiary].[ToDeleteID], [INBeneficiary].[Notes], [INBeneficiary].[TelContact], [INBeneficiary].[StampDate], [INBeneficiary].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INBeneficiary].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INBeneficiary] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
