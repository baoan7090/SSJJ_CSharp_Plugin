using UnityEngine;

namespace H4ck.Helper
{
    static class PosHelper
    {
        internal static Vector3 EntityToWorld(Vector3 pos)
        {
            return new Vector3(-pos.y, pos.z, pos.x);
        }

        internal static Vector3 GetPosition(this PlayerEntity player)
        {
            return player.GetCompenstatePos(player.fpos.Change.ࢱ);
        }
        //备选方案，如果第一种获取坐标无法使用即替换成此方法
        internal static Vector3 GetPosition2(this PlayerEntity player)
        {
            return new Vector3((float)player.GetX(), (float)player.GetY(), (float)player.GetZ());
        }
    }
}
