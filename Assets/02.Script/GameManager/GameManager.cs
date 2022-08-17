using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using lib;

//���� �Ŵ���
public class GameManager : Singleton<GameManager>
{
    // ���� ���� Ŭ��
    [SerializeField]
    public AudioClip main_bgm;

    [SerializeField]
    public GameObject boss; //���� ������Ʈ
    public AudioClip boss_bgm; //���� ����
    public bool boss_isLiving; //���� ��������

    [SerializeField]
    private AudioClip death_bgm; //�׾����� ����

    [SerializeField]
    private Text level; //���� �ؽ�Ʈ
    [SerializeField]
    private Text exp; //����ġ �ؽ�Ʈ
    [SerializeField]
    private Text hp; //ü�� �ؽ�Ʈ
    [SerializeField]
    private Text dmg; //������ �ؽ�Ʈ
    [SerializeField]
    private Text atks; //���ݼӵ� �ؽ�Ʈ
    [SerializeField]
    private Text speed; //�̵��ӵ� �׽�Ʈ
    [SerializeField]
    private Text now_time; //�÷��̽ð� �ؽ�Ʈ

    public GameObject Check_1;
    //public GameObject Check_2;

    public GameObject Death_panel; //���� �г�

    [SerializeField]
    public GameObject ItemPickPanel;
    // �ִ�ü������,��������,�������,������,HP20%ȸ��
    public GameObject [] ItemList;

    private float dTime = 0; //�ӽ� ����

    private string temp; // �ӽ� ����

    //���� ����
    private void Start()
    {
        boss_isLiving = false;
        //���� ������� ��ü,���
        this.gameObject.GetComponent<AudioSource>().clip = main_bgm;
        this.gameObject.GetComponent<AudioSource>().Play();
    }

    //������ ���� ����
    private void Update()
    {
        SetText();
        if (Death_panel.activeSelf == true)
        {
            //���� ���� ��ü,���
            this.gameObject.GetComponent<AudioSource>().clip = death_bgm;
            this.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    //�ؽ�Ʈ ����
    private void SetText()
    {
        level.text = "Level : " + PlayerManager.Instance.level.ToString();
        if(PlayerManager.Instance.exp < 0)
        {
            PlayerManager.Instance.exp = 0;
        }
        exp.text = "EXP : " + PlayerManager.Instance.exp.ToString() + "%";
        hp.text = "HP : " + PlayerManager.Instance.hp.ToString() + "%";
        dmg.text = "DMG : " + PlayerManager.Instance.atk_dmg.ToString();
        atks.text = "ATKS : " + PlayerManager.Instance.atk_speed.ToString();
        speed.text = "SPEED : " + PlayerManager.Instance.speed.ToString();

        //�÷��� Ÿ�� ���ϴ� �˰���
        dTime += Time.deltaTime;
        if(dTime >= 180f)
        {
            Death_panel.SetActive(true);
        }

        temp = ((int)dTime).ToString();

        now_time.text = temp +" sec";
    }

    //1�� ������
    public void choice_1()
    {
        PlayerManager.Instance.hp += 20;
        if (PlayerManager.Instance.hp > PlayerManager.Instance.maxhp)
        {
            PlayerManager.Instance.hp = 100;
        }
        ItemPickPanel.SetActive(false);
    }
    //2�� ������
    public void choice_2()
    {
        PlayerManager.Instance.maxhp += 30;
        ItemPickPanel.SetActive(false);
    }
    //3�� ������
    public void choice_3()
    {
        PlayerManager.Instance.atk_speed -= 0.2f;
        ItemPickPanel.SetActive(false);
    }
    //���� ��ư ������
    public void nextBtn()
    {
        PlayerPrefs.SetInt("HP", PlayerManager.Instance.hp);
        SceneManager.LoadScene("03.End");
    }
}
