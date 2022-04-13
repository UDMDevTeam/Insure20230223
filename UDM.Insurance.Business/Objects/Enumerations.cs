namespace UDM.Insurance.Business
{
    public enum Lookups
    {
        lkpCompany,
        lkpSystemStatus,
        lkpSystem,
        lkpUserType,
        lkpINCampaignType,
        lkpINCampaignGroup,
        lkpINPolicyType,
        lkpINTitle,
        lkpINRelationship,
        lkpLanguage,
        lkpGender,
        lkpPaymentType
    }

    public enum lkpUserType
    {
        Administrator = 1,
        SalesAgent = 2,
        ConfirmationAgent = 3,
        Manager = 4,
        DataCapturer = 5,
        StatusLoader = 6,
        SeniorAdministrator = 7,
        CallMonitoringAgent = 8,
        Preserver = 10,
        DebiCheckAgent = 12
    }

    public enum lkpCompany
    {
        IG = 1,
        Insurance = 2,
        IGInsurance = 3 //HR?
    }

    public enum lkpSystem
    {
        Blush = 1,
        Insurance = 2,
        HR = 3
    }

    public enum lkpSystemStatus
    {
        Active = 1,
        Inactive = 2
    }

    public enum lkpINCampaignGroup
    {
        Base = 1,
        Upgrade = 2,
        Rejuvenation = 3,
        Defrosted = 4,
        Starter = 5,
        Renewals = 6,
        Upgrade1 = 7,
        Upgrade2 = 8,
        Upgrade3 = 9,
        Upgrade4 = 10,
        Upgrade5 = 11,
        Upgrade6 = 12,
        Upgrade7 = 13,
        DoubleUpgrade1 = 14,
        DoubleUpgrade2 = 15,
        DoubleUpgrade3 = 16,
        DoubleUpgrade4 = 17,
        DoubleUpgrade5 = 18,
        DoubleUpgrade6 = 19,
        DoubleUpgrade7 = 20,
        Reactivation = 21,
        Extension = 22,
        DoubleUpgrade8 = 23,
        ReDefrost = 24,
        Mining = 25,
        Admin = 26,
        Upgrade8 = 27,
        Upgrade9 = 28,
        DoubleUpgrade9 = 29,
        DoubleUpgrade10 = 30,
        DoubleUpgrade11 = 31,
        Upgrade10 = 32,
        DoubleUpgrade12 = 33,
        Resurrection = 34,
        Upgrade11 = 35,
        Upgrade12 = 37,
        DoubleUpgrade13 = 36,
        DoubleUpgrade14 = 38,
        DefrostR99 = 39,
        Lite = 40,
        Upgrade13 = 41,
        R99 = 42,
        SpouseLite = 43,
        Upgrade14 = 44,
        DoubleUpgrade15 = 45,
        R99NG = 46,
        R99Upgrade = 47
    }

    public enum lkpINCampaignType
    {
        Cancer = 1,
        Macc = 2,
        MaccMillion = 3,
        AccDis = 4,
        CancerFuneral = 5,
        MaccFuneral = 6,
        BlackMacc = 7,
        FemaleDis = 8,
        IGCancer = 9,
        Any = 10,
        BlackMaccMillion = 11,
        TermCancer = 12,
        CancerFuneral99 = 13,
        IGFemaleDisability = 14,
        MaccCancer = 15,
        MaccMillionCancer = 16,
        FemaleDisCancer = 17
    }

    public enum lkpINPolicyType
    {
        Cancer = 1,
        Macc = 2,
        MaccMillion = 3,
        AccDis = 4,
        CancerFuneral = 5,
        MaccFuneral = 6,
        BlackMacc = 7,
        FemaleDis = 8,
        IGCancer = 9,
        Any = 10,
        BlackMaccMillion = 11,
        TermCancer = 12,
        CancerFuneral99 = 13,
        IGFemaleDisability = 14,
        MaccCancer = 15,
        MaccMillionCancer = 16
    }

    public enum lkpLanguage
    {
        Afrikaans = 1,
        English = 2
    }

    public enum lkpGender
    {
        Male = 1,
        Female = 2
    }

    public enum ScreenDirection
    {
        Forward,
        Reverse
    }

    public enum lkpINLeadStatus
    {
        Accepted = 1,
        Declined = 2,
        Pending = 3,
        NoContact = 4,
        NotConfirmed = 5,
        Incomplete = 6,
        Cancelled = 7,
        CarriedForward = 8,
        Diary = 9,
        DiaryGT7Weeks = 10,
        DoNotContactClient = 11,
        CallMonitoringCancellation = 16,
        CallMonitoringCarriedForward = 17,
        ForwardToCMAgent = 24
    }

    public enum lkpPaymentType
    {
        DebitOrder = 9,
        DirectDeposit = 10,
        EFT = 15,
        CreditCard = 16,
        Debit = 19
    }

    public enum lkpTelNumberType
    {
        Work = 1,
        Home = 2,
        Cell = 3
    }

    public enum lkpKeywordType
    {
        Bank = 1,
        AccountType = 2,
        PaymentMethod = 3,
        Gender = 4,
        INGiftOption = 5
    }

    public enum lkpLeadFeedback
    {
        NoFeedback = 11
    }

    public enum lkpConfirmationFeedback
    {
        NoFeedback = 7
    }

    public enum lkpDeclineReason
    {
        NotInterested = 1,
        SufficientCover = 2,
        Financial = 3,
        Partnersaidno = 4,
        Wontgivebanking = 5,
        Clientdied = 6,
        Clienthashadcancer = 7,
        Alreadyhaspolicy = 8,
        Duplicatesale = 9,
        Hasbroker = 10,
        Emigrating = 11,
        Nopackreceived = 12,
        Poorservice = 13,
        Unemployed = 14,
        NoContact = 15,
        Wasapolicyholderbefore = 16,
        Wantstoseedocumentation = 17,
        Religion = 18,
        Nobusinessoverphone = 19,
        Underadministration = 20,
        WantstoCancel = 21,
        HasCancelled = 22,
        MoneyBuilder = 24
    }

    public enum lkpTitle
    {
        Mr = 1,
        Mrs = 2,
        Miss = 3,
        Ms = 4,
        Dr = 5,
        LtCol = 6,
        Rev = 7,
        Prof = 8,
        Pastor = 9,
        Capt = 10,
        Sister = 11,
        Captain = 12,
        Advoc = 13,
        Reverend = 14,
        Adv = 15
    }

    /// <summary>
    /// This dictates whether the System Units Column should be included on the report
    /// </summary>
    public enum SalaryReportMode
    {
        Default = 1, // No,
        Checking = 2 // Yes
    }

    //Will most likely not be used
    //public enum LeadAnalysisReportMode
    //{
    //    ByDateRange = 1,
    //    ByCampaignAndDateRange = 2,
    //    ByCampaignAndBatches = 3,
    //    Default = 4 // In this mode, all controls are disabled, except the drop-down specifying the input parameter options
    //}

    public enum LeadAnalysisReportResultsToInclude
    {
        All = 1,
        OnlyLeadsWithFeedback = 2
    }

    public enum lkpINGiftRedeemStatus
    {
        Redeemed = 1,
        NotRedeemed = 2
    }

    public enum lkpScriptType
    {
        Closure = 1,
        ObjectionInformation = 2,
        ClaimInformation = 3,
        ExclusionInformation = 4,
        DebicheckInformation = 5
    }

    public enum lkpINCampaignGroupType
    {
        Base = 1,
        Upgrade = 2
    }

    public enum lkpINCampaignTypeGroup
    {
        Cancer = 1,
        Macc = 2
    }

    public enum CTPhoneCallStatus
    {
        Answered = 1, //Green
        Disconnected = 2, //Black
        Ringing = 3, //Blue
        Busy = 4, //Yellow
        Invalid = 5, //Red
        Held = 6 //Magenta
    }

    public enum CTPhoneAgentStatus
    {
        Ready = 1, //Green
        NotReady = 2, //Red
        Busy = 3 //Blue
    }

    public enum lkpINAccNumCheckStatus
    {
        UnChecked = 0,//No Symbol
        Valid = 1,//Green Tick Symbol
        Invalid = 2//Red X Symbol
    }

    public enum lkpINSalesNotTransferrdReason
    {
        DebiCheckAgentNotAvailable = 1,
        OvertimeSale = 2,
        DifficultClient = 3,
        DebiCheckNotApplicable = 4,
        RemoteShift = 5
    }

    public enum lkpINCampTSRReportMode
    {
        ByCampaign = 0,
        ByTSR = 1,
        ByQA = 2,
        TrainingSupervisor = 4,
        SalesCoaches = 3
    }

    public enum lkpINTurnoverCompanyMode
    {
        IG = 1,
        Insurance = 2,
        Both = 3
    }

    public enum lkpINBatchType
    {
        Normal = 0,
        Redeemed = 1,
        NonRedeemed = 2
    }

    public enum lkpINSTLBatchType
    {
        Normal = 0,
        Redeemed = 1,
        NonRedeemed = 2,
        Combined = 3
    }

    public enum lkpAgentMode
    {
        Both = 0,
        Permanent = 1,
        Temporary = 2
    }

    public enum lkpGiftClaimsImportType
    {
        Normal = 1,
        SMS = 2
    }

    public enum lkpSolutionConfiguration
    {
        Release = 1,
        Debug = 2,
        Test = 3,
        Training = 4
    }

    public enum lkpBaseSalesContactsWeeks
    {
        Weeks13 = 13,
        Weeks20 = 20,
        Weeks24 = 24
    }

    public enum lkpStaffType
    {
        Permanent = 1,
        Temporary = 2
    }

    public enum QADetailsQuestionType
    {
        CallIntro = 1,
        Pitch = 2,
        Borderline = 3,
        Admin = 4,
        Partner = 5,
        Closure = 6
    }

    public enum QADetailsAnswerType
    {
        Bit = 1,
        Int = 2,
        Date = 3,
        DateTime = 4,
        Text = 5
    }

}
