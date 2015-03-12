using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace REDFS_WebApp2
{
    public partial class DevelopmentCostsSchedule : BaseControl
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
        void bindpvtDevCostsA(int scenarioId)
        {
            pvtDevCostsA.Visible = true;
            pvtDevCostsA.DataSource = CashFlowsLongTermAssetsProxy.DevelopmentCosts_ScheduleA(scenarioId);
            pvtDevCostsA.DataBind();
        }
        void bindpvtDevCostsQ(int scenarioId)
        {
            pvtDevCostsQ.Visible = true;
            pvtDevCostsQ.DataSource = CashFlowsLongTermAssetsProxy.DevelopmentCosts_ScheduleQ(scenarioId);
            pvtDevCostsQ.DataBind();
        }
        public void LoadDevelopmentCostsSchedule()
        {
            bindpvtDevCostsA(PlanningScenarioId);
            bindpvtDevCostsQ(PlanningScenarioId);
        }
        public void Clear()
        {
            pvtDevCostsA.Visible = false;
            pvtDevCostsQ.Visible = false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pvtDevCostsA.Visible = false;
                pvtDevCostsQ.Visible = false;
            }
        }
        protected void pvtDevCostsA_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
        {
            if (e.Cell is PivotGridRowHeaderCell)
            {
                PivotGridRowHeaderCell rowCell = e.Cell as PivotGridRowHeaderCell;
                if (rowCell.ParentIndexes != null && rowCell.ParentIndexes.Length == 2 && !rowCell.IsGrandTotalCell)
                {
                    rowCell.Wrap = false;
                }
                if (rowCell.ParentIndexes != null && rowCell.ParentIndexes.Length == 1)
                {
                    rowCell.Width = new Unit(0);
                    rowCell.CssClass = "hide";
                }
            }
            if (e.Cell is PivotGridDataCell)
            {
                PivotGridDataCell cell = e.Cell as PivotGridDataCell;
                if (cell.ParentRowIndexes != null && cell.ParentRowIndexes.Length == 2 && cell.ParentRowIndexes[1].ToString() == "(blank)")
                {
                    cell.CssClass = "hide";
                }
            }
            if (e.Cell is PivotGridRowHeaderCell)
            {
                PivotGridRowHeaderCell rowCell = e.Cell as PivotGridRowHeaderCell;
                if (rowCell.DataItem.ToString() == "(blank)")
                {
                    rowCell.CssClass = "hide";
                }
            }
        }

        protected void pvtDevCostsQ_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
        {
            if (e.Cell is PivotGridRowHeaderCell)
            {
                PivotGridRowHeaderCell rowCell = e.Cell as PivotGridRowHeaderCell;
                if (rowCell.ParentIndexes != null && rowCell.ParentIndexes.Length == 2 && !rowCell.IsGrandTotalCell)
                {
                    rowCell.Wrap = false;
                }
                if (rowCell.ParentIndexes != null && rowCell.ParentIndexes.Length == 1)
                {
                    rowCell.Width = new Unit(0);
                    rowCell.CssClass = "hide";
                }
            }
            if (e.Cell is PivotGridDataCell)
            {
                PivotGridDataCell cell = e.Cell as PivotGridDataCell;
                if (cell.ParentRowIndexes != null && cell.ParentRowIndexes.Length == 2 && cell.ParentRowIndexes[1].ToString() == "(blank)")
                {
                    cell.CssClass = "hide";
                }
            }
            if (e.Cell is PivotGridRowHeaderCell)
            {
                PivotGridRowHeaderCell rowCell = e.Cell as PivotGridRowHeaderCell;
                if (rowCell.DataItem.ToString() == "(blank)")
                {
                    rowCell.CssClass = "hide";
                }
            }
        }
    }
}
