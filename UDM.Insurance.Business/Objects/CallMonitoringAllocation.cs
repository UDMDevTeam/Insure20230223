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
    public partial class CallMonitoringAllocation : ObjectBase<long>
    {
        #region Members
        private long? _fkinimportid = null;
        private long? _fkuserid = null;
        private bool? _issavedcarriedforward = null;
        private DateTime? _expirydate = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the CallMonitoringAllocation class.
        /// </summary>
        public CallMonitoringAllocation()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the CallMonitoringAllocation class.
        /// </summary>
        public CallMonitoringAllocation(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKINImportID
        {
            get
            {
                Fill();
                return _fkinimportid;
            }
            set 
            {
                Fill();
                if (value != _fkinimportid)
                {
                    _fkinimportid = value;
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

        public bool? IsSavedCarriedForward
        {
            get
            {
                Fill();
                return _issavedcarriedforward;
            }
            set 
            {
                Fill();
                if (value != _issavedcarriedforward)
                {
                    _issavedcarriedforward = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? ExpiryDate
        {
            get
            {
                Fill();
                return _expirydate;
            }
            set 
            {
                Fill();
                if (value != _expirydate)
                {
                    _expirydate = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an CallMonitoringAllocation object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                CallMonitoringAllocationMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an CallMonitoringAllocation object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CallMonitoringAllocation object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = CallMonitoringAllocationMapper.Save(this);
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
        /// Deletes an CallMonitoringAllocation object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CallMonitoringAllocation object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && CallMonitoringAllocationMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the CallMonitoringAllocation.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<callmonitoringallocation>");
            xml.Append("<fkinimportid>" + FKINImportID.ToString() + "</fkinimportid>");
            xml.Append("<fkuserid>" + FKUserID.ToString() + "</fkuserid>");
            xml.Append("<issavedcarriedforward>" + IsSavedCarriedForward.ToString() + "</issavedcarriedforward>");
            xml.Append("<expirydate>" + ExpiryDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</expirydate>");
            xml.Append("</callmonitoringallocation>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the CallMonitoringAllocation object from a list of parameters.
        /// </summary>
        /// <param name="fkinimportid"></param>
        /// <param name="fkuserid"></param>
        /// <param name="issavedcarriedforward"></param>
        /// <param name="expirydate"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinimportid, long? fkuserid, bool? issavedcarriedforward, DateTime? expirydate)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINImportID = fkinimportid;
                this.FKUserID = fkuserid;
                this.IsSavedCarriedForward = issavedcarriedforward;
                this.ExpiryDate = expirydate;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) CallMonitoringAllocation's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CallMonitoringAllocation history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return CallMonitoringAllocationMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an CallMonitoringAllocation object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CallMonitoringAllocation object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return CallMonitoringAllocationMapper.UnDelete(this);
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
    /// A collection of the CallMonitoringAllocation object.
    /// </summary>
    public partial class CallMonitoringAllocationCollection : ObjectCollection<CallMonitoringAllocation>
    { 
    }
    #endregion
}
