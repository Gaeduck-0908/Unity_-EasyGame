using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    //ī�޶� ������ ������Ʈ
    [SerializeField]
    GameObject pos;

    private void LateUpdate()
    {
        //ī�޶� �̵�
        this.gameObject.transform.position = new Vector3(pos.transform.position.x,0,-10);
    }
}
