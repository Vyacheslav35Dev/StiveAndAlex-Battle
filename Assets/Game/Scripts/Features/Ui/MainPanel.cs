using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    [Header("Stats view")]
    [SerializeField]
    private StatsView statViewPrefab;
    
    [Header("Player stats root panel")]
    [SerializeField]
    private Transform playerPanelRoot;
    
    [Header("Enemy stats root panel")]
    [SerializeField]
    private Transform enemyPanelRoot;
    
    [Header("Attack Buttons")]
    [SerializeField] 
    private Button playerAttackButton;
    [SerializeField] 
    private Button enemyAttackButton;
    
    [Header("Buff Buttons")]
    [SerializeField] 
    private Button playerActivateBuff;
    [SerializeField] 
    private Button playerDeActivateBuff;
    [SerializeField] 
    private Button enemyActivateBuff;
    [SerializeField] 
    private Button enemyDeActivateBuff;
    
    [Header("Restart Button")]
    [SerializeField] 
    private Button restartButton;
    
    [Header("Restart Popup")]
    [SerializeField] 
    private GameObject restartPopup;
    
    private bool _playerActivBuff = false;
    private bool _enemyActivBuff = false;
    
    private List<StatsView> _playerBuffs = new List<StatsView>();
    private List<StatsView> _enemyBuffs = new List<StatsView>();

    public Action OnPlayerAttackAction;
    public Action OnEnemyAttackAction;
    
    public Action OnPlayerBuffAction;
    public Action OnEnemyBuffAction;
    
    public Action OnRestartAction;

    private void Start()
    {
        playerActivateBuff.gameObject.SetActive(true);
        playerDeActivateBuff.gameObject.SetActive(false);
        enemyActivateBuff.gameObject.SetActive(true);
        enemyDeActivateBuff.gameObject.SetActive(false);
    }

    public void ShowPlayerPanelStats(CharacterModel characterModel)
    {
        Reset(playerPanelRoot);
        foreach (var modelStat in characterModel.Stats)
        {
            CreateStat(modelStat.icon, modelStat.value.ToString(), playerPanelRoot);
        }
    }
    
    public void ShowEnemyPanelStats(CharacterModel characterModel)
    {
        Reset(enemyPanelRoot);
        foreach (var modelStat in characterModel.Stats)
        {
            CreateStat(modelStat.icon, modelStat.value.ToString(), enemyPanelRoot);
        }
    }

    private void CreateStat(string icon, string value, Transform root)
    {
        var obj = Instantiate(statViewPrefab, root);
        var iconSprite = Resources.Load<Sprite>("Icons/" + icon);
        obj.Init(iconSprite, value);
    }

    private void Reset(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    private void OnPlayerAttackClick()
    {
        OnPlayerAttackAction?.Invoke();
    }
    
    private void OnEnemyAttackClick()
    {
        OnEnemyAttackAction?.Invoke();
    }
    
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

    public void ShowRestartPopup()
    {
        restartPopup.SetActive(true);
    }
    
    private void OnRestartClick()
    {
        restartPopup.SetActive(false);
        OnRestartAction?.Invoke();
    }

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
