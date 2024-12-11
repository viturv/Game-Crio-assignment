using UnityEngine;
using UnityEngine.SceneManagement; // To load scenes

public class MainMenuManager : MonoBehaviour
{
    public void OnPlayerVsPlayerClick()
    {
        // Load the TicTacToe scene for Player vs Player
        Debug.Log("Player Vs Player mode selected");
        SceneManager.LoadScene("Tic-Tac-Toe_Player");
        

    }

    public void OnPlayerVsAIClick()
    {
        // Load the TicTacToe scene for Player vs AI (If it's a different scene)
        Debug.Log("Player Vs AI mode selected");

        SceneManager.LoadScene("Tic-Tac-Toe_AI"); // Replace with your scene name
    }
}

