using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/InputReader", order = 1)]
public class InputReaderSO : ScriptableObject, InputSystem_Actions.IPlayerActions
{
    public event UnityAction<Vector2> RunEvent = delegate { };
    public event UnityAction<Vector2> LookEvent = delegate { };
    public event UnityAction<InputAction.CallbackContext> AttackEvent = delegate { };

    private InputSystem_Actions _gameInput;

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new InputSystem_Actions();

            _gameInput.Player.SetCallbacks(this);
        }
    }

    public void EnableGameplayInput()
    {
        _gameInput.Player.Enable();
        _gameInput.UI.Disable();
    }

    public void EnableMenuInput()
    {
        _gameInput.Player.Disable();
        _gameInput.UI.Enable();
    }

    // --- Event Listeners ---
    public void OnMove(InputAction.CallbackContext context) { RunEvent.Invoke(context.ReadValue<Vector2>()); }
    public void OnLook(InputAction.CallbackContext context) { 
        
        if(context.performed)
        {
            LookEvent.Invoke(context.ReadValue<Vector2>());
        }
    }
    public void OnAttack(InputAction.CallbackContext context) { AttackEvent.Invoke(context);  }
}
