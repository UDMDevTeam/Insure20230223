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
    /// Contains methods to fill, save and delete scripts objects.
    /// </summary>
    public partial class ScriptsMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) scripts object from the database.
        /// </summary>
        /// <param name="scripts">The id of the scripts object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the scripts object.</param>
        /// <returns>True if the scripts object was deleted successfully, else false.</returns>
        internal static bool Delete(Scripts scripts)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(scripts.ConnectionName, ScriptsQueries.Delete(scripts, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a Scripts object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) scripts object from the database.
        /// </summary>
        /// <param name="scripts">The id of the scripts object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the scripts history.</param>
        /// <returns>True if the scripts history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(Scripts scripts)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(scripts.ConnectionName, ScriptsQueries.DeleteHistory(scripts, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete Scripts history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) scripts object from the database.
        /// </summary>
        /// <param name="scripts">The id of the scripts object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the scripts object.</param>
        /// <returns>True if the scripts object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(Scripts scripts)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(scripts.ConnectionName, ScriptsQueries.UnDelete(scripts, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a Scripts object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the scripts object from the data reader.
        /// </summary>
        /// <param name="scripts">The scripts object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(Scripts scripts, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    scripts.IsLoaded = true;
                    scripts.FKScriptTypeID = reader["FKScriptTypeID"] != DBNull.Value ? (long)reader["FKScriptTypeID"] : (long?)null;
                    scripts.FKCampaignID = reader["FKCampaignID"] != DBNull.Value ? (long)reader["FKCampaignID"] : (long?)null;
                    scripts.FKCampaignTypeID = reader["FKCampaignTypeID"] != DBNull.Value ? (long)reader["FKCampaignTypeID"] : (long?)null;
                    scripts.FKCampaignGroupID = reader["FKCampaignGroupID"] != DBNull.Value ? (long)reader["FKCampaignGroupID"] : (long?)null;
                    scripts.FKCampaignTypeGroupID = reader["FKCampaignTypeGroupID"] != DBNull.Value ? (long)reader["FKCampaignTypeGroupID"] : (long?)null;
                    scripts.FKCampaignGroupTypeID = reader["FKCampaignGroupTypeID"] != DBNull.Value ? (long)reader["FKCampaignGroupTypeID"] : (long?)null;
                    scripts.FKLanguageID = reader["FKLanguageID"] != DBNull.Value ? (long)reader["FKLanguageID"] : (long?)null;
                    scripts.Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : (string)null;
                    scripts.Document = reader["Document"] != DBNull.Value ? (byte[])reader["Document"] : (byte[])null;
                    scripts.Date = reader["Date"] != DBNull.Value ? (DateTime)reader["Date"] : (DateTime?)null;
                    scripts.IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : (bool?)null;
                    scripts.StampDate = (DateTime)reader["StampDate"];
                    scripts.HasChanged = false;
                }
                else
                {
                    throw new MapperException("Scripts does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Scripts object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an scripts object from the database.
        /// </summary>
        /// <param name="scripts">The scripts to fill.</param>
        internal static void Fill(Scripts scripts)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(scripts.ConnectionName, ScriptsQueries.Fill(scripts, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(scripts, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Scripts object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a scripts object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(Scripts scripts)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(scripts.ConnectionName, ScriptsQueries.FillData(scripts, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a Scripts object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an scripts object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="scripts">The scripts to fill from history.</param>
        internal static void FillHistory(Scripts scripts, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(scripts.ConnectionName, ScriptsQueries.FillHistory(scripts, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(scripts, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Scripts object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the scripts objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static ScriptsCollection List(bool showDeleted, string connectionName)
        {
            ScriptsCollection collection = new ScriptsCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? ScriptsQueries.ListDeleted() : ScriptsQueries.List(), null);
                while (reader.Read())
                {
                    Scripts scripts = new Scripts((long)reader["ID"]);
                    scripts.ConnectionName = connectionName;
                    collection.Add(scripts);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list Scripts objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the scripts objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? ScriptsQueries.ListDeleted() : ScriptsQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list Scripts objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) scripts object from the database.
        /// </summary>
        /// <param name="scripts">The scripts to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(Scripts scripts)
        {
            ScriptsCollection collection = new ScriptsCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(scripts.ConnectionName, ScriptsQueries.ListHistory(scripts, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) Scripts in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an scripts object to the database.
        /// </summary>
        /// <param name="scripts">The Scripts object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the scripts was saved successfull, otherwise, false.</returns>
        internal static bool Save(Scripts scripts)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (scripts.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(scripts.ConnectionName, ScriptsQueries.Save(scripts, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(scripts.ConnectionName, ScriptsQueries.Save(scripts, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            scripts.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= scripts.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a Scripts object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for scripts objects in the database.
        /// </summary>
        /// <param name="fkscripttypeid">The fkscripttypeid search criteria.</param>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="fkcampaigntypeid">The fkcampaigntypeid search criteria.</param>
        /// <param name="fkcampaigngroupid">The fkcampaigngroupid search criteria.</param>
        /// <param name="fkcampaigntypegroupid">The fkcampaigntypegroupid search criteria.</param>
        /// <param name="fkcampaigngrouptypeid">The fkcampaigngrouptypeid search criteria.</param>
        /// <param name="fklanguageid">The fklanguageid search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <param name="date">The date search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static ScriptsCollection Search(long? fkscripttypeid, long? fkcampaignid, long? fkcampaigntypeid, long? fkcampaigngroupid, long? fkcampaigntypegroupid, long? fkcampaigngrouptypeid, long? fklanguageid, string description, byte[] document, DateTime? date, bool? isactive, string connectionName)
        {
            ScriptsCollection collection = new ScriptsCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, ScriptsQueries.Search(fkscripttypeid, fkcampaignid, fkcampaigntypeid, fkcampaigngroupid, fkcampaigntypegroupid, fkcampaigngrouptypeid, fklanguageid, description, document, date, isactive), null);
                while (reader.Read())
                {
                    Scripts scripts = new Scripts((long)reader["ID"]);
                    scripts.ConnectionName = connectionName;
                    collection.Add(scripts);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for Scripts objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for scripts objects in the database.
        /// </summary>
        /// <param name="fkscripttypeid">The fkscripttypeid search criteria.</param>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="fkcampaigntypeid">The fkcampaigntypeid search criteria.</param>
        /// <param name="fkcampaigngroupid">The fkcampaigngroupid search criteria.</param>
        /// <param name="fkcampaigntypegroupid">The fkcampaigntypegroupid search criteria.</param>
        /// <param name="fkcampaigngrouptypeid">The fkcampaigngrouptypeid search criteria.</param>
        /// <param name="fklanguageid">The fklanguageid search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <param name="date">The date search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkscripttypeid, long? fkcampaignid, long? fkcampaigntypeid, long? fkcampaigngroupid, long? fkcampaigntypegroupid, long? fkcampaigngrouptypeid, long? fklanguageid, string description, byte[] document, DateTime? date, bool? isactive, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, ScriptsQueries.Search(fkscripttypeid, fkcampaignid, fkcampaigntypeid, fkcampaigngroupid, fkcampaigntypegroupid, fkcampaigngrouptypeid, fklanguageid, description, document, date, isactive), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for Scripts objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 scripts objects in the database.
        /// </summary>
        /// <param name="fkscripttypeid">The fkscripttypeid search criteria.</param>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="fkcampaigntypeid">The fkcampaigntypeid search criteria.</param>
        /// <param name="fkcampaigngroupid">The fkcampaigngroupid search criteria.</param>
        /// <param name="fkcampaigntypegroupid">The fkcampaigntypegroupid search criteria.</param>
        /// <param name="fkcampaigngrouptypeid">The fkcampaigngrouptypeid search criteria.</param>
        /// <param name="fklanguageid">The fklanguageid search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <param name="date">The date search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static Scripts SearchOne(long? fkscripttypeid, long? fkcampaignid, long? fkcampaigntypeid, long? fkcampaigngroupid, long? fkcampaigntypegroupid, long? fkcampaigngrouptypeid, long? fklanguageid, string description, byte[] document, DateTime? date, bool? isactive, string connectionName)
        {
            Scripts scripts = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, ScriptsQueries.Search(fkscripttypeid, fkcampaignid, fkcampaigntypeid, fkcampaigngroupid, fkcampaigntypegroupid, fkcampaigngrouptypeid, fklanguageid, description, document, date, isactive), null);
                if (reader.Read())
                {
                    scripts = new Scripts((long)reader["ID"]);
                    scripts.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for Scripts objects in the database", ex);
            }
            return scripts;
        }
        #endregion
    }
}
