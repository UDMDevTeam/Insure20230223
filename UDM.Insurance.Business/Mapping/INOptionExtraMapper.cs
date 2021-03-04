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
    /// Contains methods to fill, save and delete inoptionextra objects.
    /// </summary>
    public partial class INOptionExtraMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inoptionextra object from the database.
        /// </summary>
        /// <param name="inoptionextra">The id of the inoptionextra object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inoptionextra object.</param>
        /// <returns>True if the inoptionextra object was deleted successfully, else false.</returns>
        internal static bool Delete(INOptionExtra inoptionextra)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inoptionextra.ConnectionName, INOptionExtraQueries.Delete(inoptionextra, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INOptionExtra object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inoptionextra object from the database.
        /// </summary>
        /// <param name="inoptionextra">The id of the inoptionextra object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inoptionextra history.</param>
        /// <returns>True if the inoptionextra history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INOptionExtra inoptionextra)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inoptionextra.ConnectionName, INOptionExtraQueries.DeleteHistory(inoptionextra, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INOptionExtra history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inoptionextra object from the database.
        /// </summary>
        /// <param name="inoptionextra">The id of the inoptionextra object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inoptionextra object.</param>
        /// <returns>True if the inoptionextra object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INOptionExtra inoptionextra)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inoptionextra.ConnectionName, INOptionExtraQueries.UnDelete(inoptionextra, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INOptionExtra object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inoptionextra object from the data reader.
        /// </summary>
        /// <param name="inoptionextra">The inoptionextra object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INOptionExtra inoptionextra, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inoptionextra.IsLoaded = true;
                    inoptionextra.FKOptionID = reader["FKOptionID"] != DBNull.Value ? (long)reader["FKOptionID"] : (long?)null;
                    inoptionextra.LA1CancerComponent = reader["LA1CancerComponent"] != DBNull.Value ? (string)reader["LA1CancerComponent"] : (string)null;
                    inoptionextra.LA1CancerCover = reader["LA1CancerCover"] != DBNull.Value ? (decimal)reader["LA1CancerCover"] : (decimal?)null;
                    inoptionextra.LA1CancerPremium = reader["LA1CancerPremium"] != DBNull.Value ? (decimal)reader["LA1CancerPremium"] : (decimal?)null;
                    inoptionextra.LA1CancerCost = reader["LA1CancerCost"] != DBNull.Value ? (decimal)reader["LA1CancerCost"] : (decimal?)null;
                    inoptionextra.LA2CancerComponent = reader["LA2CancerComponent"] != DBNull.Value ? (string)reader["LA2CancerComponent"] : (string)null;
                    inoptionextra.LA2CancerCover = reader["LA2CancerCover"] != DBNull.Value ? (decimal)reader["LA2CancerCover"] : (decimal?)null;
                    inoptionextra.LA2CancerPremium = reader["LA2CancerPremium"] != DBNull.Value ? (decimal)reader["LA2CancerPremium"] : (decimal?)null;
                    inoptionextra.LA2CancerCost = reader["LA2CancerCost"] != DBNull.Value ? (decimal)reader["LA2CancerCost"] : (decimal?)null;
                    inoptionextra.StampDate = (DateTime)reader["StampDate"];
                    inoptionextra.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INOptionExtra does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INOptionExtra object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inoptionextra object from the database.
        /// </summary>
        /// <param name="inoptionextra">The inoptionextra to fill.</param>
        internal static void Fill(INOptionExtra inoptionextra)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inoptionextra.ConnectionName, INOptionExtraQueries.Fill(inoptionextra, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inoptionextra, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INOptionExtra object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inoptionextra object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INOptionExtra inoptionextra)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inoptionextra.ConnectionName, INOptionExtraQueries.FillData(inoptionextra, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INOptionExtra object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inoptionextra object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inoptionextra">The inoptionextra to fill from history.</param>
        internal static void FillHistory(INOptionExtra inoptionextra, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inoptionextra.ConnectionName, INOptionExtraQueries.FillHistory(inoptionextra, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inoptionextra, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INOptionExtra object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inoptionextra objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INOptionExtraCollection List(bool showDeleted, string connectionName)
        {
            INOptionExtraCollection collection = new INOptionExtraCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INOptionExtraQueries.ListDeleted() : INOptionExtraQueries.List(), null);
                while (reader.Read())
                {
                    INOptionExtra inoptionextra = new INOptionExtra((long)reader["ID"]);
                    inoptionextra.ConnectionName = connectionName;
                    collection.Add(inoptionextra);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INOptionExtra objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inoptionextra objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INOptionExtraQueries.ListDeleted() : INOptionExtraQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INOptionExtra objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inoptionextra object from the database.
        /// </summary>
        /// <param name="inoptionextra">The inoptionextra to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INOptionExtra inoptionextra)
        {
            INOptionExtraCollection collection = new INOptionExtraCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inoptionextra.ConnectionName, INOptionExtraQueries.ListHistory(inoptionextra, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INOptionExtra in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inoptionextra object to the database.
        /// </summary>
        /// <param name="inoptionextra">The INOptionExtra object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inoptionextra was saved successfull, otherwise, false.</returns>
        internal static bool Save(INOptionExtra inoptionextra)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inoptionextra.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inoptionextra.ConnectionName, INOptionExtraQueries.Save(inoptionextra, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inoptionextra.ConnectionName, INOptionExtraQueries.Save(inoptionextra, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inoptionextra.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inoptionextra.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INOptionExtra object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inoptionextra objects in the database.
        /// </summary>
        /// <param name="fkoptionid">The fkoptionid search criteria.</param>
        /// <param name="la1cancercomponent">The la1cancercomponent search criteria.</param>
        /// <param name="la1cancercover">The la1cancercover search criteria.</param>
        /// <param name="la1cancerpremium">The la1cancerpremium search criteria.</param>
        /// <param name="la1cancercost">The la1cancercost search criteria.</param>
        /// <param name="la2cancercomponent">The la2cancercomponent search criteria.</param>
        /// <param name="la2cancercover">The la2cancercover search criteria.</param>
        /// <param name="la2cancerpremium">The la2cancerpremium search criteria.</param>
        /// <param name="la2cancercost">The la2cancercost search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INOptionExtraCollection Search(long? fkoptionid, string la1cancercomponent, decimal? la1cancercover, decimal? la1cancerpremium, decimal? la1cancercost, string la2cancercomponent, decimal? la2cancercover, decimal? la2cancerpremium, decimal? la2cancercost, string connectionName)
        {
            INOptionExtraCollection collection = new INOptionExtraCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INOptionExtraQueries.Search(fkoptionid, la1cancercomponent, la1cancercover, la1cancerpremium, la1cancercost, la2cancercomponent, la2cancercover, la2cancerpremium, la2cancercost), null);
                while (reader.Read())
                {
                    INOptionExtra inoptionextra = new INOptionExtra((long)reader["ID"]);
                    inoptionextra.ConnectionName = connectionName;
                    collection.Add(inoptionextra);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INOptionExtra objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inoptionextra objects in the database.
        /// </summary>
        /// <param name="fkoptionid">The fkoptionid search criteria.</param>
        /// <param name="la1cancercomponent">The la1cancercomponent search criteria.</param>
        /// <param name="la1cancercover">The la1cancercover search criteria.</param>
        /// <param name="la1cancerpremium">The la1cancerpremium search criteria.</param>
        /// <param name="la1cancercost">The la1cancercost search criteria.</param>
        /// <param name="la2cancercomponent">The la2cancercomponent search criteria.</param>
        /// <param name="la2cancercover">The la2cancercover search criteria.</param>
        /// <param name="la2cancerpremium">The la2cancerpremium search criteria.</param>
        /// <param name="la2cancercost">The la2cancercost search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkoptionid, string la1cancercomponent, decimal? la1cancercover, decimal? la1cancerpremium, decimal? la1cancercost, string la2cancercomponent, decimal? la2cancercover, decimal? la2cancerpremium, decimal? la2cancercost, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INOptionExtraQueries.Search(fkoptionid, la1cancercomponent, la1cancercover, la1cancerpremium, la1cancercost, la2cancercomponent, la2cancercover, la2cancerpremium, la2cancercost), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INOptionExtra objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inoptionextra objects in the database.
        /// </summary>
        /// <param name="fkoptionid">The fkoptionid search criteria.</param>
        /// <param name="la1cancercomponent">The la1cancercomponent search criteria.</param>
        /// <param name="la1cancercover">The la1cancercover search criteria.</param>
        /// <param name="la1cancerpremium">The la1cancerpremium search criteria.</param>
        /// <param name="la1cancercost">The la1cancercost search criteria.</param>
        /// <param name="la2cancercomponent">The la2cancercomponent search criteria.</param>
        /// <param name="la2cancercover">The la2cancercover search criteria.</param>
        /// <param name="la2cancerpremium">The la2cancerpremium search criteria.</param>
        /// <param name="la2cancercost">The la2cancercost search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INOptionExtra SearchOne(long? fkoptionid, string la1cancercomponent, decimal? la1cancercover, decimal? la1cancerpremium, decimal? la1cancercost, string la2cancercomponent, decimal? la2cancercover, decimal? la2cancerpremium, decimal? la2cancercost, string connectionName)
        {
            INOptionExtra inoptionextra = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INOptionExtraQueries.Search(fkoptionid, la1cancercomponent, la1cancercover, la1cancerpremium, la1cancercost, la2cancercomponent, la2cancercover, la2cancerpremium, la2cancercost), null);
                if (reader.Read())
                {
                    inoptionextra = new INOptionExtra((long)reader["ID"]);
                    inoptionextra.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INOptionExtra objects in the database", ex);
            }
            return inoptionextra;
        }
        #endregion
    }
}
