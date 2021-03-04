using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inchild objects.
    /// </summary>
    internal abstract partial class INChildQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inchild from the database.
        /// </summary>
        /// <param name="inchild">The inchild object to delete.</param>
        /// <returns>A query that can be used to delete the inchild from the database.</returns>
        internal static string Delete(INChild inchild, ref object[] parameters)
        {
            string query = string.Empty;
            if (inchild != null)
            {
                query = "INSERT INTO [zHstINChild] ([ID], [FirstName], [DateOfBirth], [ToDeleteID], [StampDate], [StampUserID]) SELECT [ID], [FirstName], [DateOfBirth], [ToDeleteID], [StampDate], [StampUserID] FROM [INChild] WHERE [INChild].[ID] = @ID; ";
                query += "DELETE FROM [INChild] WHERE [INChild].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inchild.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inchild from the database.
        /// </summary>
        /// <param name="inchild">The inchild object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inchild from the database.</returns>
        internal static string DeleteHistory(INChild inchild, ref object[] parameters)
        {
            string query = string.Empty;
            if (inchild != null)
            {
                query = "DELETE FROM [zHstINChild] WHERE [zHstINChild].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inchild.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inchild from the database.
        /// </summary>
        /// <param name="inchild">The inchild object to undelete.</param>
        /// <returns>A query that can be used to undelete the inchild from the database.</returns>
        internal static string UnDelete(INChild inchild, ref object[] parameters)
        {
            string query = string.Empty;
            if (inchild != null)
            {
                query = "INSERT INTO [INChild] ([ID], [FirstName], [DateOfBirth], [ToDeleteID], [StampDate], [StampUserID]) SELECT [ID], [FirstName], [DateOfBirth], [ToDeleteID], [StampDate], [StampUserID] FROM [zHstINChild] WHERE [zHstINChild].[ID] = @ID AND [zHstINChild].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINChild] WHERE [zHstINChild].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INChild] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINChild] WHERE [zHstINChild].[ID] = @ID AND [zHstINChild].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINChild] WHERE [zHstINChild].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INChild] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inchild.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inchild object.
        /// </summary>
        /// <param name="inchild">The inchild object to fill.</param>
        /// <returns>A query that can be used to fill the inchild object.</returns>
        internal static string Fill(INChild inchild, ref object[] parameters)
        {
            string query = string.Empty;
            if (inchild != null)
            {
                query = "SELECT [ID], [FirstName], [DateOfBirth], [ToDeleteID], [StampDate], [StampUserID] FROM [INChild] WHERE [INChild].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inchild.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inchild data.
        /// </summary>
        /// <param name="inchild">The inchild to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inchild data.</returns>
        internal static string FillData(INChild inchild, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inchild != null)
            {
            query.Append("SELECT [INChild].[ID], [INChild].[FirstName], [INChild].[DateOfBirth], [INChild].[ToDeleteID], [INChild].[StampDate], [INChild].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INChild].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INChild] ");
                query.Append(" WHERE [INChild].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inchild.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inchild object from history.
        /// </summary>
        /// <param name="inchild">The inchild object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inchild object from history.</returns>
        internal static string FillHistory(INChild inchild, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inchild != null)
            {
                query = "SELECT [ID], [FirstName], [DateOfBirth], [ToDeleteID], [StampDate], [StampUserID] FROM [zHstINChild] WHERE [zHstINChild].[ID] = @ID AND [zHstINChild].[StampUserID] = @StampUserID AND [zHstINChild].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inchild.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inchilds in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inchilds in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INChild].[ID], [INChild].[FirstName], [INChild].[DateOfBirth], [INChild].[ToDeleteID], [INChild].[StampDate], [INChild].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INChild].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INChild] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inchilds in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inchilds in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINChild].[ID], [zHstINChild].[FirstName], [zHstINChild].[DateOfBirth], [zHstINChild].[ToDeleteID], [zHstINChild].[StampDate], [zHstINChild].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINChild].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINChild] ");
            query.Append("INNER JOIN (SELECT [zHstINChild].[ID], MAX([zHstINChild].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINChild] ");
            query.Append("WHERE [zHstINChild].[ID] NOT IN (SELECT [INChild].[ID] FROM [INChild]) ");
            query.Append("GROUP BY [zHstINChild].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINChild].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINChild].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inchild in the database.
        /// </summary>
        /// <param name="inchild">The inchild object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inchild in the database.</returns>
        public static string ListHistory(INChild inchild, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inchild != null)
            {
            query.Append("SELECT [zHstINChild].[ID], [zHstINChild].[FirstName], [zHstINChild].[DateOfBirth], [zHstINChild].[ToDeleteID], [zHstINChild].[StampDate], [zHstINChild].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINChild].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINChild] ");
                query.Append(" WHERE [zHstINChild].[ID] = @ID");
                query.Append(" ORDER BY [zHstINChild].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inchild.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inchild to the database.
        /// </summary>
        /// <param name="inchild">The inchild to save.</param>
        /// <returns>A query that can be used to save the inchild to the database.</returns>
        internal static string Save(INChild inchild, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inchild != null)
            {
                if (inchild.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINChild] ([ID], [FirstName], [DateOfBirth], [ToDeleteID], [StampDate], [StampUserID]) SELECT [ID], [FirstName], [DateOfBirth], [ToDeleteID], [StampDate], [StampUserID] FROM [INChild] WHERE [INChild].[ID] = @ID; ");
                    query.Append("UPDATE [INChild]");
                    parameters = new object[4];
                    query.Append(" SET [FirstName] = @FirstName");
                    parameters[0] = Database.GetParameter("@FirstName", string.IsNullOrEmpty(inchild.FirstName) ? DBNull.Value : (object)inchild.FirstName);
                    query.Append(", [DateOfBirth] = @DateOfBirth");
                    parameters[1] = Database.GetParameter("@DateOfBirth", inchild.DateOfBirth.HasValue ? (object)inchild.DateOfBirth.Value : DBNull.Value);
                    query.Append(", [ToDeleteID] = @ToDeleteID");
                    parameters[2] = Database.GetParameter("@ToDeleteID", inchild.ToDeleteID.HasValue ? (object)inchild.ToDeleteID.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INChild].[ID] = @ID"); 
                    parameters[3] = Database.GetParameter("@ID", inchild.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INChild] ([FirstName], [DateOfBirth], [ToDeleteID], [StampDate], [StampUserID]) VALUES(@FirstName, @DateOfBirth, @ToDeleteID, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@FirstName", string.IsNullOrEmpty(inchild.FirstName) ? DBNull.Value : (object)inchild.FirstName);
                    parameters[1] = Database.GetParameter("@DateOfBirth", inchild.DateOfBirth.HasValue ? (object)inchild.DateOfBirth.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@ToDeleteID", inchild.ToDeleteID.HasValue ? (object)inchild.ToDeleteID.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inchilds that match the search criteria.
        /// </summary>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="dateofbirth">The dateofbirth search criteria.</param>
        /// <param name="todeleteid">The todeleteid search criteria.</param>
        /// <returns>A query that can be used to search for inchilds based on the search criteria.</returns>
        internal static string Search(string firstname, DateTime? dateofbirth, long? todeleteid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (firstname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INChild].[FirstName] LIKE '" + firstname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (dateofbirth != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INChild].[DateOfBirth] = '" + dateofbirth.Value.ToString(Database.DateFormat) + "'");
            }
            if (todeleteid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INChild].[ToDeleteID] = " + todeleteid + "");
            }
            query.Append("SELECT [INChild].[ID], [INChild].[FirstName], [INChild].[DateOfBirth], [INChild].[ToDeleteID], [INChild].[StampDate], [INChild].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INChild].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INChild] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
