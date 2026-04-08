using TMPro;
using UnityEngine;

public class HudView : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthField;
    private PlayerCharacter _player;

    public void Setup(PlayerCharacter player)
    {
        _player = player;

        _player.TakedDamage += OnTakedDamage;
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        _healthField.text = $"{_player.Health}/{_player.MaxHealth}";
    }

    private void OnTakedDamage()
    {
        UpdateHealthText();
    }

    private void OnDestroy()
    {
        _player.TakedDamage -= OnTakedDamage;
    }
}