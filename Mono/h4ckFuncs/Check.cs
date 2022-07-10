using UnityEngine;

namespace H4ck.h4ckFuncs
{
    class Check
    {
		internal static bool CheckIsLoadOver //检查游戏是否加载完毕
		{
			get
			{
				if (Contexts.sharedInstance == null || Contexts.sharedInstance.player == null || Contexts.sharedInstance.player.myPlayerEntity == null || Contexts.sharedInstance.player.myPlayerEntity.basicInfo == null || Camera.main == null)
				{
					return false;
				}
				return true;
			}
		}
    }
}
