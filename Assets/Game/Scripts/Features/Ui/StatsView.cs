using TMPro; // TextMeshPro for advanced text rendering
using UnityEngine;
using UnityEngine.UI; // UI components like Image

/// <summary>
/// A component to display a stat with an icon and a value.
/// </summary>
public class StatsView : MonoBehaviour
{
    [SerializeField]
    private Image icon; // UI Image to display the icon sprite

    [SerializeField]
    private TMP_Text value; // TextMeshPro text component to display the stat value

    /// <summary>
    /// Initializes the StatsView with a sprite icon and a string value.
    /// </summary>
    /// <param name="sprite">The sprite to set as the icon.</param>
    /// <param name="value">The string representation of the stat value.</param>
    public void Init(Sprite sprite, string value)
    {
        if (icon != null)
        {
            icon.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("Icon Image component is not assigned.");
        }

        if (this.value != null)
        {
            this.value.text = value;
        }
        else
        {
            Debug.LogWarning("Value TMP_Text component is not assigned.");
        }
    }

    /// <summary>
    /// Optional: Method to update only the value without changing the icon.
    /// </summary>
    /// <param name="newValue">New string value to display.</param>
    public void UpdateValue(string newValue)
    {
        if (this.value != null)
        {
            this.value.text = newValue;
        }
        else
        {
            Debug.LogWarning("Value TMP_Text component is not assigned.");
        }
    }

    /// <summary>
    /// Optional: Method to update only the icon sprite without changing the value.
    /// </summary>
    /// <param name="sprite">New sprite for the icon.</param>
    public void UpdateIcon(Sprite sprite)
    {
        if (icon != null)
        {
            icon.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("Icon Image component is not assigned.");
        }
    }
}