using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScroll : MonoBehaviour

{
    private static Vector2 scrollVector = new Vector2(0, 0);
    private static Vector2 screen_LeftBottom;
    private static Vector2 screen_RightTop;
    private static float rightEdge;
    private static float leftEdge;
    private static float topEdge;
    private static float bottomEdge;

    void Start()
    {
        screen_LeftBottom = Camera.main.ScreenToWorldPoint(Vector2.zero);
        screen_RightTop = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        rightEdge = screen_RightTop.x;
        leftEdge = screen_LeftBottom.x;
        topEdge = screen_RightTop.y;
        bottomEdge = screen_LeftBottom.y;
    }

    public static Vector2 GetScrollVector()
    {
        return scrollVector;
    }

    public static bool IsOutOfCamera(Vector2 point)
    {
        return point.x > rightEdge || point.x < leftEdge || point.y > topEdge || point.y < bottomEdge;
    }

    public static float GetRelativePosX(float x)    //カメラ内の相対位置を取得(左が0,右が1)
    {
        return (x - leftEdge) / (rightEdge - leftEdge);
    }

    public static float GetRelativePosY(float y)
    {
        return (y - bottomEdge) / (topEdge - bottomEdge);
    }

    public static float GetRightEdge()
    {
        return rightEdge;
    }

    public static float GetLeftEdge()
    {
        return leftEdge;
    }

    public static float GetTopEdge()
    {
        return topEdge;
    }

    public static float GetBottomEdge()
    {
        return bottomEdge;
    }
}
