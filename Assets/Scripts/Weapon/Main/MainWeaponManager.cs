using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWeaponManager : MonoBehaviour
{
    public static MainWeaponManager instance;

    [SerializeField] int level;
    int levelProcess;

    private Shooter shooter;
    private int shotNum;
    private float lapseTime;
    public enum MainWeaponType { Normal, Laser, Wave, Homing, Tail}


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        level = 1;
        levelProcess = 0;

        shotNum = 0;
        lapseTime = 0;

        ChangeWeaponShooter<TailShooter>();
    }

    private void Update()
    {
        lapseTime += Time.deltaTime;
    }

    public void Shot()
    {
        if (shotNum < shooter.GetExistenceLimitOfShots(level) && lapseTime > shooter.GetShotInterval())
        {
            lapseTime = 0;
            shooter.MakeShot(level);
        }
    }

    public void AddShotNum()
    {
        shotNum++;
    }

    public void SubtractShotNum()
    {
        shotNum--;
    }

    public void ChangeWeaponShooter<Type>() where Type : Shooter
    {
        if (shooter != null) Destroy(shooter);
        shooter = gameObject.AddComponent<Type>();
    }

}
