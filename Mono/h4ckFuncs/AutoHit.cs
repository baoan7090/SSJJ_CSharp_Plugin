using Assets.Sources.Utils.Weapon;
using share;
using System.Runtime.InteropServices;
using UnityEngine;

namespace H4ck.h4ckFuncs
{
    //自动扳机
    class AutoHit : MonoBehaviour
    { 
        //开关
        internal static bool autoShoot = true;
        void Update()
        {
            if (!Check.CheckIsLoadOver) { return; }
            if (GetData.me.IsDead()) { return; } //死亡不继续
            if (autoShoot) { AutomaticHit(); }
        }
        void AutomaticHit()
        {
            int num = AimOrNot(); //如果数值小于等于0，不继续
            if (num <= 0)
            {
                return;
            }
            //遍历玩家
            foreach (PlayerEntity entity in GetData.players)
            {
                PlayerEntity playerEntity = entity;
                if (playerEntity.GetId() == num) //如果此玩家的id = 子弹落点的玩家的id
                {
                    if (playerEntity.GetTeam() != GetData.me.GetTeam()) //此玩家不是队友 -> 继续
                    {
                        if (!playerEntity.IsDead()) //此玩家未死亡 -> 继续
                        {
                            mouse_event(2, 0, 0, 0, 0); //单击鼠标左键
                            mouse_event(4, 0, 0, 0, 0); //松开鼠标左键
                            break;
                        }
                    }
                    break;
                }
            }
        }
        int AimOrNot() //判断子弹落点是否在玩家身上(?)
        {
            Vector3 vector = WorldToEntityPos(Camera.main.transform.forward);
            return FireUtility.BulletTrace(Contexts.sharedInstance.battleRoom.pyEngine.PyEngine, GetData.me, Contexts.sharedInstance.player, 1E+07f, new Vector3D(vector.x, vector.y, vector.z), new float[3], new float[3], knife: false).EntityId;
        }

        Vector3 WorldToEntityPos(Vector3 position)
        {
            return new Vector3(position.z, -position.x, position.y);
        }
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    }
}
