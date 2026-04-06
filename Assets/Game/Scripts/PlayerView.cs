using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public PlayerCharacter Character;

    public void Setup(PlayerCharacter character)
    {
        Character = character;
    }
}