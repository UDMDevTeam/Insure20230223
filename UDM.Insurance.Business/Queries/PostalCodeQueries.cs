using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to postalcode objects.
    /// </summary>
    internal abstract partial class PostalCodeQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) postalcode from the database.
        /// </summary>
        /// <param name="postalcode">The postalcode object to delete.</param>
        /// <returns>A query that can be used to delete the postalcode from the database.</returns>
        internal static string Delete(PostalCode postalcode, ref object[] parameters)
        {
            string query = string.Empty;
            if (postalcode != null)
            {
                query = "INSERT INTO [zHstPostalCode] ([ID], [Suburb], [BoxCode], [StreetCode], [City], [StampDate], [StampUserID]) SELECT [ID], [Suburb], [BoxCode], [StreetCode], [City], [StampDate], [StampUserID] FROM [PostalCode] WHERE [PostalCode].[ID] = @ID; ";
                query += "DELETE FROM [PostalCode] WHERE [PostalCode].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", postalcode.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) postalcode from the database.
        /// </summary>
        /// <param name="postalcode">The postalcode object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the postalcode from the database.</returns>
        internal static string DeleteHistory(PostalCode postalcode, ref object[] parameters)
        {
            string query = string.Empty;
            if (postalcode != null)
            {
                query = "DELETE FROM [zHstPostalCode] WHERE [zHstPostalCode].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", postalcode.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) postalcode from the database.
        /// </summary>
        /// <param name="postalcode">The postalcode object to undelete.</param>
        /// <returns>A query that can be used to undelete the postalcode from the database.</returns>
        internal static string UnDelete(PostalCode postalcode, ref object[] parameters)
        {
            string query = string.Empty;
            if (postalcode != null)
            {
                query = "INSERT INTO [PostalCode] ([ID], [Suburb], [BoxCode], [StreetCode], [City], [StampDate], [StampUserID]) SELECT [ID], [Suburb], [BoxCode], [StreetCode], [City], [StampDate], [StampUserID] FROM [zHstPostalCode] WHERE [zHstPostalCode].[ID] = @ID AND [zHstPostalCode].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstPostalCode] WHERE [zHstPostalCode].[ID] = @ID) AND (SELECT COUNT(ID) FROM [PostalCode] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstPostalCode] WHERE [zHstPostalCode].[ID] = @ID AND [zHstPostalCode].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstPostalCode] WHERE [zHstPostalCode].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [PostalCode] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", postalcode.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an postalcode object.
        /// </summary>
        /// <param name="postalcode">The postalcode object to fill.</param>
        /// <returns>A query that can be used to fill the postalcode object.</returns>
        internal static string Fill(PostalCode postalcode, ref object[] parameters)
        {
            string query = string.Empty;
            if (postalcode != null)
            {
                query = "SELECT [ID], [Suburb], [BoxCode], [StreetCode], [City], [StampDate], [StampUserID] FROM [PostalCode] WHERE [PostalCode].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", postalcode.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  postalcode data.
        /// </summary>
        /// <param name="postalcode">The postalcode to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  postalcode data.</returns>
        internal static string FillData(PostalCode postalcode, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (postalcode != null)
            {
            query.Append("SELECT [PostalCode].[ID], [PostalCode].[Suburb], [PostalCode].[BoxCode], [PostalCode].[StreetCode], [PostalCode].[City], [PostalCode].[StampDate], [PostalCode].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [PostalCode].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [PostalCode] ");
                query.Append(" WHERE [PostalCode].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", postalcode.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an postalcode object from history.
        /// </summary>
        /// <param name="postalcode">The postalcode object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the postalcode object from history.</returns>
        internal static string FillHistory(PostalCode postalcode, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (postalcode != null)
            {
                query = "SELECT [ID], [Suburb], [BoxCode], [StreetCode], [City], [StampDate], [StampUserID] FROM [zHstPostalCode] WHERE [zHstPostalCode].[ID] = @ID AND [zHstPostalCode].[StampUserID] = @StampUserID AND [zHstPostalCode].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", postalcode.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the postalcodes in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the postalcodes in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [PostalCode].[ID], [PostalCode].[Suburb], [PostalCode].[BoxCode], [PostalCode].[StreetCode], [PostalCode].[City], [PostalCode].[StampDate], [PostalCode].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [PostalCode].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [PostalCode] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted postalcodes in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted postalcodes in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstPostalCode].[ID], [zHstPostalCode].[Suburb], [zHstPostalCode].[BoxCode], [zHstPostalCode].[StreetCode], [zHstPostalCode].[City], [zHstPostalCode].[StampDate], [zHstPostalCode].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstPostalCode].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstPostalCode] ");
            query.Append("INNER JOIN (SELECT [zHstPostalCode].[ID], MAX([zHstPostalCode].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstPostalCode] ");
            query.Append("WHERE [zHstPostalCode].[ID] NOT IN (SELECT [PostalCode].[ID] FROM [PostalCode]) ");
            query.Append("GROUP BY [zHstPostalCode].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstPostalCode].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstPostalCode].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) postalcode in the database.
        /// </summary>
        /// <param name="postalcode">The postalcode object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) postalcode in the database.</returns>
        public static string ListHistory(PostalCode postalcode, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (postalcode != null)
            {
            query.Append("SELECT [zHstPostalCode].[ID], [zHstPostalCode].[Suburb], [zHstPostalCode].[BoxCode], [zHstPostalCode].[StreetCode], [zHstPostalCode].[City], [zHstPostalCode].[StampDate], [zHstPostalCode].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstPostalCode].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstPostalCode] ");
                query.Append(" WHERE [zHstPostalCode].[ID] = @ID");
                query.Append(" ORDER BY [zHstPostalCode].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", postalcode.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) postalcode to the database.
        /// </summary>
        /// <param name="postalcode">The postalcode to save.</param>
        /// <returns>A query that can be used to save the postalcode to the database.</returns>
        internal static string Save(PostalCode postalcode, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (postalcode != null)
            {
                if (postalcode.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstPostalCode] ([ID], [Suburb], [BoxCode], [StreetCode], [City], [StampDate], [StampUserID]) SELECT [ID], [Suburb], [BoxCode], [StreetCode], [City], [StampDate], [StampUserID] FROM [PostalCode] WHERE [PostalCode].[ID] = @ID; ");
                    query.Append("UPDATE [PostalCode]");
                    parameters = new object[5];
                    query.Append(" SET [Suburb] = @Suburb");
                    parameters[0] = Database.GetParameter("@Suburb", string.IsNullOrEmpty(postalcode.Suburb) ? DBNull.Value : (object)postalcode.Suburb);
                    query.Append(", [BoxCode] = @BoxCode");
                    parameters[1] = Database.GetParameter("@BoxCode", string.IsNullOrEmpty(postalcode.BoxCode) ? DBNull.Value : (object)postalcode.BoxCode);
                    query.Append(", [StreetCode] = @StreetCode");
                    parameters[2] = Database.GetParameter("@StreetCode", string.IsNullOrEmpty(postalcode.StreetCode) ? DBNull.Value : (object)postalcode.StreetCode);
                    query.Append(", [City] = @City");
                    parameters[3] = Database.GetParameter("@City", string.IsNullOrEmpty(postalcode.City) ? DBNull.Value : (object)postalcode.City);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [PostalCode].[ID] = @ID"); 
                    parameters[4] = Database.GetParameter("@ID", postalcode.ID);
                }
                else
                {
                    query.Append("INSERT INTO [PostalCode] ([Suburb], [BoxCode], [StreetCode], [City], [StampDate], [StampUserID]) VALUES(@Suburb, @BoxCode, @StreetCode, @City, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[4];
                    parameters[0] = Database.GetParameter("@Suburb", string.IsNullOrEmpty(postalcode.Suburb) ? DBNull.Value : (object)postalcode.Suburb);
                    parameters[1] = Database.GetParameter("@BoxCode", string.IsNullOrEmpty(postalcode.BoxCode) ? DBNull.Value : (object)postalcode.BoxCode);
                    parameters[2] = Database.GetParameter("@StreetCode", string.IsNullOrEmpty(postalcode.StreetCode) ? DBNull.Value : (object)postalcode.StreetCode);
                    parameters[3] = Database.GetParameter("@City", string.IsNullOrEmpty(postalcode.City) ? DBNull.Value : (object)postalcode.City);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for postalcodes that match the search criteria.
        /// </summary>
        /// <param name="suburb">The suburb search criteria.</param>
        /// <param name="boxcode">The boxcode search criteria.</param>
        /// <param name="streetcode">The streetcode search criteria.</param>
        /// <param name="city">The city search criteria.</param>
        /// <returns>A query that can be used to search for postalcodes based on the search criteria.</returns>
        internal static string Search(string suburb, string boxcode, string streetcode, string city)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (suburb != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[PostalCode].[Suburb] LIKE '" + suburb.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (boxcode != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[PostalCode].[BoxCode] LIKE '" + boxcode.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (streetcode != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[PostalCode].[StreetCode] LIKE '" + streetcode.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (city != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[PostalCode].[City] LIKE '" + city.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [PostalCode].[ID], [PostalCode].[Suburb], [PostalCode].[BoxCode], [PostalCode].[StreetCode], [PostalCode].[City], [PostalCode].[StampDate], [PostalCode].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [PostalCode].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [PostalCode] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
