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
    public partial class INPermissionLeadMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inimportnote object from the database.
        /// </summary>
        /// <param name="inimportnote">The id of the inimportnote object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inimportnote object.</param>
        /// <returns>True if the inimportnote object was deleted successfully, else false.</returns>
        internal static bool Delete(INPermissionLead inpermissionlead)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpermissionlead.ConnectionName, INPermissionLeadQueries.Delete(inpermissionlead, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INPermissionLead object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inimportnote object from the database.
        /// </summary>
        /// <param name="inimportnote">The id of the inimportnote object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inimportnote history.</param>
        /// <returns>True if the inimportnote history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INPermissionLead inpermissionlead)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpermissionlead.ConnectionName, INPermissionLeadQueries.DeleteHistory(inpermissionlead, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INPermissionLead history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inimportnote object from the database.
        /// </summary>
        /// <param name="inimportnote">The id of the inimportnote object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inimportnote object.</param>
        /// <returns>True if the inimportnote object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INPermissionLead inpermissionlead)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpermissionlead.ConnectionName, INPermissionLeadQueries.UnDelete(inpermissionlead, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INPermissionLead object from the database", ex);
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
        private static void Fill(INPermissionLead inpermissionlead, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inpermissionlead.IsLoaded = true;
                    inpermissionlead.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (long)reader["FKINImportID"] : (long?)null;
                    inpermissionlead.Title = reader["Title"] != DBNull.Value ? (string)reader["Title"] : (string)null;
                    inpermissionlead.Firstname = reader["Firstname"] != DBNull.Value ? (string)reader["Firstname"] : (string)null;
                    inpermissionlead.Surname = reader["Surname"] != DBNull.Value ? (string)reader["Surname"] : (string)null;
                    inpermissionlead.Cellnumber = reader["CellNumber"] != DBNull.Value ? (string)reader["CellNumber"] : (string)null;
                    inpermissionlead.AltNumber = reader["AltNumber"] != DBNull.Value ? (string)reader["AltNumber"] : (string)null;
                    inpermissionlead.SavedBy = reader["SavedBy"] != DBNull.Value ? (string)reader["SavedBy"] : (string)null;
                    inpermissionlead.DateSaved = reader["DateSaved"] != DBNull.Value ? (DateTime?)reader["DateSaved"] : (DateTime?)null;
                    inpermissionlead.DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime?)reader["DateOfBirth"] : (DateTime?)null;
                    inpermissionlead.StampDate = (DateTime)reader["StampDate"];
                    inpermissionlead.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INPermissionLead does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPermissionLead object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportnote object from the database.
        /// </summary>
        /// <param name="inimportnote">The inimportnote to fill.</param>
        internal static void Fill(INPermissionLead inpermissionlead)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inpermissionlead.ConnectionName, INPermissionLeadQueries.Fill(inpermissionlead, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inpermissionlead, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPermissionLead object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inimportnote object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INPermissionLead inpermissionlead)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inpermissionlead.ConnectionName, INPermissionLeadQueries.FillData(inpermissionlead, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INPermissionLead object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportnote object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inimportnote">The inimportnote to fill from history.</param>
        internal static void FillHistory(INPermissionLead inpermissionlead, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inpermissionlead.ConnectionName, INPermissionLeadQueries.FillHistory(inpermissionlead, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inpermissionlead, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPermissionLead object from the database", ex);
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
        public static INPermissionLeadCollection List(bool showDeleted, string connectionName)
        {
            INPermissionLeadCollection collection = new INPermissionLeadCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INPermissionLeadQueries.ListDeleted() : INPermissionLeadQueries.List(), null);
                while (reader.Read())
                {
                    INPermissionLead inimportnote = new INPermissionLead((long)reader["ID"]);
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
                return Database.ExecuteDataSet(connectionName, showDeleted ? INPermissionLeadQueries.ListDeleted() : INPermissionLeadQueries.List(), null);
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
        public static DataSet ListHistory(INPermissionLead inpermissionlead)
        {
            INImportNoteCollection collection = new INImportNoteCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inpermissionlead.ConnectionName, INPermissionLeadQueries.ListHistory(inpermissionlead, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INPermissionLead in the database", ex);
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
        internal static bool Save(INPermissionLead inpermissionlead)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inpermissionlead.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inpermissionlead.ConnectionName, INPermissionLeadQueries.Save(inpermissionlead, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inpermissionlead.ConnectionName, INPermissionLeadQueries.Save(inpermissionlead, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inpermissionlead.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inpermissionlead.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INPermissionLead object to the database", ex);
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
        public static INPermissionLeadCollection Search(long? fkinimportid, string title, string firstname, string surname, string cellnumber, string altnumber, string savedby, DateTime? datesaved, DateTime? dateofbirth, string connectionName)
        {
            INPermissionLeadCollection collection = new INPermissionLeadCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPermissionLeadQueries.Search(fkinimportid, title, firstname, surname, cellnumber, altnumber, savedby, datesaved, dateofbirth), null);
                while (reader.Read())
                {
                    INPermissionLead inpermissionlead = new INPermissionLead((long)reader["ID"]);
                    inpermissionlead.ConnectionName = connectionName;
                    collection.Add(inpermissionlead);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPermissionLead objects in the database", ex);
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
        public static DataSet SearchData(long? fkinimportid, string title, string firstname, string surname, string cellnumber, string altnumber, string savedby, DateTime? datesaved, DateTime? dateofbirth, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INPermissionLeadQueries.Search(fkinimportid, title, firstname, surname, cellnumber, altnumber, savedby,datesaved, dateofbirth), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPermissionLead objects in the database", ex);
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
        public static INPermissionLead SearchOne(long? fkinimportid, string title, string firstname, string surname, string cellnumber, string altnumber, string savedby, DateTime? datesaved, DateTime? dateofbirth,  string connectionName)
        {
            INPermissionLead inpermissionlead = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPermissionLeadQueries.Search(fkinimportid, title, firstname, surname, cellnumber, altnumber, savedby, datesaved, dateofbirth), null);
                if (reader.Read())
                {
                    inpermissionlead = new INPermissionLead((long)reader["ID"]);
                    inpermissionlead.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for inpermissionlead objects in the database", ex);
            }
            return inpermissionlead;
        }
        #endregion
    }
}
