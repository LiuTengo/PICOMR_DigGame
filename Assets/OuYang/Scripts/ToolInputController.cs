using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class ToolInputController : MonoBehaviour
{
    [SerializeField] private InputActionReference xrLeftTriggerAction;
    [SerializeField] private InputActionReference xrRightTriggerAction;
    
    [SerializeField] private InputActionReference yButtonPressedAction;
    [SerializeField] private InputActionReference switchToolAction;

    [SerializeField] private ActionBasedSnapTurnProvider snapTurn;
    [SerializeField] private ActionBasedContinuousTurnProvider continuousTurn;
    
    private ToolManager toolManager;

    private void Start()
    {
        toolManager = GetComponent<ToolManager>();
    }

    private void OnEnable()
    {
        snapTurn.enabled = true;
        continuousTurn.enabled = true;
        yButtonPressedAction.action.Enable();
        switchToolAction.action.Enable();
        
        switchToolAction.action.performed += TestButton;
        
        yButtonPressedAction.action.started += DisableRightTurnInput;
        yButtonPressedAction.action.canceled += EnableRightTurnInput;
    }

    private void EnableRightTurnInput(InputAction.CallbackContext obj)
    {
        //switchToolAction.action.Enable();
        
        snapTurn.enabled = true;
        continuousTurn.enabled = true;
    }

    private void DisableRightTurnInput(InputAction.CallbackContext obj)
    {
        //switchToolAction.action.Disable();
        snapTurn.enabled = false;
        continuousTurn.enabled = false;
    }

    private void OnDisable()
    {
        snapTurn.enabled = true;
        continuousTurn.enabled = true;
        
        switchToolAction.action.performed -= TestButton;
        
        yButtonPressedAction.action.started -= DisableRightTurnInput;
        yButtonPressedAction.action.canceled -= EnableRightTurnInput;
        
        yButtonPressedAction.action.Disable();
        switchToolAction.action.Disable();
    }

    private void TestButton(InputAction.CallbackContext context)
    {
        var val = context.ReadValue<Vector2>();
        if (val.x > 0.5f)
        {
            Debug.LogWarning("Use Next Tool");
            toolManager.SwitchToNextObject();
        }
        else if (val.x < -0.5f)
        {
            Debug.LogWarning("Use Primary Tool");
            toolManager.SwitchToPrimaryObject();
        }
    }

}