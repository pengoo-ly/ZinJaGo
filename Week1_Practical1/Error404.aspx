<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error404.aspx.cs" Inherits="YourProjectName.Error404" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Page Not Found - 404</title>
    <style>
        body { 
            background-color: #f4f7f6; 
            font-family: 'Segoe UI', Arial, sans-serif; 
            margin: 0; 
            display: flex; 
            justify-content: center; 
            align-items: center; 
            height: 100vh; 
            color: #333;
        }
        .error-container { 
            text-align: center; 
            max-width: 500px; 
            padding: 40px;
            background: white;
            border-radius: 12px;
            box-shadow: 0 10px 25px rgba(0,0,0,0.1);
        }
        h1 { 
            font-size: 100px; 
            margin: 0; 
            color: #e74c3c; 
            line-height: 1;
        }
        h2 { 
            font-size: 24px; 
            margin-bottom: 20px; 
        }
        p { 
            font-size: 16px; 
            color: #7f8c8d; 
            margin-bottom: 30px; 
        }
        .btn-home { 
            display: inline-block; 
            padding: 12px 30px; 
            background-color: #3498db; 
            color: white; 
            text-decoration: none; 
            border-radius: 25px; 
            font-weight: bold;
            transition: background 0.3s ease;
        }
        .btn-home:hover { 
            background-color: #2980b9; 
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="error-container">
            <h1>404</h1>
            <h2>Oops! Page Not Found</h2>
            <p>The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.</p>
            <a href="home.aspx" class="btn-home">Go Back to Homepage</a>
        </div>
    </form>
</body>
</html>