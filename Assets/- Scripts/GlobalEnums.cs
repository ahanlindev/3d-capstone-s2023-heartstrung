#region SCENE_NAMES
/// <summary>Enum that maps to the string names of given scenes. Call ToName to get string version</summary>
public enum SceneID {
    MAINMENU,
    VICTORY,
    GAME_OVER,
    COMIC1,
    COMIC2,
    COMIC3,
    COMIC4,
    COMIC5,
    COMIC6,
    TUTORIAL_1,
    TUTORIAL_2, // TODO remove
    TUTORIAL_2_1,
    TUTORIAL_2_2,
    TUTORIAL_2_3,
    TUTORIAL_2_4,
    STRAWBERRY_1,
    STRAWBERRY_2,
    KINGDOM_1,
    KINGDOM_2,
}

public static class SceneIDsToString {
    public static string GetName(this SceneID name) => name switch {
        SceneID.MAINMENU => "Main Menu",
        SceneID.VICTORY => "Victory",
        SceneID.GAME_OVER => "Defeat",
        SceneID.COMIC1 => "Comic1",
        SceneID.COMIC2 => "Comic2",
        SceneID.COMIC3 => "Comic3",
        SceneID.COMIC4 => "Comic4",
        SceneID.COMIC5 => "Comic5",
        SceneID.COMIC6 => "Comic6",
        SceneID.TUTORIAL_1 => "Tutorial 1",
        SceneID.TUTORIAL_2 => "Tutorial 2",  // TODO remove
        SceneID.TUTORIAL_2_1 => "Tutorial 2.1",
        SceneID.TUTORIAL_2_2 => "Tutorial 2.2",
        SceneID.TUTORIAL_2_3 => "Tutorial 2.3",
        SceneID.TUTORIAL_2_4 => "Tutorial 2.4",
        SceneID.STRAWBERRY_1 => "Strawberry 1",
        SceneID.STRAWBERRY_2 => "Strawberry 2",
        SceneID.KINGDOM_1 => "Kingdom 1",
        SceneID.KINGDOM_2 => "Kingdom 2",
        _ => UnhandledEnum(),
    };

    private static string UnhandledEnum() {
        UnityEngine.Debug.LogError("unhandled scene name enum value");
        return "";
    }
}
#endregion
