using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using lib;

//ȭ�� ���� ��ũ��Ʈ
public class Arrow : Singleton<Arrow>
{
    //ȭ���� ���ư� ����
    private Vector2 movedir;

    //ȭ���� �ӵ�
    private float Speed = 11.0f;

    //ȭ���� ������
    public int now_dmg = 0;

    //�ʱⰪ ����
    private void Start()
    {
        // �÷��̾ �������� �����ִ� ���⿡ ���� ȭ���� ���⼳��
        if (PlayerManager.Instance.atk_1.GetComponent<SpriteRenderer>().flipX == true)
        {
            //left
            movedir = Vector2.left;
        }
        else
        {
            //right
            movedir = Vector2.right;
        }

        //������ ����
        switch (PlayerManager.Instance.level)
        {
            case 1:
                now_dmg = PlayerManager.Instance.lv_1_dmg;
                break;
            case 2:
                now_dmg = PlayerManager.Instance.lv_2_dmg;
                break;
            case 3:
                now_dmg = PlayerManager.Instance.lv_3_dmg;
                break;
            case 4:
                now_dmg = PlayerManager.Instance.lv_4_dmg;
                break;
            case 5:
                now_dmg = PlayerManager.Instance.lv_5_dmg;
                break;
        }
    }

    //�����Ӵ��� ����
    private void Update()
    {
        //����������� Speed * ������ ��ŭ �̵�
        transform.Translate(movedir * Speed * Time.deltaTime);
    }

    //�浹 ����
    private void OnTriggerEnter2D(Collider2D col)
    {
        switch(col.tag)
        {
            //���� �浹��
            case "Enemy":
                Debug.Log(now_dmg);
                Enemy.Instance.hp -= now_dmg; //���� ü�°���
                if(PlayerManager.Instance.is_spear == false) //���뼦 false
                {
                    Destroy(this.gameObject); //������Ʈ ����
                }
                break;

            //������ �浹��
            case "Boss":
                Boss.Instance.hp -= now_dmg;
                if(PlayerManager.Instance.is_spear == false)
                {
                    Destroy(this.gameObject);
                }
                break;
            //�ٴ�,�� �浹��
            case "floor":
                Destroy(this.gameObject);
                break;
        }
    }

    //ī�޶󿡼� �������
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject); //������Ʈ ����
    }
}
