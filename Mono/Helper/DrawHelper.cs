using UnityEngine;

namespace H4ck.Helper
{
    //绘制助手(网上抄的)
    class DrawHelper
	{
		internal static Color Color
		{
			get
			{
				return GUI.color;
			}
			set
			{
				GUI.color = value;
			}
		}
		internal static void DrawBox(Vector2 position, Vector2 size, float thickness, Color color)
		{
			Color = color;
			DrawBox(position, size, thickness);
		}
		internal static void DrawBox(Vector2 position, Vector2 size, float thickness)
		{
			GUI.DrawTexture(new Rect(position.x, position.y, size.x, thickness), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(position.x, position.y, thickness, size.y), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(position.x + size.x, position.y, thickness, size.y), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(position.x, position.y + size.y, size.x + thickness, thickness), Texture2D.whiteTexture);
		}
		internal static void DrawLine(Vector2 from, Vector2 to, float thickness)
		{
			Vector2 normalized = (to - from).normalized;
			float num = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
			GUIUtility.RotateAroundPivot(num, from);
			DrawBox(from, Vector2.right * (from - to).magnitude, thickness);
			GUIUtility.RotateAroundPivot(-num, from);
		}
		internal static void DrawLine(Vector2 from, Vector2 to, float thickness, Color color)
		{
			Color = color;
			DrawLine(from, to, thickness);
		}
		internal static string MakeEnable(string text, bool state)
		{
			return string.Format("{0}{1}", text, state ? "[开启]" : "[关闭]");
		}

	}
}

/*
        int toolbarInt;
        string[] toolbarStrings = new string[] { "Toolbar1", "Toolbar2", "Toolbar3" };
        toolbarInt = GUI.Toolbar(new Rect(25, 25, 250, 30), toolbarInt, toolbarStrings);
        画一排按钮


*/