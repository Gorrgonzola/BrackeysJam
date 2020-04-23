using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisabler : MonoBehaviour
{
    [SerializeField] private Player player;

    private Canvas canvas;

    private void OnEnable()
    {
        player.EatNotification += ToggleUI;
    }

    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    private void ToggleUI()
    {
        canvas.enabled = !canvas.enabled;
    }

    private void OnDisable()
    {
        player.EatNotification -= ToggleUI;
    }
}
