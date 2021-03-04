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
    /// Contains methods to fill, save and delete inpolicylifeassured objects.
    /// </summary>
    public partial class INPolicyLifeAssuredMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inpolicylifeassured object from the database.
        /// </summary>
        /// <param name="inpolicylifeassured">The id of the inpolicylifeassured object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inpolicylifeassured object.</param>
        /// <returns>True if the inpolicylifeassured object was deleted successfully, else false.</returns>
        internal static bool Delete(INPolicyLifeAssured inpolicylifeassured)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpolicylifeassured.ConnectionName, INPolicyLifeAssuredQueries.Delete(inpolicylifeassured, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INPolicyLifeAssured object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inpolicylifeassured object from the database.
        /// </summary>
        /// <param name="inpolicylifeassured">The id of the inpolicylifeassured object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inpolicylifeassured history.</param>
        /// <returns>True if the inpolicylifeassured history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INPolicyLifeAssured inpolicylifeassured)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpolicylifeassured.ConnectionName, INPolicyLifeAssuredQueries.DeleteHistory(inpolicylifeassured, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INPolicyLifeAssured history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inpolicylifeassured object from the database.
        /// </summary>
        /// <param name="inpolicylifeassured">The id of the inpolicylifeassured object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inpolicylifeassured object.</param>
        /// <returns>True if the inpolicylifeassured object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INPolicyLifeAssured inpolicylifeassured)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpolicylifeassured.ConnectionName, INPolicyLifeAssuredQueries.UnDelete(inpolicylifeassured, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INPolicyLifeAssured object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inpolicylifeassured object from the data reader.
        /// </summary>
        /// <param name="inpolicylifeassured">The inpolicylifeassured object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INPolicyLifeAssured inpolicylifeassured, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inpolicylifeassured.IsLoaded = true;
                    inpolicylifeassured.FKINPolicyID = reader["FKINPolicyID"] != DBNull.Value ? (long)reader["FKINPolicyID"] : (long?)null;
                    inpolicylifeassured.FKINLifeAssuredID = reader["FKINLifeAssuredID"] != DBNull.Value ? (long)reader["FKINLifeAssuredID"] : (long?)null;
                    inpolicylifeassured.LifeAssuredRank = reader["LifeAssuredRank"] != DBNull.Value ? (int)reader["LifeAssuredRank"] : (int?)null;
                    inpolicylifeassured.StampDate = (DateTime)reader["StampDate"];
                    inpolicylifeassured.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INPolicyLifeAssured does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPolicyLifeAssured object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inpolicylifeassured object from the database.
        /// </summary>
        /// <param name="inpolicylifeassured">The inpolicylifeassured to fill.</param>
        internal static void Fill(INPolicyLifeAssured inpolicylifeassured)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inpolicylifeassured.ConnectionName, INPolicyLifeAssuredQueries.Fill(inpolicylifeassured, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inpolicylifeassured, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPolicyLifeAssured object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inpolicylifeassured object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INPolicyLifeAssured inpolicylifeassured)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inpolicylifeassured.ConnectionName, INPolicyLifeAssuredQueries.FillData(inpolicylifeassured, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INPolicyLifeAssured object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inpolicylifeassured object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inpolicylifeassured">The inpolicylifeassured to fill from history.</param>
        internal static void FillHistory(INPolicyLifeAssured inpolicylifeassured, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inpolicylifeassured.ConnectionName, INPolicyLifeAssuredQueries.FillHistory(inpolicylifeassured, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inpolicylifeassured, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPolicyLifeAssured object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inpolicylifeassured objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INPolicyLifeAssuredCollection List(bool showDeleted, string connectionName)
        {
            INPolicyLifeAssuredCollection collection = new INPolicyLifeAssuredCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INPolicyLifeAssuredQueries.ListDeleted() : INPolicyLifeAssuredQueries.List(), null);
                while (reader.Read())
                {
                    INPolicyLifeAssured inpolicylifeassured = new INPolicyLifeAssured((long)reader["ID"]);
                    inpolicylifeassured.ConnectionName = connectionName;
                    collection.Add(inpolicylifeassured);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INPolicyLifeAssured objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inpolicylifeassured objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INPolicyLifeAssuredQueries.ListDeleted() : INPolicyLifeAssuredQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INPolicyLifeAssured objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inpolicylifeassured object from the database.
        /// </summary>
        /// <param name="inpolicylifeassured">The inpolicylifeassured to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INPolicyLifeAssured inpolicylifeassured)
        {
            INPolicyLifeAssuredCollection collection = new INPolicyLifeAssuredCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inpolicylifeassured.ConnectionName, INPolicyLifeAssuredQueries.ListHistory(inpolicylifeassured, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INPolicyLifeAssured in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inpolicylifeassured object to the database.
        /// </summary>
        /// <param name="inpolicylifeassured">The INPolicyLifeAssured object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inpolicylifeassured was saved successfull, otherwise, false.</returns>
        internal static bool Save(INPolicyLifeAssured inpolicylifeassured)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inpolicylifeassured.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inpolicylifeassured.ConnectionName, INPolicyLifeAssuredQueries.Save(inpolicylifeassured, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inpolicylifeassured.ConnectionName, INPolicyLifeAssuredQueries.Save(inpolicylifeassured, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inpolicylifeassured.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inpolicylifeassured.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INPolicyLifeAssured object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inpolicylifeassured objects in the database.
        /// </summary>
        /// <param name="fkinpolicyid">The fkinpolicyid search criteria.</param>
        /// <param name="fkinlifeassuredid">The fkinlifeassuredid search criteria.</param>
        /// <param name="lifeassuredrank">The lifeassuredrank search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INPolicyLifeAssuredCollection Search(long? fkinpolicyid, long? fkinlifeassuredid, int? lifeassuredrank, string connectionName)
        {
            INPolicyLifeAssuredCollection collection = new INPolicyLifeAssuredCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPolicyLifeAssuredQueries.Search(fkinpolicyid, fkinlifeassuredid, lifeassuredrank), null);
                while (reader.Read())
                {
                    INPolicyLifeAssured inpolicylifeassured = new INPolicyLifeAssured((long)reader["ID"]);
                    inpolicylifeassured.ConnectionName = connectionName;
                    collection.Add(inpolicylifeassured);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPolicyLifeAssured objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inpolicylifeassured objects in the database.
        /// </summary>
        /// <param name="fkinpolicyid">The fkinpolicyid search criteria.</param>
        /// <param name="fkinlifeassuredid">The fkinlifeassuredid search criteria.</param>
        /// <param name="lifeassuredrank">The lifeassuredrank search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinpolicyid, long? fkinlifeassuredid, int? lifeassuredrank, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INPolicyLifeAssuredQueries.Search(fkinpolicyid, fkinlifeassuredid, lifeassuredrank), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPolicyLifeAssured objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inpolicylifeassured objects in the database.
        /// </summary>
        /// <param name="fkinpolicyid">The fkinpolicyid search criteria.</param>
        /// <param name="fkinlifeassuredid">The fkinlifeassuredid search criteria.</param>
        /// <param name="lifeassuredrank">The lifeassuredrank search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INPolicyLifeAssured SearchOne(long? fkinpolicyid, long? fkinlifeassuredid, int? lifeassuredrank, string connectionName)
        {
            INPolicyLifeAssured inpolicylifeassured = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPolicyLifeAssuredQueries.Search(fkinpolicyid, fkinlifeassuredid, lifeassuredrank), null);
                if (reader.Read())
                {
                    inpolicylifeassured = new INPolicyLifeAssured((long)reader["ID"]);
                    inpolicylifeassured.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPolicyLifeAssured objects in the database", ex);
            }
            return inpolicylifeassured;
        }
        #endregion
    }
}
