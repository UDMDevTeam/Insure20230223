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
    public partial class INPolicy : ObjectBase<long>
    {
        #region Members
        private string _policyid = null;
        private long? _fkpolicytypeid = null;
        private long? _fkinpolicyholderid = null;
        private long? _fkinbankdetailsid = null;
        private long? _fkinoptionid = null;
        private long? _fkinmoneybackid = null;
        private long? _fkinbumpupoptionid = null;
        private bool? _udmbumpupoption = null;
        private decimal? _bumpupamount = null;
        private bool? _reducedpremiumoption = null;
        private decimal? _reducedpremiumamount = null;
        private decimal? _policyfee = null;
        private decimal? _totalpremium = null;
        private DateTime? _commencedate = null;
        private bool? _optionla2 = null;
        private bool? _optionchild = null;
        private bool? _optionfuneral = null;
        private bool? _bumpupoffered = null;
        private decimal? _totalinvoicefee = null;
        private long? _fkinoptionfeesid = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INPolicy class.
        /// </summary>
        public INPolicy()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INPolicy class.
        /// </summary>
        public INPolicy(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public string PolicyID
        {
            get
            {
                Fill();
                return _policyid;
            }
            set 
            {
                Fill();
                if (value != _policyid)
                {
                    _policyid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKPolicyTypeID
        {
            get
            {
                Fill();
                return _fkpolicytypeid;
            }
            set 
            {
                Fill();
                if (value != _fkpolicytypeid)
                {
                    _fkpolicytypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINPolicyHolderID
        {
            get
            {
                Fill();
                return _fkinpolicyholderid;
            }
            set 
            {
                Fill();
                if (value != _fkinpolicyholderid)
                {
                    _fkinpolicyholderid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINBankDetailsID
        {
            get
            {
                Fill();
                return _fkinbankdetailsid;
            }
            set 
            {
                Fill();
                if (value != _fkinbankdetailsid)
                {
                    _fkinbankdetailsid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINOptionID
        {
            get
            {
                Fill();
                return _fkinoptionid;
            }
            set 
            {
                Fill();
                if (value != _fkinoptionid)
                {
                    _fkinoptionid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINMoneyBackID
        {
            get
            {
                Fill();
                return _fkinmoneybackid;
            }
            set 
            {
                Fill();
                if (value != _fkinmoneybackid)
                {
                    _fkinmoneybackid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINBumpUpOptionID
        {
            get
            {
                Fill();
                return _fkinbumpupoptionid;
            }
            set 
            {
                Fill();
                if (value != _fkinbumpupoptionid)
                {
                    _fkinbumpupoptionid = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? UDMBumpUpOption
        {
            get
            {
                Fill();
                return _udmbumpupoption;
            }
            set 
            {
                Fill();
                if (value != _udmbumpupoption)
                {
                    _udmbumpupoption = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? BumpUpAmount
        {
            get
            {
                Fill();
                return _bumpupamount;
            }
            set 
            {
                Fill();
                if (value != _bumpupamount)
                {
                    _bumpupamount = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? ReducedPremiumOption
        {
            get
            {
                Fill();
                return _reducedpremiumoption;
            }
            set 
            {
                Fill();
                if (value != _reducedpremiumoption)
                {
                    _reducedpremiumoption = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? ReducedPremiumAmount
        {
            get
            {
                Fill();
                return _reducedpremiumamount;
            }
            set 
            {
                Fill();
                if (value != _reducedpremiumamount)
                {
                    _reducedpremiumamount = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? PolicyFee
        {
            get
            {
                Fill();
                return _policyfee;
            }
            set 
            {
                Fill();
                if (value != _policyfee)
                {
                    _policyfee = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? TotalPremium
        {
            get
            {
                Fill();
                return _totalpremium;
            }
            set 
            {
                Fill();
                if (value != _totalpremium)
                {
                    _totalpremium = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? CommenceDate
        {
            get
            {
                Fill();
                return _commencedate;
            }
            set 
            {
                Fill();
                if (value != _commencedate)
                {
                    _commencedate = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? OptionLA2
        {
            get
            {
                Fill();
                return _optionla2;
            }
            set 
            {
                Fill();
                if (value != _optionla2)
                {
                    _optionla2 = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? OptionChild
        {
            get
            {
                Fill();
                return _optionchild;
            }
            set 
            {
                Fill();
                if (value != _optionchild)
                {
                    _optionchild = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? OptionFuneral
        {
            get
            {
                Fill();
                return _optionfuneral;
            }
            set 
            {
                Fill();
                if (value != _optionfuneral)
                {
                    _optionfuneral = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? BumpUpOffered
        {
            get
            {
                Fill();
                return _bumpupoffered;
            }
            set 
            {
                Fill();
                if (value != _bumpupoffered)
                {
                    _bumpupoffered = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? TotalInvoiceFee
        {
            get
            {
                Fill();
                return _totalinvoicefee;
            }
            set 
            {
                Fill();
                if (value != _totalinvoicefee)
                {
                    _totalinvoicefee = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINOptionFeesID
        {
            get
            {
                Fill();
                return _fkinoptionfeesid;
            }
            set 
            {
                Fill();
                if (value != _fkinoptionfeesid)
                {
                    _fkinoptionfeesid = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INPolicy object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INPolicyMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INPolicy object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicy object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INPolicyMapper.Save(this);
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
        /// Deletes an INPolicy object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicy object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INPolicyMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INPolicy.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inpolicy>");
            xml.Append("<policyid>" + PolicyID.ToString() + "</policyid>");
            xml.Append("<fkpolicytypeid>" + FKPolicyTypeID.ToString() + "</fkpolicytypeid>");
            xml.Append("<fkinpolicyholderid>" + FKINPolicyHolderID.ToString() + "</fkinpolicyholderid>");
            xml.Append("<fkinbankdetailsid>" + FKINBankDetailsID.ToString() + "</fkinbankdetailsid>");
            xml.Append("<fkinoptionid>" + FKINOptionID.ToString() + "</fkinoptionid>");
            xml.Append("<fkinmoneybackid>" + FKINMoneyBackID.ToString() + "</fkinmoneybackid>");
            xml.Append("<fkinbumpupoptionid>" + FKINBumpUpOptionID.ToString() + "</fkinbumpupoptionid>");
            xml.Append("<udmbumpupoption>" + UDMBumpUpOption.ToString() + "</udmbumpupoption>");
            xml.Append("<bumpupamount>" + BumpUpAmount.ToString() + "</bumpupamount>");
            xml.Append("<reducedpremiumoption>" + ReducedPremiumOption.ToString() + "</reducedpremiumoption>");
            xml.Append("<reducedpremiumamount>" + ReducedPremiumAmount.ToString() + "</reducedpremiumamount>");
            xml.Append("<policyfee>" + PolicyFee.ToString() + "</policyfee>");
            xml.Append("<totalpremium>" + TotalPremium.ToString() + "</totalpremium>");
            xml.Append("<commencedate>" + CommenceDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</commencedate>");
            xml.Append("<optionla2>" + OptionLA2.ToString() + "</optionla2>");
            xml.Append("<optionchild>" + OptionChild.ToString() + "</optionchild>");
            xml.Append("<optionfuneral>" + OptionFuneral.ToString() + "</optionfuneral>");
            xml.Append("<bumpupoffered>" + BumpUpOffered.ToString() + "</bumpupoffered>");
            xml.Append("<totalinvoicefee>" + TotalInvoiceFee.ToString() + "</totalinvoicefee>");
            xml.Append("<fkinoptionfeesid>" + FKINOptionFeesID.ToString() + "</fkinoptionfeesid>");
            xml.Append("</inpolicy>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INPolicy object from a list of parameters.
        /// </summary>
        /// <param name="policyid"></param>
        /// <param name="fkpolicytypeid"></param>
        /// <param name="fkinpolicyholderid"></param>
        /// <param name="fkinbankdetailsid"></param>
        /// <param name="fkinoptionid"></param>
        /// <param name="fkinmoneybackid"></param>
        /// <param name="fkinbumpupoptionid"></param>
        /// <param name="udmbumpupoption"></param>
        /// <param name="bumpupamount"></param>
        /// <param name="reducedpremiumoption"></param>
        /// <param name="reducedpremiumamount"></param>
        /// <param name="policyfee"></param>
        /// <param name="totalpremium"></param>
        /// <param name="commencedate"></param>
        /// <param name="optionla2"></param>
        /// <param name="optionchild"></param>
        /// <param name="optionfuneral"></param>
        /// <param name="bumpupoffered"></param>
        /// <param name="totalinvoicefee"></param>
        /// <param name="fkinoptionfeesid"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(string policyid, long? fkpolicytypeid, long? fkinpolicyholderid, long? fkinbankdetailsid, long? fkinoptionid, long? fkinmoneybackid, long? fkinbumpupoptionid, bool? udmbumpupoption, decimal? bumpupamount, bool? reducedpremiumoption, decimal? reducedpremiumamount, decimal? policyfee, decimal? totalpremium, DateTime? commencedate, bool? optionla2, bool? optionchild, bool? optionfuneral, bool? bumpupoffered, decimal? totalinvoicefee, long? fkinoptionfeesid)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.PolicyID = policyid;
                this.FKPolicyTypeID = fkpolicytypeid;
                this.FKINPolicyHolderID = fkinpolicyholderid;
                this.FKINBankDetailsID = fkinbankdetailsid;
                this.FKINOptionID = fkinoptionid;
                this.FKINMoneyBackID = fkinmoneybackid;
                this.FKINBumpUpOptionID = fkinbumpupoptionid;
                this.UDMBumpUpOption = udmbumpupoption;
                this.BumpUpAmount = bumpupamount;
                this.ReducedPremiumOption = reducedpremiumoption;
                this.ReducedPremiumAmount = reducedpremiumamount;
                this.PolicyFee = policyfee;
                this.TotalPremium = totalpremium;
                this.CommenceDate = commencedate;
                this.OptionLA2 = optionla2;
                this.OptionChild = optionchild;
                this.OptionFuneral = optionfuneral;
                this.BumpUpOffered = bumpupoffered;
                this.TotalInvoiceFee = totalinvoicefee;
                this.FKINOptionFeesID = fkinoptionfeesid;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INPolicy's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicy history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INPolicyMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INPolicy object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicy object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INPolicyMapper.UnDelete(this);
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
    /// A collection of the INPolicy object.
    /// </summary>
    public partial class INPolicyCollection : ObjectCollection<INPolicy>
    { 
    }
    #endregion
}
