using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    PlayerScript player;
    public static int gameComplete = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") && GameObject.FindGameObjectWithTag("Altar").transform.childCount == 0 && player.dors.engaged){
            int currLevelNum = int.Parse(SceneManager.GetActiveScene().name.Substring(6));
            int nextLevelNum = currLevelNum + 1;
            string nextLevel = "Level " + nextLevelNum;
            PlayerPrefs.SetInt("CurrentLevel", nextLevelNum);
            SceneManager.LoadScene(nextLevel);
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        gameComplete = 1;
        PlayerPrefs.SetInt("Complete", 1);
        PlayerPrefs.SetInt("Cutscene", 0);
        PlayerPrefs.SetInt("CurrentLevel", 1);
        SceneManager.LoadScene("End Scene");
    }
}
