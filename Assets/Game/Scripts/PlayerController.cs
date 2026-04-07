using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerView _viewPrefab;

    public PlayerView PlayerView { get; private set; }

    public void Spawn(PlayerCharacter character)
    {
        if (PlayerView == null)
            PlayerView = Instantiate(_viewPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);

        PlayerView.Setup(character);
        PlayerView.Character.Died += OnDied;
    }

    private void OnDied(Character character)
    {
        PlayerView.Character.Died -= OnDied;
        Time.timeScale = 0;
        Debug.Log("Game over!");
    }
}