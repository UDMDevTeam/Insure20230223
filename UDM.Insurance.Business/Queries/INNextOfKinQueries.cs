using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to innextofkin objects.
    /// </summary>
    internal abstract partial class INNextOfKinQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) innextofkin from the database.
        /// </summary>
        /// <param name="innextofkin">The innextofkin object to delete.</param>
        /// <returns>A query that can be used to delete the innextofkin from the database.</returns>
        internal static string Delete(INNextOfKin innextofkin, ref object[] parameters)
        {
            string query = string.Empty;
            if (innextofkin != null)
            {
                query = "INSERT INTO [zHstINNextOfKin] ([ID], [FKINImportID], [FKINRelationshipID], [FirstName], [Surname], [TelContact], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKINRelationshipID], [FirstName], [Surname], [TelContact], [StampDate], [StampUserID] FROM [INNextOfKin] WHERE [INNextOfKin].[ID] = @ID; ";
                query += $"DELETE FROM [INNextOfKin] WHERE [INNextOfKin].[ID] = {innextofkin.ID}; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", innextofkin.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) innextofkin from the database.
        /// </summary>
        /// <param name="innextofkin">The innextofkin object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the innextofkin from the database.</returns>
        internal static string DeleteHistory(INNextOfKin innextofkin, ref object[] parameters)
        {
            string query = string.Empty;
            if (innextofkin != null)
            {
                query = "DELETE FROM [zHstINNextOfKin] WHERE [zHstINNextOfKin].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", innextofkin.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) innextofkin from the database.
        /// </summary>
        /// <param name="innextofkin">The innextofkin object to undelete.</param>
        /// <returns>A query that can be used to undelete the innextofkin from the database.</returns>
        internal static string UnDelete(INNextOfKin innextofkin, ref object[] parameters)
        {
            string query = string.Empty;
            if (innextofkin != null)
            {
                query = "INSERT INTO [INNextOfKin] ([ID], [FKINImportID], [FKINRelationshipID], [FirstName], [Surname], [TelContact], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKINRelationshipID], [FirstName], [Surname], [TelContact], [StampDate], [StampUserID] FROM [zHstINNextOfKin] WHERE [zHstINNextOfKin].[ID] = @ID AND [zHstINNextOfKin].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINNextOfKin] WHERE [zHstINNextOfKin].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INNextOfKin] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINNextOfKin] WHERE [zHstINNextOfKin].[ID] = @ID AND [zHstINNextOfKin].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINNextOfKin] WHERE [zHstINNextOfKin].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INNextOfKin] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", innextofkin.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an innextofkin object.
        /// </summary>
        /// <param name="innextofkin">The innextofkin object to fill.</param>
        /// <returns>A query that can be used to fill the innextofkin object.</returns>
        internal static string Fill(INNextOfKin innextofkin, ref object[] parameters)
        {
            string query = string.Empty;
            if (innextofkin != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKINRelationshipID], [FirstName], [Surname], [TelContact], [StampDate], [StampUserID] FROM [INNextOfKin] WHERE [INNextOfKin].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", innextofkin.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  innextofkin data.
        /// </summary>
        /// <param name="innextofkin">The innextofkin to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  innextofkin data.</returns>
        internal static string FillData(INNextOfKin innextofkin, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (innextofkin != null)
            {
            query.Append("SELECT [INNextOfKin].[ID], [INNextOfKin].[FKINImportID], [INNextOfKin].[FKINRelationshipID], [INNextOfKin].[FirstName], [INNextOfKin].[Surname], [INNextOfKin].[TelContact], [INNextOfKin].[StampDate], [INNextOfKin].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INNextOfKin].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INNextOfKin] ");
                query.Append(" WHERE [INNextOfKin].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", innextofkin.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an innextofkin object from history.
        /// </summary>
        /// <param name="innextofkin">The innextofkin object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the innextofkin object from history.</returns>
        internal static string FillHistory(INNextOfKin innextofkin, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (innextofkin != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKINRelationshipID], [FirstName], [Surname], [TelContact], [StampDate], [StampUserID] FROM [zHstINNextOfKin] WHERE [zHstINNextOfKin].[ID] = @ID AND [zHstINNextOfKin].[StampUserID] = @StampUserID AND [zHstINNextOfKin].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", innextofkin.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the innextofkins in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the innextofkins in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INNextOfKin].[ID], [INNextOfKin].[FKINImportID], [INNextOfKin].[FKINRelationshipID], [INNextOfKin].[FirstName], [INNextOfKin].[Surname], [INNextOfKin].[TelContact], [INNextOfKin].[StampDate], [INNextOfKin].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INNextOfKin].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INNextOfKin] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted innextofkins in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted innextofkins in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINNextOfKin].[ID], [zHstINNextOfKin].[FKINImportID], [zHstINNextOfKin].[FKINRelationshipID], [zHstINNextOfKin].[FirstName], [zHstINNextOfKin].[Surname], [zHstINNextOfKin].[TelContact], [zHstINNextOfKin].[StampDate], [zHstINNextOfKin].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINNextOfKin].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINNextOfKin] ");
            query.Append("INNER JOIN (SELECT [zHstINNextOfKin].[ID], MAX([zHstINNextOfKin].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINNextOfKin] ");
            query.Append("WHERE [zHstINNextOfKin].[ID] NOT IN (SELECT [INNextOfKin].[ID] FROM [INNextOfKin]) ");
            query.Append("GROUP BY [zHstINNextOfKin].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINNextOfKin].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINNextOfKin].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) innextofkin in the database.
        /// </summary>
        /// <param name="innextofkin">The innextofkin object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) innextofkin in the database.</returns>
        public static string ListHistory(INNextOfKin innextofkin, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (innextofkin != null)
            {
            query.Append("SELECT [zHstINNextOfKin].[ID], [zHstINNextOfKin].[FKINImportID], [zHstINNextOfKin].[FKINRelationshipID], [zHstINNextOfKin].[FirstName], [zHstINNextOfKin].[Surname], [zHstINNextOfKin].[TelContact], [zHstINNextOfKin].[StampDate], [zHstINNextOfKin].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINNextOfKin].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINNextOfKin] ");
                query.Append(" WHERE [zHstINNextOfKin].[ID] = @ID");
                query.Append(" ORDER BY [zHstINNextOfKin].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", innextofkin.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) innextofkin to the database.
        /// </summary>
        /// <param name="innextofkin">The innextofkin to save.</param>
        /// <returns>A query that can be used to save the innextofkin to the database.</returns>
        internal static string Save(INNextOfKin innextofkin, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (innextofkin != null)
            {
                if (innextofkin.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINNextOfKin] ([ID], [FKINImportID], [FKINRelationshipID], [FirstName], [Surname], [TelContact], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKINRelationshipID], [FirstName], [Surname], [TelContact], [StampDate], [StampUserID] FROM [INNextOfKin] WHERE [INNextOfKin].[ID] = @ID; ");
                    query.Append("UPDATE [INNextOfKin]");
                    parameters = new object[6];
                    query.Append(" SET [FKINImportID] = @FKINImportID");
                    parameters[0] = Database.GetParameter("@FKINImportID", innextofkin.FKINImportID.HasValue ? (object)innextofkin.FKINImportID.Value : DBNull.Value);
                    query.Append(", [FKINRelationshipID] = @FKINRelationshipID");
                    parameters[1] = Database.GetParameter("@FKINRelationshipID", innextofkin.FKINRelationshipID.HasValue ? (object)innextofkin.FKINRelationshipID.Value : DBNull.Value);
                    query.Append(", [FirstName] = @FirstName");
                    parameters[2] = Database.GetParameter("@FirstName", string.IsNullOrEmpty(innextofkin.FirstName) ? DBNull.Value : (object)innextofkin.FirstName);
                    query.Append(", [Surname] = @Surname");
                    parameters[3] = Database.GetParameter("@Surname", string.IsNullOrEmpty(innextofkin.Surname) ? DBNull.Value : (object)innextofkin.Surname);
                    query.Append(", [TelContact] = @TelContact");
                    parameters[4] = Database.GetParameter("@TelContact", string.IsNullOrEmpty(innextofkin.TelContact) ? DBNull.Value : (object)innextofkin.TelContact);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INNextOfKin].[ID] = @ID"); 
                    parameters[5] = Database.GetParameter("@ID", innextofkin.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INNextOfKin] ([FKINImportID], [FKINRelationshipID], [FirstName], [Surname], [TelContact], [StampDate], [StampUserID]) VALUES(@FKINImportID, @FKINRelationshipID, @FirstName, @Surname, @TelContact, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[5];
                    parameters[0] = Database.GetParameter("@FKINImportID", innextofkin.FKINImportID.HasValue ? (object)innextofkin.FKINImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKINRelationshipID", innextofkin.FKINRelationshipID.HasValue ? (object)innextofkin.FKINRelationshipID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@FirstName", string.IsNullOrEmpty(innextofkin.FirstName) ? DBNull.Value : (object)innextofkin.FirstName);
                    parameters[3] = Database.GetParameter("@Surname", string.IsNullOrEmpty(innextofkin.Surname) ? DBNull.Value : (object)innextofkin.Surname);
                    parameters[4] = Database.GetParameter("@TelContact", string.IsNullOrEmpty(innextofkin.TelContact) ? DBNull.Value : (object)innextofkin.TelContact);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for innextofkins that match the search criteria.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkinrelationshipid">The fkinrelationshipid search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="telcontact">The telcontact search criteria.</param>
        /// <returns>A query that can be used to search for innextofkins based on the search criteria.</returns>
        internal static string Search(long? fkinimportid, long? fkinrelationshipid, string firstname, string surname, string telcontact)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INNextOfKin].[FKINImportID] = " + fkinimportid + "");
            }
            if (fkinrelationshipid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INNextOfKin].[FKINRelationshipID] = " + fkinrelationshipid + "");
            }
            if (firstname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INNextOfKin].[FirstName] LIKE '" + firstname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (surname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INNextOfKin].[Surname] LIKE '" + surname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (telcontact != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INNextOfKin].[TelContact] LIKE '" + telcontact.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INNextOfKin].[ID], [INNextOfKin].[FKINImportID], [INNextOfKin].[FKINRelationshipID], [INNextOfKin].[FirstName], [INNextOfKin].[Surname], [INNextOfKin].[TelContact], [INNextOfKin].[StampDate], [INNextOfKin].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INNextOfKin].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INNextOfKin] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
