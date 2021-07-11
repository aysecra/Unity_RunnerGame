using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextScenePlay()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        index = (index < 4) ? index : 0;
        SceneManager.LoadScene(index);
    }
}
