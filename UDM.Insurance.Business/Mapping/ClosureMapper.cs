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
    /// Contains methods to fill, save and delete closure objects.
    /// </summary>
    public partial class ClosureMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) closure object from the database.
        /// </summary>
        /// <param name="closure">The id of the closure object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the closure object.</param>
        /// <returns>True if the closure object was deleted successfully, else false.</returns>
        internal static bool Delete(Closure closure)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(closure.ConnectionName, ClosureQueries.Delete(closure, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a Closure object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) closure object from the database.
        /// </summary>
        /// <param name="closure">The id of the closure object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the closure history.</param>
        /// <returns>True if the closure history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(Closure closure)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(closure.ConnectionName, ClosureQueries.DeleteHistory(closure, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete Closure history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) closure object from the database.
        /// </summary>
        /// <param name="closure">The id of the closure object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the closure object.</param>
        /// <returns>True if the closure object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(Closure closure)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(closure.ConnectionName, ClosureQueries.UnDelete(closure, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a Closure object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the closure object from the data reader.
        /// </summary>
        /// <param name="closure">The closure object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(Closure closure, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    closure.IsLoaded = true;
                    closure.FKSystemID = reader["FKSystemID"] != DBNull.Value ? (long)reader["FKSystemID"] : (long?)null;
                    closure.FKCampaignID = reader["FKCampaignID"] != DBNull.Value ? (long)reader["FKCampaignID"] : (long?)null;
                    closure.FKLanguageID = reader["FKLanguageID"] != DBNull.Value ? (long)reader["FKLanguageID"] : (long?)null;
                    closure.Document = reader["Document"] != DBNull.Value ? (byte[])reader["Document"] : (byte[])null;
                    closure.IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : (bool?)null;
                    closure.StampDate = (DateTime)reader["StampDate"];
                    closure.HasChanged = false;
                }
                else
                {
                    throw new MapperException("Closure does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Closure object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an closure object from the database.
        /// </summary>
        /// <param name="closure">The closure to fill.</param>
        internal static void Fill(Closure closure)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(closure.ConnectionName, ClosureQueries.Fill(closure, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(closure, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Closure object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a closure object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(Closure closure)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(closure.ConnectionName, ClosureQueries.FillData(closure, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a Closure object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an closure object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="closure">The closure to fill from history.</param>
        internal static void FillHistory(Closure closure, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(closure.ConnectionName, ClosureQueries.FillHistory(closure, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(closure, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Closure object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the closure objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static ClosureCollection List(bool showDeleted, string connectionName)
        {
            ClosureCollection collection = new ClosureCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? ClosureQueries.ListDeleted() : ClosureQueries.List(), null);
                while (reader.Read())
                {
                    Closure closure = new Closure((long)reader["ID"]);
                    closure.ConnectionName = connectionName;
                    collection.Add(closure);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list Closure objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the closure objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? ClosureQueries.ListDeleted() : ClosureQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list Closure objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) closure object from the database.
        /// </summary>
        /// <param name="closure">The closure to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(Closure closure)
        {
            ClosureCollection collection = new ClosureCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(closure.ConnectionName, ClosureQueries.ListHistory(closure, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) Closure in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an closure object to the database.
        /// </summary>
        /// <param name="closure">The Closure object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the closure was saved successfull, otherwise, false.</returns>
        internal static bool Save(Closure closure)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (closure.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(closure.ConnectionName, ClosureQueries.Save(closure, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(closure.ConnectionName, ClosureQueries.Save(closure, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            closure.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= closure.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a Closure object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for closure objects in the database.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="fklanguageid">The fklanguageid search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static ClosureCollection Search(long? fksystemid, long? fkcampaignid, long? fklanguageid, byte[] document, bool? isactive, string connectionName)
        {
            ClosureCollection collection = new ClosureCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, ClosureQueries.Search(fksystemid, fkcampaignid, fklanguageid, document, isactive), null);
                while (reader.Read())
                {
                    Closure closure = new Closure((long)reader["ID"]);
                    closure.ConnectionName = connectionName;
                    collection.Add(closure);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for Closure objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for closure objects in the database.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="fklanguageid">The fklanguageid search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fksystemid, long? fkcampaignid, long? fklanguageid, byte[] document, bool? isactive, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, ClosureQueries.Search(fksystemid, fkcampaignid, fklanguageid, document, isactive), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for Closure objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 closure objects in the database.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="fklanguageid">The fklanguageid search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static Closure SearchOne(long? fksystemid, long? fkcampaignid, long? fklanguageid, byte[] document, bool? isactive, string connectionName)
        {
            Closure closure = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, ClosureQueries.Search(fksystemid, fkcampaignid, fklanguageid, document, isactive), null);
                if (reader.Read())
                {
                    closure = new Closure((long)reader["ID"]);
                    closure.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for Closure objects in the database", ex);
            }
            return closure;
        }
        #endregion
    }
}
