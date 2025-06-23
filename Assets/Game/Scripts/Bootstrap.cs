using Game.Scripts.Features;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    // Reference to the GameManager, assign via Inspector
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        // Initialize the game manager (e.g., set up game state, load resources)
        gameManager.Init();

        // Start the main game loop or gameplay sequence
        gameManager.Run();
    }
}