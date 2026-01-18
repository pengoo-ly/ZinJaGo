<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error500.aspx.cs" Inherits="YourProjectName.Error500" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Server Error - 500</title>
    <style>
        body { background-color: #f8f9fa; font-family: 'Segoe UI', sans-serif; display: flex; justify-content: center; align-items: center; height: 100vh; margin: 0; }
        .container { text-align: center; padding: 20px; }
        h1 { font-size: 100px; margin: 0; color: #ffc107; } /* Warning Yellow */
        p { font-size: 20px; color: #6c757d; }
        .btn-home { margin-top: 20px; display: inline-block; padding: 12px 24px; background-color: #212529; color: white; text-decoration: none; border-radius: 5px; }
        .btn-home:hover { background-color: #000; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>500</h1>
            <h2>Something went wrong on our end.</h2>
            <p>We are experiencing a technical issue. Please try again later.</p>
            <a href="home.aspx" class="btn-home">Return to Home</a>
        </div>
    </form>
</body>
</html>