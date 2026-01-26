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
            background: transparent;
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
            background: transparent;
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

            .modal-content {
                max-width: 100%;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="coupon-header">
        <div>
            <h2>Coupon Management</h2>
            <p style="color: var(--muted); margin: 6px 0 0 0; font-size: 13px;">Create and manage discount coupons for your customers</p>
        </div>
        <button type="button" class="btn-primary-custom" id="btnCreateCoupon" onclick="openAddPanel()">
            ➕ Create Coupon
        </button>
    </div>
    <div id="addCouponPanel" class="modal-backdrop">
        <div class="modal-content">
            <h3>Add Coupon</h3>

            <div class="form-group">
                <label for="txtCode">Code</label>
                <input id="txtCode" placeholder="Enter coupon code" />
            </div>

            <div class="form-group">
                <label for="txtDiscount">Discount Value</label>
                <input id="txtDiscount" type="number" placeholder="Enter discount" />
            </div>

            <div class="form-group">
                <label for="txtExpiry">Expiry Date</label>
                <input id="txtExpiry" type="date" />
            </div>

            <div class="form-group">
                <label for="ddlType">Discount Type</label>
                <select id="ddlType">
                    <option value="Percentage">Percentage</option>
                    <option value="Fixed">Fixed</option>
                </select>
            </div>

            <div class="modal-footer">
                <button class="btn-secondary" onclick="saveCoupon()">Save</button>
                <button class="btn-primary" onclick="closeAddPanel()">Cancel</button>
            </div>
        </div>
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
                <input type="text" placeholder="Search coupons..." id="searchCoupons" />
            </div>

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

        <table id="couponsTable">
            <thead>
                <tr>
                    <th>Code</th>
                    <th>Type</th>
                    <th>Discount Value</th>
                    <th>Cost</th>
                    <th>Expiry Date</th>
                    <th>Status</th>
                    <th style="width: 120px;">Actions</th>
                </tr>
            </thead>
            <tbody id="couponsTableBody">
                <asp:Repeater ID="rptCoupons" runat="server">
                    <ItemTemplate>
                        <tr class="coupon-row" data-voucher-id="<%# Eval("VoucherID") %>" data-status="<%# Eval("Status").ToString().ToLower() %>">
                            <td><strong><%# Eval("Code") %></strong></td>
                            <td><%# Eval("VoucherType") %></td>
                            <td><%# Eval("DiscountType").ToString() == "Percentage" ? Eval("DiscountValue", "{0}") + "%" : "$" + Eval("DiscountValue", "{0:0.00}") %></td>
                            <td>$<%# Eval("CoinCost", "{0:0.00}") %></td>
                            <td><%# Eval("ExpiryDate", "{0:dd-MM-yyyy}") %></td>
                            <td>
                                <span class="badge <%# "badge-" + Eval("Status").ToString().ToLower() %>">
                                    ● <%# Eval("Status") %>
                                </span>
                            </td>
                            <td>
                                <div class="action-buttons">
                                    <button type="button" class="btn-sm edit-coupon" data-voucher-id="<%# Eval("VoucherID") %>" onclick="editCoupon(
                                        '<%# Eval("VoucherID") %>',
                                        '<%# Eval("Code") %>',
                                        '<%# Eval("DiscountType") %>',
                                        '<%# Eval("DiscountValue") %>',
                                        '<%# Eval("ExpiryDate","{0:yyyy-MM-dd}") %>')">Edit</button>
                                    <button type="button" class="btn-sm danger delete-coupon" data-voucher-id="<%# Eval("VoucherID") %>">Delete</button>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>

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
                <button type="button" class="modal-close" id="closeModal">×</button>
            </div>

            <div id="alertMessage"></div>

            <form id="couponForm">
                <input type="hidden" id="voucherId" />

                <div class="form-group">
                    <label for="couponCode">Coupon Code *</label>
                    <input type="text" id="couponCode" placeholder="e.g., SUMMER20" maxlength="50" required />
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label for="voucherType">Voucher Type *</label>
                        <select id="voucherType" required>
                            <option value="">Select Type</option>
                            <option value="Discount">Discount</option>
                            <option value="FreeShipping">Free Shipping</option>
                            <option value="Gift">Gift</option>
                        </select>
                    </div>

                    <div class="form-group">
                        <label for="discountType">Discount Type *</label>
                        <select id="discountType" required>
                            <option value="">Select Type</option>
                            <option value="Percentage">Percentage (%)</option>
                            <option value="FixedAmount">Fixed Amount ($)</option>
                        </select>
                    </div>
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label for="discountValue">Discount Value *</label>
                        <input type="number" id="discountValue" placeholder="0.00" step="0.01" min="0" required />
                    </div>

                    <div class="form-group">
                        <label for="coinCost">Coin Cost *</label>
                        <input type="number" id="coinCost" placeholder="0.00" step="0.01" min="0" required />
                    </div>
                </div>

                <div class="form-group">
                    <label for="expiryDate">Expiry Date *</label>
                    <input type="date" id="expiryDate" required />
                </div>

                <div class="form-group">
                    <label for="status">Status *</label>
                    <select id="status" required>
                        <option value="">Select Status</option>
                        <option value="Active">Active</option>
                        <option value="Inactive">Inactive</option>
                    </select>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn-secondary" id="cancelBtn">Cancel</button>
                    <button type="submit" class="btn-primary" id="submitBtn">Save Coupon</button>
                </div>
            </form>
        </div>
    </div>

    <script>
        const modal = document.getElementById('couponModal');
        const modalTitle = document.getElementById('modalTitle');
        const couponForm = document.getElementById('couponForm');
        const voucherId = document.getElementById('voucherId');
        const btnCreateCoupon = document.getElementById('btnCreateCoupon');
        const closeModal = document.getElementById('closeModal');
        const cancelBtn = document.getElementById('cancelBtn');
        const submitBtn = document.getElementById('submitBtn');
        const couponsTableBody = document.getElementById('couponsTableBody');
        const searchCoupons = document.getElementById('searchCoupons');
        const dropdownMenu = document.getElementById('dropdownMenu');
        const moreBtn = document.querySelector('.more-btn');
        const alertMessage = document.getElementById('alertMessage');

        function openAddPanel() {
            document.getElementById("addCouponPanel").style.display = "block";
        }
        function closeAddPanel() {
            document.getElementById("addCouponPanel").style.display = "none";
        }

        function saveCoupon() {
            const formData = new FormData();
            formData.append("action", "create");
            formData.append("code", txtCode.value);
            formData.append("voucherType", "Discount");
            formData.append("discountType", ddlType.value);
            formData.append("discountValue", txtDiscount.value);
            formData.append("coinCost", 0);
            formData.append("expiryDate", txtExpiry.value);
            formData.append("status", "Active");

            fetch("AdminCupon.aspx", {
                method: "POST",
                body: formData
            })
                .then(r => r.json())
                .then(res => {
                    if (res.success) {
                        closeAddPanel();
                        location.reload();
                    } else {
                        alert(res.message);
                    }
                });
        }

        function editCoupon(id, code, type, value, expiry) {
            openAddPanel();
            document.getElementById("hdnVoucherID").value = id;
            txtCode.value = code;
            ddlType.value = type;
            txtDiscount.value = value;
            txtExpiry.value = expiry;
        }


        // Show alert message
        function showAlert(message, type = 'error') {
            const alertDiv = document.createElement('div');
            alertDiv.className = `alert alert-${type}`;
            alertDiv.textContent = message;
            alertMessage.innerHTML = '';
            alertMessage.appendChild(alertDiv);

            if (type === 'success') {
                setTimeout(() => {
                    alertDiv.remove();
                }, 3000);
            }
        }

        // Open modal for creating coupon
        btnCreateCoupon.addEventListener('click', () => {
            voucherId.value = '';
            couponForm.reset();
            modalTitle.textContent = 'Create Coupon';
            alertMessage.innerHTML = '';
            modal.classList.add('show');
        });

        // Close modal
        closeModal.addEventListener('click', () => {
            modal.classList.remove('show');
            alertMessage.innerHTML = '';
        });

        cancelBtn.addEventListener('click', () => {
            modal.classList.remove('show');
            alertMessage.innerHTML = '';
        });

        // Close modal when clicking outside
        modal.addEventListener('click', (e) => {
            if (e.target === modal) {
                modal.classList.remove('show');
                alertMessage.innerHTML = '';
            }
        });

        // Edit coupon
        document.addEventListener('click', (e) => {
            if (e.target.classList.contains('edit-coupon')) {
                const row = e.target.closest('tr');
                const vId = row.dataset.voucherId;

                submitBtn.disabled = true;
                submitBtn.innerHTML = '<span class="loading-spinner"></span> Loading...';

                fetch(`AdminCupon.aspx?action=get&id=${vId}`)
                    .then(response => {
                        if (!response.ok) throw new Error('Failed to fetch coupon');
                        return response.json();
                    })
                    .then(data => {
                        voucherId.value = data.VoucherID;
                        document.getElementById('couponCode').value = data.Code;
                        document.getElementById('voucherType').value = data.VoucherType;
                        document.getElementById('discountType').value = data.DiscountType;
                        document.getElementById('discountValue').value = data.DiscountValue;
                        document.getElementById('coinCost').value = data.CoinCost;
                        document.getElementById('expiryDate').value = data.ExpiryDate;
                        document.getElementById('status').value = data.Status;

                        modalTitle.textContent = 'Edit Coupon';
                        alertMessage.innerHTML = '';
                        modal.classList.add('show');
                        submitBtn.disabled = false;
                        submitBtn.textContent = 'Update Coupon';
                    })
                    .catch(error => {
                        showAlert('Error loading coupon: ' + error.message);
                        submitBtn.disabled = false;
                        submitBtn.textContent = 'Save Coupon';
                    });
            }
        });

        // Delete coupon
        document.addEventListener('click', (e) => {
            if (e.target.classList.contains('delete-coupon')) {
                const row = e.target.closest('tr');
                const vId = row.dataset.voucherId;

                if (confirm('Are you sure you want to delete this coupon? This action cannot be undone.')) {
                    e.target.disabled = true;
                    e.target.innerHTML = '<span class="loading-spinner"></span>';

                    fetch(`AdminCupon.aspx?action=delete&id=${vId}`, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded'
                        }
                    })
                        .then(response => {
                            if (!response.ok) throw new Error('Failed to delete coupon');
                            return response.json();
                        })
                        .then(data => {
                            if (data.success) {
                                row.style.animation = 'fadeOut 0.3s ease';
                                setTimeout(() => {
                                    row.remove();
                                    location.reload();
                                }, 300);
                            } else {
                                throw new Error(data.message || 'Failed to delete coupon');
                            }
                        })
                        .catch(error => {
                            alert('Error: ' + error.message);
                            e.target.disabled = false;
                            e.target.textContent = 'Delete';
                        });
                }
            }
        });

        // Save coupon
        couponForm.addEventListener('submit', (e) => {
            e.preventDefault();

            submitBtn.disabled = true;
            submitBtn.innerHTML = '<span class="loading-spinner"></span> Saving...';

            const formData = new FormData();
            formData.append('action', voucherId.value ? 'update' : 'create');
            formData.append('voucherId', voucherId.value);
            formData.append('code', document.getElementById('couponCode').value.trim());
            formData.append('voucherType', document.getElementById('voucherType').value);
            formData.append('discountType', document.getElementById('discountType').value);
            formData.append('discountValue', document.getElementById('discountValue').value);
            formData.append('coinCost', document.getElementById('coinCost').value);
            formData.append('expiryDate', document.getElementById('expiryDate').value);
            formData.append('status', document.getElementById('status').value);

            fetch('AdminCupon.aspx', {
                method: 'POST',
                body: formData
            })
                .then(response => {
                    if (!response.ok) throw new Error('Request failed');
                    return response.json();
                })
                .then(data => {
                    if (data.success) {
                        showAlert(data.message, 'success');
                        setTimeout(() => {
                            modal.classList.remove('show');
                            location.reload();
                        }, 1500);
                    } else {
                        showAlert(data.message || 'An error occurred');
                        submitBtn.disabled = false;
                        submitBtn.innerHTML = voucherId.value ? 'Update Coupon' : 'Save Coupon';
                    }
                })
                .catch(error => {
                    showAlert('Error: ' + error.message);
                    submitBtn.disabled = false;
                    submitBtn.innerHTML = voucherId.value ? 'Update Coupon' : 'Save Coupon';
                });
        });

        // Search functionality
        searchCoupons.addEventListener('input', (e) => {
            const query = e.target.value.toLowerCase();
            let visibleCount = 0;

            document.querySelectorAll('.coupon-row').forEach(row => {
                const text = row.textContent.toLowerCase();
                const isVisible = text.includes(query);
                row.style.display = isVisible ? '' : 'none';
                if (isVisible) visibleCount++;
            });

            updateEmptyState(visibleCount === 0);
        });

        // Filter dropdown
        moreBtn.addEventListener('click', () => {
            dropdownMenu.classList.toggle('show');
        });

        document.querySelectorAll('.dropdown-item').forEach(item => {
            item.addEventListener('click', (e) => {
                const action = e.target.dataset.action;
                let visibleCount = 0;

                document.querySelectorAll('.coupon-row').forEach(row => {
                    const status = row.dataset.status;
                    const isVisible = action === 'all' || status === action;
                    row.style.display = isVisible ? '' : 'none';
                    if (isVisible) visibleCount++;
                });

                updateEmptyState(visibleCount === 0);
                dropdownMenu.classList.remove('show');
            });
        });

        // Close dropdown when clicking outside
        document.addEventListener('click', (e) => {
            if (!e.target.closest('.more-btn') && !e.target.closest('.dropdown-menu')) {
                dropdownMenu.classList.remove('show');
            }
        });

        // Update empty state
        function updateEmptyState(isEmpty = false) {
            const emptyState = document.getElementById('emptyState');
            const tableWrapper = document.querySelector('.table-wrapper');

            if (isEmpty || couponsTableBody.children.length === 0) {
                emptyState.style.display = 'block';
                document.getElementById('couponsTable').style.display = 'none';
            } else {
                emptyState.style.display = 'none';
                document.getElementById('couponsTable').style.display = 'table';
            }
        }

        updateEmptyState();
    </script>
</asp:Content>