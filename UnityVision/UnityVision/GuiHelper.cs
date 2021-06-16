using UnityEngine;

namespace UnityVision
{

    // Renders Everything

    public static class GuiHelper
    {
        public static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Color color)
        {
            GuiHelper.DrawLine(lineStart, lineEnd, color, 1);
        }

        public static void DrawBox(float x, float y, float w, float h, Color color)
        {
            GuiHelper.DrawLine(new Vector2(x, y), new Vector2(x + w, y), color);
            GuiHelper.DrawLine(new Vector2(x, y), new Vector2(x, y + h), color);
            GuiHelper.DrawLine(new Vector2(x + w, y), new Vector2(x + w, y + h), color);
            GuiHelper.DrawLine(new Vector2(x, y + h), new Vector2(x + w, y + h), color);
        }

        public static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Color color, int thickness)
        {
            if (GuiHelper._coloredLineTexture == null || GuiHelper._coloredLineColor != color)
            {
                GuiHelper._coloredLineColor = color;
                GuiHelper._coloredLineTexture = new Texture2D(1, 1);
                GuiHelper._coloredLineTexture.SetPixel(0, 0, GuiHelper._coloredLineColor);
                GuiHelper._coloredLineTexture.wrapMode = TextureWrapMode.Repeat;
                GuiHelper._coloredLineTexture.Apply();
            }
            GuiHelper.DrawLineStretched(lineStart, lineEnd, GuiHelper._coloredLineTexture, thickness);
        }

        public static void DrawLineStretched(Vector2 lineStart, Vector2 lineEnd, Texture2D texture, int thickness)
        {
            Vector2 vector = lineEnd - lineStart;
            float num = 57.29578f * Mathf.Atan(vector.y / vector.x);
            if (vector.x < 0f)
            {
                num += 180f;
            }
            if (thickness < 1)
            {
                thickness = 1;
            }
            int num2 = (int)Mathf.Ceil((float)(thickness / 2));
            GUIUtility.RotateAroundPivot(num, lineStart);
            GUI.DrawTexture(new Rect(lineStart.x, lineStart.y - (float)num2, vector.magnitude, (float)thickness), texture);
            GUIUtility.RotateAroundPivot(-num, lineStart);
        }

        public static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Texture2D texture)
        {
            GuiHelper.DrawLine(lineStart, lineEnd, texture, 1);
        }

        public static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Texture2D texture, int thickness)
        {
            Vector2 vector = lineEnd - lineStart;
            float num = 57.29578f * Mathf.Atan(vector.y / vector.x);
            if (vector.x < 0f)
            {
                num += 180f;
            }
            if (thickness < 1)
            {
                thickness = 1;
            }
            int num2 = (int)Mathf.Ceil((float)(thickness / 2));
            Rect position = new Rect(lineStart.x, lineStart.y - (float)num2, Vector2.Distance(lineStart, lineEnd), (float)thickness);
            GUIUtility.RotateAroundPivot(num, lineStart);
            GUI.BeginGroup(position);
            int num3 = Mathf.RoundToInt(position.width);
            int num4 = Mathf.RoundToInt(position.height);
            for (int i = 0; i < num4; i += texture.height)
            {
                for (int j = 0; j < num3; j += texture.width)
                {
                    GUI.DrawTexture(new Rect((float)j, (float)i, (float)texture.width, (float)texture.height), texture);
                }
            }
            GUI.EndGroup();
            GUIUtility.RotateAroundPivot(-num, lineStart);
        }

        private static Texture2D _coloredLineTexture;

        private static Color _coloredLineColor;
    }


}
