using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinnerName : MonoBehaviour
{
    public static string winnerName = "";

    void Awake()
    {
        Text Name = this.GetComponent<Text>();
        Name.text = winnerName;
    }
}
