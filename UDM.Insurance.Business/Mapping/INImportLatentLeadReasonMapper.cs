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
    /// Contains methods to fill, save and delete inimportlatentleadreason objects.
    /// </summary>
    public partial class INImportLatentLeadReasonMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inimportlatentleadreason object from the database.
        /// </summary>
        /// <param name="inimportlatentleadreason">The id of the inimportlatentleadreason object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inimportlatentleadreason object.</param>
        /// <returns>True if the inimportlatentleadreason object was deleted successfully, else false.</returns>
        internal static bool Delete(INImportLatentLeadReason inimportlatentleadreason)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportlatentleadreason.ConnectionName, INImportLatentLeadReasonQueries.Delete(inimportlatentleadreason, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INImportLatentLeadReason object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inimportlatentleadreason object from the database.
        /// </summary>
        /// <param name="inimportlatentleadreason">The id of the inimportlatentleadreason object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inimportlatentleadreason history.</param>
        /// <returns>True if the inimportlatentleadreason history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INImportLatentLeadReason inimportlatentleadreason)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportlatentleadreason.ConnectionName, INImportLatentLeadReasonQueries.DeleteHistory(inimportlatentleadreason, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INImportLatentLeadReason history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inimportlatentleadreason object from the database.
        /// </summary>
        /// <param name="inimportlatentleadreason">The id of the inimportlatentleadreason object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inimportlatentleadreason object.</param>
        /// <returns>True if the inimportlatentleadreason object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INImportLatentLeadReason inimportlatentleadreason)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportlatentleadreason.ConnectionName, INImportLatentLeadReasonQueries.UnDelete(inimportlatentleadreason, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INImportLatentLeadReason object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inimportlatentleadreason object from the data reader.
        /// </summary>
        /// <param name="inimportlatentleadreason">The inimportlatentleadreason object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INImportLatentLeadReason inimportlatentleadreason, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inimportlatentleadreason.IsLoaded = true;
                    inimportlatentleadreason.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (long)reader["FKINImportID"] : (long?)null;
                    inimportlatentleadreason.FKINLatentLeadReasonID1 = reader["FKINLatentLeadReasonID1"] != DBNull.Value ? (long)reader["FKINLatentLeadReasonID1"] : (long?)null;
                    inimportlatentleadreason.FKINLatentLeadReasonID2 = reader["FKINLatentLeadReasonID2"] != DBNull.Value ? (long)reader["FKINLatentLeadReasonID2"] : (long?)null;
                    inimportlatentleadreason.FKINLatentLeadReasonID3 = reader["FKINLatentLeadReasonID3"] != DBNull.Value ? (long)reader["FKINLatentLeadReasonID3"] : (long?)null;
                    inimportlatentleadreason.StampDate = (DateTime)reader["StampDate"];
                    inimportlatentleadreason.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INImportLatentLeadReason does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportLatentLeadReason object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportlatentleadreason object from the database.
        /// </summary>
        /// <param name="inimportlatentleadreason">The inimportlatentleadreason to fill.</param>
        internal static void Fill(INImportLatentLeadReason inimportlatentleadreason)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportlatentleadreason.ConnectionName, INImportLatentLeadReasonQueries.Fill(inimportlatentleadreason, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportlatentleadreason, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportLatentLeadReason object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inimportlatentleadreason object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INImportLatentLeadReason inimportlatentleadreason)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportlatentleadreason.ConnectionName, INImportLatentLeadReasonQueries.FillData(inimportlatentleadreason, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INImportLatentLeadReason object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportlatentleadreason object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inimportlatentleadreason">The inimportlatentleadreason to fill from history.</param>
        internal static void FillHistory(INImportLatentLeadReason inimportlatentleadreason, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportlatentleadreason.ConnectionName, INImportLatentLeadReasonQueries.FillHistory(inimportlatentleadreason, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportlatentleadreason, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportLatentLeadReason object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportlatentleadreason objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INImportLatentLeadReasonCollection List(bool showDeleted, string connectionName)
        {
            INImportLatentLeadReasonCollection collection = new INImportLatentLeadReasonCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INImportLatentLeadReasonQueries.ListDeleted() : INImportLatentLeadReasonQueries.List(), null);
                while (reader.Read())
                {
                    INImportLatentLeadReason inimportlatentleadreason = new INImportLatentLeadReason((long)reader["ID"]);
                    inimportlatentleadreason.ConnectionName = connectionName;
                    collection.Add(inimportlatentleadreason);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportLatentLeadReason objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inimportlatentleadreason objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INImportLatentLeadReasonQueries.ListDeleted() : INImportLatentLeadReasonQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportLatentLeadReason objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inimportlatentleadreason object from the database.
        /// </summary>
        /// <param name="inimportlatentleadreason">The inimportlatentleadreason to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INImportLatentLeadReason inimportlatentleadreason)
        {
            INImportLatentLeadReasonCollection collection = new INImportLatentLeadReasonCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportlatentleadreason.ConnectionName, INImportLatentLeadReasonQueries.ListHistory(inimportlatentleadreason, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INImportLatentLeadReason in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inimportlatentleadreason object to the database.
        /// </summary>
        /// <param name="inimportlatentleadreason">The INImportLatentLeadReason object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inimportlatentleadreason was saved successfull, otherwise, false.</returns>
        internal static bool Save(INImportLatentLeadReason inimportlatentleadreason)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inimportlatentleadreason.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inimportlatentleadreason.ConnectionName, INImportLatentLeadReasonQueries.Save(inimportlatentleadreason, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inimportlatentleadreason.ConnectionName, INImportLatentLeadReasonQueries.Save(inimportlatentleadreason, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inimportlatentleadreason.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inimportlatentleadreason.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INImportLatentLeadReason object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportlatentleadreason objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkinlatentleadreasonid1">The fkinlatentleadreasonid1 search criteria.</param>
        /// <param name="fkinlatentleadreasonid2">The fkinlatentleadreasonid2 search criteria.</param>
        /// <param name="fkinlatentleadreasonid3">The fkinlatentleadreasonid3 search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportLatentLeadReasonCollection Search(long? fkinimportid, long? fkinlatentleadreasonid1, long? fkinlatentleadreasonid2, long? fkinlatentleadreasonid3, string connectionName)
        {
            INImportLatentLeadReasonCollection collection = new INImportLatentLeadReasonCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportLatentLeadReasonQueries.Search(fkinimportid, fkinlatentleadreasonid1, fkinlatentleadreasonid2, fkinlatentleadreasonid3), null);
                while (reader.Read())
                {
                    INImportLatentLeadReason inimportlatentleadreason = new INImportLatentLeadReason((long)reader["ID"]);
                    inimportlatentleadreason.ConnectionName = connectionName;
                    collection.Add(inimportlatentleadreason);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportLatentLeadReason objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inimportlatentleadreason objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkinlatentleadreasonid1">The fkinlatentleadreasonid1 search criteria.</param>
        /// <param name="fkinlatentleadreasonid2">The fkinlatentleadreasonid2 search criteria.</param>
        /// <param name="fkinlatentleadreasonid3">The fkinlatentleadreasonid3 search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinimportid, long? fkinlatentleadreasonid1, long? fkinlatentleadreasonid2, long? fkinlatentleadreasonid3, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INImportLatentLeadReasonQueries.Search(fkinimportid, fkinlatentleadreasonid1, fkinlatentleadreasonid2, fkinlatentleadreasonid3), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportLatentLeadReason objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inimportlatentleadreason objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkinlatentleadreasonid1">The fkinlatentleadreasonid1 search criteria.</param>
        /// <param name="fkinlatentleadreasonid2">The fkinlatentleadreasonid2 search criteria.</param>
        /// <param name="fkinlatentleadreasonid3">The fkinlatentleadreasonid3 search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportLatentLeadReason SearchOne(long? fkinimportid, long? fkinlatentleadreasonid1, long? fkinlatentleadreasonid2, long? fkinlatentleadreasonid3, string connectionName)
        {
            INImportLatentLeadReason inimportlatentleadreason = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportLatentLeadReasonQueries.Search(fkinimportid, fkinlatentleadreasonid1, fkinlatentleadreasonid2, fkinlatentleadreasonid3), null);
                if (reader.Read())
                {
                    inimportlatentleadreason = new INImportLatentLeadReason((long)reader["ID"]);
                    inimportlatentleadreason.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportLatentLeadReason objects in the database", ex);
            }
            return inimportlatentleadreason;
        }
        #endregion
    }
}
