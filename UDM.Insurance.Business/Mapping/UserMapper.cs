using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections;

using UDM.Insurance.Business;
using UDM.Insurance.Business.Queries;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using Embriant.Framework.Exceptions;
using Embriant.Framework;
using Embriant.Framework.Validation;

namespace UDM.Insurance.Business.Mapping
{
    /// <summary>
    /// Contains methods to fill, save and delete user objects.
    /// </summary>
    public partial class UserMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) user object from the database.
        /// </summary>
        /// <param name="user">The id of the user object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the user object.</param>
        /// <returns>True if the user object was deleted successfully, else false.</returns>
        internal static bool Delete(User user)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(user.ConnectionName, UserQueries.Delete(user, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a User object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) user object from the database.
        /// </summary>
        /// <param name="user">The id of the user object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the user history.</param>
        /// <returns>True if the user history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(User user)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(user.ConnectionName, UserQueries.DeleteHistory(user, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete User history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) user object from the database.
        /// </summary>
        /// <param name="user">The id of the user object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the user object.</param>
        /// <returns>True if the user object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(User user)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(user.ConnectionName, UserQueries.UnDelete(user, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a User object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the user object from the data reader.
        /// </summary>
        /// <param name="user">The user object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(User user, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    user.IsLoaded = true;
                    user.FirstName = reader["FirstName"] != DBNull.Value ? (string)reader["FirstName"] : (string)null;
                    user.LastName = reader["LastName"] != DBNull.Value ? (string)reader["LastName"] : (string)null;
                    user.FKUserType = reader["FKUserType"] != DBNull.Value ? (long)reader["FKUserType"] : (long?)null;
                    user.LoginName = reader["LoginName"] != DBNull.Value ? (string)reader["LoginName"] : (string)null;
                    user.Password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : (string)null;
                    user.StartDate = reader["StartDate"] != DBNull.Value ? (DateTime)reader["StartDate"] : (DateTime?)null;
                    user.RequiresPasswordChange = reader["RequiresPasswordChange"] != DBNull.Value ? (bool)reader["RequiresPasswordChange"] : false;
                    user.IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : (bool?)null;
                    user.StampDate = (DateTime)reader["StampDate"];
                    user.HasChanged = false;
                }
                else
                {
                    throw new MapperException("User does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a User object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an user object from the database.
        /// </summary>
        /// <param name="user">The user to fill.</param>
        internal static void Fill(User user)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(user.ConnectionName, UserQueries.Fill(user, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(user, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a User object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a user object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(User user)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(user.ConnectionName, UserQueries.FillData(user, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a User object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an user object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="user">The user to fill from history.</param>
        internal static void FillHistory(User user, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(user.ConnectionName, UserQueries.FillHistory(user, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(user, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a User object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the user objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static UserCollection List(bool showDeleted, string connectionName)
        {
            UserCollection collection = new UserCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? UserQueries.ListDeleted() : UserQueries.List(), null);
                while (reader.Read())
                {
                    User user = new User((long)reader["ID"]);
                    user.ConnectionName = connectionName;
                    collection.Add(user);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list User objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the user objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? UserQueries.ListDeleted() : UserQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list User objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) user object from the database.
        /// </summary>
        /// <param name="user">The user to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(User user)
        {
            UserCollection collection = new UserCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(user.ConnectionName, UserQueries.ListHistory(user, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) User in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an user object to the database.
        /// </summary>
        /// <param name="user">The User object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the user was saved successfull, otherwise, false.</returns>
        internal static bool Save(User user)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (user.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(user.ConnectionName, UserQueries.Save(user, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(user.ConnectionName, UserQueries.Save(user, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            user.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= user.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a User object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for user objects in the database.
        /// </summary>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="lastname">The lastname search criteria.</param>
        /// <param name="fkusertype">The fkusertype search criteria.</param>
        /// <param name="loginname">The loginname search criteria.</param>
        /// <param name="password">The password search criteria.</param>
        /// <param name="startdate">The startdate search criteria.</param>
        /// <param name="requirespasswordchange">The requirespasswordchange search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static UserCollection Search(string firstname, string lastname, long? fkusertype, string loginname, string password, DateTime? startdate, bool? requirespasswordchange, bool? isactive, string connectionName)
        {
            UserCollection collection = new UserCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, UserQueries.Search(firstname, lastname, fkusertype, loginname, password, startdate, requirespasswordchange, isactive), null);
                while (reader.Read())
                {
                    User user = new User((long)reader["ID"]);
                    user.ConnectionName = connectionName;
                    collection.Add(user);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for User objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for user objects in the database.
        /// </summary>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="lastname">The lastname search criteria.</param>
        /// <param name="fkusertype">The fkusertype search criteria.</param>
        /// <param name="loginname">The loginname search criteria.</param>
        /// <param name="password">The password search criteria.</param>
        /// <param name="startdate">The startdate search criteria.</param>
        /// <param name="requirespasswordchange">The requirespasswordchange search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string firstname, string lastname, long? fkusertype, string loginname, string password, DateTime? startdate, bool? requirespasswordchange, bool? isactive, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, UserQueries.Search(firstname, lastname, fkusertype, loginname, password, startdate, requirespasswordchange, isactive), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for User objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 user objects in the database.
        /// </summary>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="lastname">The lastname search criteria.</param>
        /// <param name="fkusertype">The fkusertype search criteria.</param>
        /// <param name="loginname">The loginname search criteria.</param>
        /// <param name="password">The password search criteria.</param>
        /// <param name="startdate">The startdate search criteria.</param>
        /// <param name="requirespasswordchange">The requirespasswordchange search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static User SearchOne(string firstname, string lastname, long? fkusertype, string loginname, string password, DateTime? startdate, bool? requirespasswordchange, bool? isactive, string connectionName)
        {
            User user = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, UserQueries.Search(firstname, lastname, fkusertype, loginname, password, startdate, requirespasswordchange, isactive), null);
                if (reader.Read())
                {
                    user = new User((long)reader["ID"]);
                    user.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for User objects in the database", ex);
            }
            return user;
        }
        #endregion
    }
}
