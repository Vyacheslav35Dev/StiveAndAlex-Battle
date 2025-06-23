using UnityEngine;
using UnityEngine.UI;

public class CharacterController
{
    private CharacterModel _model;
    private CharacterView _view;
        
    public void Init(CharacterModel model, CharacterView view)
    {
        _model = model;
        _view = view;
        _view.Init();
        _view.UpdateHealthBar(1f);
    }

    public float TryAttack()
    {
        _view.ShowAttackEffect();
        return _model.CalculateAttack();
    }

    public (bool, float) TryAddDamage(float damage)
    {
        float finalDamage = _model.CalculateDamage(damage);
        
        _view.ShowTextEffect(finalDamage.ToString(), Color.red);
        var isDead = _model.AddDamage(finalDamage);
        if (!isDead)
        {
            var percent = _model.GetHealth() / _model.GetMaxHealth();
            _view.UpdateHealthBar(percent);
            return (false, finalDamage);
        }
        return (true, finalDamage);
    }

    public void TrySetVampireHealth(float damage)
    {
        float healAmount = damage * (_model.GetVampirism() / 100f);
        if (healAmount == 0)
        {
            return;
        }
        _view.ShowTextEffect(healAmount.ToString(), Color.green);
        _model.AddHealth(healAmount);
    }
    
    public void Dead()
    {
        _view.ShowDeadEffect();
    }

    public void Reset()
    {
        _model.Reset();
        _view.Reset();
    }
}