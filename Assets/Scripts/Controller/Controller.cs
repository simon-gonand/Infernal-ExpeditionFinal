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
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""65cf9322-0f3b-4b5a-95f9-20a64e4b77ea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ValidationUI"",
                    ""type"": ""Button"",
                    ""id"": ""6b1ea310-ac9a-48c0-89a5-323a3e430353"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ResetPlayer"",
                    ""type"": ""Button"",
                    ""id"": ""c32155c7-5d39-4063-b9a0-308662dc4a3f"",
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
                    ""path"": ""<Mouse>/leftButton"",
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
                    ""id"": ""5ff8962f-3023-4549-a3f4-3812159117f0"",
                    ""path"": ""<Gamepad>/leftTrigger"",
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
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Controller"",
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
                    ""id"": ""a3649022-e808-4dee-80a9-d920be414de0"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""PlayerDash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""71dc561e-70d6-4756-80e2-1b0d094e0164"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d526bff9-c08b-4a53-8557-18b4a0142562"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""370ae87f-bcf1-44e5-a517-2011154d5be6"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ValidationUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5bd6a0c0-45dd-4bfe-96ff-c0a93b28ef80"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ValidationUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c7a4624f-91d4-4675-9b5c-3b628cb4b660"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ResetPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ab612fd-8e95-4501-9fbc-aec0fc2b24e0"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ResetPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""ControlsUI"",
            ""id"": ""5b91eee1-e05e-4c03-989f-4a5725978bd1"",
            ""actions"": [
                {
                    ""name"": ""CancelUI"",
                    ""type"": ""Button"",
                    ""id"": ""41385977-5afc-41d0-aedb-34f43913c689"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""MoveUI"",
                    ""type"": ""PassThrough"",
                    ""id"": ""8cdedc71-925d-4485-9600-d51e96172f79"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ValidModifUI"",
                    ""type"": ""Button"",
                    ""id"": ""8348d3e6-93b9-462f-af8b-d4e4429d3a30"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ValidUI"",
                    ""type"": ""Button"",
                    ""id"": ""75dcad06-d410-4eaf-95aa-1cd088c51e9e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""b226cb24-9dc9-4dc5-8135-a8bdc33b91b9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4a8dc73d-0532-4090-afff-7ef2e2817696"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""CancelUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c45315be-6c27-43a9-ab76-285e62ab7d28"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""CancelUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2da24bbd-98f7-4efc-82dd-3645cbb4908c"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""MoveUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""ZQSD"",
                    ""id"": ""fe71f516-fc2e-4d0b-876f-40d4b5a42472"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveUI"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9ee7fca2-815a-46cb-9ec8-848c77383cfb"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""407f23d2-f56c-47a1-b3aa-bf736fe5f264"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f8ba8aca-3657-4da7-ac9c-5cbfceef1b38"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6b7eaf32-57c4-4eb5-88f1-81fd7b90cee0"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""f9d11fd2-f869-448c-8f15-e96ca2af7f97"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveUI"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2ad70acc-6014-4fd8-ab3b-15e7ddfc38ce"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2fcc507b-bdf1-476f-ba06-e2677d3ef49b"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a2329b6b-55c0-490e-abe3-5cb9f8c40b81"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b5634f08-30fb-412d-90f1-2014ea47710f"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Cross"",
                    ""id"": ""09724d04-8cd8-4897-8792-161ffe64a644"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveUI"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b7ecc028-1445-4904-bc8b-12b40ca63f79"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""MoveUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""06324274-8262-4559-a8e9-8a302bcd65c2"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""MoveUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""106d0e6b-60fc-4ab9-85f4-69f1dd403b67"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""MoveUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a361bf95-910d-4ef2-ab04-e35519242876"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""MoveUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""6cdc356c-791c-4a46-b62b-86a21759f016"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ValidModifUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1692ece8-8e6f-4fe6-93c9-5ada9320cf98"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ValidModifUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e62bd09-b619-4fc6-bfcf-e90fcfb7e796"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ValidUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""392c18aa-114a-4d99-95d9-06c2b9d0a297"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ValidUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95e019fa-bc14-4f30-b06b-6aba607e4145"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3179ea38-ed5a-4687-b722-de0166326cf8"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""DebugControls"",
            ""id"": ""c8b7f802-e083-4a44-80be-4c98ce7f04b0"",
            ""actions"": [
                {
                    ""name"": ""ResetPlayer"",
                    ""type"": ""Button"",
                    ""id"": ""54bb8e01-43d6-477f-a02c-30bd755d4fb0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ValidationUI"",
                    ""type"": ""Button"",
                    ""id"": ""e6ff55eb-a4ff-4079-9cb4-1f2f1b4be031"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""8d61a3ac-cb30-4366-ad17-02ae684ebcb8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""PlayerDash"",
                    ""type"": ""Button"",
                    ""id"": ""e1baa49c-cb0f-490a-9ad9-be7d9474c2ff"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""PlayerAction"",
                    ""type"": ""Button"",
                    ""id"": ""a54e2138-eee1-4f29-9d8f-fce5f69dd971"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""PlayerInteraction"",
                    ""type"": ""Button"",
                    ""id"": ""394138dc-7f5c-42eb-9ad0-7bcdef7cc42c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""PlayerMovements"",
                    ""type"": ""PassThrough"",
                    ""id"": ""bfa2b9a8-3d5c-4107-940b-f90e9ebe3b99"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwitchPlayer"",
                    ""type"": ""Button"",
                    ""id"": ""9fcf9d33-9b18-4e2f-b3dd-24c45fce6b68"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ControllAllPlayers"",
                    ""type"": ""Button"",
                    ""id"": ""99e5d163-d071-40a8-9b1d-fe8a7038bd62"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""ZQSD"",
                    ""id"": ""bab0fb6e-ec24-4f49-b4b7-e9a91fcddfb7"",
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
                    ""id"": ""70ccfc84-0802-4baa-b9b4-ca070a9dd2da"",
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
                    ""id"": ""09e93ac2-8f5c-456b-9d43-bfebdb123bd5"",
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
                    ""id"": ""17b422f1-35ce-4b64-9e79-f26ccaeecc41"",
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
                    ""id"": ""0e127533-2c3d-45dc-9b91-50f37e479081"",
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
                    ""id"": ""558a96cd-bde3-4d45-830f-92c41c8c9eb8"",
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
                    ""id"": ""f59b2f13-9cd7-4c51-977d-771b89081a4f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PlayerInteraction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8fb957c4-e663-4bb4-9511-991a6ab33da9"",
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
                    ""id"": ""78ca5295-de46-412d-8f08-b6c9f6d0d698"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""PlayerInteraction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""692bd33b-a086-442d-a909-aec903f669b5"",
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
                    ""id"": ""4d38f8cd-c165-4c1f-90c4-7844e517a392"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""PlayerAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9f4924e9-3c1f-4857-a145-b95b247e1cf7"",
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
                    ""id"": ""c0e9ed6d-5773-45df-86fc-ae0654755f08"",
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
                    ""id"": ""35d6a958-e7bf-4fc0-b886-fa0e67339b49"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""PlayerDash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18a2fa7a-46ff-43eb-99fe-b99cae649c9d"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ee4c8cb-9c4f-48d9-8419-057e738b0095"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b82376c1-d4ab-4541-b4b9-750ba77829d4"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ValidationUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e57ef944-1b68-4e65-b6d2-486f9d07491f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ValidationUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ddae9b1-bb1a-421b-95d2-e6fbdd266089"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ResetPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2d51a389-2d93-4dc2-adb9-4bece2a05b7e"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ResetPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d5ab9ce6-8bf6-4d6f-b279-a4d9eb0b9837"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""SwitchPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""88c0d028-e0e3-4d40-aa6b-6a8f3c9eb5c6"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ControllAllPlayers"",
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
        m_Controls_Pause = m_Controls.FindAction("Pause", throwIfNotFound: true);
        m_Controls_ValidationUI = m_Controls.FindAction("ValidationUI", throwIfNotFound: true);
        m_Controls_ResetPlayer = m_Controls.FindAction("ResetPlayer", throwIfNotFound: true);
        // ControlsUI
        m_ControlsUI = asset.FindActionMap("ControlsUI", throwIfNotFound: true);
        m_ControlsUI_CancelUI = m_ControlsUI.FindAction("CancelUI", throwIfNotFound: true);
        m_ControlsUI_MoveUI = m_ControlsUI.FindAction("MoveUI", throwIfNotFound: true);
        m_ControlsUI_ValidModifUI = m_ControlsUI.FindAction("ValidModifUI", throwIfNotFound: true);
        m_ControlsUI_ValidUI = m_ControlsUI.FindAction("ValidUI", throwIfNotFound: true);
        m_ControlsUI_Pause = m_ControlsUI.FindAction("Pause", throwIfNotFound: true);
        // DebugControls
        m_DebugControls = asset.FindActionMap("DebugControls", throwIfNotFound: true);
        m_DebugControls_ResetPlayer = m_DebugControls.FindAction("ResetPlayer", throwIfNotFound: true);
        m_DebugControls_ValidationUI = m_DebugControls.FindAction("ValidationUI", throwIfNotFound: true);
        m_DebugControls_Pause = m_DebugControls.FindAction("Pause", throwIfNotFound: true);
        m_DebugControls_PlayerDash = m_DebugControls.FindAction("PlayerDash", throwIfNotFound: true);
        m_DebugControls_PlayerAction = m_DebugControls.FindAction("PlayerAction", throwIfNotFound: true);
        m_DebugControls_PlayerInteraction = m_DebugControls.FindAction("PlayerInteraction", throwIfNotFound: true);
        m_DebugControls_PlayerMovements = m_DebugControls.FindAction("PlayerMovements", throwIfNotFound: true);
        m_DebugControls_SwitchPlayer = m_DebugControls.FindAction("SwitchPlayer", throwIfNotFound: true);
        m_DebugControls_ControllAllPlayers = m_DebugControls.FindAction("ControllAllPlayers", throwIfNotFound: true);
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
    private readonly InputAction m_Controls_Pause;
    private readonly InputAction m_Controls_ValidationUI;
    private readonly InputAction m_Controls_ResetPlayer;
    public struct ControlsActions
    {
        private @Controller m_Wrapper;
        public ControlsActions(@Controller wrapper) { m_Wrapper = wrapper; }
        public InputAction @PlayerMovements => m_Wrapper.m_Controls_PlayerMovements;
        public InputAction @PlayerInteraction => m_Wrapper.m_Controls_PlayerInteraction;
        public InputAction @PlayerAction => m_Wrapper.m_Controls_PlayerAction;
        public InputAction @PlayerDash => m_Wrapper.m_Controls_PlayerDash;
        public InputAction @Pause => m_Wrapper.m_Controls_Pause;
        public InputAction @ValidationUI => m_Wrapper.m_Controls_ValidationUI;
        public InputAction @ResetPlayer => m_Wrapper.m_Controls_ResetPlayer;
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
                @Pause.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPause;
                @ValidationUI.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnValidationUI;
                @ValidationUI.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnValidationUI;
                @ValidationUI.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnValidationUI;
                @ResetPlayer.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnResetPlayer;
                @ResetPlayer.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnResetPlayer;
                @ResetPlayer.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnResetPlayer;
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
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @ValidationUI.started += instance.OnValidationUI;
                @ValidationUI.performed += instance.OnValidationUI;
                @ValidationUI.canceled += instance.OnValidationUI;
                @ResetPlayer.started += instance.OnResetPlayer;
                @ResetPlayer.performed += instance.OnResetPlayer;
                @ResetPlayer.canceled += instance.OnResetPlayer;
            }
        }
    }
    public ControlsActions @Controls => new ControlsActions(this);

    // ControlsUI
    private readonly InputActionMap m_ControlsUI;
    private IControlsUIActions m_ControlsUIActionsCallbackInterface;
    private readonly InputAction m_ControlsUI_CancelUI;
    private readonly InputAction m_ControlsUI_MoveUI;
    private readonly InputAction m_ControlsUI_ValidModifUI;
    private readonly InputAction m_ControlsUI_ValidUI;
    private readonly InputAction m_ControlsUI_Pause;
    public struct ControlsUIActions
    {
        private @Controller m_Wrapper;
        public ControlsUIActions(@Controller wrapper) { m_Wrapper = wrapper; }
        public InputAction @CancelUI => m_Wrapper.m_ControlsUI_CancelUI;
        public InputAction @MoveUI => m_Wrapper.m_ControlsUI_MoveUI;
        public InputAction @ValidModifUI => m_Wrapper.m_ControlsUI_ValidModifUI;
        public InputAction @ValidUI => m_Wrapper.m_ControlsUI_ValidUI;
        public InputAction @Pause => m_Wrapper.m_ControlsUI_Pause;
        public InputActionMap Get() { return m_Wrapper.m_ControlsUI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControlsUIActions set) { return set.Get(); }
        public void SetCallbacks(IControlsUIActions instance)
        {
            if (m_Wrapper.m_ControlsUIActionsCallbackInterface != null)
            {
                @CancelUI.started -= m_Wrapper.m_ControlsUIActionsCallbackInterface.OnCancelUI;
                @CancelUI.performed -= m_Wrapper.m_ControlsUIActionsCallbackInterface.OnCancelUI;
                @CancelUI.canceled -= m_Wrapper.m_ControlsUIActionsCallbackInterface.OnCancelUI;
                @MoveUI.started -= m_Wrapper.m_ControlsUIActionsCallbackInterface.OnMoveUI;
                @MoveUI.performed -= m_Wrapper.m_ControlsUIActionsCallbackInterface.OnMoveUI;
                @MoveUI.canceled -= m_Wrapper.m_ControlsUIActionsCallbackInterface.OnMoveUI;
                @ValidModifUI.started -= m_Wrapper.m_ControlsUIActionsCallbackInterface.OnValidModifUI;
                @ValidModifUI.performed -= m_Wrapper.m_ControlsUIActionsCallbackInterface.OnValidModifUI;
                @ValidModifUI.canceled -= m_Wrapper.m_ControlsUIActionsCallbackInterface.OnValidModifUI;
                @ValidUI.started -= m_Wrapper.m_ControlsUIActionsCallbackInterface.OnValidUI;
                @ValidUI.performed -= m_Wrapper.m_ControlsUIActionsCallbackInterface.OnValidUI;
                @ValidUI.canceled -= m_Wrapper.m_ControlsUIActionsCallbackInterface.OnValidUI;
                @Pause.started -= m_Wrapper.m_ControlsUIActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_ControlsUIActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_ControlsUIActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_ControlsUIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @CancelUI.started += instance.OnCancelUI;
                @CancelUI.performed += instance.OnCancelUI;
                @CancelUI.canceled += instance.OnCancelUI;
                @MoveUI.started += instance.OnMoveUI;
                @MoveUI.performed += instance.OnMoveUI;
                @MoveUI.canceled += instance.OnMoveUI;
                @ValidModifUI.started += instance.OnValidModifUI;
                @ValidModifUI.performed += instance.OnValidModifUI;
                @ValidModifUI.canceled += instance.OnValidModifUI;
                @ValidUI.started += instance.OnValidUI;
                @ValidUI.performed += instance.OnValidUI;
                @ValidUI.canceled += instance.OnValidUI;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public ControlsUIActions @ControlsUI => new ControlsUIActions(this);

    // DebugControls
    private readonly InputActionMap m_DebugControls;
    private IDebugControlsActions m_DebugControlsActionsCallbackInterface;
    private readonly InputAction m_DebugControls_ResetPlayer;
    private readonly InputAction m_DebugControls_ValidationUI;
    private readonly InputAction m_DebugControls_Pause;
    private readonly InputAction m_DebugControls_PlayerDash;
    private readonly InputAction m_DebugControls_PlayerAction;
    private readonly InputAction m_DebugControls_PlayerInteraction;
    private readonly InputAction m_DebugControls_PlayerMovements;
    private readonly InputAction m_DebugControls_SwitchPlayer;
    private readonly InputAction m_DebugControls_ControllAllPlayers;
    public struct DebugControlsActions
    {
        private @Controller m_Wrapper;
        public DebugControlsActions(@Controller wrapper) { m_Wrapper = wrapper; }
        public InputAction @ResetPlayer => m_Wrapper.m_DebugControls_ResetPlayer;
        public InputAction @ValidationUI => m_Wrapper.m_DebugControls_ValidationUI;
        public InputAction @Pause => m_Wrapper.m_DebugControls_Pause;
        public InputAction @PlayerDash => m_Wrapper.m_DebugControls_PlayerDash;
        public InputAction @PlayerAction => m_Wrapper.m_DebugControls_PlayerAction;
        public InputAction @PlayerInteraction => m_Wrapper.m_DebugControls_PlayerInteraction;
        public InputAction @PlayerMovements => m_Wrapper.m_DebugControls_PlayerMovements;
        public InputAction @SwitchPlayer => m_Wrapper.m_DebugControls_SwitchPlayer;
        public InputAction @ControllAllPlayers => m_Wrapper.m_DebugControls_ControllAllPlayers;
        public InputActionMap Get() { return m_Wrapper.m_DebugControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DebugControlsActions set) { return set.Get(); }
        public void SetCallbacks(IDebugControlsActions instance)
        {
            if (m_Wrapper.m_DebugControlsActionsCallbackInterface != null)
            {
                @ResetPlayer.started -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnResetPlayer;
                @ResetPlayer.performed -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnResetPlayer;
                @ResetPlayer.canceled -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnResetPlayer;
                @ValidationUI.started -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnValidationUI;
                @ValidationUI.performed -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnValidationUI;
                @ValidationUI.canceled -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnValidationUI;
                @Pause.started -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnPause;
                @PlayerDash.started -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnPlayerDash;
                @PlayerDash.performed -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnPlayerDash;
                @PlayerDash.canceled -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnPlayerDash;
                @PlayerAction.started -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnPlayerAction;
                @PlayerAction.performed -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnPlayerAction;
                @PlayerAction.canceled -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnPlayerAction;
                @PlayerInteraction.started -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnPlayerInteraction;
                @PlayerInteraction.performed -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnPlayerInteraction;
                @PlayerInteraction.canceled -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnPlayerInteraction;
                @PlayerMovements.started -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnPlayerMovements;
                @PlayerMovements.performed -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnPlayerMovements;
                @PlayerMovements.canceled -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnPlayerMovements;
                @SwitchPlayer.started -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnSwitchPlayer;
                @SwitchPlayer.performed -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnSwitchPlayer;
                @SwitchPlayer.canceled -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnSwitchPlayer;
                @ControllAllPlayers.started -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnControllAllPlayers;
                @ControllAllPlayers.performed -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnControllAllPlayers;
                @ControllAllPlayers.canceled -= m_Wrapper.m_DebugControlsActionsCallbackInterface.OnControllAllPlayers;
            }
            m_Wrapper.m_DebugControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ResetPlayer.started += instance.OnResetPlayer;
                @ResetPlayer.performed += instance.OnResetPlayer;
                @ResetPlayer.canceled += instance.OnResetPlayer;
                @ValidationUI.started += instance.OnValidationUI;
                @ValidationUI.performed += instance.OnValidationUI;
                @ValidationUI.canceled += instance.OnValidationUI;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @PlayerDash.started += instance.OnPlayerDash;
                @PlayerDash.performed += instance.OnPlayerDash;
                @PlayerDash.canceled += instance.OnPlayerDash;
                @PlayerAction.started += instance.OnPlayerAction;
                @PlayerAction.performed += instance.OnPlayerAction;
                @PlayerAction.canceled += instance.OnPlayerAction;
                @PlayerInteraction.started += instance.OnPlayerInteraction;
                @PlayerInteraction.performed += instance.OnPlayerInteraction;
                @PlayerInteraction.canceled += instance.OnPlayerInteraction;
                @PlayerMovements.started += instance.OnPlayerMovements;
                @PlayerMovements.performed += instance.OnPlayerMovements;
                @PlayerMovements.canceled += instance.OnPlayerMovements;
                @SwitchPlayer.started += instance.OnSwitchPlayer;
                @SwitchPlayer.performed += instance.OnSwitchPlayer;
                @SwitchPlayer.canceled += instance.OnSwitchPlayer;
                @ControllAllPlayers.started += instance.OnControllAllPlayers;
                @ControllAllPlayers.performed += instance.OnControllAllPlayers;
                @ControllAllPlayers.canceled += instance.OnControllAllPlayers;
            }
        }
    }
    public DebugControlsActions @DebugControls => new DebugControlsActions(this);
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
        void OnPause(InputAction.CallbackContext context);
        void OnValidationUI(InputAction.CallbackContext context);
        void OnResetPlayer(InputAction.CallbackContext context);
    }
    public interface IControlsUIActions
    {
        void OnCancelUI(InputAction.CallbackContext context);
        void OnMoveUI(InputAction.CallbackContext context);
        void OnValidModifUI(InputAction.CallbackContext context);
        void OnValidUI(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
    public interface IDebugControlsActions
    {
        void OnResetPlayer(InputAction.CallbackContext context);
        void OnValidationUI(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnPlayerDash(InputAction.CallbackContext context);
        void OnPlayerAction(InputAction.CallbackContext context);
        void OnPlayerInteraction(InputAction.CallbackContext context);
        void OnPlayerMovements(InputAction.CallbackContext context);
        void OnSwitchPlayer(InputAction.CallbackContext context);
        void OnControllAllPlayers(InputAction.CallbackContext context);
    }
}
