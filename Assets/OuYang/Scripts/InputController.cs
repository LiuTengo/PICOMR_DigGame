using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

[RequireComponent(typeof(XRDeviceSimulator))] // �豸ģ����֧��
public class InputController : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] private InputActionAsset xriInputActions; // ����XRI Default Input Actions

    private InputAction leftTriggerAction;
    private InputAction rightTriggerAction;

    private ToolManager toolManager;

    private void Awake()
    {
        // ��ȡ�����ְ������
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
        // �󶨴����¼�
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
        if (context.ReadValue<float>() > 0.5f) // ��ѹ��ֵ
        {
            Debug.Log("���ְ������");
            toolManager.SwitchObject();
        }
    }

    private void OnRightTriggerPressed(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0.5f)
        {
            Debug.Log("���ְ������");
            toolManager.SwitchObject();
        }
    }
}