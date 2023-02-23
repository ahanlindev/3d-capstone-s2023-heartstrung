#region SCENE_NAMES
/// <summary>Enum that maps to the string names of given scenes. Call ToName to get string version</summary>
public enum SceneNames {
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

public static class SceneNamesToString {
    public static string ToName(this SceneNames name) => name switch {
        SceneNames.MAINMENU => "Main Menu",
        SceneNames.VICTORY => "Victory",
        SceneNames.GAME_OVER => "Defeat",
        SceneNames.COMIC1 => "Comic1",
        SceneNames.COMIC2 => "Comic2",
        SceneNames.COMIC3 => "Comic3",
        SceneNames.COMIC4 => "Comic4",
        SceneNames.COMIC5 => "Comic5",
        SceneNames.COMIC6 => "Comic6",
        SceneNames.TUTORIAL_1 => "Tutorial 1",
        SceneNames.TUTORIAL_2 => "Tutorial 2",  // TODO remove
        SceneNames.TUTORIAL_2_1 => "Tutorial 2.1",
        SceneNames.TUTORIAL_2_2 => "Tutorial 2.2",
        SceneNames.TUTORIAL_2_3 => "Tutorial 2.3",
        SceneNames.TUTORIAL_2_4 => "Tutorial 2.4",
        SceneNames.STRAWBERRY_1 => "Strawberry 1",
        SceneNames.STRAWBERRY_2 => "Strawberry 2",
        SceneNames.KINGDOM_1 => "Kingdom 1",
        SceneNames.KINGDOM_2 => "Kingdom 2",
        _ => UnhandledEnum(),
    };

    private static string UnhandledEnum() {
        UnityEngine.Debug.LogError("unhandled scene name enum value");
        return "";
    }
}
#endregion
