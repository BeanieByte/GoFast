using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryLevelButton : MonoBehaviour
{
    public void RetryLevel() { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
