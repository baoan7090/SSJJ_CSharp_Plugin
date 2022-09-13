using UnityEngine;

namespace H4ck.Helper
{
    static class PosHelper
    {
        internal static Vector3 EntityToWorld(Vector3 pos)
        {
            return new Vector3(-pos.y, pos.z, pos.x);
        }
        //备选方案
        internal static Vector3 GetPosition2(this PlayerEntity player)
        {
            return new Vector3((float)player.GetX(), (float)player.GetY(), (float)player.GetZ());
        }
    }
}
