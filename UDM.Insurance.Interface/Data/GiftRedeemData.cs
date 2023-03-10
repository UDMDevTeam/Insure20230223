using System;
using UDM.Insurance.Business;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Data
{
    public class GiftRedeemData : ObservableObject
    {

        #region Constants



        #endregion Constants

        #region Members

        private long? _importID;
        public long? ImportID
        {
            get { return _importID; }
            set { SetProperty(ref _importID, value, () => ImportID); }
        }

        private long? _PreviousRedeemStatus;

        public long? PreviousRedeemStatus
        {
            get { return _PreviousRedeemStatus; }
            set { SetProperty(ref _PreviousRedeemStatus, value, () => PreviousRedeemStatus); }
        }

        private long? _giftRedeemID;
        public long? GiftRedeemID
        {
            get { return _giftRedeemID; }
            set { SetProperty(ref _giftRedeemID, value, () => GiftRedeemID); }
        }

        private long? _giftRedeemStatusID;
        public long? GiftRedeemStatusID
        {
            get { return _giftRedeemStatusID; }
            set { SetProperty(ref _giftRedeemStatusID, value, () => GiftRedeemStatusID); }
        }

        private lkpINGiftRedeemStatus? _giftRedeemStatus;
        public lkpINGiftRedeemStatus? GiftRedeemStatus
        {
            get { return _giftRedeemStatus; }
            set { SetProperty(ref _giftRedeemStatus, value, () => GiftRedeemStatus); }
        }

        private long? _giftOptionID;
        public long? GiftOptionID
        {
            get { return _giftOptionID; }
            set { SetProperty(ref _giftOptionID, value, () => GiftOptionID); }
        }

        private DateTime? _dateRedeemed;
        public DateTime? DateRedeemed
        {
            get { return _dateRedeemed; }
            set { SetProperty(ref _dateRedeemed, value, () => DateRedeemed); }
        }

        private DateTime? _pODDate;
        public DateTime? PODDate
        {
            get { return _pODDate; }
            set { SetProperty(ref _pODDate, value, () => PODDate); }
        }

        private string _pODSignature;
        public string PODSignature
        {
            get { return _pODSignature; }
            set { SetProperty(ref _pODSignature, value, () => PODSignature); }
        }

        private bool? _isWebRedeemed;
        public bool? IsWebRedeemed
        {
            get
            {
                return _isWebRedeemed;
            }
            set
            {
                SetProperty(ref _isWebRedeemed, value, () => IsWebRedeemed);
            }
        }

        private bool _isRedeemedGiftFieldModifiable;

        /// <summary>
        /// This indicates whether or not the controls on the Redeemed Gift screen is enabled or not
        /// </summary>
        public bool IsRedeemedGiftFieldModifiable
        {
            get
            {
                return _isRedeemedGiftFieldModifiable;
            }
            set
            {
                SetProperty(ref _isRedeemedGiftFieldModifiable, value, () => IsRedeemedGiftFieldModifiable);
            }
        }

        private bool _canEditDetails;
        public bool CanEditDetails
        {
            get
            {
                return _canEditDetails;
            }
            set
            {
                SetProperty(ref _canEditDetails, value, () => CanEditDetails);
            }
        }

        private long? _fkUserID;
        public long? FKUserID
        {
            get
            {
                return _fkUserID;
            }
            set
            {
                SetProperty(ref _fkUserID, value, () => FKUserID);
            }
        }

        #endregion Members

        #region Constructor

        public GiftRedeemData()
        {
            
        }

        #endregion

        #region Public Methods

        public void Clear()
        {

        }

        #endregion

        #region Private Methods



        #endregion

    }
}
