using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.SceneManagement;
using UnityEditor.Animations;

public class EditorMenu : MonoBehaviour
{
    [MenuItem("Assets/Create/Unit Resources", priority = 0)]
    public static void CreateUnitResources()
    {
        string path = $"{Application.dataPath}/Battle/Units";
        Directory.CreateDirectory($"{path}/NewUnit");
        Directory.CreateDirectory($"{path}/NewUnit/Animations");
        Directory.CreateDirectory($"{path}/NewUnit/Sprites");
    }

    [MenuItem("Assets/Create/Unit Prefab", priority = 0)]
    public static void CreateUnitUnitPrefab()
    {
        var scene = SceneManager.GetActiveScene();
        var objs = scene.GetRootGameObjects();
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].name == "TestUnits")
            {
                // Get the Prefab Asset root GameObject and its asset path.
                GameObject assetRoot = Selection.activeObject as GameObject;
                string assetPath = AssetDatabase.GetAssetPath(assetRoot);

                Debug.Log(assetPath);

                // Load the contents of the Prefab Asset.
                GameObject prefab = PrefabUtility.LoadPrefabContents(assetPath);

                // Modify Prefab contents.
                //contentsRoot.AddComponent<BoxCollider>();

                string path = $"Assets/Battle/Units/Giant";
                string spritePath = $"{path}/Sprites";
                string animPath = $"{path}/Animations";

                //Texture2D t2d = AssetDatabase.LoadAssetAtPath<Texture2D>(spritePath + "/default.png");
                //Sprite sprite = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f), 64);
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath + "/default.png");

                var sr = prefab.GetComponent<SpriteRenderer>();
                sr.sprite = sprite;

                List<Sprite> sprites = new();
                sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(spritePath + "/move/00.png"));
                sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(spritePath + "/move/01.png"));
                sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(spritePath + "/move/02.png"));
                sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(spritePath + "/move/03.png"));
                sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(spritePath + "/move/04.png"));
                sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(spritePath + "/move/05.png"));
                sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(spritePath + "/move/06.png"));
                sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(spritePath + "/move/07.png"));
                sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(spritePath + "/move/08.png"));
                sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(spritePath + "/move/09.png"));
                sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(spritePath + "/move/10.png"));
                sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(spritePath + "/move/11.png"));
                sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(spritePath + "/move/12.png"));

                AnimationClip clip = new AnimationClip();
                clip.frameRate = 24;
                EditorCurveBinding curveBinding = new EditorCurveBinding();
                curveBinding.type = typeof(SpriteRenderer);
                curveBinding.path = "";
                curveBinding.propertyName = "m_Sprite";
                ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[sprites.Count];
                for (int j = 0; j < sprites.Count; j++)
                {
                    keyframes[j] = new ObjectReferenceKeyframe();
                    keyframes[j].time = j * 1 / clip.frameRate;
                    keyframes[j].value = sprites[j];
                }

                AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip);
                clipSettings.loopTime = true;
                AnimationUtility.SetAnimationClipSettings(clip, clipSettings);
                AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyframes);
                AssetDatabase.CreateAsset(clip, animPath + "/Move.anim");

                var animCtrl = AnimatorController.CreateAnimatorControllerAtPath(animPath + "/Giant.controller");
                animCtrl.AddParameter("Moving", AnimatorControllerParameterType.Bool);
                animCtrl.AddParameter("Speed", AnimatorControllerParameterType.Float);

                animCtrl.AddMotion(clip);

                var rootSm = animCtrl.layers[0].stateMachine;


                var anim = prefab.GetComponent<Animator>();
                anim.runtimeAnimatorController = animCtrl;



                Instantiate(prefab, objs[i].transform);

                // Save contents back to Prefab Asset and unload contents.
                //PrefabUtility.SaveAsPrefabAsset(contentsRoot, assetPath);
                //PrefabUtility.UnloadPrefabContents(contentsRoot);
            }
        }
    }
}
