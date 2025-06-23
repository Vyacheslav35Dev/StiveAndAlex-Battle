using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterModel
{
    // List of character stats
    public List<Stat> Stats { get; private set; }
    // List of active buffs
    public List<Buff> Buffs { get; private set; }

    private float _maxHealth;

    /// <summary>
    /// Constructor initializes stats and buffs based on settings.
    /// </summary>
    /// <param name="settings">Character data settings</param>
    /// <param name="activateBuffs">Whether to activate buffs</param>
    public CharacterModel(Data settings, bool activateBuffs)
    {
        // Initialize stats from settings
        Stats = new List<Stat>();
        foreach (var statSettings in settings.stats)
        {
            Stats.Add(new Stat(statSettings.id, statSettings.title, statSettings.icon, statSettings.value));
        }

        // Initialize buffs if activation is enabled
        Buffs = new List<Buff>();
        if (activateBuffs)
        {
            int count = Random.Range(settings.settings.buffCountMin, settings.settings.buffCountMax + 1); // +1 because Range is exclusive on upper bound

            var availableBuffs = settings.buffs.ToList();

            // Shuffle the buffs list for randomness
            availableBuffs.Shuffle();

            for (int i = 0; i < count && availableBuffs.Count > 0; i++)
            {
                int index = Random.Range(0, availableBuffs.Count);
                var selectedBuff = availableBuffs[index];

                if (!settings.settings.allowDuplicateBuffs)
                {
                    // Avoid duplicates
                    if (!Buffs.Any(x => x.id == selectedBuff.id))
                    {
                        Buffs.Add(selectedBuff);
                    }
                }
                else
                {
                    Buffs.Add(selectedBuff);
                }

                // Remove selected buff if duplicates are not allowed to prevent re-selection
                if (!settings.settings.allowDuplicateBuffs)
                {
                    availableBuffs.RemoveAt(index);
                }
            }
        }

        Init();
    }

    /// <summary>
    /// Initializes character stats considering buffs.
    /// </summary>
    private void Init()
    {
        var health = Stats.FirstOrDefault(x => x.id == StatType.Health);
        var armor = Stats.FirstOrDefault(x => x.id == StatType.Armor);
        var damage = Stats.FirstOrDefault(x => x.id == StatType.Damage);
        var vampirism = Stats.FirstOrDefault(x => x.id == StatType.Vampirism);

        float baseHealth = health?.value ?? 0f;
        float baseArmor = armor?.value ?? 0f;
        float baseDamage = damage?.value ?? 0f;
        float baseVampirism = vampirism?.value ?? 0f;

        // Aggregate buff effects
        foreach (var buff in Buffs)
        {
            baseHealth += buff.stats.Where(s => s.statId == StatType.Health).Sum(s => s.value);
            baseArmor += buff.stats.Where(s => s.statId == StatType.Armor).Sum(s => s.value);
            baseDamage += buff.stats.Where(s => s.statId == StatType.Damage).Sum(s => s.value);
            baseVampirism += buff.stats.Where(s => s.statId == StatType.Vampirism).Sum(s => s.value);
        }

        // Update stats with total values
        if (health != null) health.value = baseHealth;
        if (armor != null) armor.value = baseArmor;
        if (damage != null) damage.value = baseDamage;
        if (vampirism != null) vampirism.value = baseVampirism;

        _maxHealth = baseHealth;
    }

    /// <summary>
    /// Gets current health value.
    /// </summary>
    public float GetHealth()
    {
        return Stats.FirstOrDefault(x => x.id == StatType.Health)?.value ?? 0f;
    }

    /// <summary>
    /// Gets maximum health value.
    /// </summary>
    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    /// <summary>
    /// Gets current vampirism value.
    /// </summary>
    public float GetVampirism()
    {
        return Stats.FirstOrDefault(x => x.id == StatType.Vampirism)?.value ?? 0f;
    }

    /// <summary>
    /// Calculates damage after applying armor reduction.
    /// </summary>
    public float CalculateDamage(float damage)
    {
        var armorStat = Stats.FirstOrDefault(x => x.id == StatType.Armor);
        float armorPercent = armorStat?.value ?? 0f;
        return CalculateDamage(damage, armorPercent);
    }

    /// <summary>
    /// Gets the attack damage value.
    /// </summary>
    public float CalculateAttack()
    {
        return Stats.FirstOrDefault(x => x.id == StatType.Damage)?.value ?? 0f;
    }

   /// <summary>
   /// Applies damage to health. Returns true if character is dead.
   /// </summary>
   public bool AddDamage(float damage)
   {
       var healthStat = Stats.FirstOrDefault(x => x.id == StatType.Health);
       if (healthStat != null)
       {
           if (healthStat.value > damage)
           {
               healthStat.value -= damage;
               return false; // Not dead yet
           }
           else
           {
               healthStat.value = 0; // Health can't be negative
               return true; // Dead
           }
       }
       return false; // No health stat found, assume not dead
   }

   /// <summary>
   /// Adds health to the character, capped at max health.
   /// </summary>
   public void AddHealth(float health)
   {
       var healthStat = Stats.FirstOrDefault(x => x.id == StatType.Health);
       if (healthStat != null)
       {
           healthStat.value += health;
           if (healthStat.value > _maxHealth)
               healthStat.value = _maxHealth;
       }
   }

   /// <summary>
   /// Calculates final damage considering armor percentage reduction.
   /// </summary>
   private float CalculateDamage(float damage, float armorPercent)
   {
       float damageMultiplier = 1f - (armorPercent / 100f);
       return damage * damageMultiplier;
   }

   /// <summary>
   /// Resets stats and buffs.
   /// </summary>
   public void Reset()
   {
       Stats.Clear();
       Buffs.Clear();
   }
}