using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ゲームの停止状態を切り替える
    public void GamePause()
    {
        if (pauseUI.activeSelf)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
        pauseUI.SetActive(!pauseUI.activeSelf);
    }
}
