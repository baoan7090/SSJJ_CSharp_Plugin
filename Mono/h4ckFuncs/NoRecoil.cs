using UnityEngine;
using Assets.Sources.Components.UserComand;
using Assets.Sources.Free.Data;

namespace H4ck.h4ckFuncs
{
    //无后座 自行研究
    class NoRecoil : MonoBehaviour
    {
		internal static bool isNorecoil = true;
		float oldPunchPitch;
		float oldPunchYaw;

		void Update()
        {
            if (!Check.CheckIsLoadOver) { return; }
			if (GetData.me.IsDead()) { return; }
			if (isNorecoil) { Norecoil(); }
		}

		void Norecoil()
		{
			float punchPitch = GetData.me.GetPunchPitch();
			float punchYaw = GetData.me.GetPunchYaw();
			InputComponent input = Contexts.sharedInstance.userCommand.input;
			input.Pitch -= 2f * (punchPitch - oldPunchPitch);
			input.Yaw -= 2f * (punchYaw - oldPunchYaw);

            //没有这行代码开枪时会像手动下拉，但是自瞄时会导致子弹飞天上
			Camera.main.transform.Rotate(0f - oldPunchPitch - GameModelLocator.GetInstance().GameModel.ShakeAngleOffect.y, 0f - oldPunchYaw - GameModelLocator.GetInstance().GameModel.ShakeAngleOffect.x, 0f);
			oldPunchPitch = punchPitch;
			oldPunchYaw = punchYaw;
		}
	}
}
