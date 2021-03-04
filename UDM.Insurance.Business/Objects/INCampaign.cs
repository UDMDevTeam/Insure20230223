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
    public partial class INCampaign : ObjectBase<long>
    {
        #region Members
        private long? _fkincampaigngroupid = null;
        private long? _fkincampaigntypeid = null;
        private string _name = null;
        private string _code = null;
        private Double? _conversion1 = null;
        private Double? _conversion2 = null;
        private Double? _covnersion3 = null;
        private Double? _conversion4 = null;
        private Double? _conversion5 = null;
        private Double? _conversion6 = null;
        private bool? _isactive = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INCampaign class.
        /// </summary>
        public INCampaign()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INCampaign class.
        /// </summary>
        public INCampaign(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKINCampaignGroupID
        {
            get
            {
                Fill();
                return _fkincampaigngroupid;
            }
            set 
            {
                Fill();
                if (value != _fkincampaigngroupid)
                {
                    _fkincampaigngroupid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINCampaignTypeID
        {
            get
            {
                Fill();
                return _fkincampaigntypeid;
            }
            set 
            {
                Fill();
                if (value != _fkincampaigntypeid)
                {
                    _fkincampaigntypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public string Name
        {
            get
            {
                Fill();
                return _name;
            }
            set 
            {
                Fill();
                if (value != _name)
                {
                    _name = value;
                    _hasChanged = true;
                }
            }
        }

        public string Code
        {
            get
            {
                Fill();
                return _code;
            }
            set 
            {
                Fill();
                if (value != _code)
                {
                    _code = value;
                    _hasChanged = true;
                }
            }
        }

        public Double? Conversion1
        {
            get
            {
                Fill();
                return _conversion1;
            }
            set 
            {
                Fill();
                if (value != _conversion1)
                {
                    _conversion1 = value;
                    _hasChanged = true;
                }
            }
        }

        public Double? Conversion2
        {
            get
            {
                Fill();
                return _conversion2;
            }
            set 
            {
                Fill();
                if (value != _conversion2)
                {
                    _conversion2 = value;
                    _hasChanged = true;
                }
            }
        }

        public Double? Covnersion3
        {
            get
            {
                Fill();
                return _covnersion3;
            }
            set 
            {
                Fill();
                if (value != _covnersion3)
                {
                    _covnersion3 = value;
                    _hasChanged = true;
                }
            }
        }

        public Double? Conversion4
        {
            get
            {
                Fill();
                return _conversion4;
            }
            set 
            {
                Fill();
                if (value != _conversion4)
                {
                    _conversion4 = value;
                    _hasChanged = true;
                }
            }
        }

        public Double? Conversion5
        {
            get
            {
                Fill();
                return _conversion5;
            }
            set 
            {
                Fill();
                if (value != _conversion5)
                {
                    _conversion5 = value;
                    _hasChanged = true;
                }
            }
        }

        public Double? Conversion6
        {
            get
            {
                Fill();
                return _conversion6;
            }
            set 
            {
                Fill();
                if (value != _conversion6)
                {
                    _conversion6 = value;
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
        /// Fills an INCampaign object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INCampaignMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INCampaign object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INCampaign object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INCampaignMapper.Save(this);
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
        /// Deletes an INCampaign object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INCampaign object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INCampaignMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INCampaign.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<incampaign>");
            xml.Append("<fkincampaigngroupid>" + FKINCampaignGroupID.ToString() + "</fkincampaigngroupid>");
            xml.Append("<fkincampaigntypeid>" + FKINCampaignTypeID.ToString() + "</fkincampaigntypeid>");
            xml.Append("<name>" + Name.ToString() + "</name>");
            xml.Append("<code>" + Code.ToString() + "</code>");
            xml.Append("<conversion1>" + Conversion1.ToString() + "</conversion1>");
            xml.Append("<conversion2>" + Conversion2.ToString() + "</conversion2>");
            xml.Append("<covnersion3>" + Covnersion3.ToString() + "</covnersion3>");
            xml.Append("<conversion4>" + Conversion4.ToString() + "</conversion4>");
            xml.Append("<conversion5>" + Conversion5.ToString() + "</conversion5>");
            xml.Append("<conversion6>" + Conversion6.ToString() + "</conversion6>");
            xml.Append("<isactive>" + IsActive.ToString() + "</isactive>");
            xml.Append("</incampaign>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INCampaign object from a list of parameters.
        /// </summary>
        /// <param name="fkincampaigngroupid"></param>
        /// <param name="fkincampaigntypeid"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="conversion1"></param>
        /// <param name="conversion2"></param>
        /// <param name="covnersion3"></param>
        /// <param name="conversion4"></param>
        /// <param name="conversion5"></param>
        /// <param name="conversion6"></param>
        /// <param name="isactive"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkincampaigngroupid, long? fkincampaigntypeid, string name, string code, Double? conversion1, Double? conversion2, Double? covnersion3, Double? conversion4, Double? conversion5, Double? conversion6, bool? isactive)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINCampaignGroupID = fkincampaigngroupid;
                this.FKINCampaignTypeID = fkincampaigntypeid;
                this.Name = name;
                this.Code = code;
                this.Conversion1 = conversion1;
                this.Conversion2 = conversion2;
                this.Covnersion3 = covnersion3;
                this.Conversion4 = conversion4;
                this.Conversion5 = conversion5;
                this.Conversion6 = conversion6;
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
        /// Deletes a(n) INCampaign's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INCampaign history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INCampaignMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INCampaign object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INCampaign object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INCampaignMapper.UnDelete(this);
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
    /// A collection of the INCampaign object.
    /// </summary>
    public partial class INCampaignCollection : ObjectCollection<INCampaign>
    { 
    }
    #endregion
}
