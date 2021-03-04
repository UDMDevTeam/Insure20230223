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
    /// Contains methods to fill, save and delete postalcode objects.
    /// </summary>
    public partial class PostalCodeMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) postalcode object from the database.
        /// </summary>
        /// <param name="postalcode">The id of the postalcode object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the postalcode object.</param>
        /// <returns>True if the postalcode object was deleted successfully, else false.</returns>
        internal static bool Delete(PostalCode postalcode)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(postalcode.ConnectionName, PostalCodeQueries.Delete(postalcode, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a PostalCode object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) postalcode object from the database.
        /// </summary>
        /// <param name="postalcode">The id of the postalcode object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the postalcode history.</param>
        /// <returns>True if the postalcode history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(PostalCode postalcode)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(postalcode.ConnectionName, PostalCodeQueries.DeleteHistory(postalcode, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete PostalCode history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) postalcode object from the database.
        /// </summary>
        /// <param name="postalcode">The id of the postalcode object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the postalcode object.</param>
        /// <returns>True if the postalcode object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(PostalCode postalcode)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(postalcode.ConnectionName, PostalCodeQueries.UnDelete(postalcode, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a PostalCode object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the postalcode object from the data reader.
        /// </summary>
        /// <param name="postalcode">The postalcode object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(PostalCode postalcode, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    postalcode.IsLoaded = true;
                    postalcode.Suburb = reader["Suburb"] != DBNull.Value ? (string)reader["Suburb"] : (string)null;
                    postalcode.BoxCode = reader["BoxCode"] != DBNull.Value ? (string)reader["BoxCode"] : (string)null;
                    postalcode.StreetCode = reader["StreetCode"] != DBNull.Value ? (string)reader["StreetCode"] : (string)null;
                    postalcode.City = reader["City"] != DBNull.Value ? (string)reader["City"] : (string)null;
                    postalcode.StampDate = (DateTime)reader["StampDate"];
                    postalcode.HasChanged = false;
                }
                else
                {
                    throw new MapperException("PostalCode does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a PostalCode object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an postalcode object from the database.
        /// </summary>
        /// <param name="postalcode">The postalcode to fill.</param>
        internal static void Fill(PostalCode postalcode)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(postalcode.ConnectionName, PostalCodeQueries.Fill(postalcode, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(postalcode, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a PostalCode object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a postalcode object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(PostalCode postalcode)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(postalcode.ConnectionName, PostalCodeQueries.FillData(postalcode, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a PostalCode object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an postalcode object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="postalcode">The postalcode to fill from history.</param>
        internal static void FillHistory(PostalCode postalcode, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(postalcode.ConnectionName, PostalCodeQueries.FillHistory(postalcode, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(postalcode, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a PostalCode object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the postalcode objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static PostalCodeCollection List(bool showDeleted, string connectionName)
        {
            PostalCodeCollection collection = new PostalCodeCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? PostalCodeQueries.ListDeleted() : PostalCodeQueries.List(), null);
                while (reader.Read())
                {
                    PostalCode postalcode = new PostalCode((long)reader["ID"]);
                    postalcode.ConnectionName = connectionName;
                    collection.Add(postalcode);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list PostalCode objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the postalcode objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? PostalCodeQueries.ListDeleted() : PostalCodeQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list PostalCode objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) postalcode object from the database.
        /// </summary>
        /// <param name="postalcode">The postalcode to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(PostalCode postalcode)
        {
            PostalCodeCollection collection = new PostalCodeCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(postalcode.ConnectionName, PostalCodeQueries.ListHistory(postalcode, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) PostalCode in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an postalcode object to the database.
        /// </summary>
        /// <param name="postalcode">The PostalCode object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the postalcode was saved successfull, otherwise, false.</returns>
        internal static bool Save(PostalCode postalcode)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (postalcode.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(postalcode.ConnectionName, PostalCodeQueries.Save(postalcode, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(postalcode.ConnectionName, PostalCodeQueries.Save(postalcode, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            postalcode.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= postalcode.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a PostalCode object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for postalcode objects in the database.
        /// </summary>
        /// <param name="suburb">The suburb search criteria.</param>
        /// <param name="boxcode">The boxcode search criteria.</param>
        /// <param name="streetcode">The streetcode search criteria.</param>
        /// <param name="city">The city search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static PostalCodeCollection Search(string suburb, string boxcode, string streetcode, string city, string connectionName)
        {
            PostalCodeCollection collection = new PostalCodeCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, PostalCodeQueries.Search(suburb, boxcode, streetcode, city), null);
                while (reader.Read())
                {
                    PostalCode postalcode = new PostalCode((long)reader["ID"]);
                    postalcode.ConnectionName = connectionName;
                    collection.Add(postalcode);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for PostalCode objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for postalcode objects in the database.
        /// </summary>
        /// <param name="suburb">The suburb search criteria.</param>
        /// <param name="boxcode">The boxcode search criteria.</param>
        /// <param name="streetcode">The streetcode search criteria.</param>
        /// <param name="city">The city search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string suburb, string boxcode, string streetcode, string city, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, PostalCodeQueries.Search(suburb, boxcode, streetcode, city), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for PostalCode objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 postalcode objects in the database.
        /// </summary>
        /// <param name="suburb">The suburb search criteria.</param>
        /// <param name="boxcode">The boxcode search criteria.</param>
        /// <param name="streetcode">The streetcode search criteria.</param>
        /// <param name="city">The city search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static PostalCode SearchOne(string suburb, string boxcode, string streetcode, string city, string connectionName)
        {
            PostalCode postalcode = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, PostalCodeQueries.Search(suburb, boxcode, streetcode, city), null);
                if (reader.Read())
                {
                    postalcode = new PostalCode((long)reader["ID"]);
                    postalcode.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for PostalCode objects in the database", ex);
            }
            return postalcode;
        }
        #endregion
    }
}
