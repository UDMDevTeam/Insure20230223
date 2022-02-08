using Embriant.Framework.Configuration;
using Infragistics.Windows.Editors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Data;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class CallMonitoringViewDetailsScreen
    {

        private CallMonitoringDetailsScreen _CallMonitoringDetailsScreen;
        DataSet dsBulkNoesponseData;
        private SalesScreenGlobalData _ssGlobalData = new SalesScreenGlobalData();
        private int _funeralCover;
        string _childCover = string.Empty;
        string _la1Cover = string.Empty;
        string _totalLa1Cover = string.Empty;
        string _la2Cover = string.Empty;
        string _totalLa2Cover = string.Empty;
        private int _la1AccidentalDeathCover = 0;
        string _la1AccDeathCover = string.Empty;
        string _totalLa1AccDeathCover = string.Empty;
        string _la2AccDeathCover = string.Empty;
        string _totalLa2AccDeathCover = string.Empty;
        private int _la1FuneralCover = 0;




        readonly IEnumerable<lkpINCampaignType?> campaignTypesCancer = new lkpINCampaignType?[] { lkpINCampaignType.Cancer, lkpINCampaignType.CancerFuneral, lkpINCampaignType.IGCancer, lkpINCampaignType.TermCancer, };
        readonly IEnumerable<lkpINCampaignType?> campaignTypesMacc = new lkpINCampaignType?[] { lkpINCampaignType.Macc, lkpINCampaignType.MaccFuneral, lkpINCampaignType.MaccMillion, lkpINCampaignType.BlackMacc, lkpINCampaignType.FemaleDis, lkpINCampaignType.AccDis, lkpINCampaignType.IGFemaleDisability, lkpINCampaignType.BlackMaccMillion };
        readonly IEnumerable<lkpINCampaignType?> campaignTypesMaccNotAccDis = new lkpINCampaignType?[] { lkpINCampaignType.Macc, lkpINCampaignType.MaccFuneral, lkpINCampaignType.MaccMillion, lkpINCampaignType.BlackMacc, lkpINCampaignType.BlackMaccMillion, lkpINCampaignType.FemaleDis, lkpINCampaignType.IGFemaleDisability };


        public CallMonitoringViewDetailsScreen(CallMonitoringDetailsScreen callMonitoringDetailsScreen)
        {
            InitializeComponent();
            
            _CallMonitoringDetailsScreen = callMonitoringDetailsScreen;

            //try { tbxNotes.ItemsSource = null; } catch { }

            //dsBulkNoesponseData = Business.Insure.INGetCallMonitoringExtraDetails(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.ImportID);
            //tbxNotes.ItemsSource = new DataView(dsBulkNoesponseData.Tables[0]);

            lblFirstNameFill.Text = _CallMonitoringDetailsScreen._leadApplicationScreenData.LeadData.Name;
            lblSurNameFill.Text = _CallMonitoringDetailsScreen._leadApplicationScreenData.LeadData.Surname;


            if (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.IsLeadUpgrade == true)
            {
                PolicyDetailsBase.Visibility = Visibility.Collapsed;
                PolicyDetailsUpgrade.Visibility = Visibility.Visible;
                PopulateImportedPolicyDataUpgrade();
                CalculateCostUpgrade(true);
                //xamCETotalPremiumUpg.Value = _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.
            }
            else
            {
                PolicyDetailsBase.Visibility = Visibility.Visible;
                PolicyDetailsUpgrade.Visibility = Visibility.Collapsed;
                CalculateCost(true);
                xamCETotalPremiumAnnual.Text = (_CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.LoadedTotalPremium * 12).ToString();
                xamCEUpgradePremiumUpg.Text = _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.UpgradePremium.ToString();
            }
        }

        private void CalculateCost(bool checkBumpup)
        {
            try
            {
                if (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.ImportID == null) return;

                if (_CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.OptionID != null)
                {
                    DataTable dtOption = Methods.GetTableData("SELECT * FROM INOption WHERE ID = '" + _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.OptionID + "'");
                    DataTable dtOptionExtra = Methods.GetTableData("SELECT * FROM INOptionExtra WHERE FKOptionID = '" + _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.OptionID + "'");

                    _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium = 0;

                    if (dtOption.Rows.Count == 1)
                    {
                        decimal dtLA1Cost;
                        try { dtLA1Cost = Convert.ToDecimal(dtOption.Rows[0]["LA1Cost"]); } catch{ dtLA1Cost = 0; }

                        decimal dtLA1Cover;
                        try { dtLA1Cover = Convert.ToDecimal(dtOption.Rows[0]["LA1Cover"]); } catch { dtLA1Cover = 0; }

                        decimal dtLA2Cost;
                        try { dtLA2Cost = Convert.ToDecimal(dtOption.Rows[0]["LA2Cost"]); } catch { dtLA2Cost = 0; }

                        decimal dtLA2Cover;
                        try { dtLA2Cover = Convert.ToDecimal(dtOption.Rows[0]["LA2Cover"]); } catch { dtLA2Cover = 0; }

                        decimal LA1Cost = dtLA1Cost;
                        xamCELA1Cost.Value = LA1Cost;
                        decimal LA1Cover = dtLA1Cover;
                        xamCELA1Cover.Value = LA1Cover;
                        _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium = _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium + LA1Cost;

                        decimal LA2Cover = 0.00m;
                        if (_CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.IsLA2Checked)
                        {
                            decimal LA2Cost = dtLA2Cost;
                            xamCELA2Cost.Value = LA2Cost;
                            LA2Cover = dtLA2Cover;
                            xamCELA2Cover.Value = LA2Cover;
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium = _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium + LA2Cost;
                        }

                        decimal LA1LA2TotalCover = LA1Cover + LA2Cover;


                        decimal ChildCost = 0.00m;
                        decimal ChildCover = 0.00m;
                        if (_CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.IsChildChecked)
                        {
                            if (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.FemaleDisCancer)
                            {
                                string optionCode = dtOption.Rows[0]["OptionCode"] as string;
                                string[] disabilityCodes = { "AAA", "BBB", "CCC", "DDD", "EEE", "aaa", "bbb", "ccc", "ddd", "eee" };
                                if (disabilityCodes.Contains(optionCode.Substring(optionCode.Length - 3)))
                                {
                                    lblChildCostCover.Text = "Child (Disability)";
                                }
                                else
                                {
                                    lblChildCostCover.Text = "Child (Cancer)";
                                }
                            }
                            else
                            {
                                lblChildCostCover.Text = "Child";
                            }

                            decimal dtChildCost;
                            try { dtChildCost = Convert.ToDecimal(dtOption.Rows[0]["ChildCost"]); } catch { dtChildCost = 0; }

                            decimal dtChildCover;
                            try { dtChildCover = Convert.ToDecimal(dtOption.Rows[0]["ChildCover"]); } catch { dtChildCover = 0; }

                            ChildCost = dtChildCost;
                            xamCEChildCost.Value = ChildCost;
                            ChildCover = dtChildCover;
                            xamCEChildCover.Value = ChildCover;
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium = _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium + ChildCost;
                        }
                        else
                        {
                            lblChildCostCover.Text = "Child";
                            xamCEChildCost.Value = 0.00m;
                            xamCEChildCover.Value = 0.00m;
                        }

                        decimal FuneralCostLA1 = 0.00m;
                        decimal FuneralCoverLA1 = 0.00m;
                        decimal FuneralCostLA2 = 0.00m;
                        decimal FuneralCoverLA2 = 0.00m;
                        if (_CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.IsFuneralChecked)
                        {
                            decimal dtLA1FuneralCost;
                            try { dtLA1FuneralCost = Convert.ToDecimal(dtOption.Rows[0]["LA1FuneralCost"]); } catch { dtLA1FuneralCost = 0; }

                            decimal dtFuneralCost;
                            try { dtFuneralCost = Convert.ToDecimal(dtOption.Rows[0]["FuneralCost"]); } catch { dtFuneralCost = 0; }

                            decimal dtFuneralCover;
                            try { dtFuneralCover = Convert.ToDecimal(dtOption.Rows[0]["FuneralCover"]); } catch { dtFuneralCover = 0; }

                            decimal dtLA1FuneralCover;
                            try { dtLA1FuneralCover = Convert.ToDecimal(dtOption.Rows[0]["LA1FuneralCover"]); } catch { dtLA1FuneralCover = 0; }

                            decimal dtLA2FuneralCost;
                            try { dtLA2FuneralCost = Convert.ToDecimal(dtOption.Rows[0]["LA2FuneralCost"]); } catch { dtLA2FuneralCost = 0; }

                            decimal dtLA2FuneralCover;
                            try { dtLA2FuneralCover = Convert.ToDecimal(dtOption.Rows[0]["LA2FuneralCover"]); } catch { dtLA2FuneralCover = 0; }

                            FuneralCostLA1 = _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.FemaleDisCancer
                                ? dtLA1FuneralCost
                                : dtFuneralCost;
                            xamCEFuneralCostLA1.Value = FuneralCostLA1;

                            FuneralCoverLA1 = dtFuneralCover;
                            FuneralCoverLA1 = _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.FemaleDisCancer
                                ? dtLA1FuneralCover
                                : dtFuneralCover;
                            _funeralCover = Convert.ToInt32(dtFuneralCover.ToString());
                            xamCEFuneralCoverLA1.Value = FuneralCoverLA1;

                            _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium = _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium + FuneralCostLA1;

                            FuneralCostLA2 = dtLA2FuneralCost;
                            xamCEFuneralCostLA2.Value = FuneralCostLA2;
                            FuneralCoverLA2 = dtLA2FuneralCover;
                            _funeralCover = _funeralCover + Convert.ToInt32(dtLA2FuneralCover.ToString());

                            xamCEFuneralCoverLA2.Value = FuneralCoverLA2;
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium = _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium + FuneralCostLA2;
                        }
                        else
                        {
                            _funeralCover = 0;
                            xamCEFuneralCostLA1.Value = 0.00m;
                            xamCEFuneralCoverLA1.Value = 0.00m;
                            xamCEFuneralCostLA2.Value = 0.00m;
                            xamCEFuneralCoverLA2.Value = 0.00m;
                        }

                        //Extra Options Added from 2020-03-01
                        decimal LA1CostOther = 0.00m;
                        decimal LA1CoverOther = 0.00m;
                        decimal LA2CostOther = 0.00m;
                        decimal LA2CoverOther = 0.00m;

                        if (dtOptionExtra?.Rows.Count == 1)
                        {
                            if (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.FemaleDisCancer)
                            {
                                decimal dtLA1CancerCost;
                                try { dtLA1CancerCost = Convert.ToDecimal(dtOptionExtra.Rows[0]["LA1CancerCost"]); } catch { dtLA1CancerCost = 0; }

                                decimal dtLA1CancerCover;
                                try { dtLA1CancerCover = Convert.ToDecimal(dtOptionExtra.Rows[0]["LA1CancerCover"]); } catch { dtLA1CancerCover = 0; }

                                decimal dtLA2CancerCost;
                                try { dtLA2CancerCost = Convert.ToDecimal(dtOptionExtra.Rows[0]["LA2CancerCost"]); } catch { dtLA2CancerCost = 0; }

                                decimal dtLA2CancerCover;
                                try { dtLA2CancerCover = Convert.ToDecimal(dtOptionExtra.Rows[0]["LA2CancerCover"]); } catch { dtLA2CancerCover = 0; }

                                LA1CostOther = dtLA1CancerCost;
                                xamCELA1CostOther.Value = LA1CostOther;
                                LA1CoverOther = dtLA1CancerCover;
                                xamCELA1CoverOther.Value = LA1CoverOther;

                                LA2CostOther = dtLA2CancerCost;
                                xamCELA2CostOther.Value = LA2CostOther;
                                LA2CoverOther = dtLA2CancerCover;
                                xamCELA2CoverOther.Value = LA2CoverOther;
                            }
                            else
                            {

                            }
                        }

                        xamCETotalPremium.Value = _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium;

                        SqlParameter[] parameters = new SqlParameter[5];
                        parameters[0] = new SqlParameter("@ImportID", _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.ImportID);
                        parameters[1] = new SqlParameter("@NewOptionID", _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.OptionID);
                        parameters[2] = new SqlParameter("@NewOptionLA2", _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.IsLA2Checked);
                        parameters[3] = new SqlParameter("@NewOptionChild", _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.IsChildChecked);
                        parameters[4] = new SqlParameter("@NewPremium", _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium);
                        _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalInvoiceFee = Convert.ToDecimal(Methods.ExecuteFunction("fnGetTotalFeeByOptions", parameters));

                        #region calculate moneyback payout

                        //if (dteDateOfBirth.Value != DBNull.Value && dteDateOfBirth.DateValue != null && dteDateOfBirth.IsValueValid)
                        //{
                        //    int birthYear = ((DateTime)dteDateOfBirth.DateValue).Year;
                        //    int policyYear;

                        //    if (dteDateOfSale.DateValue != null && dteDateOfSale.IsValueValid)
                        //    {
                        //        policyYear = ((DateTime)dteDateOfSale.DateValue).Year;
                        //    }
                        //    else
                        //    {
                        //        policyYear = DateTime.Now.Year;
                        //    }

                        //    LaData.PolicyData.MoneyBackPayout = GetMoneyBackPayout(policyYear, birthYear, Convert.ToDecimal(LaData.PolicyData.TotalPremium));
                        //    LaData.PolicyData.MoneyBackPayoutAge = GetMoneyBackPayoutAge(LaData.LeadData.DateOfBirth, LaData.PolicyData.CommenceDate);
                        //}
                        //else
                        //{
                        //    LaData.PolicyData.MoneyBackPayout = 0.00m;
                        //    LaData.PolicyData.MoneyBackPayoutAge = null;
                        //}

                        #endregion

                        //calculate 50%violence benefit for macc campaigns
                        switch (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType)
                        {
                            case lkpINCampaignType.Macc:
                            case lkpINCampaignType.MaccMillion:
                            case lkpINCampaignType.AccDis:
                                xamCE50PercViolenceBenefit.Value = LA1Cover + (LA1Cover * 50 / 100);
                                xamCE50PercViolenceBenefit.Visibility = Visibility.Visible;
                                lbl50PercViolenceBenefit.Visibility = Visibility.Visible;
                                break;

                            default:
                                xamCE50PercViolenceBenefit.Value = 0.00m;
                                xamCE50PercViolenceBenefit.Visibility = Visibility.Collapsed;
                                lbl50PercViolenceBenefit.Visibility = Visibility.Collapsed;
                                break;
                        }

                        //calculate IG cancer free cover
                        switch (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignID)
                        {
                            case 3:
                            case 4:
                            case 334:
                            case 264:

                                //DataTable dt = Methods.GetTableData("SELECT IGFreeCover FROM INPlan WHERE ID = '" + LaData.PolicyData.PlanID + "'");
                                //xamCEIGLA1FreeCover.Value = dt.Rows.Count > 0 ? Convert.ToDecimal(dt.Rows[0]["IGFreeCover"]) : 0.00m;

                                //if (cmbLA1Cover.SelectedIndex == 0) //only show free cover if it is the higher option
                                //{
                                //    xamCEIGLA1FreeCover.Visibility = Visibility.Visible;
                                //    lblIGLA1FreeCover.Visibility = Visibility.Visible;
                                //}
                                //else
                                //{
                                //    xamCEIGLA1FreeCover.Value = 0.00m;
                                //    xamCEIGLA1FreeCover.Visibility = Visibility.Collapsed;
                                //    lblIGLA1FreeCover.Visibility = Visibility.Collapsed;
                                //}
                                //break;

                            default:
                                xamCEIGLA1FreeCover.Value = 0.00m;
                                xamCEIGLA1FreeCover.Visibility = Visibility.Collapsed;
                                lblIGLA1FreeCover.Visibility = Visibility.Collapsed;
                                break;
                        }

                        //Show or Hide Base Cover Lines
                        grdBaseLine1.Visibility = LA1Cover > 1 ? Visibility.Visible : Visibility.Collapsed;
                        grdBaseLine2.Visibility = LA2Cover > 1 ? Visibility.Visible : Visibility.Collapsed;
                        grdBaseLine3.Visibility = LA1CoverOther > 1 ? Visibility.Visible : Visibility.Collapsed;
                        grdBaseLine4.Visibility = LA2CoverOther > 1 ? Visibility.Visible : Visibility.Collapsed;
                        grdBaseLine5.Visibility = ChildCover > 1 ? Visibility.Visible : Visibility.Collapsed;

                        if (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.FemaleDisCancer || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMacc || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.FemaleDis)
                        {
                            lblLA1CostCover.Text = "Life Assured 1 (Disability)";
                            lblLA2CostCover.Text = "Life Assured 2 (Disability)";
                            lblLA1CostCoverOther.Text = "Life Assured 1 (Cancer)";
                            lblLA2CostCoverOther.Text = "Life Assured 2 (Cancer)";

                            _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium = _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium + LA1CostOther + LA2CostOther;

                            LA1LA2TotalCover = LA1LA2TotalCover + LA1CoverOther + LA2CoverOther; // + FuneralCoverLA1 + FuneralCoverLA2
                        }
                        else
                        {
                            lblLA1CostCover.Text = "Life Assured 1";
                            lblLA2CostCover.Text = "Life Assured 2";
                            lblLA1CostCoverOther.Text = "Life Assured 1";
                            lblLA2CostCoverOther.Text = "Life Assured 2";
                        }

                        //Funeral hide/show
                        if (FuneralCoverLA1 > 1)
                        {
                            lblFuneralCostCoverLA1.Visibility = Visibility.Visible;
                            xamCEFuneralCostLA1.Visibility = Visibility.Visible;
                            xamCEFuneralCoverLA1.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            lblFuneralCostCoverLA1.Visibility = Visibility.Collapsed;
                            xamCEFuneralCostLA1.Visibility = Visibility.Collapsed;
                            xamCEFuneralCoverLA1.Visibility = Visibility.Collapsed;
                        }

                        if (FuneralCoverLA2 > 1)
                        {
                            lblFuneralCostCoverLA2.Visibility = Visibility.Visible;
                            xamCEFuneralCostLA2.Visibility = Visibility.Visible;
                            xamCEFuneralCoverLA2.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            lblFuneralCostCoverLA2.Visibility = Visibility.Collapsed;
                            xamCEFuneralCostLA2.Visibility = Visibility.Collapsed;
                            xamCEFuneralCoverLA2.Visibility = Visibility.Collapsed;
                        }

                        xamCELA1LA2TotalCover.Value = LA1LA2TotalCover;
                    }
                }
                else
                {
                    //ResetDisplayedCosts();
                }

                //if (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.IsLeadLoaded && checkBumpup) CalculateBumpUpOrReducedPremium();

                //xamCETotalPremiumAnnual.Text = (LaData.PolicyData.TotalPremium * 12).ToString();

            }

            catch (Exception ex)
            {
                //HandleException(ex);
            }

        }

        private void CalculateCostUpgrade(bool checkBumpUp)
        {
            try
            {
                if (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.IsLeadSaving || !_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.IsLeadLoaded) return;

                #region Initialize

                for (int row = 1; row <= 6; row++)
                {
                    for (int column = 0; column <= 10; column++)
                    {
                        UIElement test = grdPolicyDetailsUpgrade.Children.Cast<UIElement>().FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
                        grdPolicyDetailsUpgrade.Children.Remove(test);
                    }
                }

                _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.UpgradePremium = null;
                _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium = null;

                #endregion

                if (_CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.OptionID != null)
                {
                    DataTable dtOption = Methods.GetTableData("SELECT * FROM INOption WHERE ID = '" + _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.OptionID + "'");
                    //DataTable dtOptionFees = Methods.GetTableData("SELECT * FROM INOptionFees WHERE FKINOptionID = '" + LaData.PolicyData.Option)

                    _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalPremium = 0;
                    int gridRow = 1;
                    FrameworkElement control;

                    if (grdPolicyDetailsUpgradeA.RowDefinitions.Count > 1)
                    {
                        grdPolicyDetailsUpgradeA.RowDefinitions.RemoveRange(1, grdPolicyDetailsUpgradeA.RowDefinitions.Count - 1);
                    }
                    if (grdPolicyDetailsUpgradeA.Children.Count > 2)
                    {
                        grdPolicyDetailsUpgradeA.Children.RemoveRange(2, grdPolicyDetailsUpgradeA.Children.Count - 2);
                    }

                    #region LA1 Cover
                    decimal dtLA1Cover;
                    try { dtLA1Cover = Convert.ToInt32(dtOption.Rows[0]["LA1Cover"]); } catch { dtLA1Cover = 0; }
                    if (dtLA1Cover != 0)
                    {
                        grdPolicyDetailsUpgradeA.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(21) }); // continue with the scrollviewer when needed
                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgradeA, gridRow, 0, 6);
                        if ((_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                                                    &&
                                                    (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                                                    ||
                                                    _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                                                    ||
                                                    _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                                                    ||
                                                    _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8
                                                    ||
                                                    (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.Cancer
                                                    &&
                                                    _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade9)

                                                    )))

                        {
                            ((TextBlock)control).Text = "LA1 Cancer";
                        }
                        else if (campaignTypesCancer.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType) || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                        {
                            ((TextBlock)control).Text = "LA1 Cancer";
                        }
                        else if (campaignTypesMacc.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType))
                        {
                            ((TextBlock)control).Text = "LA1 Disability";
                        }
                        else
                        {
                            ((TextBlock)control).Text = "LA1";
                        }

                        //if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA1Cost"], 0)) != 0)
                        //{
                        //    control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 4, 3);
                        //    ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA1Cost"]).ToString();
                        //}

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgradeA, gridRow, 7, 4);
                        ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA1Cover"]).ToString(CultureInfo.CurrentCulture);

                        _la1Cover = Convert.ToDecimal(dtOption.Rows[0]["LA1Cover"]).ToString(CultureInfo.CurrentCulture);
                        gridRow++;

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);
                        if ((_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                            &&
                            (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.Cancer
                            &&
                            (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade9

                            )
                            )
                            ))

                        {
                            ((TextBlock)control).Text = "Total LA1 Cancer";
                        }
                        else if (campaignTypesCancer.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType) || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                        {
                            ((TextBlock)control).Text = "Total LA1 Cancer";
                        }
                        else if (campaignTypesMacc.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType))
                        {
                            ((TextBlock)control).Text = "Total LA1 Disability";
                        }
                        else
                        {
                            ((TextBlock)control).Text = "Total LA1";
                        }
                        //else
                        //{
                        //    ((TextBlock)control).Text = campaignTypesCancer.Contains(LaData.AppData.CampaignType) ? "Total LA1 Cancer" : "Total LA1";
                        //    ((TextBlock)control).Text = campaignTypesMacc.Contains(LaData.AppData.CampaignType) ? "Total LA1 Acc Disability" : "Total LA1";
                        //}

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        decimal total = 0.00m;
                        if ((_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                                                    &&
                                                    (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                                                    ||
                                                    _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                                                    ||
                                                    _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                                                    ||
                                                    _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8
                                                    ||
                                                    (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.Cancer
                                                    &&
                                                    (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade9
                                                    )
                                                    ))))

                        {
                            total = Convert.ToDecimal(dtOption.Rows[0]["LA1Cover"]) + Convert.ToDecimal(_CallMonitoringDetailsScreen._leadApplicationScreenData.ImportedCovers[0].Cover);
                        }
                        else if (campaignTypesCancer.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType) || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                        {
                            total = Convert.ToDecimal(dtOption.Rows[0]["LA1Cover"]) + Convert.ToDecimal(_CallMonitoringDetailsScreen._leadApplicationScreenData.ImportedCovers[0].Cover);
                        }
                        else if (campaignTypesMacc.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType))
                        {
                            total = Convert.ToDecimal(dtOption.Rows[0]["LA1Cover"]) + Convert.ToDecimal(_CallMonitoringDetailsScreen._leadApplicationScreenData.ImportedCovers[1].Cover);
                        }
                        ((XamCurrencyEditor)control).Text = total.ToString(CultureInfo.CurrentCulture);
                        _totalLa1Cover = total.ToString(CultureInfo.CurrentCulture);
                        gridRow++;
                    }
                    else
                    {
                        _la1Cover = "0.0";
                        _totalLa1Cover = "0.0";
                    }

                    #endregion

                    #region LA2 Cover

                    int? dtOptionLA2Cover;
                    try
                    {
                        dtOptionLA2Cover = Convert.ToInt32(dtOption.Rows[0]["LA2Cover"]);
                    }
                    catch
                    {
                        dtOptionLA2Cover = 0;
                    }

                    if (dtOptionLA2Cover != 0)
                    {
                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 4);
                        if ((_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                            &&
                            (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.Cancer
                            &&
                            (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade9

                            )
                            )))
                        {
                            ((TextBlock)control).Text = "LA2 Cancer";
                        }


                        else if (campaignTypesCancer.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType) || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                        {
                            ((TextBlock)control).Text = "LA2 Cancer";
                        }
                        else if (campaignTypesMacc.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType))
                        {
                            ((TextBlock)control).Text = "LA2 Disability";
                        }
                        else
                        {
                            ((TextBlock)control).Text = "LA2";
                        }
                        //else
                        //{
                        //    ((TextBlock)control).Text = campaignTypesCancer.Contains(LaData.AppData.CampaignType) ? "LA2 Cancer" : "LA2";
                        //    ((TextBlock)control).Text = campaignTypesMacc.Contains(LaData.AppData.CampaignType) ? "LA2 Acc Disability" : "LA2";
                        //}

                        //if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA2Cost"], 0)) != 0)
                        //{
                        //    control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 4, 3);
                        //    ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA2Cost"]).ToString();
                        //}

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA2Cover"]).ToString(CultureInfo.CurrentCulture);

                        _la2Cover = Convert.ToDecimal(dtOption.Rows[0]["LA2Cover"]).ToString(CultureInfo.CurrentCulture);
                        gridRow++;

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);
                        if ((_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                            &&
                            (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.Cancer
                            &&
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade9

                       )
                       ))
                        {
                            ((TextBlock)control).Text = "Total LA2 Cancer";
                        }
                        else if (campaignTypesCancer.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType) || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                        {
                            ((TextBlock)control).Text = "Total LA2 Cancer";
                        }
                        else if (campaignTypesMacc.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType))
                        {
                            ((TextBlock)control).Text = "Total LA2 Disability";
                        }
                        else
                        {
                            ((TextBlock)control).Text = "Total LA2";
                        }
                        //else
                        //{
                        //    ((TextBlock)control).Text = campaignTypesCancer.Contains(LaData.AppData.CampaignType) ? "Total LA2 Cancer" : "Total LA2";
                        //    ((TextBlock)control).Text = campaignTypesMacc.Contains(LaData.AppData.CampaignType) ? "Total LA2 Acc Disability" : "Total LA2";
                        //}

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        decimal total = 0.00m;
                        if ((_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                            &&
                            (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.Cancer
                            &&
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade9

                            )
                            ))
                        {
                            total = Convert.ToDecimal(dtOption.Rows[0]["LA2Cover"]) + Convert.ToDecimal(_CallMonitoringDetailsScreen._leadApplicationScreenData.ImportedCovers[4].Cover);
                        }
                        else if (campaignTypesCancer.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType) || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                        {
                            total = Convert.ToDecimal(dtOption.Rows[0]["LA2Cover"]) + Convert.ToDecimal(_CallMonitoringDetailsScreen._leadApplicationScreenData.ImportedCovers[4].Cover);
                        }
                        else if (campaignTypesMacc.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType))
                        {
                            total = Convert.ToDecimal(dtOption.Rows[0]["LA2Cover"]) + Convert.ToDecimal(_CallMonitoringDetailsScreen._leadApplicationScreenData.ImportedCovers[5].Cover);
                        }
                        ((XamCurrencyEditor)control).Text = total.ToString(CultureInfo.CurrentCulture);
                        _totalLa2Cover = total.ToString(CultureInfo.CurrentCulture);
                        gridRow++;
                    }
                    else
                    {
                        _la2Cover = "0.0";
                        _totalLa2Cover = "0.0";
                    }

                    #endregion

                    #region LA1 Death Cover
                    int dtLA1AccidentalDeathCover;

                    try
                    {
                        dtLA1AccidentalDeathCover = Convert.ToInt32(dtOption.Rows[0]["LA1AccidentalDeathCover"]);
                    }
                    catch
                    {
                        dtLA1AccidentalDeathCover = 0;
                    }

                    if (dtLA1AccidentalDeathCover != 0)
                    {
                        _la1AccidentalDeathCover = dtLA1AccidentalDeathCover;

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 4);
                        ((TextBlock)control).Text = "LA1 Acc Death";

                        //if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA1AccidentalDeathCost"], 0)) != 0)
                        //{
                        //    control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 4, 3);
                        //    ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA1AccidentalDeathCost"]).ToString();
                        //}

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA1AccidentalDeathCover"]).ToString(CultureInfo.CurrentCulture);
                        _la1AccDeathCover = Convert.ToDecimal(dtOption.Rows[0]["LA1AccidentalDeathCover"]).ToString(CultureInfo.CurrentCulture);
                        gridRow++;

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);
                        ((TextBlock)control).Text = "Total LA1 Acc Death";

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        var total = Convert.ToDecimal(dtOption.Rows[0]["LA1AccidentalDeathCover"]) + Convert.ToDecimal(_CallMonitoringDetailsScreen._leadApplicationScreenData.ImportedCovers[2].Cover);
                        ((XamCurrencyEditor)control).Text = total.ToString(CultureInfo.CurrentCulture);
                        _totalLa1AccDeathCover = total.ToString(CultureInfo.CurrentCulture);
                        gridRow++;
                    }
                    else
                    {
                        _la1AccidentalDeathCover = 0;
                        _la1AccDeathCover = "0.0";
                        _totalLa1AccDeathCover = "0.0";
                    }

                    #endregion

                    #region LA2 Death Cover
                    int dtLA2AccidentalDeathCover;
                    try
                    {
                        dtLA2AccidentalDeathCover = Convert.ToInt32(dtOption.Rows[0]["LA2AccidentalDeathCover"]);
                    }
                    catch
                    {
                        dtLA2AccidentalDeathCover = 0;
                    }
                    
                    
                    if (dtLA2AccidentalDeathCover != 0)
                    {
                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 4);
                        ((TextBlock)control).Text = "LA2 Acc Death";

                        //if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA2AccidentalDeathCost"], 0)) != 0)
                        //{
                        //    control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 4, 3);
                        //    ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA2AccidentalDeathCost"]).ToString();
                        //}

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA2AccidentalDeathCover"]).ToString(CultureInfo.CurrentCulture);
                        _la2AccDeathCover = Convert.ToDecimal(dtOption.Rows[0]["LA2AccidentalDeathCover"]).ToString(CultureInfo.CurrentCulture);
                        gridRow++;

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);
                        ((TextBlock)control).Text = "Total LA2 Acc Death";

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        var total = Convert.ToDecimal(dtOption.Rows[0]["LA2AccidentalDeathCover"]) + Convert.ToDecimal(_CallMonitoringDetailsScreen._leadApplicationScreenData.ImportedCovers[6].Cover);
                        ((XamCurrencyEditor)control).Text = total.ToString(CultureInfo.CurrentCulture);
                        _totalLa2AccDeathCover = total.ToString(CultureInfo.CurrentCulture);
                        gridRow++;
                    }
                    else
                    {
                        _la2AccDeathCover = "0.0";
                        _totalLa2AccDeathCover = "0.0";
                    }

                    #endregion

                    #region Funeral Cover
                    int dtFuneralCover;

                    try
                    {
                        dtFuneralCover = Convert.ToInt32(dtOption.Rows[0]["FuneralCover"]);
                    }
                    catch
                    {
                        dtFuneralCover = 0;
                    }

                    if (dtFuneralCover != 0)
                    {
                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 4);
                        ((TextBlock)control).Text = "Funeral";

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["FuneralCover"]).ToString(CultureInfo.CurrentCulture);

                        gridRow++;

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);
                        ((TextBlock)control).Text = "Total Funeral";

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        var total = Convert.ToDecimal(dtOption.Rows[0]["FuneralCover"]) + Convert.ToDecimal(_CallMonitoringDetailsScreen._leadApplicationScreenData.ImportedCovers[3].Cover);
                        ((XamCurrencyEditor)control).Text = total.ToString(CultureInfo.CurrentCulture);

                        gridRow++;
                    }
                    #endregion Funeral Cover

                    #region LA1 Funeral cover

                    int dtLA1FuneralCover;

                    try
                    {
                        dtLA1FuneralCover = Convert.ToInt32(dtOption.Rows[0]["LA1FuneralCover"]);
                    }
                    catch
                    {
                        dtLA1FuneralCover = 0;
                    }

                    if (dtLA1FuneralCover != 0)
                    {
                        _la1FuneralCover = Convert.ToInt32(dtOption.Rows[0]["LA1FuneralCover"]);

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 4);
                        ((TextBlock)control).Text = "LA1 Funeral";

                        //if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA1AccidentalDeathCost"], 0)) != 0)
                        //{
                        //    control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 4, 3);
                        //    ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA1AccidentalDeathCost"]).ToString();
                        //}

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA1FuneralCover"]).ToString(CultureInfo.CurrentCulture);

                        gridRow++;

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);
                        ((TextBlock)control).Text = "Total LA1 Funeral";

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        var total = Convert.ToDecimal(dtOption.Rows[0]["LA1FuneralCover"]) + Convert.ToDecimal(_CallMonitoringDetailsScreen._leadApplicationScreenData.ImportedCovers[3].Cover);
                        ((XamCurrencyEditor)control).Text = total.ToString(CultureInfo.CurrentCulture);

                        gridRow++;
                    }
                    else
                    {
                        _la1FuneralCover = 0;
                    }

                    #endregion

                    #region LA2 Funeral cover

                    int dtLA2FuneralCover;

                    try
                    {
                        dtLA2FuneralCover = Convert.ToInt32(dtOption.Rows[0]["LA2FuneralCover"]);
                    }
                    catch
                    {
                        dtLA2FuneralCover = 0;
                    }

                    if (dtLA2FuneralCover != 0)
                    {
                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 4);
                        ((TextBlock)control).Text = "LA2 Funeral";

                        //if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA1AccidentalDeathCost"], 0)) != 0)
                        //{
                        //    control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 4, 3);
                        //    ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA1AccidentalDeathCost"]).ToString();
                        //}

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA2FuneralCover"]).ToString(CultureInfo.CurrentCulture);

                        gridRow++;

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);
                        ((TextBlock)control).Text = "Total LA2 Funeral";

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        var total = Convert.ToDecimal(dtOption.Rows[0]["LA2FuneralCover"]) + Convert.ToDecimal(_CallMonitoringDetailsScreen._leadApplicationScreenData.ImportedCovers[3].Cover);
                        ((XamCurrencyEditor)control).Text = total.ToString(CultureInfo.CurrentCulture);

                        gridRow++;
                    }

                    #endregion

                    #region Child Cover

                    if (_CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.IsChildUpgradeChecked)
                    {
                        int dtChildCover;
                        try
                        {
                            dtChildCover = Convert.ToInt32(dtOption.Rows[0]["ChildCover"]);
                        }
                        catch
                        {
                            dtChildCover = 0;
                        }

                        if (dtChildCover != 0)
                        {
                            control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);

                            if ((_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                            &&
                            (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8)
                            ))
                            {
                                ((TextBlock)control).Text = "Child Cancer";
                            }
                            else if (campaignTypesCancer.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType) || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                            {
                                ((TextBlock)control).Text = "Child Cancer";
                            }
                            else if (campaignTypesMacc.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType))
                            {
                                ((TextBlock)control).Text = "Child Disability";
                            }
                            else
                            {
                                ((TextBlock)control).Text = "Child";
                            }

                            //if (Convert.ToInt32(NoNull(dtOption.Rows[0]["ChildCost"], 0)) != 0)
                            //{
                            //    control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 4, 3);
                            //    ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["ChildCost"]).ToString();
                            //}

                            control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                            ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["ChildCover"]).ToString(CultureInfo.CurrentCulture);

                            //Child Premium
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.UpgradeChildPremium = Convert.ToDecimal(dtOption.Rows[0]["ChildCost"]);


                            _childCover = Convert.ToDecimal(dtOption.Rows[0]["ChildCover"]).ToString(CultureInfo.CurrentCulture);
                            gridRow++;

                            control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);
                            //((TextBlock)control).Text = "Total Child";
                            if ((_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                            &&
                            (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                            ||
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8)
                            ))
                            {
                                ((TextBlock)control).Text = "Total Child Cancer";
                            }
                            else if (campaignTypesCancer.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType) || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                            {
                                ((TextBlock)control).Text = "Total Child Cancer";
                            }
                            else if (campaignTypesMacc.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType))
                            {
                                ((TextBlock)control).Text = "Total Child Disability";
                            }
                            else
                            {
                                ((TextBlock)control).Text = "Total Child";
                            }

                            control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                            decimal total = 0.00m;

                            if (campaignTypesCancer.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType))
                            {
                                total = Convert.ToDecimal(dtOption.Rows[0]["ChildCover"]) + Convert.ToDecimal(_CallMonitoringDetailsScreen._leadApplicationScreenData.ImportedCovers[8].Cover);
                            }
                            else if (campaignTypesMacc.Contains(_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType))
                            {
                                if ((_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                                    &&
                                    (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                                    ||
                                    _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                                    ||
                                    _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                                    ||
                                    _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8)
                                    ))
                                {
                                    total = Convert.ToDecimal(dtOption.Rows[0]["ChildCover"]) + Convert.ToDecimal(_CallMonitoringDetailsScreen._leadApplicationScreenData.ImportedCovers[8].Cover);
                                }
                                else
                                {
                                    total = Convert.ToDecimal(dtOption.Rows[0]["ChildCover"]) + Convert.ToDecimal(_CallMonitoringDetailsScreen._leadApplicationScreenData.ImportedCovers[9].Cover);
                                }
                            }

                            ((XamCurrencyEditor)control).Text = total.ToString(CultureInfo.CurrentCulture);

                        }
                        else
                        {
                            _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.IsChildUpgradeChecked = false;
                            _childCover = "0.0";
                        }
                    }
                    else
                    {
                        _childCover = "0.0";
                    }

                    #endregion

                    #region Total Upgrade Premium

                    _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.UpgradePremium = _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.IsChildUpgradeChecked ? dtOption.Rows[0]["TotalPremium2"] as decimal? : dtOption.Rows[0]["TotalPremium1"] as decimal?;

                    SqlParameter[] parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@ImportID", _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.ImportID);
                    parameters[1] = new SqlParameter("@NewOptionID", _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.OptionID);
                    parameters[2] = new SqlParameter("@NewOptionLA2", _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.IsLA2Checked);
                    parameters[3] = new SqlParameter("@NewOptionChild", _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.IsChildChecked);
                    parameters[4] = new SqlParameter("@NewPremium", _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.UpgradePremium);
                    _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.TotalInvoiceFee = Convert.ToDecimal(Methods.ExecuteFunction("fnGetTotalFeeByOptions", parameters));

                    #endregion

                    #region Total Premium

                    xamCETotalPremiumUpg.Text = (_CallMonitoringDetailsScreen._leadApplicationScreenData.ImportedPolicyData.ContractPremium + _CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.UpgradePremium).ToString();
                    xamCEUpgradePremiumUpg.Text = (_CallMonitoringDetailsScreen._leadApplicationScreenData.PolicyData.UpgradePremium).ToString();
                    #endregion

                    #region Moneyback Payout

                    //if (dteDateOfBirth.Value != DBNull.Value && dteDateOfBirth.DateValue != null && dteDateOfBirth.IsValueValid)
                    //{
                    //    int birthYear = ((DateTime)dteDateOfBirth.DateValue).Year;
                    //    int policyYear;

                    //    if (dteDateOfSale.DateValue != null && dteDateOfSale.IsValueValid)
                    //    {
                    //        policyYear = ((DateTime)dteDateOfSale.DateValue).Year;
                    //    }
                    //    else
                    //    {
                    //        policyYear = DateTime.Now.Year;
                    //    }

                    //    xamCEMoneyBackUpg.Value = GetMoneyBackPayout(policyYear, birthYear, LaData.PolicyData.TotalPremium);
                    //}
                    //else
                    //{
                    //    xamCEMoneyBackUpg.Value = 0.00m;
                    //}

                    #endregion

                }

                //if (checkBumpUp)
                //{
                //    CalculateBumpUpOrReducedPremium();
                //}
            }

            catch (Exception ex)
            {
                //HandleException(ex);
            }
        }

        private FrameworkElement CreateControl(Type type, string strStyle, Grid grid, int row, int column, int columnSpan)
        {
            FrameworkElement control = (FrameworkElement)Activator.CreateInstance(type);
            Style style = new Style(type);
            style.BasedOn = (Style)FindResource(strStyle);
            control.Style = style;
            grid.Children.Add(control);
            control.SetValue(Grid.RowProperty, row);
            control.SetValue(Grid.ColumnProperty, column);
            control.SetValue(Grid.ColumnSpanProperty, columnSpan);

            return control;
        }

        private void PopulateImportedPolicyDataUpgrade()
        {
            try
            {
                int gridRow = 1;
                int maxRow = 5;

                if (_CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignCode == "PLDMM6U" || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignCode == "PLDMM5U" || _CallMonitoringDetailsScreen._leadApplicationScreenData.AppData.CampaignCode == "PLDMM7U")
                {
                    maxRow = 8;
                }
                FrameworkElement control;

                #region Initialize

                for (int row = 1; row <= maxRow; row++)
                {
                    for (int column = 13; column <= 23; column++)
                    {
                        UIElement test = grdPolicyDetailsUpgrade.Children.Cast<UIElement>().FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
                        grdPolicyDetailsUpgrade.Children.Remove(test);
                    }
                }

                #endregion

                foreach (var importedCover in _CallMonitoringDetailsScreen._leadApplicationScreenData.ImportedCovers)
                {
                    if (importedCover.Cover != null && importedCover.Premium != null)
                    {
                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 13, 4);
                        ((TextBlock)control).Text = importedCover.Name;

                        //control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 17, 3);
                        //((XamCurrencyEditor)control).Text = importedCover.Premium.ToString();

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 20, 4);
                        ((XamCurrencyEditor)control).Text = importedCover.Cover.ToString();

                        if (gridRow < maxRow) { gridRow++; } else { break; }
                    }
                }
            }

            catch (Exception ex)
            {
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //OnDialogClose(_dialogResult);
        }

    }
}
