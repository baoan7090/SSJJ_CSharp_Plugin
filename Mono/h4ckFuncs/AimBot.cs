using H4ck.Helper;
using System.Runtime.InteropServices;
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
                "鼠标侧键",
                "F",
                "Control",
                "Alt"
            }; //自瞄热键_用于显示
        internal static int KeyCode_ = 5;
        internal static KeyCode[] AimKeyCode = new KeyCode[6]
            {
                KeyCode.Mouse0,
                KeyCode.Mouse1,
                KeyCode.Mouse3,
                KeyCode.F,
                KeyCode.LeftControl,
                KeyCode.LeftAlt
            };

        void Update() //(复制粘贴)
        {
            if (!Check.CheckIsLoadOver) { return; }
			if (GetData.me.IsDead()) { return; } //死亡后不触发
			if(GetData.A1mt4rget == null) { return; } //无自瞄目标 -> 返回

            //取自瞄目标实体坐标
            Vector3 pos = GetData.A1mt4rget.GetCompenstatePos(GetData.A1mt4rget.fpos.Change.ࢱ);
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
			double DistX = Aimtarget.x - Screen.width / 2f;
			double DistY = Aimtarget.y - Screen.height / 2f;
            /*
             另一种获取XY坐标写法(未测试)
             Vector2 Dist;
             Dist.X = Aimtarget.x - Screen.width / 2f;
             Dist.Y = Aimtarget.y - Screen.height / 2f;
             */

            //设置平滑 0.2需要配合游戏内10灵敏度+0鼠标平滑，否则会乱飘
            DistX /= 0.2;
			DistY /= 0.2;

            //利用鼠标事件自瞄，移动到自瞄点
            mouse_event(1, (int)DistX, (int)DistY, 0, 0);

            return true;
		}

        //获取自瞄坐标(世界坐标)(复制粘贴)
		Vector3 GetAimBotPos(PlayerEntity Target)
		{
			Vector3 vector = Vector3.zero;
			Transform transform = null;
			Transform transform2;
			if (vector == Vector3.zero)
			{
				transform = Target.GetBone("Bip01_Head");
				transform2 = Target.GetBone("Bip01_HeadNub");
				if (transform != null && transform2 != null)
				{
					vector = (transform.position + transform2.position) / 2f;
				}
			}
			if (vector == Vector3.zero && transform != null)
			{
				vector = transform.position;
			}
			return vector;
		}
		[DllImport("user32.dll")]
		static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo); 
	}
}
