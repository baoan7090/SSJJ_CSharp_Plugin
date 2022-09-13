using Assets.Scripts.Input;
using H4ck.Helper;
using System.Collections.Generic;
using UnityEngine;

namespace H4ck.h4ckFuncs
{
    class AimBot : MonoBehaviour
    {
        internal static bool Aiming; //配合 获取自瞄目标使用，如果不加此bool，自瞄时将不会锁死一个目标
        internal static string[] AimKey = new string[]
            {
                "鼠标左键",
                "鼠标右键",
                "鼠标上侧键",
				"鼠标下侧键",
				"F",
                "Control",
                "Alt"
            }; //自瞄热键_用于显示
        internal static int KeyCode_ = 5;
		internal static KeyCode[] AimKeyCode = new KeyCode[7]
			{
				KeyCode.Mouse0,
				KeyCode.Mouse1,
				KeyCode.Mouse3,
				KeyCode.Mouse4,
                KeyCode.F,
                KeyCode.LeftControl,
                KeyCode.LeftAlt
            };

        void Update() //(复制粘贴)
        {
            if (!Check.CheckIsLoadOver) { return; }
			if (GetData.me.IsDead()) { return; } //死亡后不触发
			if (GetData.A1mt4rget == null) { return; } //无自瞄目标 -> 返回

            //取自瞄目标实体坐标
            Vector3 pos = GetData.A1mt4rget.GetPosition2();
            //转世界坐标
            Vector3 a = PosHelper.EntityToWorld(pos);
            //转屏幕坐标 
            Vector3 b = Camera.main.WorldToScreenPoint(a);
            if (b.z < 0f) { return; } //在屏幕外 -> 不锁 (有问题)

            //热键触发
            if (Input.GetKey(AimKeyCode[KeyCode_]))
			{
				Vector2 bone = Camera.main.WorldToScreenPoint(GetAimBotPos(GetData.A1mt4rget));
                //将骨骼坐标转换到屏幕
                WeaponContext weapon = Contexts.sharedInstance.weapon;
                //判断武器类型(?)
                if (weapon == null || !(weapon.currentWeaponEntity?.slot.Slot > 3))
                {
                    //开锁！
                    Aiming = Aim(bone);
                }
            }
            else { Aiming = false; }//松开热键重置状态+重置自瞄目标(如果自瞄目标未重置)

		}
		bool Aim(Vector2 bone) //自瞄逻辑(复制粘贴)
		{
			if (GetData.A1mt4rget == null || bone == null) { return false; }  //如果自瞄目标不存在 或 骨骼不存在 -> 返回假
			Vector2 Aimtarget = new Vector2(bone.x, Screen.height - bone.y);


			//获取自瞄点X Y坐标

			Vector2 Dist = new Vector2(Aimtarget.x - Screen.width / 2f / 0.2f, Aimtarget.y - Screen.height / 2f / 0.2f);


            //利用伪造(输入)自瞄
            输入.forceAxis = Dist;

            return true;
		}

        //获取自瞄坐标(世界坐标)(复制粘贴)
		Vector3 GetAimBotPos(PlayerEntity target)
		{
			Vector3 vector = Vector3.zero;
			Transform transform = null;
			if (vector == Vector3.zero)
			{
				transform = target.GetBone("Bip01_Head");
				Transform bone = target.GetBone("Bip01_HeadNub");
				if (transform != null && bone != null)
				{
					vector = (transform.position + bone.position) / 2f;
				}
			}
			if (vector == Vector3.zero)
			{
				Transform bone2 = target.GetBone("Bone002");
				Transform bone3 = target.GetBone("Bone004");
				if (bone2 != null && bone3 != null)
				{
					vector = (bone2.position + bone3.position) / 2f;
				}
			}
			if (vector == Vector3.zero)
			{
				Transform bone4 = target.GetBone("Bone001");
				Transform bone5 = target.GetBone("Bone003");
				if (transform != null && bone4 != null && bone5 != null)
				{
					Vector3 b = (bone4.position + bone5.position) / 2f;
					vector = (transform.position + b) / 2f;
				}
			}
			if (vector == Vector3.zero)
			{
				Transform bone6 = target.GetBone("Bone005");
				if (transform != null && bone6 != null)
				{
					vector = (transform.position + bone6.position) / 2f;
				}
			}
			if (vector == Vector3.zero && transform != null)
			{
				vector = transform.position;
			}

			return vector;
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

		internal static void ForceMouse(int mouseButton, InputST st)
		{
			forceMouse[mouseButton] = (int)st;
		}
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
			if (button == 1 && AimBot.KeyCode_ == 2 && GetData.me.currentWeapon.Weapon != 3 && GetData.me.currentWeapon.Weapon != 4) 
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
