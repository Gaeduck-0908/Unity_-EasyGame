using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using lib;

// �÷��̾� ���� ��ũ��Ʈ
public class PlayerManager : Singleton<PlayerManager>
{
    // SerializeField ������ �ν����Ϳ��� ���ٰ����ϰ� ���ִ� ���
    // public �̱��� ���� ����,�Լ�
    // private ���� ���� �Լ�

    [SerializeField]
    public GameObject idle; //���ִ� ����
    [SerializeField]
    public GameObject run; //�ٴ� ����
    [SerializeField]
    GameObject jump_up; //���� ����
    [SerializeField]
    GameObject jump_down; //���� ��
    [SerializeField]
    GameObject Roll; //������
    [SerializeField]
    public GameObject atk_1; //���� 1
    [SerializeField]
    GameObject atk_2; //���� 2(��ų)
    [SerializeField]
    GameObject Death; //����

    [SerializeField]
    private Text cool_text; //��ų ��Ÿ�� �ؽ�Ʈ

    [SerializeField]
    private AudioClip atk_sound; //���� �Ҹ�

    [SerializeField]
    private AudioClip jump_sound; //���� �Ҹ�

    [SerializeField]
    public AudioClip hit_sound; //�´� �Ҹ�

    [SerializeField]
    private AudioClip roll_sound; //������ �Ҹ�

    Rigidbody2D rig; //Rigidbody2D ������Ʈ

    public int level; //�÷��̾� ����
    public float exp; //�÷��̾� ����ġ

    public int maxhp; //�÷��̾� �ִ� ü��
    public int hp; //�÷��̾� ���� ü��
    public float speed = 3.0f; //�÷��̾� ���� �ӵ�
    public int atk_dmg; //�÷��̾� ���� ������
    public float atk_speed; //�÷��̾� ���� �ӵ�

    public int lv_1_dmg = 10; //lv1 ������
    public int lv_2_dmg = 20; //lv2 ������
    public int lv_3_dmg = 30; //lv3 ������
    public int lv_4_dmg = 40; //lv4 ������
    public int lv_5_dmg = 60; //lv5 ������

    [SerializeField]
    private GameObject Player_arrow; //�÷��̾� ȭ��

    public bool Player_god; //�÷��̾� ���� ����
    private bool is_ground; //�÷��̾� �� ����ִ��� ����
    private float jump_speed = 8.5f; //�÷��̾� ���� �ӵ�

    private bool is_atk; //�÷��̾� ���� ����
    private bool is_roll; //�÷��̾� ������ ����

    public bool is_spear; //�÷��̾� ���뼦 ����

    private bool cooltime = true; //�÷��̾� ��ų ��Ÿ�� ����
    private int sp_atk_cool = 10; //�÷��̾� ��ų ��Ÿ��
    private bool cooldown = true; //�÷��̾� ���� ��Ÿ�� ����

    private bool is_living; //�÷��̾� ���� ����

    [SerializeField]
    private GameObject[] Enemy_list;  //���� ����Ʈ
    private void Start() //�ʱ⼳��
    {
        rig = GetComponent<Rigidbody2D>(); //������Ʈ ã�� (Rigidzbody2D)
        maxhp = 100; //�ִ�ü�� ����
        level = 1; //���۷��� ����
        exp = 0; //����ġ ����
        hp = 100; //���� ü�� ����
        atk_dmg = 10; //���ݷ� ����
        atk_speed = 1.0f; //���� �ӵ� ����
        Player_god = false; //�������� ����
        is_atk = false; //���ݿ��� ����
        is_spear = false; //���뼦 ���� ����
        cooltime = true; //��ų ��Ÿ�� ����
        is_living = true; //�÷��̾� ���� ���� ����
    }

