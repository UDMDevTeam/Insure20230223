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
    public partial class INPlan : ObjectBase<long>
    {
        #region Members
        private long? _fkinplangroupid = null;
        private string _plancode = null;
        private short? _agemin = null;
        private short? _agemax = null;
        private decimal? _igfreecover = null;
        private bool? _isactive = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INPlan class.
        /// </summary>
        public INPlan()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INPlan class.
        /// </summary>
        public INPlan(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKINPlanGroupID
        {
            get
            {
                Fill();
                return _fkinplangroupid;
            }
            set 
            {
                Fill();
                if (value != _fkinplangroupid)
                {
                    _fkinplangroupid = value;
                    _hasChanged = true;
                }
            }
        }

        public string PlanCode
        {
            get
            {
                Fill();
                return _plancode;
            }
            set 
            {
                Fill();
                if (value != _plancode)
                {
                    _plancode = value;
                    _hasChanged = true;
                }
            }
        }

        public short? AgeMin
        {
            get
            {
                Fill();
                return _agemin;
            }
            set 
            {
                Fill();
                if (value != _agemin)
                {
                    _agemin = value;
                    _hasChanged = true;
                }
            }
        }

        public short? AgeMax
        {
            get
            {
                Fill();
                return _agemax;
            }
            set 
            {
                Fill();
                if (value != _agemax)
                {
                    _agemax = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? IGFreeCover
        {
            get
            {
                Fill();
                return _igfreecover;
            }
            set 
            {
                Fill();
                if (value != _igfreecover)
                {
                    _igfreecover = value;
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
        /// Fills an INPlan object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INPlanMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INPlan object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPlan object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INPlanMapper.Save(this);
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
        /// Deletes an INPlan object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPlan object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INPlanMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INPlan.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inplan>");
            xml.Append("<fkinplangroupid>" + FKINPlanGroupID.ToString() + "</fkinplangroupid>");
            xml.Append("<plancode>" + PlanCode.ToString() + "</plancode>");
            xml.Append("<agemin>" + AgeMin.ToString() + "</agemin>");
            xml.Append("<agemax>" + AgeMax.ToString() + "</agemax>");
            xml.Append("<igfreecover>" + IGFreeCover.ToString() + "</igfreecover>");
            xml.Append("<isactive>" + IsActive.ToString() + "</isactive>");
            xml.Append("</inplan>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INPlan object from a list of parameters.
        /// </summary>
        /// <param name="fkinplangroupid"></param>
        /// <param name="plancode"></param>
        /// <param name="agemin"></param>
        /// <param name="agemax"></param>
        /// <param name="igfreecover"></param>
        /// <param name="isactive"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinplangroupid, string plancode, short? agemin, short? agemax, decimal? igfreecover, bool? isactive)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINPlanGroupID = fkinplangroupid;
                this.PlanCode = plancode;
                this.AgeMin = agemin;
                this.AgeMax = agemax;
                this.IGFreeCover = igfreecover;
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
        /// Deletes a(n) INPlan's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPlan history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INPlanMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INPlan object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPlan object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INPlanMapper.UnDelete(this);
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
    /// A collection of the INPlan object.
    /// </summary>
    public partial class INPlanCollection : ObjectCollection<INPlan>
    { 
    }
    #endregion
}
