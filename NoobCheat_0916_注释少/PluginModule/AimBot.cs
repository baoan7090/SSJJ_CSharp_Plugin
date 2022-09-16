using Assets.Sources.Components.UserComand;
using System;
using UnityEngine;
using static __.PluginModule.GetNeedData;

namespace __.PluginModule
{
    class AimBot : BaseModule
	{
        internal override void Update()
        {
			Aim();
		}

		internal static bool Aiming = false;
		internal static int KeyCode_ = 1;
		KeyCode[] HotKey = new KeyCode[7]
		{
				KeyCode.Mouse0,
				KeyCode.Mouse1,
				KeyCode.Mouse3,
				KeyCode.Mouse4,
				KeyCode.F,
				KeyCode.LeftControl,
				KeyCode.LeftAlt
		};
		void Aim()
        {
			try
            {
				if (Input.GetKey(HotKey[KeyCode_]))
				{
					WeaponContext weapon = Contexts.sharedInstance.weapon;
					if (weapon != null)
					{
						WeaponEntity currentWeaponEntity = weapon.currentWeaponEntity;
						if (currentWeaponEntity != null && currentWeaponEntity.slot.Slot > 3)
						{
							Aiming = false;
							return;
						}

						Aiming = true;
						LockPos(GetAimPoint(AimTarget));
						return;
					}
					return;
				}
				Aiming = false;
			}
			catch (Exception) { }
		}

		
		void LockPos(Vector3 pos)
		{
			//来源：TKR
			Vector3 vector = pos - Camera.main.transform.position;

			Vector3 eulerAngles = Quaternion.FromToRotation(vector.normalized, Vector3.forward).eulerAngles;

			//来源：老外
			if (eulerAngles.x > 180f)
			{
				eulerAngles.x -= 360f;
			}
			

			//来源：CSGO作弊
			eulerAngles.x -= MyEntity.GetPunchPitch() * 2.0f;
			eulerAngles.y -= MyEntity.GetPunchYaw() * 2.0f;

			//来源：绝影
			InputComponent input = Contexts.sharedInstance.userCommand.input;
			input.Pitch = eulerAngles.x;
			input.Yaw = eulerAngles.y;
		}
		//来源：绝影
		Vector3 GetAimPoint(PlayerEntity target)
		{
			Vector3 vector = Vector3.zero;
			Transform transform = null;
			if (vector == Vector3.zero)
			{
				transform = target.GetBone("Bip01_Head");
				Transform bone = target.GetBone("Bip01_HeadNub");
				if (transform != null && bone != null)
				{
					vector = (transform.position + bone.position) / 2f;
				}
			}
			if (vector == Vector3.zero)
			{
				Transform bone2 = target.GetBone("Bone002");
				Transform bone3 = target.GetBone("Bone004");
				if (bone2 != null && bone3 != null)
				{
					vector = (bone2.position + bone3.position) / 2f;
				}
			}
			if (vector == Vector3.zero)
			{
				Transform bone4 = target.GetBone("Bone001");
				Transform bone5 = target.GetBone("Bone003");
				if (transform != null && bone4 != null && bone5 != null)
				{
					Vector3 b = (bone4.position + bone5.position) / 2f;
					vector = (transform.position + b) / 2f;
				}
			}
			if (vector == Vector3.zero)
			{
				Transform bone6 = target.GetBone("Bone005");
				if (transform != null && bone6 != null)
				{
					vector = (transform.position + bone6.position) / 2f;
				}
			}
			if (vector == Vector3.zero && transform != null)
			{
				vector = transform.position;
			}
			return vector;
		}
	}
}
