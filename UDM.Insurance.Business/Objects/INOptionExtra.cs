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
    public partial class INOptionExtra : ObjectBase<long>
    {
        #region Members
        private long? _fkoptionid = null;
        private string _la1cancercomponent = null;
        private decimal? _la1cancercover = null;
        private decimal? _la1cancerpremium = null;
        private decimal? _la1cancercost = null;
        private string _la2cancercomponent = null;
        private decimal? _la2cancercover = null;
        private decimal? _la2cancerpremium = null;
        private decimal? _la2cancercost = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INOptionExtra class.
        /// </summary>
        public INOptionExtra()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INOptionExtra class.
        /// </summary>
        public INOptionExtra(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKOptionID
        {
            get
            {
                Fill();
                return _fkoptionid;
            }
            set 
            {
                Fill();
                if (value != _fkoptionid)
                {
                    _fkoptionid = value;
                    _hasChanged = true;
                }
            }
        }

        public string LA1CancerComponent
        {
            get
            {
                Fill();
                return _la1cancercomponent;
            }
            set 
            {
                Fill();
                if (value != _la1cancercomponent)
                {
                    _la1cancercomponent = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA1CancerCover
        {
            get
            {
                Fill();
                return _la1cancercover;
            }
            set 
            {
                Fill();
                if (value != _la1cancercover)
                {
                    _la1cancercover = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA1CancerPremium
        {
            get
            {
                Fill();
                return _la1cancerpremium;
            }
            set 
            {
                Fill();
                if (value != _la1cancerpremium)
                {
                    _la1cancerpremium = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA1CancerCost
        {
            get
            {
                Fill();
                return _la1cancercost;
            }
            set 
            {
                Fill();
                if (value != _la1cancercost)
                {
                    _la1cancercost = value;
                    _hasChanged = true;
                }
            }
        }

        public string LA2CancerComponent
        {
            get
            {
                Fill();
                return _la2cancercomponent;
            }
            set 
            {
                Fill();
                if (value != _la2cancercomponent)
                {
                    _la2cancercomponent = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA2CancerCover
        {
            get
            {
                Fill();
                return _la2cancercover;
            }
            set 
            {
                Fill();
                if (value != _la2cancercover)
                {
                    _la2cancercover = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA2CancerPremium
        {
            get
            {
                Fill();
                return _la2cancerpremium;
            }
            set 
            {
                Fill();
                if (value != _la2cancerpremium)
                {
                    _la2cancerpremium = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA2CancerCost
        {
            get
            {
                Fill();
                return _la2cancercost;
            }
            set 
            {
                Fill();
                if (value != _la2cancercost)
                {
                    _la2cancercost = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INOptionExtra object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INOptionExtraMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INOptionExtra object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INOptionExtra object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INOptionExtraMapper.Save(this);
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
        /// Deletes an INOptionExtra object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INOptionExtra object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INOptionExtraMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INOptionExtra.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inoptionextra>");
            xml.Append("<fkoptionid>" + FKOptionID.ToString() + "</fkoptionid>");
            xml.Append("<la1cancercomponent>" + LA1CancerComponent.ToString() + "</la1cancercomponent>");
            xml.Append("<la1cancercover>" + LA1CancerCover.ToString() + "</la1cancercover>");
            xml.Append("<la1cancerpremium>" + LA1CancerPremium.ToString() + "</la1cancerpremium>");
            xml.Append("<la1cancercost>" + LA1CancerCost.ToString() + "</la1cancercost>");
            xml.Append("<la2cancercomponent>" + LA2CancerComponent.ToString() + "</la2cancercomponent>");
            xml.Append("<la2cancercover>" + LA2CancerCover.ToString() + "</la2cancercover>");
            xml.Append("<la2cancerpremium>" + LA2CancerPremium.ToString() + "</la2cancerpremium>");
            xml.Append("<la2cancercost>" + LA2CancerCost.ToString() + "</la2cancercost>");
            xml.Append("</inoptionextra>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INOptionExtra object from a list of parameters.
        /// </summary>
        /// <param name="fkoptionid"></param>
        /// <param name="la1cancercomponent"></param>
        /// <param name="la1cancercover"></param>
        /// <param name="la1cancerpremium"></param>
        /// <param name="la1cancercost"></param>
        /// <param name="la2cancercomponent"></param>
        /// <param name="la2cancercover"></param>
        /// <param name="la2cancerpremium"></param>
        /// <param name="la2cancercost"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkoptionid, string la1cancercomponent, decimal? la1cancercover, decimal? la1cancerpremium, decimal? la1cancercost, string la2cancercomponent, decimal? la2cancercover, decimal? la2cancerpremium, decimal? la2cancercost)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKOptionID = fkoptionid;
                this.LA1CancerComponent = la1cancercomponent;
                this.LA1CancerCover = la1cancercover;
                this.LA1CancerPremium = la1cancerpremium;
                this.LA1CancerCost = la1cancercost;
                this.LA2CancerComponent = la2cancercomponent;
                this.LA2CancerCover = la2cancercover;
                this.LA2CancerPremium = la2cancerpremium;
                this.LA2CancerCost = la2cancercost;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INOptionExtra's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INOptionExtra history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INOptionExtraMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INOptionExtra object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INOptionExtra object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INOptionExtraMapper.UnDelete(this);
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
    /// A collection of the INOptionExtra object.
    /// </summary>
    public partial class INOptionExtraCollection : ObjectCollection<INOptionExtra>
    { 
    }
    #endregion
}
