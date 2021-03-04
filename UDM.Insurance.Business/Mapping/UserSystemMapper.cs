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
    /// Contains methods to fill, save and delete usersystem objects.
    /// </summary>
    public partial class UserSystemMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) usersystem object from the database.
        /// </summary>
        /// <param name="usersystem">The id of the usersystem object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the usersystem object.</param>
        /// <returns>True if the usersystem object was deleted successfully, else false.</returns>
        internal static bool Delete(UserSystem usersystem)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(usersystem.ConnectionName, UserSystemQueries.Delete(usersystem, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a UserSystem object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) usersystem object from the database.
        /// </summary>
        /// <param name="usersystem">The id of the usersystem object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the usersystem history.</param>
        /// <returns>True if the usersystem history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(UserSystem usersystem)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(usersystem.ConnectionName, UserSystemQueries.DeleteHistory(usersystem, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete UserSystem history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) usersystem object from the database.
        /// </summary>
        /// <param name="usersystem">The id of the usersystem object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the usersystem object.</param>
        /// <returns>True if the usersystem object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(UserSystem usersystem)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(usersystem.ConnectionName, UserSystemQueries.UnDelete(usersystem, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a UserSystem object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the usersystem object from the data reader.
        /// </summary>
        /// <param name="usersystem">The usersystem object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(UserSystem usersystem, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    usersystem.IsLoaded = true;
                    usersystem.FKUserID = reader["FKUserID"] != DBNull.Value ? (long)reader["FKUserID"] : 0;
                    usersystem.FKSystemID = reader["FKSystemID"] != DBNull.Value ? (long)reader["FKSystemID"] : 0;
                    usersystem.HasChanged = false;
                }
                else
                {
                    throw new MapperException("UserSystem does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a UserSystem object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an usersystem object from the database.
        /// </summary>
        /// <param name="usersystem">The usersystem to fill.</param>
        internal static void Fill(UserSystem usersystem)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(usersystem.ConnectionName, UserSystemQueries.Fill(usersystem, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(usersystem, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a UserSystem object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a usersystem object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(UserSystem usersystem)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(usersystem.ConnectionName, UserSystemQueries.FillData(usersystem, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a UserSystem object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an usersystem object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="usersystem">The usersystem to fill from history.</param>
        internal static void FillHistory(UserSystem usersystem, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(usersystem.ConnectionName, UserSystemQueries.FillHistory(usersystem, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(usersystem, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a UserSystem object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the usersystem objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static UserSystemCollection List(bool showDeleted, string connectionName)
        {
            UserSystemCollection collection = new UserSystemCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? UserSystemQueries.ListDeleted() : UserSystemQueries.List(), null);
                while (reader.Read())
                {
                    UserSystem usersystem = new UserSystem((long)reader["ID"]);
                    usersystem.ConnectionName = connectionName;
                    collection.Add(usersystem);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list UserSystem objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the usersystem objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? UserSystemQueries.ListDeleted() : UserSystemQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list UserSystem objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) usersystem object from the database.
        /// </summary>
        /// <param name="usersystem">The usersystem to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(UserSystem usersystem)
        {
            UserSystemCollection collection = new UserSystemCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(usersystem.ConnectionName, UserSystemQueries.ListHistory(usersystem, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) UserSystem in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an usersystem object to the database.
        /// </summary>
        /// <param name="usersystem">The UserSystem object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the usersystem was saved successfull, otherwise, false.</returns>
        internal static bool Save(UserSystem usersystem)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (usersystem.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(usersystem.ConnectionName, UserSystemQueries.Save(usersystem, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(usersystem.ConnectionName, UserSystemQueries.Save(usersystem, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            usersystem.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= usersystem.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a UserSystem object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for usersystem objects in the database.
        /// </summary>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static UserSystemCollection Search(long? fkuserid, long? fksystemid, string connectionName)
        {
            UserSystemCollection collection = new UserSystemCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, UserSystemQueries.Search(fkuserid, fksystemid), null);
                while (reader.Read())
                {
                    UserSystem usersystem = new UserSystem((long)reader["ID"]);
                    usersystem.ConnectionName = connectionName;
                    collection.Add(usersystem);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for UserSystem objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for usersystem objects in the database.
        /// </summary>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkuserid, long? fksystemid, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, UserSystemQueries.Search(fkuserid, fksystemid), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for UserSystem objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 usersystem objects in the database.
        /// </summary>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static UserSystem SearchOne(long? fkuserid, long? fksystemid, string connectionName)
        {
            UserSystem usersystem = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, UserSystemQueries.Search(fkuserid, fksystemid), null);
                if (reader.Read())
                {
                    usersystem = new UserSystem((long)reader["ID"]);
                    usersystem.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for UserSystem objects in the database", ex);
            }
            return usersystem;
        }
        #endregion
    }
}
