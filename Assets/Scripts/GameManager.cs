using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Button[] buttons; // Array of buttons in the grid
    private string currentPlayer = "X"; // Track the current player
    private bool gameOver = false; // Game state
    public TextMeshProUGUI gameOverText; // Reference to the UI text for displaying game result
    public Button resetButton; // Reference to the Reset Button

    void Start()
    {
        // Ensure that your buttons array has the correct number of elements (9)
        if (buttons.Length != 9)
        {
            Debug.LogError("Buttons array must have 9 buttons!");
        }

        // Initialize button texts
        foreach (Button button in buttons)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = ""; // Clear the button text
            button.onClick.AddListener(() => OnButtonClick(System.Array.IndexOf(buttons, button))); // Add click listener
        }

        // Set up the reset button
        resetButton.onClick.AddListener(ResetGame); // Reset game when the reset button is clicked
        resetButton.gameObject.SetActive(false); // Hide the reset button at the start
    }

    public void OnButtonClick(int index)
    {
        // Ensure the game isn't over and the button is not already clicked
        if (gameOver || buttons[index].GetComponentInChildren<TextMeshProUGUI>().text != "")
            return;

        // Set the current player's mark (X or O)
        buttons[index].GetComponentInChildren<TextMeshProUGUI>().text = currentPlayer;

        // Check for a winner
        CheckForWinner();

        // Switch the player after every move
        if (!gameOver)
        {
            currentPlayer = (currentPlayer == "X") ? "O" : "X";
        }
    }

    // Check for a winner after every move
    void CheckForWinner()
    {
        // Define winning combinations (3 rows, 3 columns, 2 diagonals)
        int[,] winPatterns = new int[,]
        {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Horizontal
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Vertical
            {0, 4, 8}, {2, 4, 6} // Diagonals
        };

        // Check each winning pattern
        for (int i = 0; i < winPatterns.GetLength(0); i++)
        {
            int a = winPatterns[i, 0];
            int b = winPatterns[i, 1];
            int c = winPatterns[i, 2];

            // If all three positions have the same text (X or O), declare a winner
            if (buttons[a].GetComponentInChildren<TextMeshProUGUI>().text == currentPlayer &&
                buttons[b].GetComponentInChildren<TextMeshProUGUI>().text == currentPlayer &&
                buttons[c].GetComponentInChildren<TextMeshProUGUI>().text == currentPlayer)
            {
                gameOver = true;
                ShowGameOverMessage(currentPlayer + " wins!"); // Show winner message
                resetButton.gameObject.SetActive(true); // Show reset button
                return;
            }
        }

        // Check for a tie (if all buttons are filled and no winner)
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
            ShowGameOverMessage("It's a tie!"); // Show tie message
            resetButton.gameObject.SetActive(true); // Show reset button
        }
    }

    // Show the game over message
    void ShowGameOverMessage(string message)
    {
        gameOverText.text = message; // Update the text on screen
    }

    // Reset the game
    public void ResetGame()
    {
        // Reset all button texts
        foreach (Button button in buttons)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }

        // Reset the game state
        currentPlayer = "X";
        gameOver = false;
        gameOverText.text = ""; // Clear the game over message
        resetButton.gameObject.SetActive(false); // Hide reset button
    }
}
