using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoseScreenUIScript : MonoBehaviour
{
    [SerializeField] private Transform _loseScreen;
    [SerializeField] private GameObject _loseScreenFirstButton;

    private void Start()
    {
        if (_loseScreen.gameObject.activeInHierarchy)
        {
            _loseScreen.gameObject.SetActive(false);
        }

        GameManager.Instance.OnGameLost += Instance_OnGameLost;
    }

    private void Instance_OnGameLost(object sender, System.EventArgs e)
    {
        PlayLoseScreen();
    }

    private void PlayLoseScreen()
    {
        if(!_loseScreen.gameObject.activeInHierarchy) {
            _loseScreen.gameObject.SetActive(true);
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_loseScreenFirstButton);
    }
}
