<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerSignup.aspx.cs" Inherits="Week1_Practical1.CustomerSignup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Sign Up - ZinJaGO</title>
    <link rel="icon" href="Images/zinjago.png" type="image/png" />
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            height: 100vh;
            overflow: hidden;
            background: #f5f5f5;
        }

        .login-container {
            display: grid;
            grid-template-columns: 1fr 1fr;
            height: 100vh;
            gap: 0;
        }

        .login-form-section {
            background: #f6efe2;
            display: flex;
            flex-direction: column;
            justify-content: center;
            padding: 60px 50px;
            position: relative;
            z-index: 10;
            overflow-y: auto;
        }

        .logo {
            position: absolute;
            top: 30px;
            left: 50px;
            width: 50px;
            height: 50px;
            background-size: contain;
            background-repeat: no-repeat;
            background-position: center;
        }

        .logo img {
            width: 50px;
            height: 50px;
            object-fit: contain;
        }

        .login-form-section h1 {
            font-size: 42px;
            color: #222;
            margin-bottom: 15px;
            font-weight: 600;
        }

        .login-form-section p {
            color: #666;
            font-size: 16px;
            margin-bottom: 30px;
            line-height: 1.5;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .form-group label {
            display: block;
            font-size: 14px;
            font-weight: 600;
            color: #222;
            margin-bottom: 10px;
        }

        .form-group input[type="text"],
        .form-group input[type="email"],
        .form-group input[type="password"] {
            width: 100%;
            padding: 14px 16px;
            border: 1px solid #ddd;
            border-radius: 8px;
            font-size: 15px;
            transition: all 0.3s ease;
            background: white;
        }

        .form-group input[type="text"]:focus,
        .form-group input[type="email"]:focus,
        .form-group input[type="password"]:focus {
            outline: none;
            border-color: #1cb074;
            box-shadow: 0 0 0 3px rgba(28, 176, 116, 0.1);
        }

        .form-row {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 20px;
        }

        .signup-button {
            width: 100%;
            padding: 16px;
            background: linear-gradient(135deg, #1cb074 0%, #0f8452 100%);
            color: white;
            border: none;
            border-radius: 8px;
            font-size: 16px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
            box-shadow: 0 4px 15px rgba(28, 176, 116, 0.2);
        }

        .signup-button:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(28, 176, 116, 0.3);
        }

        .signup-button:disabled {
            opacity: 0.6;
            cursor: not-allowed;
        }

        .login-link {
            text-align: center;
            margin-top: 20px;
            color: #666;
            font-size: 14px;
        }

        .login-link a {
            color: #1cb074;
            font-weight: 600;
            text-decoration: none;
            transition: color 0.3s ease;
        }

        .login-link a:hover {
            color: #0f8452;
        }

        .error-message {
            background: #fee;
            border: 1px solid #fcc;
            color: #c33;
            padding: 12px 16px;
            border-radius: 6px;
            margin-bottom: 25px;
            display: none;
            font-size: 14px;
        }

        .error-message.show {
            display: block;
        }

        .success-message {
            background: #efe;
            border: 1px solid #cfc;
            color: #3c3;
            padding: 12px 16px;
            border-radius: 6px;
            margin-bottom: 25px;
            display: none;
            font-size: 14px;
        }

        .success-message.show {
            display: block;
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

        /* Right Side - Image */
        .login-image-section {
            background: #f6efe2;
            position: relative;
            overflow: hidden;
        }

        .beach-image {
            width: 100%;
            height: 100%;
            background-size: cover;
            background-position: center;
            background-repeat: no-repeat;
            object-fit: cover;
            filter: brightness(1.05) contrast(1.1) saturate(1.1);
        }

        .beach-overlay {
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: linear-gradient(135deg, rgba(246, 239, 226, 0.15) 0%, rgba(246, 239, 226, 0.25) 100%);
            pointer-events: none;
        }

        /* Responsive */
        @media (max-width: 1024px) {
            .login-form-section {
                padding: 50px 40px;
            }

            .login-form-section h1 {
                font-size: 36px;
            }
        }

        @media (max-width: 768px) {
            .login-container {
                grid-template-columns: 1fr;
            }

            .login-image-section {
                display: none;
            }

            .login-form-section {
                padding: 40px 30px;
                justify-content: flex-start;
                padding-top: 100px;
            }

            .form-row {
                grid-template-columns: 1fr;
            }

            .logo {
                top: 20px;
                left: 30px;
                width: 40px;
                height: 40px;
            }

            .logo img {
                width: 40px;
                height: 40px;
            }

            .login-form-section h1 {
                font-size: 32px;
                margin-top: 20px;
            }

            .login-form-section p {
                font-size: 14px;
                margin-bottom: 30px;
            }
        }

        @media (max-width: 480px) {
            .login-form-section {
                padding: 30px 20px;
                padding-top: 80px;
            }

            .logo {
                left: 20px;
                width: 36px;
                height: 36px;
            }

            .logo img {
                width: 36px;
                height: 36px;
            }

            .login-form-section h1 {
                font-size: 28px;
            }

            .login-form-section p {
                font-size: 13px;
            }

            .form-group input[type="text"],
            .form-group input[type="email"],
            .form-group input[type="password"] {
                padding: 12px 14px;
                font-size: 14px;
            }

            .signup-button {
                padding: 14px;
                font-size: 15px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <!-- Left Side - Signup Form -->
            <div class="login-form-section">
                <div class="logo">
                    <img src="../Images/zinjago.png" alt="ZinJaGO Logo" />
                </div>

                <h1>Create Account</h1>
                <p>Join us today and start exploring amazing destinations</p>

                <div class="error-message" id="errorMessage" runat="server">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                </div>

                <div class="success-message" id="successMessage" runat="server">
                    <asp:Label ID="lblSuccess" runat="server"></asp:Label>
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label for="txtFirstName">First Name</label>
                        <asp:TextBox ID="txtFirstName" runat="server" placeholder="John"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" Display="Dynamic" ErrorMessage="First name is required" ForeColor="#CC0000"></asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group">
                        <label for="txtLastName">Last Name</label>
                        <asp:TextBox ID="txtLastName" runat="server" placeholder="Doe"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" Display="Dynamic" ErrorMessage="Last name is required" ForeColor="#CC0000"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <label for="txtEmail">Email address</label>
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" placeholder="your@email.com"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Email is required" ForeColor="#CC0000"></asp:RequiredFieldValidator>
                    <br />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Invalid email format" ForeColor="#CC0000" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                </div>

                <div class="form-group">
                    <label for="txtPassword">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Enter a strong password"></asp:TextBox>
                    <div class="password-strength">
                        <div class="password-strength-bar" id="passwordStrengthBar"></div>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" Display="Dynamic" ErrorMessage="Password is required" ForeColor="#CC0000"></asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label for="txtConfirmPassword">Confirm Password</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" placeholder="Confirm your password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" Display="Dynamic" ErrorMessage="Please confirm your password" ForeColor="#CC0000"></asp:RequiredFieldValidator>
                    <br />
                    <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" Display="Dynamic" ErrorMessage="Passwords do not match" ForeColor="#CC0000" Operator="Equal"></asp:CompareValidator>
                </div>

                <asp:Button ID="btnSignup" runat="server" Text="Create Account" CssClass="signup-button" OnClick="btnSignup_Click" />
                
                <div class="login-link">
                    Already have an account? <a href="../Login.aspx">Sign in here</a>
                </div>

                <br />
                <asp:ValidationSummary ID="vsSignup" runat="server" ForeColor="#CC0000" HeaderText="Please fix the following errors:" />
            </div>

            <!-- Right Side - Image -->
            <div class="login-image-section">
                <div class="beach-image" style="background-image: url('Images/login.png');"></div>
                <div class="beach-overlay"></div>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function hideErrorAfterDelay() {
            var errorDiv = document.getElementById('errorMessage');
            if (errorDiv && errorDiv.classList.contains('show')) {
                setTimeout(function () {
                    errorDiv.classList.remove('show');
                }, 5000);
            }
        }

        function checkPasswordStrength() {
            var password = document.getElementById('<%= txtPassword.ClientID %>').value;
            var strengthBar = document.getElementById('passwordStrengthBar');
            var strength = 0;

            if (password.length >= 8) strength += 25;
            if (password.length >= 12) strength += 25;
            if (/[a-z]/.test(password) && /[A-Z]/.test(password)) strength += 25;
            if (/[0-9]/.test(password) && /[!@#$%^&*]/.test(password)) strength += 25;

            strengthBar.style.width = strength + '%';
            
            if (strength <= 25) strengthBar.style.backgroundColor = '#cc3333';
            else if (strength <= 50) strengthBar.style.backgroundColor = '#ffcc00';
            else if (strength <= 75) strengthBar.style.backgroundColor = '#66cc33';
            else strengthBar.style.backgroundColor = '#1cb074';
        }

        document.getElementById('<%= txtPassword.ClientID %>').addEventListener('keyup', checkPasswordStrength);
        window.addEventListener('load', hideErrorAfterDelay);
    </script>
</body>
</html>
