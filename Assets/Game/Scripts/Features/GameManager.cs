using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using TextAsset = UnityEngine.TextAsset;

namespace Game.Scripts.Features
{
    public class GameManager : MonoBehaviour
    {
        [Header("Main panel")]
        [SerializeField] 
        private MainPanel mainPanel;
        
        [Header("Camera Mover")]
        [SerializeField] 
        private CameraMover cameraMover;

        [Header("Players View")]
        [SerializeField] 
        private CharacterView playerView;
        [SerializeField] 
        private CharacterView enemyView;
        
        private CharacterModel _playerModel;
        private CharacterModel _enemyModel;

        private CharacterController _playerController;
        private CharacterController _enemyController;
        
        private Data _settings;
        private bool _isGameOver;
        
        public void Init()
        {
            var text = Resources.Load<TextAsset>("data");
            _settings = JsonUtility.FromJson<Data>(text.text);
        }

        public void Run()
        {
            cameraMover.Run(_settings.cameraSettings);
            
            _playerModel = new CharacterModel(_settings.stats.ToList(), new List<Buff>());
            _enemyModel = new CharacterModel(_settings.stats.ToList(), new List<Buff>());

            _playerController = new CharacterController();
            _playerController.Init(_playerModel, playerView);
            mainPanel.ShowPlayerPanelStats(_playerModel);
            
            _enemyController = new CharacterController();
            _enemyController.Init(_enemyModel, enemyView);
            mainPanel.ShowEnemyPanelStats(_enemyModel);
        }
        
        private void OnPlayerClickAttackButton()
        {
            TryAttackAction(_playerController, _enemyController);
        }
        
        private void OnEnemyClickAttackButton()
        {
            TryAttackAction(_enemyController, _playerController);
        }

        private void TryAttackAction(CharacterController attacker, CharacterController target)
        {
            var damage = attacker.TryAttack();
            var isDead = target.TryDamage(damage);
            if (isDead)
            {
                _isGameOver = true;
                mainPanel.ShowRestartPopup();
                target.Dead();
            }
        }
        
        private void OnPlayerClickBuffButton()
        {
            _playerController.Reset();
            _enemyController.Reset();
            Run();
        }
        
        private void OnEnemyClickBuffButton()
        {
            _playerController.Reset();
            _enemyController.Reset();
            Run();
        }

        private void Restart()
        {
            _isGameOver = false;
            _playerController.Reset();
            _enemyController.Reset();
            Run();
        }

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

        private void OnDestroy()
        {
            _playerModel = null;
            _playerController = null;

            _enemyModel = null;
            _enemyController = null;
        }
    }
}