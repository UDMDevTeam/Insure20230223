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
    /// Contains methods to fill, save and delete infolog1 objects.
    /// </summary>
    public partial class InfoLog1Mapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) infolog1 object from the database.
        /// </summary>
        /// <param name="infolog1">The id of the infolog1 object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the infolog1 object.</param>
        /// <returns>True if the infolog1 object was deleted successfully, else false.</returns>
        internal static bool Delete(InfoLog1 infolog1)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(infolog1.ConnectionName, InfoLog1Queries.Delete(infolog1, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a InfoLog1 object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) infolog1 object from the database.
        /// </summary>
        /// <param name="infolog1">The id of the infolog1 object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the infolog1 history.</param>
        /// <returns>True if the infolog1 history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(InfoLog1 infolog1)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(infolog1.ConnectionName, InfoLog1Queries.DeleteHistory(infolog1, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete InfoLog1 history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) infolog1 object from the database.
        /// </summary>
        /// <param name="infolog1">The id of the infolog1 object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the infolog1 object.</param>
        /// <returns>True if the infolog1 object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(InfoLog1 infolog1)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(infolog1.ConnectionName, InfoLog1Queries.UnDelete(infolog1, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a InfoLog1 object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the infolog1 object from the data reader.
        /// </summary>
        /// <param name="infolog1">The infolog1 object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(InfoLog1 infolog1, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    infolog1.IsLoaded = true;
                    infolog1.FKImportID = reader["FKImportID"] != DBNull.Value ? (long)reader["FKImportID"] : (long?)null;
                    infolog1.FKPlanID = reader["FKPlanID"] != DBNull.Value ? (long)reader["FKPlanID"] : (long?)null;
                    infolog1.LA1Cover = reader["LA1Cover"] != DBNull.Value ? (decimal)reader["LA1Cover"] : (decimal?)null;
                    infolog1.IsLA2Checked = reader["IsLA2Checked"] != DBNull.Value ? (bool)reader["IsLA2Checked"] : (bool?)null;
                    infolog1.FKOptionID1 = reader["FKOptionID1"] != DBNull.Value ? (long)reader["FKOptionID1"] : (long?)null;
                    infolog1.FKOptionID2 = reader["FKOptionID2"] != DBNull.Value ? (long)reader["FKOptionID2"] : (long?)null;
                    infolog1.StampDate = (DateTime)reader["StampDate"];
                    infolog1.HasChanged = false;
                }
                else
                {
                    throw new MapperException("InfoLog1 does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a InfoLog1 object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an infolog1 object from the database.
        /// </summary>
        /// <param name="infolog1">The infolog1 to fill.</param>
        internal static void Fill(InfoLog1 infolog1)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(infolog1.ConnectionName, InfoLog1Queries.Fill(infolog1, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(infolog1, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a InfoLog1 object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a infolog1 object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(InfoLog1 infolog1)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(infolog1.ConnectionName, InfoLog1Queries.FillData(infolog1, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a InfoLog1 object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an infolog1 object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="infolog1">The infolog1 to fill from history.</param>
        internal static void FillHistory(InfoLog1 infolog1, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(infolog1.ConnectionName, InfoLog1Queries.FillHistory(infolog1, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(infolog1, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a InfoLog1 object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the infolog1 objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static InfoLog1Collection List(bool showDeleted, string connectionName)
        {
            InfoLog1Collection collection = new InfoLog1Collection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? InfoLog1Queries.ListDeleted() : InfoLog1Queries.List(), null);
                while (reader.Read())
                {
                    InfoLog1 infolog1 = new InfoLog1((long)reader["ID"]);
                    infolog1.ConnectionName = connectionName;
                    collection.Add(infolog1);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list InfoLog1 objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the infolog1 objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? InfoLog1Queries.ListDeleted() : InfoLog1Queries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list InfoLog1 objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) infolog1 object from the database.
        /// </summary>
        /// <param name="infolog1">The infolog1 to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(InfoLog1 infolog1)
        {
            InfoLog1Collection collection = new InfoLog1Collection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(infolog1.ConnectionName, InfoLog1Queries.ListHistory(infolog1, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) InfoLog1 in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an infolog1 object to the database.
        /// </summary>
        /// <param name="infolog1">The InfoLog1 object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the infolog1 was saved successfull, otherwise, false.</returns>
        internal static bool Save(InfoLog1 infolog1)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (infolog1.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(infolog1.ConnectionName, InfoLog1Queries.Save(infolog1, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(infolog1.ConnectionName, InfoLog1Queries.Save(infolog1, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            infolog1.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= infolog1.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a InfoLog1 object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for infolog1 objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkplanid">The fkplanid search criteria.</param>
        /// <param name="la1cover">The la1cover search criteria.</param>
        /// <param name="isla2checked">The isla2checked search criteria.</param>
        /// <param name="fkoptionid1">The fkoptionid1 search criteria.</param>
        /// <param name="fkoptionid2">The fkoptionid2 search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static InfoLog1Collection Search(long? fkimportid, long? fkplanid, decimal? la1cover, bool? isla2checked, long? fkoptionid1, long? fkoptionid2, string connectionName)
        {
            InfoLog1Collection collection = new InfoLog1Collection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, InfoLog1Queries.Search(fkimportid, fkplanid, la1cover, isla2checked, fkoptionid1, fkoptionid2), null);
                while (reader.Read())
                {
                    InfoLog1 infolog1 = new InfoLog1((long)reader["ID"]);
                    infolog1.ConnectionName = connectionName;
                    collection.Add(infolog1);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for InfoLog1 objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for infolog1 objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkplanid">The fkplanid search criteria.</param>
        /// <param name="la1cover">The la1cover search criteria.</param>
        /// <param name="isla2checked">The isla2checked search criteria.</param>
        /// <param name="fkoptionid1">The fkoptionid1 search criteria.</param>
        /// <param name="fkoptionid2">The fkoptionid2 search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkimportid, long? fkplanid, decimal? la1cover, bool? isla2checked, long? fkoptionid1, long? fkoptionid2, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, InfoLog1Queries.Search(fkimportid, fkplanid, la1cover, isla2checked, fkoptionid1, fkoptionid2), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for InfoLog1 objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 infolog1 objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkplanid">The fkplanid search criteria.</param>
        /// <param name="la1cover">The la1cover search criteria.</param>
        /// <param name="isla2checked">The isla2checked search criteria.</param>
        /// <param name="fkoptionid1">The fkoptionid1 search criteria.</param>
        /// <param name="fkoptionid2">The fkoptionid2 search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static InfoLog1 SearchOne(long? fkimportid, long? fkplanid, decimal? la1cover, bool? isla2checked, long? fkoptionid1, long? fkoptionid2, string connectionName)
        {
            InfoLog1 infolog1 = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, InfoLog1Queries.Search(fkimportid, fkplanid, la1cover, isla2checked, fkoptionid1, fkoptionid2), null);
                if (reader.Read())
                {
                    infolog1 = new InfoLog1((long)reader["ID"]);
                    infolog1.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for InfoLog1 objects in the database", ex);
            }
            return infolog1;
        }
        #endregion
    }
}
