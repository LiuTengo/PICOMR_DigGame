using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

[RequireComponent(typeof(XRDeviceSimulator))] // 设备模拟器支持
public class InputController : MonoBehaviour
{
    [Header("输入配置")]
    [SerializeField] private InputActionAsset xriInputActions; // 拖入XRI Default Input Actions

    private InputAction leftTriggerAction;
    private InputAction rightTriggerAction;

    private ToolManager toolManager;

    private void Awake()
    {
        // 获取左右手扳机动作
        InputActionMap leftHandMap = xriInputActions.FindActionMap("XRI LeftHand Interaction");
        InputActionMap rightHandMap = xriInputActions.FindActionMap("XRI RightHand Interaction");

        leftTriggerAction = leftHandMap.FindAction("Activate");
        rightTriggerAction = rightHandMap.FindAction("Activate");
    }

    private void Start()
    {
        toolManager = ToolManager.Instance;
    }

    private void OnEnable()
    {
        // 绑定触发事件
        leftTriggerAction.performed += OnLeftTriggerPressed;
        rightTriggerAction.performed += OnRightTriggerPressed;

        leftTriggerAction.Enable();
        rightTriggerAction.Enable();
    }

    private void OnDisable()
    {
        leftTriggerAction.performed -= OnLeftTriggerPressed;
        rightTriggerAction.performed -= OnRightTriggerPressed;

        leftTriggerAction.Disable();
        rightTriggerAction.Disable();
    }

    private void OnLeftTriggerPressed(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0.5f) // 按压阈值
        {
            Debug.Log("左手扳机按下");
            toolManager.SwitchObject();
        }
    }

    private void OnRightTriggerPressed(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0.5f)
        {
            Debug.Log("右手扳机按下");
            toolManager.SwitchObject();
        }
    }
}