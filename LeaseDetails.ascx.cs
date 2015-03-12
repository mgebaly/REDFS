using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using REDFS_WebApp2.REDFSServices;
using Telerik.Web.UI;
using System.Data;

namespace REDFS_WebApp2
{
    public partial class LeaseDetails :BaseControl
    {
        public int PlanningScenarioId
        {
            get
            {
                if (ViewState["PlanningScenarioId"] != null)
                {
                    return Convert.ToInt32(ViewState["PlanningScenarioId"]);
                }
                else
                {
                    return 0;
                }
            }
            set { ViewState["PlanningScenarioId"] = value; }
        }
        public int UserId { get; set; }
        void bindddlDurationUnit(RadComboBox ddlLeaseTermUnit, RadComboBox ddlPeriodicPaymentTermUnit, RadComboBox ddlLeaseEsclationTermUnit)
        {
            List<DurationUnit_GetAll_Result> listDurationUnits = DurationUnitProxy.GetAllDurationUnits();
            ddlLeaseTermUnit.DataSource = listDurationUnits;
            ddlLeaseTermUnit.DataValueField = "DurationUnitId";
            ddlLeaseTermUnit.DataTextField = "DurationUnitName";
            ddlLeaseTermUnit.DataBind();
            ddlPeriodicPaymentTermUnit.DataSource = listDurationUnits;
            ddlPeriodicPaymentTermUnit.DataValueField = "DurationUnitId";
            ddlPeriodicPaymentTermUnit.DataTextField = "DurationUnitName";
            ddlPeriodicPaymentTermUnit.DataBind();
            ddlLeaseEsclationTermUnit.DataSource = listDurationUnits;
            ddlLeaseEsclationTermUnit.DataValueField = "DurationUnitId";
            ddlLeaseEsclationTermUnit.DataTextField = "DurationUnitName";
            ddlLeaseEsclationTermUnit.DataBind();
        }
        void bindddlEsclationMethod(RadComboBox ddlEsclationMethod)
        {
            ddlEsclationMethod.DataSource = InterestTypeProxy.GetAllInterestTypes();
            ddlEsclationMethod.DataValueField = "InterestTypeId";
            ddlEsclationMethod.DataTextField = "InterestTypeName";
            ddlEsclationMethod.DataBind();
            string ddlZero = GetGlobalResourceObject("GlobalResources", "ddlZero").ToString();
            ddlEsclationMethod.Items.Insert(0, new RadComboBoxItem { Value = "0", Text = ddlZero });
        }
        void bindfvLeaseDetails(int scenarioId)
        {
            fvLeaseDetails.DataSource = LeaseDetailProxy.GetLeaseDetailsByPlanningScenarioId_Detail(scenarioId);
            fvLeaseDetails.DataBind();
        }
        public void LoadLeaseDetails()
        {
            fvLeaseDetails.Visible = true;
            fvLeaseDetails.ChangeMode(FormViewMode.ReadOnly);
            bindfvLeaseDetails(PlanningScenarioId);
        }
        public void ClearLeaseDetails()
        {
            fvLeaseDetails.Visible = false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void fvLeaseDetails_DataBound(object sender, EventArgs e)
        {
            RadComboBox ddlLeaseTermUnit = fvLeaseDetails.FindControl("ddlLeaseTermUnit") as RadComboBox;
            RadComboBox ddlPeriodicPaymentTermUnit = fvLeaseDetails.FindControl("ddlPeriodicPaymentTermUnit") as RadComboBox;
            RadComboBox ddlLeaseEsclationTermUnit = fvLeaseDetails.FindControl("ddlLeaseEsclationTermUnit") as RadComboBox;
            RadComboBox ddlEsclationMethod = fvLeaseDetails.FindControl("ddlEsclationMethod") as RadComboBox;
            switch (fvLeaseDetails.CurrentMode)
            {
                case FormViewMode.Edit:
                    Label lblLeaseTermUnit = fvLeaseDetails.FindControl("lblLeaseTermUnit") as Label;
                    Label lblPeriodicPaymentTermUnit = fvLeaseDetails.FindControl("lblPeriodicPaymentTermUnit") as Label;
                    Label lblLeaseEsclationTermUnit = fvLeaseDetails.FindControl("lblLeaseEsclationTermUnit") as Label;
                    Label lblEsclationMethod = fvLeaseDetails.FindControl("lblEsclationMethod") as Label;
                    string strLeaseTermUnit = lblLeaseTermUnit.Text;
                    string strPeriodicPaymentTermUnit = lblPeriodicPaymentTermUnit.Text;
                    string strLeaseEsclationTermUnit = lblLeaseEsclationTermUnit.Text;
                    string strEsclationMethod = lblEsclationMethod.Text;
                    bindddlDurationUnit(ddlLeaseTermUnit, ddlPeriodicPaymentTermUnit, ddlLeaseEsclationTermUnit);
                    bindddlEsclationMethod(ddlEsclationMethod);
                    ddlLeaseTermUnit.SelectedValue = strLeaseTermUnit;
                    ddlPeriodicPaymentTermUnit.SelectedValue = strPeriodicPaymentTermUnit;
                    ddlLeaseEsclationTermUnit.SelectedValue = strLeaseEsclationTermUnit;
                    ddlEsclationMethod.SelectedValue = strEsclationMethod;
                    break;
            }
            if (fvLeaseDetails.DataItemCount == 0)
            {
                bindddlDurationUnit(ddlLeaseTermUnit, ddlPeriodicPaymentTermUnit, ddlLeaseEsclationTermUnit);
                bindddlEsclationMethod(ddlEsclationMethod);
            }
        }

        protected void fvLeaseDetails_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            switch (e.NewMode)
            {
                case FormViewMode.Edit:
                    fvLeaseDetails.ChangeMode(FormViewMode.Edit);
                    bindfvLeaseDetails(PlanningScenarioId);
                    break;
                case FormViewMode.Insert:
                    break;
                case FormViewMode.ReadOnly:
                    fvLeaseDetails.ChangeMode(FormViewMode.ReadOnly);
                    bindfvLeaseDetails(PlanningScenarioId);
                    break;
            }
        }

