using Entitas;
using H4ck.Helper;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace H4ck.h4ckFuncs
{
    //获取所需数据
    class GetData : MonoBehaviour
    {
		internal static PlayerEntity me;
		internal static List<IEntity> players;
		internal static PlayerEntity A1mt4rget;
		internal static Vector3 myPos; 

		void Update()
        {
			if (!Check.CheckIsLoadOver) { return; } //游戏未加载完毕 -> 返回
			Get();
		}
		void Get()
        {
            //取所有玩家
			players = Contexts.sharedInstance.player.GetEntities();
            //取自身
			me = Contexts.sharedInstance.player.myPlayerEntity;
			if (AimBot.Aiming && A1mt4rget != null)
			{
                //如果自瞄目标的血量(百分比)小于等于0 就返回
				if (!(A1mt4rget.GetHpPercent() <= 0f))
				{
					return;
				}
                //清空自瞄目标
				A1mt4rget = null;
			}
			int num1 = 10000;
            //判断自瞄目标(距离准星最近的目标)
			foreach (PlayerEntity entity in players)
			{
				if (entity.GetTeam() == me.GetTeam() || entity.GetHpPercent() == 0f)
				{
					continue;
				}
				Vector3 pos = entity.GetCompenstatePos(entity.fpos.Change.ࢱ);
				Vector3 vector = Camera.main.WorldToScreenPoint(PosHelper.EntityToWorld(pos) + new Vector3(0f, 180f, 0f));
				if (!(vector.z < 0f))
				{
					int Middle_On_Screen = (int)(Mathf.Abs(vector.x - Screen.width / 2f) + Mathf.Abs(vector.y - Screen.height / 2f));
					if (Middle_On_Screen < num1)
					{
						A1mt4rget = entity;
						num1 = Middle_On_Screen;
					}
				}
			} 
		}
	}
}
