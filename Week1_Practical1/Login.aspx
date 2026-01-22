<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Week1_Practical1.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Login - ZinJaGO</title>
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

        /* Left Side - Form */
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

        /* Tab Navigation */
        .tab-navigation {
            display: flex;
            gap: 10px;
            margin-bottom: 40px;
            border-bottom: 2px solid #eee;
            padding-bottom: 0;
        }

        .tab-btn {
            padding: 12px 24px;
            background: none;
            border: none;
            border-bottom: 3px solid transparent;
            font-size: 16px;
            font-weight: 600;
            color: #999;
            cursor: pointer;
            transition: all 0.3s ease;
            position: relative;
            bottom: -2px;
        }

        .tab-btn:hover {
            color: #1cb074;
        }

        .tab-btn.active {
            color: #1cb074;
            border-bottom-color: #1cb074;
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
            margin-bottom: 35px;
            line-height: 1.5;
        }

        .form-group {
            margin-bottom: 25px;
        }

        .form-group label {
            display: block;
            font-size: 14px;
            font-weight: 600;
            color: #222;
            margin-bottom: 10px;
        }

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

        .form-group input[type="email"]:focus,
        .form-group input[type="password"]:focus {
            outline: none;
            border-color: #1cb074;
            box-shadow: 0 0 0 3px rgba(28, 176, 116, 0.1);
        }

        .form-options {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-bottom: 35px;
            font-size: 14px;
            flex-wrap: wrap;
            gap: 12px;
        }

        .checkbox-group {
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .checkbox-group input[type="checkbox"] {
            width: 18px;
            height: 18px;
            cursor: pointer;
            accent-color: #1cb074;
        }

        .checkbox-group label {
            cursor: pointer;
            color: #666;
            margin: 0;
        }

        .auth-links {
            display: flex;
            align-items: center;
            gap: 16px;
        }

        .forgot-password,
        .sign-up-link {
            text-decoration: none;
            color: #1cb074;
            font-weight: 600;
            transition: color 0.3s ease;
        }

        .forgot-password:hover,
        .sign-up-link:hover {
            color: #0f8452;
        }

        .auth-links .divider {
            width: 1px;
            height: 20px;
            background: #ddd;
        }

        .login-button {
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

        .login-button:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(28, 176, 116, 0.3);
        }

        .login-button:active {
            transform: translateY(0);
        }

        .login-button:disabled {
            opacity: 0.6;
            cursor: not-allowed;
        }

        .signup-link-text {
            text-align: center;
            margin-top: 25px;
            color: #666;
            font-size: 14px;
        }

        .signup-link-text a {
            color: #1cb074;
            font-weight: 600;
            text-decoration: none;
            transition: color 0.3s ease;
        }

        .signup-link-text a:hover {
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

        .tab-content {
            display: none;
        }

        .tab-content.active {
            display: block;
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

            .tab-navigation {
                margin-bottom: 30px;
            }
        }

        @media (max-width: 768px) {
            .login-container {
                grid-template-columns: 1fr;
                gap: 0;
            }

            .login-image-section {
                display: none;
            }

            .login-form-section {
                padding: 40px 30px;
                justify-content: flex-start;
                padding-top: 80px;
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

            .form-options {
                flex-direction: column;
                align-items: flex-start;
                gap: 15px;
            }

            .auth-links {
                width: 100%;
                flex-direction: column;
                align-items: flex-start;
                gap: 12px;
            }

            .auth-links .divider {
                display: none;
            }

            .tab-navigation {
                gap: 5px;
            }

            .tab-btn {
                padding: 10px 16px;
                font-size: 14px;
            }
        }

        @media (max-width: 480px) {
            .login-form-section {
                padding: 30px 20px;
                padding-top: 70px;
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
                margin-bottom: 25px;
            }

            .form-group input[type="email"],
            .form-group input[type="password"] {
                padding: 12px 14px;
                font-size: 14px;
            }

            .login-button {
                padding: 14px;
                font-size: 15px;
            }

            .tab-btn {
                padding: 8px 12px;
                font-size: 12px;
            }

            .tab-navigation {
                gap: 0;
                margin-bottom: 25px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <!-- Left Side - Login Forms -->
            <div class="login-form-section">
                <div class="logo">
                    <img src="Images/zinjago.png" alt="ZinJaGO Logo" />
                </div>

                <!-- Tab Navigation -->
                <div class="tab-navigation">
                    <button type="button" class="tab-btn active" data-tab="customer-tab" onclick="switchTab(event, 'customer-tab')">
                        Customer Login
                    </button>
                    <button type="button" class="tab-btn" data-tab="seller-tab" onclick="switchTab(event, 'seller-tab')">
                        Seller Login
                    </button>
                </div>

                <!-- Customer Login Tab -->
                <div id="customer-tab" class="tab-content active">
                    <h1>Welcome Back</h1>
                    <p>Enter your credentials to access your account</p>

                    <div class="error-message" id="customerErrorMessage" runat="server">
                        <asp:Label ID="lblCustomerError" runat="server"></asp:Label>
                    </div>

                    <div class="form-group">
                        <label for="txtCustomerEmail">Email address</label>
                        <asp:TextBox ID="txtCustomerEmail" runat="server" TextMode="Email" placeholder="your@email.com" ValidationGroup="Customer"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCustomerEmail" runat="server" ControlToValidate="txtCustomerEmail" Display="Dynamic" ErrorMessage="Email is required" ForeColor="#CC0000" ValidationGroup="Customer"></asp:RequiredFieldValidator>
                        <br />
                        <asp:RegularExpressionValidator ID="revCustomerEmail" runat="server" ControlToValidate="txtCustomerEmail" Display="Dynamic" ErrorMessage="Invalid email format" ForeColor="#CC0000" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Customer"></asp:RegularExpressionValidator>
                    </div>

                    <div class="form-group">
                        <label for="txtCustomerPassword">Password</label>
                        <asp:TextBox ID="txtCustomerPassword" runat="server" TextMode="Password" placeholder="Enter your password" ValidationGroup="Customer"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCustomerPassword" runat="server" ControlToValidate="txtCustomerPassword" Display="Dynamic" ErrorMessage="Password is required" ForeColor="#CC0000" ValidationGroup="Customer"></asp:RequiredFieldValidator>
                    </div>

                    <div class="form-options">
                        <div class="checkbox-group">
                            <asp:CheckBox ID="chkCustomerRemember" runat="server" />
                            <label for="chkCustomerRemember">Remember me for 30 days</label>
                        </div>
                        <div class="auth-links">
                            <asp:LinkButton ID="lnkCustomerForgotPassword" runat="server" CssClass="forgot-password" OnClick="lnkCustomerForgotPassword_Click" CausesValidation="false">Forgot password?</asp:LinkButton>
                        </div>
                    </div>

                    <asp:Button ID="btnCustomerLogin" runat="server" Text="Login" CssClass="login-button" OnClick="btnCustomerLogin_Click" ValidationGroup="Customer" />
                    
                    <div class="signup-link-text">
                        Don't have an account? <a href="CustomerSignup.aspx">Sign up here</a>
                    </div>

                    <asp:ValidationSummary ID="vsCustomerLogin" runat="server" ForeColor="#CC0000" HeaderText="Please fix the following errors:" ValidationGroup="Customer" />
                </div>

                <!-- Seller Login Tab -->
                <div id="seller-tab" class="tab-content">
                    <h1>Seller Login</h1>
                    <p>Access your seller dashboard</p>

                    <div class="error-message" id="sellerErrorMessage" runat="server">
                        <asp:Label ID="lblSellerError" runat="server"></asp:Label>
                    </div>

                    <div class="form-group">
                        <label for="txtSellerEmail">Email address</label>
                        <asp:TextBox ID="txtSellerEmail" runat="server" TextMode="Email" placeholder="admin@example.com" ValidationGroup="Seller"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSellerEmail" runat="server" ControlToValidate="txtSellerEmail" Display="Dynamic" ErrorMessage="Email is required" ForeColor="#CC0000" ValidationGroup="Seller"></asp:RequiredFieldValidator>
                        <br />
                        <asp:RegularExpressionValidator ID="revSellerEmail" runat="server" ControlToValidate="txtSellerEmail" Display="Dynamic" ErrorMessage="Invalid email format" ForeColor="#CC0000" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Seller"></asp:RegularExpressionValidator>
                    </div>

                    <div class="form-group">
                        <label for="txtSellerPassword">Password</label>
                        <asp:TextBox ID="txtSellerPassword" runat="server" TextMode="Password" placeholder="Enter your password" ValidationGroup="Seller"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSellerPassword" runat="server" ControlToValidate="txtSellerPassword" Display="Dynamic" ErrorMessage="Password is required" ForeColor="#CC0000" ValidationGroup="Seller"></asp:RequiredFieldValidator>
                    </div>

                    <div class="form-options">
                        <div class="checkbox-group">
                            <asp:CheckBox ID="chkSellerRemember" runat="server" />
                            <label for="chkSellerRemember">Remember me for 30 days</label>
                        </div>
                        <div class="auth-links">
                            <asp:LinkButton ID="lnkSellerForgotPassword" runat="server" CssClass="forgot-password" OnClick="lnkSellerForgotPassword_Click" CausesValidation="false">Forgot password?</asp:LinkButton>
                        </div>
                    </div>

                    <asp:Button ID="btnSellerLogin" runat="server" Text="Login" CssClass="login-button" OnClick="btnSellerLogin_Click" ValidationGroup="Seller" />
                    
                    <div class="signup-link-text">
                        Don't have an account? <a href="AdminSignUp.aspx">Sign up here</a>
                    </div>

                    <asp:ValidationSummary ID="vsSellerLogin" runat="server" ForeColor="#CC0000" HeaderText="Please fix the following errors:" />
                </div>
            </div>

            <!-- Right Side - Image -->
            <div class="login-image-section">
                <div class="beach-image" style="background-image: url('Images/login.png');"></div>
                <div class="beach-overlay"></div>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function switchTab(e, tabName) {
            e.preventDefault();

            // Hide all tab contents
            var tabs = document.getElementsByClassName('tab-content');
            for (var i = 0; i < tabs.length; i++) {
                tabs[i].classList.remove('active');
            }

            // Remove active class from all buttons
            var buttons = document.getElementsByClassName('tab-btn');
            for (var i = 0; i < buttons.length; i++) {
                buttons[i].classList.remove('active');
            }

            // Show selected tab
            document.getElementById(tabName).classList.add('active');

            // Add active class to clicked button
            event.target.classList.add('active');

            // Hide error messages
            var customerError = document.getElementById('customerErrorMessage');
            var sellerError = document.getElementById('sellerErrorMessage');

            if (customerError) customerError.classList.remove('show');
            if (sellerError) sellerError.classList.remove('show');
        }

        function hideErrorAfterDelay() {
            var customerError = document.getElementById('customerErrorMessage');
            var sellerError = document.getElementById('sellerErrorMessage');

            if (customerError && customerError.classList.contains('show')) {
                setTimeout(function () {
                    customerError.classList.remove('show');
                }, 5000);
            }

            if (sellerError && sellerError.classList.contains('show')) {
                setTimeout(function () {
                    sellerError.classList.remove('show');
                }, 5000);
            }
        }

        window.addEventListener('load', hideErrorAfterDelay);
    </script>
</body>
</html>