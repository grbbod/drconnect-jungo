﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Activity.ascx.cs" Inherits="N2.Management.Myself.Activity" %>
<%@ Register TagPrefix="edit" Namespace="N2.Edit.Web.UI.Controls" Assembly="N2.Management" %>

<div class="uc">
	<h4 class="header"><%= CurrentItem.Title %></h4>
	<div class="box">
	<asp:Repeater ID="rptLatestChanges" runat="server">
		<HeaderTemplate>
	<table class="data">
		<thead><tr><th>Changed</th><th>Version</th><th>Saved by</th><th>Last updated</th></tr></thead>
		<tbody>
		</HeaderTemplate>
		<ItemTemplate>
			<tr><td>
				<edit:ItemLink DataSource="<%# Container.DataItem %>" runat="server" />
			</td><td>
				<%# Eval("VersionIndex")%>
			</td><td>
				<%# Eval("SavedBy")%>
			</td><td>
				<%# Eval("Updated")%>
			</td></tr>
		</ItemTemplate>
		<FooterTemplate>
		</tbody>
	</table>
		</FooterTemplate>
	</asp:Repeater>

	<n2:Repeater ID="rptDrafts" runat="server">
		<HeaderTemplate>
	<table class="data">
		<thead><tr><th>Draft</th><th>Version</th><th>Saved by</th><th>Last updated</th></tr></thead>
		<tbody>
		</HeaderTemplate>
		<ItemTemplate>
			<tr><td>
				<edit:ItemLink ID="ItemLink1" DataSource='<%# Eval("Version") %>' runat="server" />
			</td><td>
				<%# Eval("VersionIndex")%>
			</td><td>
				<%# Eval("SavedBy")%>
			</td><td>
				<%# Eval("Saved")%>
			</td></tr>
		</ItemTemplate>
		<FooterTemplate>
		</tbody>
	</table>
		</FooterTemplate>
		<EmptyTemplate>
			<tr><td colspan="5">No drafts</td></tr>
		</EmptyTemplate>
	</n2:Repeater>
	</div>
</div>