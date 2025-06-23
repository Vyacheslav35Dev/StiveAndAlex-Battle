using Game.Scripts.Features;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        gameManager.Run();
    }
}
