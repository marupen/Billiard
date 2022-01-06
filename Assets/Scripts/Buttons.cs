using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void onClickRestart()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void onClickQuit()
    {
        Application.Quit();
    }
}
