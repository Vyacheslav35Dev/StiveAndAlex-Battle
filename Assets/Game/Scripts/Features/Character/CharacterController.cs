using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the interaction between the character model and view.
/// </summary>
public class CharacterController
{
    private CharacterModel _model;
    private CharacterView _view;

    /// <summary>
    /// Initializes the controller with model and view, and sets up initial UI state.
    /// </summary>
    /// <param name="model">The character's data model.</param>
    /// <param name="view">The character's visual representation.</param>
    public void Init(CharacterModel model, CharacterView view)
    {
        _model = model;
        _view = view;
        _view.Init();
        _view.UpdateHealthBar(1f); // Set health bar to full at start
    }

    /// <summary>
    /// Executes an attack action, showing attack effect and returning attack damage.
    /// </summary>
    /// <returns>The attack damage value.</returns>
    public float TryAttack()
    {
        _view.ShowAttackEffect();
        return _model.CalculateAttack();
    }

    /// <summary>
    /// Applies damage to the character, updates health bar, and indicates if dead.
    /// </summary>
    /// <param name="damage">Incoming damage before calculations.</param>
    /// <returns>Tuple indicating if character is dead and the final damage applied.</returns>
    public (bool isDead, float finalDamage) TryAddDamage(float damage)
    {
        float finalDamage = _model.CalculateDamage(damage);

        // Show damage text effect in red
        _view.ShowTextEffect(finalDamage.ToString("F0"), Color.red);

        // Apply damage to the model
        bool isDead = _model.AddDamage(finalDamage);

        if (!isDead)
        {
            float healthPercent = _model.GetHealth() / _model.GetMaxHealth();
            _view.UpdateHealthBar(healthPercent);
            return (false, finalDamage);
        }
        else
        {
            // Character is dead
            return (true, finalDamage);
        }
    }

    /// <summary>
    /// Heals the character based on vampirism percentage.
    /// </summary>
    /// <param name="damage">The damage dealt which determines healing amount.</param>
    public void TrySetVampireHealth(float damage)
    {
        float healAmount = damage * (_model.GetVampirism() / 100f);
        if (healAmount <= 0f)
        {
            return; // No healing if vampirism is zero or negative
        }

        // Show healing text in green
        _view.ShowTextEffect(healAmount.ToString("F0"), Color.green);
        _model.AddHealth(healAmount);
    }

    /// <summary>
    /// Handles character death effects.
    /// </summary>
    public void Dead()
    {
        _view.ShowDeadEffect();
    }

    /// <summary>
    /// Resets the character's model and view to initial state.
    /// </summary>
    public void Reset()
    {
        _model.Reset();
        _view.Reset();
        
        // Reset health bar to full after reset
        _view.UpdateHealthBar(1f);
    }
}