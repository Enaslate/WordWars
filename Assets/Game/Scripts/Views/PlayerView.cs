using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [field: SerializeField] public PlayerCharacter Character { get; private set; }

    public void Setup(PlayerCharacter character)
    {
        Character = character;
    }
}