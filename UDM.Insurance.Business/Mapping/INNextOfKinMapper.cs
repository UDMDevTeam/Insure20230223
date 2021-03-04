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
    /// Contains methods to fill, save and delete innextofkin objects.
    /// </summary>
    public partial class INNextOfKinMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) innextofkin object from the database.
        /// </summary>
        /// <param name="innextofkin">The id of the innextofkin object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the innextofkin object.</param>
        /// <returns>True if the innextofkin object was deleted successfully, else false.</returns>
        internal static bool Delete(INNextOfKin innextofkin)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(innextofkin.ConnectionName, INNextOfKinQueries.Delete(innextofkin, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INNextOfKin object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) innextofkin object from the database.
        /// </summary>
        /// <param name="innextofkin">The id of the innextofkin object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the innextofkin history.</param>
        /// <returns>True if the innextofkin history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INNextOfKin innextofkin)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(innextofkin.ConnectionName, INNextOfKinQueries.DeleteHistory(innextofkin, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INNextOfKin history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) innextofkin object from the database.
        /// </summary>
        /// <param name="innextofkin">The id of the innextofkin object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the innextofkin object.</param>
        /// <returns>True if the innextofkin object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INNextOfKin innextofkin)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(innextofkin.ConnectionName, INNextOfKinQueries.UnDelete(innextofkin, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INNextOfKin object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the innextofkin object from the data reader.
        /// </summary>
        /// <param name="innextofkin">The innextofkin object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INNextOfKin innextofkin, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    innextofkin.IsLoaded = true;
                    innextofkin.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (long)reader["FKINImportID"] : (long?)null;
                    innextofkin.FKINRelationshipID = reader["FKINRelationshipID"] != DBNull.Value ? (long)reader["FKINRelationshipID"] : (long?)null;
                    innextofkin.FirstName = reader["FirstName"] != DBNull.Value ? (string)reader["FirstName"] : (string)null;
                    innextofkin.Surname = reader["Surname"] != DBNull.Value ? (string)reader["Surname"] : (string)null;
                    innextofkin.TelContact = reader["TelContact"] != DBNull.Value ? (string)reader["TelContact"] : (string)null;
                    innextofkin.StampDate = (DateTime)reader["StampDate"];
                    innextofkin.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INNextOfKin does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INNextOfKin object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an innextofkin object from the database.
        /// </summary>
        /// <param name="innextofkin">The innextofkin to fill.</param>
        internal static void Fill(INNextOfKin innextofkin)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(innextofkin.ConnectionName, INNextOfKinQueries.Fill(innextofkin, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(innextofkin, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INNextOfKin object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a innextofkin object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INNextOfKin innextofkin)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(innextofkin.ConnectionName, INNextOfKinQueries.FillData(innextofkin, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INNextOfKin object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an innextofkin object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="innextofkin">The innextofkin to fill from history.</param>
        internal static void FillHistory(INNextOfKin innextofkin, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(innextofkin.ConnectionName, INNextOfKinQueries.FillHistory(innextofkin, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(innextofkin, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INNextOfKin object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the innextofkin objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INNextOfKinCollection List(bool showDeleted, string connectionName)
        {
            INNextOfKinCollection collection = new INNextOfKinCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INNextOfKinQueries.ListDeleted() : INNextOfKinQueries.List(), null);
                while (reader.Read())
                {
                    INNextOfKin innextofkin = new INNextOfKin((long)reader["ID"]);
                    innextofkin.ConnectionName = connectionName;
                    collection.Add(innextofkin);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INNextOfKin objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the innextofkin objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INNextOfKinQueries.ListDeleted() : INNextOfKinQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INNextOfKin objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) innextofkin object from the database.
        /// </summary>
        /// <param name="innextofkin">The innextofkin to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INNextOfKin innextofkin)
        {
            INNextOfKinCollection collection = new INNextOfKinCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(innextofkin.ConnectionName, INNextOfKinQueries.ListHistory(innextofkin, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INNextOfKin in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an innextofkin object to the database.
        /// </summary>
        /// <param name="innextofkin">The INNextOfKin object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the innextofkin was saved successfull, otherwise, false.</returns>
        internal static bool Save(INNextOfKin innextofkin)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (innextofkin.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(innextofkin.ConnectionName, INNextOfKinQueries.Save(innextofkin, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(innextofkin.ConnectionName, INNextOfKinQueries.Save(innextofkin, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            innextofkin.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= innextofkin.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INNextOfKin object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for innextofkin objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkinrelationshipid">The fkinrelationshipid search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="telcontact">The telcontact search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INNextOfKinCollection Search(long? fkinimportid, long? fkinrelationshipid, string firstname, string surname, string telcontact, string connectionName)
        {
            INNextOfKinCollection collection = new INNextOfKinCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INNextOfKinQueries.Search(fkinimportid, fkinrelationshipid, firstname, surname, telcontact), null);
                while (reader.Read())
                {
                    INNextOfKin innextofkin = new INNextOfKin((long)reader["ID"]);
                    innextofkin.ConnectionName = connectionName;
                    collection.Add(innextofkin);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INNextOfKin objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for innextofkin objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkinrelationshipid">The fkinrelationshipid search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="telcontact">The telcontact search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinimportid, long? fkinrelationshipid, string firstname, string surname, string telcontact, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INNextOfKinQueries.Search(fkinimportid, fkinrelationshipid, firstname, surname, telcontact), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INNextOfKin objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 innextofkin objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkinrelationshipid">The fkinrelationshipid search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="telcontact">The telcontact search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INNextOfKin SearchOne(long? fkinimportid, long? fkinrelationshipid, string firstname, string surname, string telcontact, string connectionName)
        {
            INNextOfKin innextofkin = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INNextOfKinQueries.Search(fkinimportid, fkinrelationshipid, firstname, surname, telcontact), null);
                if (reader.Read())
                {
                    innextofkin = new INNextOfKin((long)reader["ID"]);
                    innextofkin.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INNextOfKin objects in the database", ex);
            }
            return innextofkin;
        }
        #endregion
    }
}
