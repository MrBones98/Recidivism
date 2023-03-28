using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private PlayerControls _playerControls;
    [SerializeField] private GameObject _exitInputText;
    private void OnEnable()
    {
        _playerControls.Enable();
    }
    private void Awake()
    {
        _playerControls = new PlayerControls();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _exitInputText.SetActive(true);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_playerControls.Player.Interact.WasPressedThisFrame())
        {
            SceneLoader.Instance.QuitButtonPressed();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _exitInputText.SetActive(false);
    }
    private void OnDisable()
    {
        _playerControls.Disable();
    }
}
