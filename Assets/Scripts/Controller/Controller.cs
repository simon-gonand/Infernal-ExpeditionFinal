// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Controller/Controller.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controller : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controller()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controller"",
    ""maps"": [
        {
            ""name"": ""Controls"",
            ""id"": ""b27e42dd-1ef6-4e9b-ac37-2f32bd699275"",
            ""actions"": [
                {
                    ""name"": ""PlayerMovements"",
                    ""type"": ""PassThrough"",
                    ""id"": ""04ac00af-7e50-4866-b008-c5226b5dd8a2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerInteraction"",
                    ""type"": ""Button"",
                    ""id"": ""699e44a5-b181-499c-b086-1bb39970eaa4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""PlayerAction"",
                    ""type"": ""Button"",
                    ""id"": ""e077ef7c-8bd0-4804-a2d9-96f0e89cee87"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""PlayerDash"",
                    ""type"": ""Button"",
                    ""id"": ""ff88ec45-eaeb-439d-841c-453ba6f95a32"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ReloadScene"",
                    ""type"": ""Button"",
                    ""id"": ""ad21794e-7b57-4a09-9e44-673a0eec6ea7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""ZQSD"",
                    ""id"": ""9415348a-4bfd-4c97-9c66-0a6293d8af36"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMovements"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9d9101eb-9f38-4aae-8c66-ce6745ecbdeb"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PlayerMovements"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a7674b91-de4b-4da8-a46f-52fc4c84a967"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PlayerMovements"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6e918926-0869-4dbc-882c-8a79f04e4e6c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PlayerMovements"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cedb0feb-dc0a-4121-b10f-bc2c200c0c12"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PlayerMovements"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""076a8b10-4f10-49f4-a4e5-b85a476d6d81"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""PlayerMovements"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b33c6f87-e38f-47d4-a1f8-5220bf572cbb"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PlayerInteraction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6dcd6c43-942d-4188-b594-0f1ed881bdfc"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""PlayerInteraction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""207ef1c6-cdea-49e2-8a1d-10e2e9dd0c0a"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PlayerAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ed3ea05-2873-48db-a40b-275f98f73301"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""16310e24-6bf8-4a0c-b4a1-23c1d70e3b5a"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PlayerDash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24c4abba-6abf-4a03-bafe-f692d62b46bb"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""PlayerDash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""64fe3709-6e0f-4792-a15f-e8e82ce1cbc9"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ReloadScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e88f4d4d-3f2d-448a-a677-75c27a30ad76"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ReloadScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Controls
        m_Controls = asset.FindActionMap("Controls", throwIfNotFound: true);
        m_Controls_PlayerMovements = m_Controls.FindAction("PlayerMovements", throwIfNotFound: true);
        m_Controls_PlayerInteraction = m_Controls.FindAction("PlayerInteraction", throwIfNotFound: true);
        m_Controls_PlayerAction = m_Controls.FindAction("PlayerAction", throwIfNotFound: true);
        m_Controls_PlayerDash = m_Controls.FindAction("PlayerDash", throwIfNotFound: true);
        m_Controls_ReloadScene = m_Controls.FindAction("ReloadScene", throwIfNotFound: true);
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

    // Controls
    private readonly InputActionMap m_Controls;
    private IControlsActions m_ControlsActionsCallbackInterface;
    private readonly InputAction m_Controls_PlayerMovements;
    private readonly InputAction m_Controls_PlayerInteraction;
    private readonly InputAction m_Controls_PlayerAction;
    private readonly InputAction m_Controls_PlayerDash;
    private readonly InputAction m_Controls_ReloadScene;
    public struct ControlsActions
    {
        private @Controller m_Wrapper;
        public ControlsActions(@Controller wrapper) { m_Wrapper = wrapper; }
        public InputAction @PlayerMovements => m_Wrapper.m_Controls_PlayerMovements;
        public InputAction @PlayerInteraction => m_Wrapper.m_Controls_PlayerInteraction;
        public InputAction @PlayerAction => m_Wrapper.m_Controls_PlayerAction;
        public InputAction @PlayerDash => m_Wrapper.m_Controls_PlayerDash;
        public InputAction @ReloadScene => m_Wrapper.m_Controls_ReloadScene;
        public InputActionMap Get() { return m_Wrapper.m_Controls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControlsActions set) { return set.Get(); }
        public void SetCallbacks(IControlsActions instance)
        {
            if (m_Wrapper.m_ControlsActionsCallbackInterface != null)
            {
                @PlayerMovements.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPlayerMovements;
                @PlayerMovements.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPlayerMovements;
                @PlayerMovements.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPlayerMovements;
                @PlayerInteraction.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPlayerInteraction;
                @PlayerInteraction.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPlayerInteraction;
                @PlayerInteraction.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPlayerInteraction;
                @PlayerAction.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPlayerAction;
                @PlayerAction.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPlayerAction;
                @PlayerAction.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPlayerAction;
                @PlayerDash.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPlayerDash;
                @PlayerDash.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPlayerDash;
                @PlayerDash.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPlayerDash;
                @ReloadScene.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnReloadScene;
                @ReloadScene.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnReloadScene;
                @ReloadScene.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnReloadScene;
            }
            m_Wrapper.m_ControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PlayerMovements.started += instance.OnPlayerMovements;
                @PlayerMovements.performed += instance.OnPlayerMovements;
                @PlayerMovements.canceled += instance.OnPlayerMovements;
                @PlayerInteraction.started += instance.OnPlayerInteraction;
                @PlayerInteraction.performed += instance.OnPlayerInteraction;
                @PlayerInteraction.canceled += instance.OnPlayerInteraction;
                @PlayerAction.started += instance.OnPlayerAction;
                @PlayerAction.performed += instance.OnPlayerAction;
                @PlayerAction.canceled += instance.OnPlayerAction;
                @PlayerDash.started += instance.OnPlayerDash;
                @PlayerDash.performed += instance.OnPlayerDash;
                @PlayerDash.canceled += instance.OnPlayerDash;
                @ReloadScene.started += instance.OnReloadScene;
                @ReloadScene.performed += instance.OnReloadScene;
                @ReloadScene.canceled += instance.OnReloadScene;
            }
        }
    }
    public ControlsActions @Controls => new ControlsActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IControlsActions
    {
        void OnPlayerMovements(InputAction.CallbackContext context);
        void OnPlayerInteraction(InputAction.CallbackContext context);
        void OnPlayerAction(InputAction.CallbackContext context);
        void OnPlayerDash(InputAction.CallbackContext context);
        void OnReloadScene(InputAction.CallbackContext context);
    }
}
