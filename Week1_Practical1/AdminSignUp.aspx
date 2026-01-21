<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminSignUp.aspx.cs" Inherits="Week1_Practical1.AdminSignUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Admin Sign Up - ZinJaGO</title>
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

        .signup-container {
            display: grid;
            grid-template-columns: 1fr 1fr;
            height: 100vh;
            gap: 0;
        }

        /* Left Side - Form */
        .signup-form-section {
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

        .signup-form-section h1 {
            font-size: 42px;
            color: #222;
            margin-bottom: 15px;
            font-weight: 600;
        }

        .signup-form-section p {
            color: #666;
            font-size: 16px;
            margin-bottom: 40px;
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
            margin-bottom: 8px;
        }

        .form-group input[type="text"],
        .form-group input[type="email"],
        .form-group input[type="password"] {
            width: 100%;
            padding: 12px 14px;
            border: 1px solid #ddd;
            border-radius: 8px;
            font-size: 14px;
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
            gap: 16px;
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

        .signup-button {
            width: 100%;
            padding: 14px;
            background: linear-gradient(135deg, #1cb074 0%, #0f8452 100%);
            color: white;
            border: none;
            border-radius: 8px;
            font-size: 16px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
            box-shadow: 0 4px 15px rgba(28, 176, 116, 0.2);
            margin-bottom: 16px;
        }

        .signup-button:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(28, 176, 116, 0.3);
        }

        .signup-button:active {
            transform: translateY(0);
        }

        .signup-button:disabled {
            opacity: 0.6;
            cursor: not-allowed;
        }

        .login-link {
            text-align: center;
            margin-top: 20px;
            font-size: 14px;
            color: #666;
        }

        .login-link a {
            color: #1cb074;
            text-decoration: none;
            font-weight: 600;
            transition: color 0.3s ease;
        }

        .login-link a:hover {
            color: #0f8452;
        }

        /* Right Side - Image */
        .signup-image-section {
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
            .signup-form-section {
                padding: 50px 40px;
            }

            .signup-form-section h1 {
                font-size: 36px;
            }

            .form-row {
                grid-template-columns: 1fr;
            }
        }

        @media (max-width: 768px) {
            .signup-container {
                grid-template-columns: 1fr;
                gap: 0;
            }

            .signup-image-section {
                display: none;
            }

            .signup-form-section {
                padding: 40px 30px;
                justify-content: flex-start;
                padding-top: 100px;
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

            .signup-form-section h1 {
                font-size: 32px;
                margin-top: 20px;
            }

            .signup-form-section p {
                font-size: 14px;
                margin-bottom: 30px;
            }

            .form-row {
                grid-template-columns: 1fr;
            }
        }

        @media (max-width: 480px) {
            .signup-form-section {
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

            .signup-form-section h1 {
                font-size: 28px;
            }

            .signup-form-section p {
                font-size: 13px;
            }

            .form-group input[type="text"],
            .form-group input[type="email"],
            .form-group input[type="password"] {
                padding: 12px 14px;
                font-size: 14px;
            }

            .signup-button {
                padding: 12px;
                font-size: 15px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="signup-container">
            <!-- Left Side - Signup Form -->
            <div class="signup-form-section">
                <div class="logo">
                    <img src="Images/zinjago.png" alt="ZinJaGO Logo" />
                </div>

                <h1>Create Account</h1>
                <p>Join as an admin and manage your business</p>

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
                    </div>
                    <div class="form-group">
                        <label for="txtLastName">Last Name</label>
                        <asp:TextBox ID="txtLastName" runat="server" placeholder="Doe"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <label for="txtEmail">Email address</label>
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" placeholder="admin@example.com"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtPassword">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="At least 8 characters"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtConfirmPassword">Confirm Password</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" placeholder="Confirm your password"></asp:TextBox>
                </div>

                <asp:Button ID="btnSignUp" runat="server" Text="Create Account" CssClass="signup-button" OnClick="btnSignUp_Click" />

                <div class="login-link">
                    Already have an account? <a href="Login.aspx">Login here</a>
                </div>
            </div>

            <!-- Right Side - Image -->
            <div class="signup-image-section">
                <div class="beach-image" style="background-image: url('Images/login.png');"></div>
                <div class="beach-overlay"></div>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function hideMessageAfterDelay() {
            var errorDiv = document.getElementById('errorMessage');
            var successDiv = document.getElementById('successMessage');
            
            if (errorDiv && errorDiv.classList.contains('show')) {
                setTimeout(function () {
                    errorDiv.classList.remove('show');
                }, 5000);
            }

            if (successDiv && successDiv.classList.contains('show')) {
                setTimeout(function () {
                    successDiv.classList.remove('show');
                }, 3000);
                setTimeout(function () {
                    window.location.href = 'Login.aspx';
                }, 3500);
            }
        }

        window.addEventListener('load', hideMessageAfterDelay);
    </script>
</body>
</html>