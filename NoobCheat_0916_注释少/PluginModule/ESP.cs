using System;
using UnityEngine;
using static __.PluginModule.AimBot;
using static __.PluginModule.GetNeedData;
using static __.Helper;

namespace __.PluginModule
{
    class ESP : BaseModule
    {
		Rect wind = new Rect(250f, 40f, 200f, 60);
		bool SettingWindow = true;
		string[] 自瞄键 = new string[]
		{
				"鼠标左键",
				"鼠标右键",
				"鼠标上侧键",
				"鼠标下侧键",
				"F",
				"Control",
				"Alt"
		};
        internal override void OnGUI()
        {
			ShowSetting();
			ForeachDraw();
		}

        internal override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Home)) { SettingWindow = !SettingWindow; }
        }
        void ForeachDraw()
        {
			try
            {
				foreach (PlayerEntity playerlist in PlayerList)
				{
					DrawESP(playerlist);
				}
			}
			catch (Exception) { }
        }
		void DrawESP(PlayerEntity Player)
        {
			if (Player == null || !Player.hasBasicInfo || Player.IsDead() || Player.GetTeam() == MyEntity.GetTeam())
			{
				return;
			}
			Vector3 vector = EntityPos2World(GetPosition(Player));
			Vector3 vector2 = Camera.main.WorldToScreenPoint(vector);
			if (vector2.z < 0f)
			{
				return;
			}
			Vector3 vector3 = Camera.main.WorldToScreenPoint(vector + new Vector3(0f, 180f, 0f));
			float num = Mathf.Abs(vector2.y - vector3.y);
			float num2 = num * 0.4f;
			Rect rect = new Rect(vector3.x - num2 / 2f, Screen.height - vector3.y, num2, num);
			Vector2 vector4 = new Vector2(vector3.x, Screen.height - vector3.y);
			Vector2 vector5 = new Vector2(Screen.width * 0.5f, 0f);
			int num3 = checked((int)Player.basicInfo.Current.Hp);
			GUI.color = Color.cyan;
			Draw4angleRect(new Vector2(rect.x, rect.y), new Vector2(rect.width, rect.height), 1f);

			GUI.color = Color.green;
			DrawLine(new Vector2(vector4.x, vector4.y), new Vector2(vector5.x, vector5.y), 1f);

			GUI.color = Color.yellow;
			GUI.DrawTexture(new Rect(rect.x - 3f, rect.y, 3f, num3 / 100f * num), Texture2D.whiteTexture);
			if (Player == AimTarget)
			{
				GUI.color = Color.red;
			}
			else
			{
				GUI.color = Color.cyan;
			}
			DrawText(new Vector2(vector3.x, rect.yMax + 14f), Player.basicInfo.PlayerName, true);
			if (Player.currentWeapon.Weapon == 4) { GUI.color = Color.yellow; DrawText(new Vector2(vector3.x, rect.yMax + 28f), "捏雷"); }
		}


		void ShowSetting()
        {
			try 
			{
				if (SettingWindow)
				{
					wind = GUI.Window(888888, wind, Settings, "NoobCheat1.4设置-Home显隐");
				}
			}
			catch (Exception) { }
		}
		void Settings(int winId)
		{
			Rect rect = new Rect(10f, 10f, 190f, 50f);
			GUI.BeginGroup(rect);

			GUI.color = Color.white;
			if (GUI.Button(new Rect(0f, rect.y + 5f, 180f, 20f), "自瞄热键：" + 自瞄键[KeyCode_]))
			{ KeyCode_ = (KeyCode_ + 1) % 7; }; //%后面的数值取决于AimBot.HotKey有多少个值

			GUI.EndGroup();
			GUI.DragWindow();
		}
	}
}
