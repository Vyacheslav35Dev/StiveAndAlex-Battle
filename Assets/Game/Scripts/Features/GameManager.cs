using UnityEngine;
using TextAsset = UnityEngine.TextAsset;

namespace Game.Scripts.Features
{
    public class GameManager : MonoBehaviour
    {
        [Header("Main panel")]
        [SerializeField] 
        private MainPanel mainPanel; // Reference to the main UI panel
        
        [Header("Camera Mover")]
        [SerializeField] 
        private CameraMover cameraMover; // Handles camera movement

        [Header("Players View")]
        [SerializeField] 
        private CharacterView playerView; // Visual representation of the player
        [SerializeField] 
        private CharacterView enemyView;  // Visual representation of the enemy
        
        private CharacterModel _playerModel; // Data model for the player
        private CharacterModel _enemyModel;  // Data model for the enemy

        private CharacterController _playerController; // Controller for player actions
        private CharacterController _enemyController;  // Controller for enemy actions
        
        private Data _settings; // Game settings loaded from JSON
        private bool _isGameOver; // Flag indicating if game has ended
        
        private bool _isPlayerBuffActive; // Buff status for player
        private bool _isEnemyBuffActive;  // Buff status for enemy

        /// <summary>
        /// Initializes game settings and prepares game state.
        /// </summary>
        public void Init()
        {
            var text = Resources.Load<TextAsset>("data"); // Load JSON data from Resources
            _settings = JsonUtility.FromJson<Data>(text.text); // Parse JSON into Data object
        }

        /// <summary>
        /// Starts or restarts the game, setting up models, controllers, and camera.
        /// </summary>
        public void Run()
        {
            cameraMover.Run(_settings.cameraSettings); // Initialize camera with settings
            
            // Create character models with current buff states
            _playerModel = new CharacterModel(_settings, _isPlayerBuffActive);
            _enemyModel = new CharacterModel(_settings, _isEnemyBuffActive);

            // Initialize player controller and link to view and model
            _playerController = new CharacterController();
            _playerController.Init(_playerModel, playerView);
            mainPanel.ShowPlayerPanelStats(_playerModel); // Update UI with player stats
            
            // Initialize enemy controller and link to view and model
            _enemyController = new CharacterController();
            _enemyController.Init(_enemyModel, enemyView);
            mainPanel.ShowEnemyPanelStats(_enemyModel);   // Update UI with enemy stats
        }
        
        /// <summary>
        /// Called when the attack button for the player is clicked.
        /// </summary>
        private void OnPlayerClickAttackButton()
        {
            TryAttackAction(_playerController, _enemyController);
        }
        
        /// <summary>
        /// Called when the attack button for the enemy is clicked.
        /// </summary>
        private void OnEnemyClickAttackButton()
        {
            TryAttackAction(_enemyController, _playerController);
        }

        /// <summary>
        /// Executes an attack from attacker to target and handles game over condition.
        /// </summary>
        private void TryAttackAction(CharacterController attacker, CharacterController target)
        {
            var damage = attacker.TryAttack(); // Calculate damage from attacker
            var stateDamage = target.TryAddDamage(damage); // Apply damage to target
            
            if (stateDamage.Item1) // If target's health reaches zero or below
            {
                _isGameOver = true; // Set game over flag
                mainPanel.ShowRestartPopup(); // Show restart prompt UI
                target.Dead(); // Handle target death (animations, disable controls)
            }
            
            attacker.TrySetVampireHealth(stateDamage.Item2); // Heal attacker based on damage dealt (vampirism)
        }
        
        /// <summary>
        /// Toggles player's buff status and restarts the game.
        /// </summary>
        private void OnPlayerClickBuffButton()
        {
            Reset(); // Reset current game state
            
            _isPlayerBuffActive = !_isPlayerBuffActive; // Toggle buff status
            
            Run(); // Restart game with updated buff status
        }
        
        /// <summary>
       /// Toggles enemy's buff status and restarts the game.
       /// </summary>
       private void OnEnemyClickBuffButton()
       {
           Reset(); // Reset current game state

           _isEnemyBuffActive = !_isEnemyBuffActive; // Toggle buff status

           Run(); // Restart game with updated buff status
       }

       /// <summary>
       /// Resets models and controllers to initial state without restarting the entire scene.
       /// </summary>
       private void Reset()
       {
           _playerModel.Reset();
           _enemyModel.Reset();
           _playerController.Reset();
           _enemyController.Reset();
       }

       /// <summary>
       /// Restarts the entire game session.
       /// </summary>
       private void Restart()
       {
           _isGameOver = false;
           Reset();
           Run();
       }

       #region Event Subscriptions

       private void OnEnable()
       {
           mainPanel.OnPlayerAttackAction += OnPlayerClickAttackButton;
           mainPanel.OnEnemyAttackAction += OnEnemyClickAttackButton;
           mainPanel.OnPlayerBuffAction += OnPlayerClickBuffButton;
           mainPanel.OnEnemyBuffAction += OnEnemyClickBuffButton;

           mainPanel.OnRestartAction += Restart;
       }

       private void OnDisable()
       {
           mainPanel.OnPlayerAttackAction -= OnPlayerClickAttackButton;
           mainPanel.OnEnemyAttackAction -= OnEnemyClickAttackButton;
           mainPanel.OnPlayerBuffAction -= OnPlayerClickBuffButton;
           mainPanel.OnEnemyBuffAction -= OnEnemyClickBuffButton;

           mainPanel.OnRestartAction -= Restart;
       }

       #endregion

       private void OnDestroy()
       {
           // Clear references to allow garbage collection if needed
           _playerModel = null;
           _playerController = null;

           _enemyModel = null;
           _enemyController = null;
       }
    }
}