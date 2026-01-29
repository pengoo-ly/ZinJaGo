<%@ Page Title="Coupon Management" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminCupon.aspx.cs" Inherits="Week1_Practical1.AdminCupon" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .coupon-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 24px;
        }

        .btn-primary-custom {
            background: var(--accent);
            color: #fff;
            border: none;
            padding: 10px 18px;
            border-radius: 8px;
            font-size: 14px;
            cursor: pointer;
            display: inline-flex;
            align-items: center;
            gap: 6px;
            transition: opacity 0.2s ease;
        }

            .btn-primary-custom:hover {
                opacity: 0.9;
            }

        .coupon-stats {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 16px;
            margin-bottom: 28px;
        }

        .stat-card {
            background: var(--card);
            border-radius: 12px;
            padding: 18px;
            box-shadow: var(--shadow);
            text-align: center;
            transition: transform 0.2s ease, box-shadow 0.2s ease;
        }

            .stat-card:hover {
                transform: translateY(-4px);
                box-shadow: 0 8px 20px rgba(0,0,0,0.1);
            }

        .stat-number {
            font-size: 28px;
            font-weight: 700;
            color: var(--text);
            margin: 8px 0;
        }

        .stat-label {
            font-size: 13px;
            color: var(--muted);
            margin-bottom: 8px;
        }

        .table-wrapper {
            background: var(--card);
            border-radius: 14px;
            padding: 20px;
            box-shadow: var(--shadow);
            overflow-x: auto;
        }

        .table-toolbar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
            gap: 12px;
            flex-wrap: wrap;
        }

        .table-search {
            position: relative;
            flex: 1;
            min-width: 200px;
        }

        .table-search input {
            width: 100%;
            background: transparent;
            border: 1px solid rgba(0,0,0,0.1);
            padding: 8px 12px;
            border-radius: 6px;
            color: var(--text);
            font-size: 13px;
        }

        .dark .table-search input {
            border-color: rgba(255,255,255,0.1);
        }

        .table-search input::placeholder {
            color: var(--muted);
        }

        .icon-btn {
            background: transparent;
            border: none;
            color: var(--muted);
            cursor: pointer;
            font-size: 18px;
            padding: 6px;
            transition: color 0.2s ease;
        }

            .icon-btn:hover {
                color: var(--accent);
            }

        .dropdown-menu {
            display: none;
            position: absolute;
            top: 35px;
            right: 0;
            background: var(--card);
            border: 1px solid rgba(0,0,0,0.1);
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.15);
            min-width: 150px;
            z-index: 1000;
        }

        .dark .dropdown-menu {
            border-color: rgba(255,255,255,0.1);
            box-shadow: 0 4px 12px rgba(0,0,0,0.3);
        }

        .dropdown-menu.show {
            display: block;
        }

        .dropdown-item {
            padding: 10px 16px;
            color: var(--text);
            cursor: pointer;
            border: none;
            background: transparent;
            width: 100%;
            text-align: left;
            font-size: 13px;
            transition: background 0.2s ease;
        }

            .dropdown-item:hover {
                background: rgba(79, 163, 146, 0.1);
                color: var(--accent);
            }

        .dropdown-divider {
            height: 1px;
            background: rgba(0,0,0,0.05);
            margin: 6px 0;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            font-size: 13px;
        }

        table thead {
            border-bottom: 2px solid rgba(0,0,0,0.08);
        }

        .dark table thead {
            border-bottom-color: rgba(255,255,255,0.08);
        }

        table th {
            text-align: left;
            padding: 12px 8px;
            font-weight: 600;
            color: var(--muted);
            font-size: 12px;
            text-transform: uppercase;
        }

        table td {
            padding: 14px 8px;
            border-bottom: 1px solid rgba(0,0,0,0.05);
            vertical-align: middle; 
        }

        .dark table td {
            border-bottom-color: rgba(255,255,255,0.05);
        }

        table tbody tr:hover {
            background: rgba(79, 163, 146, 0.04);
        }

        .badge {
            display: inline-flex;
            align-items: center;
            gap: 4px;
            padding: 4px 10px;
            border-radius: 4px;
            font-size: 12px;
            font-weight: 500;
        }

        .badge-active {
            background: rgba(79, 163, 146, 0.1);
            color: var(--accent);
        }

        .badge-inactive {
            background: rgba(220, 77, 77, 0.1);
            color: #dc4d4d;
        }

        .badge-expired {
            background: rgba(255, 159, 64, 0.1);
            color: #ff9f40;
        }

        .action-buttons {
            display: flex;
            gap: 8px;
        }

        .btn-sm {
            background: var(--card);
            border: 1px solid rgba(0,0,0,0.1);
            color: var(--text);
            padding: 6px 12px;
            border-radius: 6px;
            font-size: 12px;
            cursor: pointer;
            transition: all 0.2s ease;
        }

            .btn-sm:hover {
                background: rgba(0,0,0,0.03);
                border-color: var(--accent);
            }

        .btn-sm.danger {
            border-color: rgba(220, 77, 77, 0.3);
            color: #dc4d4d;
        }

            .btn-sm.danger:hover {
                background: rgba(220, 77, 77, 0.1);
            }

        /* Modal Styles */
        .modal-backdrop {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(0,0,0,0.5);
            z-index: 2000;
            align-items: center;
            justify-content: center;
        }

        .modal-backdrop.show {
            display: flex;
        }

        .modal-content {
            background: var(--card);
            border-radius: 12px;
            box-shadow: 0 10px 40px rgba(0,0,0,0.2);
            max-width: 500px;
            width: 90%;
            max-height: 90vh;
            overflow-y: auto;
            padding: 28px;
            position: relative;
            margin: auto;
            opacity:1;
            backdrop-filter:none;
        }

        .modal-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
            padding-bottom: 16px;
            border-bottom: 1px solid rgba(0,0,0,0.05);
        }

        .modal-header h3 {
            margin: 0;
            font-size: 18px;
            font-weight: 600;
        }

        .modal-close {
            background: transparent;
            border: none;
            color: var(--muted);
            font-size: 24px;
            cursor: pointer;
            padding: 0;
            width: 30px;
            height: 30px;
            display: flex;
            align-items: center;
            justify-content: center;
        }

            .modal-close:hover {
                color: var(--text);
            }

        .form-group {
            margin-bottom: 16px;
        }

        .form-group label {
            display: block;
            font-size: 13px;
            font-weight: 500;
            margin-bottom: 6px;
            color: var(--text);
        }

        .form-group input,
        .form-group select {
            width: 100%;
            padding: 10px 12px;
            border: 1px solid rgba(0,0,0,0.1);
            border-radius: 6px;
            background: var(--card);
            color: var(--text);
            font-size: 13px;
            font-family: inherit;
            box-sizing: border-box;
        }

        .dark .form-group input,
        .dark .form-group select {
            border-color: rgba(255,255,255,0.1);
            background: #0f1720;
            color: #ffffff;
        }

        .form-group input:focus,
        .form-group select:focus {
            outline: none;
            border-color: var(--accent);
            box-shadow: 0 0 0 2px rgba(79, 163, 146, 0.1);
        }

        .form-row {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 16px;
        }

        .modal-footer {
            display: flex;
            gap: 12px;
            margin-top: 24px;
            padding-top: 16px;
            border-top: 1px solid rgba(0,0,0,0.05);
        }

        .modal-footer button {
            flex: 1;
            padding: 10px 16px;
            border-radius: 6px;
            border: none;
            font-size: 13px;
            font-weight: 500;
            cursor: pointer;
            transition: opacity 0.2s ease;
        }

        .btn-primary {
            background: var(--accent);
            color: #fff;
        }

            .btn-primary:hover {
                opacity: 0.9;
            }

        .btn-primary:disabled {
            opacity: 0.5;
            cursor: not-allowed;
        }

        .btn-secondary {
            background: transparent;
            border: 1px solid rgba(0,0,0,0.1);
            color: var(--text);
        }

            .btn-secondary:hover {
                background: rgba(0,0,0,0.03);
            }

        .alert {
            padding: 12px 16px;
            border-radius: 6px;
            margin-bottom: 16px;
            font-size: 13px;
        }

        .alert-success {
            background: rgba(79, 163, 146, 0.1);
            color: var(--accent);
            border: 1px solid var(--accent);
        }

        .alert-error {
            background: rgba(220, 77, 77, 0.1);
            color: #dc4d4d;
            border: 1px solid #dc4d4d;
        }

        .empty-state {
            text-align: center;
            padding: 40px 20px;
        }

        .empty-state-icon {
            font-size: 48px;
            margin-bottom: 16px;
        }

        .empty-state h3 {
            font-size: 16px;
            font-weight: 600;
            margin: 0 0 8px 0;
        }

        .empty-state p {
            color: var(--muted);
            font-size: 13px;
            margin: 0;
        }

        .loading-spinner {
            display: inline-block;
            width: 16px;
            height: 16px;
            border: 2px solid rgba(255, 255, 255, 0.3);
            border-top: 2px solid #fff;
            border-radius: 50%;
            animation: spin 0.8s linear infinite;
        }

        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }

        @media (max-width: 768px) {
            .coupon-header {
                flex-direction: column;
                align-items: flex-start;
            }

            .coupon-stats {
                grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
            }

            .table-toolbar {
                flex-direction: column;
                align-items: stretch;
            }

            table {
                font-size: 12px;
            }

            table th, table td {
                padding: 10px 6px;
            }

            .action-buttons {
                flex-wrap: wrap;
            }

            .form-row {
                grid-template-columns: 1fr;
            }
        }
        .auto-style1 {
            height: 68px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="coupon-header">
        <div>
            <h2>Coupon Management</h2>
            <p style="color: var(--muted); margin: 6px 0 0 0; font-size: 13px;">Create and manage discount coupons for your customers</p>
        </div>
         <asp:Button ID="btnOpenAdd" runat="server" Text="➕ Create Coupon" CssClass="btn-add" OnClientClick="openCouponModal(); return false;" OnClick="btnOpenAdd_Click"/>
    </div>
    <br />

    <!-- Coupon Statistics -->
    <div class="coupon-stats">
        <div class="stat-card">
            <div class="stat-label">Total Coupons</div>
            <div class="stat-number"><asp:Label ID="lblTotalCoupons" runat="server" Text="0" /></div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Active Coupons</div>
            <div class="stat-number"><asp:Label ID="lblActiveCoupons" runat="server" Text="0" /></div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Total Discount Value</div>
            <div class="stat-number">$<asp:Label ID="lblTotalDiscount" runat="server" Text="0.00" /></div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Times Used</div>
            <div class="stat-number"><asp:Label ID="lblTimesUsed" runat="server" Text="0" /></div>
        </div>
    </div>

    <!-- Coupons Table -->
    <div class="table-wrapper">
        <div class="table-toolbar">
            <div class="table-search">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="table-search-input" placeholder="Search coupons..." />
            </div>
            <asp:Button ID="btnSearch" runat="server" Text="Search"  CssClass="btn-primary-custom" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn-secondary" OnClick="btnClear_Click" />

            <div style="position: relative;">
                <button type="button" class="icon-btn more-btn" title="More options">⋯</button>
                <div class="dropdown-menu" id="dropdownMenu">
                    <button class="dropdown-item" data-action="active">Active Only</button>
                    <button class="dropdown-item" data-action="inactive">Inactive Only</button>
                    <button class="dropdown-item" data-action="expired">Expired Only</button>
                    <div class="dropdown-divider"></div>
                    <button class="dropdown-item" data-action="all">Show All</button>
                </div>
            </div>
        </div>

        <asp:GridView ID="gvCoupons" runat="server"
            AutoGenerateColumns="False"
            CssClass="table"
            DataKeyNames="VoucherID" OnRowCancelingEdit="gvCoupons_RowCancelingEdit" OnRowDeleting="gvCoupons_RowDeleting" OnRowEditing="gvCoupons_RowEditing">

            <Columns>
                <asp:BoundField DataField="Code" HeaderText="Code" />
                <asp:BoundField DataField="VoucherType" HeaderText="Type" />
                <asp:BoundField DataField="DiscountValue" HeaderText="Discount" />
                <asp:BoundField DataField="CoinCost" HeaderText="Coin Cost" />
                <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" DataFormatString="{0:dd-MM-yyyy}" />
                <asp:BoundField DataField="Status" HeaderText="Status" />

                <asp:CommandField ButtonType="Button" InsertVisible="False" ShowDeleteButton="True" ShowEditButton="True" />

            </Columns>
        </asp:GridView>


        <div id="emptyState" class="empty-state" style="display: none;">
            <div class="empty-state-icon">🏷️</div>
            <h3>No Coupons Yet</h3>
            <p>Start by creating a coupon to offer discounts to your customers</p>
        </div>
    </div>

    <!-- Create/Edit Modal -->
    <div class="modal-backdrop" id="couponModal">
        <div class="modal-content">
    <div class="modal-header">
        <h3 id="modalTitle">Create Coupon</h3>
        <asp:Button ID="btnCloseModal" runat="server" Text="×" CssClass="modal-close" OnClientClick="closeCouponModal(); return false;" />
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false">
        <asp:Label ID="lblAlert" runat="server" Text="" />
    </asp:Panel>

    <asp:Panel ID="pnlCouponForm" runat="server">
        <asp:HiddenField ID="hfVoucherID" runat="server" />

        <div class="form-group">
            <label>Code</label>
            <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label>Voucher Type</label>
            <asp:DropDownList ID="ddlVoucherType" runat="server" CssClass="form-control">
                <asp:ListItem Text="Voucher" Value="Voucher" />
                <asp:ListItem Text="Coupon" Value="Coupon" />
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <label>Discount Type</label>
            <asp:DropDownList ID="ddlDiscountType" runat="server" CssClass="form-control">
                <asp:ListItem Text="Percentage" Value="Percentage" />
                <asp:ListItem Text="Fixed" Value="Fixed" />
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <label>Discount Value</label>
            <asp:TextBox ID="txtDiscountValue" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label>Coin Cost</label>
            <asp:TextBox ID="txtCoinCost" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label>Status</label>
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                <asp:ListItem Text="Active" Value="Active" />
                <asp:ListItem Text="Inactive" Value="Inactive" />
            </asp:DropDownList>
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSaveCoupon" runat="server" Text="Save Coupon" CssClass="btn-add" OnClick="btnSaveCoupon_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-cancel" OnClientClick="closeCouponModal(); return false;" />
        </div>
    </asp:Panel>
</div>

    </div>

   <script>

       function openCouponModal() {
           document.getElementById('couponModal').classList.add('show');
       }
       function closeCouponModal() {
           document.getElementById('couponModal').classList.remove('show');
       }
   </script>
</asp:Content>