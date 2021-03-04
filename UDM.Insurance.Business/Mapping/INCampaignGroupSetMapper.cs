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
    /// Contains methods to fill, save and delete incampaigngroupset objects.
    /// </summary>
    public partial class INCampaignGroupSetMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) incampaigngroupset object from the database.
        /// </summary>
        /// <param name="incampaigngroupset">The id of the incampaigngroupset object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the incampaigngroupset object.</param>
        /// <returns>True if the incampaigngroupset object was deleted successfully, else false.</returns>
        internal static bool Delete(INCampaignGroupSet incampaigngroupset)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(incampaigngroupset.ConnectionName, INCampaignGroupSetQueries.Delete(incampaigngroupset, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INCampaignGroupSet object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) incampaigngroupset object from the database.
        /// </summary>
        /// <param name="incampaigngroupset">The id of the incampaigngroupset object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the incampaigngroupset history.</param>
        /// <returns>True if the incampaigngroupset history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INCampaignGroupSet incampaigngroupset)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(incampaigngroupset.ConnectionName, INCampaignGroupSetQueries.DeleteHistory(incampaigngroupset, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INCampaignGroupSet history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) incampaigngroupset object from the database.
        /// </summary>
        /// <param name="incampaigngroupset">The id of the incampaigngroupset object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the incampaigngroupset object.</param>
        /// <returns>True if the incampaigngroupset object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INCampaignGroupSet incampaigngroupset)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(incampaigngroupset.ConnectionName, INCampaignGroupSetQueries.UnDelete(incampaigngroupset, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INCampaignGroupSet object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the incampaigngroupset object from the data reader.
        /// </summary>
        /// <param name="incampaigngroupset">The incampaigngroupset object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INCampaignGroupSet incampaigngroupset, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    incampaigngroupset.IsLoaded = true;
                    incampaigngroupset.FKlkpINCampaignGroup = reader["FKlkpINCampaignGroup"] != DBNull.Value ? (long)reader["FKlkpINCampaignGroup"] : (long?)null;
                    incampaigngroupset.FKlkpINCampaignGroupType = reader["FKlkpINCampaignGroupType"] != DBNull.Value ? (long)reader["FKlkpINCampaignGroupType"] : (long?)null;
                    incampaigngroupset.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INCampaignGroupSet does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INCampaignGroupSet object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an incampaigngroupset object from the database.
        /// </summary>
        /// <param name="incampaigngroupset">The incampaigngroupset to fill.</param>
        internal static void Fill(INCampaignGroupSet incampaigngroupset)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(incampaigngroupset.ConnectionName, INCampaignGroupSetQueries.Fill(incampaigngroupset, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(incampaigngroupset, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INCampaignGroupSet object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a incampaigngroupset object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INCampaignGroupSet incampaigngroupset)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(incampaigngroupset.ConnectionName, INCampaignGroupSetQueries.FillData(incampaigngroupset, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INCampaignGroupSet object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an incampaigngroupset object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="incampaigngroupset">The incampaigngroupset to fill from history.</param>
        internal static void FillHistory(INCampaignGroupSet incampaigngroupset, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(incampaigngroupset.ConnectionName, INCampaignGroupSetQueries.FillHistory(incampaigngroupset, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(incampaigngroupset, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INCampaignGroupSet object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the incampaigngroupset objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INCampaignGroupSetCollection List(bool showDeleted, string connectionName)
        {
            INCampaignGroupSetCollection collection = new INCampaignGroupSetCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INCampaignGroupSetQueries.ListDeleted() : INCampaignGroupSetQueries.List(), null);
                while (reader.Read())
                {
                    INCampaignGroupSet incampaigngroupset = new INCampaignGroupSet((long)reader["ID"]);
                    incampaigngroupset.ConnectionName = connectionName;
                    collection.Add(incampaigngroupset);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INCampaignGroupSet objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the incampaigngroupset objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INCampaignGroupSetQueries.ListDeleted() : INCampaignGroupSetQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INCampaignGroupSet objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) incampaigngroupset object from the database.
        /// </summary>
        /// <param name="incampaigngroupset">The incampaigngroupset to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INCampaignGroupSet incampaigngroupset)
        {
            INCampaignGroupSetCollection collection = new INCampaignGroupSetCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(incampaigngroupset.ConnectionName, INCampaignGroupSetQueries.ListHistory(incampaigngroupset, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INCampaignGroupSet in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an incampaigngroupset object to the database.
        /// </summary>
        /// <param name="incampaigngroupset">The INCampaignGroupSet object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the incampaigngroupset was saved successfull, otherwise, false.</returns>
        internal static bool Save(INCampaignGroupSet incampaigngroupset)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (incampaigngroupset.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(incampaigngroupset.ConnectionName, INCampaignGroupSetQueries.Save(incampaigngroupset, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(incampaigngroupset.ConnectionName, INCampaignGroupSetQueries.Save(incampaigngroupset, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            incampaigngroupset.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= incampaigngroupset.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INCampaignGroupSet object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for incampaigngroupset objects in the database.
        /// </summary>
        /// <param name="fklkpincampaigngroup">The fklkpincampaigngroup search criteria.</param>
        /// <param name="fklkpincampaigngrouptype">The fklkpincampaigngrouptype search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INCampaignGroupSetCollection Search(long? fklkpincampaigngroup, long? fklkpincampaigngrouptype, string connectionName)
        {
            INCampaignGroupSetCollection collection = new INCampaignGroupSetCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INCampaignGroupSetQueries.Search(fklkpincampaigngroup, fklkpincampaigngrouptype), null);
                while (reader.Read())
                {
                    INCampaignGroupSet incampaigngroupset = new INCampaignGroupSet((long)reader["ID"]);
                    incampaigngroupset.ConnectionName = connectionName;
                    collection.Add(incampaigngroupset);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INCampaignGroupSet objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for incampaigngroupset objects in the database.
        /// </summary>
        /// <param name="fklkpincampaigngroup">The fklkpincampaigngroup search criteria.</param>
        /// <param name="fklkpincampaigngrouptype">The fklkpincampaigngrouptype search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fklkpincampaigngroup, long? fklkpincampaigngrouptype, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INCampaignGroupSetQueries.Search(fklkpincampaigngroup, fklkpincampaigngrouptype), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INCampaignGroupSet objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 incampaigngroupset objects in the database.
        /// </summary>
        /// <param name="fklkpincampaigngroup">The fklkpincampaigngroup search criteria.</param>
        /// <param name="fklkpincampaigngrouptype">The fklkpincampaigngrouptype search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INCampaignGroupSet SearchOne(long? fklkpincampaigngroup, long? fklkpincampaigngrouptype, string connectionName)
        {
            INCampaignGroupSet incampaigngroupset = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INCampaignGroupSetQueries.Search(fklkpincampaigngroup, fklkpincampaigngrouptype), null);
                if (reader.Read())
                {
                    incampaigngroupset = new INCampaignGroupSet((long)reader["ID"]);
                    incampaigngroupset.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INCampaignGroupSet objects in the database", ex);
            }
            return incampaigngroupset;
        }
        #endregion
    }
}
