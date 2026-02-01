<%@ Page Title="Return Management" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ReturnManagement.aspx.cs" Inherits="Week1_Practical1.ReturnManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Return Management</h2>
    <p>Approve, reject or process customer return requests</p>

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
                <asp:BoundField DataField="ReturnStatus" HeaderText="Status" />

                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button runat="server" Text="Approve"
                            CssClass="gv-btn"
                            CommandName="Approve"
                            CommandArgument='<%# Eval("ReturnID") %>'
                            Visible='<%# Eval("ReturnStatus").ToString() == "Pending" %>' />

                        <asp:Button runat="server" Text="Reject"
                            CssClass="gv-btn"
                            CommandName="Reject"
                            CommandArgument='<%# Eval("ReturnID") %>'
                            Visible='<%# Eval("ReturnStatus").ToString() == "Pending" %>' />

                        <asp:Button runat="server" Text="Processed"
                            CssClass="gv-btn"
                            CommandName="Processed"
                            CommandArgument='<%# Eval("ReturnID") %>'
                            Visible='<%# Eval("ReturnStatus").ToString() == "Approved" %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>

    </asp:Panel>
</asp:Content>
