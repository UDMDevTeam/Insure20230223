using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to systeminfo objects.
    /// </summary>
    internal abstract partial class SystemInfoQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) systeminfo from the database.
        /// </summary>
        /// <param name="systeminfo">The systeminfo object to delete.</param>
        /// <returns>A query that can be used to delete the systeminfo from the database.</returns>
        internal static string Delete(SystemInfo systeminfo, ref object[] parameters)
        {
            string query = string.Empty;
            if (systeminfo != null)
            {
                query = "INSERT INTO [zHstSystemInfo] ([ID], [FKSystemID], [SystemVersion], [ComputerName], [UserName], [UserDomainName], [SimpleOSName], [OSDescription], [OSArchitecture], [ProcessArchitecture], [FrameworkDescription], [IPAddresses], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [SystemVersion], [ComputerName], [UserName], [UserDomainName], [SimpleOSName], [OSDescription], [OSArchitecture], [ProcessArchitecture], [FrameworkDescription], [IPAddresses], [StampDate], [StampUserID] FROM [SystemInfo] WHERE [SystemInfo].[ID] = @ID; ";
                query += "DELETE FROM [SystemInfo] WHERE [SystemInfo].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", systeminfo.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) systeminfo from the database.
        /// </summary>
        /// <param name="systeminfo">The systeminfo object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the systeminfo from the database.</returns>
        internal static string DeleteHistory(SystemInfo systeminfo, ref object[] parameters)
        {
            string query = string.Empty;
            if (systeminfo != null)
            {
                query = "DELETE FROM [zHstSystemInfo] WHERE [zHstSystemInfo].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", systeminfo.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) systeminfo from the database.
        /// </summary>
        /// <param name="systeminfo">The systeminfo object to undelete.</param>
        /// <returns>A query that can be used to undelete the systeminfo from the database.</returns>
        internal static string UnDelete(SystemInfo systeminfo, ref object[] parameters)
        {
            string query = string.Empty;
            if (systeminfo != null)
            {
                query = "INSERT INTO [SystemInfo] ([ID], [FKSystemID], [SystemVersion], [ComputerName], [UserName], [UserDomainName], [SimpleOSName], [OSDescription], [OSArchitecture], [ProcessArchitecture], [FrameworkDescription], [IPAddresses], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [SystemVersion], [ComputerName], [UserName], [UserDomainName], [SimpleOSName], [OSDescription], [OSArchitecture], [ProcessArchitecture], [FrameworkDescription], [IPAddresses], [StampDate], [StampUserID] FROM [zHstSystemInfo] WHERE [zHstSystemInfo].[ID] = @ID AND [zHstSystemInfo].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstSystemInfo] WHERE [zHstSystemInfo].[ID] = @ID) AND (SELECT COUNT(ID) FROM [SystemInfo] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstSystemInfo] WHERE [zHstSystemInfo].[ID] = @ID AND [zHstSystemInfo].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstSystemInfo] WHERE [zHstSystemInfo].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [SystemInfo] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", systeminfo.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an systeminfo object.
        /// </summary>
        /// <param name="systeminfo">The systeminfo object to fill.</param>
        /// <returns>A query that can be used to fill the systeminfo object.</returns>
        internal static string Fill(SystemInfo systeminfo, ref object[] parameters)
        {
            string query = string.Empty;
            if (systeminfo != null)
            {
                query = "SELECT [ID], [FKSystemID], [SystemVersion], [ComputerName], [UserName], [UserDomainName], [SimpleOSName], [OSDescription], [OSArchitecture], [ProcessArchitecture], [FrameworkDescription], [IPAddresses], [StampDate], [StampUserID] FROM [SystemInfo] WHERE [SystemInfo].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", systeminfo.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  systeminfo data.
        /// </summary>
        /// <param name="systeminfo">The systeminfo to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  systeminfo data.</returns>
        internal static string FillData(SystemInfo systeminfo, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (systeminfo != null)
            {
            query.Append("SELECT [SystemInfo].[ID], [SystemInfo].[FKSystemID], [SystemInfo].[SystemVersion], [SystemInfo].[ComputerName], [SystemInfo].[UserName], [SystemInfo].[UserDomainName], [SystemInfo].[SimpleOSName], [SystemInfo].[OSDescription], [SystemInfo].[OSArchitecture], [SystemInfo].[ProcessArchitecture], [SystemInfo].[FrameworkDescription], [SystemInfo].[IPAddresses], [SystemInfo].[StampDate], [SystemInfo].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [SystemInfo].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [SystemInfo] ");
                query.Append(" WHERE [SystemInfo].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", systeminfo.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an systeminfo object from history.
        /// </summary>
        /// <param name="systeminfo">The systeminfo object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the systeminfo object from history.</returns>
        internal static string FillHistory(SystemInfo systeminfo, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (systeminfo != null)
            {
                query = "SELECT [ID], [FKSystemID], [SystemVersion], [ComputerName], [UserName], [UserDomainName], [SimpleOSName], [OSDescription], [OSArchitecture], [ProcessArchitecture], [FrameworkDescription], [IPAddresses], [StampDate], [StampUserID] FROM [zHstSystemInfo] WHERE [zHstSystemInfo].[ID] = @ID AND [zHstSystemInfo].[StampUserID] = @StampUserID AND [zHstSystemInfo].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", systeminfo.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the systeminfos in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the systeminfos in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [SystemInfo].[ID], [SystemInfo].[FKSystemID], [SystemInfo].[SystemVersion], [SystemInfo].[ComputerName], [SystemInfo].[UserName], [SystemInfo].[UserDomainName], [SystemInfo].[SimpleOSName], [SystemInfo].[OSDescription], [SystemInfo].[OSArchitecture], [SystemInfo].[ProcessArchitecture], [SystemInfo].[FrameworkDescription], [SystemInfo].[IPAddresses], [SystemInfo].[StampDate], [SystemInfo].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [SystemInfo].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [SystemInfo] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted systeminfos in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted systeminfos in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstSystemInfo].[ID], [zHstSystemInfo].[FKSystemID], [zHstSystemInfo].[SystemVersion], [zHstSystemInfo].[ComputerName], [zHstSystemInfo].[UserName], [zHstSystemInfo].[UserDomainName], [zHstSystemInfo].[SimpleOSName], [zHstSystemInfo].[OSDescription], [zHstSystemInfo].[OSArchitecture], [zHstSystemInfo].[ProcessArchitecture], [zHstSystemInfo].[FrameworkDescription], [zHstSystemInfo].[IPAddresses], [zHstSystemInfo].[StampDate], [zHstSystemInfo].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstSystemInfo].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstSystemInfo] ");
            query.Append("INNER JOIN (SELECT [zHstSystemInfo].[ID], MAX([zHstSystemInfo].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstSystemInfo] ");
            query.Append("WHERE [zHstSystemInfo].[ID] NOT IN (SELECT [SystemInfo].[ID] FROM [SystemInfo]) ");
            query.Append("GROUP BY [zHstSystemInfo].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstSystemInfo].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstSystemInfo].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) systeminfo in the database.
        /// </summary>
        /// <param name="systeminfo">The systeminfo object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) systeminfo in the database.</returns>
        public static string ListHistory(SystemInfo systeminfo, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (systeminfo != null)
            {
            query.Append("SELECT [zHstSystemInfo].[ID], [zHstSystemInfo].[FKSystemID], [zHstSystemInfo].[SystemVersion], [zHstSystemInfo].[ComputerName], [zHstSystemInfo].[UserName], [zHstSystemInfo].[UserDomainName], [zHstSystemInfo].[SimpleOSName], [zHstSystemInfo].[OSDescription], [zHstSystemInfo].[OSArchitecture], [zHstSystemInfo].[ProcessArchitecture], [zHstSystemInfo].[FrameworkDescription], [zHstSystemInfo].[IPAddresses], [zHstSystemInfo].[StampDate], [zHstSystemInfo].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstSystemInfo].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstSystemInfo] ");
                query.Append(" WHERE [zHstSystemInfo].[ID] = @ID");
                query.Append(" ORDER BY [zHstSystemInfo].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", systeminfo.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) systeminfo to the database.
        /// </summary>
        /// <param name="systeminfo">The systeminfo to save.</param>
        /// <returns>A query that can be used to save the systeminfo to the database.</returns>
        internal static string Save(SystemInfo systeminfo, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (systeminfo != null)
            {
                if (systeminfo.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstSystemInfo] ([ID], [FKSystemID], [SystemVersion], [ComputerName], [UserName], [UserDomainName], [SimpleOSName], [OSDescription], [OSArchitecture], [ProcessArchitecture], [FrameworkDescription], [IPAddresses], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [SystemVersion], [ComputerName], [UserName], [UserDomainName], [SimpleOSName], [OSDescription], [OSArchitecture], [ProcessArchitecture], [FrameworkDescription], [IPAddresses], [StampDate], [StampUserID] FROM [SystemInfo] WHERE [SystemInfo].[ID] = @ID; ");
                    query.Append("UPDATE [SystemInfo]");
                    parameters = new object[12];
                    query.Append(" SET [FKSystemID] = @FKSystemID");
                    parameters[0] = Database.GetParameter("@FKSystemID", systeminfo.FKSystemID.HasValue ? (object)systeminfo.FKSystemID.Value : DBNull.Value);
                    query.Append(", [SystemVersion] = @SystemVersion");
                    parameters[1] = Database.GetParameter("@SystemVersion", string.IsNullOrEmpty(systeminfo.SystemVersion) ? DBNull.Value : (object)systeminfo.SystemVersion);
                    query.Append(", [ComputerName] = @ComputerName");
                    parameters[2] = Database.GetParameter("@ComputerName", string.IsNullOrEmpty(systeminfo.ComputerName) ? DBNull.Value : (object)systeminfo.ComputerName);
                    query.Append(", [UserName] = @UserName");
                    parameters[3] = Database.GetParameter("@UserName", string.IsNullOrEmpty(systeminfo.UserName) ? DBNull.Value : (object)systeminfo.UserName);
                    query.Append(", [UserDomainName] = @UserDomainName");
                    parameters[4] = Database.GetParameter("@UserDomainName", string.IsNullOrEmpty(systeminfo.UserDomainName) ? DBNull.Value : (object)systeminfo.UserDomainName);
                    query.Append(", [SimpleOSName] = @SimpleOSName");
                    parameters[5] = Database.GetParameter("@SimpleOSName", string.IsNullOrEmpty(systeminfo.SimpleOSName) ? DBNull.Value : (object)systeminfo.SimpleOSName);
                    query.Append(", [OSDescription] = @OSDescription");
                    parameters[6] = Database.GetParameter("@OSDescription", string.IsNullOrEmpty(systeminfo.OSDescription) ? DBNull.Value : (object)systeminfo.OSDescription);
                    query.Append(", [OSArchitecture] = @OSArchitecture");
                    parameters[7] = Database.GetParameter("@OSArchitecture", string.IsNullOrEmpty(systeminfo.OSArchitecture) ? DBNull.Value : (object)systeminfo.OSArchitecture);
                    query.Append(", [ProcessArchitecture] = @ProcessArchitecture");
                    parameters[8] = Database.GetParameter("@ProcessArchitecture", string.IsNullOrEmpty(systeminfo.ProcessArchitecture) ? DBNull.Value : (object)systeminfo.ProcessArchitecture);
                    query.Append(", [FrameworkDescription] = @FrameworkDescription");
                    parameters[9] = Database.GetParameter("@FrameworkDescription", string.IsNullOrEmpty(systeminfo.FrameworkDescription) ? DBNull.Value : (object)systeminfo.FrameworkDescription);
                    query.Append(", [IPAddresses] = @IPAddresses");
                    parameters[10] = Database.GetParameter("@IPAddresses", string.IsNullOrEmpty(systeminfo.IPAddresses) ? DBNull.Value : (object)systeminfo.IPAddresses);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [SystemInfo].[ID] = @ID"); 
                    parameters[11] = Database.GetParameter("@ID", systeminfo.ID);
                }
                else
                {
                    query.Append("INSERT INTO [SystemInfo] ([FKSystemID], [SystemVersion], [ComputerName], [UserName], [UserDomainName], [SimpleOSName], [OSDescription], [OSArchitecture], [ProcessArchitecture], [FrameworkDescription], [IPAddresses], [StampDate], [StampUserID]) VALUES(@FKSystemID, @SystemVersion, @ComputerName, @UserName, @UserDomainName, @SimpleOSName, @OSDescription, @OSArchitecture, @ProcessArchitecture, @FrameworkDescription, @IPAddresses, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[11];
                    parameters[0] = Database.GetParameter("@FKSystemID", systeminfo.FKSystemID.HasValue ? (object)systeminfo.FKSystemID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@SystemVersion", string.IsNullOrEmpty(systeminfo.SystemVersion) ? DBNull.Value : (object)systeminfo.SystemVersion);
                    parameters[2] = Database.GetParameter("@ComputerName", string.IsNullOrEmpty(systeminfo.ComputerName) ? DBNull.Value : (object)systeminfo.ComputerName);
                    parameters[3] = Database.GetParameter("@UserName", string.IsNullOrEmpty(systeminfo.UserName) ? DBNull.Value : (object)systeminfo.UserName);
                    parameters[4] = Database.GetParameter("@UserDomainName", string.IsNullOrEmpty(systeminfo.UserDomainName) ? DBNull.Value : (object)systeminfo.UserDomainName);
                    parameters[5] = Database.GetParameter("@SimpleOSName", string.IsNullOrEmpty(systeminfo.SimpleOSName) ? DBNull.Value : (object)systeminfo.SimpleOSName);
                    parameters[6] = Database.GetParameter("@OSDescription", string.IsNullOrEmpty(systeminfo.OSDescription) ? DBNull.Value : (object)systeminfo.OSDescription);
                    parameters[7] = Database.GetParameter("@OSArchitecture", string.IsNullOrEmpty(systeminfo.OSArchitecture) ? DBNull.Value : (object)systeminfo.OSArchitecture);
                    parameters[8] = Database.GetParameter("@ProcessArchitecture", string.IsNullOrEmpty(systeminfo.ProcessArchitecture) ? DBNull.Value : (object)systeminfo.ProcessArchitecture);
                    parameters[9] = Database.GetParameter("@FrameworkDescription", string.IsNullOrEmpty(systeminfo.FrameworkDescription) ? DBNull.Value : (object)systeminfo.FrameworkDescription);
                    parameters[10] = Database.GetParameter("@IPAddresses", string.IsNullOrEmpty(systeminfo.IPAddresses) ? DBNull.Value : (object)systeminfo.IPAddresses);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for systeminfos that match the search criteria.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="systemversion">The systemversion search criteria.</param>
        /// <param name="computername">The computername search criteria.</param>
        /// <param name="username">The username search criteria.</param>
        /// <param name="userdomainname">The userdomainname search criteria.</param>
        /// <param name="simpleosname">The simpleosname search criteria.</param>
        /// <param name="osdescription">The osdescription search criteria.</param>
        /// <param name="osarchitecture">The osarchitecture search criteria.</param>
        /// <param name="processarchitecture">The processarchitecture search criteria.</param>
        /// <param name="frameworkdescription">The frameworkdescription search criteria.</param>
        /// <param name="ipaddresses">The ipaddresses search criteria.</param>
        /// <returns>A query that can be used to search for systeminfos based on the search criteria.</returns>
        internal static string Search(long? fksystemid, string systemversion, string computername, string username, string userdomainname, string simpleosname, string osdescription, string osarchitecture, string processarchitecture, string frameworkdescription, string ipaddresses)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fksystemid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SystemInfo].[FKSystemID] = " + fksystemid + "");
            }
            if (systemversion != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SystemInfo].[SystemVersion] LIKE '" + systemversion.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (computername != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SystemInfo].[ComputerName] LIKE '" + computername.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (username != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SystemInfo].[UserName] LIKE '" + username.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (userdomainname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SystemInfo].[UserDomainName] LIKE '" + userdomainname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (simpleosname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SystemInfo].[SimpleOSName] LIKE '" + simpleosname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (osdescription != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SystemInfo].[OSDescription] LIKE '" + osdescription.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (osarchitecture != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SystemInfo].[OSArchitecture] LIKE '" + osarchitecture.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (processarchitecture != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SystemInfo].[ProcessArchitecture] LIKE '" + processarchitecture.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (frameworkdescription != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SystemInfo].[FrameworkDescription] LIKE '" + frameworkdescription.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (ipaddresses != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SystemInfo].[IPAddresses] LIKE '" + ipaddresses.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [SystemInfo].[ID], [SystemInfo].[FKSystemID], [SystemInfo].[SystemVersion], [SystemInfo].[ComputerName], [SystemInfo].[UserName], [SystemInfo].[UserDomainName], [SystemInfo].[SimpleOSName], [SystemInfo].[OSDescription], [SystemInfo].[OSArchitecture], [SystemInfo].[ProcessArchitecture], [SystemInfo].[FrameworkDescription], [SystemInfo].[IPAddresses], [SystemInfo].[StampDate], [SystemInfo].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [SystemInfo].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [SystemInfo] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
