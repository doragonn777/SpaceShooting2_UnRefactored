using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BacktoTitle : MonoBehaviour
{
    private PauseController pause;
    [SerializeField] GameObject pauseController;

    private void Start()
    {
        pause = pauseController.GetComponent<PauseController>();
    }
    public void OnClick()
    {
        pause.GamePause();
        SceneManager.LoadScene("Title");
    }
}
