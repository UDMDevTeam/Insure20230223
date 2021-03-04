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
    /// Contains methods to fill, save and delete importcmstandardnotes objects.
    /// </summary>
    public partial class ImportCMStandardNotesMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) importcmstandardnotes object from the database.
        /// </summary>
        /// <param name="importcmstandardnotes">The id of the importcmstandardnotes object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the importcmstandardnotes object.</param>
        /// <returns>True if the importcmstandardnotes object was deleted successfully, else false.</returns>
        internal static bool Delete(ImportCMStandardNotes importcmstandardnotes)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(importcmstandardnotes.ConnectionName, ImportCMStandardNotesQueries.Delete(importcmstandardnotes, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a ImportCMStandardNotes object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) importcmstandardnotes object from the database.
        /// </summary>
        /// <param name="importcmstandardnotes">The id of the importcmstandardnotes object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the importcmstandardnotes history.</param>
        /// <returns>True if the importcmstandardnotes history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(ImportCMStandardNotes importcmstandardnotes)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(importcmstandardnotes.ConnectionName, ImportCMStandardNotesQueries.DeleteHistory(importcmstandardnotes, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete ImportCMStandardNotes history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) importcmstandardnotes object from the database.
        /// </summary>
        /// <param name="importcmstandardnotes">The id of the importcmstandardnotes object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the importcmstandardnotes object.</param>
        /// <returns>True if the importcmstandardnotes object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(ImportCMStandardNotes importcmstandardnotes)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(importcmstandardnotes.ConnectionName, ImportCMStandardNotesQueries.UnDelete(importcmstandardnotes, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a ImportCMStandardNotes object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the importcmstandardnotes object from the data reader.
        /// </summary>
        /// <param name="importcmstandardnotes">The importcmstandardnotes object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(ImportCMStandardNotes importcmstandardnotes, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    importcmstandardnotes.IsLoaded = true;
                    importcmstandardnotes.FKCallMonitoringStandardNotesID = reader["FKCallMonitoringStandardNotesID"] != DBNull.Value ? (long)reader["FKCallMonitoringStandardNotesID"] : (long?)null;
                    importcmstandardnotes.FKImportCallMonitoringID = reader["FKImportCallMonitoringID"] != DBNull.Value ? (long)reader["FKImportCallMonitoringID"] : (long?)null;
                    importcmstandardnotes.HasChanged = false;
                }
                else
                {
                    throw new MapperException("ImportCMStandardNotes does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a ImportCMStandardNotes object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an importcmstandardnotes object from the database.
        /// </summary>
        /// <param name="importcmstandardnotes">The importcmstandardnotes to fill.</param>
        internal static void Fill(ImportCMStandardNotes importcmstandardnotes)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(importcmstandardnotes.ConnectionName, ImportCMStandardNotesQueries.Fill(importcmstandardnotes, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(importcmstandardnotes, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a ImportCMStandardNotes object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a importcmstandardnotes object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(ImportCMStandardNotes importcmstandardnotes)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(importcmstandardnotes.ConnectionName, ImportCMStandardNotesQueries.FillData(importcmstandardnotes, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a ImportCMStandardNotes object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an importcmstandardnotes object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="importcmstandardnotes">The importcmstandardnotes to fill from history.</param>
        internal static void FillHistory(ImportCMStandardNotes importcmstandardnotes, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(importcmstandardnotes.ConnectionName, ImportCMStandardNotesQueries.FillHistory(importcmstandardnotes, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(importcmstandardnotes, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a ImportCMStandardNotes object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the importcmstandardnotes objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static ImportCMStandardNotesCollection List(bool showDeleted, string connectionName)
        {
            ImportCMStandardNotesCollection collection = new ImportCMStandardNotesCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? ImportCMStandardNotesQueries.ListDeleted() : ImportCMStandardNotesQueries.List(), null);
                while (reader.Read())
                {
                    ImportCMStandardNotes importcmstandardnotes = new ImportCMStandardNotes((long)reader["ID"]);
                    importcmstandardnotes.ConnectionName = connectionName;
                    collection.Add(importcmstandardnotes);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list ImportCMStandardNotes objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the importcmstandardnotes objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? ImportCMStandardNotesQueries.ListDeleted() : ImportCMStandardNotesQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list ImportCMStandardNotes objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) importcmstandardnotes object from the database.
        /// </summary>
        /// <param name="importcmstandardnotes">The importcmstandardnotes to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(ImportCMStandardNotes importcmstandardnotes)
        {
            ImportCMStandardNotesCollection collection = new ImportCMStandardNotesCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(importcmstandardnotes.ConnectionName, ImportCMStandardNotesQueries.ListHistory(importcmstandardnotes, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) ImportCMStandardNotes in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an importcmstandardnotes object to the database.
        /// </summary>
        /// <param name="importcmstandardnotes">The ImportCMStandardNotes object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the importcmstandardnotes was saved successfull, otherwise, false.</returns>
        internal static bool Save(ImportCMStandardNotes importcmstandardnotes)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (importcmstandardnotes.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(importcmstandardnotes.ConnectionName, ImportCMStandardNotesQueries.Save(importcmstandardnotes, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(importcmstandardnotes.ConnectionName, ImportCMStandardNotesQueries.Save(importcmstandardnotes, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            importcmstandardnotes.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= importcmstandardnotes.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a ImportCMStandardNotes object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for importcmstandardnotes objects in the database.
        /// </summary>
        /// <param name="fkcallmonitoringstandardnotesid">The fkcallmonitoringstandardnotesid search criteria.</param>
        /// <param name="fkimportcallmonitoringid">The fkimportcallmonitoringid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static ImportCMStandardNotesCollection Search(long? fkcallmonitoringstandardnotesid, long? fkimportcallmonitoringid, string connectionName)
        {
            ImportCMStandardNotesCollection collection = new ImportCMStandardNotesCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, ImportCMStandardNotesQueries.Search(fkcallmonitoringstandardnotesid, fkimportcallmonitoringid), null);
                while (reader.Read())
                {
                    ImportCMStandardNotes importcmstandardnotes = new ImportCMStandardNotes((long)reader["ID"]);
                    importcmstandardnotes.ConnectionName = connectionName;
                    collection.Add(importcmstandardnotes);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for ImportCMStandardNotes objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for importcmstandardnotes objects in the database.
        /// </summary>
        /// <param name="fkcallmonitoringstandardnotesid">The fkcallmonitoringstandardnotesid search criteria.</param>
        /// <param name="fkimportcallmonitoringid">The fkimportcallmonitoringid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkcallmonitoringstandardnotesid, long? fkimportcallmonitoringid, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, ImportCMStandardNotesQueries.Search(fkcallmonitoringstandardnotesid, fkimportcallmonitoringid), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for ImportCMStandardNotes objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 importcmstandardnotes objects in the database.
        /// </summary>
        /// <param name="fkcallmonitoringstandardnotesid">The fkcallmonitoringstandardnotesid search criteria.</param>
        /// <param name="fkimportcallmonitoringid">The fkimportcallmonitoringid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static ImportCMStandardNotes SearchOne(long? fkcallmonitoringstandardnotesid, long? fkimportcallmonitoringid, string connectionName)
        {
            ImportCMStandardNotes importcmstandardnotes = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, ImportCMStandardNotesQueries.Search(fkcallmonitoringstandardnotesid, fkimportcallmonitoringid), null);
                if (reader.Read())
                {
                    importcmstandardnotes = new ImportCMStandardNotes((long)reader["ID"]);
                    importcmstandardnotes.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for ImportCMStandardNotes objects in the database", ex);
            }
            return importcmstandardnotes;
        }
        #endregion
    }
}
