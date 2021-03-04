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
    /// Contains methods to fill, save and delete systeminfo objects.
    /// </summary>
    public partial class SystemInfoMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) systeminfo object from the database.
        /// </summary>
        /// <param name="systeminfo">The id of the systeminfo object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the systeminfo object.</param>
        /// <returns>True if the systeminfo object was deleted successfully, else false.</returns>
        internal static bool Delete(SystemInfo systeminfo)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(systeminfo.ConnectionName, SystemInfoQueries.Delete(systeminfo, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a SystemInfo object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) systeminfo object from the database.
        /// </summary>
        /// <param name="systeminfo">The id of the systeminfo object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the systeminfo history.</param>
        /// <returns>True if the systeminfo history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(SystemInfo systeminfo)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(systeminfo.ConnectionName, SystemInfoQueries.DeleteHistory(systeminfo, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete SystemInfo history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) systeminfo object from the database.
        /// </summary>
        /// <param name="systeminfo">The id of the systeminfo object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the systeminfo object.</param>
        /// <returns>True if the systeminfo object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(SystemInfo systeminfo)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(systeminfo.ConnectionName, SystemInfoQueries.UnDelete(systeminfo, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a SystemInfo object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the systeminfo object from the data reader.
        /// </summary>
        /// <param name="systeminfo">The systeminfo object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(SystemInfo systeminfo, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    systeminfo.IsLoaded = true;
                    systeminfo.FKSystemID = reader["FKSystemID"] != DBNull.Value ? (long)reader["FKSystemID"] : (long?)null;
                    systeminfo.SystemVersion = reader["SystemVersion"] != DBNull.Value ? (string)reader["SystemVersion"] : (string)null;
                    systeminfo.ComputerName = reader["ComputerName"] != DBNull.Value ? (string)reader["ComputerName"] : (string)null;
                    systeminfo.UserName = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : (string)null;
                    systeminfo.UserDomainName = reader["UserDomainName"] != DBNull.Value ? (string)reader["UserDomainName"] : (string)null;
                    systeminfo.SimpleOSName = reader["SimpleOSName"] != DBNull.Value ? (string)reader["SimpleOSName"] : (string)null;
                    systeminfo.OSDescription = reader["OSDescription"] != DBNull.Value ? (string)reader["OSDescription"] : (string)null;
                    systeminfo.OSArchitecture = reader["OSArchitecture"] != DBNull.Value ? (string)reader["OSArchitecture"] : (string)null;
                    systeminfo.ProcessArchitecture = reader["ProcessArchitecture"] != DBNull.Value ? (string)reader["ProcessArchitecture"] : (string)null;
                    systeminfo.FrameworkDescription = reader["FrameworkDescription"] != DBNull.Value ? (string)reader["FrameworkDescription"] : (string)null;
                    systeminfo.IPAddresses = reader["IPAddresses"] != DBNull.Value ? (string)reader["IPAddresses"] : (string)null;
                    systeminfo.StampDate = (DateTime)reader["StampDate"];
                    systeminfo.HasChanged = false;
                }
                else
                {
                    throw new MapperException("SystemInfo does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a SystemInfo object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an systeminfo object from the database.
        /// </summary>
        /// <param name="systeminfo">The systeminfo to fill.</param>
        internal static void Fill(SystemInfo systeminfo)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(systeminfo.ConnectionName, SystemInfoQueries.Fill(systeminfo, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(systeminfo, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a SystemInfo object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a systeminfo object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(SystemInfo systeminfo)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(systeminfo.ConnectionName, SystemInfoQueries.FillData(systeminfo, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a SystemInfo object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an systeminfo object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="systeminfo">The systeminfo to fill from history.</param>
        internal static void FillHistory(SystemInfo systeminfo, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(systeminfo.ConnectionName, SystemInfoQueries.FillHistory(systeminfo, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(systeminfo, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a SystemInfo object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the systeminfo objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static SystemInfoCollection List(bool showDeleted, string connectionName)
        {
            SystemInfoCollection collection = new SystemInfoCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? SystemInfoQueries.ListDeleted() : SystemInfoQueries.List(), null);
                while (reader.Read())
                {
                    SystemInfo systeminfo = new SystemInfo((long)reader["ID"]);
                    systeminfo.ConnectionName = connectionName;
                    collection.Add(systeminfo);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list SystemInfo objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the systeminfo objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? SystemInfoQueries.ListDeleted() : SystemInfoQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list SystemInfo objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) systeminfo object from the database.
        /// </summary>
        /// <param name="systeminfo">The systeminfo to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(SystemInfo systeminfo)
        {
            SystemInfoCollection collection = new SystemInfoCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(systeminfo.ConnectionName, SystemInfoQueries.ListHistory(systeminfo, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) SystemInfo in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an systeminfo object to the database.
        /// </summary>
        /// <param name="systeminfo">The SystemInfo object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the systeminfo was saved successfull, otherwise, false.</returns>
        internal static bool Save(SystemInfo systeminfo)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (systeminfo.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(systeminfo.ConnectionName, SystemInfoQueries.Save(systeminfo, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(systeminfo.ConnectionName, SystemInfoQueries.Save(systeminfo, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            systeminfo.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= systeminfo.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a SystemInfo object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for systeminfo objects in the database.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="systemversion">The systemversion search criteria.</param>
        /// <param name="computername">The computername search criteria.</param>
        /// <param name="username">The username search criteria.</param>
        /// <param name="userdomainname">The userdomainname search criteria.</param>
        /// <param name="simpleosname">The simpleosname search criteria.</param>
        /// <param name="osdescription">The osdescription search criteria.</param>
        /// <param name="osarchitecture">The osarchitecture search criteria.</param>
        /// <param name="processarchitecture">The processarchitecture search criteria.</param>
        /// <param name="frameworkdescription">The frameworkdescription search criteria.</param>
        /// <param name="ipaddresses">The ipaddresses search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static SystemInfoCollection Search(long? fksystemid, string systemversion, string computername, string username, string userdomainname, string simpleosname, string osdescription, string osarchitecture, string processarchitecture, string frameworkdescription, string ipaddresses, string connectionName)
        {
            SystemInfoCollection collection = new SystemInfoCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, SystemInfoQueries.Search(fksystemid, systemversion, computername, username, userdomainname, simpleosname, osdescription, osarchitecture, processarchitecture, frameworkdescription, ipaddresses), null);
                while (reader.Read())
                {
                    SystemInfo systeminfo = new SystemInfo((long)reader["ID"]);
                    systeminfo.ConnectionName = connectionName;
                    collection.Add(systeminfo);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for SystemInfo objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for systeminfo objects in the database.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="systemversion">The systemversion search criteria.</param>
        /// <param name="computername">The computername search criteria.</param>
        /// <param name="username">The username search criteria.</param>
        /// <param name="userdomainname">The userdomainname search criteria.</param>
        /// <param name="simpleosname">The simpleosname search criteria.</param>
        /// <param name="osdescription">The osdescription search criteria.</param>
        /// <param name="osarchitecture">The osarchitecture search criteria.</param>
        /// <param name="processarchitecture">The processarchitecture search criteria.</param>
        /// <param name="frameworkdescription">The frameworkdescription search criteria.</param>
        /// <param name="ipaddresses">The ipaddresses search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fksystemid, string systemversion, string computername, string username, string userdomainname, string simpleosname, string osdescription, string osarchitecture, string processarchitecture, string frameworkdescription, string ipaddresses, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, SystemInfoQueries.Search(fksystemid, systemversion, computername, username, userdomainname, simpleosname, osdescription, osarchitecture, processarchitecture, frameworkdescription, ipaddresses), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for SystemInfo objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 systeminfo objects in the database.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="systemversion">The systemversion search criteria.</param>
        /// <param name="computername">The computername search criteria.</param>
        /// <param name="username">The username search criteria.</param>
        /// <param name="userdomainname">The userdomainname search criteria.</param>
        /// <param name="simpleosname">The simpleosname search criteria.</param>
        /// <param name="osdescription">The osdescription search criteria.</param>
        /// <param name="osarchitecture">The osarchitecture search criteria.</param>
        /// <param name="processarchitecture">The processarchitecture search criteria.</param>
        /// <param name="frameworkdescription">The frameworkdescription search criteria.</param>
        /// <param name="ipaddresses">The ipaddresses search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static SystemInfo SearchOne(long? fksystemid, string systemversion, string computername, string username, string userdomainname, string simpleosname, string osdescription, string osarchitecture, string processarchitecture, string frameworkdescription, string ipaddresses, string connectionName)
        {
            SystemInfo systeminfo = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, SystemInfoQueries.Search(fksystemid, systemversion, computername, username, userdomainname, simpleosname, osdescription, osarchitecture, processarchitecture, frameworkdescription, ipaddresses), null);
                if (reader.Read())
                {
                    systeminfo = new SystemInfo((long)reader["ID"]);
                    systeminfo.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for SystemInfo objects in the database", ex);
            }
            return systeminfo;
        }
        #endregion
    }
}
