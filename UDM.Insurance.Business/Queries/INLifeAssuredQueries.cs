using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inlifeassured objects.
    /// </summary>
    internal abstract partial class INLifeAssuredQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inlifeassured from the database.
        /// </summary>
        /// <param name="inlifeassured">The inlifeassured object to delete.</param>
        /// <returns>A query that can be used to delete the inlifeassured from the database.</returns>
        internal static string Delete(INLifeAssured inlifeassured, ref object[] parameters)
        {
            string query = string.Empty;
            if (inlifeassured != null)
            {
                query = "INSERT INTO [zHstINLifeAssured] ([ID], [IDNo], [FKINTitleID], [FirstName], [Surname], [FKGenderID], [DateOfBirth], [FKINRelationshipID], [ToDeleteID], [TelContact], [StampDate], [StampUserID]) SELECT [ID], [IDNo], [FKINTitleID], [FirstName], [Surname], [FKGenderID], [DateOfBirth], [FKINRelationshipID], [ToDeleteID], [TelContact], [StampDate], [StampUserID] FROM [INLifeAssured] WHERE [INLifeAssured].[ID] = @ID; ";
                query += "DELETE FROM [INLifeAssured] WHERE [INLifeAssured].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inlifeassured.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inlifeassured from the database.
        /// </summary>
        /// <param name="inlifeassured">The inlifeassured object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inlifeassured from the database.</returns>
        internal static string DeleteHistory(INLifeAssured inlifeassured, ref object[] parameters)
        {
            string query = string.Empty;
            if (inlifeassured != null)
            {
                query = "DELETE FROM [zHstINLifeAssured] WHERE [zHstINLifeAssured].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inlifeassured.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inlifeassured from the database.
        /// </summary>
        /// <param name="inlifeassured">The inlifeassured object to undelete.</param>
        /// <returns>A query that can be used to undelete the inlifeassured from the database.</returns>
        internal static string UnDelete(INLifeAssured inlifeassured, ref object[] parameters)
        {
            string query = string.Empty;
            if (inlifeassured != null)
            {
                query = "INSERT INTO [INLifeAssured] ([ID], [IDNo], [FKINTitleID], [FirstName], [Surname], [FKGenderID], [DateOfBirth], [FKINRelationshipID], [ToDeleteID], [TelContact], [StampDate], [StampUserID]) SELECT [ID], [IDNo], [FKINTitleID], [FirstName], [Surname], [FKGenderID], [DateOfBirth], [FKINRelationshipID], [ToDeleteID], [TelContact], [StampDate], [StampUserID] FROM [zHstINLifeAssured] WHERE [zHstINLifeAssured].[ID] = @ID AND [zHstINLifeAssured].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINLifeAssured] WHERE [zHstINLifeAssured].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INLifeAssured] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINLifeAssured] WHERE [zHstINLifeAssured].[ID] = @ID AND [zHstINLifeAssured].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINLifeAssured] WHERE [zHstINLifeAssured].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INLifeAssured] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inlifeassured.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inlifeassured object.
        /// </summary>
        /// <param name="inlifeassured">The inlifeassured object to fill.</param>
        /// <returns>A query that can be used to fill the inlifeassured object.</returns>
        internal static string Fill(INLifeAssured inlifeassured, ref object[] parameters)
        {
            string query = string.Empty;
            if (inlifeassured != null)
            {
                query = "SELECT [ID], [IDNo], [FKINTitleID], [FirstName], [Surname], [FKGenderID], [DateOfBirth], [FKINRelationshipID], [ToDeleteID], [TelContact], [StampDate], [StampUserID] FROM [INLifeAssured] WHERE [INLifeAssured].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inlifeassured.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inlifeassured data.
        /// </summary>
        /// <param name="inlifeassured">The inlifeassured to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inlifeassured data.</returns>
        internal static string FillData(INLifeAssured inlifeassured, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inlifeassured != null)
            {
            query.Append("SELECT [INLifeAssured].[ID], [INLifeAssured].[IDNo], [INLifeAssured].[FKINTitleID], [INLifeAssured].[FirstName], [INLifeAssured].[Surname], [INLifeAssured].[FKGenderID], [INLifeAssured].[DateOfBirth], [INLifeAssured].[FKINRelationshipID], [INLifeAssured].[ToDeleteID], [INLifeAssured].[TelContact], [INLifeAssured].[StampDate], [INLifeAssured].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INLifeAssured].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INLifeAssured] ");
                query.Append(" WHERE [INLifeAssured].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inlifeassured.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inlifeassured object from history.
        /// </summary>
        /// <param name="inlifeassured">The inlifeassured object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inlifeassured object from history.</returns>
        internal static string FillHistory(INLifeAssured inlifeassured, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inlifeassured != null)
            {
                query = "SELECT [ID], [IDNo], [FKINTitleID], [FirstName], [Surname], [FKGenderID], [DateOfBirth], [FKINRelationshipID], [ToDeleteID], [TelContact], [StampDate], [StampUserID] FROM [zHstINLifeAssured] WHERE [zHstINLifeAssured].[ID] = @ID AND [zHstINLifeAssured].[StampUserID] = @StampUserID AND [zHstINLifeAssured].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inlifeassured.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inlifeassureds in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inlifeassureds in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INLifeAssured].[ID], [INLifeAssured].[IDNo], [INLifeAssured].[FKINTitleID], [INLifeAssured].[FirstName], [INLifeAssured].[Surname], [INLifeAssured].[FKGenderID], [INLifeAssured].[DateOfBirth], [INLifeAssured].[FKINRelationshipID], [INLifeAssured].[ToDeleteID], [INLifeAssured].[TelContact], [INLifeAssured].[StampDate], [INLifeAssured].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INLifeAssured].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INLifeAssured] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inlifeassureds in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inlifeassureds in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINLifeAssured].[ID], [zHstINLifeAssured].[IDNo], [zHstINLifeAssured].[FKINTitleID], [zHstINLifeAssured].[FirstName], [zHstINLifeAssured].[Surname], [zHstINLifeAssured].[FKGenderID], [zHstINLifeAssured].[DateOfBirth], [zHstINLifeAssured].[FKINRelationshipID], [zHstINLifeAssured].[ToDeleteID], [zHstINLifeAssured].[TelContact], [zHstINLifeAssured].[StampDate], [zHstINLifeAssured].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINLifeAssured].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINLifeAssured] ");
            query.Append("INNER JOIN (SELECT [zHstINLifeAssured].[ID], MAX([zHstINLifeAssured].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINLifeAssured] ");
            query.Append("WHERE [zHstINLifeAssured].[ID] NOT IN (SELECT [INLifeAssured].[ID] FROM [INLifeAssured]) ");
            query.Append("GROUP BY [zHstINLifeAssured].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINLifeAssured].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINLifeAssured].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inlifeassured in the database.
        /// </summary>
        /// <param name="inlifeassured">The inlifeassured object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inlifeassured in the database.</returns>
        public static string ListHistory(INLifeAssured inlifeassured, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inlifeassured != null)
            {
            query.Append("SELECT [zHstINLifeAssured].[ID], [zHstINLifeAssured].[IDNo], [zHstINLifeAssured].[FKINTitleID], [zHstINLifeAssured].[FirstName], [zHstINLifeAssured].[Surname], [zHstINLifeAssured].[FKGenderID], [zHstINLifeAssured].[DateOfBirth], [zHstINLifeAssured].[FKINRelationshipID], [zHstINLifeAssured].[ToDeleteID], [zHstINLifeAssured].[TelContact], [zHstINLifeAssured].[StampDate], [zHstINLifeAssured].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINLifeAssured].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINLifeAssured] ");
                query.Append(" WHERE [zHstINLifeAssured].[ID] = @ID");
                query.Append(" ORDER BY [zHstINLifeAssured].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inlifeassured.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inlifeassured to the database.
        /// </summary>
        /// <param name="inlifeassured">The inlifeassured to save.</param>
        /// <returns>A query that can be used to save the inlifeassured to the database.</returns>
        internal static string Save(INLifeAssured inlifeassured, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inlifeassured != null)
            {
                if (inlifeassured.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINLifeAssured] ([ID], [IDNo], [FKINTitleID], [FirstName], [Surname], [FKGenderID], [DateOfBirth], [FKINRelationshipID], [ToDeleteID], [TelContact], [StampDate], [StampUserID]) SELECT [ID], [IDNo], [FKINTitleID], [FirstName], [Surname], [FKGenderID], [DateOfBirth], [FKINRelationshipID], [ToDeleteID], [TelContact], [StampDate], [StampUserID] FROM [INLifeAssured] WHERE [INLifeAssured].[ID] = @ID; ");
                    query.Append("UPDATE [INLifeAssured]");
                    parameters = new object[10];
                    query.Append(" SET [IDNo] = @IDNo");
                    parameters[0] = Database.GetParameter("@IDNo", string.IsNullOrEmpty(inlifeassured.IDNo) ? DBNull.Value : (object)inlifeassured.IDNo);
                    query.Append(", [FKINTitleID] = @FKINTitleID");
                    parameters[1] = Database.GetParameter("@FKINTitleID", inlifeassured.FKINTitleID.HasValue ? (object)inlifeassured.FKINTitleID.Value : DBNull.Value);
                    query.Append(", [FirstName] = @FirstName");
                    parameters[2] = Database.GetParameter("@FirstName", string.IsNullOrEmpty(inlifeassured.FirstName) ? DBNull.Value : (object)inlifeassured.FirstName);
                    query.Append(", [Surname] = @Surname");
                    parameters[3] = Database.GetParameter("@Surname", string.IsNullOrEmpty(inlifeassured.Surname) ? DBNull.Value : (object)inlifeassured.Surname);
                    query.Append(", [FKGenderID] = @FKGenderID");
                    parameters[4] = Database.GetParameter("@FKGenderID", inlifeassured.FKGenderID.HasValue ? (object)inlifeassured.FKGenderID.Value : DBNull.Value);
                    query.Append(", [DateOfBirth] = @DateOfBirth");
                    parameters[5] = Database.GetParameter("@DateOfBirth", inlifeassured.DateOfBirth.HasValue ? (object)inlifeassured.DateOfBirth.Value : DBNull.Value);
                    query.Append(", [FKINRelationshipID] = @FKINRelationshipID");
                    parameters[6] = Database.GetParameter("@FKINRelationshipID", inlifeassured.FKINRelationshipID.HasValue ? (object)inlifeassured.FKINRelationshipID.Value : DBNull.Value);
                    query.Append(", [ToDeleteID] = @ToDeleteID");
                    parameters[7] = Database.GetParameter("@ToDeleteID", inlifeassured.ToDeleteID.HasValue ? (object)inlifeassured.ToDeleteID.Value : DBNull.Value);
                    query.Append(", [TelContact] = @TelContact");
                    parameters[8] = Database.GetParameter("@TelContact", string.IsNullOrEmpty(inlifeassured.TelContact) ? DBNull.Value : (object)inlifeassured.TelContact);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INLifeAssured].[ID] = @ID"); 
                    parameters[9] = Database.GetParameter("@ID", inlifeassured.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INLifeAssured] ([IDNo], [FKINTitleID], [FirstName], [Surname], [FKGenderID], [DateOfBirth], [FKINRelationshipID], [ToDeleteID], [TelContact], [StampDate], [StampUserID]) VALUES(@IDNo, @FKINTitleID, @FirstName, @Surname, @FKGenderID, @DateOfBirth, @FKINRelationshipID, @ToDeleteID, @TelContact, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[9];
                    parameters[0] = Database.GetParameter("@IDNo", string.IsNullOrEmpty(inlifeassured.IDNo) ? DBNull.Value : (object)inlifeassured.IDNo);
                    parameters[1] = Database.GetParameter("@FKINTitleID", inlifeassured.FKINTitleID.HasValue ? (object)inlifeassured.FKINTitleID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@FirstName", string.IsNullOrEmpty(inlifeassured.FirstName) ? DBNull.Value : (object)inlifeassured.FirstName);
                    parameters[3] = Database.GetParameter("@Surname", string.IsNullOrEmpty(inlifeassured.Surname) ? DBNull.Value : (object)inlifeassured.Surname);
                    parameters[4] = Database.GetParameter("@FKGenderID", inlifeassured.FKGenderID.HasValue ? (object)inlifeassured.FKGenderID.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@DateOfBirth", inlifeassured.DateOfBirth.HasValue ? (object)inlifeassured.DateOfBirth.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@FKINRelationshipID", inlifeassured.FKINRelationshipID.HasValue ? (object)inlifeassured.FKINRelationshipID.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@ToDeleteID", inlifeassured.ToDeleteID.HasValue ? (object)inlifeassured.ToDeleteID.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@TelContact", string.IsNullOrEmpty(inlifeassured.TelContact) ? DBNull.Value : (object)inlifeassured.TelContact);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inlifeassureds that match the search criteria.
        /// </summary>
        /// <param name="idno">The idno search criteria.</param>
        /// <param name="fkintitleid">The fkintitleid search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="fkgenderid">The fkgenderid search criteria.</param>
        /// <param name="dateofbirth">The dateofbirth search criteria.</param>
        /// <param name="fkinrelationshipid">The fkinrelationshipid search criteria.</param>
        /// <param name="todeleteid">The todeleteid search criteria.</param>
        /// <param name="telcontact">The telcontact search criteria.</param>
        /// <returns>A query that can be used to search for inlifeassureds based on the search criteria.</returns>
        internal static string Search(string idno, long? fkintitleid, string firstname, string surname, long? fkgenderid, DateTime? dateofbirth, long? fkinrelationshipid, long? todeleteid, string telcontact)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (idno != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLifeAssured].[IDNo] LIKE '" + idno.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkintitleid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLifeAssured].[FKINTitleID] = " + fkintitleid + "");
            }
            if (firstname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLifeAssured].[FirstName] LIKE '" + firstname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (surname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLifeAssured].[Surname] LIKE '" + surname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkgenderid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLifeAssured].[FKGenderID] = " + fkgenderid + "");
            }
            if (dateofbirth != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLifeAssured].[DateOfBirth] = '" + dateofbirth.Value.ToString(Database.DateFormat) + "'");
            }
            if (fkinrelationshipid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLifeAssured].[FKINRelationshipID] = " + fkinrelationshipid + "");
            }
            if (todeleteid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLifeAssured].[ToDeleteID] = " + todeleteid + "");
            }
            if (telcontact != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLifeAssured].[TelContact] LIKE '" + telcontact.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INLifeAssured].[ID], [INLifeAssured].[IDNo], [INLifeAssured].[FKINTitleID], [INLifeAssured].[FirstName], [INLifeAssured].[Surname], [INLifeAssured].[FKGenderID], [INLifeAssured].[DateOfBirth], [INLifeAssured].[FKINRelationshipID], [INLifeAssured].[ToDeleteID], [INLifeAssured].[TelContact], [INLifeAssured].[StampDate], [INLifeAssured].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INLifeAssured].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INLifeAssured] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
