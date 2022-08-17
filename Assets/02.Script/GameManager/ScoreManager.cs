using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    //���� �ؽ�Ʈ
    [SerializeField]
    private Text scoreText;
    //�̸� �ؽ�Ʈ
    [SerializeField]
    private InputField inputName;

    //����
    private int score;

    //1�� �ؽ�Ʈ
    [SerializeField]
    private Text first_t;

    //2�� �ؽ�Ʈ
    [SerializeField]
    private Text second_t;

    //3�� �ؽ�Ʈ
    [SerializeField]
    private Text third_t;

    private void Start()
    {
        //�ʱ�ȭ �ڵ�
        //PlayerPrefs.DeleteAll();
        /*PlayerPrefs.SetInt("Rank1", 300);
        PlayerPrefs.SetInt("Rank2", 200);
        PlayerPrefs.SetInt("Rank3", 100);

        PlayerPrefs.SetString("Rank1_name", "AAA");
        PlayerPrefs.SetString("Rank2_name", "BBB");
        PlayerPrefs.SetString("Rank3_name", "CCC");*/

        //�Ͻ����� ����
        Time.timeScale = 1;

        //����� ������ �ҷ�����(ü��)
        score = 500 + PlayerPrefs.GetInt("HP");

        //�� �Ǻ��� �ڵ� ����
        if (SceneManager.GetActiveScene().name == "03.End")
        {
            scoreText.text = "Your Score : " + score;
        }

        if(SceneManager.GetActiveScene().name == "04.Ranking" || SceneManager.GetActiveScene().name == "05.Ranking")
        {
            first_t.text = "Ranking Top 1 : " + PlayerPrefs.GetString("Rank1_name") + ":" + PlayerPrefs.GetInt("Rank1") + ".";
            second_t.text = "Ranking Top 2 : " + PlayerPrefs.GetString("Rank2_name") + ":" + PlayerPrefs.GetInt("Rank2") + ".";
            third_t.text = "Ranking Top 3 : " + PlayerPrefs.GetString("Rank3_name") + ":" + PlayerPrefs.GetInt("Rank3") + ".";
        }
    }

    //���� ��ư
    public void submit()
    {
        string temp = inputName.text;

        if(temp.Length > 3)
        {
            temp = temp.Substring(0, 3);
        }

        insert(temp);
        commit();
    }

    //������ ����
    private void insert(string temp)
    {
        //��� ����
        if(PlayerPrefs.GetInt("Rank1") <= score)
        {
            PlayerPrefs.SetInt("Rank3", PlayerPrefs.GetInt("Rank2"));
            PlayerPrefs.SetString("Rank3_name", PlayerPrefs.GetString("Rank2_name"));

            PlayerPrefs.SetInt("Rank2", PlayerPrefs.GetInt("Rank1"));
            PlayerPrefs.SetString("Rank2_name", PlayerPrefs.GetString("Rank1_name"));

            PlayerPrefs.SetInt("Rank1", score);
            PlayerPrefs.SetString("Rank1_name", temp);

        }
        else if (PlayerPrefs.GetInt("Rank2") <= score)
        {
            PlayerPrefs.SetInt("Rank3", PlayerPrefs.GetInt("Rank2"));
            PlayerPrefs.SetString("Rank3_name", PlayerPrefs.GetString("Rank2_name"));

            PlayerPrefs.SetInt("Rank2", score);
            PlayerPrefs.SetString("Rank2_name", temp);
        }
        else if (PlayerPrefs.GetInt("Rank3") <= score)
        {
            PlayerPrefs.SetInt("Rank3", score);
            PlayerPrefs.SetString("Rank3_name", temp);
        }
    }

    //Ŀ��
    private void commit()
    {
        first_t.text = "Ranking Top 1 : " + PlayerPrefs.GetString("Rank1_name") + ":" + PlayerPrefs.GetInt("Rank1") + ".";
        second_t.text = "Ranking Top 2 : " + PlayerPrefs.GetString("Rank2_name") + ":" + PlayerPrefs.GetInt("Rank2") + ".";
        third_t.text = "Ranking Top 3 : " + PlayerPrefs.GetString("Rank3_name") + ":" + PlayerPrefs.GetInt("Rank3") + ".";
    }
}
