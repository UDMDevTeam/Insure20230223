using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using UDM.Insurance.Business.Mapping;
using Embriant.Framework.Configuration;
using Embriant.Framework;
using Embriant.Framework.Validation;

namespace UDM.Insurance.Business
{
    public partial class SalesToCallMonitoring : ObjectBase<long>
    {
        #region Members
        private long? _fkimportid = null;
        private long? _fkuserid = null;
        private string _isdisplayed = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the SalesToCallMonitoring class.
        /// </summary>
        public SalesToCallMonitoring()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the SalesToCallMonitoring class.
        /// </summary>
        public SalesToCallMonitoring(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKImportID
        {
            get
            {
                Fill();
                return _fkimportid;
            }
            set 
            {
                Fill();
                if (value != _fkimportid)
                {
                    _fkimportid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKUserID
        {
            get
            {
                Fill();
                return _fkuserid;
            }
            set 
            {
                Fill();
                if (value != _fkuserid)
                {
                    _fkuserid = value;
                    _hasChanged = true;
                }
            }
        }

        public string IsDisplayed
        {
            get
            {
                Fill();
                return _isdisplayed;
            }
            set 
            {
                Fill();
                if (value != _isdisplayed)
                {
                    _isdisplayed = value;
                    _hasChanged = true;
                }
            }
        }

        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an SalesToCallMonitoring object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                SalesToCallMonitoringMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an SalesToCallMonitoring object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the SalesToCallMonitoring object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = SalesToCallMonitoringMapper.Save(this);
                    result &= _isLoaded;
                }
                _hasChanged = !result;
                return result && AfterSave(validationResult);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes an SalesToCallMonitoring object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the SalesToCallMonitoring object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && SalesToCallMonitoringMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the SalesToCallMonitoring.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<SalesToCallMonitoring>");
            xml.Append("<fkimportid>" + FKImportID.ToString() + "</fkimportid>");
            xml.Append("<fkuserid>" + FKUserID.ToString() + "</fkuserid>");
            xml.Append("<isdisplayed>" + IsDisplayed.ToString() + "</isdisplayed>");
            xml.Append("</SalesToCallMonitoring>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the SalesToCallMonitoring object from a list of parameters.
        /// </summary>
        /// <param name="fkimportid"></param>
        /// <param name="number"></param>
        /// <param name="extension"></param>
        /// <param name="recref"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkimportid, long? fkuserid, string isdisplayed)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKImportID = fkimportid;
                this.FKUserID = fkuserid;
                this.IsDisplayed = isdisplayed;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) SalesToCallMonitoring's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the SalesToCallMonitoring history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return SalesToCallMonitoringMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an SalesToCallMonitoring object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the SalesToCallMonitoring object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return SalesToCallMonitoringMapper.UnDelete(this);
            }
            else
            {
                return false;
            }
        }
        #endregion
    }

    #region Collection
    /// <summary>
    /// A collection of the SalesToCallMonitoring object.
    /// </summary>
    public partial class SalesToCallMonitoringCollection : ObjectCollection<SalesToCallMonitoring>
    { 
    }
    #endregion
}
