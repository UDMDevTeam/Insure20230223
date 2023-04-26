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
    public partial class INImportOther : ObjectBase<long>
    {
        #region Members
        private long? _fkinimportid = null;
        private long? _fkinbatchid = null;
        private string _refno = null;
        private string _accounttype = null;
        private DateTime? _startdate = null;
        private DateTime? _enddate = null;
        private string _referralfrom = null;
        private string _addressfrom = null;
        private short? _timesremarketed = null;
        private DateTime? _lastdateremarketed = null;
        private DateTime? _collecteddate = null;
        private DateTime? _commencementdate = null;
        private int? _durationinforce = null;
        private int? _durationsinceoof = null;
        private int? _numcolls = null;
        private DateTime? _oofdate = null;
        private string _ooftype = null;
        private int? _upgradecount = null;
        private decimal? _premium = null;
        private string _bank = null;
        private string _last4digits = null;
        private DateTime? _extendedsalesdate = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INImportOther class.
        /// </summary>
        public INImportOther()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INImportOther class.
        /// </summary>
        public INImportOther(long id)
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

        public long? FKINBatchID
        {
            get
            {
                Fill();
                return _fkinbatchid;
            }
            set 
            {
                Fill();
                if (value != _fkinbatchid)
                {
                    _fkinbatchid = value;
                    _hasChanged = true;
                }
            }
        }

        public string RefNo
        {
            get
            {
                Fill();
                return _refno;
            }
            set 
            {
                Fill();
                if (value != _refno)
                {
                    _refno = value;
                    _hasChanged = true;
                }
            }
        }

        public string AccountType
        {
            get
            {
                Fill();
                return _accounttype;
            }
            set 
            {
                Fill();
                if (value != _accounttype)
                {
                    _accounttype = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? StartDate
        {
            get
            {
                Fill();
                return _startdate;
            }
            set 
            {
                Fill();
                if (value != _startdate)
                {
                    _startdate = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? EndDate
        {
            get
            {
                Fill();
                return _enddate;
            }
            set 
            {
                Fill();
                if (value != _enddate)
                {
                    _enddate = value;
                    _hasChanged = true;
                }
            }
        }

        public string ReferralFrom
        {
            get
            {
                Fill();
                return _referralfrom;
            }
            set 
            {
                Fill();
                if (value != _referralfrom)
                {
                    _referralfrom = value;
                    _hasChanged = true;
                }
            }
        }

        public string AddressFrom
        {
            get
            {
                Fill();
                return _addressfrom;
            }
            set 
            {
                Fill();
                if (value != _addressfrom)
                {
                    _addressfrom = value;
                    _hasChanged = true;
                }
            }
        }

        public short? TimesRemarketed
        {
            get
            {
                Fill();
                return _timesremarketed;
            }
            set 
            {
                Fill();
                if (value != _timesremarketed)
                {
                    _timesremarketed = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? LastDateRemarketed
        {
            get
            {
                Fill();
                return _lastdateremarketed;
            }
            set 
            {
                Fill();
                if (value != _lastdateremarketed)
                {
                    _lastdateremarketed = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? CollectedDate
        {
            get
            {
                Fill();
                return _collecteddate;
            }
            set 
            {
                Fill();
                if (value != _collecteddate)
                {
                    _collecteddate = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? CommencementDate
        {
            get
            {
                Fill();
                return _commencementdate;
            }
            set 
            {
                Fill();
                if (value != _commencementdate)
                {
                    _commencementdate = value;
                    _hasChanged = true;
                }
            }
        }

        public int? DurationInForce
        {
            get
            {
                Fill();
                return _durationinforce;
            }
            set 
            {
                Fill();
                if (value != _durationinforce)
                {
                    _durationinforce = value;
                    _hasChanged = true;
                }
            }
        }

        public int? DurationSinceOOF
        {
            get
            {
                Fill();
                return _durationsinceoof;
            }
            set 
            {
                Fill();
                if (value != _durationsinceoof)
                {
                    _durationsinceoof = value;
                    _hasChanged = true;
                }
            }
        }

        public int? NumColls
        {
            get
            {
                Fill();
                return _numcolls;
            }
            set 
            {
                Fill();
                if (value != _numcolls)
                {
                    _numcolls = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? OOFDate
        {
            get
            {
                Fill();
                return _oofdate;
            }
            set 
            {
                Fill();
                if (value != _oofdate)
                {
                    _oofdate = value;
                    _hasChanged = true;
                }
            }
        }

        public string OOFType
        {
            get
            {
                Fill();
                return _ooftype;
            }
            set 
            {
                Fill();
                if (value != _ooftype)
                {
                    _ooftype = value;
                    _hasChanged = true;
                }
            }
        }

        public int? UpgradeCount
        {
            get
            {
                Fill();
                return _upgradecount;
            }
            set 
            {
                Fill();
                if (value != _upgradecount)
                {
                    _upgradecount = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? Premium
        {
            get
            {
                Fill();
                return _premium;
            }
            set 
            {
                Fill();
                if (value != _premium)
                {
                    _premium = value;
                    _hasChanged = true;
                }
            }
        }

        public string Bank
        {
            get
            {
                Fill();
                return _bank;
            }
            set 
            {
                Fill();
                if (value != _bank)
                {
                    _bank = value;
                    _hasChanged = true;
                }
            }
        }

        public string Last4Digits
        {
            get
            {
                Fill();
                return _last4digits;
            }
            set 
            {
                Fill();
                if (value != _last4digits)
                {
                    _last4digits = value;
                    _hasChanged = true;
                }
            }
        }
        public DateTime? ExtendedSalesDate
        {
            get
            {
                Fill();
                return _extendedsalesdate;
            }
            set
            {
                Fill();
                if (value != _extendedsalesdate)
                {
                    _extendedsalesdate = value;
                    _hasChanged = true;
                }
            }
        }

        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INImportOther object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INImportOtherMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INImportOther object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportOther object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INImportOtherMapper.Save(this);
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
        /// Deletes an INImportOther object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportOther object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INImportOtherMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INImportOther.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inimportother>");
            xml.Append("<fkinimportid>" + FKINImportID.ToString() + "</fkinimportid>");
            xml.Append("<fkinbatchid>" + FKINBatchID.ToString() + "</fkinbatchid>");
            xml.Append("<refno>" + RefNo.ToString() + "</refno>");
            xml.Append("<accounttype>" + AccountType.ToString() + "</accounttype>");
            xml.Append("<startdate>" + StartDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</startdate>");
            xml.Append("<enddate>" + EndDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</enddate>");
            xml.Append("<referralfrom>" + ReferralFrom.ToString() + "</referralfrom>");
            xml.Append("<addressfrom>" + AddressFrom.ToString() + "</addressfrom>");
            xml.Append("<timesremarketed>" + TimesRemarketed.ToString() + "</timesremarketed>");
            xml.Append("<lastdateremarketed>" + LastDateRemarketed.Value.ToString("dd MMM yyyy HH:mm:ss") + "</lastdateremarketed>");
            xml.Append("<collecteddate>" + CollectedDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</collecteddate>");
            xml.Append("<commencementdate>" + CommencementDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</commencementdate>");
            xml.Append("<durationinforce>" + DurationInForce.ToString() + "</durationinforce>");
            xml.Append("<durationsinceoof>" + DurationSinceOOF.ToString() + "</durationsinceoof>");
            xml.Append("<numcolls>" + NumColls.ToString() + "</numcolls>");
            xml.Append("<oofdate>" + OOFDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</oofdate>");
            xml.Append("<ooftype>" + OOFType.ToString() + "</ooftype>");
            xml.Append("<upgradecount>" + UpgradeCount.ToString() + "</upgradecount>");
            xml.Append("<premium>" + Premium.ToString() + "</premium>");
            xml.Append("<bank>" + Bank.ToString() + "</bank>");
            xml.Append("<last4digits>" + Last4Digits.ToString() + "</last4digits>");
            xml.Append("<extendedsalesdate>" + ExtendedSalesDate.ToString() + "</extendedsalesdisplay>");
            xml.Append("</inimportother>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INImportOther object from a list of parameters.
        /// </summary>
        /// <param name="fkinimportid"></param>
        /// <param name="fkinbatchid"></param>
        /// <param name="refno"></param>
        /// <param name="accounttype"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="referralfrom"></param>
        /// <param name="addressfrom"></param>
        /// <param name="timesremarketed"></param>
        /// <param name="lastdateremarketed"></param>
        /// <param name="collecteddate"></param>
        /// <param name="commencementdate"></param>
        /// <param name="durationinforce"></param>
        /// <param name="durationsinceoof"></param>
        /// <param name="numcolls"></param>
        /// <param name="oofdate"></param>
        /// <param name="ooftype"></param>
        /// <param name="upgradecount"></param>
        /// <param name="premium"></param>
        /// <param name="bank"></param>
        /// <param name="last4digits"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinimportid, long? fkinbatchid, string refno, string accounttype, DateTime? startdate, DateTime? enddate, string referralfrom, string addressfrom, short? timesremarketed, DateTime? lastdateremarketed, DateTime? collecteddate, DateTime? commencementdate, int? durationinforce, int? durationsinceoof, int? numcolls, DateTime? oofdate, string ooftype, int? upgradecount, decimal? premium, string bank, string last4digits, DateTime? extendedsalesdate)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINImportID = fkinimportid;
                this.FKINBatchID = fkinbatchid;
                this.RefNo = refno;
                this.AccountType = accounttype;
                this.StartDate = startdate;
                this.EndDate = enddate;
                this.ReferralFrom = referralfrom;
                this.AddressFrom = addressfrom;
                this.TimesRemarketed = timesremarketed;
                this.LastDateRemarketed = lastdateremarketed;
                this.CollectedDate = collecteddate;
                this.CommencementDate = commencementdate;
                this.DurationInForce = durationinforce;
                this.DurationSinceOOF = durationsinceoof;
                this.NumColls = numcolls;
                this.OOFDate = oofdate;
                this.OOFType = ooftype;
                this.UpgradeCount = upgradecount;
                this.Premium = premium;
                this.Bank = bank;
                this.Last4Digits = last4digits;
                this.ExtendedSalesDate = extendedsalesdate;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INImportOther's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportOther history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportOtherMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INImportOther object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportOther object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportOtherMapper.UnDelete(this);
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
    /// A collection of the INImportOther object.
    /// </summary>
    public partial class INImportOtherCollection : ObjectCollection<INImportOther>
    { 
    }
    #endregion
}
