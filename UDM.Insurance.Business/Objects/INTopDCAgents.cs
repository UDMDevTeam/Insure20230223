using Embriant.Framework;
using Embriant.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Business.Queries;

namespace UDM.Insurance.Business.Objects
{
    public partial class INTopDCAgents : ObjectBase<long>
    {
        #region Members
        private int _fkUserID;
        private int _campaignType;
        private long _stampUserID;
        private decimal _acceptedRates;
        private DateTime _stampDate;

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INTopDCAgents class.
        /// </summary>
        public INTopDCAgents()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INTopDCAgents class with an ID.
        /// </summary>
        /// <param name="id">The unique identifier for the agent.</param>
        public INTopDCAgents(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public int FKUserID
        {
            get { return _fkUserID; }
            set
            {
                Fill();
                if (value != _fkUserID)
                {
                    _fkUserID = value;
                    _hasChanged = true;
                }
            }
        }

        public int CampaignType
        {
            get { return _campaignType; }
            set
            {
                Fill();
                if (value != _campaignType)
                {
                    _campaignType = value;
                    _hasChanged = true;
                }
            }
        }

        public long StampUserID
        {
            get { return _stampUserID; }
            set
            {
                Fill();
                if (value != _stampUserID)
                {
                    _stampUserID = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal AcceptedRates
        {
            get { return _acceptedRates; }
            set
            {
                Fill();
                if (value != _acceptedRates)
                {
                    _acceptedRates = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime StampDate
        {
            get { return _stampDate; }
            set
            {
                Fill();
                if (value != _stampDate)
                {
                    _stampDate = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INTopDCAgentsMapper.Fill(this);
            }
        }

        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INTopDCAgentsMapper.Save(this);
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

        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INTopDCAgentsMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        public override string ToXML()
        {
            var xml = new StringBuilder();
            xml.Append("<INTopDCAgents>");
            xml.Append($"<fkUserID>{FKUserID}</fkUserID>");
            xml.Append($"<campaignType>{CampaignType}</campaignType>");
            xml.Append($"<stampUserID>{StampUserID}</stampUserID>");
            xml.Append($"<acceptedRates>{AcceptedRates}</acceptedRates>");
            xml.Append($"<stampDate>{StampDate}</stampDate>");
            xml.Append("</INTopDCAgents>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        public bool RecordExists()
        {
            return INTopDCAgentsQueries.RecordExists(this);
        }

        // Add any additional methods specific to INTopDCAgents
        #endregion
    }

    #region Collection
    /// <summary>
    /// A collection of the INTopDCAgents object.
    /// </summary>
    public partial class INTopDCAgentsCollection : ObjectCollection<INTopDCAgents>
    {
    }
    #endregion

}
