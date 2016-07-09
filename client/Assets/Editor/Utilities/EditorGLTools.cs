using System;
using UnityEngine;

public sealed class EditorGLTools
{
	public static void DrawTitleRect(Rect rect, string title,string content,Color color,Color bg, float width)
	{
		GLDraw.DrawFillBox (rect, color,bg, width);
		int top = 20;
		var lRect = new Rect (rect.x, rect.y+2, rect.width, top);
		GLDraw.DrawLine (
			new Vector2 (rect.x, lRect.yMax), 
			new Vector2 (rect.x + rect.width, lRect.yMax),
			color, 1);
		GUI.Label (lRect, title);
		var bRect = new Rect (rect.x, rect.y + top+2, rect.width, rect.height-top);
		GUI.Label (bRect, content);
	}


	public static bool DrawExpandBox(Rect rect, string title, string content , bool expand ,Color color, Color bg,float width)
	{
		DrawTitleRect(rect,title,content,color,bg,width);

		float r = 8;
		var p = new Vector2 (rect.xMax + r, rect.center.y-r/2);
		var exRect = new Rect (p.x - r, p.y - r, r * 2, r * 2);
		GUI.Label (exRect, expand ? "-" : "+");

		if (Event.current.type == EventType.MouseDown) {
			if (exRect.Contains (Event.current.mousePosition)) {
				expand = !expand;
				Event.current.Use ();
			}
		}
		return expand;
	}
}

