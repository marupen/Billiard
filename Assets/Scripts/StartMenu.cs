using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Image Back;
    public Sprite[] sprites;

    private int spritesNum;
    private int spritesCnt = 0;
    private float timeLeft = 5f;

    private void Awake()
    {
        spritesNum = sprites.Length;
        Back.sprite = sprites[spritesCnt];
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            timeLeft = 5f;
            spritesCnt++;
            if (spritesCnt >= spritesNum) spritesCnt = 0;
            Back.sprite = sprites[spritesCnt];
        }
    }
}
