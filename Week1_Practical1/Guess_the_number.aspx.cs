using System;
using System.Web.UI;

namespace Week1_Practical1
{
    public partial class Guess_the_number : System.Web.UI.Page
    {
        private const int MAX_GUESSES = 10;
        private const int MAX_NUMBER = 100;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializeGame();
            }
        }

        private void InitializeGame()
        {
            // Generate random number between 1 and 100
            Random rand = new Random();
            int randomNumber = rand.Next(1, MAX_NUMBER + 1);

            // Store in session
            Session["RandomNumber"] = randomNumber;
            Session["GuessCount"] = 0;
            Session["GameOver"] = false;

            txtGuess.Text = "";
            
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "clearMessage",
                "document.getElementById('messageBox').textContent = ''; document.getElementById('statsBox').textContent = 'Attempt: 0 out of 10';", true);
        }

        protected void btnGuess_Click(object sender, EventArgs e)
        {
            if (Session["GameOver"] != null && (bool)Session["GameOver"])
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "gameOverMessage",
                    "showMessage('Game Over! Click \"New Game\" to play again.', 'hint');", true);
                return;
            }

            string input = txtGuess.Text.Trim();
            int guessCount = (int)Session["GuessCount"] + 1;
            int randomNumber = (int)Session["RandomNumber"];

            // Validate input
            if (!int.TryParse(input, out int guess) || guess < 1 || guess > MAX_NUMBER)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "invalidInput",
                    "showMessage('Please enter a number between 1 and 100.', 'error');", true);
                return;
            }

            Session["GuessCount"] = guessCount;
            txtGuess.Text = "";

            if (guess == randomNumber)
            {
                // Correct guess - calculate points
                int points = CalculatePoints(guessCount);
                int currentScore = (Session["Score"] != null) ? (int)Session["Score"] : 0;
                int newScore = currentScore + points;
                Session["Score"] = newScore;
                Session["GameOver"] = true;

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "correctGuess",
                    string.Format("showMessage('🎉 Correct! You earned {0} points!', 'success'); updateScore({1}); updateStats({2}, {3});",
                    points, newScore, guessCount, MAX_GUESSES), true);
            }
            else if (guessCount >= MAX_GUESSES)
            {
                // Out of guesses
                Session["GameOver"] = true;
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "noGuesses",
                    string.Format("showMessage('Game Over! The number was {0}.', 'error'); updateStats({1}, {2});",
                    randomNumber, guessCount, MAX_GUESSES), true);
            }
            else
            {
                // Give hint
                string hint = guess < randomNumber ? "higher" : "lower";
                int remainingGuesses = MAX_GUESSES - guessCount;
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "hint",
                    string.Format("showMessage('The number is {0}. You have {1} guesses left.', 'hint'); updateStats({2}, {3});",
                    hint, remainingGuesses, guessCount, MAX_GUESSES), true);
            }
        }

        protected void btnNewGame_Click(object sender, EventArgs e)
        {
            InitializeGame();
        }

        private int CalculatePoints(int guessCount)
        {
            // More points for fewer guesses
            // Max 100 points for 1 guess, decreasing as guesses increase
            return Math.Max(100 - (guessCount - 1) * 10, 10);
        }
    }
}