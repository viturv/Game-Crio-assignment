using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AIModeGameManager : MonoBehaviour
{
    public Button[] buttons; // No of box for the grid
    private string currentPlayer = "X"; // Track the current player (X for Player, O for AI)
    private bool gameOver = false; // Game state
    public TextMeshProUGUI gameOverText; // Reference to the UI text for displaying game result
    public Button resetButton; // Restrat Button

    void Start()
    {
        // Size of button array(Number of Box) should be 9 
        if (buttons.Length != 9)
        {
            Debug.LogError("Buttons array must have 9 buttons!");
        }

        // Intializing Button Value to empty
        foreach (Button button in buttons)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = ""; // Clear the button text
            button.onClick.AddListener(() => OnButtonClick(System.Array.IndexOf(buttons, button))); // Add click listener
        }
    }

    public void OnButtonClick(int index)
    {
        // Check that is game over or not, button is pressed before or not, and if AI has taken turn then it's players turn
        if (gameOver || buttons[index].GetComponentInChildren<TextMeshProUGUI>().text != "" || currentPlayer == "O")
            return; // Disable player input during AI's turn

        // It will Update text value on the basis of the turn X for player and O for AI
        buttons[index].GetComponentInChildren<TextMeshProUGUI>().text = currentPlayer;

        // Check for a winner
        CheckForWinner();

        // Switch to AI's turn
        if (!gameOver)
        {
            currentPlayer = "O"; // Switch to AI
            AI_Move(); // Make AI move
        }
    }

    void AI_Move()
    {
        // Button are disable during the AI's turn to avaoid player taking turn
        SetButtonsInteractable(false);

        // Wait for few time before making the move
        Invoke("MakeAIMove", 0.5f); // MakeAIMove method will be called after Invoke (here after 0.5 second)
    }

    void MakeAIMove()
    {
        int bestMove = GetBestMove();

        // AI marks O
        if (bestMove != -1)
        {
            buttons[bestMove].GetComponentInChildren<TextMeshProUGUI>().text = "O";
        }

        // Check for a winner
        CheckForWinner();

        // Switch back to player's turn after AI's move
        if (!gameOver)
        {
            currentPlayer = "X"; // Switch to Player
            SetButtonsInteractable(true); // Enables Buttons after AI move
        }
    }

    int GetBestMove()
    {
        // Winning combinations
        int[,] winPatterns = new int[,]
        {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Horizontal
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Vertical
            {0, 4, 8}, {2, 4, 6}             // Diagonals
        };

        // Step 1: Check if AI can win
        for (int i = 0; i < winPatterns.GetLength(0); i++)
        {
            int a = winPatterns[i, 0];
            int b = winPatterns[i, 1];
            int c = winPatterns[i, 2];

            if (buttons[a].GetComponentInChildren<TextMeshProUGUI>().text == "O" &&
                buttons[b].GetComponentInChildren<TextMeshProUGUI>().text == "O" &&
                buttons[c].GetComponentInChildren<TextMeshProUGUI>().text == "")
                return c;

            if (buttons[a].GetComponentInChildren<TextMeshProUGUI>().text == "O" &&
                buttons[c].GetComponentInChildren<TextMeshProUGUI>().text == "O" &&
                buttons[b].GetComponentInChildren<TextMeshProUGUI>().text == "")
                return b;

            if (buttons[b].GetComponentInChildren<TextMeshProUGUI>().text == "O" &&
                buttons[c].GetComponentInChildren<TextMeshProUGUI>().text == "O" &&
                buttons[a].GetComponentInChildren<TextMeshProUGUI>().text == "")
                return a;
        }

        // Step 2: Block the player if they are about to win
        for (int i = 0; i < winPatterns.GetLength(0); i++)
        {
            int a = winPatterns[i, 0];
            int b = winPatterns[i, 1];
            int c = winPatterns[i, 2];

            if (buttons[a].GetComponentInChildren<TextMeshProUGUI>().text == "X" &&
                buttons[b].GetComponentInChildren<TextMeshProUGUI>().text == "X" &&
                buttons[c].GetComponentInChildren<TextMeshProUGUI>().text == "")
                return c;

            if (buttons[a].GetComponentInChildren<TextMeshProUGUI>().text == "X" &&
                buttons[c].GetComponentInChildren<TextMeshProUGUI>().text == "X" &&
                buttons[b].GetComponentInChildren<TextMeshProUGUI>().text == "")
                return b;

            if (buttons[b].GetComponentInChildren<TextMeshProUGUI>().text == "X" &&
                buttons[c].GetComponentInChildren<TextMeshProUGUI>().text == "X" &&
                buttons[a].GetComponentInChildren<TextMeshProUGUI>().text == "")
                return a;
        }

        // Step 3: Take the center if available
        if (buttons[4].GetComponentInChildren<TextMeshProUGUI>().text == "")
            return 4;

        // Step 4: Take a corner if available
        int[] corners = { 0, 2, 6, 8 };
        foreach (int corner in corners)
        {
            if (buttons[corner].GetComponentInChildren<TextMeshProUGUI>().text == "")
                return corner;
        }

        // Step 5: Take a random available space
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetComponentInChildren<TextMeshProUGUI>().text == "")
                return i;
        }

        return -1; // No moves available
    }

    // Check for a winner after every move
    void CheckForWinner()
    {
        // Define winning combinations (3 rows, 3 columns, 2 diagonals)
        int[,] winPatterns = new int[,]
        {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Horizontal
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Vertical
            {0, 4, 8}, {2, 4, 6}             // Diagonals
        };

        // Check each winning pattern
        for (int i = 0; i < winPatterns.GetLength(0); i++)
        {
            int a = winPatterns[i, 0];
            int b = winPatterns[i, 1];
            int c = winPatterns[i, 2];

            if (buttons[a].GetComponentInChildren<TextMeshProUGUI>().text == currentPlayer &&
                buttons[b].GetComponentInChildren<TextMeshProUGUI>().text == currentPlayer &&
                buttons[c].GetComponentInChildren<TextMeshProUGUI>().text == currentPlayer)
            {
                gameOver = true;
                ShowGameOverMessage(currentPlayer + " Wins!");
                return;
            }
        }

        // Check for a tie
        bool tie = true;
        foreach (Button button in buttons)
        {
            if (button.GetComponentInChildren<TextMeshProUGUI>().text == "")
            {
                tie = false;
                break;
            }
        }

        if (tie)
        {
            gameOver = true;
            ShowGameOverMessage("It's a tie!");
        }
    }

    // Method to enable or disable buttons
    void SetButtonsInteractable(bool interactable)
    {
        foreach (Button button in buttons)
        {
            button.interactable = interactable;
        }
    }

    void ShowGameOverMessage(string message)
        {
            gameOverText.text = message; // Update the text on screen
        }
    public void ResetGame()
    {
        // Reset all button texts
        foreach (Button button in buttons)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = "";
            button.interactable = true; // Re-enable all buttons
        }

        // Reset the game state
        currentPlayer = "X";
        gameOver = false;
        gameOverText.text = ""; // Clear the game over message
        resetButton.gameObject.SetActive(false); // Hide reset button
    }
}

