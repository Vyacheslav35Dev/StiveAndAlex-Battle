using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

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
        return _model.GetDamage();
    }

    public bool TryAddDamage(float damage)
    {
        float _finalDamage = _model.GetArmor(damage);
        
        _view.ShowTextEffect(_finalDamage.ToString(), Color.red);
        var isDead = _model.AddDamage(_finalDamage);
        if (!isDead)
        {
            var percent = _model.GetHealth() / 100;
            _view.UpdateHealthBar(percent);
            return false;
        }
        return true;
    }
    
    public void Dead()
    {
        _view.ShowDeadEffect();
    }

    public void Reset()
    {
        _view.Reset();
    }
}