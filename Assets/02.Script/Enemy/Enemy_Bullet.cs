using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    private Vector2 movedir;

    private int dmg = 10;

    private float speed = 10.0f;

    private void Start()
    {
        this.gameObject.SetActive(true);
        target = GameObject.FindGameObjectWithTag("Player");

        if (target.transform.position.x > this.transform.position.x)
        {
            movedir = Vector2.right;
        }
        else
        {
            movedir = Vector2.left;
        }
    }

    private void Update()
    {
        transform.Translate(movedir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch(col.tag)
        {
            case "Player":
                Debug.Log("�Ѿ� �� : " + dmg + " �÷��̾� ü�� " + PlayerManager.Instance.hp);
                if(PlayerManager.Instance.Player_god == false)
                {
                    PlayerManager.Instance.hp -= dmg;
                }
                PlayerManager.Instance.gameObject.GetComponent<AudioSource>().clip = PlayerManager.Instance.hit_sound;
                PlayerManager.Instance.gameObject.GetComponent<AudioSource>().Play();
                Destroy(this.gameObject);
                break;
            case "floor":
                Destroy(this.gameObject);
                break;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
