using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class EndLevel : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string levelName = SceneManager.GetActiveScene().name;
            levelName = levelName.Substring(levelName.Length - 1);
            int level = Int32.Parse(levelName) + 1;
            if (level <= 3) {
                SceneManager.LoadScene("Level" + level);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level" + level));
                SceneManager.UnloadSceneAsync("Level" + (level - 1));
            }
            return;
        }
    }
}
