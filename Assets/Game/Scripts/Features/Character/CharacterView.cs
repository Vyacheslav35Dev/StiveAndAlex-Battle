using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour
{
    [SerializeField] 
    private Animator animator;
    
    [Header("Healthbar")]
    [SerializeField]
    private Transform canvasTransform;
    [SerializeField]
    private Image healthBarSlider;

    [Header("Text effects")] 
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private float moveDistance = 4;
    [SerializeField]
    private float duration = 0.10f;
    
    public void Init()
    {
        animator.Play("basePlayer_idle");
        healthBarSlider.gameObject.SetActive(true);
    }
    
    private void LateUpdate()
    {
        canvasTransform.LookAt(Camera.main.transform);
    }

    public void UpdateHealthBar(float value)
    {
        healthBarSlider.fillAmount = value;
    }

    public void ShowAttackEffect()
    {
        animator.SetBool("Attack", true);
    }

    public void ShowTextEffect(string text, Color color)
    {
        this.text.text = text;
        this.text.color = color;
        AnimateText();
    }
    
    public void ShowDeadEffect()
    {
        healthBarSlider.gameObject.SetActive(false);
        animator.SetInteger("Health", 0);
    }
    
    private void AnimateText()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }
        this.text.gameObject.SetActive(true);
        Vector3 targetPosition = this.text.transform.localPosition + new Vector3(0, moveDistance, 0);
        
        this.text.transform.DOLocalMove(targetPosition, duration).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            this.text.gameObject.SetActive(false);
            canvasGroup.alpha = 1;
            this.text.transform.localPosition = Vector3.zero;
        });
    }

    public void Reset()
    {
        animator.SetInteger("Health", 100);
        animator.SetBool("Attack", false);
    }
}
