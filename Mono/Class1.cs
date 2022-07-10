using H4ck.h4ckFuncs;
using UnityEngine;

namespace H4ck
{
    class Class1
    {
        static GameObject h4ck; 
        static void Load()
        { 
            h4ck = new GameObject();
            h4ck.AddComponent<Reset>();
            h4ck.AddComponent<GetData>();
            h4ck.AddComponent<NoRecoil>();
            h4ck.AddComponent<Esp>();
            h4ck.AddComponent<AimBot>();
            h4ck.AddComponent<AutoHit>();
            h4ck.AddComponent<Report>();
            UnityEngine.Object.DontDestroyOnLoad(h4ck);
        }

        //刷新(已弃用)
        internal static void Refresh()
        {
            UnityEngine.Object.Destroy(h4ck.GetComponent<GetData>());
            UnityEngine.Object.Destroy(h4ck.GetComponent<NoRecoil>());
            UnityEngine.Object.Destroy(h4ck.GetComponent<Esp>());
            UnityEngine.Object.Destroy(h4ck.GetComponent<AimBot>());
            UnityEngine.Object.Destroy(h4ck.GetComponent<AutoHit>());
            UnityEngine.Object.Destroy(h4ck.GetComponent<Report>());

            h4ck.AddComponent<GetData>();
            h4ck.AddComponent<NoRecoil>();
            h4ck.AddComponent<Esp>(); 
            h4ck.AddComponent<AimBot>(); 
            h4ck.AddComponent<AutoHit>();
            h4ck.AddComponent<Report>(); 
        }
    }
}
