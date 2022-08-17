using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ų ���� ��ũ��Ʈ
public class sp_atk : MonoBehaviour
{
    //�ڷ�ƾ
    IEnumerator Atk()
    {
        yield return new WaitForSeconds(1.5f); //1.5�� ���
    }

    //�浹 ����
    private void OnTriggerExit2D(Collider2D col)
    {
        switch(col.tag)
        {
            // ������ ������
            case "Enemy":
                if (this.gameObject.activeInHierarchy)
                {
                    StartCoroutine("Atk");
                }
                Enemy.Instance.hp -= 100;
                break;
            // ������ ������
            case "Boss":
                if (this.gameObject.activeInHierarchy)
                {
                    StartCoroutine("Atk");
                }
                Boss.Instance.hp -= 200;
                break;
        }
    }
}
