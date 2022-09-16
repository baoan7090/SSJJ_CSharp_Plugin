using Entitas;
using System;
using System.Collections.Generic;
using UnityEngine;
using static __.Helper;
using static __.PluginModule.AimBot;

namespace __.PluginModule
{
    class GetNeedData : BaseModule
    {
        #region Data
        internal static List<IEntity> PlayerList;
        internal static PlayerEntity AimTarget;
        internal static PlayerEntity MyEntity;
        #endregion


        internal override void Update()
        {
            GetData();
            UpdateAimTarget();
        }

        void GetData()
        {
            try
            {
                PlayerList = Contexts.sharedInstance.player.GetEntities();
                MyEntity = Contexts.sharedInstance.player.myPlayerEntity;
            }
            catch (Exception) { }
        }

        void UpdateAimTarget()
        {
            try
            {
                if (Aiming && AimTarget != null)
                {
                    if (!AimTarget.IsDead()) { return; }
                    AimTarget = null;
                }
                int lastDistance = 10000;
                foreach (PlayerEntity AimEntity in PlayerList)
                {
                    if (AimEntity.GetTeam() == MyEntity.GetTeam() || AimEntity.IsDead()) { continue; }
                    Vector3 Pos = Camera.main.WorldToScreenPoint(EntityPos2World(GetPosition(AimEntity)) + new Vector3(0f, 150f, 0f));
                    int Center = (int)(Math.Abs(Pos.x - Screen.width / 2f) + Math.Abs(Pos.y - Screen.height / 2f));

                    if (Center < lastDistance)
                    {
                        AimTarget = AimEntity;
                        lastDistance = Center;
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
