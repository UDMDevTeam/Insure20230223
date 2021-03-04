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
    public partial class INOption : ObjectBase<long>
    {
        #region Members
        private long? _fkinplanid = null;
        private string _optioncode = null;
        private string _category = null;
        private string _display = null;
        private decimal? _policyfee = null;
        private string _la1component = null;
        private decimal? _la1cover = null;
        private decimal? _la1premium = null;
        private decimal? _la1cost = null;
        private string _la2component = null;
        private decimal? _la2cover = null;
        private decimal? _la2premium = null;
        private decimal? _la2cost = null;
        private string _childcomponent = null;
        private decimal? _childcover = null;
        private decimal? _childpremium = null;
        private decimal? _childcost = null;
        private string _funeralcomponent = null;
        private decimal? _funeralcover = null;
        private decimal? _funeralpremium = null;
        private decimal? _funeralcost = null;
        private string _la1accidentaldeathcomponent = null;
        private decimal? _la1accidentaldeathcover = null;
        private decimal? _la1accidentaldeathpremium = null;
        private decimal? _la1accidentaldeathcost = null;
        private string _la2accidentaldeathcomponent = null;
        private decimal? _la2accidentaldeathcover = null;
        private decimal? _la2accidentaldeathpremium = null;
        private decimal? _la2accidentaldeathcost = null;
        private string _la1funeralcomponent = null;
        private decimal? _la1funeralcover = null;
        private decimal? _la1funeralpremium = null;
        private decimal? _la1funeralcost = null;
        private string _la2funeralcomponent = null;
        private decimal? _la2funeralcover = null;
        private decimal? _la2funeralpremium = null;
        private decimal? _la2funeralcost = null;
        private decimal? _totalpremium1 = null;
        private decimal? _totalpremium1systemunits = null;
        private decimal? _totalpremium1salaryunits = null;
        private decimal? _totalpremium2 = null;
        private decimal? _totalpremium2systemunits = null;
        private decimal? _totalpremium2salaryunits = null;
        private bool? _isactive = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INOption class.
        /// </summary>
        public INOption()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INOption class.
        /// </summary>
        public INOption(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
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

        public string OptionCode
        {
            get
            {
                Fill();
                return _optioncode;
            }
            set 
            {
                Fill();
                if (value != _optioncode)
                {
                    _optioncode = value;
                    _hasChanged = true;
                }
            }
        }

        public string Category
        {
            get
            {
                Fill();
                return _category;
            }
            set 
            {
                Fill();
                if (value != _category)
                {
                    _category = value;
                    _hasChanged = true;
                }
            }
        }

        public string Display
        {
            get
            {
                Fill();
                return _display;
            }
            set 
            {
                Fill();
                if (value != _display)
                {
                    _display = value;
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

        public string LA1Component
        {
            get
            {
                Fill();
                return _la1component;
            }
            set 
            {
                Fill();
                if (value != _la1component)
                {
                    _la1component = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA1Cover
        {
            get
            {
                Fill();
                return _la1cover;
            }
            set 
            {
                Fill();
                if (value != _la1cover)
                {
                    _la1cover = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA1Premium
        {
            get
            {
                Fill();
                return _la1premium;
            }
            set 
            {
                Fill();
                if (value != _la1premium)
                {
                    _la1premium = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA1Cost
        {
            get
            {
                Fill();
                return _la1cost;
            }
            set 
            {
                Fill();
                if (value != _la1cost)
                {
                    _la1cost = value;
                    _hasChanged = true;
                }
            }
        }

        public string LA2Component
        {
            get
            {
                Fill();
                return _la2component;
            }
            set 
            {
                Fill();
                if (value != _la2component)
                {
                    _la2component = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA2Cover
        {
            get
            {
                Fill();
                return _la2cover;
            }
            set 
            {
                Fill();
                if (value != _la2cover)
                {
                    _la2cover = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA2Premium
        {
            get
            {
                Fill();
                return _la2premium;
            }
            set 
            {
                Fill();
                if (value != _la2premium)
                {
                    _la2premium = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA2Cost
        {
            get
            {
                Fill();
                return _la2cost;
            }
            set 
            {
                Fill();
                if (value != _la2cost)
                {
                    _la2cost = value;
                    _hasChanged = true;
                }
            }
        }

        public string ChildComponent
        {
            get
            {
                Fill();
                return _childcomponent;
            }
            set 
            {
                Fill();
                if (value != _childcomponent)
                {
                    _childcomponent = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? ChildCover
        {
            get
            {
                Fill();
                return _childcover;
            }
            set 
            {
                Fill();
                if (value != _childcover)
                {
                    _childcover = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? ChildPremium
        {
            get
            {
                Fill();
                return _childpremium;
            }
            set 
            {
                Fill();
                if (value != _childpremium)
                {
                    _childpremium = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? ChildCost
        {
            get
            {
                Fill();
                return _childcost;
            }
            set 
            {
                Fill();
                if (value != _childcost)
                {
                    _childcost = value;
                    _hasChanged = true;
                }
            }
        }

        public string FuneralComponent
        {
            get
            {
                Fill();
                return _funeralcomponent;
            }
            set 
            {
                Fill();
                if (value != _funeralcomponent)
                {
                    _funeralcomponent = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? FuneralCover
        {
            get
            {
                Fill();
                return _funeralcover;
            }
            set 
            {
                Fill();
                if (value != _funeralcover)
                {
                    _funeralcover = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? FuneralPremium
        {
            get
            {
                Fill();
                return _funeralpremium;
            }
            set 
            {
                Fill();
                if (value != _funeralpremium)
                {
                    _funeralpremium = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? FuneralCost
        {
            get
            {
                Fill();
                return _funeralcost;
            }
            set 
            {
                Fill();
                if (value != _funeralcost)
                {
                    _funeralcost = value;
                    _hasChanged = true;
                }
            }
        }

        public string LA1AccidentalDeathComponent
        {
            get
            {
                Fill();
                return _la1accidentaldeathcomponent;
            }
            set 
            {
                Fill();
                if (value != _la1accidentaldeathcomponent)
                {
                    _la1accidentaldeathcomponent = value;
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

        public decimal? LA1AccidentalDeathCost
        {
            get
            {
                Fill();
                return _la1accidentaldeathcost;
            }
            set 
            {
                Fill();
                if (value != _la1accidentaldeathcost)
                {
                    _la1accidentaldeathcost = value;
                    _hasChanged = true;
                }
            }
        }

        public string LA2AccidentalDeathComponent
        {
            get
            {
                Fill();
                return _la2accidentaldeathcomponent;
            }
            set 
            {
                Fill();
                if (value != _la2accidentaldeathcomponent)
                {
                    _la2accidentaldeathcomponent = value;
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

        public decimal? LA2AccidentalDeathCost
        {
            get
            {
                Fill();
                return _la2accidentaldeathcost;
            }
            set 
            {
                Fill();
                if (value != _la2accidentaldeathcost)
                {
                    _la2accidentaldeathcost = value;
                    _hasChanged = true;
                }
            }
        }

        public string LA1FuneralComponent
        {
            get
            {
                Fill();
                return _la1funeralcomponent;
            }
            set 
            {
                Fill();
                if (value != _la1funeralcomponent)
                {
                    _la1funeralcomponent = value;
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

        public decimal? LA1FuneralCost
        {
            get
            {
                Fill();
                return _la1funeralcost;
            }
            set 
            {
                Fill();
                if (value != _la1funeralcost)
                {
                    _la1funeralcost = value;
                    _hasChanged = true;
                }
            }
        }

        public string LA2FuneralComponent
        {
            get
            {
                Fill();
                return _la2funeralcomponent;
            }
            set 
            {
                Fill();
                if (value != _la2funeralcomponent)
                {
                    _la2funeralcomponent = value;
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

        public decimal? LA2FuneralCost
        {
            get
            {
                Fill();
                return _la2funeralcost;
            }
            set 
            {
                Fill();
                if (value != _la2funeralcost)
                {
                    _la2funeralcost = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? TotalPremium1
        {
            get
            {
                Fill();
                return _totalpremium1;
            }
            set 
            {
                Fill();
                if (value != _totalpremium1)
                {
                    _totalpremium1 = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? TotalPremium1SystemUnits
        {
            get
            {
                Fill();
                return _totalpremium1systemunits;
            }
            set 
            {
                Fill();
                if (value != _totalpremium1systemunits)
                {
                    _totalpremium1systemunits = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? TotalPremium1SalaryUnits
        {
            get
            {
                Fill();
                return _totalpremium1salaryunits;
            }
            set 
            {
                Fill();
                if (value != _totalpremium1salaryunits)
                {
                    _totalpremium1salaryunits = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? TotalPremium2
        {
            get
            {
                Fill();
                return _totalpremium2;
            }
            set 
            {
                Fill();
                if (value != _totalpremium2)
                {
                    _totalpremium2 = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? TotalPremium2SystemUnits
        {
            get
            {
                Fill();
                return _totalpremium2systemunits;
            }
            set 
            {
                Fill();
                if (value != _totalpremium2systemunits)
                {
                    _totalpremium2systemunits = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? TotalPremium2SalaryUnits
        {
            get
            {
                Fill();
                return _totalpremium2salaryunits;
            }
            set 
            {
                Fill();
                if (value != _totalpremium2salaryunits)
                {
                    _totalpremium2salaryunits = value;
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
        /// Fills an INOption object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INOptionMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INOption object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INOption object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INOptionMapper.Save(this);
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
        /// Deletes an INOption object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INOption object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INOptionMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INOption.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inoption>");
            xml.Append("<fkinplanid>" + FKINPlanID.ToString() + "</fkinplanid>");
            xml.Append("<optioncode>" + OptionCode.ToString() + "</optioncode>");
            xml.Append("<category>" + Category.ToString() + "</category>");
            xml.Append("<display>" + Display.ToString() + "</display>");
            xml.Append("<policyfee>" + PolicyFee.ToString() + "</policyfee>");
            xml.Append("<la1component>" + LA1Component.ToString() + "</la1component>");
            xml.Append("<la1cover>" + LA1Cover.ToString() + "</la1cover>");
            xml.Append("<la1premium>" + LA1Premium.ToString() + "</la1premium>");
            xml.Append("<la1cost>" + LA1Cost.ToString() + "</la1cost>");
            xml.Append("<la2component>" + LA2Component.ToString() + "</la2component>");
            xml.Append("<la2cover>" + LA2Cover.ToString() + "</la2cover>");
            xml.Append("<la2premium>" + LA2Premium.ToString() + "</la2premium>");
            xml.Append("<la2cost>" + LA2Cost.ToString() + "</la2cost>");
            xml.Append("<childcomponent>" + ChildComponent.ToString() + "</childcomponent>");
            xml.Append("<childcover>" + ChildCover.ToString() + "</childcover>");
            xml.Append("<childpremium>" + ChildPremium.ToString() + "</childpremium>");
            xml.Append("<childcost>" + ChildCost.ToString() + "</childcost>");
            xml.Append("<funeralcomponent>" + FuneralComponent.ToString() + "</funeralcomponent>");
            xml.Append("<funeralcover>" + FuneralCover.ToString() + "</funeralcover>");
            xml.Append("<funeralpremium>" + FuneralPremium.ToString() + "</funeralpremium>");
            xml.Append("<funeralcost>" + FuneralCost.ToString() + "</funeralcost>");
            xml.Append("<la1accidentaldeathcomponent>" + LA1AccidentalDeathComponent.ToString() + "</la1accidentaldeathcomponent>");
            xml.Append("<la1accidentaldeathcover>" + LA1AccidentalDeathCover.ToString() + "</la1accidentaldeathcover>");
            xml.Append("<la1accidentaldeathpremium>" + LA1AccidentalDeathPremium.ToString() + "</la1accidentaldeathpremium>");
            xml.Append("<la1accidentaldeathcost>" + LA1AccidentalDeathCost.ToString() + "</la1accidentaldeathcost>");
            xml.Append("<la2accidentaldeathcomponent>" + LA2AccidentalDeathComponent.ToString() + "</la2accidentaldeathcomponent>");
            xml.Append("<la2accidentaldeathcover>" + LA2AccidentalDeathCover.ToString() + "</la2accidentaldeathcover>");
            xml.Append("<la2accidentaldeathpremium>" + LA2AccidentalDeathPremium.ToString() + "</la2accidentaldeathpremium>");
            xml.Append("<la2accidentaldeathcost>" + LA2AccidentalDeathCost.ToString() + "</la2accidentaldeathcost>");
            xml.Append("<la1funeralcomponent>" + LA1FuneralComponent.ToString() + "</la1funeralcomponent>");
            xml.Append("<la1funeralcover>" + LA1FuneralCover.ToString() + "</la1funeralcover>");
            xml.Append("<la1funeralpremium>" + LA1FuneralPremium.ToString() + "</la1funeralpremium>");
            xml.Append("<la1funeralcost>" + LA1FuneralCost.ToString() + "</la1funeralcost>");
            xml.Append("<la2funeralcomponent>" + LA2FuneralComponent.ToString() + "</la2funeralcomponent>");
            xml.Append("<la2funeralcover>" + LA2FuneralCover.ToString() + "</la2funeralcover>");
            xml.Append("<la2funeralpremium>" + LA2FuneralPremium.ToString() + "</la2funeralpremium>");
            xml.Append("<la2funeralcost>" + LA2FuneralCost.ToString() + "</la2funeralcost>");
            xml.Append("<totalpremium1>" + TotalPremium1.ToString() + "</totalpremium1>");
            xml.Append("<totalpremium1systemunits>" + TotalPremium1SystemUnits.ToString() + "</totalpremium1systemunits>");
            xml.Append("<totalpremium1salaryunits>" + TotalPremium1SalaryUnits.ToString() + "</totalpremium1salaryunits>");
            xml.Append("<totalpremium2>" + TotalPremium2.ToString() + "</totalpremium2>");
            xml.Append("<totalpremium2systemunits>" + TotalPremium2SystemUnits.ToString() + "</totalpremium2systemunits>");
            xml.Append("<totalpremium2salaryunits>" + TotalPremium2SalaryUnits.ToString() + "</totalpremium2salaryunits>");
            xml.Append("<isactive>" + IsActive.ToString() + "</isactive>");
            xml.Append("</inoption>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INOption object from a list of parameters.
        /// </summary>
        /// <param name="fkinplanid"></param>
        /// <param name="optioncode"></param>
        /// <param name="category"></param>
        /// <param name="display"></param>
        /// <param name="policyfee"></param>
        /// <param name="la1component"></param>
        /// <param name="la1cover"></param>
        /// <param name="la1premium"></param>
        /// <param name="la1cost"></param>
        /// <param name="la2component"></param>
        /// <param name="la2cover"></param>
        /// <param name="la2premium"></param>
        /// <param name="la2cost"></param>
        /// <param name="childcomponent"></param>
        /// <param name="childcover"></param>
        /// <param name="childpremium"></param>
        /// <param name="childcost"></param>
        /// <param name="funeralcomponent"></param>
        /// <param name="funeralcover"></param>
        /// <param name="funeralpremium"></param>
        /// <param name="funeralcost"></param>
        /// <param name="la1accidentaldeathcomponent"></param>
        /// <param name="la1accidentaldeathcover"></param>
        /// <param name="la1accidentaldeathpremium"></param>
        /// <param name="la1accidentaldeathcost"></param>
        /// <param name="la2accidentaldeathcomponent"></param>
        /// <param name="la2accidentaldeathcover"></param>
        /// <param name="la2accidentaldeathpremium"></param>
        /// <param name="la2accidentaldeathcost"></param>
        /// <param name="la1funeralcomponent"></param>
        /// <param name="la1funeralcover"></param>
        /// <param name="la1funeralpremium"></param>
        /// <param name="la1funeralcost"></param>
        /// <param name="la2funeralcomponent"></param>
        /// <param name="la2funeralcover"></param>
        /// <param name="la2funeralpremium"></param>
        /// <param name="la2funeralcost"></param>
        /// <param name="totalpremium1"></param>
        /// <param name="totalpremium1systemunits"></param>
        /// <param name="totalpremium1salaryunits"></param>
        /// <param name="totalpremium2"></param>
        /// <param name="totalpremium2systemunits"></param>
        /// <param name="totalpremium2salaryunits"></param>
        /// <param name="isactive"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinplanid, string optioncode, string category, string display, decimal? policyfee, string la1component, decimal? la1cover, decimal? la1premium, decimal? la1cost, string la2component, decimal? la2cover, decimal? la2premium, decimal? la2cost, string childcomponent, decimal? childcover, decimal? childpremium, decimal? childcost, string funeralcomponent, decimal? funeralcover, decimal? funeralpremium, decimal? funeralcost, string la1accidentaldeathcomponent, decimal? la1accidentaldeathcover, decimal? la1accidentaldeathpremium, decimal? la1accidentaldeathcost, string la2accidentaldeathcomponent, decimal? la2accidentaldeathcover, decimal? la2accidentaldeathpremium, decimal? la2accidentaldeathcost, string la1funeralcomponent, decimal? la1funeralcover, decimal? la1funeralpremium, decimal? la1funeralcost, string la2funeralcomponent, decimal? la2funeralcover, decimal? la2funeralpremium, decimal? la2funeralcost, decimal? totalpremium1, decimal? totalpremium1systemunits, decimal? totalpremium1salaryunits, decimal? totalpremium2, decimal? totalpremium2systemunits, decimal? totalpremium2salaryunits, bool? isactive)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINPlanID = fkinplanid;
                this.OptionCode = optioncode;
                this.Category = category;
                this.Display = display;
                this.PolicyFee = policyfee;
                this.LA1Component = la1component;
                this.LA1Cover = la1cover;
                this.LA1Premium = la1premium;
                this.LA1Cost = la1cost;
                this.LA2Component = la2component;
                this.LA2Cover = la2cover;
                this.LA2Premium = la2premium;
                this.LA2Cost = la2cost;
                this.ChildComponent = childcomponent;
                this.ChildCover = childcover;
                this.ChildPremium = childpremium;
                this.ChildCost = childcost;
                this.FuneralComponent = funeralcomponent;
                this.FuneralCover = funeralcover;
                this.FuneralPremium = funeralpremium;
                this.FuneralCost = funeralcost;
                this.LA1AccidentalDeathComponent = la1accidentaldeathcomponent;
                this.LA1AccidentalDeathCover = la1accidentaldeathcover;
                this.LA1AccidentalDeathPremium = la1accidentaldeathpremium;
                this.LA1AccidentalDeathCost = la1accidentaldeathcost;
                this.LA2AccidentalDeathComponent = la2accidentaldeathcomponent;
                this.LA2AccidentalDeathCover = la2accidentaldeathcover;
                this.LA2AccidentalDeathPremium = la2accidentaldeathpremium;
                this.LA2AccidentalDeathCost = la2accidentaldeathcost;
                this.LA1FuneralComponent = la1funeralcomponent;
                this.LA1FuneralCover = la1funeralcover;
                this.LA1FuneralPremium = la1funeralpremium;
                this.LA1FuneralCost = la1funeralcost;
                this.LA2FuneralComponent = la2funeralcomponent;
                this.LA2FuneralCover = la2funeralcover;
                this.LA2FuneralPremium = la2funeralpremium;
                this.LA2FuneralCost = la2funeralcost;
                this.TotalPremium1 = totalpremium1;
                this.TotalPremium1SystemUnits = totalpremium1systemunits;
                this.TotalPremium1SalaryUnits = totalpremium1salaryunits;
                this.TotalPremium2 = totalpremium2;
                this.TotalPremium2SystemUnits = totalpremium2systemunits;
                this.TotalPremium2SalaryUnits = totalpremium2salaryunits;
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
        /// Deletes a(n) INOption's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INOption history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INOptionMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INOption object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INOption object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INOptionMapper.UnDelete(this);
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
    /// A collection of the INOption object.
    /// </summary>
    public partial class INOptionCollection : ObjectCollection<INOption>
    { 
    }
    #endregion
}
