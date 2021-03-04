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
    /// Contains methods to fill, save and delete possiblebumpupallocation objects.
    /// </summary>
    public partial class PossibleBumpUpAllocationMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) possiblebumpupallocation object from the database.
        /// </summary>
        /// <param name="possiblebumpupallocation">The id of the possiblebumpupallocation object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the possiblebumpupallocation object.</param>
        /// <returns>True if the possiblebumpupallocation object was deleted successfully, else false.</returns>
        internal static bool Delete(PossibleBumpUpAllocation possiblebumpupallocation)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(possiblebumpupallocation.ConnectionName, PossibleBumpUpAllocationQueries.Delete(possiblebumpupallocation, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a PossibleBumpUpAllocation object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) possiblebumpupallocation object from the database.
        /// </summary>
        /// <param name="possiblebumpupallocation">The id of the possiblebumpupallocation object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the possiblebumpupallocation history.</param>
        /// <returns>True if the possiblebumpupallocation history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(PossibleBumpUpAllocation possiblebumpupallocation)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(possiblebumpupallocation.ConnectionName, PossibleBumpUpAllocationQueries.DeleteHistory(possiblebumpupallocation, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete PossibleBumpUpAllocation history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) possiblebumpupallocation object from the database.
        /// </summary>
        /// <param name="possiblebumpupallocation">The id of the possiblebumpupallocation object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the possiblebumpupallocation object.</param>
        /// <returns>True if the possiblebumpupallocation object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(PossibleBumpUpAllocation possiblebumpupallocation)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(possiblebumpupallocation.ConnectionName, PossibleBumpUpAllocationQueries.UnDelete(possiblebumpupallocation, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a PossibleBumpUpAllocation object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the possiblebumpupallocation object from the data reader.
        /// </summary>
        /// <param name="possiblebumpupallocation">The possiblebumpupallocation object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(PossibleBumpUpAllocation possiblebumpupallocation, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    possiblebumpupallocation.IsLoaded = true;
                    possiblebumpupallocation.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (long)reader["FKINImportID"] : (long?)null;
                    possiblebumpupallocation.FKUserID = reader["FKUserID"] != DBNull.Value ? (long)reader["FKUserID"] : (long?)null;
                    possiblebumpupallocation.CallBackDate = reader["CallBackDate"] != DBNull.Value ? (DateTime)reader["CallBackDate"] : (DateTime?)null;
                    possiblebumpupallocation.StampDate = (DateTime)reader["StampDate"];
                    possiblebumpupallocation.HasChanged = false;
                }
                else
                {
                    throw new MapperException("PossibleBumpUpAllocation does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a PossibleBumpUpAllocation object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an possiblebumpupallocation object from the database.
        /// </summary>
        /// <param name="possiblebumpupallocation">The possiblebumpupallocation to fill.</param>
        internal static void Fill(PossibleBumpUpAllocation possiblebumpupallocation)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(possiblebumpupallocation.ConnectionName, PossibleBumpUpAllocationQueries.Fill(possiblebumpupallocation, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(possiblebumpupallocation, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a PossibleBumpUpAllocation object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a possiblebumpupallocation object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(PossibleBumpUpAllocation possiblebumpupallocation)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(possiblebumpupallocation.ConnectionName, PossibleBumpUpAllocationQueries.FillData(possiblebumpupallocation, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a PossibleBumpUpAllocation object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an possiblebumpupallocation object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="possiblebumpupallocation">The possiblebumpupallocation to fill from history.</param>
        internal static void FillHistory(PossibleBumpUpAllocation possiblebumpupallocation, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(possiblebumpupallocation.ConnectionName, PossibleBumpUpAllocationQueries.FillHistory(possiblebumpupallocation, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(possiblebumpupallocation, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a PossibleBumpUpAllocation object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the possiblebumpupallocation objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static PossibleBumpUpAllocationCollection List(bool showDeleted, string connectionName)
        {
            PossibleBumpUpAllocationCollection collection = new PossibleBumpUpAllocationCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? PossibleBumpUpAllocationQueries.ListDeleted() : PossibleBumpUpAllocationQueries.List(), null);
                while (reader.Read())
                {
                    PossibleBumpUpAllocation possiblebumpupallocation = new PossibleBumpUpAllocation((long)reader["ID"]);
                    possiblebumpupallocation.ConnectionName = connectionName;
                    collection.Add(possiblebumpupallocation);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list PossibleBumpUpAllocation objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the possiblebumpupallocation objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? PossibleBumpUpAllocationQueries.ListDeleted() : PossibleBumpUpAllocationQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list PossibleBumpUpAllocation objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) possiblebumpupallocation object from the database.
        /// </summary>
        /// <param name="possiblebumpupallocation">The possiblebumpupallocation to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(PossibleBumpUpAllocation possiblebumpupallocation)
        {
            PossibleBumpUpAllocationCollection collection = new PossibleBumpUpAllocationCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(possiblebumpupallocation.ConnectionName, PossibleBumpUpAllocationQueries.ListHistory(possiblebumpupallocation, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) PossibleBumpUpAllocation in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an possiblebumpupallocation object to the database.
        /// </summary>
        /// <param name="possiblebumpupallocation">The PossibleBumpUpAllocation object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the possiblebumpupallocation was saved successfull, otherwise, false.</returns>
        internal static bool Save(PossibleBumpUpAllocation possiblebumpupallocation)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (possiblebumpupallocation.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(possiblebumpupallocation.ConnectionName, PossibleBumpUpAllocationQueries.Save(possiblebumpupallocation, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(possiblebumpupallocation.ConnectionName, PossibleBumpUpAllocationQueries.Save(possiblebumpupallocation, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            possiblebumpupallocation.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= possiblebumpupallocation.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a PossibleBumpUpAllocation object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for possiblebumpupallocation objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="callbackdate">The callbackdate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static PossibleBumpUpAllocationCollection Search(long? fkinimportid, long? fkuserid, DateTime? callbackdate, string connectionName)
        {
            PossibleBumpUpAllocationCollection collection = new PossibleBumpUpAllocationCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, PossibleBumpUpAllocationQueries.Search(fkinimportid, fkuserid, callbackdate), null);
                while (reader.Read())
                {
                    PossibleBumpUpAllocation possiblebumpupallocation = new PossibleBumpUpAllocation((long)reader["ID"]);
                    possiblebumpupallocation.ConnectionName = connectionName;
                    collection.Add(possiblebumpupallocation);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for PossibleBumpUpAllocation objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for possiblebumpupallocation objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="callbackdate">The callbackdate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinimportid, long? fkuserid, DateTime? callbackdate, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, PossibleBumpUpAllocationQueries.Search(fkinimportid, fkuserid, callbackdate), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for PossibleBumpUpAllocation objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 possiblebumpupallocation objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="callbackdate">The callbackdate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static PossibleBumpUpAllocation SearchOne(long? fkinimportid, long? fkuserid, DateTime? callbackdate, string connectionName)
        {
            PossibleBumpUpAllocation possiblebumpupallocation = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, PossibleBumpUpAllocationQueries.Search(fkinimportid, fkuserid, callbackdate), null);
                if (reader.Read())
                {
                    possiblebumpupallocation = new PossibleBumpUpAllocation((long)reader["ID"]);
                    possiblebumpupallocation.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for PossibleBumpUpAllocation objects in the database", ex);
            }
            return possiblebumpupallocation;
        }
        #endregion
    }
}
