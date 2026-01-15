<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Week1_Practical1.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Forgot Password - ZinJaGO</title>
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

        .forgot-password-container {
            background: white;
            border-radius: 10px;
            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.2);
            width: 100%;
            max-width: 400px;
            padding: 40px;
        }

        .forgot-password-header {
            text-align: center;
            margin-bottom: 30px;
        }

        .forgot-password-header h1 {
            font-size: 28px;
            color: #333;
            margin-bottom: 10px;
        }

        .forgot-password-header p {
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
            border-color: #667eea;
            box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
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

        .loading {
            display: none;
            text-align: center;
            color: #666;
            font-size: 14px;
        }

        .spinner {
            display: inline-block;
            width: 16px;
            height: 16px;
            border: 2px solid #f3f3f3;
            border-top: 2px solid #667eea;
            border-radius: 50%;
            animation: spin 1s linear infinite;
            margin-right: 8px;
        }

        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="forgot-password-container">
            <div class="forgot-password-header">
                <h1>Reset Password</h1>
                <p>Enter your email address and we'll send you a link to reset your password</p>
            </div>

            <div class="message-alert success" id="successMessage" runat="server">
                <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
            </div>

            <div class="message-alert error" id="errorMessage" runat="server">
                <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
            </div>

            <div class="form-group">
                <label for="txtEmail">Email Address</label>
                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" placeholder="Enter your email address"></asp:TextBox>
            </div>

            <div class="loading" id="loadingIndicator">
                <span class="spinner"></span>
                <span>Sending reset link...</span>
            </div>

            <asp:Button ID="btnSendReset" runat="server" Text="Send Reset Link" CssClass="btn-reset" OnClick="btnSendReset_Click" />

            <div class="back-to-login">
                <a href="AdminLogin.aspx">← Back to Login</a>
            </div>
        </div>
    </form>
</body>
</html>