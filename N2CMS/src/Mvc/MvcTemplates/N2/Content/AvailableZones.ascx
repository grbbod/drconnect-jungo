<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AvailableZones.ascx.cs" Inherits="N2.Edit.AvailableZones" meta:resourceKey="AvailableZonesResource" %>
<%@ Register TagPrefix="edit" Namespace="N2.Edit.Web.UI.Controls" Assembly="N2.Management" %>
<%@ Register TagPrefix="n2" Namespace="N2.Web.UI.WebControls" Assembly="N2" %>
<n2:Box ID="boxZones" HeadingText="Zones" CssClass="box zonesBox" runat="server" meta:resourceKey="boxZones">
	<asp:Repeater ID="rptZones" runat="server">
		<HeaderTemplate><dl></HeaderTemplate>
		<ItemTemplate>
			<dt>
				<asp:HyperLink CssClass="new" ID="hlNew" runat="server" ToolTip="New item" NavigateUrl="<%# GetNewDataItemUrl(Container.DataItem) %>">
					<b class="n2-icon-plus-sign"></b>
					<%# GetZoneString((string)Eval("ZoneName")) ?? Eval("Title") %>
				</asp:HyperLink>
			</dt>
			<asp:Repeater ID="rptItems" runat="server" DataSource="<%# GetItemsInZone(Container.DataItem) %>">
				<HeaderTemplate><dd class="items"></HeaderTemplate>
				<ItemTemplate>
					<div class="edit">
						<edit:ItemLink DataSource="<%# Container.DataItem %>" runat="server" InterfaceUrl="Edit.aspx" />
						<asp:ImageButton runat="server" CommandArgument="<%#GetEditDataItemID(Container.DataItem)%>" 
							Enabled="<%#CanMoveItemDown(Container.DataItem) %>"
							CssClass="<%#MoveItemDownClass(Container.DataItem)%>"
							ImageUrl="../Resources/icons/bullet_arrow_down.png" OnClick="MoveItemDown" />
						<asp:ImageButton runat="server" CommandArgument="<%#GetEditDataItemID(Container.DataItem)%>"
							Enabled="<%#CanMoveItemUp(Container.DataItem) %>"
							CssClass="<%#MoveItemUpClass(Container.DataItem)%>"
							ImageUrl="../Resources/icons/bullet_arrow_up.png" OnClick="MoveItemUp"/>
						<asp:HyperLink NavigateUrl="<%# GetDeleteDataItemUrl(Container.DataItem) %>" CssClass="delete" runat="server">
							<b class="n2-icon-trash"></b>
						</asp:HyperLink>
					</div>
				</ItemTemplate>
				<FooterTemplate></dd></FooterTemplate>
			</asp:Repeater>
		</ItemTemplate>
		<FooterTemplate></dl></FooterTemplate>
	</asp:Repeater>
</n2:Box>