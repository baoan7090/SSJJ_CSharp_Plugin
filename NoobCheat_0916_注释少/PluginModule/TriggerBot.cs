using Assets.Sources.Utils.Weapon;
using System;
using UnityEngine;
using static __.Helper;
using static __.PluginModule.GetNeedData;

namespace __.PluginModule
{
	//自动扳机 瞄准人即开枪  不多解释
    class TriggerBot : BaseModule
    {
        internal override void Update()
        {
			//使用try是为了不产生垃圾日志

            try { Trigger(); }
            catch (Exception) { }
        }
        void Trigger()
		{
			int num = AimOrNot();
			if (num <= 0)
			{
				return;
			}
			foreach (PlayerEntity 玩家 in PlayerList)
			{
				if (玩家.GetId() == num)
				{
					if (玩家.GetTeam() != MyEntity.GetTeam())
					{
						if (玩家.IsDead()) { break; }
						输入.ForceMouse(0, 输入.InputST.TrueOnce);
						break;
					}
					break;
				}
			}
		}
		int AimOrNot() //判断子弹落点是否在玩家身上
		{
			Vector3 vector = World2EntityPos(Camera.main.transform.forward);
			return FireUtility.BulletTraceNormal(Contexts.sharedInstance.battleRoom.pyEngine.PyEngine, MyEntity, 1E+07f, new Vector3(vector.x, vector.y, vector.z), new float[3], new float[3]).EntityId;
			//BulletTrace 产生大量垃圾日志 故使用BulletTraceNormal替代
		}
	}
}
