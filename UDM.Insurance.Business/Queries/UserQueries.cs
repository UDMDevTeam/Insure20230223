using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to user objects.
    /// </summary>
    internal abstract partial class UserQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) user from the database.
        /// </summary>
        /// <param name="user">The user object to delete.</param>
        /// <returns>A query that can be used to delete the user from the database.</returns>
        internal static string Delete(User user, ref object[] parameters)
        {
            string query = string.Empty;
            if (user != null)
            {
                query = "INSERT INTO [zHstUser] ([ID], [FirstName], [LastName], [FKUserType], [LoginName], [Password], [StartDate], [RequiresPasswordChange], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FirstName], [LastName], [FKUserType], [LoginName], [Password], [StartDate], [RequiresPasswordChange], [IsActive], [StampDate], [StampUserID] FROM [User] WHERE [User].[ID] = @ID; ";
                query += "DELETE FROM [User] WHERE [User].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", user.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) user from the database.
        /// </summary>
        /// <param name="user">The user object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the user from the database.</returns>
        internal static string DeleteHistory(User user, ref object[] parameters)
        {
            string query = string.Empty;
            if (user != null)
            {
                query = "DELETE FROM [zHstUser] WHERE [zHstUser].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", user.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) user from the database.
        /// </summary>
        /// <param name="user">The user object to undelete.</param>
        /// <returns>A query that can be used to undelete the user from the database.</returns>
        internal static string UnDelete(User user, ref object[] parameters)
        {
            string query = string.Empty;
            if (user != null)
            {
                query = "INSERT INTO [User] ([ID], [FirstName], [LastName], [FKUserType], [LoginName], [Password], [StartDate], [RequiresPasswordChange], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FirstName], [LastName], [FKUserType], [LoginName], [Password], [StartDate], [RequiresPasswordChange], [IsActive], [StampDate], [StampUserID] FROM [zHstUser] WHERE [zHstUser].[ID] = @ID AND [zHstUser].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstUser] WHERE [zHstUser].[ID] = @ID) AND (SELECT COUNT(ID) FROM [User] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstUser] WHERE [zHstUser].[ID] = @ID AND [zHstUser].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstUser] WHERE [zHstUser].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [User] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", user.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an user object.
        /// </summary>
        /// <param name="user">The user object to fill.</param>
        /// <returns>A query that can be used to fill the user object.</returns>
        internal static string Fill(User user, ref object[] parameters)
        {
            string query = string.Empty;
            if (user != null)
            {
                query = "SELECT [ID], [FirstName], [LastName], [FKUserType], [LoginName], [Password], [StartDate], [RequiresPasswordChange], [IsActive], [StampDate], [StampUserID] FROM [User] WHERE [User].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", user.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  user data.
        /// </summary>
        /// <param name="user">The user to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  user data.</returns>
        internal static string FillData(User user, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (user != null)
            {
            query.Append("SELECT [User].[ID], [User].[FirstName], [User].[LastName], [User].[FKUserType], [User].[LoginName], [User].[Password], [User].[StartDate], [User].[RequiresPasswordChange], [User].[IsActive], [User].[StampDate], [User].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [User].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [User] ");
                query.Append(" WHERE [User].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", user.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an user object from history.
        /// </summary>
        /// <param name="user">The user object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the user object from history.</returns>
        internal static string FillHistory(User user, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (user != null)
            {
                query = "SELECT [ID], [FirstName], [LastName], [FKUserType], [LoginName], [Password], [StartDate], [RequiresPasswordChange], [IsActive], [StampDate], [StampUserID] FROM [zHstUser] WHERE [zHstUser].[ID] = @ID AND [zHstUser].[StampUserID] = @StampUserID AND [zHstUser].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", user.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the users in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the users in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [User].[ID], [User].[FirstName], [User].[LastName], [User].[FKUserType], [User].[LoginName], [User].[Password], [User].[StartDate], [User].[RequiresPasswordChange], [User].[IsActive], [User].[StampDate], [User].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [User].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [User] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted users in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted users in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstUser].[ID], [zHstUser].[FirstName], [zHstUser].[LastName], [zHstUser].[FKUserType], [zHstUser].[LoginName], [zHstUser].[Password], [zHstUser].[StartDate], [zHstUser].[RequiresPasswordChange], [zHstUser].[IsActive], [zHstUser].[StampDate], [zHstUser].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstUser].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstUser] ");
            query.Append("INNER JOIN (SELECT [zHstUser].[ID], MAX([zHstUser].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstUser] ");
            query.Append("WHERE [zHstUser].[ID] NOT IN (SELECT [User].[ID] FROM [User]) ");
            query.Append("GROUP BY [zHstUser].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstUser].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstUser].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) user in the database.
        /// </summary>
        /// <param name="user">The user object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) user in the database.</returns>
        public static string ListHistory(User user, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (user != null)
            {
            query.Append("SELECT [zHstUser].[ID], [zHstUser].[FirstName], [zHstUser].[LastName], [zHstUser].[FKUserType], [zHstUser].[LoginName], [zHstUser].[Password], [zHstUser].[StartDate], [zHstUser].[RequiresPasswordChange], [zHstUser].[IsActive], [zHstUser].[StampDate], [zHstUser].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstUser].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstUser] ");
                query.Append(" WHERE [zHstUser].[ID] = @ID");
                query.Append(" ORDER BY [zHstUser].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", user.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) user to the database.
        /// </summary>
        /// <param name="user">The user to save.</param>
        /// <returns>A query that can be used to save the user to the database.</returns>
        internal static string Save(User user, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (user != null)
            {
                if (user.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstUser] ([ID], [FirstName], [LastName], [FKUserType], [LoginName], [Password], [StartDate], [RequiresPasswordChange], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FirstName], [LastName], [FKUserType], [LoginName], [Password], [StartDate], [RequiresPasswordChange], [IsActive], [StampDate], [StampUserID] FROM [User] WHERE [User].[ID] = @ID; ");
                    query.Append("UPDATE [User]");
                    parameters = new object[9];
                    query.Append(" SET [FirstName] = @FirstName");
                    parameters[0] = Database.GetParameter("@FirstName", string.IsNullOrEmpty(user.FirstName) ? DBNull.Value : (object)user.FirstName);
                    query.Append(", [LastName] = @LastName");
                    parameters[1] = Database.GetParameter("@LastName", string.IsNullOrEmpty(user.LastName) ? DBNull.Value : (object)user.LastName);
                    query.Append(", [FKUserType] = @FKUserType");
                    parameters[2] = Database.GetParameter("@FKUserType", user.FKUserType.HasValue ? (object)user.FKUserType.Value : DBNull.Value);
                    query.Append(", [LoginName] = @LoginName");
                    parameters[3] = Database.GetParameter("@LoginName", string.IsNullOrEmpty(user.LoginName) ? DBNull.Value : (object)user.LoginName);
                    query.Append(", [Password] = @Password");
                    parameters[4] = Database.GetParameter("@Password", string.IsNullOrEmpty(user.Password) ? DBNull.Value : (object)user.Password);
                    query.Append(", [StartDate] = @StartDate");
                    parameters[5] = Database.GetParameter("@StartDate", user.StartDate.HasValue ? (object)user.StartDate.Value : DBNull.Value);
                    query.Append(", [RequiresPasswordChange] = @RequiresPasswordChange");
                    parameters[6] = Database.GetParameter("@RequiresPasswordChange", user.RequiresPasswordChange);
                    query.Append(", [IsActive] = @IsActive");
                    parameters[7] = Database.GetParameter("@IsActive", user.IsActive.HasValue ? (object)user.IsActive.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [User].[ID] = @ID"); 
                    parameters[8] = Database.GetParameter("@ID", user.ID);
                }
                else
                {
                    query.Append("INSERT INTO [User] ([FirstName], [LastName], [FKUserType], [LoginName], [Password], [StartDate], [RequiresPasswordChange], [IsActive], [StampDate], [StampUserID]) VALUES(@FirstName, @LastName, @FKUserType, @LoginName, @Password, @StartDate, @RequiresPasswordChange, @IsActive, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[8];
                    parameters[0] = Database.GetParameter("@FirstName", string.IsNullOrEmpty(user.FirstName) ? DBNull.Value : (object)user.FirstName);
                    parameters[1] = Database.GetParameter("@LastName", string.IsNullOrEmpty(user.LastName) ? DBNull.Value : (object)user.LastName);
                    parameters[2] = Database.GetParameter("@FKUserType", user.FKUserType.HasValue ? (object)user.FKUserType.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@LoginName", string.IsNullOrEmpty(user.LoginName) ? DBNull.Value : (object)user.LoginName);
                    parameters[4] = Database.GetParameter("@Password", string.IsNullOrEmpty(user.Password) ? DBNull.Value : (object)user.Password);
                    parameters[5] = Database.GetParameter("@StartDate", user.StartDate.HasValue ? (object)user.StartDate.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@RequiresPasswordChange", user.RequiresPasswordChange);
                    parameters[7] = Database.GetParameter("@IsActive", user.IsActive.HasValue ? (object)user.IsActive.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for users that match the search criteria.
        /// </summary>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="lastname">The lastname search criteria.</param>
        /// <param name="fkusertype">The fkusertype search criteria.</param>
        /// <param name="loginname">The loginname search criteria.</param>
        /// <param name="password">The password search criteria.</param>
        /// <param name="startdate">The startdate search criteria.</param>
        /// <param name="requirespasswordchange">The requirespasswordchange search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <returns>A query that can be used to search for users based on the search criteria.</returns>
        internal static string Search(string firstname, string lastname, long? fkusertype, string loginname, string password, DateTime? startdate, bool? requirespasswordchange, bool? isactive)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (firstname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[User].[FirstName] LIKE '" + firstname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (lastname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[User].[LastName] LIKE '" + lastname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkusertype != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[User].[FKUserType] = " + fkusertype + "");
            }
            if (loginname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[User].[LoginName] LIKE '" + loginname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (password != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[User].[Password] LIKE '" + password.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (startdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[User].[StartDate] = '" + startdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (requirespasswordchange != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[User].[RequiresPasswordChange] = " + ((bool)requirespasswordchange ? "1" : "0"));
            }
            if (isactive != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[User].[IsActive] = " + ((bool)isactive ? "1" : "0"));
            }
            query.Append("SELECT [User].[ID], [User].[FirstName], [User].[LastName], [User].[FKUserType], [User].[LoginName], [User].[Password], [User].[StartDate], [User].[RequiresPasswordChange], [User].[IsActive], [User].[StampDate], [User].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [User].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [User] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