    private void Update() //������ ���� ����
    {
        Hp_Check(); //ü�� üũ �Լ�
        if(is_living == true) //�÷��̾� ���� ���� Ȯ�� (����ִٸ� ��)
        {
            Player_Move(); //�÷��̾� ������ �Լ�
            Player_Atk(); //�÷��̾� ���� �Լ�
            Player_Lv_Check(); //�÷��̾� ���� üũ �Լ�

            switch(level) //������ ���� ������ ����
            {
                case 1:
                    atk_dmg = lv_1_dmg;
                    break;
                case 2:
                    atk_dmg = lv_2_dmg;
                    break;
                case 3:
                    atk_dmg = lv_3_dmg;
                    break;
                case 4:
                    atk_dmg = lv_4_dmg;
                    break;
                case 5:
                    atk_dmg = lv_5_dmg;
                    break;
            }
        }
        if(sp_atk_cool == 10) //��ų ��Ÿ���� �Ǿ��ٸ�
        {
            cool_text.text = "��"; //�ؽ�Ʈ ����
        }
        else
        {
            cool_text.text = sp_atk_cool.ToString(); //�ƴ϶�� ��ų��Ÿ�� ǥ��
        }
    }

    private void Player_Move() //�÷��̾� ������ �Լ�
    {
        if(is_atk == false) //�������� �ƴҋ�
        {
            StartCoroutine("Player_Roll"); //������ �ڷ�ƾ ����
            if (Input.GetKey(KeyCode.D) && is_roll == false) //D ���������� �����̸鼭 ������ ������
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime); // right �������� speed * ������ ������
                if (is_ground == true) //���� ����ִٸ�
                {
                    //�÷��̾� �ִϸ��̼� ����
                    //flip.x or flip.y = true �Ͻ� ���� false �Ͻ� ������
                    idle.SetActive(false);
                    run.SetActive(true);
                    idle.GetComponent<SpriteRenderer>().flipX = false;
                    run.GetComponent<SpriteRenderer>().flipX = false;
                    jump_up.GetComponent<SpriteRenderer>().flipX = false;
                    jump_down.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            // A �������� �����̸鼭 ������ ������
            else if (Input.GetKey(KeyCode.A) && is_roll == false)
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime); // left �������� speed * ������ ������

                if (is_ground == true) //���� ���������
                {
                    //�÷��̾� �ִϸ��̼� ����
                    //flip.x or flip.y = true �Ͻ� ���� false �Ͻ� ������
                    idle.SetActive(false);
                    run.SetActive(true);
                    idle.GetComponent<SpriteRenderer>().flipX = true;
                    run.GetComponent<SpriteRenderer>().flipX = true;
                    jump_up.GetComponent<SpriteRenderer>().flipX = true;
                    jump_down.GetComponent<SpriteRenderer>().flipX = true;
                }
            }
            // �ƹ� ���� ���ϰ� ��������
            else if (is_ground == true && is_roll == false)
            {
                //�ִϸ��̼� ����
                idle.SetActive(true);
                run.SetActive(false);
                jump_up.SetActive(false);
                jump_down.SetActive(false);
            }

            // ����Ű�� ������
            if (Input.GetKeyDown(KeyCode.Space) && is_roll == false)
            {
                // ���� ����ִ��� üũ
                if (is_ground == true)
                {
                    idle.SetActive(false);
                    run.SetActive(false);

                    // up �������� jump_speed ��ŭ ������ ���� ������
                    rig.AddForce(Vector2.up * jump_speed, ForceMode2D.Impulse);
                    // ���� ��ü,���
                    this.gameObject.GetComponent<AudioSource>().clip = jump_sound;
                    this.gameObject.GetComponent<AudioSource>().Play();

                    jump_up.SetActive(true);

                    is_ground = false;
                }
            }

            //���� ������� ������
            if (is_ground == false)
            {
                // �ִϸ��̼� ����
                if (Input.GetKeyDown(KeyCode.A))
                {
                    jump_up.GetComponent<SpriteRenderer>().flipX = true;
                    jump_down.GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    jump_up.GetComponent<SpriteRenderer>().flipX = false;
                    jump_down.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
        }
    }
    // �÷��̾� ���� �Լ�
    private void Player_Atk()
    {
        // �ڷ�ƾ ����
        StartCoroutine("Player_atk_1");
    }
    
