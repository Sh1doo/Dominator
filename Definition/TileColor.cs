using UnityEngine;

public static class TileColor
{
    public static Color32 WHITE = new Color32(255, 255, 255, 255);
    public static Color32 GREEN = new Color32(51,161,94,255);
    public static Color32 ORANGE = new Color32(255, 153, 20, 255);
    public static Color32 PINK = new Color32(223, 76, 148, 255);
    public static Color32 BLUE = new Color32(56, 108, 176, 255);

    public static Color32 DARKWHITE = new Color32(171, 171, 171, 255);
    public static Color32 DARKGREEN = new Color32(0, 83, 24, 255);
    public static Color32 DARKORANGE = new Color32(160, 76, 0, 255);
    public static Color32 DARKPINK = new Color32(134, 0, 73, 255);
    public static Color32 DARKBLUE = new Color32(0, 40, 97, 255);

    public static Color32 getColor(int num)
    {
        switch (num)
        {
            case 0: return GREEN;
            case 1: return ORANGE;
            case 2: return PINK;
            case 3: return BLUE;
            default: return WHITE;
        }
    }

    public static Color32 getDarkColor(int num)
    {
        switch (num)
        {
            case 0: return DARKGREEN;
            case 1: return DARKORANGE;
            case 2: return DARKPINK;
            case 3: return DARKBLUE;
            default: return DARKWHITE;
        }
    }
}
