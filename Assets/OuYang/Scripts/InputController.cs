using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] private InputActionAsset xriInputActions; // ����XRI Default Input Actions

    [SerializeField] private InputActionReference xrLeftTriggerAction;
    [SerializeField] private InputActionReference xrRightTriggerAction;
    
    // private InputAction leftTriggerAction;
    // private InputAction rightTriggerAction;
    
    

    private ToolManager toolManager;

    private void Awake()
    {
        // ��ȡ�����ְ������
        //InputActionMap leftHandMap = xriInputActions.FindActionMap("XRI LeftHand Interaction");
        //InputActionMap rightHandMap = xriInputActions.FindActionMap("XRI RightHand Interaction");
        //leftTriggerAction = leftHandMap.FindAction("Activate");
        //rightTriggerAction = rightHandMap.FindAction("Activate");
    }

    private void Start()
    {
        toolManager = ToolManager.Instance;
    }

    private void OnEnable()
    {
        // �󶨴����¼�
        //leftTriggerAction.performed += OnLeftTriggerPressed;
        //rightTriggerAction.performed += OnRightTriggerPressed;

        xrLeftTriggerAction.action.Enable();
        xrRightTriggerAction.action.Enable();
        
        xrLeftTriggerAction.action.performed += OnLeftTriggerPressed;
        xrRightTriggerAction.action.performed += OnRightTriggerPressed;
        
        //leftTriggerAction.Enable();
        //rightTriggerAction.Enable();
    }

    private void OnDisable()
    {
        xrLeftTriggerAction.action.Disable();
        xrRightTriggerAction.action.Disable();
        //leftTriggerAction.performed -= OnLeftTriggerPressed;
        //rightTriggerAction.performed -= OnRightTriggerPressed;

        //leftTriggerAction.Disable();
        //rightTriggerAction.Disable();
    }

    private void OnLeftTriggerPressed(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0.5f) // ��ѹ��ֵ
        {
            //Debug.Log("���ְ������");
            toolManager.SwitchObject();
        }
    }

    private void OnRightTriggerPressed(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0.5f)
        {
            //Debug.Log("���ְ������");
            toolManager.SwitchObject();
        }
    }
}