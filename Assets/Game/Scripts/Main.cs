using UnityEngine;

public class Main : MonoBehaviour
{
    private InputActions _inputActions;
    private InputController _inputController;

    private void Awake()
    {
        ConfigureInput();
    }

    private void Start()
    {
        
    }

    private void ConfigureInput()
    {
        _inputActions = new InputActions();
        _inputController = new InputController(_inputActions);
    }
}
