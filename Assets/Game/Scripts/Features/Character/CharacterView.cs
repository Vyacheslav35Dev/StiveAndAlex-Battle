using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour
{
    [SerializeField] 
    private Animator animator; // Reference to the character's animator

    [Header("Healthbar")]
    [SerializeField]
    private Transform canvasTransform; // Transform of the canvas that faces the camera
    [SerializeField]
    private Image healthBarSlider; // UI Image representing the health bar fill

    [Header("Text effects")]
    [SerializeField]
    private TMP_Text text; // Text component for displaying floating text
    [SerializeField]
    private CanvasGroup canvasGroup; // CanvasGroup for controlling text visibility and fade
    [SerializeField]
    private float moveDistance = 4f; // Distance the text moves upward during animation
    [SerializeField]
    private float duration = 0.10f; // Duration of the text move animation

    /// <summary>
    /// Initializes the character view by setting default states.
    /// </summary>
    public void Init()
    {
        if (animator != null)
        {
            animator.Play("basePlayer_idle");
        }
        if (healthBarSlider != null)
        {
            healthBarSlider.gameObject.SetActive(true);
        }
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }
        if (text != null)
        {
            text.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Ensures the health bar's canvas always faces the main camera.
    /// Called every frame after all Update functions.
    /// </summary>
    private void LateUpdate()
    {
        if (canvasTransform != null && Camera.main != null)
        {
            canvasTransform.LookAt(Camera.main.transform);
        }
    }

    /// <summary>
    /// Updates the health bar fill amount.
    /// </summary>
    /// <param name="value">Normalized health value between 0 and 1.</param>
    public void UpdateHealthBar(float value)
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.fillAmount = Mathf.Clamp01(value);
        }
    }

    /// <summary>
    /// Triggers attack animation.
    /// </summary>
    public void ShowAttackEffect()
    {
        if (animator != null)
        {
            animator.SetBool("Attack", true);
            // Optionally, reset attack bool after animation completes
            // animator.SetBool("Attack", false);
        }
    }

    /// <summary>
    /// Displays floating text with color effect.
    /// </summary>
    /// <param name="text">Text to display.</param>
    /// <param name="color">Color of the text.</param>
    public void ShowTextEffect(string text, Color color)
    {
        if (this.text != null)
        {
            this.text.text = text;
            this.text.color = color;
            AnimateText();
        }
   }

   /// <summary>
   /// Displays dead effect by hiding health bar and setting health to zero.
   /// </summary>
   public void ShowDeadEffect()
   {
       if (healthBarSlider != null)
       {
           healthBarSlider.gameObject.SetActive(false);
       }
       if (animator != null)
       {
           animator.SetInteger("Health", 0);
       }
   }

   /// <summary>
   /// Animates floating text moving upward and then resets its state.
   /// </summary>
   private void AnimateText()
   {
       if (text == null) return;

       // Ensure CanvasGroup exists for fade control
       if (canvasGroup == null)
       {
           canvasGroup = GetComponent<CanvasGroup>();
           if (canvasGroup == null)
           {
               canvasGroup = gameObject.AddComponent<CanvasGroup>();
           }
       }

       // Activate text object
       text.gameObject.SetActive(true);

       // Calculate target position for upward movement
       Vector3 targetPosition = text.transform.localPosition + new Vector3(0, moveDistance, 0);

       // Animate movement using DOTween
       text.transform.DOLocalMove(targetPosition, duration).SetEase(Ease.OutCubic).OnComplete(() =>
       {
           // Reset position and hide after animation completes
           text.gameObject.SetActive(false);
           text.transform.localPosition = Vector3.zero;
           // Optional: Fade out effect can be added here
           // StartCoroutine(FadeOutText());
       });
   }

   /*
   private IEnumerator FadeOutText()
   {
       float elapsedTime = 0f;
       float fadeDuration = 0.2f; // Duration of fade out

       while (elapsedTime < fadeDuration)
       {
           elapsedTime += Time.deltaTime;
           canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
           yield return null;
       }

       canvasGroup.alpha = 1f; // Reset alpha for next use
   }
   */

   /// <summary>
   /// Resets character state to default.
   /// </summary>
   public void Reset()
   {
       if (animator != null)
       {
           animator.SetInteger("Health", 100); // Assuming max health is 100
           animator.SetBool("Attack", false);
       }
       
       // Reset health bar visibility and value
       if (healthBarSlider != null)
       {
           healthBarSlider.gameObject.SetActive(true);
           UpdateHealthBar(1f); // Full health
       }

       // Hide floating text if active
       if (text != null && text.gameObject.activeSelf)
       {
           text.gameObject.SetActive(false);
           text.transform.localPosition = Vector3.zero;
           if (canvasGroup != null)
               canvasGroup.alpha = 1;
       }
   }
}