    //�Ҽ� �Ǻ� �Լ�
    private int isPrime(int temp)
    {
        int cnt = 0;

        for (int i = 2; i < temp; i++)
        {
            if(temp % i == 0)
            {
                continue;
            }
            else
            {
                cnt++;
            }
        }

        return cnt;
    }

    //�÷��̾� ���� üũ �Լ�
    private void Player_Lv_Check()
    {
        // ����÷� ����
        if(exp < 0)
        {
            exp = 0;
        }

        // ����ġ ���� ����
        if(exp >= isPrime(100) && level == 1)
        {
            level++;
            exp -= 100;
            GameManager.Instance.ItemPickPanel.SetActive(true);
        }
        else if (exp >= isPrime(1000) && level == 2)
        {
            level++;
            exp -= 1000;
            GameManager.Instance.ItemPickPanel.SetActive(true);
        }
        else if (exp >= isPrime(10000) && level == 3)
        {
            level++;
            exp -= 10000;
            GameManager.Instance.ItemPickPanel.SetActive(true);
        }
        else if (exp >= isPrime(100000) && level == 4)
        {
            level++;
            exp -= 100000;
            GameManager.Instance.ItemPickPanel.SetActive(true);
        }

    }

    //ü�� üũ �Լ�
    private void Hp_Check()
    {
        //ü���� 0���� ������
        if(hp <= 0)
        {
            //�ڷ�ƾ ����
            StartCoroutine("Player_Death");
        }
    }

    //�÷��̾� ���� �ڷ�ƾ
    IEnumerator Player_Death()
    {
        //�ִϸ��̼� ����
        is_living = false;
        idle.SetActive(false);
        run.SetActive(false);
        Death.SetActive(true);

        yield return new WaitForSeconds(1.0f); //1�� ���
        Death.SetActive(false);
        hp -= 100;
        PlayerPrefs.SetInt("HP", hp); // hp ���� HP��� �� ����
        GameManager.Instance.Death_panel.SetActive(true);
        Time.timeScale = 0; //�Ͻ�����
    }

    //�÷��̾� ������ �ڷ�ƾ
    IEnumerator Player_Roll()
    {
        //���� ���������
        if(is_ground == true)
        {
            // ������ Ű �� ������
            if (Input.GetKeyDown(KeyCode.LeftShift) && cooltime == true)
            {
                //�ִϸ��̼� ����
                is_roll = true;
                Roll.SetActive(true);
                idle.SetActive(false);
                run.SetActive(false);

                if (idle.GetComponent<SpriteRenderer>().flipX == true || run.GetComponent<SpriteRenderer>().flipX == true)
                {
                    Roll.GetComponent<SpriteRenderer>().flipX = true;
                    rig.AddForce(Vector2.left * 7.5f, ForceMode2D.Impulse); //���� �������� 7.5 ��ŭ ������
                }
                else
                {
                    Roll.GetComponent<SpriteRenderer>().flipX = false;
                    rig.AddForce(Vector2.right * 7.5f, ForceMode2D.Impulse); //������ �������� 7.5 ��ŭ ������
                }

                //���� ��ü,���
                this.gameObject.GetComponent<AudioSource>().clip = roll_sound; 
                this.gameObject.GetComponent<AudioSource>().Play();

                //������ ����
                cooltime = false;
                Player_god = true;
                yield return new WaitForSeconds(0.5f); //0.5�� ���
                Roll.SetActive(false);
                Player_god = false;
                is_roll = false;
                yield return new WaitForSeconds(1.0f);
                cooltime = true;
            }
        }
    }

