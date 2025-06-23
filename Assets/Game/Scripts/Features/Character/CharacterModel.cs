using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterModel
{
    public List<Stat> Stats { get; private set; }
    public List<Buff> Buffs { get; private set; }

    private float _maxHealth;
    
    public CharacterModel(Data settings, bool activateBuffs)
    {
        var statsEnternal = new List<Stat>();
        foreach (var statSettings in settings.stats.ToList())
        {
            statsEnternal.Add(new Stat(statSettings.id, statSettings.title, statSettings.icon, statSettings.value));
        }
        
        Stats = statsEnternal;
        
        var playerBuffs = new List<Buff>();
        if (activateBuffs)
        {
            var count = Random.Range(settings.settings.buffCountMin, settings.settings.buffCountMax);
            for (int i = 0; i < count; i++)
            {
                var index = Random.Range(0, settings.buffs.Length);
                if (!settings.settings.allowDuplicateBuffs)
                {
                    var duplicate = playerBuffs.FirstOrDefault(x => x.id == settings.buffs[index].id);
                    if (duplicate == null)
                    {
                        playerBuffs.Add(settings.buffs[index]);
                    }
                }
                else
                {
                    playerBuffs.Add(settings.buffs[index]);
                }
            }
            var buffs = settings.buffs;
            buffs.Shuffle();
            playerBuffs = buffs.Take(count).ToList();
        }
        
        Buffs = playerBuffs;
        Init();
    }

    private void Init()
    {
        var health = Stats.FirstOrDefault(x => x.id == StatType.Health);
        var armor = Stats.FirstOrDefault(x => x.id == StatType.Armor);
        var damage = Stats.FirstOrDefault(x => x.id == StatType.Damage);
        var vampirism = Stats.FirstOrDefault(x => x.id == StatType.Vampirism);
        
        float _health = health?.value ?? 0;
        float _armor = armor?.value ?? 0;
        float _damage = damage?.value ?? 0;
        float _vampirerism = vampirism?.value ?? 0;
        
        foreach (var buff in Buffs)
        {
            _health += buff.stats.
                Where(buffStat => buffStat.statId == StatType.Health)
                .Sum(buffStat => buffStat.value);
            _armor += buff.stats.
                Where(buffStat => buffStat.statId == StatType.Armor)
                .Sum(buffStat => buffStat.value);
            _damage += buff.stats.
                Where(buffStat => buffStat.statId == StatType.Damage)
                .Sum(buffStat => buffStat.value);
            _vampirerism += buff.stats.
                Where(buffStat => buffStat.statId == StatType.Vampirism)
                .Sum(buffStat => buffStat.value);
        }

        if (health != null) health.value = _health;
        if (armor != null) armor.value = _armor;
        if (damage != null) damage.value = _damage;
        if (vampirism != null) vampirism.value = _vampirerism;
        _maxHealth = _health;
    }

    public float GetHealth()
    {
        var health = Stats.FirstOrDefault(x => x.id == StatType.Health);
        return health.value;
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    public float GetVampirism()
    {
        var vampirism = Stats.FirstOrDefault(x => x.id == StatType.Vampirism);
        return vampirism.value;
    }

    public float CalculateDamage(float damage)
    {
        var armor = Stats.FirstOrDefault(x => x.id == StatType.Armor);
        var finalDamage = CalculateDamage(damage, armor.value);
        return finalDamage;
    }

    public float CalculateAttack()
    {
        var damage = Stats.FirstOrDefault(x => x.id == StatType.Damage);
        return damage.value;
    }

    public bool AddDamage(float damage)
    {
        var health = Stats.FirstOrDefault(x => x.id == StatType.Health);
        if (health.value > damage)
        {
            health.value -= damage;
            return false;
        }
        return true;
    }

    public void AddHealth(float health)
    {
        var healthStat = Stats.FirstOrDefault(x => x.id == StatType.Health);
        healthStat.value += health;
        if (healthStat.value > _maxHealth)
        {
            healthStat.value = _maxHealth;
        }
    }
    
    private float CalculateDamage(float damage, float armorPercent)
    {
        float damageMultiplier = 1f - (armorPercent / 100f);
        return damage * damageMultiplier;
    }

    public void Reset()
    {
        Stats.Clear();
        Buffs.Clear();
    }
}
