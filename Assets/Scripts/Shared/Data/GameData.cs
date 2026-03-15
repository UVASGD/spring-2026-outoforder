using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    // save relevant data
    public static int escapeRoomNumber;
    public static int visualNovelNumber;
    public static int visualNovelDialogueIndex;
    public static Dictionary<string, bool> routeFlags = new();
    public static TimeSpan playTime; 

    // shared
    public static bool currentlyTalking;
    public static Coroutine fadeCoroutine;

    // escape room
    // change this name convention, to no script
    public static EscapeRoomGameplay escapeRoomGameplayManager;

    // escape room
    public static VisualNovelGameplay visualNovelGameplayManager;
}