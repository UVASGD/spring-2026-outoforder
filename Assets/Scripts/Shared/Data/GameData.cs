using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    // save relevant data
    public static int escapeRoomNumber;
    public static int visualNovelDialogueIndex;
    public static List<string> routeFlags;

    // shared
    public static bool currentlyTalking;
    public static Coroutine fadeCoroutine;

    // escape room
    public static EscapeRoomGameplayManagerScript escapeRoomGameplayManagerScript;
}