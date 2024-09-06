using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class Gun : MonoBehaviour
{
    public TextMeshProUGUI realBulletTxt;
    public TextMeshProUGUI fakeBulletTxt;
    public TextMeshProUGUI Player;
    public TextMeshProUGUI EnemyHp;
    [SerializeField] private int pr1 = 3;
    [SerializeField] private int em1 = 3;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shotClip;
    [SerializeField] private AudioClip noShotClip;
    [SerializeField] private AudioClip reloadClip;

    public Score score_save;
    public int isme;
    public Animator playerd;
    public Animator Enemyd;
    public Button button;
    public Button button2;

    public int bullet;

    private bool Intro = false;
    private bool isEnemy = false; //적 턴인가?

    private bool reload = false; // 장전을 해야할까?

    //총알
    public int real; //실탄 개수
    public int fake; //공포탄 개수

    public bool Shot = false;

    //총알
    public Animator anim;
    public List<int> shotgunBullit;


// Start is called before the first frame update
    void Start()
    {
        anim.GetComponent<Animator>();

        shotgunBullit = Enumerable.Repeat(0, 10).ToList(); // 배열에 for 문으로 총알 하나씩 집어넣기.
        isEnemy = false;
        reLoad();
        HideButton();
        HideButton2();
        fake = bullet - real; // 가짜탄 개수
        Intro = true;
        player();
        Enemy();
        Invoke(nameof(ShowButton), 5f);
        Invoke(nameof(ShowButton2), 5f);
        anim.Play("gunIntro");
    }

    private void Update()
    {
        if (Shot)
        {
            Shot = false;
            isme = Random.Range(1, 3);
            if (isme == 1)
            {
                EnemyShot();
            }

            if (isme == 2)
            {
                EnemySelf();
            }
        }

        if (em1 == 0)
        {
            enemyDie();
            Invoke(nameof(win), 3f);
        }

        if (pr1 == 0)
        {
            Invoke(nameof(realDie), 1.5f);
            Invoke(nameof(lose), 3f);
        }
    }

    void enemyDie()
    {
        Enemyd.Play("EnemyRealDie");
    }

    void realDie()
    {
        playerd.Play("prDie");
    }


    //플레이어 공격 구현
    //적 턴==========
    void Enemy()
    {
        anim.SetBool("gunEnemy", true);
        HideButton();
        HideButton2();
    }

    void Enemyf()
    {
        anim.SetBool("gunEnemy", false);
        Invoke(nameof(ShowButton), 2f);
        Invoke(nameof(ShowButton2), 2f);
    }
    //적 턴===========

//재장전=============
    void reLoad()
    {
        anim.Play("gunReLoding");
        audioSource.PlayOneShot(reloadClip);

        bullet = Random.Range(2, 8); //총 탄 개수
        real = (int)(Random.Range(1f, bullet / 2f)); //실탄 개수
        fake = bullet - real; // 가짜탄 개수

        for (int i = 0; i < real; i++) //실탄인지 공포탄인지 구별하는 
        {
            shotgunBullit[i] = 1;
        }

        for (int i = 0; i < fake; i++)
        {
            shotgunBullit[i] = 2;
        }

        Debug.Log(real);
        Debug.Log(fake);
        realBulletTxt.text = $"real : {real}";
        fakeBulletTxt.text = $"fake : {fake}";
    }

    //재장전=============

    //플레이어
    void player()
    {
        anim.SetBool("gunplayer", true);
    }

    void Playerf()
    {
        anim.SetBool("gunplayer", false);
    }

    public void EnemySelf()
    {
        if (bullet != 0)
        {
            Debug.Log(shotgunBullit[1]);
            int nowBullit;
            int random = Random.Range(0, bullet);
            nowBullit = shotgunBullit[random];
            shotgunBullit.RemoveAt(random);
            anim.Play("gunEnemySelf");
            bullet--;
            if (nowBullit == 1)
            {
                Debug.Log("aya"); //죽는 애니메이션, hp1--
                Enemyd.Play("EnemyDie");
                audioSource.PlayOneShot(shotClip);
                em1--;
            }
            
            if (nowBullit == 0)
            {
                Debug.Log("aya"); //죽는 애니메이션, hp1--
                Enemyd.Play("EnemyDie");
                audioSource.PlayOneShot(shotClip);
            }
            else
            {
                Debug.Log("an aya");
                audioSource.PlayOneShot(noShotClip);
            }
        }
        else
        {
            reLoad();
        }
    }

    public void EnemyShot()
    {
        if (bullet != 0)
        {
            Debug.Log(shotgunBullit[1]);
            int nowBullit;
            int random = Random.Range(0, bullet);
            nowBullit = shotgunBullit[random];
            shotgunBullit.RemoveAt(random);
            anim.Play("gunEnemyShot");
            bullet--;
            Enemyf();
            if (nowBullit == 1)
            {
                Debug.Log("aya"); //죽는 애니메이션, hp1--
                audioSource.PlayOneShot(shotClip);
                selfdie();
                isEnemy = false;
                pr1--;
                Debug.Log(em1);
                player();
            }
            
            if (nowBullit == 0)
            {
                Debug.Log("aya"); //죽는 애니메이션, hp1--
                audioSource.PlayOneShot(shotClip);
                selfdie();
                isEnemy = false;
                pr1--;
                Debug.Log(em1);
                player();
            }
            else
            {
                Debug.Log("an aya");
                audioSource.PlayOneShot(noShotClip);
                player();
            }
        }
        else
        {
            reLoad();
        }
    }

    //공격
    public void shot()
    {
        if (bullet != 0)
        {
            Debug.Log(shotgunBullit[1]);
            int nowBullit;
            int random = Random.Range(0, bullet);
            nowBullit = shotgunBullit[random];
            shotgunBullit.RemoveAt(random);
            Playerf();
            anim.Play("gunShot");

            bullet--;
            if (nowBullit == 1)
            {
                Debug.Log("aya"); //적 죽는 애니메이션, hp1--
                Enemyd.Play("EnemyDie");
                audioSource.PlayOneShot(shotClip);
                isEnemy = true;
                em1--;
                Debug.Log(em1);
                Enemy();
            }
            
            if (nowBullit == 0)
            {
                Debug.Log("aya"); //적 죽는 애니메이션, hp1--
                audioSource.PlayOneShot(shotClip);
                Enemyd.Play("EnemyDie");
                em1--;
                Debug.Log(em1);
                isEnemy = true;
                Enemy();
            }
            else
            {
                Debug.Log("an aya");
                audioSource.PlayOneShot(noShotClip);
                isEnemy = true;
                Enemy();
            }
        }
        else
        {
            reLoad();
        }
    }

    // 버튼을 숨기는 함수
    public void HideButton()
    {
        button.gameObject.SetActive(false);
    }

    // 버튼을 보이게 하는 함수
    public void ShowButton()
    {
        button.gameObject.SetActive(true);
    }

    //셀프 샷
    public void HideButton2()
    {
        button2.gameObject.SetActive(false);
    }

    // 버튼을 보이게 하는 함수
    public void ShowButton2()
    {
        button2.gameObject.SetActive(true);
    }
    //공격 
    //총알 뽑기

    //자신 쏘기
    public void self()
    {
        if (bullet != 0)
        {
            Debug.Log(shotgunBullit[1]);
            int nowBullit;
            int random = Random.Range(0, bullet);
            nowBullit = shotgunBullit[random];
            shotgunBullit.RemoveAt(random);
            anim.Play("gunSelfShotIdle");
            bullet--;
            Playerf();
            if (nowBullit == 1)
            {
                Debug.Log("aya"); //죽는 애니메이션, hp1--
                audioSource.PlayOneShot(shotClip);
                Invoke(nameof(selfdie), 1.5f);
                pr1--;
                Debug.Log(pr1);
                isEnemy = true;
                Enemy();
            }

            if (nowBullit == 0)
            {
                Debug.Log("aya"); //죽는 애니메이션, hp1--
                Invoke(nameof(selfdie), 1.5f);
                pr1--;
                Debug.Log(pr1);
                isEnemy = true;
                Enemy();
            }
            else
            {
                Debug.Log("an aya");
                audioSource.PlayOneShot(noShotClip);
                player();
            }
        }
        else
        {
            reLoad();
        }
    }


    void selfdie()
    {
        playerd.Play("Player");
        audioSource.PlayOneShot(shotClip);
    }

    public void win()
    {
        SceneManager.LoadScene("Scenes/Win");
    }

    public void lose()
    {
        SceneManager.LoadScene("Scenes/Lose");
    }
    //자신 쏘기

    //플레이어
}