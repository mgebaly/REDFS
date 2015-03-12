<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DevelopmentCostsSchedule.ascx.cs" Inherits="REDFS_WebApp2.DevelopmentCostsSchedule" %>
<style>
    .hide {
        display: none !important;
    }

    .rpgRowsZone {
        padding: 0px !important;
    }
</style>
<telerik:RadPivotGrid ID="pvtDevCostsA" runat="server" ShowColumnHeaderZone="False" ShowDataHeaderZone="False" ShowFilterHeaderZone="False" ShowRowHeaderZone="False" AllowNaturalSort="true" OnCellDataBound="pvtDevCostsA_CellDataBound" EnableTheming="True">
    <PagerStyle ChangePageSizeButtonToolTip="Change Page Size" PageSizeControlType="RadComboBox"></PagerStyle>
    <Fields>
        <telerik:PivotGridRowField DataField="Year" UniqueName="Yearr">
        </telerik:PivotGridRowField>
        <telerik:PivotGridRowField DataField="Definition" UniqueName="Definition" SortOrder="None">
        </telerik:PivotGridRowField>
        <telerik:PivotGridColumnField DataField="Years" UniqueName="Years" SortOrder="None">
        </telerik:PivotGridColumnField>
        <telerik:PivotGridColumnField DataField="Year" UniqueName="Year">
        </telerik:PivotGridColumnField>
        <telerik:PivotGridAggregateField DataField="DevelopmentCosts" UniqueName="DevelopmentCosts">
        </telerik:PivotGridAggregateField>
    </Fields>
    <TotalsSettings GrandTotalsVisibility="RowsOnly" RowsSubTotalsPosition="None" ColumnGrandTotalsPosition="None" ColumnsSubTotalsPosition="None" />
    <ConfigurationPanelSettings EnableOlapTreeViewLoadOnDemand="True"></ConfigurationPanelSettings>
</telerik:RadPivotGrid>
<telerik:RadPivotGrid ID="pvtDevCostsQ" runat="server" ShowColumnHeaderZone="False" ShowDataHeaderZone="False" ShowFilterHeaderZone="False" ShowRowHeaderZone="False" AllowNaturalSort="true" OnCellDataBound="pvtDevCostsQ_CellDataBound" EnableTheming="True">
    <PagerStyle ChangePageSizeButtonToolTip="Change Page Size" PageSizeControlType="RadComboBox"></PagerStyle>
    <Fields>
        <telerik:PivotGridRowField DataField="Year" UniqueName="Yearr">
        </telerik:PivotGridRowField>
        <telerik:PivotGridRowField DataField="Definition" UniqueName="Definition" SortOrder="None">
        </telerik:PivotGridRowField>
        <telerik:PivotGridColumnField DataField="Years" UniqueName="Years" SortOrder="None">
        </telerik:PivotGridColumnField>
        <telerik:PivotGridColumnField DataField="Year" UniqueName="Year">
        </telerik:PivotGridColumnField>
        <telerik:PivotGridColumnField DataField="Quarter" UniqueName="Quarter" DataFormatString="Quarter {0}">
        </telerik:PivotGridColumnField>
        <telerik:PivotGridAggregateField DataField="DevelopmentCosts" UniqueName="DevelopmentCosts">
        </telerik:PivotGridAggregateField>
    </Fields>
    <TotalsSettings GrandTotalsVisibility="RowsOnly" RowsSubTotalsPosition="None" ColumnGrandTotalsPosition="None" ColumnsSubTotalsPosition="None" />
    <ConfigurationPanelSettings EnableOlapTreeViewLoadOnDemand="True"></ConfigurationPanelSettings>
</telerik:RadPivotGrid>
