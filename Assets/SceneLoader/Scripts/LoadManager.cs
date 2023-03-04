using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.InputSystem;

public class LoadManager : MonoBehaviour
{
    public GameObject panel;
    public Slider slider;
    public Text text;

    public GameObject loadScene;
    public Dropdown dropdown;

    public EditorSettings editorSettings;

    private PlayerInput playerInput;
    private AsyncOperation oper;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.enabled = false;
    }

    private void Start()
    {
        dropdown.AddOptions(editorSettings.scenePaths);
    }

    public void OnLoadClicked()
    {
        loadScene.SetActive(false);
        string scenePath = dropdown.options[dropdown.value].text;
        Debug.Log(scenePath);
        StartCoroutine(LoadSceneAsync(scenePath));
    }

    public IEnumerator LoadSceneAsync(string scenePath)
    {
        panel.SetActive(true);
        oper = SceneManager.LoadSceneAsync(scenePath);
        oper.allowSceneActivation = false;

        while (!oper.isDone)
        {
            if (oper.progress >= 0.9f)
            {
                slider.value = 1;
                text.text = "Presh any key to continue";
                playerInput.enabled = true;
                yield break;
            }
            else
            {
                slider.value = oper.progress;
                text.text = $"{(int)(oper.progress * 100)}%";
            }
            yield return null;
        }
    }

    public void OnAny(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
        {
            return;
        }
        oper.allowSceneActivation = true;
    }
}
