﻿using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.Input;

namespace StandardAssets.Characters.CharacterInput
{
	/// <summary>
	/// Unity original input implementation
	/// </summary>
	public class LegacyUnityCharacterInput : MonoBehaviour, ICharacterInput
	{
		[Header("Cinemachine Axes")]
		[SerializeField]
		protected string cinemachineLookXAxisName = "Horizontal";
		
		[SerializeField]
		protected string cinemachineLookYAxisName = "Vertical";
		
		[Header("Movement Input Axes")]
		[SerializeField]
		protected string horizontalAxisName = "Horizontal";
		
		[SerializeField]
		protected string verticalAxisName = "Vertical";

		[SerializeField]
		protected bool useLookInput = true;
		
		[SerializeField]
		protected string mouseLookXAxisName = "LookX";
		
		[SerializeField]
		protected string mouseLookYAxisName = "LookY";

		[SerializeField]
		protected string lookXAxisName = "LookX";
		
		[SerializeField]
		protected string lookYAxisName = "LookY";

		[SerializeField]
		protected string keyboardJumpName = "Jump";

		[SerializeField] 
		protected bool enableOnScreenJoystickControls;
		

		private Vector2 moveInputVector;
		private Action jumped;

		private Vector2 look;

		public StaticOnScreenJoystick leftOnScreenJoystick;
		public StaticOnScreenJoystick rightOnScreenJoystick;


		public Vector2 lookInput
		{
			get { return look; }
		}
		
		public Vector2 moveInput
		{
			get { return moveInputVector; }
		}
		
		public bool hasMovementInput 
		{ 
			get { return moveInput != Vector2.zero; }
		}
		
		public Action jumpPressed
		{
			get { return jumped; }
			set { jumped = value; }
		}
		
		private void OnEnable()
		{
			CinemachineCore.GetInputAxis = LookInputOverride;
		
		}

		private void Update()
		{
			UpdateLookVector();
			
			//Update Move Vector
			moveInputVector.Set(Input.GetAxis(horizontalAxisName), Input.GetAxis(verticalAxisName));
			
			if(Input.GetButtonDown(LegacyCharacterInputDevicesCache.ResolveControl(keyboardJumpName))||Input.GetButtonDown("Jump"))
			{
				if (jumpPressed != null)
				{
					jumpPressed();
				}
			}

			if (enableOnScreenJoystickControls)
			{
				GetOnScreenJoystickVectors();
			}
			
			
		}

		public void OnScreenTouchJump()
		{
			if (jumpPressed != null)
			{
				jumpPressed();
			}
		}

		void GetOnScreenJoystickVectors()
		{

			Vector2 leftStickVector = leftOnScreenJoystick.GetStickVector();
			moveInputVector.Set(leftStickVector.x,leftStickVector.y);

			Vector2 rightStickVector = rightOnScreenJoystick.GetStickVector();
			look.x = -rightStickVector.x;
			look.y = -rightStickVector.y;

		}

		/// <summary>
		/// Sets the Cinemachine cam POV to mouse inputs.
		/// </summary>
		private float LookInputOverride(string cinemachineAxisName)
		{
			if (!useLookInput)
			{
				return 0;
			}
			
			 if (cinemachineAxisName == cinemachineLookXAxisName)
			{
				return lookInput.x;
			}
			if (cinemachineAxisName == cinemachineLookYAxisName)
			{
				return lookInput.y;
			}
			
			return 0;
		}
		
		/// <summary>
		/// Update the look vector2, this is used in 3rd person
		/// and allows mouse and controller to both work at the same time.
		/// mouse look will take preference.
		/// IF there is no mouse inputs, it will get gamepad axis.  
		/// </summary>
		void UpdateLookVector()
		{
			if (!enableOnScreenJoystickControls)
			{
				if (mouseLookXAxisName == "" || mouseLookYAxisName == "") //Use gamepad look if no mouse look set
				{
					look.x = Input.GetAxis(LegacyCharacterInputDevicesCache.ResolveControl(lookXAxisName));
					look.y = Input.GetAxis(LegacyCharacterInputDevicesCache.ResolveControl(lookYAxisName));
				}
				else
				{
					if (Input.GetAxis(mouseLookXAxisName) != 0)
					{
						look.x = Input.GetAxis(mouseLookXAxisName);
					}
					else
					{
						look.x = Input.GetAxis(LegacyCharacterInputDevicesCache.ResolveControl(lookXAxisName));
					}
					if (Input.GetAxis(mouseLookYAxisName) != 0)
					{
						look.y = Input.GetAxis(mouseLookYAxisName);
					}
					else
					{
						look.y = Input.GetAxis(LegacyCharacterInputDevicesCache.ResolveControl(lookYAxisName));
					}
				}
			}
			
			

		}
	}
}