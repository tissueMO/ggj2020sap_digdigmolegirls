// GENERATED AUTOMATICALLY FROM 'Assets/Kamata/InputActions/Player2Act.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Player2Act : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Player2Act()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player2Act"",
    ""maps"": [
        {
            ""name"": ""Move"",
            ""id"": ""d4b10749-737c-4973-93c7-8a867b8708cc"",
            ""actions"": [
                {
                    ""name"": ""Forward"",
                    ""type"": ""Button"",
                    ""id"": ""3f9d864e-846f-40a0-b3c8-dd3f894ea34a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""d2398542-ab8f-4581-935a-18f6ff4938f2"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""3557e0f7-1c5f-49d9-a93c-d9cc4c3ef1e7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8a8f91de-d670-4704-9989-d306f15fe201"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Forward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a9b24efc-1773-44dc-87f8-185725e9e2d8"",
                    ""path"": ""<SwitchProControllerHID>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Forward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""7053c8e9-cf37-46c8-a139-c09335e37ab6"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""9d749f17-3179-44de-b852-13a6604e82a6"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""6e6bb2dd-c028-439c-8ecd-f855224a1656"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""19d51a0e-7944-4cee-83bc-77e6ebd8d7be"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""e1a6d93b-1337-4026-a694-5a339bb24d25"",
                    ""path"": ""<SwitchProControllerHID>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""6530dda5-392e-49ff-a541-681e0bcf84e4"",
                    ""path"": ""<SwitchProControllerHID>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""cbef82b5-d530-4afb-a510-d0d588e19a9b"",
                    ""path"": ""<SwitchProControllerHID>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Move
        m_Move = asset.FindActionMap("Move", throwIfNotFound: true);
        m_Move_Forward = m_Move.FindAction("Forward", throwIfNotFound: true);
        m_Move_Rotate = m_Move.FindAction("Rotate", throwIfNotFound: true);
        m_Move_Pause = m_Move.FindAction("Pause", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Move
    private readonly InputActionMap m_Move;
    private IMoveActions m_MoveActionsCallbackInterface;
    private readonly InputAction m_Move_Forward;
    private readonly InputAction m_Move_Rotate;
    private readonly InputAction m_Move_Pause;
    public struct MoveActions
    {
        private @Player2Act m_Wrapper;
        public MoveActions(@Player2Act wrapper) { m_Wrapper = wrapper; }
        public InputAction @Forward => m_Wrapper.m_Move_Forward;
        public InputAction @Rotate => m_Wrapper.m_Move_Rotate;
        public InputAction @Pause => m_Wrapper.m_Move_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Move; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MoveActions set) { return set.Get(); }
        public void SetCallbacks(IMoveActions instance)
        {
            if (m_Wrapper.m_MoveActionsCallbackInterface != null)
            {
                @Forward.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnForward;
                @Forward.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnForward;
                @Forward.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnForward;
                @Rotate.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnRotate;
                @Pause.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_MoveActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Forward.started += instance.OnForward;
                @Forward.performed += instance.OnForward;
                @Forward.canceled += instance.OnForward;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public MoveActions @Move => new MoveActions(this);
    public interface IMoveActions
    {
        void OnForward(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}
