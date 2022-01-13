using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FG.Assignment.InputManager
{
    
    public static class MultiSelect //static so we can reference its functions. 
    {
        private static Texture2D _whiteTexture;

        public static Texture2D WhiteTexture
        {
            get
            {
                if (_whiteTexture == null)
                {
                    _whiteTexture = new Texture2D(1, 1);
                    _whiteTexture.SetPixel(0,0, Color.white);
                    _whiteTexture.Apply();
                }

                return _whiteTexture;
            }
        }

        public static void DrawScreenRect(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, WhiteTexture);
        }

        public static void DrawScreenRectBorder(Rect rect, float thickness, Color color) //draws a border around the rectangle.
        {
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color); //top
            DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color); //bottom
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color); //left
            DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness,rect.height), color); //right
        }
        
        public static Rect GetScreenRect(Vector3 screenPos1, Vector3 screenPos2)
        {
            screenPos1.y = Screen.height - screenPos1.y;
            screenPos2.y = Screen.height - screenPos2.y;

            Vector3 bR = Vector3.Max(screenPos1, screenPos2);
            Vector3 tL = Vector3.Min(screenPos1, screenPos2);

            return Rect.MinMaxRect(tL.x, tL.y, bR.x, bR.y);
        }
        
        //translates the 3d space to screenspace.
        public static Bounds GetVpBounds(Camera cam, Vector3 screenPos1, Vector3 screenPos2)
        {
            Vector3 pos1 = cam.ScreenToViewportPoint(screenPos1);
            Vector3 pos2 = cam.ScreenToViewportPoint(screenPos2);

            Vector3 min = Vector3.Min(pos1, pos2);
            Vector3 max = Vector3.Max(pos1, pos2);

            min.z = cam.nearClipPlane;
            max.z = cam.farClipPlane;

            Bounds bounds = new Bounds();
            bounds.SetMinMax(min,max);

            return bounds;
        }

    } 
    
}