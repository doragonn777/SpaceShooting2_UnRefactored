using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    private Player p;
    private MainWeaponManager mainWeaponManager;
    private PauseController pause;
    [SerializeField] GameObject player;
    [SerializeField] GameObject pauseController;
    // Start is called before the first frame update
    void Start()
    {
        p = player.GetComponent<Player>();
        mainWeaponManager = player.GetComponent<MainWeaponManager>();
        pause = pauseController.GetComponent<PauseController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && CamScroll.GetRelativePosY(player.transform.position.y) < 1)
        {
            p.SetVelocityY(p.GetSpeed());
        }
        else if (Input.GetKey(KeyCode.S) && CamScroll.GetRelativePosY(player.transform.position.y) > 0)
        {
            p.SetVelocityY(-p.GetSpeed());
        }
        else
        {
            p.SetVelocityY(0);
        }

        if (Input.GetKey(KeyCode.A) && CamScroll.GetRelativePosX(player.transform.position.x) > 0)
        {
            p.SetVelocityX(-p.GetSpeed());
        }
        else if (Input.GetKey(KeyCode.D) && CamScroll.GetRelativePosX(player.transform.position.x) < 1)
        {
            p.SetVelocityX(p.GetSpeed());
        }
        else
        {
            p.SetVelocityX(0);
        }

        if (Input.GetKey(KeyCode.Space) && Time.timeScale != 0)
        {
            mainWeaponManager.Shot();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause.GamePause();
        }

        //デバッグ用
        if (Input.GetKey(KeyCode.Q) && Time.timeScale != 0)
        {
            p.transform.eulerAngles += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.E) && Time.timeScale != 0)
        {
            p.transform.eulerAngles += new Vector3(0, 0, -1);
        }
    }
}
