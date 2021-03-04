using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inimportextra objects.
    /// </summary>
    internal abstract partial class INImportExtraQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inimportextra from the database.
        /// </summary>
        /// <param name="inimportextra">The inimportextra object to delete.</param>
        /// <returns>A query that can be used to delete the inimportextra from the database.</returns>
        internal static string Delete(INImportExtra inimportextra, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportextra != null)
            {
                query = "INSERT INTO [zHstINImportExtra] ([ID], [FKINImportID], [Extension1], [Extension2], [NotPossibleBumpUp], [FKCMCallRefUserID], [EMailRequested], [CustomerCareRequested], [IsGoldenLead], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [Extension1], [Extension2], [NotPossibleBumpUp], [FKCMCallRefUserID], [EMailRequested], [CustomerCareRequested], [IsGoldenLead], [StampDate], [StampUserID] FROM [INImportExtra] WHERE [INImportExtra].[ID] = @ID; ";
                query += "DELETE FROM [INImportExtra] WHERE [INImportExtra].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportextra.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inimportextra from the database.
        /// </summary>
        /// <param name="inimportextra">The inimportextra object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inimportextra from the database.</returns>
        internal static string DeleteHistory(INImportExtra inimportextra, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportextra != null)
            {
                query = "DELETE FROM [zHstINImportExtra] WHERE [zHstINImportExtra].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportextra.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inimportextra from the database.
        /// </summary>
        /// <param name="inimportextra">The inimportextra object to undelete.</param>
        /// <returns>A query that can be used to undelete the inimportextra from the database.</returns>
        internal static string UnDelete(INImportExtra inimportextra, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportextra != null)
            {
                query = "INSERT INTO [INImportExtra] ([ID], [FKINImportID], [Extension1], [Extension2], [NotPossibleBumpUp], [FKCMCallRefUserID], [EMailRequested], [CustomerCareRequested], [IsGoldenLead], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [Extension1], [Extension2], [NotPossibleBumpUp], [FKCMCallRefUserID], [EMailRequested], [CustomerCareRequested], [IsGoldenLead], [StampDate], [StampUserID] FROM [zHstINImportExtra] WHERE [zHstINImportExtra].[ID] = @ID AND [zHstINImportExtra].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportExtra] WHERE [zHstINImportExtra].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INImportExtra] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINImportExtra] WHERE [zHstINImportExtra].[ID] = @ID AND [zHstINImportExtra].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportExtra] WHERE [zHstINImportExtra].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INImportExtra] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportextra.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inimportextra object.
        /// </summary>
        /// <param name="inimportextra">The inimportextra object to fill.</param>
        /// <returns>A query that can be used to fill the inimportextra object.</returns>
        internal static string Fill(INImportExtra inimportextra, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportextra != null)
            {
                query = "SELECT [ID], [FKINImportID], [Extension1], [Extension2], [NotPossibleBumpUp], [FKCMCallRefUserID], [EMailRequested], [CustomerCareRequested], [IsGoldenLead], [StampDate], [StampUserID] FROM [INImportExtra] WHERE [INImportExtra].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportextra.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inimportextra data.
        /// </summary>
        /// <param name="inimportextra">The inimportextra to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inimportextra data.</returns>
        internal static string FillData(INImportExtra inimportextra, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportextra != null)
            {
            query.Append("SELECT [INImportExtra].[ID], [INImportExtra].[FKINImportID], [INImportExtra].[Extension1], [INImportExtra].[Extension2], [INImportExtra].[NotPossibleBumpUp], [INImportExtra].[FKCMCallRefUserID], [INImportExtra].[EMailRequested], [INImportExtra].[CustomerCareRequested], [INImportExtra].[IsGoldenLead], [INImportExtra].[StampDate], [INImportExtra].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportExtra].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportExtra] ");
                query.Append(" WHERE [INImportExtra].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportextra.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inimportextra object from history.
        /// </summary>
        /// <param name="inimportextra">The inimportextra object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inimportextra object from history.</returns>
        internal static string FillHistory(INImportExtra inimportextra, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportextra != null)
            {
                query = "SELECT [ID], [FKINImportID], [Extension1], [Extension2], [NotPossibleBumpUp], [FKCMCallRefUserID], [EMailRequested], [CustomerCareRequested], [IsGoldenLead], [StampDate], [StampUserID] FROM [zHstINImportExtra] WHERE [zHstINImportExtra].[ID] = @ID AND [zHstINImportExtra].[StampUserID] = @StampUserID AND [zHstINImportExtra].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inimportextra.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportextras in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inimportextras in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INImportExtra].[ID], [INImportExtra].[FKINImportID], [INImportExtra].[Extension1], [INImportExtra].[Extension2], [INImportExtra].[NotPossibleBumpUp], [INImportExtra].[FKCMCallRefUserID], [INImportExtra].[EMailRequested], [INImportExtra].[CustomerCareRequested], [INImportExtra].[IsGoldenLead], [INImportExtra].[StampDate], [INImportExtra].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportExtra].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportExtra] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inimportextras in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inimportextras in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINImportExtra].[ID], [zHstINImportExtra].[FKINImportID], [zHstINImportExtra].[Extension1], [zHstINImportExtra].[Extension2], [zHstINImportExtra].[NotPossibleBumpUp], [zHstINImportExtra].[FKCMCallRefUserID], [zHstINImportExtra].[EMailRequested], [zHstINImportExtra].[CustomerCareRequested], [zHstINImportExtra].[IsGoldenLead], [zHstINImportExtra].[StampDate], [zHstINImportExtra].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportExtra].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportExtra] ");
            query.Append("INNER JOIN (SELECT [zHstINImportExtra].[ID], MAX([zHstINImportExtra].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINImportExtra] ");
            query.Append("WHERE [zHstINImportExtra].[ID] NOT IN (SELECT [INImportExtra].[ID] FROM [INImportExtra]) ");
            query.Append("GROUP BY [zHstINImportExtra].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINImportExtra].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINImportExtra].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inimportextra in the database.
        /// </summary>
        /// <param name="inimportextra">The inimportextra object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inimportextra in the database.</returns>
        public static string ListHistory(INImportExtra inimportextra, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportextra != null)
            {
            query.Append("SELECT [zHstINImportExtra].[ID], [zHstINImportExtra].[FKINImportID], [zHstINImportExtra].[Extension1], [zHstINImportExtra].[Extension2], [zHstINImportExtra].[NotPossibleBumpUp], [zHstINImportExtra].[FKCMCallRefUserID], [zHstINImportExtra].[EMailRequested], [zHstINImportExtra].[CustomerCareRequested], [zHstINImportExtra].[IsGoldenLead], [zHstINImportExtra].[StampDate], [zHstINImportExtra].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportExtra].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportExtra] ");
                query.Append(" WHERE [zHstINImportExtra].[ID] = @ID");
                query.Append(" ORDER BY [zHstINImportExtra].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportextra.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inimportextra to the database.
        /// </summary>
        /// <param name="inimportextra">The inimportextra to save.</param>
        /// <returns>A query that can be used to save the inimportextra to the database.</returns>
        internal static string Save(INImportExtra inimportextra, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportextra != null)
            {
                if (inimportextra.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINImportExtra] ([ID], [FKINImportID], [Extension1], [Extension2], [NotPossibleBumpUp], [FKCMCallRefUserID], [EMailRequested], [CustomerCareRequested], [IsGoldenLead], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [Extension1], [Extension2], [NotPossibleBumpUp], [FKCMCallRefUserID], [EMailRequested], [CustomerCareRequested], [IsGoldenLead], [StampDate], [StampUserID] FROM [INImportExtra] WHERE [INImportExtra].[ID] = @ID; ");
                    query.Append("UPDATE [INImportExtra]");
                    parameters = new object[9];
                    query.Append(" SET [FKINImportID] = @FKINImportID");
                    parameters[0] = Database.GetParameter("@FKINImportID", inimportextra.FKINImportID.HasValue ? (object)inimportextra.FKINImportID.Value : DBNull.Value);
                    query.Append(", [Extension1] = @Extension1");
                    parameters[1] = Database.GetParameter("@Extension1", string.IsNullOrEmpty(inimportextra.Extension1) ? DBNull.Value : (object)inimportextra.Extension1);
                    query.Append(", [Extension2] = @Extension2");
                    parameters[2] = Database.GetParameter("@Extension2", string.IsNullOrEmpty(inimportextra.Extension2) ? DBNull.Value : (object)inimportextra.Extension2);
                    query.Append(", [NotPossibleBumpUp] = @NotPossibleBumpUp");
                    parameters[3] = Database.GetParameter("@NotPossibleBumpUp", inimportextra.NotPossibleBumpUp.HasValue ? (object)inimportextra.NotPossibleBumpUp.Value : DBNull.Value);
                    query.Append(", [FKCMCallRefUserID] = @FKCMCallRefUserID");
                    parameters[4] = Database.GetParameter("@FKCMCallRefUserID", inimportextra.FKCMCallRefUserID.HasValue ? (object)inimportextra.FKCMCallRefUserID.Value : DBNull.Value);
                    query.Append(", [EMailRequested] = @EMailRequested");
                    parameters[5] = Database.GetParameter("@EMailRequested", inimportextra.EMailRequested.HasValue ? (object)inimportextra.EMailRequested.Value : DBNull.Value);
                    query.Append(", [CustomerCareRequested] = @CustomerCareRequested");
                    parameters[6] = Database.GetParameter("@CustomerCareRequested", inimportextra.CustomerCareRequested.HasValue ? (object)inimportextra.CustomerCareRequested.Value : DBNull.Value);
                    query.Append(", [IsGoldenLead] = @IsGoldenLead");
                    parameters[7] = Database.GetParameter("@IsGoldenLead", inimportextra.IsGoldenLead.HasValue ? (object)inimportextra.IsGoldenLead.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INImportExtra].[ID] = @ID"); 
                    parameters[8] = Database.GetParameter("@ID", inimportextra.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INImportExtra] ([FKINImportID], [Extension1], [Extension2], [NotPossibleBumpUp], [FKCMCallRefUserID], [EMailRequested], [CustomerCareRequested], [IsGoldenLead], [StampDate], [StampUserID]) VALUES(@FKINImportID, @Extension1, @Extension2, @NotPossibleBumpUp, @FKCMCallRefUserID, @EMailRequested, @CustomerCareRequested, @IsGoldenLead, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[8];
                    parameters[0] = Database.GetParameter("@FKINImportID", inimportextra.FKINImportID.HasValue ? (object)inimportextra.FKINImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@Extension1", string.IsNullOrEmpty(inimportextra.Extension1) ? DBNull.Value : (object)inimportextra.Extension1);
                    parameters[2] = Database.GetParameter("@Extension2", string.IsNullOrEmpty(inimportextra.Extension2) ? DBNull.Value : (object)inimportextra.Extension2);
                    parameters[3] = Database.GetParameter("@NotPossibleBumpUp", inimportextra.NotPossibleBumpUp.HasValue ? (object)inimportextra.NotPossibleBumpUp.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@FKCMCallRefUserID", inimportextra.FKCMCallRefUserID.HasValue ? (object)inimportextra.FKCMCallRefUserID.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@EMailRequested", inimportextra.EMailRequested.HasValue ? (object)inimportextra.EMailRequested.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@CustomerCareRequested", inimportextra.CustomerCareRequested.HasValue ? (object)inimportextra.CustomerCareRequested.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@IsGoldenLead", inimportextra.IsGoldenLead.HasValue ? (object)inimportextra.IsGoldenLead.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportextras that match the search criteria.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="extension1">The extension1 search criteria.</param>
        /// <param name="extension2">The extension2 search criteria.</param>
        /// <param name="notpossiblebumpup">The notpossiblebumpup search criteria.</param>
        /// <param name="fkcmcallrefuserid">The fkcmcallrefuserid search criteria.</param>
        /// <param name="emailrequested">The emailrequested search criteria.</param>
        /// <param name="customercarerequested">The customercarerequested search criteria.</param>
        /// <param name="isgoldenlead">The isgoldenlead search criteria.</param>
        /// <returns>A query that can be used to search for inimportextras based on the search criteria.</returns>
        internal static string Search(long? fkinimportid, string extension1, string extension2, bool? notpossiblebumpup, long? fkcmcallrefuserid, bool? emailrequested, bool? customercarerequested, bool? isgoldenlead)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportExtra].[FKINImportID] = " + fkinimportid + "");
            }
            if (extension1 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportExtra].[Extension1] LIKE '" + extension1.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (extension2 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportExtra].[Extension2] LIKE '" + extension2.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (notpossiblebumpup != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportExtra].[NotPossibleBumpUp] = " + ((bool)notpossiblebumpup ? "1" : "0"));
            }
            if (fkcmcallrefuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportExtra].[FKCMCallRefUserID] = " + fkcmcallrefuserid + "");
            }
            if (emailrequested != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportExtra].[EMailRequested] = " + ((bool)emailrequested ? "1" : "0"));
            }
            if (customercarerequested != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportExtra].[CustomerCareRequested] = " + ((bool)customercarerequested ? "1" : "0"));
            }
            if (isgoldenlead != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportExtra].[IsGoldenLead] = " + ((bool)isgoldenlead ? "1" : "0"));
            }
            query.Append("SELECT [INImportExtra].[ID], [INImportExtra].[FKINImportID], [INImportExtra].[Extension1], [INImportExtra].[Extension2], [INImportExtra].[NotPossibleBumpUp], [INImportExtra].[FKCMCallRefUserID], [INImportExtra].[EMailRequested], [INImportExtra].[CustomerCareRequested], [INImportExtra].[IsGoldenLead], [INImportExtra].[StampDate], [INImportExtra].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportExtra].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportExtra] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
