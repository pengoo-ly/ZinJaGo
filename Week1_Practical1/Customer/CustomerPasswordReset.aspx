<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerPasswordReset.aspx.cs" Inherits="Week1_Practical1.Customer_PasswordReset" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Reset Password - ZinJaGO</title>
    <link rel="icon" href="../Images/zinjago.png" type="image/png" />
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            min-height: 100vh;
            background: linear-gradient(135deg, #e0ccb1 0%, #e0ccb1 100%);
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 20px;
        }

        .reset-password-container {
            background: white;
            border-radius: 10px;
            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.2);
            width: 100%;
            max-width: 400px;
            padding: 40px;
        }

        .reset-password-header {
            text-align: center;
            margin-bottom: 30px;
        }

        .reset-password-header h1 {
            font-size: 28px;
            color: #333;
            margin-bottom: 10px;
        }

        .reset-password-header p {
            color: #666;
            font-size: 14px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .form-group label {
            display: block;
            font-size: 14px;
            font-weight: 600;
            color: #333;
            margin-bottom: 8px;
        }

        .form-group input {
            width: 100%;
            padding: 12px 15px;
            border: 1px solid #ddd;
            border-radius: 6px;
            font-size: 14px;
            transition: all 0.3s ease;
        }

        .form-group input:focus {
            outline: none;
            border-color: #1cb074;
            box-shadow: 0 0 0 3px rgba(28, 176, 116, 0.1);
        }

        .btn-reset {
            width: 100%;
            padding: 12px;
            background: linear-gradient(135deg, #1cb074 0%, #0f8452 100%);
            color: white;
            border: none;
            border-radius: 6px;
            font-size: 14px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
            margin-top: 20px;
        }

        .btn-reset:hover {
            transform: translateY(-2px);
            box-shadow: 0 10px 20px rgba(28, 176, 116, 0.3);
        }

        .btn-reset:active {
            transform: translateY(0);
        }

        .message-alert {
            padding: 12px 15px;
            border-radius: 6px;
            margin-bottom: 20px;
            font-size: 14px;
            display: none;
        }

        .message-alert.show {
            display: block;
        }

        .message-alert.success {
            background: rgba(76, 175, 80, 0.1);
            color: #2e7d32;
            border: 1px solid rgba(76, 175, 80, 0.3);
        }

        .message-alert.error {
            background: rgba(244, 67, 54, 0.1);
            color: #c62828;
            border: 1px solid rgba(244, 67, 54, 0.3);
        }

        .password-strength {
            margin-top: 8px;
            height: 4px;
            background: #ddd;
            border-radius: 4px;
            overflow: hidden;
        }

        .password-strength-bar {
            height: 100%;
            width: 0%;
            transition: width 0.3s ease, background-color 0.3s ease;
        }

        .back-to-login {
            text-align: center;
            margin-top: 20px;
        }

        .back-to-login a {
            color: #1cb074;
            text-decoration: none;
            font-size: 14px;
            font-weight: 600;
        }

        .back-to-login a:hover {
            text-decoration: underline;
        }

        .step-indicator {
            display: flex;
            gap: 10px;
            margin-bottom: 25px;
            justify-content: space-between;
        }

        .step {
            flex: 1;
            text-align: center;
        }

        .step-number {
            display: inline-block;
            width: 32px;
            height: 32px;
            background: #ddd;
            color: #666;
            border-radius: 50%;
            line-height: 32px;
            font-weight: 600;
            margin-bottom: 8px;
            font-size: 12px;
        }

        .step.active .step-number {
            background: #1cb074;
            color: white;
        }

        .step-label {
            font-size: 12px;
            color: #666;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="reset-password-container">
            <div class="reset-password-header">
                <h1>🔐 Reset Password</h1>
                <p>Follow the steps to reset your password</p>
            </div>

            <div class="step-indicator">
                <div class="step active" id="step1">
                    <div class="step-number">1</div>
                    <div class="step-label">Email</div>
                </div>
                <div class="step" id="step2">
                    <div class="step-number">2</div>
                    <div class="step-label">Code</div>
                </div>
                <div class="step" id="step3">
                    <div class="step-number">3</div>
                    <div class="step-label">Password</div>
                </div>
            </div>

            <div class="message-alert success" id="successMessage" runat="server">
                <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
            </div>

            <div class="message-alert error" id="errorMessage" runat="server">
                <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
            </div>

            <!-- Step 1: Email Verification -->
            <div id="emailPanel" runat="server" visible="true">
                <p style="color: #666; font-size: 13px; margin-bottom: 20px;">Enter your email address to receive a password reset code</p>
                <div class="form-group">
                    <label for="txtEmail">Email Address</label>
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" placeholder="your@email.com"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Email is required" ForeColor="#CC0000"></asp:RequiredFieldValidator>
                    <br />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Invalid email format" ForeColor="#CC0000" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                </div>

                <asp:Button ID="btnVerifyEmail" runat="server" Text="Send Verification Code" CssClass="btn-reset" OnClick="btnVerifyEmail_Click" />
            </div>

            <!-- Step 2: Code Verification -->
            <div id="codePanel" runat="server" visible="false">
                <p style="color: #666; font-size: 13px; margin-bottom: 20px;">Enter the 6-digit code we sent to your email</p>
                <div class="form-group">
                    <label for="txtResetCode">Verification Code</label>
                    <asp:TextBox ID="txtResetCode" runat="server" placeholder="000000" TextMode="SingleLine"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvResetCode" runat="server" ControlToValidate="txtResetCode" Display="Dynamic" ErrorMessage="Verification code is required" ForeColor="#CC0000"></asp:RequiredFieldValidator>
                </div>

                <asp:Button ID="btnVerifyCode" runat="server" Text="Verify Code" CssClass="btn-reset" OnClick="btnVerifyCode_Click" />
            </div>

            <!-- Step 3: New Password -->
            <div id="passwordPanel" runat="server" visible="false">
                <p style="color: #666; font-size: 13px; margin-bottom: 20px;">Create a new password for your account</p>
                <div class="form-group">
                    <label for="txtNewPassword">New Password</label>
                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" placeholder="Enter a strong password"></asp:TextBox>
                    <div class="password-strength">
                        <div class="password-strength-bar" id="passwordStrengthBar"></div>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword" Display="Dynamic" ErrorMessage="Password is required" ForeColor="#CC0000"></asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label for="txtConfirmPassword">Confirm Password</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" placeholder="Confirm your password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" Display="Dynamic" ErrorMessage="Please confirm your password" ForeColor="#CC0000"></asp:RequiredFieldValidator>
                    <br />
                    <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtNewPassword" Display="Dynamic" ErrorMessage="Passwords do not match" ForeColor="#CC0000" Operator="Equal"></asp:CompareValidator>
                </div>

                <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" CssClass="btn-reset" OnClick="btnResetPassword_Click" />
            </div>

            <div class="back-to-login">
                <a href="../Login.aspx">← Back to Login</a>
            </div>

            <br />
            <asp:ValidationSummary ID="vsReset" runat="server" ForeColor="#CC0000" HeaderText="Please fix the following errors:" />
        </div>
    </form>

    <script type="text/javascript">
        function updateStepIndicator(step) {
            for (var i = 1; i <= 3; i++) {
                var stepElement = document.getElementById('step' + i);
                if (i <= step) {
                    stepElement.classList.add('active');
                } else {
                    stepElement.classList.remove('active');
                }
            }
        }

        function checkPasswordStrength() {
            var password = document.getElementById('<%= txtNewPassword.ClientID %>').value;
            var strengthBar = document.getElementById('passwordStrengthBar');
            var strength = 0;

            if (password.length >= 8) strength += 25;
            if (password.length >= 12) strength += 25;
            if (/[a-z]/.test(password) && /[A-Z]/.test(password)) strength += 25;
            if (/[0-9]/.test(password) && /[!@#$%^&*]/.test(password)) strength += 25;

            strengthBar.style.width = strength + '%';
            
            if (strength <= 25) strengthBar.style.backgroundColor = '#f44336';
            else if (strength <= 50) strengthBar.style.backgroundColor = '#ff9800';
            else if (strength <= 75) strengthBar.style.backgroundColor = '#ffeb3b';
            else strengthBar.style.backgroundColor = '#1cb074';
        }

        var passwordInput = document.getElementById('<%= txtNewPassword.ClientID %>');
        if (passwordInput) {
            passwordInput.addEventListener('keyup', checkPasswordStrength);
        }
    </script>
</body>
</html>
