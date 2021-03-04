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
    /// Contains methods to fill, save and delete incampaigntypeset objects.
    /// </summary>
    public partial class INCampaignTypeSetMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) incampaigntypeset object from the database.
        /// </summary>
        /// <param name="incampaigntypeset">The id of the incampaigntypeset object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the incampaigntypeset object.</param>
        /// <returns>True if the incampaigntypeset object was deleted successfully, else false.</returns>
        internal static bool Delete(INCampaignTypeSet incampaigntypeset)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(incampaigntypeset.ConnectionName, INCampaignTypeSetQueries.Delete(incampaigntypeset, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INCampaignTypeSet object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) incampaigntypeset object from the database.
        /// </summary>
        /// <param name="incampaigntypeset">The id of the incampaigntypeset object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the incampaigntypeset history.</param>
        /// <returns>True if the incampaigntypeset history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INCampaignTypeSet incampaigntypeset)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(incampaigntypeset.ConnectionName, INCampaignTypeSetQueries.DeleteHistory(incampaigntypeset, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INCampaignTypeSet history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) incampaigntypeset object from the database.
        /// </summary>
        /// <param name="incampaigntypeset">The id of the incampaigntypeset object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the incampaigntypeset object.</param>
        /// <returns>True if the incampaigntypeset object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INCampaignTypeSet incampaigntypeset)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(incampaigntypeset.ConnectionName, INCampaignTypeSetQueries.UnDelete(incampaigntypeset, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INCampaignTypeSet object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the incampaigntypeset object from the data reader.
        /// </summary>
        /// <param name="incampaigntypeset">The incampaigntypeset object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INCampaignTypeSet incampaigntypeset, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    incampaigntypeset.IsLoaded = true;
                    incampaigntypeset.FKCampaignTypeID = reader["FKCampaignTypeID"] != DBNull.Value ? (long)reader["FKCampaignTypeID"] : (long?)null;
                    incampaigntypeset.FKCampaignTypeGroupID = reader["FKCampaignTypeGroupID"] != DBNull.Value ? (long)reader["FKCampaignTypeGroupID"] : (long?)null;
                    incampaigntypeset.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INCampaignTypeSet does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INCampaignTypeSet object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an incampaigntypeset object from the database.
        /// </summary>
        /// <param name="incampaigntypeset">The incampaigntypeset to fill.</param>
        internal static void Fill(INCampaignTypeSet incampaigntypeset)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(incampaigntypeset.ConnectionName, INCampaignTypeSetQueries.Fill(incampaigntypeset, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(incampaigntypeset, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INCampaignTypeSet object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a incampaigntypeset object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INCampaignTypeSet incampaigntypeset)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(incampaigntypeset.ConnectionName, INCampaignTypeSetQueries.FillData(incampaigntypeset, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INCampaignTypeSet object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an incampaigntypeset object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="incampaigntypeset">The incampaigntypeset to fill from history.</param>
        internal static void FillHistory(INCampaignTypeSet incampaigntypeset, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(incampaigntypeset.ConnectionName, INCampaignTypeSetQueries.FillHistory(incampaigntypeset, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(incampaigntypeset, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INCampaignTypeSet object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the incampaigntypeset objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INCampaignTypeSetCollection List(bool showDeleted, string connectionName)
        {
            INCampaignTypeSetCollection collection = new INCampaignTypeSetCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INCampaignTypeSetQueries.ListDeleted() : INCampaignTypeSetQueries.List(), null);
                while (reader.Read())
                {
                    INCampaignTypeSet incampaigntypeset = new INCampaignTypeSet((long)reader["ID"]);
                    incampaigntypeset.ConnectionName = connectionName;
                    collection.Add(incampaigntypeset);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INCampaignTypeSet objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the incampaigntypeset objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INCampaignTypeSetQueries.ListDeleted() : INCampaignTypeSetQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INCampaignTypeSet objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) incampaigntypeset object from the database.
        /// </summary>
        /// <param name="incampaigntypeset">The incampaigntypeset to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INCampaignTypeSet incampaigntypeset)
        {
            INCampaignTypeSetCollection collection = new INCampaignTypeSetCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(incampaigntypeset.ConnectionName, INCampaignTypeSetQueries.ListHistory(incampaigntypeset, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INCampaignTypeSet in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an incampaigntypeset object to the database.
        /// </summary>
        /// <param name="incampaigntypeset">The INCampaignTypeSet object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the incampaigntypeset was saved successfull, otherwise, false.</returns>
        internal static bool Save(INCampaignTypeSet incampaigntypeset)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (incampaigntypeset.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(incampaigntypeset.ConnectionName, INCampaignTypeSetQueries.Save(incampaigntypeset, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(incampaigntypeset.ConnectionName, INCampaignTypeSetQueries.Save(incampaigntypeset, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            incampaigntypeset.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= incampaigntypeset.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INCampaignTypeSet object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for incampaigntypeset objects in the database.
        /// </summary>
        /// <param name="fkcampaigntypeid">The fkcampaigntypeid search criteria.</param>
        /// <param name="fkcampaigntypegroupid">The fkcampaigntypegroupid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INCampaignTypeSetCollection Search(long? fkcampaigntypeid, long? fkcampaigntypegroupid, string connectionName)
        {
            INCampaignTypeSetCollection collection = new INCampaignTypeSetCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INCampaignTypeSetQueries.Search(fkcampaigntypeid, fkcampaigntypegroupid), null);
                while (reader.Read())
                {
                    INCampaignTypeSet incampaigntypeset = new INCampaignTypeSet((long)reader["ID"]);
                    incampaigntypeset.ConnectionName = connectionName;
                    collection.Add(incampaigntypeset);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INCampaignTypeSet objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for incampaigntypeset objects in the database.
        /// </summary>
        /// <param name="fkcampaigntypeid">The fkcampaigntypeid search criteria.</param>
        /// <param name="fkcampaigntypegroupid">The fkcampaigntypegroupid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkcampaigntypeid, long? fkcampaigntypegroupid, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INCampaignTypeSetQueries.Search(fkcampaigntypeid, fkcampaigntypegroupid), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INCampaignTypeSet objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 incampaigntypeset objects in the database.
        /// </summary>
        /// <param name="fkcampaigntypeid">The fkcampaigntypeid search criteria.</param>
        /// <param name="fkcampaigntypegroupid">The fkcampaigntypegroupid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INCampaignTypeSet SearchOne(long? fkcampaigntypeid, long? fkcampaigntypegroupid, string connectionName)
        {
            INCampaignTypeSet incampaigntypeset = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INCampaignTypeSetQueries.Search(fkcampaigntypeid, fkcampaigntypegroupid), null);
                if (reader.Read())
                {
                    incampaigntypeset = new INCampaignTypeSet((long)reader["ID"]);
                    incampaigntypeset.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INCampaignTypeSet objects in the database", ex);
            }
            return incampaigntypeset;
        }
        #endregion
    }
}
