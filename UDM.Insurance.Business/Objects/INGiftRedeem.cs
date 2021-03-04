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
    public partial class INGiftRedeem : ObjectBase<long>
    {
        #region Members
        private long? _fkinimportid = null;
        private long? _fklkpgiftredeemstatusid = null;
        private long? _fkgiftoptionid = null;
        private DateTime? _redeemeddate = null;
        private DateTime? _poddate = null;
        private string _podsignature = null;
        private bool? _iswebredeemed = null;
        private long? _fkuserid = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INGiftRedeem class.
        /// </summary>
        public INGiftRedeem()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INGiftRedeem class.
        /// </summary>
        public INGiftRedeem(long id)
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

        public long? FKlkpGiftRedeemStatusID
        {
            get
            {
                Fill();
                return _fklkpgiftredeemstatusid;
            }
            set 
            {
                Fill();
                if (value != _fklkpgiftredeemstatusid)
                {
                    _fklkpgiftredeemstatusid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKGiftOptionID
        {
            get
            {
                Fill();
                return _fkgiftoptionid;
            }
            set 
            {
                Fill();
                if (value != _fkgiftoptionid)
                {
                    _fkgiftoptionid = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? RedeemedDate
        {
            get
            {
                Fill();
                return _redeemeddate;
            }
            set 
            {
                Fill();
                if (value != _redeemeddate)
                {
                    _redeemeddate = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? PODDate
        {
            get
            {
                Fill();
                return _poddate;
            }
            set 
            {
                Fill();
                if (value != _poddate)
                {
                    _poddate = value;
                    _hasChanged = true;
                }
            }
        }

        public string PODSignature
        {
            get
            {
                Fill();
                return _podsignature;
            }
            set 
            {
                Fill();
                if (value != _podsignature)
                {
                    _podsignature = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsWebRedeemed
        {
            get
            {
                Fill();
                return _iswebredeemed;
            }
            set 
            {
                Fill();
                if (value != _iswebredeemed)
                {
                    _iswebredeemed = value;
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
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INGiftRedeem object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INGiftRedeemMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INGiftRedeem object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INGiftRedeem object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INGiftRedeemMapper.Save(this);
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
        /// Deletes an INGiftRedeem object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INGiftRedeem object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INGiftRedeemMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INGiftRedeem.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<ingiftredeem>");
            xml.Append("<fkinimportid>" + FKINImportID.ToString() + "</fkinimportid>");
            xml.Append("<fklkpgiftredeemstatusid>" + FKlkpGiftRedeemStatusID.ToString() + "</fklkpgiftredeemstatusid>");
            xml.Append("<fkgiftoptionid>" + FKGiftOptionID.ToString() + "</fkgiftoptionid>");
            xml.Append("<redeemeddate>" + RedeemedDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</redeemeddate>");
            xml.Append("<poddate>" + PODDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</poddate>");
            xml.Append("<podsignature>" + PODSignature.ToString() + "</podsignature>");
            xml.Append("<iswebredeemed>" + IsWebRedeemed.ToString() + "</iswebredeemed>");
            xml.Append("<fkuserid>" + FKUserID.ToString() + "</fkuserid>");
            xml.Append("</ingiftredeem>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INGiftRedeem object from a list of parameters.
        /// </summary>
        /// <param name="fkinimportid"></param>
        /// <param name="fklkpgiftredeemstatusid"></param>
        /// <param name="fkgiftoptionid"></param>
        /// <param name="redeemeddate"></param>
        /// <param name="poddate"></param>
        /// <param name="podsignature"></param>
        /// <param name="iswebredeemed"></param>
        /// <param name="fkuserid"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinimportid, long? fklkpgiftredeemstatusid, long? fkgiftoptionid, DateTime? redeemeddate, DateTime? poddate, string podsignature, bool? iswebredeemed, long? fkuserid)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINImportID = fkinimportid;
                this.FKlkpGiftRedeemStatusID = fklkpgiftredeemstatusid;
                this.FKGiftOptionID = fkgiftoptionid;
                this.RedeemedDate = redeemeddate;
                this.PODDate = poddate;
                this.PODSignature = podsignature;
                this.IsWebRedeemed = iswebredeemed;
                this.FKUserID = fkuserid;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INGiftRedeem's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INGiftRedeem history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INGiftRedeemMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INGiftRedeem object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INGiftRedeem object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INGiftRedeemMapper.UnDelete(this);
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
    /// A collection of the INGiftRedeem object.
    /// </summary>
    public partial class INGiftRedeemCollection : ObjectCollection<INGiftRedeem>
    { 
    }
    #endregion
}
