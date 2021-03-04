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
    public partial class InfoLog1 : ObjectBase<long>
    {
        #region Members
        private long? _fkimportid = null;
        private long? _fkplanid = null;
        private decimal? _la1cover = null;
        private bool? _isla2checked = null;
        private long? _fkoptionid1 = null;
        private long? _fkoptionid2 = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the InfoLog1 class.
        /// </summary>
        public InfoLog1()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the InfoLog1 class.
        /// </summary>
        public InfoLog1(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKImportID
        {
            get
            {
                Fill();
                return _fkimportid;
            }
            set 
            {
                Fill();
                if (value != _fkimportid)
                {
                    _fkimportid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKPlanID
        {
            get
            {
                Fill();
                return _fkplanid;
            }
            set 
            {
                Fill();
                if (value != _fkplanid)
                {
                    _fkplanid = value;
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

        public bool? IsLA2Checked
        {
            get
            {
                Fill();
                return _isla2checked;
            }
            set 
            {
                Fill();
                if (value != _isla2checked)
                {
                    _isla2checked = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKOptionID1
        {
            get
            {
                Fill();
                return _fkoptionid1;
            }
            set 
            {
                Fill();
                if (value != _fkoptionid1)
                {
                    _fkoptionid1 = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKOptionID2
        {
            get
            {
                Fill();
                return _fkoptionid2;
            }
            set 
            {
                Fill();
                if (value != _fkoptionid2)
                {
                    _fkoptionid2 = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an InfoLog1 object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                InfoLog1Mapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an InfoLog1 object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the InfoLog1 object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = InfoLog1Mapper.Save(this);
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
        /// Deletes an InfoLog1 object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the InfoLog1 object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && InfoLog1Mapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the InfoLog1.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<infolog1>");
            xml.Append("<fkimportid>" + FKImportID.ToString() + "</fkimportid>");
            xml.Append("<fkplanid>" + FKPlanID.ToString() + "</fkplanid>");
            xml.Append("<la1cover>" + LA1Cover.ToString() + "</la1cover>");
            xml.Append("<isla2checked>" + IsLA2Checked.ToString() + "</isla2checked>");
            xml.Append("<fkoptionid1>" + FKOptionID1.ToString() + "</fkoptionid1>");
            xml.Append("<fkoptionid2>" + FKOptionID2.ToString() + "</fkoptionid2>");
            xml.Append("</infolog1>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the InfoLog1 object from a list of parameters.
        /// </summary>
        /// <param name="fkimportid"></param>
        /// <param name="fkplanid"></param>
        /// <param name="la1cover"></param>
        /// <param name="isla2checked"></param>
        /// <param name="fkoptionid1"></param>
        /// <param name="fkoptionid2"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkimportid, long? fkplanid, decimal? la1cover, bool? isla2checked, long? fkoptionid1, long? fkoptionid2)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKImportID = fkimportid;
                this.FKPlanID = fkplanid;
                this.LA1Cover = la1cover;
                this.IsLA2Checked = isla2checked;
                this.FKOptionID1 = fkoptionid1;
                this.FKOptionID2 = fkoptionid2;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) InfoLog1's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the InfoLog1 history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return InfoLog1Mapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an InfoLog1 object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the InfoLog1 object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return InfoLog1Mapper.UnDelete(this);
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
    /// A collection of the InfoLog1 object.
    /// </summary>
    public partial class InfoLog1Collection : ObjectCollection<InfoLog1>
    { 
    }
    #endregion
}
