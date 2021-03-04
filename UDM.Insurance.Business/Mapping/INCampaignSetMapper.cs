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
    /// Contains methods to fill, save and delete incampaignset objects.
    /// </summary>
    public partial class INCampaignSetMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) incampaignset object from the database.
        /// </summary>
        /// <param name="incampaignset">The id of the incampaignset object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the incampaignset object.</param>
        /// <returns>True if the incampaignset object was deleted successfully, else false.</returns>
        internal static bool Delete(INCampaignSet incampaignset)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(incampaignset.ConnectionName, INCampaignSetQueries.Delete(incampaignset, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INCampaignSet object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) incampaignset object from the database.
        /// </summary>
        /// <param name="incampaignset">The id of the incampaignset object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the incampaignset history.</param>
        /// <returns>True if the incampaignset history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INCampaignSet incampaignset)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(incampaignset.ConnectionName, INCampaignSetQueries.DeleteHistory(incampaignset, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INCampaignSet history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) incampaignset object from the database.
        /// </summary>
        /// <param name="incampaignset">The id of the incampaignset object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the incampaignset object.</param>
        /// <returns>True if the incampaignset object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INCampaignSet incampaignset)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(incampaignset.ConnectionName, INCampaignSetQueries.UnDelete(incampaignset, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INCampaignSet object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the incampaignset object from the data reader.
        /// </summary>
        /// <param name="incampaignset">The incampaignset object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INCampaignSet incampaignset, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    incampaignset.IsLoaded = true;
                    incampaignset.FKlkpINCampaignGroup = reader["FKlkpINCampaignGroup"] != DBNull.Value ? (long)reader["FKlkpINCampaignGroup"] : 0;
                    incampaignset.FKlkpINCampaignGroupType = reader["FKlkpINCampaignGroupType"] != DBNull.Value ? (long)reader["FKlkpINCampaignGroupType"] : 0;
                    incampaignset.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INCampaignSet does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INCampaignSet object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an incampaignset object from the database.
        /// </summary>
        /// <param name="incampaignset">The incampaignset to fill.</param>
        internal static void Fill(INCampaignSet incampaignset)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(incampaignset.ConnectionName, INCampaignSetQueries.Fill(incampaignset, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(incampaignset, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INCampaignSet object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a incampaignset object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INCampaignSet incampaignset)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(incampaignset.ConnectionName, INCampaignSetQueries.FillData(incampaignset, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INCampaignSet object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an incampaignset object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="incampaignset">The incampaignset to fill from history.</param>
        internal static void FillHistory(INCampaignSet incampaignset, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(incampaignset.ConnectionName, INCampaignSetQueries.FillHistory(incampaignset, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(incampaignset, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INCampaignSet object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the incampaignset objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INCampaignSetCollection List(bool showDeleted, string connectionName)
        {
            INCampaignSetCollection collection = new INCampaignSetCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INCampaignSetQueries.ListDeleted() : INCampaignSetQueries.List(), null);
                while (reader.Read())
                {
                    INCampaignSet incampaignset = new INCampaignSet((long)reader["ID"]);
                    incampaignset.ConnectionName = connectionName;
                    collection.Add(incampaignset);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INCampaignSet objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the incampaignset objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INCampaignSetQueries.ListDeleted() : INCampaignSetQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INCampaignSet objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) incampaignset object from the database.
        /// </summary>
        /// <param name="incampaignset">The incampaignset to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INCampaignSet incampaignset)
        {
            INCampaignSetCollection collection = new INCampaignSetCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(incampaignset.ConnectionName, INCampaignSetQueries.ListHistory(incampaignset, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INCampaignSet in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an incampaignset object to the database.
        /// </summary>
        /// <param name="incampaignset">The INCampaignSet object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the incampaignset was saved successfull, otherwise, false.</returns>
        internal static bool Save(INCampaignSet incampaignset)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (incampaignset.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(incampaignset.ConnectionName, INCampaignSetQueries.Save(incampaignset, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(incampaignset.ConnectionName, INCampaignSetQueries.Save(incampaignset, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            incampaignset.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= incampaignset.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INCampaignSet object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for incampaignset objects in the database.
        /// </summary>
        /// <param name="fklkpincampaigngroup">The fklkpincampaigngroup search criteria.</param>
        /// <param name="fklkpincampaigngrouptype">The fklkpincampaigngrouptype search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INCampaignSetCollection Search(long? fklkpincampaigngroup, long? fklkpincampaigngrouptype, string connectionName)
        {
            INCampaignSetCollection collection = new INCampaignSetCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INCampaignSetQueries.Search(fklkpincampaigngroup, fklkpincampaigngrouptype), null);
                while (reader.Read())
                {
                    INCampaignSet incampaignset = new INCampaignSet((long)reader["ID"]);
                    incampaignset.ConnectionName = connectionName;
                    collection.Add(incampaignset);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INCampaignSet objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for incampaignset objects in the database.
        /// </summary>
        /// <param name="fklkpincampaigngroup">The fklkpincampaigngroup search criteria.</param>
        /// <param name="fklkpincampaigngrouptype">The fklkpincampaigngrouptype search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fklkpincampaigngroup, long? fklkpincampaigngrouptype, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INCampaignSetQueries.Search(fklkpincampaigngroup, fklkpincampaigngrouptype), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INCampaignSet objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 incampaignset objects in the database.
        /// </summary>
        /// <param name="fklkpincampaigngroup">The fklkpincampaigngroup search criteria.</param>
        /// <param name="fklkpincampaigngrouptype">The fklkpincampaigngrouptype search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INCampaignSet SearchOne(long? fklkpincampaigngroup, long? fklkpincampaigngrouptype, string connectionName)
        {
            INCampaignSet incampaignset = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INCampaignSetQueries.Search(fklkpincampaigngroup, fklkpincampaigngrouptype), null);
                if (reader.Read())
                {
                    incampaignset = new INCampaignSet((long)reader["ID"]);
                    incampaignset.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INCampaignSet objects in the database", ex);
            }
            return incampaignset;
        }
        #endregion
    }
}
