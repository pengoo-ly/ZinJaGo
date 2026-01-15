<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Week1_Practical1.Profile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .profile-container {
            display: grid;
            grid-template-columns: 1fr 2fr;
            gap: 28px;
            max-width: 1200px;
            margin: 0 auto;
        }

        /* Left Column - Profile Card */
        .profile-card {
            background: var(--card);
            border-radius: 14px;
            padding: 28px;
            box-shadow: var(--shadow);
            text-align: center;
            height: fit-content;
        }

        .profile-avatar {
            width: 120px;
            height: 120px;
            border-radius: 50%;
            background: linear-gradient(135deg, #1cb074 0%, #0f8452 100%);
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 auto 16px;
            font-size: 48px;
            font-weight: 700;
            color: white;
            overflow: hidden;
            object-fit: cover;
        }

        .profile-avatar img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }

        .profile-name {
            font-size: 20px;
            font-weight: 600;
            color: var(--text);
            margin-bottom: 4px;
        }

        .profile-email {
            font-size: 13px;
            color: var(--muted);
            margin-bottom: 16px;
            word-break: break-all;
        }

        .copy-btn {
            background: transparent;
            border: none;
            color: var(--accent);
            cursor: pointer;
            font-size: 14px;
            padding: 0;
            text-decoration: none;
            margin-left: 6px;
        }

        .social-media {
            margin: 20px 0;
            padding: 20px 0;
            border-top: 1px solid rgba(0,0,0,0.05);
            border-bottom: 1px solid rgba(0,0,0,0.05);
        }

        .social-label {
            font-size: 12px;
            color: var(--muted);
            margin-bottom: 10px;
            display: block;
        }

        .social-icons {
            display: flex;
            gap: 10px;
            justify-content: center;
            flex-wrap: wrap;
        }

        .social-icon {
            width: 36px;
            height: 36px;
            border-radius: 6px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 16px;
            cursor: pointer;
            transition: transform 0.2s ease;
            border: 1px solid rgba(0,0,0,0.1);
        }

        .social-icon:hover {
            transform: translateY(-2px);
        }

        .logout-section {
            margin-top: 24px;
            padding-top: 24px;
            border-top: 1px solid rgba(0,0,0,0.05);
            display: flex;
            flex-direction: column;
            gap: 12px;
        }

        .btn-logout {
            width: 100%;
            padding: 12px;
            background: linear-gradient(135deg, #dc4d4d 0%, #a83a3a 100%);
            color: white;
            border: none;
            border-radius: 6px;
            font-size: 14px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s ease;
        }

        .btn-logout:hover {
            opacity: 0.9;
            transform: translateY(-2px);
        }

        .btn-logout:active {
            transform: translateY(0);
        }

        .btn-delete-account {
            width: 100%;
            padding: 12px;
            background: linear-gradient(135deg, #ff6b6b 0%, #d63031 100%);
            color: white;
            border: none;
            border-radius: 6px;
            font-size: 14px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s ease;
            margin-top: 8px;
        }

        .btn-delete-account:hover {
            opacity: 0.9;
            transform: translateY(-2px);
        }

        /* Right Column - Profile Update */
        .profile-update-card {
            background: var(--card);
            border-radius: 14px;
            padding: 28px;
            box-shadow: var(--shadow);
        }

        .profile-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 24px;
        }

        .profile-header h2 {
            font-size: 18px;
            font-weight: 600;
            color: var(--text);
            margin: 0;
        }

        .edit-icon {
            background: none;
            border: none;
            color: var(--muted);
            font-size: 18px;
            cursor: pointer;
            padding: 6px;
        }

        .edit-icon:hover {
            color: var(--accent);
        }

        .profile-update-content {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 20px;
        }

        .form-group {
            display: flex;
            flex-direction: column;
        }

        .form-group.full {
            grid-column: 1 / -1;
        }

        .form-group label {
            font-size: 13px;
            font-weight: 600;
            color: var(--text);
            margin-bottom: 8px;
        }

        .form-group input,
        .form-group textarea,
        .form-group select {
            padding: 10px 12px;
            border: 1px solid rgba(0,0,0,0.1);
            border-radius: 6px;
            font-size: 13px;
            background: transparent;
            color: var(--text);
            font-family: inherit;
        }

        .form-group input::placeholder,
        .form-group textarea::placeholder {
            color: var(--muted);
        }

        .form-group input:focus,
        .form-group textarea:focus,
        .form-group select:focus {
            outline: none;
            border-color: var(--accent);
            box-shadow: 0 0 0 2px rgba(28, 176, 116, 0.1);
        }

        .form-group textarea {
            resize: vertical;
            min-height: 80px;
        }

        .form-group input:disabled,
        .form-group textarea:disabled {
            opacity: 0.6;
            cursor: not-allowed;
        }

        .upload-section {
            display: flex;
            align-items: flex-end;
            gap: 12px;
            margin-bottom: 20px;
        }

        .upload-preview {
            width: 60px;
            height: 60px;
            border-radius: 8px;
            background: rgba(0,0,0,0.05);
            display: flex;
            align-items: center;
            justify-content: center;
            overflow: hidden;
            font-size: 28px;
            flex-shrink: 0;
        }

        .upload-preview img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }

        .upload-buttons {
            display: flex;
            gap: 8px;
        }

        .btn-upload {
            padding: 8px 16px;
            background: var(--accent);
            color: white;
            border: none;
            border-radius: 6px;
            font-size: 13px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s ease;
        }

        .btn-upload:hover {
            opacity: 0.9;
        }

        .btn-delete {
            padding: 8px 16px;
            background: transparent;
            color: #dc4d4d;
            border: 1px solid rgba(220, 77, 77, 0.3);
            border-radius: 6px;
            font-size: 13px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s ease;
        }

        .btn-delete:hover {
            background: rgba(220, 77, 77, 0.1);
        }

        .file-input {
            display: none;
        }

        .action-buttons {
            display: flex;
            gap: 12px;
            margin-top: 24px;
        }

        .btn-save {
            flex: 1;
            padding: 12px;
            background: var(--accent);
            color: white;
            border: none;
            border-radius: 6px;
            font-size: 14px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s ease;
        }

        .btn-save:hover {
            opacity: 0.9;
        }

        .btn-cancel {
            flex: 1;
            padding: 12px;
            background: transparent;
            color: var(--text);
            border: 1px solid rgba(0,0,0,0.1);
            border-radius: 6px;
            font-size: 14px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s ease;
        }

        .btn-cancel:hover {
            background: rgba(0,0,0,0.03);
        }

        .message-alert {
            padding: 12px 16px;
            border-radius: 6px;
            margin-bottom: 16px;
            font-size: 13px;
            display: none;
        }

        .message-alert.show {
            display: block;
        }

        .message-alert.success {
            background: rgba(60, 204, 60, 0.1);
            color: #3c4c3c;
            border: 1px solid rgba(60, 204, 60, 0.2);
        }

        .message-alert.error {
            background: rgba(220, 77, 77, 0.1);
            color: #8b3c3c;
            border: 1px solid rgba(220, 77, 77, 0.2);
        }

        /* Credit Cards Section */
        .tabs {
            display: flex;
            gap: 0;
            border-bottom: 2px solid rgba(0,0,0,0.08);
            margin-bottom: 24px;
        }

        .tab-btn {
            padding: 12px 20px;
            background: transparent;
            border: none;
            color: var(--muted);
            cursor: pointer;
            font-size: 14px;
            font-weight: 600;
            border-bottom: 3px solid transparent;
            transition: all 0.2s ease;
            position: relative;
            bottom: -2px;
        }

        .tab-btn.active {
            color: var(--accent);
            border-bottom-color: var(--accent);
        }

        .tab-btn:hover {
            color: var(--accent);
        }

        .tab-content {
            display: none;
        }

        .tab-content.active {
            display: block;
        }

        .credit-cards-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
            gap: 20px;
            margin-bottom: 24px;
        }

        /* Credit Cards Section - Updated Colors */
        .credit-card {
            background: linear-gradient(135deg, #4FA392 0%, #76B29F 100%);
            border-radius: 12px;
            padding: 24px;
            color: white;
            position: relative;
            overflow: hidden;
            box-shadow: 0 4px 12px rgba(79, 163, 146, 0.2);
        }

        .credit-card::before {
            content: '';
            position: absolute;
            top: -50%;
            right: -50%;
            width: 200px;
            height: 200px;
            background: rgba(255, 255, 255, 0.1);
            border-radius: 50%;
        }

        .card-logo {
            font-size: 24px;
            margin-bottom: 20px;
            position: relative;
            z-index: 1;
        }

        .card-number {
            font-size: 18px;
            letter-spacing: 2px;
            margin-bottom: 16px;
            position: relative;
            z-index: 1;
            font-family: 'Courier New', monospace;
        }

        .card-info {
            display: flex;
            justify-content: space-between;
            align-items: flex-end;
            position: relative;
            z-index: 1;
        }

        .card-holder {
            font-size: 13px;
            color: rgba(255, 255, 255, 0.8);
            margin-bottom: 4px;
        }

        .card-holder-name {
            font-size: 14px;
            font-weight: 600;
        }

        .card-expiry {
            text-align: right;
        }

        .card-expiry-label {
            font-size: 10px;
            color: rgba(255, 255, 255, 0.8);
            margin-bottom: 4px;
        }

        .card-expiry-date {
            font-size: 14px;
            font-weight: 600;
        }

        .card-actions {
            position: absolute;
            top: 12px;
            right: 12px;
            display: flex;
            gap: 8px;
        }

        .card-btn {
            width: 32px;
            height: 32px;
            border-radius: 50%;
            border: none;
            background: rgba(255, 255, 255, 0.2);
            color: white;
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 16px;
            transition: all 0.2s ease;
        }

        .card-btn:hover {
            background: rgba(255, 255, 255, 0.3);
        }

        .add-card-btn {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 12px;
            background: rgba(79, 163, 146, 0.05);
            border: 2px dashed #4FA392;
            border-radius: 12px;
            padding: 60px 24px;
            cursor: pointer;
            transition: all 0.2s ease;
        }

        .add-card-btn:hover {
            border-color: #4FA392;
            background: rgba(79, 163, 146, 0.1);
        }

        /* Modal */
        .modal {
            position: fixed;
            z-index: 1000;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5);
            display: none;
            align-items: center;
            justify-content: center;
        }

        .modal.active {
            display: flex;
        }

        .modal-content {
            background-color: var(--card);
            border-radius: 12px;
            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.3);
            max-width: 500px;
            width: 90%;
            overflow: hidden;
        }

        .modal-header {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 20px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .modal-header h2 {
            margin: 0;
            font-size: 20px;
        }

        .close {
            color: white;
            font-size: 28px;
            font-weight: bold;
            cursor: pointer;
            transition: all 0.2s ease;
        }

        .close:hover {
            opacity: 0.8;
        }

        .modal-body {
            padding: 24px;
            color: var(--text);
        }

        .modal-footer {
            padding: 20px;
            background: rgba(0, 0, 0, 0.02);
            display: flex;
            gap: 12px;
            justify-content: flex-end;
        }

        .btn-modal {
            padding: 10px 20px;
            border-radius: 6px;
            font-size: 14px;
            font-weight: 600;
            cursor: pointer;
            border: none;
            transition: all 0.2s ease;
        }

        .btn-modal-cancel {
            background: transparent;
            color: var(--text);
            border: 1px solid rgba(0, 0, 0, 0.1);
        }

        .btn-modal-cancel:hover {
            background: rgba(0, 0, 0, 0.03);
        }

        .btn-modal-primary {
            background: var(--accent);
            color: white;
        }

        .btn-modal-primary:hover {
            opacity: 0.9;
        }

        /* Delete Confirmation Modal */
        .modal-header.danger {
            background: linear-gradient(135deg, #ff6b6b 0%, #d63031 100%);
        }

        .btn-modal-danger {
            background: linear-gradient(135deg, #ff6b6b 0%, #d63031 100%);
            color: white;
        }

        .btn-modal-danger:hover {
            opacity: 0.9;
        }

        @media (max-width: 768px) {
            .profile-container {
                grid-template-columns: 1fr;
            }

            .profile-update-content {
                grid-template-columns: 1fr;
            }

            .form-group.full {
                grid-column: 1;
            }

            .credit-cards-grid {
                grid-template-columns: 1fr;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="profile-container">
        <!-- Left Column - Profile Card -->
        <div class="profile-card">
            <div class="profile-avatar" id="avatarDisplay">
                <asp:Label ID="lblInitial" runat="server" Text="A"></asp:Label>
            </div>

            <div class="profile-name"><asp:Label ID="lblAdminName" runat="server" Text="Admin User"></asp:Label></div>
            <div class="profile-email">
                <asp:Label ID="lblAdminEmail" runat="server" Text="admin@example.com"></asp:Label>
                <button class="copy-btn" onclick="copyToClipboard(this); return false;">📋</button>
            </div>

            <div class="social-media">
                <span class="social-label">Linked with Social media</span>
                <div class="social-icons">
                    <div class="social-icon" title="Google">G</div>
                    <div class="social-icon" title="Facebook">f</div>
                    <div class="social-icon" title="Twitter">𝕏</div>
                </div>
                <div style="font-size: 12px; color: var(--muted); margin-top: 12px;">
                    <input type="checkbox" id="chkSocialMedia" />
                    <label for="chkSocialMedia" style="display: inline; margin-left: 4px;">Social media</label>
                </div>
            </div>

            <!-- Logout Section -->
            <div class="logout-section">
                <asp:Button ID="btnLogout" runat="server" Text="🚪 Logout" CssClass="btn-logout" OnClick="btnLogout_Click" />
                <asp:Button ID="btnDeleteAccount" runat="server" Text="⚠️ Delete Account" CssClass="btn-delete-account" OnClick="btnDeleteAccount_Click" />
            </div>

            <!-- Delete Account Confirmation Modal -->
            <div class="modal" id="deleteModal">
                <div class="modal-content">
                    <div class="modal-header danger">
                        <h2>Delete Account?</h2>
                        <span class="close" onclick="closeDeleteModal()">&times;</span>
                    </div>
                    <div class="modal-body">
                        <p><strong>⚠️ Warning:</strong> This action cannot be undone.</p>
                        <p>Deleting your account will:</p>
                        <ul>
                            <li>Permanently delete all your data</li>
                            <li>Remove your profile and settings</li>
                            <li>Log you out of the system</li>
                        </ul>
                        <p>Are you sure you want to continue?</p>
                    </div>
                    <div class="modal-footer">
                        <button class="btn-modal btn-modal-cancel" onclick="closeDeleteModal()">Cancel</button>
                        <asp:Button ID="btnConfirmDelete" runat="server" Text="Yes, Delete My Account" CssClass="btn-modal btn-modal-danger" OnClick="btnConfirmDelete_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Right Column - Profile Update & Credit Cards -->
        <div class="profile-update-card">
            <!-- Tabs -->
            <div class="tabs">
                <button type="button" class="tab-btn active" data-tab="profile">Profile Information</button>
                <button type="button" class="tab-btn" data-tab="cards">Credit Cards</button>
            </div>

            <!-- Profile Tab -->
            <div id="profile-tab" class="tab-content active">
                <div class="profile-header">
                    <h2>Profile Update</h2>
                    <button type="button" class="edit-icon" title="Edit profile">✎</button>
                </div>

                <div class="upload-section">
                    <div class="upload-preview" id="uploadPreview">
                        <asp:Label ID="lblAvatarPreview" runat="server" Text="A"></asp:Label>
                    </div>
                    <div class="upload-buttons">
                        <button type="button" class="btn-upload" onclick="document.getElementById('<%= fileUpload.ClientID %>').click(); return false;">Upload Now</button>
                        <asp:Button ID="btnDeletePhoto" runat="server" Text="Delete" CssClass="btn-delete" OnClick="btnDeletePhoto_Click" />
                    </div>
                    <asp:FileUpload ID="fileUpload" runat="server" CssClass="file-input" accept="image/*" onchange="handleFileSelect(this)" />
                </div>

                <!-- Profile Update Form -->
                <div class="profile-update-content">
                    <div class="form-group">
                        <label for="txtFirstName">First Name</label>
                        <asp:TextBox ID="txtFirstName" runat="server" placeholder="First name"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="txtLastName">Last Name</label>
                        <asp:TextBox ID="txtLastName" runat="server" placeholder="Last name"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="txtEmail">E-mail</label>
                        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" placeholder="Email address" Enabled="false"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="txtPasswordDisplay">Password</label>
                        <asp:TextBox ID="txtPasswordDisplay" runat="server" TextMode="Password" placeholder="Password" Enabled="false"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="txtPhoneNumber">Phone Number</label>
                        <asp:TextBox ID="txtPhoneNumber" runat="server" placeholder="+1 (555) 123-4567"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="txtDateOfBirth">Date of Birth</label>
                        <asp:TextBox ID="txtDateOfBirth" runat="server" TextMode="Date"></asp:TextBox>
                    </div>

                    <div class="form-group full">
                        <label for="txtLocation">Location</label>
                        <asp:TextBox ID="txtLocation" runat="server" placeholder="Enter your location"></asp:TextBox>
                    </div>

                    <div class="form-group full">
                        <label for="txtBiography">Biography</label>
                        <asp:TextBox ID="txtBiography" runat="server" TextMode="MultiLine" placeholder="Enter a biography about you"></asp:TextBox>
                    </div>
                </div>

                <!-- Messages -->
                <div class="message-alert success" id="successMessage">✓ Profile updated successfully!</div>
                <div class="message-alert error" id="errorMessage"></div>

                <!-- Action Buttons -->
                <div class="action-buttons">
                    <asp:Button ID="btnSaveProfile" runat="server" Text="Save Changes" CssClass="btn-save" OnClick="btnSaveProfile_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-cancel" OnClick="btnCancel_Click" />
                </div>
            </div>

            <!-- Credit Cards Tab -->
            <div id="cards-tab" class="tab-content">
                <div class="profile-header">
                    <h2>My Credit Cards</h2>
                </div>

                <div class="credit-cards-grid" id="creditCardsGrid">
                    <!-- Credit cards will be loaded here -->
                </div>

                <!-- Add Card Button -->
                <div class="credit-cards-grid">
                    <div class="add-card-btn" onclick="openAddCardModal()">
                        <div class="add-card-icon">➕</div>
                        <div class="add-card-text">
                            <h3>Add New Card</h3>
                            <p>Add a new credit or debit card</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Add Credit Card Modal -->
    <div class="modal" id="addCardModal">
        <div class="modal-content">
            <div class="modal-header">
                <h2>Add Credit Card</h2>
                <span class="close" onclick="closeAddCardModal()">&times;</span>
            </div>
            <div class="modal-body">
                <div class="form-group full">
                    <label for="txtCardName">Card Holder Name</label>
                    <asp:TextBox ID="txtCardName" runat="server" placeholder="John Doe"></asp:TextBox>
                </div>

                <div class="form-group full">
                    <label for="txtCardNumber">Card Number</label>
                    <asp:TextBox ID="txtCardNumber" runat="server" placeholder="1234 5678 9012 3456" maxlength="19"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtCardExpiry">Expiry Date</label>
                    <asp:TextBox ID="txtCardExpiry" runat="server" placeholder="MM/YY" maxlength="5"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtCardCVV">CVV</label>
                    <asp:TextBox ID="txtCardCVV" runat="server" placeholder="123" maxlength="3" TextMode="Password"></asp:TextBox>
                </div>
            </div>
            <div class="modal-footer">
                <button class="btn-modal btn-modal-cancel" onclick="closeAddCardModal()">Cancel</button>
                <asp:Button ID="btnAddCard" runat="server" Text="Add Card" CssClass="btn-modal btn-modal-primary" OnClick="btnAddCard_Click" />
            </div>
        </div>
    </div>

    <!-- Delete Card Confirmation Modal -->
    <div class="modal" id="deleteCardModal">
        <div class="modal-content">
            <div class="modal-header danger">
                <h2>Delete Card?</h2>
                <span class="close" onclick="closeDeleteCardModal()">&times;</span>
            </div>
            <div class="modal-body">
                <p>Are you sure you want to delete this credit card? This action cannot be undone.</p>
                <p id="cardToDeleteInfo" style="color: var(--accent); font-weight: 600;"></p>
            </div>
            <div class="modal-footer">
                <button class="btn-modal btn-modal-cancel" onclick="closeDeleteCardModal()">Cancel</button>
                <asp:Button ID="btnConfirmDeleteCard" runat="server" Text="Yes, Delete Card" CssClass="btn-modal btn-modal-danger" OnClick="btnConfirmDeleteCard_Click" />
            </div>
        </div>
    </div>

    <script>
        let cardToDeleteId = null;

        function copyToClipboard(btn) {
            const email = btn.previousElementSibling.textContent.trim();
            navigator.clipboard.writeText(email).then(() => {
                btn.textContent = '✓';
                setTimeout(() => {
                    btn.textContent = '📋';
                }, 2000);
            });
        }

        function handleFileSelect(input) {
            if (input.files && input.files[0]) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    const preview = document.getElementById('uploadPreview');
                    preview.innerHTML = '<img src="' + e.target.result + '" />';
                };
                reader.readAsDataURL(input.files[0]);
            }
        }

        function showMessage(message, isSuccess = true) {
            const successMsg = document.getElementById('successMessage');
            const errorMsg = document.getElementById('errorMessage');

            if (isSuccess) {
                successMsg.textContent = '✓ ' + message;
                successMsg.classList.add('show');
                setTimeout(() => {
                    successMsg.classList.remove('show');
                }, 3000);
            } else {
                errorMsg.textContent = '✗ ' + message;
                errorMsg.classList.add('show');
                setTimeout(() => {
                    errorMsg.classList.remove('show');
                }, 3000);
            }
        }

        // Tab switching
        document.querySelectorAll('.tab-btn').forEach(btn => {
            btn.addEventListener('click', function () {
                const tabName = this.dataset.tab;

                // Remove active class from all tabs and contents
                document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
                document.querySelectorAll('.tab-content').forEach(c => c.classList.remove('active'));

                // Add active class to clicked tab
                this.classList.add('active');
                document.getElementById(tabName + '-tab').classList.add('active');

                // Load credit cards when switching to cards tab
                if (tabName === 'cards') {
                    loadCreditCards();
                }
            });
        });

        function openDeleteModal() {
            document.getElementById('deleteModal').classList.add('active');
        }

        function closeDeleteModal() {
            document.getElementById('deleteModal').classList.remove('active');
        }

        function openAddCardModal() {
            clearCardForm();
            document.getElementById('addCardModal').classList.add('active');
        }

        function closeAddCardModal() {
            document.getElementById('addCardModal').classList.remove('active');
        }

        function openDeleteCardModal(cardId, cardNumber) {
            cardToDeleteId = cardId;
            document.getElementById('cardToDeleteInfo').textContent = 'Card: ' + cardNumber;
            document.getElementById('deleteCardModal').classList.add('active');
        }

        function closeDeleteCardModal() {
            document.getElementById('deleteCardModal').classList.remove('active');
            cardToDeleteId = null;
        }

        function clearCardForm() {
            document.getElementById('<%= txtCardName.ClientID %>').value = '';
            document.getElementById('<%= txtCardNumber.ClientID %>').value = '';
            document.getElementById('<%= txtCardExpiry.ClientID %>').value = '';
            document.getElementById('<%= txtCardCVV.ClientID %>').value = '';
        }

        function loadCreditCards() {
            // This will be populated by server-side code
            __doPostBack('loadCards', '');
        }

        function formatCardNumber(number) {
            return number.replace(/\s/g, '').replace(/(\d{4})/g, '$1 ').trim();
        }

        function displayCreditCard(card) {
            const maskedNumber = '**** **** **** ' + card.cardNumber.slice(-4);
            const colors = [
                'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                'linear-gradient(135deg, #f093fb 0%, #f5576c 100%)',
                'linear-gradient(135deg, #4facfe 0%, #00f2fe 100%)',
                'linear-gradient(135deg, #43e97b 0%, #38f9d7 100%)',
            ];
            const randomColor = colors[Math.floor(Math.random() * colors.length)];

            return `
                <div class="credit-card" style="background: ${randomColor};">
                    <div class="card-logo">💳</div>
                    <div class="card-number">${maskedNumber}</div>
                    <div class="card-info">
                        <div>
                            <div class="card-holder">Card Holder</div>
                            <div class="card-holder-name">${card.cardName}</div>
                        </div>
                        <div class="card-expiry">
                            <div class="card-expiry-label">Expires</div>
                            <div class="card-expiry-date">${card.expireDate}</div>
                        </div>
                    </div>
                    <div class="card-actions">
                        <button class="card-btn" onclick="openDeleteCardModal('${card.cardId}', '${maskedNumber}')" title="Delete">🗑️</button>
                    </div>
                </div>
            `;
        }

        // Close modals when clicking outside
        window.onclick = function (event) {
            const deleteModal = document.getElementById('deleteModal');
            const addCardModal = document.getElementById('addCardModal');
            const deleteCardModal = document.getElementById('deleteCardModal');

            if (event.target === deleteModal) deleteModal.classList.remove('active');
            if (event.target === addCardModal) addCardModal.classList.remove('active');
            if (event.target === deleteCardModal) deleteCardModal.classList.remove('active');
        }
    </script>
</asp:Content>