using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject exitButton;

    private void Start()
    {
        #if UNITY_STANDALONE || UNITY_EDITOR
            this.exitButton.SetActive(true);
        #endif
    }

    public void Play()
    {
        StartCoroutine(this.ChangeScene("game"));
    }

    public void Exit()
    {
        StartCoroutine(this.WaitExit());
    }

    IEnumerator ChangeScene(string name)
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(name);
    }

    IEnumerator WaitExit()
    {
        yield return new WaitForSeconds(0.3f);

        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
