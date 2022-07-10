using H4ck.Helper;
using UnityEngine;

namespace H4ck.h4ckFuncs
{
    //绘制
    class Esp : MonoBehaviour
	{
        //设置窗口
        internal static Rect wind;
        //显示设置窗口
        bool showSetting = true; 
        //显示绘制
		internal static bool showESP = true;
        //加载后定义设置窗口大小
        void Start() { wind = new Rect(250f, 40f, 150f, 110f); }
        void Update()
        { 
            //按键调整
            if (Input.GetKeyDown(KeyCode.Home)) { showSetting = !showSetting; } 
			if (Input.GetKeyDown(KeyCode.F1)) { NoRecoil.isNorecoil = !NoRecoil.isNorecoil; }
			if (Input.GetKeyDown(KeyCode.F2)) { showESP = !showESP; }
            if (Input.GetKeyDown(KeyCode.F3)) { AutoHit.autoShoot = !AutoHit.autoShoot; }
            if (Input.GetKeyDown(KeyCode.F4)) { AimBot.KeyCode_ = (AimBot.KeyCode_ + 1) % 6; }
        }

        void OnGUI()
        {
			if (showSetting)
			{
                wind = GUI.Window(999999, wind, Settings, "Mono设置-Home显隐");
            }
			if (!Check.CheckIsLoadOver) { return; }
			if (GetData.me.IsDead()) { return; } //死亡后不绘制
			if (showESP) {
                //遍历玩家
                foreach (PlayerEntity entity in GetData.players) {
                    //绘制玩家
                    Draw(entity);
                }
            }
		}
		void Settings(int winId)
        {
            Rect rect = new Rect(10f, 10f, 140f, 100f);
            GUI.BeginGroup(rect);
            //将文本绘制在设置窗口上面
            GUI.color = Color.cyan; 
            //每一行文本的y轴加20(取决于文本的高度)
            GUI.Label(new Rect(0f, rect.y, 140f, 20f), DrawHelper.MakeEnable("无后 F1 ", NoRecoil.isNorecoil));
            GUI.Label(new Rect(0f, rect.y + 20f, 140f, 20f), DrawHelper.MakeEnable("绘制 F2 ", showESP));
            GUI.Label(new Rect(0f, rect.y + 40f, 140f, 20f), DrawHelper.MakeEnable("扳机 F3 ", AutoHit.autoShoot));
            GUI.Label(new Rect(0f, rect.y + 60f, 140f, 20f), "自瞄热键 F4 " + AimBot.AimKey[AimBot.KeyCode_]); 
            GUI.EndGroup();
            //令窗口可拖动
            GUI.DragWindow();
        }
        void Draw(PlayerEntity player)
		{
            if (!player.hasBasicInfo) { return; } //玩家没有数据 -> 不绘制
            if (player == null) { return; } //玩家不存在 -> 不绘制

			GUI.color = Color.cyan; //定义绘制颜色(青色)

            //Vector3 pos = player.GetPosition();
            //Vector3 pos = player.GetPosition2();
            Vector3 pos = player.GetCompenstatePos(player.fpos.Change.ࢱ); //获取实体坐标

            //Vector3 GetData.myPos = GetData.me.GetPosition();
            //Vector3 GetData.myPos = GetData.me.GetPosition2(); 
            GetData.myPos = GetData.me.GetCompenstatePos(GetData.me.fpos.Change.ࢱ); //获取自身实体坐标 
			if (player.GetTeam() == GetData.me.GetTeam()) { return; } //自己队友 -> 不绘制
			Vector3 a = PosHelper.EntityToWorld(pos); //获取世界坐标
			Vector3 b = Camera.main.WorldToScreenPoint(a); //获取屏幕坐标
			if (b.z < 0f) { return; } //如果在屏幕外 -> 不绘制
            //不知道啥意思
			Vector3 c = Camera.main.WorldToScreenPoint(a + new Vector3(0f, 180f, 0f));
            
			float height = Mathf.Abs(b.y - c.y); //获取人物高度(屏幕上)
			float width = height / 2f; //获取人物宽度(屏幕上)

            //画方框的位置
			Rect box = new Rect(c.x - width / 2f, Screen.height - c.y, width, height);
            //与敌人的距离
			float Dist = (pos - GetData.myPos).magnitude;

            //用于画天线
            //方框顶部
			Vector2 Box_Top = new Vector2(c.x, Screen.height - c.y);
            //屏幕顶部
			Vector2 Screen_Top = new Vector2(Screen.width * 0.5f, 0f);

            //画天线
			DrawHelper.DrawLine(Box_Top, Screen_Top, 1f, Color.white);
            //画方框
			DrawHelper.DrawBox(new Vector2(box.x, box.y), new Vector2(box.width, box.height), 1f, (player == GetData.A1mt4rget) ? Color.red : Color.cyan);

            //画各种数据

            //***由于最初另外一种画文本有问题，改成这种繁琐的

			GUI.color = Color.cyan;
			GUIStyle StringStyle_ = new GUIStyle(GUI.skin.label);

            //计算文本大小
			Vector2 hp_size = StringStyle_.CalcSize(new GUIContent($"血量:{player.basicInfo.Current.Hp}"));
            //计算坐标
			Vector2 hp_pos = new Vector2(c.x, box.yMax + 12f);
            //置中
			Vector2 hp_center = hp_pos - hp_size / 2f;
            //设置位置
			Rect hp = new Rect(hp_center, hp_size);
            //绘制
			GUI.Label(hp, $"血量:{player.basicInfo.Current.Hp}");

            //其余原理相同

            //每一次绘制间隔都需要加14，初始字体大小为12，如果只加13将会显的很挤，于是加14显的美观

			Vector2 name_size = StringStyle_.CalcSize(new GUIContent(player.basicInfo.Current.PlayerName));
			Vector2 name_pos = new Vector2(c.x, box.yMax + 26f);
			Vector2 name_center = name_pos - name_size / 2f;
			Rect name = new Rect(name_center, name_size);
			GUI.Label(name, $"{player.basicInfo.Current.PlayerName}");

			Vector2 Dist_size = StringStyle_.CalcSize(new GUIContent($"{(int)Dist / 100}米"));
			Vector2 Dist_pos = new Vector2(c.x, box.yMax + 40f);
			Vector2 Dist_center = Dist_pos - Dist_size / 2f;
			Rect dist = new Rect(Dist_center, Dist_size);
			GUI.Label(dist, $"{(int)Dist / 100}米");
		}
	}
}
/*
                GUI.Label(new Rect(wind.x + 10f, wind.y + 20f, 140f, 20f), DrawHelper.MakeEnable("无后 F1 ", NoRecoil.isNorecoil));
                GUI.Label(new Rect(wind.x + 10f, wind.y + 40f, 140f, 20f), DrawHelper.MakeEnable("绘制 F2 ", showESP));
                GUI.Label(new Rect(wind.x + 10f, wind.y + 60f, 140f, 20f), DrawHelper.MakeEnable("扳机 F3 ", AutoHit.autoShoot));
                GUI.Label(new Rect(wind.x + 10f, wind.y + 80f, 140f, 20f), "自瞄热键 F4 " + AimBot.AimKey[AimBot.KeyCode_]);
     
     */