        void RecordLandAcquisitionCostLeased(int leaseTerm, int periodicPaymentTerm, DateTime leaseStartDate, decimal initialPeriodicPayment, int leaseEsclationTerm, double esclationRate, int esclationMethod, decimal securityDeposit)
        {
            DataTable CostTbl = new DataTable("CashFlowsLongTermAssets");
            CostTbl.Columns.Add("PlanningScenarioId");
            CostTbl.Columns.Add("Definition");
            CostTbl.Columns.Add("DateRecorded");
            CostTbl.Columns.Add("LandAcquisitionCostLeased");

            int intPaymentsCount = leaseTerm / periodicPaymentTerm;
            DateTime dtDateRecorded = leaseStartDate.Date;
            decimal decLeasePayCost = initialPeriodicPayment;
            for (int i = 0; i < intPaymentsCount; i++)
            {
                string strDefinition = string.Format("{0} #{1}", GetLocalResourceObject("CostDefinition").ToString(), (i + 1).ToString());
                DataRow row = CostTbl.NewRow();
                row["PlanningScenarioId"] = PlanningScenarioId;
                row["Definition"] = strDefinition;
                row["DateRecorded"] = dtDateRecorded.ToString("yyyy-MM-dd HH:mm:ss.fff");
                row["LandAcquisitionCostLeased"] = decLeasePayCost;
                CostTbl.Rows.Add(row);
                dtDateRecorded = dtDateRecorded.AddYears(periodicPaymentTerm);
            }
            if (leaseEsclationTerm > 0)
            {
                DateTime dtEsclateDate = leaseStartDate.Date.AddYears(leaseEsclationTerm);
                switch (esclationMethod)
                {
                    case 1:  //simple interest
                        decimal decInterest = decLeasePayCost * Convert.ToDecimal(esclationRate);
                        decimal decIncrementInterest = 0;
                        foreach (DataRow row in CostTbl.Rows)
                        {
                            DateTime dateRecorded = Convert.ToDateTime(row["DateRecorded"]);
                            decimal decLeasePayCostpInterest = 0;
                            if (dateRecorded >= dtEsclateDate)
                            {
                                decIncrementInterest += decInterest;
                                decLeasePayCostpInterest = decLeasePayCost + decIncrementInterest;
                                dtEsclateDate = dtEsclateDate.Date.AddYears(leaseEsclationTerm);   
                            }
                            else
                            {
                                decLeasePayCostpInterest = decLeasePayCost + decIncrementInterest;
                            }
                            row["LandAcquisitionCostLeased"] = decLeasePayCostpInterest;
                        }
                        break;
                    case 2:  //compound interest
                        double dblEsclateNo = 0;
                        foreach (DataRow row in CostTbl.Rows)
                        {
                            DateTime dateRecorded = Convert.ToDateTime(row["DateRecorded"]);
                            decimal decLeasePayCostpInterestComp = 0;
                            if (dateRecorded >= dtEsclateDate)
                            {
                                dblEsclateNo++;
                                //amountPlusInterest"compound" = principleAmount * (interestRate + 1)^no. of payment
                                decLeasePayCostpInterestComp = decLeasePayCost * Convert.ToDecimal(Math.Pow(esclationRate + 1, dblEsclateNo));
                                dtEsclateDate = dtEsclateDate.Date.AddYears(leaseEsclationTerm); 
                            }
                            else
                            {
                                decLeasePayCostpInterestComp = decLeasePayCost * Convert.ToDecimal(Math.Pow(esclationRate + 1, dblEsclateNo));
                            }
                            row["LandAcquisitionCostLeased"] = decLeasePayCostpInterestComp;
                        }
                        break;
                }
            }
            string strSecDef = GetLocalResourceObject("SecurityDDefinition").ToString();
            CashFlowsLongTermAssetsProxy.InsertBulkLandAcquisitionCostLeased(CostTbl, PlanningScenarioId, strSecDef, leaseStartDate, securityDeposit, UserId);
        }

