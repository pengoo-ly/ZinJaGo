<%@ Page Title="Return Management" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ReturnManagement.aspx.cs" Inherits="Week1_Practical1.ReturnManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Return Management</h2>
    <p>Approve, reject or process customer return requests</p>

    <br />

    <asp:TextBox ID="txtSearch" runat="server" CssClass="textbox" Placeholder="Search Return / Order / Status" />
    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn-add" OnClick="btnSearch_Click" />
    <br />

    <asp:Panel runat="server" CssClass="panel-style">

        <asp:GridView ID="gvReturns" runat="server"
            AutoGenerateColumns="False"
            CssClass="gridview-style"
            HeaderStyle-CssClass="gv-header"
            DataKeyNames="ReturnID"
            OnRowCommand="gvReturns_RowCommand">

            <Columns>
                <asp:BoundField DataField="ReturnID" HeaderText="Return ID" />
                <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
                <asp:BoundField DataField="ProductID" HeaderText="Product ID" />
                <asp:BoundField DataField="Reason" HeaderText="Reason" />
                <asp:BoundField DataField="RefundAmount" HeaderText="Refund ($)" DataFormatString="{0:0.00}" />

                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <span class='status-badge <%# Eval("ReturnStatus").ToString().ToLower() %>'>
                            <%# Eval("ReturnStatus").ToString().ToUpper() %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button runat="server" Text="Approve"
                            CssClass="btn-add"
                            CommandName="Approve"
                            CommandArgument='<%# Eval("ReturnID") %>'
                            Visible='<%# Eval("ReturnStatus").ToString().ToUpper() == "PENDING" %>'
                            OnClientClick="return confirm('Are you sure you want to approve this return?');" />

                        <asp:Button runat="server" Text="Reject"
                            CssClass="btn-add"
                            CommandName="Reject"
                            CommandArgument='<%# Eval("ReturnID") %>'
                            Visible='<%# Eval("ReturnStatus").ToString().ToUpper() == "PENDING" %>'
                            OnClientClick="return confirm('Are you sure you want to reject this return?');" />

                        <asp:Button runat="server" Text="Processed"
                            CssClass="btn-add"
                            CommandName="Processed"
                            CommandArgument='<%# Eval("ReturnID") %>'
                            Visible='<%# Eval("ReturnStatus").ToString().ToUpper() == "APPROVED" %>'
                            OnClientClick="return confirm('Mark this return as processed?');" />

                        <asp:Button runat="server" Text="Cancel"
                            CssClass="btn-cancel"
                            CommandName="CancelReturn"
                            CommandArgument='<%# Eval("ReturnID") %>'
                            Visible='<%# Eval("ReturnStatus").ToString().ToUpper() == "REJECTED" || Eval("ReturnStatus").ToString().ToUpper() == "APPROVED" %>'
                            OnClientClick="return confirm('Cancel this return and set it back to Pending?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

            <HeaderStyle CssClass="gv-header" />

        </asp:GridView>

    </asp:Panel>
</asp:Content>
