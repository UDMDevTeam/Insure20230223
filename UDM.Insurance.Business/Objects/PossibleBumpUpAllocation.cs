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
    public partial class PossibleBumpUpAllocation : ObjectBase<long>
    {
        #region Members
        private long? _fkinimportid = null;
        private long? _fkuserid = null;
        private DateTime? _callbackdate = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the PossibleBumpUpAllocation class.
        /// </summary>
        public PossibleBumpUpAllocation()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the PossibleBumpUpAllocation class.
        /// </summary>
        public PossibleBumpUpAllocation(long id)
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

        public DateTime? CallBackDate
        {
            get
            {
                Fill();
                return _callbackdate;
            }
            set 
            {
                Fill();
                if (value != _callbackdate)
                {
                    _callbackdate = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an PossibleBumpUpAllocation object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                PossibleBumpUpAllocationMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an PossibleBumpUpAllocation object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the PossibleBumpUpAllocation object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = PossibleBumpUpAllocationMapper.Save(this);
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
        /// Deletes an PossibleBumpUpAllocation object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the PossibleBumpUpAllocation object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && PossibleBumpUpAllocationMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the PossibleBumpUpAllocation.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<possiblebumpupallocation>");
            xml.Append("<fkinimportid>" + FKINImportID.ToString() + "</fkinimportid>");
            xml.Append("<fkuserid>" + FKUserID.ToString() + "</fkuserid>");
            xml.Append("<callbackdate>" + CallBackDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</callbackdate>");
            xml.Append("</possiblebumpupallocation>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the PossibleBumpUpAllocation object from a list of parameters.
        /// </summary>
        /// <param name="fkinimportid"></param>
        /// <param name="fkuserid"></param>
        /// <param name="callbackdate"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinimportid, long? fkuserid, DateTime? callbackdate)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINImportID = fkinimportid;
                this.FKUserID = fkuserid;
                this.CallBackDate = callbackdate;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) PossibleBumpUpAllocation's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the PossibleBumpUpAllocation history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return PossibleBumpUpAllocationMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an PossibleBumpUpAllocation object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the PossibleBumpUpAllocation object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return PossibleBumpUpAllocationMapper.UnDelete(this);
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
    /// A collection of the PossibleBumpUpAllocation object.
    /// </summary>
    public partial class PossibleBumpUpAllocationCollection : ObjectCollection<PossibleBumpUpAllocation>
    { 
    }
    #endregion
}
