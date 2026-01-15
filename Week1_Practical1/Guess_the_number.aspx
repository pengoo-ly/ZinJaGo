<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Guess_the_number.aspx.cs" Inherits="Week1_Practical1.Guess_the_number" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Guess The Number</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            background-color: #f5e6d3;
            font-family: Arial, sans-serif;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
        }

        .container {
            width: 90%;
            max-width: 500px;
        }

        .header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 40px;
            font-size: 24px;
            font-weight: bold;
        }

        .score {
            color: #d32f2f;
            font-size: 28px;
        }

        .game-box {
            background: linear-gradient(135deg, #5daba5 0%, #4a9490 100%);
            border-radius: 30px;
            padding: 60px 40px;
            text-align: center;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.2);
        }

        .game-box h1 {
            color: white;
            font-size: 28px;
            margin-bottom: 30px;
            font-weight: normal;
        }

        .input-group {
            margin-bottom: 30px;
        }

        #txtGuess {
            width: 100%;
            padding: 18px;
            border: none;
            border-radius: 20px;
            font-size: 18px;
            text-align: center;
            background-color: rgba(255, 255, 255, 0.3);
            color: #333;
            outline: none;
        }

        #txtGuess::placeholder {
            color: #333;
        }

        .button-group {
            display: flex;
            gap: 15px;
            margin-bottom: 20px;
        }

        .btn {
            flex: 1;
            padding: 15px;
            border: none;
            border-radius: 20px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            transition: all 0.3s ease;
        }

        #btnGuess {
            background-color: #2e7d78;
            color: white;
        }

        #btnGuess:hover {
            background-color: #1e5f5a;
        }

        #btnNewGame {
            background-color: #5daba5;
            color: white;
            border: 2px solid white;
        }

        #btnNewGame:hover {
            background-color: #4a9490;
        }

        .message {
            color: white;
            font-size: 18px;
            margin-top: 20px;
            min-height: 25px;
        }

        .stats {
            color: white;
            font-size: 14px;
            margin-top: 15px;
        }

        .message.success {
            color: #90ee90;
            font-weight: bold;
        }

        .message.hint {
            color: #ffeb3b;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header">
                <span>Guess The Number</span>
                <span class="score" id="score">● 0</span>
            </div>

            <div class="game-box">
                <h1>Guess a number</h1>
                
                <div class="input-group">
                    <asp:TextBox ID="txtGuess" runat="server" placeholder="Type Here" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="button-group">
                    <asp:Button ID="btnGuess" runat="server" Text="Guess" OnClick="btnGuess_Click" CssClass="btn" />
                    <asp:Button ID="btnNewGame" runat="server" Text="New Game" OnClick="btnNewGame_Click" CssClass="btn" />
                </div>

                <div class="message" id="messageBox"></div>
                <div class="stats" id="statsBox"></div>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function updateScore(score) {
            document.getElementById('score').textContent = '● ' + score;
        }

        function showMessage(text, className) {
            var messageBox = document.getElementById('messageBox');
            messageBox.textContent = text;
            messageBox.className = 'message ' + className;
        }

        function updateStats(guesses, totalGuesses) {
            var statsBox = document.getElementById('statsBox');
            statsBox.textContent = 'Attempt: ' + guesses + ' out of ' + totalGuesses;
        }
    </script>
</body>
</html>