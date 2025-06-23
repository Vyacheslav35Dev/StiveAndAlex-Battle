using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterModel
{
    public List<Stat> Stats { get; private set; }
    public List<Buff> Buffs { get; private set; }

    private float _health;
    private float _armor;
    private float _damage;

    public CharacterModel(List<Stat> stats, List<Buff> buffs)
    {
        Stats = stats;
        Buffs = buffs;
        
        var health = stats.FirstOrDefault(x => x.id == StatType.Health);
        var armor = stats.FirstOrDefault(x => x.id == StatType.Armor);
        var damage = stats.FirstOrDefault(x => x.id == StatType.Damage);
        
        _health = health?.value ?? 0;
        _armor = armor?.value ?? 0;
        _damage = damage?.value ?? 0;
    }
    
    public void AddBuff(Buff buff)
    {
        Buffs.Add(buff);
    }

    public float GetHealth()
    {
        return _health;
    }

    public float GetArmor(float damage)
    {
        var finalDamage = CalculateDamage(damage, _armor);
        var _buffArmor = 0f;

        foreach (var buff in Buffs)
        {
            _buffArmor += buff.stats.
                Where(buffStat => buffStat.statId == StatType.Armor)
                .Sum(buffStat => buffStat.value);
        }
        var finishDamage = CalculateDamage(finalDamage, _buffArmor);
        return finishDamage;
    }

    public float GetDamage()
    {
        var _buffDamage = 0f;
        var _vampirism = 0f;

        foreach (var buff in Buffs)
        {
            _buffDamage += buff.stats.
                Where(buffStat => buffStat.statId == StatType.Damage)
                .Sum(buffStat => buffStat.value);
        }
        
        foreach (var buff in Buffs)
        {
            _vampirism += buff.stats.
                Where(buffStat => buffStat.statId == StatType.Vampirism)
                .Sum(buffStat => buffStat.value);
        }
        
        return (_damage + _buffDamage);
    }

    public bool AddDamage(float damage)
    {
        var health = GetHealth();
        if (health > damage)
        {
            _health -= damage;
            return false;
        }
        return true;
    }
    
    private float CalculateDamage(float damage, float armorPercent)
    {
        float damageMultiplier = 1f - (armorPercent / 100f);
        return damage * damageMultiplier;
    }
}
