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
    public partial class INDefaultOption : ObjectBase<long>
    {
        #region Members
        private long? _fkincampaign = null;
        private long? _fkinplanid = null;
        private long? _fkhigheroptionid = null;
        private long? _fkloweroptionid = null;
        private bool? _isactive = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INDefaultOption class.
        /// </summary>
        public INDefaultOption()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INDefaultOption class.
        /// </summary>
        public INDefaultOption(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKINCampaign
        {
            get
            {
                Fill();
                return _fkincampaign;
            }
            set 
            {
                Fill();
                if (value != _fkincampaign)
                {
                    _fkincampaign = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINPlanID
        {
            get
            {
                Fill();
                return _fkinplanid;
            }
            set 
            {
                Fill();
                if (value != _fkinplanid)
                {
                    _fkinplanid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKHigherOptionID
        {
            get
            {
                Fill();
                return _fkhigheroptionid;
            }
            set 
            {
                Fill();
                if (value != _fkhigheroptionid)
                {
                    _fkhigheroptionid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKLowerOptionID
        {
            get
            {
                Fill();
                return _fkloweroptionid;
            }
            set 
            {
                Fill();
                if (value != _fkloweroptionid)
                {
                    _fkloweroptionid = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsActive
        {
            get
            {
                Fill();
                return _isactive;
            }
            set 
            {
                Fill();
                if (value != _isactive)
                {
                    _isactive = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INDefaultOption object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INDefaultOptionMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INDefaultOption object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INDefaultOption object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INDefaultOptionMapper.Save(this);
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
        /// Deletes an INDefaultOption object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INDefaultOption object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INDefaultOptionMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INDefaultOption.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<indefaultoption>");
            xml.Append("<fkincampaign>" + FKINCampaign.ToString() + "</fkincampaign>");
            xml.Append("<fkinplanid>" + FKINPlanID.ToString() + "</fkinplanid>");
            xml.Append("<fkhigheroptionid>" + FKHigherOptionID.ToString() + "</fkhigheroptionid>");
            xml.Append("<fkloweroptionid>" + FKLowerOptionID.ToString() + "</fkloweroptionid>");
            xml.Append("<isactive>" + IsActive.ToString() + "</isactive>");
            xml.Append("</indefaultoption>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INDefaultOption object from a list of parameters.
        /// </summary>
        /// <param name="fkincampaign"></param>
        /// <param name="fkinplanid"></param>
        /// <param name="fkhigheroptionid"></param>
        /// <param name="fkloweroptionid"></param>
        /// <param name="isactive"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkincampaign, long? fkinplanid, long? fkhigheroptionid, long? fkloweroptionid, bool? isactive)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINCampaign = fkincampaign;
                this.FKINPlanID = fkinplanid;
                this.FKHigherOptionID = fkhigheroptionid;
                this.FKLowerOptionID = fkloweroptionid;
                this.IsActive = isactive;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INDefaultOption's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INDefaultOption history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INDefaultOptionMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INDefaultOption object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INDefaultOption object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INDefaultOptionMapper.UnDelete(this);
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
    /// A collection of the INDefaultOption object.
    /// </summary>
    public partial class INDefaultOptionCollection : ObjectCollection<INDefaultOption>
    { 
    }
    #endregion
}
