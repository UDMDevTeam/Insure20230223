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
    public partial class INImportedPolicyData : ObjectBase<long>
    {
        #region Members
        private DateTime? _commencedate = null;
        private DateTime? _appsigndate = null;
        private decimal? _contractpremium = null;
        private int? _contractterm = null;
        private DateTime? _lapsedate = null;
        private decimal? _la1cancercover = null;
        private decimal? _la1cancerpremium = null;
        private decimal? _la1accidentaldeathcover = null;
        private decimal? _la1accidentaldeathpremium = null;
        private decimal? _la1disabilitycover = null;
        private decimal? _la1disabilitypremium = null;
        private decimal? _la1funeralcover = null;
        private decimal? _la1funeralpremium = null;
        private decimal? _la2cancercover = null;
        private decimal? _la2cancerpremium = null;
        private decimal? _la2accidentaldeathcover = null;
        private decimal? _la2accidentaldeathpremium = null;
        private decimal? _la2disabilitycover = null;
        private decimal? _la2disabilitypremium = null;
        private decimal? _la2funeralcover = null;
        private decimal? _la2funeralpremium = null;
        private decimal? _kidscancercover = null;
        private decimal? _kidscancerpremium = null;
        private decimal? _kidsdisabilitycover = null;
        private decimal? _kidsdisabilitypremium = null;
        private decimal? _policyfee = null;
        private decimal? _moneybackpremium = null;
        private int? _moneybackterm = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INImportedPolicyData class.
        /// </summary>
        public INImportedPolicyData()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INImportedPolicyData class.
        /// </summary>
        public INImportedPolicyData(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
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

        public DateTime? AppSignDate
        {
            get
            {
                Fill();
                return _appsigndate;
            }
            set 
            {
                Fill();
                if (value != _appsigndate)
                {
                    _appsigndate = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? ContractPremium
        {
            get
            {
                Fill();
                return _contractpremium;
            }
            set 
            {
                Fill();
                if (value != _contractpremium)
                {
                    _contractpremium = value;
                    _hasChanged = true;
                }
            }
        }

        public int? ContractTerm
        {
            get
            {
                Fill();
                return _contractterm;
            }
            set 
            {
                Fill();
                if (value != _contractterm)
                {
                    _contractterm = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? LapseDate
        {
            get
            {
                Fill();
                return _lapsedate;
            }
            set 
            {
                Fill();
                if (value != _lapsedate)
                {
                    _lapsedate = value;
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

        public decimal? LA1AccidentalDeathCover
        {
            get
            {
                Fill();
                return _la1accidentaldeathcover;
            }
            set 
            {
                Fill();
                if (value != _la1accidentaldeathcover)
                {
                    _la1accidentaldeathcover = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA1AccidentalDeathPremium
        {
            get
            {
                Fill();
                return _la1accidentaldeathpremium;
            }
            set 
            {
                Fill();
                if (value != _la1accidentaldeathpremium)
                {
                    _la1accidentaldeathpremium = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA1DisabilityCover
        {
            get
            {
                Fill();
                return _la1disabilitycover;
            }
            set 
            {
                Fill();
                if (value != _la1disabilitycover)
                {
                    _la1disabilitycover = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA1DisabilityPremium
        {
            get
            {
                Fill();
                return _la1disabilitypremium;
            }
            set 
            {
                Fill();
                if (value != _la1disabilitypremium)
                {
                    _la1disabilitypremium = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA1FuneralCover
        {
            get
            {
                Fill();
                return _la1funeralcover;
            }
            set 
            {
                Fill();
                if (value != _la1funeralcover)
                {
                    _la1funeralcover = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA1FuneralPremium
        {
            get
            {
                Fill();
                return _la1funeralpremium;
            }
            set 
            {
                Fill();
                if (value != _la1funeralpremium)
                {
                    _la1funeralpremium = value;
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

        public decimal? LA2AccidentalDeathCover
        {
            get
            {
                Fill();
                return _la2accidentaldeathcover;
            }
            set 
            {
                Fill();
                if (value != _la2accidentaldeathcover)
                {
                    _la2accidentaldeathcover = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA2AccidentalDeathPremium
        {
            get
            {
                Fill();
                return _la2accidentaldeathpremium;
            }
            set 
            {
                Fill();
                if (value != _la2accidentaldeathpremium)
                {
                    _la2accidentaldeathpremium = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA2DisabilityCover
        {
            get
            {
                Fill();
                return _la2disabilitycover;
            }
            set 
            {
                Fill();
                if (value != _la2disabilitycover)
                {
                    _la2disabilitycover = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA2DisabilityPremium
        {
            get
            {
                Fill();
                return _la2disabilitypremium;
            }
            set 
            {
                Fill();
                if (value != _la2disabilitypremium)
                {
                    _la2disabilitypremium = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA2FuneralCover
        {
            get
            {
                Fill();
                return _la2funeralcover;
            }
            set 
            {
                Fill();
                if (value != _la2funeralcover)
                {
                    _la2funeralcover = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA2FuneralPremium
        {
            get
            {
                Fill();
                return _la2funeralpremium;
            }
            set 
            {
                Fill();
                if (value != _la2funeralpremium)
                {
                    _la2funeralpremium = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? KidsCancerCover
        {
            get
            {
                Fill();
                return _kidscancercover;
            }
            set 
            {
                Fill();
                if (value != _kidscancercover)
                {
                    _kidscancercover = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? KidsCancerPremium
        {
            get
            {
                Fill();
                return _kidscancerpremium;
            }
            set 
            {
                Fill();
                if (value != _kidscancerpremium)
                {
                    _kidscancerpremium = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? KidsDisabilityCover
        {
            get
            {
                Fill();
                return _kidsdisabilitycover;
            }
            set 
            {
                Fill();
                if (value != _kidsdisabilitycover)
                {
                    _kidsdisabilitycover = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? KidsDisabilityPremium
        {
            get
            {
                Fill();
                return _kidsdisabilitypremium;
            }
            set 
            {
                Fill();
                if (value != _kidsdisabilitypremium)
                {
                    _kidsdisabilitypremium = value;
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

        public decimal? MoneyBackPremium
        {
            get
            {
                Fill();
                return _moneybackpremium;
            }
            set 
            {
                Fill();
                if (value != _moneybackpremium)
                {
                    _moneybackpremium = value;
                    _hasChanged = true;
                }
            }
        }

        public int? MoneyBackTerm
        {
            get
            {
                Fill();
                return _moneybackterm;
            }
            set 
            {
                Fill();
                if (value != _moneybackterm)
                {
                    _moneybackterm = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INImportedPolicyData object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INImportedPolicyDataMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INImportedPolicyData object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportedPolicyData object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INImportedPolicyDataMapper.Save(this);
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
        /// Deletes an INImportedPolicyData object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportedPolicyData object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INImportedPolicyDataMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INImportedPolicyData.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inimportedpolicydata>");
            xml.Append("<commencedate>" + CommenceDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</commencedate>");
            xml.Append("<appsigndate>" + AppSignDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</appsigndate>");
            xml.Append("<contractpremium>" + ContractPremium.ToString() + "</contractpremium>");
            xml.Append("<contractterm>" + ContractTerm.ToString() + "</contractterm>");
            xml.Append("<lapsedate>" + LapseDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</lapsedate>");
            xml.Append("<la1cancercover>" + LA1CancerCover.ToString() + "</la1cancercover>");
            xml.Append("<la1cancerpremium>" + LA1CancerPremium.ToString() + "</la1cancerpremium>");
            xml.Append("<la1accidentaldeathcover>" + LA1AccidentalDeathCover.ToString() + "</la1accidentaldeathcover>");
            xml.Append("<la1accidentaldeathpremium>" + LA1AccidentalDeathPremium.ToString() + "</la1accidentaldeathpremium>");
            xml.Append("<la1disabilitycover>" + LA1DisabilityCover.ToString() + "</la1disabilitycover>");
            xml.Append("<la1disabilitypremium>" + LA1DisabilityPremium.ToString() + "</la1disabilitypremium>");
            xml.Append("<la1funeralcover>" + LA1FuneralCover.ToString() + "</la1funeralcover>");
            xml.Append("<la1funeralpremium>" + LA1FuneralPremium.ToString() + "</la1funeralpremium>");
            xml.Append("<la2cancercover>" + LA2CancerCover.ToString() + "</la2cancercover>");
            xml.Append("<la2cancerpremium>" + LA2CancerPremium.ToString() + "</la2cancerpremium>");
            xml.Append("<la2accidentaldeathcover>" + LA2AccidentalDeathCover.ToString() + "</la2accidentaldeathcover>");
            xml.Append("<la2accidentaldeathpremium>" + LA2AccidentalDeathPremium.ToString() + "</la2accidentaldeathpremium>");
            xml.Append("<la2disabilitycover>" + LA2DisabilityCover.ToString() + "</la2disabilitycover>");
            xml.Append("<la2disabilitypremium>" + LA2DisabilityPremium.ToString() + "</la2disabilitypremium>");
            xml.Append("<la2funeralcover>" + LA2FuneralCover.ToString() + "</la2funeralcover>");
            xml.Append("<la2funeralpremium>" + LA2FuneralPremium.ToString() + "</la2funeralpremium>");
            xml.Append("<kidscancercover>" + KidsCancerCover.ToString() + "</kidscancercover>");
            xml.Append("<kidscancerpremium>" + KidsCancerPremium.ToString() + "</kidscancerpremium>");
            xml.Append("<kidsdisabilitycover>" + KidsDisabilityCover.ToString() + "</kidsdisabilitycover>");
            xml.Append("<kidsdisabilitypremium>" + KidsDisabilityPremium.ToString() + "</kidsdisabilitypremium>");
            xml.Append("<policyfee>" + PolicyFee.ToString() + "</policyfee>");
            xml.Append("<moneybackpremium>" + MoneyBackPremium.ToString() + "</moneybackpremium>");
            xml.Append("<moneybackterm>" + MoneyBackTerm.ToString() + "</moneybackterm>");
            xml.Append("</inimportedpolicydata>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INImportedPolicyData object from a list of parameters.
        /// </summary>
        /// <param name="commencedate"></param>
        /// <param name="appsigndate"></param>
        /// <param name="contractpremium"></param>
        /// <param name="contractterm"></param>
        /// <param name="lapsedate"></param>
        /// <param name="la1cancercover"></param>
        /// <param name="la1cancerpremium"></param>
        /// <param name="la1accidentaldeathcover"></param>
        /// <param name="la1accidentaldeathpremium"></param>
        /// <param name="la1disabilitycover"></param>
        /// <param name="la1disabilitypremium"></param>
        /// <param name="la1funeralcover"></param>
        /// <param name="la1funeralpremium"></param>
        /// <param name="la2cancercover"></param>
        /// <param name="la2cancerpremium"></param>
        /// <param name="la2accidentaldeathcover"></param>
        /// <param name="la2accidentaldeathpremium"></param>
        /// <param name="la2disabilitycover"></param>
        /// <param name="la2disabilitypremium"></param>
        /// <param name="la2funeralcover"></param>
        /// <param name="la2funeralpremium"></param>
        /// <param name="kidscancercover"></param>
        /// <param name="kidscancerpremium"></param>
        /// <param name="kidsdisabilitycover"></param>
        /// <param name="kidsdisabilitypremium"></param>
        /// <param name="policyfee"></param>
        /// <param name="moneybackpremium"></param>
        /// <param name="moneybackterm"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(DateTime? commencedate, DateTime? appsigndate, decimal? contractpremium, int? contractterm, DateTime? lapsedate, decimal? la1cancercover, decimal? la1cancerpremium, decimal? la1accidentaldeathcover, decimal? la1accidentaldeathpremium, decimal? la1disabilitycover, decimal? la1disabilitypremium, decimal? la1funeralcover, decimal? la1funeralpremium, decimal? la2cancercover, decimal? la2cancerpremium, decimal? la2accidentaldeathcover, decimal? la2accidentaldeathpremium, decimal? la2disabilitycover, decimal? la2disabilitypremium, decimal? la2funeralcover, decimal? la2funeralpremium, decimal? kidscancercover, decimal? kidscancerpremium, decimal? kidsdisabilitycover, decimal? kidsdisabilitypremium, decimal? policyfee, decimal? moneybackpremium, int? moneybackterm)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.CommenceDate = commencedate;
                this.AppSignDate = appsigndate;
                this.ContractPremium = contractpremium;
                this.ContractTerm = contractterm;
                this.LapseDate = lapsedate;
                this.LA1CancerCover = la1cancercover;
                this.LA1CancerPremium = la1cancerpremium;
                this.LA1AccidentalDeathCover = la1accidentaldeathcover;
                this.LA1AccidentalDeathPremium = la1accidentaldeathpremium;
                this.LA1DisabilityCover = la1disabilitycover;
                this.LA1DisabilityPremium = la1disabilitypremium;
                this.LA1FuneralCover = la1funeralcover;
                this.LA1FuneralPremium = la1funeralpremium;
                this.LA2CancerCover = la2cancercover;
                this.LA2CancerPremium = la2cancerpremium;
                this.LA2AccidentalDeathCover = la2accidentaldeathcover;
                this.LA2AccidentalDeathPremium = la2accidentaldeathpremium;
                this.LA2DisabilityCover = la2disabilitycover;
                this.LA2DisabilityPremium = la2disabilitypremium;
                this.LA2FuneralCover = la2funeralcover;
                this.LA2FuneralPremium = la2funeralpremium;
                this.KidsCancerCover = kidscancercover;
                this.KidsCancerPremium = kidscancerpremium;
                this.KidsDisabilityCover = kidsdisabilitycover;
                this.KidsDisabilityPremium = kidsdisabilitypremium;
                this.PolicyFee = policyfee;
                this.MoneyBackPremium = moneybackpremium;
                this.MoneyBackTerm = moneybackterm;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INImportedPolicyData's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportedPolicyData history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportedPolicyDataMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INImportedPolicyData object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportedPolicyData object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportedPolicyDataMapper.UnDelete(this);
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
    /// A collection of the INImportedPolicyData object.
    /// </summary>
    public partial class INImportedPolicyDataCollection : ObjectCollection<INImportedPolicyData>
    { 
    }
    #endregion
}
