using UnityEngine;
namespace H4ck.h4ckFuncs
{
    //重置-->当自瞄目标卡住，或者设置窗口消失时可用
    class Reset : MonoBehaviour
    {
        void Update()
        { if (Input.GetKey(KeyCode.F)) {
                AimBot.Aiming = false;
                Esp.wind = new Rect(250f, 40f, 150f, 110f);
            }
        }
    }
}
