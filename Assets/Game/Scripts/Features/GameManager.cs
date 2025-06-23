using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;
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
        
        private bool _isPlayerBuffActive;
        private bool _isEnemyBuffActive;
        
        public void Init()
        {
            var text = Resources.Load<TextAsset>("data");
            _settings = JsonUtility.FromJson<Data>(text.text);
        }

        public void Run()
        {
            cameraMover.Run(_settings.cameraSettings);
            
            _playerModel = new CharacterModel(_settings, _isPlayerBuffActive);
            _enemyModel = new CharacterModel(_settings, _isEnemyBuffActive);

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
            var stateDamage = target.TryAddDamage(damage);
            if (stateDamage.Item1)
            {
                _isGameOver = true;
                mainPanel.ShowRestartPopup();
                target.Dead();
            }
            attacker.TrySetVampireHealth(stateDamage.Item2);
        }
        
        private void OnPlayerClickBuffButton()
        {
            Reset();
            if (_isPlayerBuffActive)
            {
                _isPlayerBuffActive = false;
            }
            else
            {
                _isPlayerBuffActive = true;
            }
            Run();
        }
        
        private void OnEnemyClickBuffButton()
        {
            Reset();
            if (_isEnemyBuffActive)
            {
                _isEnemyBuffActive = false;
            }
            else
            {
                _isEnemyBuffActive = true;
            }
            Run();
        }

        private void Reset()
        {
            _playerModel.Reset();
            _enemyModel.Reset();
            _playerController.Reset();
            _enemyController.Reset();
        }

        private void Restart()
        {
            _isGameOver = false;
            Reset();
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