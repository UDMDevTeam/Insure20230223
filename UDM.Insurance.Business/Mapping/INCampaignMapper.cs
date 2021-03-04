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
    /// Contains methods to fill, save and delete incampaign objects.
    /// </summary>
    public partial class INCampaignMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) incampaign object from the database.
        /// </summary>
        /// <param name="incampaign">The id of the incampaign object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the incampaign object.</param>
        /// <returns>True if the incampaign object was deleted successfully, else false.</returns>
        internal static bool Delete(INCampaign incampaign)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(incampaign.ConnectionName, INCampaignQueries.Delete(incampaign, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INCampaign object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) incampaign object from the database.
        /// </summary>
        /// <param name="incampaign">The id of the incampaign object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the incampaign history.</param>
        /// <returns>True if the incampaign history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INCampaign incampaign)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(incampaign.ConnectionName, INCampaignQueries.DeleteHistory(incampaign, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INCampaign history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) incampaign object from the database.
        /// </summary>
        /// <param name="incampaign">The id of the incampaign object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the incampaign object.</param>
        /// <returns>True if the incampaign object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INCampaign incampaign)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(incampaign.ConnectionName, INCampaignQueries.UnDelete(incampaign, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INCampaign object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the incampaign object from the data reader.
        /// </summary>
        /// <param name="incampaign">The incampaign object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INCampaign incampaign, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    incampaign.IsLoaded = true;
                    incampaign.FKINCampaignGroupID = reader["FKINCampaignGroupID"] != DBNull.Value ? (long)reader["FKINCampaignGroupID"] : (long?)null;
                    incampaign.FKINCampaignTypeID = reader["FKINCampaignTypeID"] != DBNull.Value ? (long)reader["FKINCampaignTypeID"] : (long?)null;
                    incampaign.Name = reader["Name"] != DBNull.Value ? (string)reader["Name"] : (string)null;
                    incampaign.Code = reader["Code"] != DBNull.Value ? (string)reader["Code"] : (string)null;
                    incampaign.Conversion1 = reader["Conversion1"] != DBNull.Value ? (Double)reader["Conversion1"] : (Double?)null;
                    incampaign.Conversion2 = reader["Conversion2"] != DBNull.Value ? (Double)reader["Conversion2"] : (Double?)null;
                    incampaign.Covnersion3 = reader["Covnersion3"] != DBNull.Value ? (Double)reader["Covnersion3"] : (Double?)null;
                    incampaign.Conversion4 = reader["Conversion4"] != DBNull.Value ? (Double)reader["Conversion4"] : (Double?)null;
                    incampaign.Conversion5 = reader["Conversion5"] != DBNull.Value ? (Double)reader["Conversion5"] : (Double?)null;
                    incampaign.Conversion6 = reader["Conversion6"] != DBNull.Value ? (Double)reader["Conversion6"] : (Double?)null;
                    incampaign.IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : (bool?)null;
                    incampaign.StampDate = (DateTime)reader["StampDate"];
                    incampaign.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INCampaign does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INCampaign object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an incampaign object from the database.
        /// </summary>
        /// <param name="incampaign">The incampaign to fill.</param>
        internal static void Fill(INCampaign incampaign)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(incampaign.ConnectionName, INCampaignQueries.Fill(incampaign, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(incampaign, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INCampaign object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a incampaign object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INCampaign incampaign)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(incampaign.ConnectionName, INCampaignQueries.FillData(incampaign, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INCampaign object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an incampaign object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="incampaign">The incampaign to fill from history.</param>
        internal static void FillHistory(INCampaign incampaign, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(incampaign.ConnectionName, INCampaignQueries.FillHistory(incampaign, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(incampaign, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INCampaign object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the incampaign objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INCampaignCollection List(bool showDeleted, string connectionName)
        {
            INCampaignCollection collection = new INCampaignCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INCampaignQueries.ListDeleted() : INCampaignQueries.List(), null);
                while (reader.Read())
                {
                    INCampaign incampaign = new INCampaign((long)reader["ID"]);
                    incampaign.ConnectionName = connectionName;
                    collection.Add(incampaign);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INCampaign objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the incampaign objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INCampaignQueries.ListDeleted() : INCampaignQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INCampaign objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) incampaign object from the database.
        /// </summary>
        /// <param name="incampaign">The incampaign to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INCampaign incampaign)
        {
            INCampaignCollection collection = new INCampaignCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(incampaign.ConnectionName, INCampaignQueries.ListHistory(incampaign, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INCampaign in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an incampaign object to the database.
        /// </summary>
        /// <param name="incampaign">The INCampaign object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the incampaign was saved successfull, otherwise, false.</returns>
        internal static bool Save(INCampaign incampaign)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (incampaign.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(incampaign.ConnectionName, INCampaignQueries.Save(incampaign, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(incampaign.ConnectionName, INCampaignQueries.Save(incampaign, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            incampaign.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= incampaign.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INCampaign object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for incampaign objects in the database.
        /// </summary>
        /// <param name="fkincampaigngroupid">The fkincampaigngroupid search criteria.</param>
        /// <param name="fkincampaigntypeid">The fkincampaigntypeid search criteria.</param>
        /// <param name="name">The name search criteria.</param>
        /// <param name="code">The code search criteria.</param>
        /// <param name="conversion1">The conversion1 search criteria.</param>
        /// <param name="conversion2">The conversion2 search criteria.</param>
        /// <param name="covnersion3">The covnersion3 search criteria.</param>
        /// <param name="conversion4">The conversion4 search criteria.</param>
        /// <param name="conversion5">The conversion5 search criteria.</param>
        /// <param name="conversion6">The conversion6 search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INCampaignCollection Search(long? fkincampaigngroupid, long? fkincampaigntypeid, string name, string code, Double? conversion1, Double? conversion2, Double? covnersion3, Double? conversion4, Double? conversion5, Double? conversion6, bool? isactive, string connectionName)
        {
            INCampaignCollection collection = new INCampaignCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INCampaignQueries.Search(fkincampaigngroupid, fkincampaigntypeid, name, code, conversion1, conversion2, covnersion3, conversion4, conversion5, conversion6, isactive), null);
                while (reader.Read())
                {
                    INCampaign incampaign = new INCampaign((long)reader["ID"]);
                    incampaign.ConnectionName = connectionName;
                    collection.Add(incampaign);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INCampaign objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for incampaign objects in the database.
        /// </summary>
        /// <param name="fkincampaigngroupid">The fkincampaigngroupid search criteria.</param>
        /// <param name="fkincampaigntypeid">The fkincampaigntypeid search criteria.</param>
        /// <param name="name">The name search criteria.</param>
        /// <param name="code">The code search criteria.</param>
        /// <param name="conversion1">The conversion1 search criteria.</param>
        /// <param name="conversion2">The conversion2 search criteria.</param>
        /// <param name="covnersion3">The covnersion3 search criteria.</param>
        /// <param name="conversion4">The conversion4 search criteria.</param>
        /// <param name="conversion5">The conversion5 search criteria.</param>
        /// <param name="conversion6">The conversion6 search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkincampaigngroupid, long? fkincampaigntypeid, string name, string code, Double? conversion1, Double? conversion2, Double? covnersion3, Double? conversion4, Double? conversion5, Double? conversion6, bool? isactive, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INCampaignQueries.Search(fkincampaigngroupid, fkincampaigntypeid, name, code, conversion1, conversion2, covnersion3, conversion4, conversion5, conversion6, isactive), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INCampaign objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 incampaign objects in the database.
        /// </summary>
        /// <param name="fkincampaigngroupid">The fkincampaigngroupid search criteria.</param>
        /// <param name="fkincampaigntypeid">The fkincampaigntypeid search criteria.</param>
        /// <param name="name">The name search criteria.</param>
        /// <param name="code">The code search criteria.</param>
        /// <param name="conversion1">The conversion1 search criteria.</param>
        /// <param name="conversion2">The conversion2 search criteria.</param>
        /// <param name="covnersion3">The covnersion3 search criteria.</param>
        /// <param name="conversion4">The conversion4 search criteria.</param>
        /// <param name="conversion5">The conversion5 search criteria.</param>
        /// <param name="conversion6">The conversion6 search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INCampaign SearchOne(long? fkincampaigngroupid, long? fkincampaigntypeid, string name, string code, Double? conversion1, Double? conversion2, Double? covnersion3, Double? conversion4, Double? conversion5, Double? conversion6, bool? isactive, string connectionName)
        {
            INCampaign incampaign = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INCampaignQueries.Search(fkincampaigngroupid, fkincampaigntypeid, name, code, conversion1, conversion2, covnersion3, conversion4, conversion5, conversion6, isactive), null);
                if (reader.Read())
                {
                    incampaign = new INCampaign((long)reader["ID"]);
                    incampaign.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INCampaign objects in the database", ex);
            }
            return incampaign;
        }
        #endregion
    }
}
