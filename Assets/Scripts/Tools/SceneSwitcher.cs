#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct SceneInfo
{
    public string name;
    public string path;

    public SceneInfo(string name, string path)
    {
        this.name = name;
        this.path = path;
    }
}

[InitializeOnLoad]
public class SceneSwitcher : EditorWindow
{
    private static SceneInfo[] scenes;
    private static int selectedSceneIndex = 0;

    static SceneSwitcher()
    {
        EditorApplication.update += UpdateSceneList;
        EditorSceneManager.sceneOpened += SceneOpened;
    }

    [MenuItem("Tools/Scene Switcher")]
    static void ShowWindow()
    {
        GetWindow<SceneSwitcher>("Scene Switcher");
    }

    private static void UpdateSceneList()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        scenes = new SceneInfo[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            scenes[i] = new SceneInfo(name, path);
        }
    }

    private static void SceneOpened(Scene scene, OpenSceneMode mode)
    {
        UpdateSceneList();
    }

    private void OnGUI()
    {
        if (scenes != null && scenes.Length > 0)
        {
            selectedSceneIndex = EditorGUILayout.Popup("Switch Scene", selectedSceneIndex, scenes.Select(s => s.name).ToArray());

            if (GUILayout.Button("Load Scene"))
            {
                if (selectedSceneIndex >= 0 && selectedSceneIndex < scenes.Length)
                {
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorSceneManager.OpenScene(scenes[selectedSceneIndex].path);
                }
            }
        }
        else
        {
            GUILayout.Label("No Scenes in Build Settings");
        }
    }
}
#endif
