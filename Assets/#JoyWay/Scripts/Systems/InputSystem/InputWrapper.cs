// GENERATED AUTOMATICALLY FROM 'Assets/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputWrapper : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputWrapper()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""DeviceBase"",
            ""id"": ""95800ded-e9f7-4a63-8b62-0dcbac136a18"",
            ""actions"": [
                {
                    ""name"": ""RightControllerPose"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e74663f9-b60c-445e-a797-8289afd2b9e2"",
                    ""expectedControlType"": ""Pose"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftControllerPose"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a3bd9802-6d03-4fc1-b15a-7c7217c501a5"",
                    ""expectedControlType"": ""Pose"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftTriggerAxis"",
                    ""type"": ""Value"",
                    ""id"": ""47db7caf-ab3e-49b2-bbef-f1a87975cc26"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftGripAxis"",
                    ""type"": ""Value"",
                    ""id"": ""b1eaef47-a971-4511-b0e7-32f960e119d6"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightTriggerAxis"",
                    ""type"": ""Value"",
                    ""id"": ""50a80c07-3671-4f55-b39f-d749f34b1891"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightGripAxis"",
                    ""type"": ""Value"",
                    ""id"": ""82e0fe25-c5dc-4203-b446-6d3e22a7d294"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4f4fa25e-42c8-48b6-9608-9d82bde607c4"",
                    ""path"": ""<XRController>{RightHand}/devicePose"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""RightControllerPose"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a9292ddb-b9cc-4ae7-9d7a-c4cbebbb679b"",
                    ""path"": ""<XRController>{LeftHand}/devicePose"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""LeftControllerPose"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""61310923-2024-4fc9-a428-fe4b757f2236"",
                    ""path"": ""<XRController>{LeftHand}/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""LeftTriggerAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf22d2b0-2106-4ed9-b1d7-a501dbce0b9d"",
                    ""path"": ""<XRController>{RightHand}/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""RightTriggerAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0bd4a09a-f608-4c61-9873-d078985eeceb"",
                    ""path"": ""<XRController>{LeftHand}/grip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""LeftGripAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f01a7d61-12c5-4164-96f0-2937da115af4"",
                    ""path"": ""<XRController>{RightHand}/grip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""RightGripAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MainInteractions"",
            ""id"": ""4800ff3e-20e4-442a-a0c1-e4d4b1bb0acb"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""8edb37c8-d7a9-4630-be6d-31a7d240bb79"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5ddd4571-096d-4d45-a2d3-7e9ad2838734"",
                    ""path"": ""<XRController>{LeftHand}/secondaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerControls"",
            ""id"": ""9c261ae1-060e-4528-95e0-c307da10dcf7"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""a0efe326-1e98-4d14-ab3d-b04788d74d6a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""9e212067-9712-49aa-a530-a4424f24e11a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""b28744fc-f614-4e37-a079-b48cce2c24b0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""41332310-ff46-440e-990f-ffa4930ae7db"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""00ca640b-d935-4593-8157-c05846ea39b3"",
                    ""path"": ""Dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e2062cb9-1b15-46a2-838c-2f8d72a0bdd9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8180e8bd-4097-4f4e-ab88-4523101a6ce9"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""320bffee-a40b-4347-ac70-c210eb8bc73a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""1c5327b5-f71c-4f60-99c7-4e737386f1d1"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d2581a9b-1d11-4566-b27d-b92aff5fabbc"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2e46982e-44cc-431b-9f0b-c11910bf467a"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fcfe95b8-67b9-4526-84b5-5d0bc98d6400"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""77bff152-3580-4b21-b6de-dcd0c7e41164"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1635d3fe-58b6-4ba9-a4e2-f4b964f6b5c8"",
                    ""path"": ""<XRController>{LeftHand}/thumbstick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""XR"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""448bb89d-03f0-49ed-a280-a72775fc1093"",
                    ""path"": ""<ViveController>{LeftHand}/trackpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""05f6913d-c316-48b2-a6bb-e225f14c7960"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8255d333-5683-4943-a58a-ccb207ff1dce"",
                    ""path"": ""<XRController>{LeftHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cadffd6f-c3c4-4eef-b5d5-3c6333a7f9c2"",
                    ""path"": ""<XRController>{RightHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""973f6c95-1ca8-4281-b199-01fab789029e"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c37e2dd-5a04-4f95-9572-add940446151"",
                    ""path"": ""<OculusTouchController>{RightHand}/thumbstick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""b1d396dc-73d3-40ad-a40a-96732503e18a"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Negative"",
                    ""id"": ""9c01db8f-d1bf-41dc-9291-0c4dee39c0a8"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Positive"",
                    ""id"": ""1c032c28-98ce-4988-be1f-de7aec52f0b8"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f8bb650d-2305-4833-9059-54d6f6bd0052"",
                    ""path"": ""<ValveIndexController>{RightHand}/thumbstick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aed5d14a-3099-45a2-8ea0-02cc810766e0"",
                    ""path"": ""<ViveController>{RightHand}/trackpad/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Utility"",
            ""id"": ""42f85d58-6a4f-4d6b-a22d-ee808bc0da8a"",
            ""actions"": [
                {
                    ""name"": ""ItemActivation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1e55d6eb-e11b-4201-90c0-3f6d91d5e60c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GemEquip"",
                    ""type"": ""PassThrough"",
                    ""id"": ""bfd743ba-ad25-4f06-ba2f-556cee62e039"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TelekinesisApplyGemEffect"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9a21640d-ff1c-4469-b208-a6e8b90364af"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e29397dc-eb2c-4d24-8a76-e003b678a41d"",
                    ""path"": ""<XRController>{RightHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""ItemActivation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c7901e7e-a067-4a04-a302-4a7d76439486"",
                    ""path"": ""<XRController>{LeftHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""ItemActivation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e4765d84-1155-46b2-a0de-e416c1e53fe6"",
                    ""path"": ""<XRController>{LeftHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TelekinesisApplyGemEffect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5df43fda-34ec-4494-a1c8-7b47d3d059ca"",
                    ""path"": ""<XRController>{RightHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TelekinesisApplyGemEffect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6952d3c6-35aa-4c24-8a55-c4e933e21d22"",
                    ""path"": ""<XRController>{RightHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""GemEquip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""936c7c3c-785f-4ae9-b925-21e1585dd8b2"",
                    ""path"": ""<XRController>{LeftHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""GemEquip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Abilities"",
            ""id"": ""c62875f1-1e7c-444d-81e0-fbd964e8b554"",
            ""actions"": [
                {
                    ""name"": ""Primary"",
                    ""type"": ""PassThrough"",
                    ""id"": ""76f7fcdc-a322-4971-b0c0-ef7344e10a14"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Secondary"",
                    ""type"": ""PassThrough"",
                    ""id"": ""bfd20297-3eeb-4f5d-819d-f7604392e382"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Grab"",
                    ""type"": ""PassThrough"",
                    ""id"": ""58ed48fb-713a-45e2-b1ab-2158fcb05de7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Telekinesis"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9ff589b8-09f9-40f6-96fb-d801b46ec40f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""PassThrough"",
                    ""id"": ""69e6abf5-2fc4-4ee0-9b8f-e15dba9fb950"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9b3256f0-054a-4268-8190-20e67813df56"",
                    ""path"": ""<XRController>{LeftHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Primary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e8c3eca-b08c-4311-9f57-ff46d2d07897"",
                    ""path"": ""<XRController>{RightHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Primary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b0f3f3ea-62c0-4b6c-9e9d-e11dcd3e4034"",
                    ""path"": ""<XRController>{LeftHand}/gripPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Secondary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""546e030f-d340-4cfa-87b7-1db03314fbc2"",
                    ""path"": ""<XRController>{RightHand}/gripPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Secondary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ef16cfe4-87ec-4d6c-8e83-74c6ae5a1da8"",
                    ""path"": ""<XRController>{LeftHand}/primaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""259bf406-fd19-4a82-b53d-0942416472bb"",
                    ""path"": ""<XRController>{RightHand}/primaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f8999214-833e-4fad-9f09-a0e56dd2cb83"",
                    ""path"": ""<XRController>{RightHand}/gripPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Grab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08ef3264-449a-406e-b8ed-cd2234d2824d"",
                    ""path"": ""<XRController>{LeftHand}/gripPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Grab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""879b32f3-47de-450f-b53b-b107c4730165"",
                    ""path"": ""<XRController>{LeftHand}/gripPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Telekinesis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96a6c41a-fd92-4672-869e-a650cb33fe42"",
                    ""path"": ""<XRController>{RightHand}/gripPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Telekinesis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MenuInteractions"",
            ""id"": ""803f4d70-0ed9-49b3-87d3-a34630580cfb"",
            ""actions"": [
                {
                    ""name"": ""UIClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""54a3f4d7-f50c-4996-bdc5-1ad33ee9d914"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UIScroll"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7665db49-486f-4fba-a1a2-02734d5e40de"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b89b3a6a-6ecf-4f4c-975d-d1295673bc3a"",
                    ""path"": ""<XRController>{LeftHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""UIClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a874ddb-840b-4609-b18e-7b38d1b60340"",
                    ""path"": ""<XRController>{RightHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""UIClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a0bab5a-2abe-431a-8a3e-dc2f722ab538"",
                    ""path"": ""<OculusTouchController>{RightHand}/thumbstick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""UIScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""44e17aaa-a3e5-40af-8edf-f00f0bcbc538"",
                    ""path"": ""<OculusTouchController>{LeftHand}/thumbstick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""UIScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""ThirdPersonSwitcher"",
            ""id"": ""9c08a89f-7ed9-4cf9-94a4-196174dbcc0d"",
            ""actions"": [
                {
                    ""name"": ""ThirdPerson"",
                    ""type"": ""Button"",
                    ""id"": ""5ad24776-e6de-4765-bcfc-cf513cf8ca61"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FirstPerson"",
                    ""type"": ""Button"",
                    ""id"": ""b70462ac-413d-4fa9-8446-fc8325c7e15f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8267436f-cd99-468f-81d4-9536469d71de"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ThirdPerson"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be828f0e-529b-4cc6-a4a2-a3d44166f831"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""FirstPerson"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""CaptureCamera"",
            ""id"": ""ad6301bb-02f7-4f10-b8ed-bc23106d467f"",
            ""actions"": [
                {
                    ""name"": ""Beautify"",
                    ""type"": ""Button"",
                    ""id"": ""56dd41ef-4baa-4435-a5ff-8d93d241c2cd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Unbeautify"",
                    ""type"": ""Button"",
                    ""id"": ""e085a490-76f2-4635-90b3-ee15b5bed7f7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""32129909-ff7b-4f04-bd9f-260bff1b5232"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Beautify"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f8ed41f3-ebea-4ee7-ae02-428b95472a51"",
                    ""path"": ""<Keyboard>/0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Unbeautify"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""DebugKeys"",
            ""id"": ""3b3028fb-3241-49eb-b7b4-9ae49550e010"",
            ""actions"": [
                {
                    ""name"": ""Opty"",
                    ""type"": ""Button"",
                    ""id"": ""d6fbb58b-ceb8-49ab-8b1e-5b45b373a096"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LoadHub"",
                    ""type"": ""Button"",
                    ""id"": ""12c878fa-0aba-4e1b-b2d5-e436aadaf3ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleDamageNumbers"",
                    ""type"": ""Button"",
                    ""id"": ""534a8168-351d-493b-8932-06690ac1b338"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f0552ce6-116e-4ede-a279-f148b63c3d3f"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Opty"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e66796a5-0b11-40aa-9b91-2410b842b440"",
                    ""path"": ""<Keyboard>/h"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""LoadHub"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""875fb7e5-391c-48fb-b722-a3d2ac846880"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ToggleDamageNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR"",
            ""bindingGroup"": ""XR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // DeviceBase
        m_DeviceBase = asset.FindActionMap("DeviceBase", throwIfNotFound: true);
        m_DeviceBase_RightControllerPose = m_DeviceBase.FindAction("RightControllerPose", throwIfNotFound: true);
        m_DeviceBase_LeftControllerPose = m_DeviceBase.FindAction("LeftControllerPose", throwIfNotFound: true);
        m_DeviceBase_LeftTriggerAxis = m_DeviceBase.FindAction("LeftTriggerAxis", throwIfNotFound: true);
        m_DeviceBase_LeftGripAxis = m_DeviceBase.FindAction("LeftGripAxis", throwIfNotFound: true);
        m_DeviceBase_RightTriggerAxis = m_DeviceBase.FindAction("RightTriggerAxis", throwIfNotFound: true);
        m_DeviceBase_RightGripAxis = m_DeviceBase.FindAction("RightGripAxis", throwIfNotFound: true);
        // MainInteractions
        m_MainInteractions = asset.FindActionMap("MainInteractions", throwIfNotFound: true);
        m_MainInteractions_Pause = m_MainInteractions.FindAction("Pause", throwIfNotFound: true);
        // PlayerControls
        m_PlayerControls = asset.FindActionMap("PlayerControls", throwIfNotFound: true);
        m_PlayerControls_Move = m_PlayerControls.FindAction("Move", throwIfNotFound: true);
        m_PlayerControls_Fire = m_PlayerControls.FindAction("Fire", throwIfNotFound: true);
        m_PlayerControls_Jump = m_PlayerControls.FindAction("Jump", throwIfNotFound: true);
        m_PlayerControls_Rotate = m_PlayerControls.FindAction("Rotate", throwIfNotFound: true);
        // Utility
        m_Utility = asset.FindActionMap("Utility", throwIfNotFound: true);
        m_Utility_ItemActivation = m_Utility.FindAction("ItemActivation", throwIfNotFound: true);
        m_Utility_GemEquip = m_Utility.FindAction("GemEquip", throwIfNotFound: true);
        m_Utility_TelekinesisApplyGemEffect = m_Utility.FindAction("TelekinesisApplyGemEffect", throwIfNotFound: true);
        // Abilities
        m_Abilities = asset.FindActionMap("Abilities", throwIfNotFound: true);
        m_Abilities_Primary = m_Abilities.FindAction("Primary", throwIfNotFound: true);
        m_Abilities_Secondary = m_Abilities.FindAction("Secondary", throwIfNotFound: true);
        m_Abilities_Grab = m_Abilities.FindAction("Grab", throwIfNotFound: true);
        m_Abilities_Telekinesis = m_Abilities.FindAction("Telekinesis", throwIfNotFound: true);
        m_Abilities_Dash = m_Abilities.FindAction("Dash", throwIfNotFound: true);
        // MenuInteractions
        m_MenuInteractions = asset.FindActionMap("MenuInteractions", throwIfNotFound: true);
        m_MenuInteractions_UIClick = m_MenuInteractions.FindAction("UIClick", throwIfNotFound: true);
        m_MenuInteractions_UIScroll = m_MenuInteractions.FindAction("UIScroll", throwIfNotFound: true);
        // ThirdPersonSwitcher
        m_ThirdPersonSwitcher = asset.FindActionMap("ThirdPersonSwitcher", throwIfNotFound: true);
        m_ThirdPersonSwitcher_ThirdPerson = m_ThirdPersonSwitcher.FindAction("ThirdPerson", throwIfNotFound: true);
        m_ThirdPersonSwitcher_FirstPerson = m_ThirdPersonSwitcher.FindAction("FirstPerson", throwIfNotFound: true);
        // CaptureCamera
        m_CaptureCamera = asset.FindActionMap("CaptureCamera", throwIfNotFound: true);
        m_CaptureCamera_Beautify = m_CaptureCamera.FindAction("Beautify", throwIfNotFound: true);
        m_CaptureCamera_Unbeautify = m_CaptureCamera.FindAction("Unbeautify", throwIfNotFound: true);
        // DebugKeys
        m_DebugKeys = asset.FindActionMap("DebugKeys", throwIfNotFound: true);
        m_DebugKeys_Opty = m_DebugKeys.FindAction("Opty", throwIfNotFound: true);
        m_DebugKeys_LoadHub = m_DebugKeys.FindAction("LoadHub", throwIfNotFound: true);
        m_DebugKeys_ToggleDamageNumbers = m_DebugKeys.FindAction("ToggleDamageNumbers", throwIfNotFound: true);
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

    // DeviceBase
    private readonly InputActionMap m_DeviceBase;
    private IDeviceBaseActions m_DeviceBaseActionsCallbackInterface;
    private readonly InputAction m_DeviceBase_RightControllerPose;
    private readonly InputAction m_DeviceBase_LeftControllerPose;
    private readonly InputAction m_DeviceBase_LeftTriggerAxis;
    private readonly InputAction m_DeviceBase_LeftGripAxis;
    private readonly InputAction m_DeviceBase_RightTriggerAxis;
    private readonly InputAction m_DeviceBase_RightGripAxis;
    public struct DeviceBaseActions
    {
        private @InputWrapper m_Wrapper;
        public DeviceBaseActions(@InputWrapper wrapper) { m_Wrapper = wrapper; }
        public InputAction @RightControllerPose => m_Wrapper.m_DeviceBase_RightControllerPose;
        public InputAction @LeftControllerPose => m_Wrapper.m_DeviceBase_LeftControllerPose;
        public InputAction @LeftTriggerAxis => m_Wrapper.m_DeviceBase_LeftTriggerAxis;
        public InputAction @LeftGripAxis => m_Wrapper.m_DeviceBase_LeftGripAxis;
        public InputAction @RightTriggerAxis => m_Wrapper.m_DeviceBase_RightTriggerAxis;
        public InputAction @RightGripAxis => m_Wrapper.m_DeviceBase_RightGripAxis;
        public InputActionMap Get() { return m_Wrapper.m_DeviceBase; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DeviceBaseActions set) { return set.Get(); }
        public void SetCallbacks(IDeviceBaseActions instance)
        {
            if (m_Wrapper.m_DeviceBaseActionsCallbackInterface != null)
            {
                @RightControllerPose.started -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnRightControllerPose;
                @RightControllerPose.performed -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnRightControllerPose;
                @RightControllerPose.canceled -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnRightControllerPose;
                @LeftControllerPose.started -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnLeftControllerPose;
                @LeftControllerPose.performed -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnLeftControllerPose;
                @LeftControllerPose.canceled -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnLeftControllerPose;
                @LeftTriggerAxis.started -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnLeftTriggerAxis;
                @LeftTriggerAxis.performed -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnLeftTriggerAxis;
                @LeftTriggerAxis.canceled -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnLeftTriggerAxis;
                @LeftGripAxis.started -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnLeftGripAxis;
                @LeftGripAxis.performed -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnLeftGripAxis;
                @LeftGripAxis.canceled -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnLeftGripAxis;
                @RightTriggerAxis.started -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnRightTriggerAxis;
                @RightTriggerAxis.performed -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnRightTriggerAxis;
                @RightTriggerAxis.canceled -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnRightTriggerAxis;
                @RightGripAxis.started -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnRightGripAxis;
                @RightGripAxis.performed -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnRightGripAxis;
                @RightGripAxis.canceled -= m_Wrapper.m_DeviceBaseActionsCallbackInterface.OnRightGripAxis;
            }
            m_Wrapper.m_DeviceBaseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @RightControllerPose.started += instance.OnRightControllerPose;
                @RightControllerPose.performed += instance.OnRightControllerPose;
                @RightControllerPose.canceled += instance.OnRightControllerPose;
                @LeftControllerPose.started += instance.OnLeftControllerPose;
                @LeftControllerPose.performed += instance.OnLeftControllerPose;
                @LeftControllerPose.canceled += instance.OnLeftControllerPose;
                @LeftTriggerAxis.started += instance.OnLeftTriggerAxis;
                @LeftTriggerAxis.performed += instance.OnLeftTriggerAxis;
                @LeftTriggerAxis.canceled += instance.OnLeftTriggerAxis;
                @LeftGripAxis.started += instance.OnLeftGripAxis;
                @LeftGripAxis.performed += instance.OnLeftGripAxis;
                @LeftGripAxis.canceled += instance.OnLeftGripAxis;
                @RightTriggerAxis.started += instance.OnRightTriggerAxis;
                @RightTriggerAxis.performed += instance.OnRightTriggerAxis;
                @RightTriggerAxis.canceled += instance.OnRightTriggerAxis;
                @RightGripAxis.started += instance.OnRightGripAxis;
                @RightGripAxis.performed += instance.OnRightGripAxis;
                @RightGripAxis.canceled += instance.OnRightGripAxis;
            }
        }
    }
    public DeviceBaseActions @DeviceBase => new DeviceBaseActions(this);

    // MainInteractions
    private readonly InputActionMap m_MainInteractions;
    private IMainInteractionsActions m_MainInteractionsActionsCallbackInterface;
    private readonly InputAction m_MainInteractions_Pause;
    public struct MainInteractionsActions
    {
        private @InputWrapper m_Wrapper;
        public MainInteractionsActions(@InputWrapper wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_MainInteractions_Pause;
        public InputActionMap Get() { return m_Wrapper.m_MainInteractions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainInteractionsActions set) { return set.Get(); }
        public void SetCallbacks(IMainInteractionsActions instance)
        {
            if (m_Wrapper.m_MainInteractionsActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_MainInteractionsActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_MainInteractionsActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_MainInteractionsActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_MainInteractionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public MainInteractionsActions @MainInteractions => new MainInteractionsActions(this);

    // PlayerControls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_Move;
    private readonly InputAction m_PlayerControls_Fire;
    private readonly InputAction m_PlayerControls_Jump;
    private readonly InputAction m_PlayerControls_Rotate;
    public struct PlayerControlsActions
    {
        private @InputWrapper m_Wrapper;
        public PlayerControlsActions(@InputWrapper wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerControls_Move;
        public InputAction @Fire => m_Wrapper.m_PlayerControls_Fire;
        public InputAction @Jump => m_Wrapper.m_PlayerControls_Jump;
        public InputAction @Rotate => m_Wrapper.m_PlayerControls_Rotate;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Fire.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnFire;
                @Jump.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                @Rotate.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRotate;
            }
            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);

    // Utility
    private readonly InputActionMap m_Utility;
    private IUtilityActions m_UtilityActionsCallbackInterface;
    private readonly InputAction m_Utility_ItemActivation;
    private readonly InputAction m_Utility_GemEquip;
    private readonly InputAction m_Utility_TelekinesisApplyGemEffect;
    public struct UtilityActions
    {
        private @InputWrapper m_Wrapper;
        public UtilityActions(@InputWrapper wrapper) { m_Wrapper = wrapper; }
        public InputAction @ItemActivation => m_Wrapper.m_Utility_ItemActivation;
        public InputAction @GemEquip => m_Wrapper.m_Utility_GemEquip;
        public InputAction @TelekinesisApplyGemEffect => m_Wrapper.m_Utility_TelekinesisApplyGemEffect;
        public InputActionMap Get() { return m_Wrapper.m_Utility; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UtilityActions set) { return set.Get(); }
        public void SetCallbacks(IUtilityActions instance)
        {
            if (m_Wrapper.m_UtilityActionsCallbackInterface != null)
            {
                @ItemActivation.started -= m_Wrapper.m_UtilityActionsCallbackInterface.OnItemActivation;
                @ItemActivation.performed -= m_Wrapper.m_UtilityActionsCallbackInterface.OnItemActivation;
                @ItemActivation.canceled -= m_Wrapper.m_UtilityActionsCallbackInterface.OnItemActivation;
                @GemEquip.started -= m_Wrapper.m_UtilityActionsCallbackInterface.OnGemEquip;
                @GemEquip.performed -= m_Wrapper.m_UtilityActionsCallbackInterface.OnGemEquip;
                @GemEquip.canceled -= m_Wrapper.m_UtilityActionsCallbackInterface.OnGemEquip;
                @TelekinesisApplyGemEffect.started -= m_Wrapper.m_UtilityActionsCallbackInterface.OnTelekinesisApplyGemEffect;
                @TelekinesisApplyGemEffect.performed -= m_Wrapper.m_UtilityActionsCallbackInterface.OnTelekinesisApplyGemEffect;
                @TelekinesisApplyGemEffect.canceled -= m_Wrapper.m_UtilityActionsCallbackInterface.OnTelekinesisApplyGemEffect;
            }
            m_Wrapper.m_UtilityActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ItemActivation.started += instance.OnItemActivation;
                @ItemActivation.performed += instance.OnItemActivation;
                @ItemActivation.canceled += instance.OnItemActivation;
                @GemEquip.started += instance.OnGemEquip;
                @GemEquip.performed += instance.OnGemEquip;
                @GemEquip.canceled += instance.OnGemEquip;
                @TelekinesisApplyGemEffect.started += instance.OnTelekinesisApplyGemEffect;
                @TelekinesisApplyGemEffect.performed += instance.OnTelekinesisApplyGemEffect;
                @TelekinesisApplyGemEffect.canceled += instance.OnTelekinesisApplyGemEffect;
            }
        }
    }
    public UtilityActions @Utility => new UtilityActions(this);

    // Abilities
    private readonly InputActionMap m_Abilities;
    private IAbilitiesActions m_AbilitiesActionsCallbackInterface;
    private readonly InputAction m_Abilities_Primary;
    private readonly InputAction m_Abilities_Secondary;
    private readonly InputAction m_Abilities_Grab;
    private readonly InputAction m_Abilities_Telekinesis;
    private readonly InputAction m_Abilities_Dash;
    public struct AbilitiesActions
    {
        private @InputWrapper m_Wrapper;
        public AbilitiesActions(@InputWrapper wrapper) { m_Wrapper = wrapper; }
        public InputAction @Primary => m_Wrapper.m_Abilities_Primary;
        public InputAction @Secondary => m_Wrapper.m_Abilities_Secondary;
        public InputAction @Grab => m_Wrapper.m_Abilities_Grab;
        public InputAction @Telekinesis => m_Wrapper.m_Abilities_Telekinesis;
        public InputAction @Dash => m_Wrapper.m_Abilities_Dash;
        public InputActionMap Get() { return m_Wrapper.m_Abilities; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AbilitiesActions set) { return set.Get(); }
        public void SetCallbacks(IAbilitiesActions instance)
        {
            if (m_Wrapper.m_AbilitiesActionsCallbackInterface != null)
            {
                @Primary.started -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnPrimary;
                @Primary.performed -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnPrimary;
                @Primary.canceled -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnPrimary;
                @Secondary.started -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnSecondary;
                @Secondary.performed -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnSecondary;
                @Secondary.canceled -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnSecondary;
                @Grab.started -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnGrab;
                @Grab.performed -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnGrab;
                @Grab.canceled -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnGrab;
                @Telekinesis.started -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnTelekinesis;
                @Telekinesis.performed -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnTelekinesis;
                @Telekinesis.canceled -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnTelekinesis;
                @Dash.started -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnDash;
            }
            m_Wrapper.m_AbilitiesActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Primary.started += instance.OnPrimary;
                @Primary.performed += instance.OnPrimary;
                @Primary.canceled += instance.OnPrimary;
                @Secondary.started += instance.OnSecondary;
                @Secondary.performed += instance.OnSecondary;
                @Secondary.canceled += instance.OnSecondary;
                @Grab.started += instance.OnGrab;
                @Grab.performed += instance.OnGrab;
                @Grab.canceled += instance.OnGrab;
                @Telekinesis.started += instance.OnTelekinesis;
                @Telekinesis.performed += instance.OnTelekinesis;
                @Telekinesis.canceled += instance.OnTelekinesis;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
            }
        }
    }
    public AbilitiesActions @Abilities => new AbilitiesActions(this);

    // MenuInteractions
    private readonly InputActionMap m_MenuInteractions;
    private IMenuInteractionsActions m_MenuInteractionsActionsCallbackInterface;
    private readonly InputAction m_MenuInteractions_UIClick;
    private readonly InputAction m_MenuInteractions_UIScroll;
    public struct MenuInteractionsActions
    {
        private @InputWrapper m_Wrapper;
        public MenuInteractionsActions(@InputWrapper wrapper) { m_Wrapper = wrapper; }
        public InputAction @UIClick => m_Wrapper.m_MenuInteractions_UIClick;
        public InputAction @UIScroll => m_Wrapper.m_MenuInteractions_UIScroll;
        public InputActionMap Get() { return m_Wrapper.m_MenuInteractions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuInteractionsActions set) { return set.Get(); }
        public void SetCallbacks(IMenuInteractionsActions instance)
        {
            if (m_Wrapper.m_MenuInteractionsActionsCallbackInterface != null)
            {
                @UIClick.started -= m_Wrapper.m_MenuInteractionsActionsCallbackInterface.OnUIClick;
                @UIClick.performed -= m_Wrapper.m_MenuInteractionsActionsCallbackInterface.OnUIClick;
                @UIClick.canceled -= m_Wrapper.m_MenuInteractionsActionsCallbackInterface.OnUIClick;
                @UIScroll.started -= m_Wrapper.m_MenuInteractionsActionsCallbackInterface.OnUIScroll;
                @UIScroll.performed -= m_Wrapper.m_MenuInteractionsActionsCallbackInterface.OnUIScroll;
                @UIScroll.canceled -= m_Wrapper.m_MenuInteractionsActionsCallbackInterface.OnUIScroll;
            }
            m_Wrapper.m_MenuInteractionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @UIClick.started += instance.OnUIClick;
                @UIClick.performed += instance.OnUIClick;
                @UIClick.canceled += instance.OnUIClick;
                @UIScroll.started += instance.OnUIScroll;
                @UIScroll.performed += instance.OnUIScroll;
                @UIScroll.canceled += instance.OnUIScroll;
            }
        }
    }
    public MenuInteractionsActions @MenuInteractions => new MenuInteractionsActions(this);

    // ThirdPersonSwitcher
    private readonly InputActionMap m_ThirdPersonSwitcher;
    private IThirdPersonSwitcherActions m_ThirdPersonSwitcherActionsCallbackInterface;
    private readonly InputAction m_ThirdPersonSwitcher_ThirdPerson;
    private readonly InputAction m_ThirdPersonSwitcher_FirstPerson;
    public struct ThirdPersonSwitcherActions
    {
        private @InputWrapper m_Wrapper;
        public ThirdPersonSwitcherActions(@InputWrapper wrapper) { m_Wrapper = wrapper; }
        public InputAction @ThirdPerson => m_Wrapper.m_ThirdPersonSwitcher_ThirdPerson;
        public InputAction @FirstPerson => m_Wrapper.m_ThirdPersonSwitcher_FirstPerson;
        public InputActionMap Get() { return m_Wrapper.m_ThirdPersonSwitcher; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ThirdPersonSwitcherActions set) { return set.Get(); }
        public void SetCallbacks(IThirdPersonSwitcherActions instance)
        {
            if (m_Wrapper.m_ThirdPersonSwitcherActionsCallbackInterface != null)
            {
                @ThirdPerson.started -= m_Wrapper.m_ThirdPersonSwitcherActionsCallbackInterface.OnThirdPerson;
                @ThirdPerson.performed -= m_Wrapper.m_ThirdPersonSwitcherActionsCallbackInterface.OnThirdPerson;
                @ThirdPerson.canceled -= m_Wrapper.m_ThirdPersonSwitcherActionsCallbackInterface.OnThirdPerson;
                @FirstPerson.started -= m_Wrapper.m_ThirdPersonSwitcherActionsCallbackInterface.OnFirstPerson;
                @FirstPerson.performed -= m_Wrapper.m_ThirdPersonSwitcherActionsCallbackInterface.OnFirstPerson;
                @FirstPerson.canceled -= m_Wrapper.m_ThirdPersonSwitcherActionsCallbackInterface.OnFirstPerson;
            }
            m_Wrapper.m_ThirdPersonSwitcherActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ThirdPerson.started += instance.OnThirdPerson;
                @ThirdPerson.performed += instance.OnThirdPerson;
                @ThirdPerson.canceled += instance.OnThirdPerson;
                @FirstPerson.started += instance.OnFirstPerson;
                @FirstPerson.performed += instance.OnFirstPerson;
                @FirstPerson.canceled += instance.OnFirstPerson;
            }
        }
    }
    public ThirdPersonSwitcherActions @ThirdPersonSwitcher => new ThirdPersonSwitcherActions(this);

    // CaptureCamera
    private readonly InputActionMap m_CaptureCamera;
    private ICaptureCameraActions m_CaptureCameraActionsCallbackInterface;
    private readonly InputAction m_CaptureCamera_Beautify;
    private readonly InputAction m_CaptureCamera_Unbeautify;
    public struct CaptureCameraActions
    {
        private @InputWrapper m_Wrapper;
        public CaptureCameraActions(@InputWrapper wrapper) { m_Wrapper = wrapper; }
        public InputAction @Beautify => m_Wrapper.m_CaptureCamera_Beautify;
        public InputAction @Unbeautify => m_Wrapper.m_CaptureCamera_Unbeautify;
        public InputActionMap Get() { return m_Wrapper.m_CaptureCamera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CaptureCameraActions set) { return set.Get(); }
        public void SetCallbacks(ICaptureCameraActions instance)
        {
            if (m_Wrapper.m_CaptureCameraActionsCallbackInterface != null)
            {
                @Beautify.started -= m_Wrapper.m_CaptureCameraActionsCallbackInterface.OnBeautify;
                @Beautify.performed -= m_Wrapper.m_CaptureCameraActionsCallbackInterface.OnBeautify;
                @Beautify.canceled -= m_Wrapper.m_CaptureCameraActionsCallbackInterface.OnBeautify;
                @Unbeautify.started -= m_Wrapper.m_CaptureCameraActionsCallbackInterface.OnUnbeautify;
                @Unbeautify.performed -= m_Wrapper.m_CaptureCameraActionsCallbackInterface.OnUnbeautify;
                @Unbeautify.canceled -= m_Wrapper.m_CaptureCameraActionsCallbackInterface.OnUnbeautify;
            }
            m_Wrapper.m_CaptureCameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Beautify.started += instance.OnBeautify;
                @Beautify.performed += instance.OnBeautify;
                @Beautify.canceled += instance.OnBeautify;
                @Unbeautify.started += instance.OnUnbeautify;
                @Unbeautify.performed += instance.OnUnbeautify;
                @Unbeautify.canceled += instance.OnUnbeautify;
            }
        }
    }
    public CaptureCameraActions @CaptureCamera => new CaptureCameraActions(this);

    // DebugKeys
    private readonly InputActionMap m_DebugKeys;
    private IDebugKeysActions m_DebugKeysActionsCallbackInterface;
    private readonly InputAction m_DebugKeys_Opty;
    private readonly InputAction m_DebugKeys_LoadHub;
    private readonly InputAction m_DebugKeys_ToggleDamageNumbers;
    public struct DebugKeysActions
    {
        private @InputWrapper m_Wrapper;
        public DebugKeysActions(@InputWrapper wrapper) { m_Wrapper = wrapper; }
        public InputAction @Opty => m_Wrapper.m_DebugKeys_Opty;
        public InputAction @LoadHub => m_Wrapper.m_DebugKeys_LoadHub;
        public InputAction @ToggleDamageNumbers => m_Wrapper.m_DebugKeys_ToggleDamageNumbers;
        public InputActionMap Get() { return m_Wrapper.m_DebugKeys; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DebugKeysActions set) { return set.Get(); }
        public void SetCallbacks(IDebugKeysActions instance)
        {
            if (m_Wrapper.m_DebugKeysActionsCallbackInterface != null)
            {
                @Opty.started -= m_Wrapper.m_DebugKeysActionsCallbackInterface.OnOpty;
                @Opty.performed -= m_Wrapper.m_DebugKeysActionsCallbackInterface.OnOpty;
                @Opty.canceled -= m_Wrapper.m_DebugKeysActionsCallbackInterface.OnOpty;
                @LoadHub.started -= m_Wrapper.m_DebugKeysActionsCallbackInterface.OnLoadHub;
                @LoadHub.performed -= m_Wrapper.m_DebugKeysActionsCallbackInterface.OnLoadHub;
                @LoadHub.canceled -= m_Wrapper.m_DebugKeysActionsCallbackInterface.OnLoadHub;
                @ToggleDamageNumbers.started -= m_Wrapper.m_DebugKeysActionsCallbackInterface.OnToggleDamageNumbers;
                @ToggleDamageNumbers.performed -= m_Wrapper.m_DebugKeysActionsCallbackInterface.OnToggleDamageNumbers;
                @ToggleDamageNumbers.canceled -= m_Wrapper.m_DebugKeysActionsCallbackInterface.OnToggleDamageNumbers;
            }
            m_Wrapper.m_DebugKeysActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Opty.started += instance.OnOpty;
                @Opty.performed += instance.OnOpty;
                @Opty.canceled += instance.OnOpty;
                @LoadHub.started += instance.OnLoadHub;
                @LoadHub.performed += instance.OnLoadHub;
                @LoadHub.canceled += instance.OnLoadHub;
                @ToggleDamageNumbers.started += instance.OnToggleDamageNumbers;
                @ToggleDamageNumbers.performed += instance.OnToggleDamageNumbers;
                @ToggleDamageNumbers.canceled += instance.OnToggleDamageNumbers;
            }
        }
    }
    public DebugKeysActions @DebugKeys => new DebugKeysActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_XRSchemeIndex = -1;
    public InputControlScheme XRScheme
    {
        get
        {
            if (m_XRSchemeIndex == -1) m_XRSchemeIndex = asset.FindControlSchemeIndex("XR");
            return asset.controlSchemes[m_XRSchemeIndex];
        }
    }
    public interface IDeviceBaseActions
    {
        void OnRightControllerPose(InputAction.CallbackContext context);
        void OnLeftControllerPose(InputAction.CallbackContext context);
        void OnLeftTriggerAxis(InputAction.CallbackContext context);
        void OnLeftGripAxis(InputAction.CallbackContext context);
        void OnRightTriggerAxis(InputAction.CallbackContext context);
        void OnRightGripAxis(InputAction.CallbackContext context);
    }
    public interface IMainInteractionsActions
    {
        void OnPause(InputAction.CallbackContext context);
    }
    public interface IPlayerControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
    }
    public interface IUtilityActions
    {
        void OnItemActivation(InputAction.CallbackContext context);
        void OnGemEquip(InputAction.CallbackContext context);
        void OnTelekinesisApplyGemEffect(InputAction.CallbackContext context);
    }
    public interface IAbilitiesActions
    {
        void OnPrimary(InputAction.CallbackContext context);
        void OnSecondary(InputAction.CallbackContext context);
        void OnGrab(InputAction.CallbackContext context);
        void OnTelekinesis(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
    }
    public interface IMenuInteractionsActions
    {
        void OnUIClick(InputAction.CallbackContext context);
        void OnUIScroll(InputAction.CallbackContext context);
    }
    public interface IThirdPersonSwitcherActions
    {
        void OnThirdPerson(InputAction.CallbackContext context);
        void OnFirstPerson(InputAction.CallbackContext context);
    }
    public interface ICaptureCameraActions
    {
        void OnBeautify(InputAction.CallbackContext context);
        void OnUnbeautify(InputAction.CallbackContext context);
    }
    public interface IDebugKeysActions
    {
        void OnOpty(InputAction.CallbackContext context);
        void OnLoadHub(InputAction.CallbackContext context);
        void OnToggleDamageNumbers(InputAction.CallbackContext context);
    }
}
