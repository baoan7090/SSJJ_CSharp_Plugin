using UnityEngine;

namespace __
{
    class Helper
    {
		#region PositionHelper

		//获取玩家坐标
		internal static Vector3 GetPosition(PlayerEntity Player) 
		{
			return new Vector3((float)Player.GetX(), (float)Player.GetY(), (float)Player.GetZ());
		}

		//实体坐标转世界坐标
		internal static Vector3 EntityPos2World(Vector3 position)
		{
			return new Vector3(0f - position.y, position.z, position.x);
		}

		//世界坐标转实体坐标
		internal static Vector3 World2EntityPos(Vector3 position)
		{
			return new Vector3(position.z, -position.x, position.y);
		}
		#endregion

		#region Draw

		//绘制线条
		internal static void DrawLine(Vector2 from, Vector2 to, float thickness)
		{
			Vector2 normalized = (to - from).normalized;
			float num = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
			GUIUtility.RotateAroundPivot(num, from);
			DrawRect(from, Vector2.right * (from - to).magnitude, thickness);
			GUIUtility.RotateAroundPivot(-num, from);
		}

		//绘制文本
		internal static void DrawText(Vector2 position, string label, bool centered = true)
		{
			GUIStyle guistyle = new GUIStyle(GUI.skin.label);
			GUIContent content = new GUIContent(label);
			Vector2 vector = guistyle.CalcSize(content);
			GUI.Label(new Rect(centered ? (position - vector / 2f) : position, vector), content);
		}

		//绘制矩形
		internal static void DrawRect(Vector2 position, Vector2 size, float thickness)
		{
			GUI.DrawTexture(new Rect(position.x, position.y, size.x, thickness), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(position.x, position.y, thickness, size.y), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(position.x + size.x, position.y, thickness, size.y), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(position.x, position.y + size.y, size.x + thickness, thickness), Texture2D.whiteTexture);
		}

		//绘制四角矩形(英文烂)
		internal static void Draw4angleRect(Vector2 position, Vector2 size, float thickness)
		{
			GUI.DrawTexture(new Rect(position.x, position.y, (size.x - size.x * 0.3f) / 2f, thickness), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(position.x + (size.x + size.x * 0.3f) / 2f, position.y, size.x - (size.x + size.x * 0.3f) / 2f, thickness), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(position.x, position.y + size.y - thickness, (size.x - size.x * 0.3f) / 2f, thickness), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(position.x + (size.x + size.x * 0.3f) / 2f, position.y + size.y - thickness, size.x - (size.x + size.x * 0.3f) / 2f, thickness), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(position.x, position.y, thickness, (size.y - size.x * 0.9f) / 2f), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(position.x, position.y + (size.y + size.x * 0.9f) / 2f, thickness, size.y - (size.y + size.x * 0.9f) / 2f), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(position.x + size.x - thickness, position.y, thickness, (size.y - size.x * 0.9f) / 2f), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(position.x + size.x - thickness, position.y + (size.y + size.x * 0.9f) / 2f, thickness, size.y - (size.y + size.x * 0.9f) / 2f), Texture2D.whiteTexture);
		}
		#endregion
	}
}
