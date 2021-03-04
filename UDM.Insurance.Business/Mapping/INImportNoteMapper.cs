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
    /// Contains methods to fill, save and delete inimportnote objects.
    /// </summary>
    public partial class INImportNoteMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inimportnote object from the database.
        /// </summary>
        /// <param name="inimportnote">The id of the inimportnote object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inimportnote object.</param>
        /// <returns>True if the inimportnote object was deleted successfully, else false.</returns>
        internal static bool Delete(INImportNote inimportnote)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportnote.ConnectionName, INImportNoteQueries.Delete(inimportnote, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INImportNote object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inimportnote object from the database.
        /// </summary>
        /// <param name="inimportnote">The id of the inimportnote object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inimportnote history.</param>
        /// <returns>True if the inimportnote history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INImportNote inimportnote)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportnote.ConnectionName, INImportNoteQueries.DeleteHistory(inimportnote, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INImportNote history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inimportnote object from the database.
        /// </summary>
        /// <param name="inimportnote">The id of the inimportnote object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inimportnote object.</param>
        /// <returns>True if the inimportnote object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INImportNote inimportnote)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportnote.ConnectionName, INImportNoteQueries.UnDelete(inimportnote, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INImportNote object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inimportnote object from the data reader.
        /// </summary>
        /// <param name="inimportnote">The inimportnote object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INImportNote inimportnote, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inimportnote.IsLoaded = true;
                    inimportnote.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (long)reader["FKINImportID"] : (long?)null;
                    inimportnote.FKUserID = reader["FKUserID"] != DBNull.Value ? (long)reader["FKUserID"] : (long?)null;
                    inimportnote.NoteDate = reader["NoteDate"] != DBNull.Value ? (DateTime)reader["NoteDate"] : (DateTime?)null;
                    inimportnote.Note = reader["Note"] != DBNull.Value ? (string)reader["Note"] : (string)null;
                    inimportnote.Sequence = reader["Sequence"] != DBNull.Value ? (int)reader["Sequence"] : (int?)null;
                    inimportnote.StampDate = (DateTime)reader["StampDate"];
                    inimportnote.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INImportNote does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportNote object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportnote object from the database.
        /// </summary>
        /// <param name="inimportnote">The inimportnote to fill.</param>
        internal static void Fill(INImportNote inimportnote)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportnote.ConnectionName, INImportNoteQueries.Fill(inimportnote, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportnote, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportNote object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inimportnote object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INImportNote inimportnote)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportnote.ConnectionName, INImportNoteQueries.FillData(inimportnote, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INImportNote object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportnote object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inimportnote">The inimportnote to fill from history.</param>
        internal static void FillHistory(INImportNote inimportnote, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportnote.ConnectionName, INImportNoteQueries.FillHistory(inimportnote, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportnote, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportNote object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportnote objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INImportNoteCollection List(bool showDeleted, string connectionName)
        {
            INImportNoteCollection collection = new INImportNoteCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INImportNoteQueries.ListDeleted() : INImportNoteQueries.List(), null);
                while (reader.Read())
                {
                    INImportNote inimportnote = new INImportNote((long)reader["ID"]);
                    inimportnote.ConnectionName = connectionName;
                    collection.Add(inimportnote);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportNote objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inimportnote objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INImportNoteQueries.ListDeleted() : INImportNoteQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportNote objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inimportnote object from the database.
        /// </summary>
        /// <param name="inimportnote">The inimportnote to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INImportNote inimportnote)
        {
            INImportNoteCollection collection = new INImportNoteCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportnote.ConnectionName, INImportNoteQueries.ListHistory(inimportnote, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INImportNote in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inimportnote object to the database.
        /// </summary>
        /// <param name="inimportnote">The INImportNote object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inimportnote was saved successfull, otherwise, false.</returns>
        internal static bool Save(INImportNote inimportnote)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inimportnote.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inimportnote.ConnectionName, INImportNoteQueries.Save(inimportnote, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inimportnote.ConnectionName, INImportNoteQueries.Save(inimportnote, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inimportnote.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inimportnote.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INImportNote object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportnote objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="notedate">The notedate search criteria.</param>
        /// <param name="note">The note search criteria.</param>
        /// <param name="sequence">The sequence search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportNoteCollection Search(long? fkinimportid, long? fkuserid, DateTime? notedate, string note, int? sequence, string connectionName)
        {
            INImportNoteCollection collection = new INImportNoteCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportNoteQueries.Search(fkinimportid, fkuserid, notedate, note, sequence), null);
                while (reader.Read())
                {
                    INImportNote inimportnote = new INImportNote((long)reader["ID"]);
                    inimportnote.ConnectionName = connectionName;
                    collection.Add(inimportnote);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportNote objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inimportnote objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="notedate">The notedate search criteria.</param>
        /// <param name="note">The note search criteria.</param>
        /// <param name="sequence">The sequence search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinimportid, long? fkuserid, DateTime? notedate, string note, int? sequence, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INImportNoteQueries.Search(fkinimportid, fkuserid, notedate, note, sequence), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportNote objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inimportnote objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="notedate">The notedate search criteria.</param>
        /// <param name="note">The note search criteria.</param>
        /// <param name="sequence">The sequence search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportNote SearchOne(long? fkinimportid, long? fkuserid, DateTime? notedate, string note, int? sequence, string connectionName)
        {
            INImportNote inimportnote = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportNoteQueries.Search(fkinimportid, fkuserid, notedate, note, sequence), null);
                if (reader.Read())
                {
                    inimportnote = new INImportNote((long)reader["ID"]);
                    inimportnote.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportNote objects in the database", ex);
            }
            return inimportnote;
        }
        #endregion
    }
}
