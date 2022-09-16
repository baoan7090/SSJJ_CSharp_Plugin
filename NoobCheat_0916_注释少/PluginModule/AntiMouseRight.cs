using Assets.Scripts.Input;
using System.Collections.Generic;
using UnityEngine;
using static __.PluginModule.GetNeedData;
using static __.PluginModule.AimBot;
using System;

namespace __.PluginModule
{

	//主要作用是屏蔽右键还有自动扳机
    class AntiMouseRight : BaseModule
    {
		bool isFakeInput = false;
		internal override void Update()
        {
			try
			{
				//由于在游戏加载过程替换也会产生垃圾日志 故替换前检查是否已进入地图
				if (isInGame)
				{
					CheckFakeInput();
				}
			}
			catch (Exception) { }
        }
		void CheckFakeInput()
		{
			if (!isFakeInput)
			{
				InputCollector.Instance.SetDeviceInput(new 输入());
				isFakeInput = true;
			}
		}
		bool isInGame
		{
			get
			{
				if (Contexts.sharedInstance == null || Contexts.sharedInstance.player == null || Contexts.sharedInstance.player.cameraOwnerEntity == null || Contexts.sharedInstance.player.cameraOwnerEntity.punchOrientation == null || Contexts.sharedInstance.player.cameraOwnerEntity.punchSmooth == null || Contexts.sharedInstance.player.myPlayerEntity == null || Contexts.sharedInstance.player.myPlayerEntity.basicInfo == null || Camera.main == null)
				{
					return false;
				}
				return true;
			}
		}
	}

	class 输入 : IDeviceInput
	{
		internal enum InputST
		{
			None,
			TrueKeep,
			TrueOnce,
			FalseKeep,
			FalseOnce
		}
		static Dictionary<int, int> forceMouse = new Dictionary<int, int>();
		static Dictionary<int, int> forceKey = new Dictionary<int, int>();

		//模拟鼠标(?)
		internal static void ForceMouse(int mouseButton, InputST st)
		{
			forceMouse[mouseButton] = (int)st;
		}
		//模拟键盘(?)
		internal static void ForceKey(KeyCode keyCode, InputST st)
		{
			forceKey[(int)keyCode] = (int)st;
		}
		public bool AnyKey()
		{
			return Input.anyKey;
		}

		public bool AnyKeyDown()
		{
			return Input.anyKeyDown;
		}

		public bool GetMouseButtonUp(int button)
		{
			return Input.GetMouseButtonUp(button);
		}

		public static Vector2 forceAxis = Vector2.zero;

		public float GetAxis(string axis)
		{
			if (axis == "Mouse X")
			{
				var x = forceAxis.x;
				forceAxis.x = 0;
				return Input.GetAxis(axis) + x;
			}
			if (axis == "Mouse Y")
			{
				var y = forceAxis.y;
				forceAxis.y = 0;
				return Input.GetAxis(axis) + y;
			}
			return Input.GetAxis(axis);
		}

		public bool GetKey(KeyCode keyCode)
		{
			if (forceKey.TryGetValue((int)keyCode, out var value) && value > 0)
			{
				switch (value)
				{
					case 1:
						return true;
					case 3:
						return false;
					case 2:
						forceKey[(int)keyCode] = 0;
						return true;
					case 4:
						forceKey[(int)keyCode] = 0;
						return false;
				}
			}
			return Input.GetKey(keyCode);
		}

		public bool GetKeyDown(KeyCode keyCode)
		{
			return Input.GetKeyDown(keyCode);
		}

		public bool GetMouseButton(int button)
		{
			if (forceMouse.TryGetValue(button, out var value) && value > 0)
			{
				switch (value)
				{
					case 1:
						return true;
					case 3:
						return false;
					case 2:
						forceMouse[button] = 0;
						return true;
					case 4:
						forceMouse[button] = 0;
						return false;
				}
			}
			//如果button为1 即玩家按下了右键
			//加一个KeyCode_ == 1 是为了判断自瞄热键是否为右，如果不是即不屏蔽
			//Weapon !=3 / !=4 为手持刀或手雷时不屏蔽 防止重击或者快投失效
			if (button == 1 && KeyCode_ == 1 && MyEntity.currentWeapon.Weapon != 3 && MyEntity.currentWeapon.Weapon != 4)
			{
				return button == 0;
			}
			return Input.GetMouseButton(button);
		}

		public bool GetMouseButtonDown(int button)
		{
			return Input.GetMouseButtonDown(button);
		}
	}
}
