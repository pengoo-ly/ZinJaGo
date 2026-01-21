<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="Week1_Practical1.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Reset Password - ZinJaGO</title>
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
            background: #F8F5F0;
            border-radius: 10px;
            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.15);
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
            color: #4FA392;
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
            color: #4FA392;
            margin-bottom: 8px;
        }

        .form-group input {
            width: 100%;
            padding: 12px 15px;
            border: 1px solid #C5CEB8;
            border-radius: 6px;
            font-size: 14px;
            background: #FFFFFF;
            transition: all 0.3s ease;
        }

        .form-group input:focus {
            outline: none;
            border-color: #4FA392;
            box-shadow: 0 0 0 3px rgba(79, 163, 146, 0.1);
        }

        .btn-reset {
            width: 100%;
            padding: 12px;
            background: linear-gradient(135deg, #4FA392 0%, #76B29F 100%);
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
            box-shadow: 0 10px 20px rgba(79, 163, 146, 0.3);
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
            background: rgba(79, 163, 146, 0.1);
            color: #2d6a5c;
            border: 1px solid rgba(79, 163, 146, 0.3);
        }

        .message-alert.error {
            background: rgba(220, 77, 77, 0.1);
            color: #8b3c3c;
            border: 1px solid rgba(220, 77, 77, 0.2);
        }

        .back-to-login {
            text-align: center;
            margin-top: 20px;
        }

        .back-to-login a {
            color: #4FA392;
            text-decoration: none;
            font-size: 14px;
            font-weight: 600;
        }

        .back-to-login a:hover {
            text-decoration: underline;
        }

        .password-strength {
            font-size: 12px;
            margin-top: 5px;
            padding: 8px;
            border-radius: 4px;
            display: none;
        }

        .password-strength.weak {
            background: rgba(220, 77, 77, 0.1);
            color: #c62828;
            display: block;
        }

        .password-strength.fair {
            background: rgba(198, 180, 100, 0.1);
            color: #8b7d3a;
            display: block;
        }

        .password-strength.good {
            background: rgba(79, 163, 146, 0.1);
            color: #2d6a5c;
            display: block;
        }

        .password-strength.strong {
            background: rgba(79, 163, 146, 0.1);
            color: #2d6a5c;
            display: block;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="reset-password-container">
            <div class="reset-password-header">
                <h1>🔑 Create New Password</h1>
                <p>Enter your new password below</p>
            </div>

            <div class="message-alert success" id="successMessage" runat="server">
                <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
            </div>

            <div class="message-alert error" id="errorMessage" runat="server">
                <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
            </div>

            <div class="form-group">
                <label for="txtNewPassword">New Password</label>
                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" placeholder="Enter new password" onkeyup="checkPasswordStrength(this.value)"></asp:TextBox>
                <div class="password-strength" id="passwordStrength"></div>
            </div>

            <div class="form-group">
                <label for="txtConfirmPassword">Confirm Password</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" placeholder="Confirm new password"></asp:TextBox>
            </div>

            <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" CssClass="btn-reset" OnClick="btnResetPassword_Click" />

            <div class="back-to-login">
                <a href="Login.aspx">← Back to Login</a>
            </div>
        </div>
    </form>

    <script>
        function checkPasswordStrength(password) {
            const strengthDiv = document.getElementById('passwordStrength');
            
            if (!password) {
                strengthDiv.className = '';
                return;
            }

            let strength = 0;
            const hasLower = /[a-z]/.test(password);
            const hasUpper = /[A-Z]/.test(password);
            const hasNumber = /[0-9]/.test(password);
            const hasSpecial = /[!@#$%^&*]/.test(password);

            if (hasLower) strength++;
            if (hasUpper) strength++;
            if (hasNumber) strength++;
            if (hasSpecial) strength++;
            if (password.length >= 12) strength++;

            strengthDiv.className = 'password-strength';

            if (strength < 2) {
                strengthDiv.className = 'password-strength weak';
                strengthDiv.textContent = '⚠ Weak password';
            } else if (strength < 3) {
                strengthDiv.className = 'password-strength fair';
                strengthDiv.textContent = '→ Fair password';
            } else if (strength < 4) {
                strengthDiv.className = 'password-strength good';
                strengthDiv.textContent = '✓ Good password';
            } else {
                strengthDiv.className = 'password-strength strong';
                strengthDiv.textContent = '✓✓ Strong password';
            }
        }
    </script>
</body>
</html>