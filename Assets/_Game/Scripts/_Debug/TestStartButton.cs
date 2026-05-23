using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class TestStartButton : MonoBehaviour
{
    private const string SceneName = "chonnhaccuaban";

    public void StartGame()
    {
        if (Application.CanStreamedLevelBeLoaded(SceneName))
        {
            SceneManager.LoadScene(SceneName);
            return;
        }

#if UNITY_EDITOR
        if (TryLoadSceneInEditor(SceneName))
        {
            return;
        }
#endif

        Debug.LogError("Unable to load scene '" + SceneName + "'. Add it to Build Settings for runtime loading, or ensure the scene asset exists for the editor-only fallback.");
    }

#if UNITY_EDITOR
    private static bool TryLoadSceneInEditor(string sceneName)
    {
        string[] guids = AssetDatabase.FindAssets(sceneName + " t:Scene");
        for (int i = 0; i < guids.Length; i++)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guids[i]);
            if (string.IsNullOrEmpty(scenePath))
            {
                continue;
            }

            if (System.IO.Path.GetFileNameWithoutExtension(scenePath) != sceneName)
            {
                continue;
            }

            EditorSceneManager.LoadSceneInPlayMode(scenePath, new LoadSceneParameters(LoadSceneMode.Single));
            return true;
        }

        return false;
    }
#endif
}
