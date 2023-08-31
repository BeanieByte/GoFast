using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenToggle;

    private void Awake()
    {
        //if (Application.platform == RuntimePlatform.WebGLPlayer)
        //{
        //    fullscreenToggle.interactable = false;
        //}
        //else
        //{
        //    fullscreenToggle.interactable = true;
        //}
    }

    
}