        protected void lbtnInsert_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                RadNumericTextBox ntxtLeaseTerm = fvLeaseDetails.FindControl("ntxtLeaseTerm") as RadNumericTextBox;
                RadComboBox ddlLeaseTermUnit = fvLeaseDetails.FindControl("ddlLeaseTermUnit") as RadComboBox;
                RadNumericTextBox ntxtPeriodicPaymentTerm = fvLeaseDetails.FindControl("ntxtPeriodicPaymentTerm") as RadNumericTextBox;
                RadComboBox ddlPeriodicPaymentTermUnit = fvLeaseDetails.FindControl("ddlPeriodicPaymentTermUnit") as RadComboBox;
                RadNumericTextBox ntxtLeaseEsclationTerm = fvLeaseDetails.FindControl("ntxtLeaseEsclationTerm") as RadNumericTextBox;
                RadComboBox ddlLeaseEsclationTermUnit = fvLeaseDetails.FindControl("ddlLeaseEsclationTermUnit") as RadComboBox;
                RadNumericTextBox ntxtEsclationRate = fvLeaseDetails.FindControl("ntxtEsclationRate") as RadNumericTextBox;
                RadComboBox ddlEsclationMethod = fvLeaseDetails.FindControl("ddlEsclationMethod") as RadComboBox;
                RadDatePicker dtxtLeaseStartDate = fvLeaseDetails.FindControl("dtxtLeaseStartDate") as RadDatePicker;
                RadNumericTextBox ntxtInitialPeriodicPayment = fvLeaseDetails.FindControl("ntxtInitialPeriodicPayment") as RadNumericTextBox;
                RadNumericTextBox ntxtSecurityDeposit = fvLeaseDetails.FindControl("ntxtSecurityDeposit") as RadNumericTextBox;
                int intLeaseTerm = Convert.ToInt32(ntxtLeaseTerm.Value);
                int intLeaseTermUnit = Convert.ToInt32(ddlLeaseTermUnit.SelectedValue);
                int intPeriodicPaymentTerm = Convert.ToInt32(ntxtPeriodicPaymentTerm.Value);
                int intPeriodicPaymentTermUnit = Convert.ToInt32(ddlPeriodicPaymentTermUnit.SelectedValue);
                int intLeaseEsclationTerm = 0;
                if (ntxtLeaseEsclationTerm.Value.HasValue)
                {
                    intLeaseEsclationTerm = Convert.ToInt32(ntxtLeaseEsclationTerm.Value);
                }
                int intLeaseEsclationTermUnit = Convert.ToInt32(ddlLeaseEsclationTermUnit.SelectedValue);
                double dblEsclationRate = 0;
                if (ntxtEsclationRate.Value.HasValue)
                {
                    dblEsclationRate = Convert.ToDouble(ntxtEsclationRate.Value) / 100;
                }
                int intEsclationMethod = Convert.ToInt32(ddlEsclationMethod.SelectedValue);
                DateTime dtLeaseStartDate = dtxtLeaseStartDate.SelectedDate.Value;
                decimal decInitialPeriodicPayment = Convert.ToDecimal(ntxtInitialPeriodicPayment.Value);
                decimal decSecurityDeposit = 0;
                if (ntxtSecurityDeposit.Value.HasValue)
                {
                    decSecurityDeposit = Convert.ToDecimal(ntxtSecurityDeposit.Value);
                }
                //*********
                //add validation if EsclationRate has value then should enter EsclationTerm & EsclationMethod
                //*********
                LeaseDetailProxy.InsertLeaseDetails(PlanningScenarioId, intLeaseTerm, intLeaseTermUnit, intPeriodicPaymentTerm, intPeriodicPaymentTermUnit, intLeaseEsclationTerm, intLeaseEsclationTermUnit, dblEsclationRate, intEsclationMethod, dtLeaseStartDate, decInitialPeriodicPayment, decSecurityDeposit, UserId);
                RecordLandAcquisitionCostLeased(intLeaseTerm, intPeriodicPaymentTerm, dtLeaseStartDate, decInitialPeriodicPayment, intLeaseEsclationTerm, dblEsclationRate, intEsclationMethod, decSecurityDeposit);
                LoadLeaseDetails();
            }
        }

        protected void lbtnCancelInsert_Click(object sender, EventArgs e)
        {
            LoadLeaseDetails();
        }

        protected void fvLeaseDetails_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                Label lblLeaseDetailId = fvLeaseDetails.FindControl("lblLeaseDetailId") as Label;
                RadNumericTextBox ntxtLeaseTerm = fvLeaseDetails.FindControl("ntxtLeaseTerm") as RadNumericTextBox;
                RadComboBox ddlLeaseTermUnit = fvLeaseDetails.FindControl("ddlLeaseTermUnit") as RadComboBox;
                RadNumericTextBox ntxtPeriodicPaymentTerm = fvLeaseDetails.FindControl("ntxtPeriodicPaymentTerm") as RadNumericTextBox;
                RadComboBox ddlPeriodicPaymentTermUnit = fvLeaseDetails.FindControl("ddlPeriodicPaymentTermUnit") as RadComboBox;
                RadNumericTextBox ntxtLeaseEsclationTerm = fvLeaseDetails.FindControl("ntxtLeaseEsclationTerm") as RadNumericTextBox;
                RadComboBox ddlLeaseEsclationTermUnit = fvLeaseDetails.FindControl("ddlLeaseEsclationTermUnit") as RadComboBox;
                RadNumericTextBox ntxtEsclationRate = fvLeaseDetails.FindControl("ntxtEsclationRate") as RadNumericTextBox;
                RadComboBox ddlEsclationMethod = fvLeaseDetails.FindControl("ddlEsclationMethod") as RadComboBox;
                RadDatePicker dtxtLeaseStartDate = fvLeaseDetails.FindControl("dtxtLeaseStartDate") as RadDatePicker;
                RadNumericTextBox ntxtInitialPeriodicPayment = fvLeaseDetails.FindControl("ntxtInitialPeriodicPayment") as RadNumericTextBox;
                RadNumericTextBox ntxtSecurityDeposit = fvLeaseDetails.FindControl("ntxtSecurityDeposit") as RadNumericTextBox;
                int intLeaseDetailId = Convert.ToInt32(lblLeaseDetailId.Text);
                int intLeaseTerm = Convert.ToInt32(ntxtLeaseTerm.Value);
                int intLeaseTermUnit = Convert.ToInt32(ddlLeaseTermUnit.SelectedValue);
                int intPeriodicPaymentTerm = Convert.ToInt32(ntxtPeriodicPaymentTerm.Value);
                int intPeriodicPaymentTermUnit = Convert.ToInt32(ddlPeriodicPaymentTermUnit.SelectedValue);
                int intLeaseEsclationTerm = 0;
                if (ntxtLeaseEsclationTerm.Value.HasValue)
                {
                    intLeaseEsclationTerm = Convert.ToInt32(ntxtLeaseEsclationTerm.Value);
                }
                int intLeaseEsclationTermUnit = Convert.ToInt32(ddlLeaseEsclationTermUnit.SelectedValue);
                double dblEsclationRate = 0;
                if (ntxtEsclationRate.Value.HasValue)
                {
                    dblEsclationRate = Convert.ToDouble(ntxtEsclationRate.Value) / 100;
                }
                int intEsclationMethod = Convert.ToInt32(ddlEsclationMethod.SelectedValue);
                DateTime dtLeaseStartDate = dtxtLeaseStartDate.SelectedDate.Value;
                decimal decInitialPeriodicPayment = Convert.ToDecimal(ntxtInitialPeriodicPayment.Value);
                decimal decSecurityDeposit = 0;
                if (ntxtSecurityDeposit.Value.HasValue)
                {
                    decSecurityDeposit = Convert.ToDecimal(ntxtSecurityDeposit.Value);
                }
                //*********
                //add validation if EsclationRate has value then should enter EsclationTerm & EsclationMethod
                //*********
                LeaseDetailProxy.UpdateLeaseDetails(intLeaseDetailId, PlanningScenarioId, intLeaseTerm, intLeaseTermUnit, intPeriodicPaymentTerm, intPeriodicPaymentTermUnit, intLeaseEsclationTerm, intLeaseEsclationTermUnit, dblEsclationRate, intEsclationMethod, dtLeaseStartDate, decInitialPeriodicPayment, decSecurityDeposit, UserId);
                RecordLandAcquisitionCostLeased(intLeaseTerm, intPeriodicPaymentTerm, dtLeaseStartDate, decInitialPeriodicPayment, intLeaseEsclationTerm, dblEsclationRate, intEsclationMethod, decSecurityDeposit);
                LoadLeaseDetails();
            }
        }
    }
}
