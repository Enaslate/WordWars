using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController
{
    public event Action<char> TextInputted;
    public event Action ConfirmPerformed;
    public event Action CancelPerformed;

    public bool IsEnable { get; private set; }

    private readonly InputActions _actions;

    public InputController(InputActions actions)
    {
        _actions = actions;
    }

    public void Enable()
    {
        if (_actions == null)
        {
            Debug.LogError($"{_actions.GetType().Name} is null");
            return;
        }

        IsEnable = true;
        _actions.Enable();

        _actions.Player.Confirm.performed += OnConfirmPerformed;
        _actions.Player.Cancel.performed += OnCancelPerformed;
        Keyboard.current.onTextInput += OnTextInputPressed;
    }

    public void Disable()
    {
        IsEnable = false;
        _actions?.Disable();

        if (_actions == null) return;

        _actions.Player.Confirm.performed -= OnConfirmPerformed;
        _actions.Player.Cancel.performed -= OnCancelPerformed;
        Keyboard.current.onTextInput -= OnTextInputPressed;
    }

    private void OnConfirmPerformed(InputAction.CallbackContext context)
    {
        ConfirmPerformed?.Invoke();
    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        CancelPerformed?.Invoke();
    }

    private void OnTextInputPressed(char inputChar)
    {
        if (char.IsControl(inputChar))
            return;

        TextInputted?.Invoke(inputChar);
        Debug.Log($"{inputChar}");
    }
}