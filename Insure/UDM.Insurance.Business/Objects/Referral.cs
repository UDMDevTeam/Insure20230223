using System;
using System.Text;
using UDM.Insurance.Business.Mapping;
using Embriant.Framework;
using Embriant.Framework.Validation;
using UDM.Insurance.Business.Queries;

namespace UDM.Insurance.Business.Objects
{
    public partial class Referral : ObjectBase<long>
    {
        #region Members
        private string _fkinimportid = null;
        private string _referralnumber = null;
        private string _name = null;
        private string _cellnumber = null;
        private long? _fkinrelatioshipid = null;
        private long? _fkgenderid = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the Referral class.
        /// </summary>
        public Referral()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the Referral class.
        /// </summary>
        public Referral(long id)
        {
            ID = id;
        }
        #endregion
        #region Properties
        public string FKINImportID
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
        public string ReferralNumber
        {
            get
            {
                Fill();
                return _referralnumber;
            }
            set
            {
                Fill();
                if (value != _referralnumber)
                {
                    _referralnumber = value;
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
        public string CellNumber
        {
            get
            {
                Fill();
                return _cellnumber;
            }
            set
            {
                Fill();
                if (value != _cellnumber)
                {
                    _cellnumber = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINRelationshipID
        {
            get
            {
                Fill();
                return _fkinrelatioshipid;
            }
            set
            {
                Fill();
                if (value != _fkinrelatioshipid)
                {
                    _fkinrelatioshipid = value;
                    _hasChanged = true;
                }
            }
        }
        public long? FKGenderID
        {
            get
            {
                Fill();
                return _fkgenderid;
            }
            set
            {
                Fill();
                if (value != _fkgenderid)
                {
                    _fkgenderid = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion
        #region Override Methods
        /// <summary>
        /// Fills an Referral object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                ReferralMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an Referral object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the Referral object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = ReferralMapper.Save(this);
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
        /// Deletes an Referral object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the Referral object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && ReferralMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the referral.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<referral>");
            xml.Append("<fkinimportid>" + FKINImportID.ToString() + "</fkimportid>");
            xml.Append("<referralnumber>" + ReferralNumber.ToString() + "</referralnumber>");
            xml.Append("<name>" + Name.ToString() + "</name>");
            xml.Append("<cellnumber>" + CellNumber.ToString() + "</cellnumber>");
            xml.Append("<fkinrelatioshipid>" + FKINRelationshipID.ToString() + "</fkinrelatioshipid>");
            xml.Append("<fkgenderid>" + FKGenderID.ToString() + "</fkgenderid>");
            xml.Append("</referral>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        public bool RecordExists()
        {
            return ReferralQueries.RecordExists(this);
        }

        public ValidationResult Fill(long? fkinrelationshipid, long? fkgenderid, string cellnumber, string name, string fkinimportid, string referralnumber)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINImportID = fkinimportid;
                this.ReferralNumber = referralnumber;
                this.Name = name;
                this.CellNumber = cellnumber;
                this.FKINRelationshipID = fkinrelationshipid;
                this.FKGenderID = fkgenderid;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) Referral's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the Referral history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return ReferralMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an Referral object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the Referral object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return ReferralMapper.UnDelete(this);
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
    /// A collection of the Referral object.
    /// </summary>
    public partial class ReferralCollection : ObjectCollection<Referral>
    {
    }
    #endregion
}
