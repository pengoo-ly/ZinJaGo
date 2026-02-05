<%@ Page Title="" Language="C#" MasterPageFile="~/SuperAdmin/SuperAdmin.Master" AutoEventWireup="true" CodeBehind="SProfile.aspx.cs" Inherits="Week1_Practical1.SuperAdmin.SProfile" %>
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

    .delete-account-section {
        margin-top: 24px;
        padding-top: 24px;
        border-top: 1px solid rgba(0,0,0,0.05);
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

    .btn-modal-danger {
        background: linear-gradient(135deg, #ff6b6b 0%, #d63031 100%);
        color: white;
    }

    .btn-modal-danger:hover {
        opacity: 0.9;
    }

    /* Delete Confirmation Modal */
    .modal-header.danger {
        background: linear-gradient(135deg, #ff6b6b 0%, #d63031 100%);
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

        <!-- Delete Account Section -->
        <div class="delete-account-section">
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

    <!-- Right Column - Profile Update -->
    <div class="profile-update-card">
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
</div>

<script>
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

    function openDeleteModal() {
        document.getElementById('deleteModal').classList.add('active');
    }

    function closeDeleteModal() {
        document.getElementById('deleteModal').classList.remove('active');
    }

    // Close modals when clicking outside
    window.onclick = function (event) {
        const deleteModal = document.getElementById('deleteModal');

        if (event.target === deleteModal) deleteModal.classList.remove('active');
    }
</script>
</asp:Content>
