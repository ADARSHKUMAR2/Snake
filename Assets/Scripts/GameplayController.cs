using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;

    public GameObject fruit_Pickup, fruit2_Pickup, bomb_Pickup;

    private float min_X = -4f, max_X = 4.3f, min_Y = -2.1f, max_Y = 2.1f;
    private float z_Pos = 5.78f;

    private Text score_Text;
    private int scoreCount;

    private void Start()
    {
        Invoke("StartSpawning", 0.5f);
        score_Text = GameObject.Find("Score").GetComponent<Text>();
    }
    void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if(instance==null)
        {
            instance = this;
        }
    }

    void StartSpawning()
    {
        StartCoroutine(SpawnPickUps());
    }

    void CancelSpawning()
    {
        CancelInvoke("StartSpawning");
    }

    IEnumerator SpawnPickUps()
    {
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));

        if(Random.Range(0,10)>=2 && Random.Range(0, 10) < 5)
        {
            Instantiate(fruit_Pickup,
                        new Vector3(Random.Range(min_X, max_X), Random.Range(min_Y, max_Y), z_Pos),
                        Quaternion.identity);
        }

        else if (Random.Range(0, 10) >= 5)
        {
            Instantiate(fruit2_Pickup,
                        new Vector3(Random.Range(min_X, max_X), Random.Range(min_Y, max_Y), z_Pos),
                        Quaternion.identity);
        }

        else
        {
            Instantiate(bomb_Pickup,
                        new Vector3(Random.Range(min_X, max_X), Random.Range(min_Y, max_Y), z_Pos),
                        Quaternion.identity);
        }

        Invoke("StartSpawning", 0f);
    }

    public void IncreaseScore()
    {
        scoreCount+=10;
        score_Text.text = "Score: " + scoreCount;
        if (PlayerPrefs.GetFloat("Highscore") < scoreCount)
            PlayerPrefs.SetFloat("Highscore", scoreCount);
    }

    public void IncreaseScore_2()
    {
        scoreCount += 20;
        score_Text.text = "Score: " + scoreCount;
        if (PlayerPrefs.GetFloat("Highscore") < scoreCount)
            PlayerPrefs.SetFloat("Highscore", scoreCount);
    }

    //public void OnDeath()
    //{
    //    if (PlayerPrefs.GetFloat("Highscore") <scoreCount)
    //        PlayerPrefs.SetFloat("Highscore", scoreCount);

    //    //deathMenu.ToggleEndMenu(scoreCount);
    //}
}
