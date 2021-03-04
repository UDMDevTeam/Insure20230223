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
    public partial class INQADetailsQuestionINImportMapper
    {
        #region Search
        
        public static INQADetailsQuestionINImport SearchOne(long? fkimportid, long? fkquestionid, string connectionName)
        {
            INQADetailsQuestionINImport inqadetailsquestioninimport = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INQADetailsQuestionINImportQueries.Search(fkimportid, fkquestionid), null);
                if (reader.Read())
                {
                    inqadetailsquestioninimport = new INQADetailsQuestionINImport((long)reader["ID"]);
                    inqadetailsquestioninimport.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INQADetailsQuestionINImport objects in the database", ex);
            }
            return inqadetailsquestioninimport;
        }

        #endregion
    }
}
