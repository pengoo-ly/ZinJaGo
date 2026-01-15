<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Snake_Game.aspx.cs" Inherits="Week1_Practical1.WebForm1" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Snake Game</title>

    <style>
        body {
            font-family: Arial;
            background-color: #faf3e5;
            text-align: center;
        }
        #gameContainer {
            margin: 20px auto;
        }
        canvas {
            background-color: #eef0d7;
            border-radius: 8px;
            border: 2px solid #b5b8a4;
        }
        #scoreBoard {
            font-size: 24px;
            color: #d33;
            font-weight: bold;
        }
        #btnPanel {
            margin-top: 15px;
            display: none;
        }
        button {
            padding: 10px 18px;
            font-size: 16px;
            border-radius: 8px;
            margin: 5px;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">

        <h2 style="color:#4a9a8a; margin-bottom:5px;">Snake</h2>
        <div id="scoreBoard">
        Score: <span id="score">0</span> &nbsp;&nbsp;&nbsp;
        High Score: <span id="highScore">0</span>
</div>


        <div id="gameContainer">
            <canvas id="gameCanvas" width="800" height="450"></canvas>

        </div>

        <div id="btnPanel">
            <button onclick="restartGame()">Restart</button>
            <button onclick="exitGame()">Exit</button>
        </div>

<script>
    const canvas = document.getElementById("gameCanvas");
    const ctx = canvas.getContext("2d");

    const box = 20;
    let snake, direction, nextDirection, score, food;
    let game = null;
    let gameStarted = false;

    // Load high score from browser
    let highScore = localStorage.getItem("snakeHighScore");
    if (!highScore) highScore = 0;
    document.getElementById("highScore").innerText = highScore;

    function initGame() {
        snake = [{ x: 10 * box, y: 10 * box }];
        direction = null;
        nextDirection = null;
        score = 0;
        gameStarted = false;

        document.getElementById("score").innerText = score;

        food = {
            x: Math.floor(Math.random() * (canvas.width / box)) * box,
            y: Math.floor(Math.random() * (canvas.height / box)) * box
        };

        drawInitialScreen();
    }

    // Detect arrow-key input to START the game
    document.addEventListener("keydown", event => {
        if (event.key === "ArrowUp" && direction !== "DOWN") nextDirection = "UP";
        else if (event.key === "ArrowDown" && direction !== "UP") nextDirection = "DOWN";
        else if (event.key === "ArrowLeft" && direction !== "RIGHT") nextDirection = "LEFT";
        else if (event.key === "ArrowRight" && direction !== "LEFT") nextDirection = "RIGHT";
        else return;

        if (!gameStarted) {
            gameStarted = true;
            direction = nextDirection;
            game = setInterval(draw, 120);
        }
    });

    function drawInitialScreen() {
        ctx.clearRect(0, 0, canvas.width, canvas.height);

        // Draw food
        ctx.fillStyle = "#e64537";
        ctx.beginPath();
        ctx.arc(food.x + box / 2, food.y + box / 2, box / 2, 0, Math.PI * 2);
        ctx.fill();

        // Draw snake head
        ctx.fillStyle = "#2e8c82";
        ctx.fillRect(snake[0].x, snake[0].y, box - 2, box - 2);

        // Draw instructions
        ctx.fillStyle = "#444";
        ctx.font = "26px Arial";
        ctx.textAlign = "center";
        ctx.fillText("Use arrow keys to start", canvas.width / 2, canvas.height / 2);
    }

    function draw() {
        if (!gameStarted) return;

        ctx.clearRect(0, 0, canvas.width, canvas.height);

        // Food
        ctx.fillStyle = "#e64537";
        ctx.beginPath();
        ctx.arc(food.x + box / 2, food.y + box / 2, box / 2, 0, Math.PI * 2);
        ctx.fill();

        // Draw snake
        ctx.fillStyle = "#2e8c82";
        snake.forEach(s => {
            ctx.fillRect(s.x, s.y, box - 2, box - 2);
        });

        // Movement
        if (nextDirection) direction = nextDirection;

        let head = { x: snake[0].x, y: snake[0].y };

        if (direction === "UP") head.y -= box;
        if (direction === "DOWN") head.y += box;
        if (direction === "LEFT") head.x -= box;
        if (direction === "RIGHT") head.x += box;

        // Wall collision
        if (
            head.x < 0 || head.x >= canvas.width ||
            head.y < 0 || head.y >= canvas.height
        ) {
            gameOver();
            return;
        }

        // Self collision
        for (let i = 0; i < snake.length; i++) {
            if (head.x === snake[i].x && head.y === snake[i].y) {
                gameOver();
                return;
            }
        }

        // Food eaten
        if (head.x === food.x && head.y === food.y) {
            score++;
            document.getElementById("score").innerText = score;

            // Update high score
            if (score > highScore) {
                highScore = score;
                localStorage.setItem("snakeHighScore", highScore);
                document.getElementById("highScore").innerText = highScore;
            }

            food = {
                x: Math.floor(Math.random() * (canvas.width / box)) * box,
                y: Math.floor(Math.random() * (canvas.height / box)) * box
            };

        } else {
            snake.pop();
        }

        snake.unshift(head);
    }

    function gameOver() {
        clearInterval(game);
        alert("Game Over! Your score: " + score);
        document.getElementById("btnPanel").style.display = "block";
    }

    function restartGame() {
        clearInterval(game);
        document.getElementById("btnPanel").style.display = "none";
        initGame();
    }

    function exitGame() {
        window.location.href = "Default.aspx";
    }

    initGame();
</script>



    </form>
</body>
</html>