    //�÷��̾� ���� �Լ�
    IEnumerator Player_atk_1()
    {
        //������ ����Ű ������
        if (Input.GetKeyDown(KeyCode.RightArrow) && is_ground == true && cooldown == true)
        {
            //�ִϸ��̼� ����
            yield return new WaitForSeconds(0.1f); //�ִϸ��̼� ������ 0.1�� ���
            cooldown = false;
            atk_1.SetActive(true);
            atk_2.SetActive(false);
            idle.SetActive(false);
            run.SetActive(false);

            atk_1.GetComponent<SpriteRenderer>().flipX = false;

            is_atk = true;
            yield return new WaitForSeconds(atk_speed);
            Instantiate(Player_arrow, this.gameObject.transform); //ȭ�� ����
            //���� ��ü,���
            this.gameObject.GetComponent<AudioSource>().clip = atk_sound;
            this.gameObject.GetComponent<AudioSource>().Play();
            atk_1.SetActive(false);
            is_atk = false;
            cooldown = true;
            yield return new WaitForSeconds(0.1f);
        }

        //���� ����Ű ������
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && is_ground == true && cooldown == true)
        {
            yield return new WaitForSeconds(0.1f); //�ִϸ��̼� ������ 0.1�� ���
            cooldown = false;
            atk_1.SetActive(true);
            atk_2.SetActive(false);
            idle.SetActive(false);
            run.SetActive(false);

            atk_1.GetComponent<SpriteRenderer>().flipX = true;

            is_atk = true;
            yield return new WaitForSeconds(atk_speed);
            Instantiate(Player_arrow, this.gameObject.transform); //ȭ�� ����
            this.gameObject.GetComponent<AudioSource>().clip = atk_sound;
            this.gameObject.GetComponent<AudioSource>().Play();
            atk_1.SetActive(false);
            is_atk = false;
            cooldown = true;
            yield return new WaitForSeconds(0.1f);
        }

        //��ų ����
        else if (Input.GetKeyDown(KeyCode.R) && is_ground == true && sp_atk_cool == 10)        
        {
            //�ִϸ��̼� ����
            atk_2.SetActive(true);
            atk_1.SetActive(false);
            idle.SetActive(false);
            run.SetActive(false);
            sp_atk_cool = 0;

            if (idle.GetComponent<SpriteRenderer>().flipX == true || run.GetComponent<SpriteRenderer>().flipX == true)
            {
                atk_2.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                atk_2.GetComponent<SpriteRenderer>().flipX = false;
            }

            is_atk = true;
            Player_god = true;
            yield return new WaitForSeconds(1.5f);
            //���� ��ü,���
            this.gameObject.GetComponent<AudioSource>().clip = atk_sound;
            this.gameObject.GetComponent<AudioSource>().Play();
            atk_2.SetActive(false);
            is_atk = false;
            Player_god = false;
            //��ų ��Ÿ��
            for(int i = 1; i <= 10; i++)
            {
                sp_atk_cool++;
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    //�÷��̾� �浹 ����
    private void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.collider.tag)
        {
            //�ٴڿ� ���������
            case "floor":
                is_ground = true;
                jump_up.SetActive(false);
                jump_down.SetActive(false);
                break;
        }
    }

    //�ÿ��̾� �浹 ����
    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            //���� ���� üũ ������Ʈ
            case "Check_1":
                //�ڷ�ƾ ����
                StartCoroutine("check_1_on");
                break;
            //���� ���� üũ ������Ʈ
            case "Check_2":
                GameManager.Instance.boss.SetActive(true);
                Destroy(col.gameObject);
                break;
            //���� ������ 
            case "i_spear":
                is_spear = true;
                atk_dmg += 10;
                Destroy(col.gameObject);
                break;
            //�� ������(�̼ӻ�����)
            case "i_poision":
                speed += 0.5f;
                Destroy(col.gameObject);
                break;
            //��� ������(�ִ�ü������)
            case "i_apple":
                maxhp += 10;
                Destroy(col.gameObject);
                break;
            //���� ������(ü��ȸ��)
            case "i_postion":
                hp += (hp / 5);
                Destroy(col.gameObject);
                break;
            //���� ������(��������)
            case "i_pie":
                atk_speed -= 0.2f;
                Destroy(col.gameObject);
                break;
        }
    }

    //check1 ������Ʈ �Լ�
    IEnumerator check_1_on()
    {
        GameManager.Instance.Check_1.SetActive(true);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.Check_1.SetActive(false);

        for(int i = 0; i < Enemy_list.Length; i++)
        {
            Enemy_list[i].SetActive(true);
        }
    }
}
