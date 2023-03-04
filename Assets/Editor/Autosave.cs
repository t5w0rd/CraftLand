using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class AutoSave : EditorWindow
{
    public float saveTime = 300;
    public float nextSave = 0;

    [MenuItem("Window/AutoSave")]
    static void Init()
    {
        AutoSave window = (AutoSave)EditorWindow.GetWindowWithRect(typeof(AutoSave), new Rect(0, 0, 200, 50));
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Save Each:", saveTime + " Secs");
        float timeToSave = nextSave - (float)EditorApplication.timeSinceStartup;
        EditorGUILayout.LabelField("Next Save:", timeToSave.ToString() + " Sec");
        Repaint();

        if (EditorApplication.timeSinceStartup > nextSave)
        {
            var activeScene = EditorSceneManager.GetActiveScene();
            if (activeScene.isDirty && !EditorApplication.isPlaying)
            {
                bool saveOK = EditorSceneManager.SaveScene(activeScene);
                Debug.Log("Saved Scene " + (saveOK ? "OK" : "Error!"));
            }

            nextSave = (float)EditorApplication.timeSinceStartup + saveTime;
        }
    }
}