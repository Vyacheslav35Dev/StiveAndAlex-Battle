using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    [Header("Stats view")]
    [SerializeField]
    private StatsView statViewPrefab; // Prefab for displaying stats and buffs
    
    [Header("Player stats root panel")]
    [SerializeField]
    private Transform playerPanelRoot; // Parent transform for player's stats and buffs
    
    [Header("Enemy stats root panel")]
    [SerializeField]
    private Transform enemyPanelRoot; // Parent transform for enemy's stats and buffs
    
    [Header("Attack Buttons")]
    [SerializeField] 
    private Button playerAttackButton; // Button for player's attack
    [SerializeField] 
    private Button enemyAttackButton; // Button for enemy's attack
    
    [Header("Buff Buttons")]
    [SerializeField] 
    private Button playerActivateBuff; // Button to activate player's buff
    [SerializeField] 
    private Button playerDeActivateBuff; // Button to deactivate player's buff
    [SerializeField] 
    private Button enemyActivateBuff; // Button to activate enemy's buff
    [SerializeField] 
    private Button enemyDeActivateBuff; // Button to deactivate enemy's buff
    
    [Header("Restart Button")]
    [SerializeField] 
    private Button restartButton; // Button to restart the game
    
    [Header("Restart Popup")]
    [SerializeField] 
    private GameObject restartPopup; // Popup window shown on restart
    
    private bool _playerActivBuff = false; // Tracks if player's buff is active
    private bool _enemyActivBuff = false;  // Tracks if enemy's buff is active
    
    private List<StatsView> _playerBuffs = new List<StatsView>(); // List of player's buff views (not used in current code)
    private List<StatsView> _enemyBuffs = new List<StatsView>();   // List of enemy's buff views (not used in current code)

    public Action OnPlayerAttackAction; // Event invoked when player attacks
    public Action OnEnemyAttackAction;  // Event invoked when enemy attacks
    
    public Action OnPlayerBuffAction;   // Event invoked when player toggles buff
    public Action OnEnemyBuffAction;    // Event invoked when enemy toggles buff
    
    public Action OnRestartAction;      // Event invoked on restart

    private void Start()
    {
        // Initialize buff buttons: activate player's buff button, deactivate deactivation button, same for enemy
        playerActivateBuff.gameObject.SetActive(true);
        playerDeActivateBuff.gameObject.SetActive(false);
        enemyActivateBuff.gameObject.SetActive(true);
        enemyDeActivateBuff.gameObject.SetActive(false);
    }

    /// <summary>
    /// Displays the player's character stats and buffs on the UI panel.
    /// </summary>
    /// <param name="characterModel">The character data model containing stats and buffs.</param>
    public void ShowPlayerPanelStats(CharacterModel characterModel)
    {
        ResetPanel(playerPanelRoot); // Clear existing UI elements
        
        foreach (var modelStat in characterModel.Stats)
        {
            CreateStat(modelStat.icon, modelStat.value.ToString(), playerPanelRoot); // Create stat UI element
        }
        
        foreach (var modelBuff in characterModel.Buffs)
        {
            CreateBuff(modelBuff.icon, modelBuff.title, playerPanelRoot); // Create buff UI element
        }
    }

    /// <summary>
    /// Displays the enemy's character stats and buffs on the UI panel.
    /// </summary>
    /// <param name="characterModel">The character data model containing stats and buffs.</param>
    public void ShowEnemyPanelStats(CharacterModel characterModel)
    {
        ResetPanel(enemyPanelRoot); // Clear existing UI elements
        
        foreach (var modelStat in characterModel.Stats)
        {
            CreateStat(modelStat.icon, modelStat.value.ToString(), enemyPanelRoot); // Create stat UI element
        }
        
        foreach (var modelBuff in characterModel.Buffs)
        {
            CreateBuff(modelBuff.icon, modelBuff.title, enemyPanelRoot); // Create buff UI element
        }
    }

    private void CreateStat(string icon, string value, Transform root)
    {
        var obj = Instantiate(statViewPrefab, root); // Instantiate stat view prefab under specified parent
        var iconSprite = Resources.Load<Sprite>("Icons/" + icon); // Load icon sprite from Resources/Icons folder
        obj.Init(iconSprite, value); // Initialize the view with icon and value
    }

    private void CreateBuff(string icon, string value, Transform root)
    {
        var obj = Instantiate(statViewPrefab, root); // Instantiate buff view prefab under specified parent
        var iconSprite = Resources.Load<Sprite>("Icons/" + icon); // Load icon sprite from Resources/Icons folder
        obj.Init(iconSprite, value); // Initialize the view with icon and title
    }

    /// <summary>
    /// Clears all child objects from a given parent transform.
    /// </summary>
    /// <param name="parent">The parent transform to clear.</param>
    private void ResetPanel(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject); // Destroy each child game object
        }
    }

    /// <summary>
    /// Handler for player's attack button click.
    /// Invokes the attack action event.
    /// </summary>
    private void OnPlayerAttackClick()
    {
        OnPlayerAttackAction?.Invoke();
    }

    /// <summary>
    /// Handler for enemy's attack button click.
    /// Invokes the attack action event.
    /// </summary>
    private void OnEnemyAttackClick()
    {
        OnEnemyAttackAction?.Invoke();
    }

    /// <summary>
    /// Handler for player's buff toggle button click.
    /// Toggles the activation state and updates button visibility.
    /// Invokes the buff action event.
    /// </summary>
    private void OnPlayerBuffClick()
    {
        if (_playerActivBuff)
        {
            _playerActivBuff = false;
            playerActivateBuff.gameObject.SetActive(true);
            playerDeActivateBuff.gameObject.SetActive(false);
        }
        else
        {
            _playerActivBuff = true;
            playerActivateBuff.gameObject.SetActive(false);
            playerDeActivateBuff.gameObject.SetActive(true);
        }
        OnPlayerBuffAction?.Invoke();
    }

    /// <summary>
    /// Handler for enemy's buff toggle button click.
    /// Toggles the activation state and updates button visibility.
    /// Invokes the buff action event.
    /// </summary>
    private void OnEnemyBuffClick()
    {
        if (_enemyActivBuff)
        {
            _enemyActivBuff = false;
            enemyActivateBuff.gameObject.SetActive(true);
            enemyDeActivateBuff.gameObject.SetActive(false);
        }
        else
        {
            _enemyActivBuff = true;
            enemyActivateBuff.gameObject.SetActive(false);
            enemyDeActivateBuff.gameObject.SetActive(true);
        }
        OnEnemyBuffAction?.Invoke();
    }

    /// <summary>
    /// Displays the restart confirmation popup window.
    /// </summary>
    public void ShowRestartPopup()
    {
        restartPopup.SetActive(true);
    }

    /// <summary>
    /// Handler for restart button click. Hides popup and triggers restart event.
    /// </summary>
    private void OnRestartClick()
    {
        restartPopup.SetActive(false);
        OnRestartAction?.Invoke();
    }

    /// <summary>
    /// Resets UI elements to initial state: activates/deactivates buttons and clears panels.
    /// </summary>
    public void Reset()
    { 
        playerActivateBuff.gameObject.SetActive(true);
        playerDeActivateBuff.gameObject.SetActive(false);
        playerActivateBuff.gameObject.SetActive(true);
        playerDeActivateBuff.gameObject.SetActive(false);

        ResetPanel(playerPanelRoot);
        ResetPanel(enemyPanelRoot);
    }

    /// <summary>
    /// Subscribes to button click events when object is enabled.
    /// </summary>
    private void OnEnable()
    {
       playerAttackButton.onClick.AddListener(OnPlayerAttackClick);
       enemyAttackButton.onClick.AddListener(OnEnemyAttackClick);

       playerActivateBuff.onClick.AddListener(OnPlayerBuffClick);
       playerDeActivateBuff.onClick.AddListener(OnPlayerBuffClick);

       enemyActivateBuff.onClick.AddListener(OnEnemyBuffClick);
       enemyDeActivateBuff.onClick.AddListener(OnEnemyBuffClick);

       restartButton.onClick.AddListener(OnRestartClick);
    }

    /// <summary>
    /// Unsubscribes from button click events when object is disabled.
    /// </summary>
    private void OnDisable()
    {
       playerAttackButton.onClick.RemoveListener(OnPlayerAttackClick);
       enemyAttackButton.onClick.RemoveListener(OnEnemyAttackClick);

       playerActivateBuff.onClick.RemoveListener(OnPlayerBuffClick);
       playerDeActivateBuff.onClick.RemoveListener(OnPlayerBuffClick);

       enemyActivateBuff.onClick.RemoveListener(OnEnemyBuffClick);
       enemyDeActivateBuff.onClick.RemoveListener(OnEnemyBuffClick);

       restartButton.onClick.RemoveListener(OnRestartClick);
    }
}