using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsView : MonoBehaviour
{
   [SerializeField] 
   private Image icon;

   [SerializeField] 
   private TMP_Text value;

   public void Init(Sprite sprite, string value)
   {
      icon.sprite = sprite;
      this.value.text = value;
   }
}
