using System;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{

    [SerializeField] private Hunger hunger;

    private Slider slider = null;
    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    
    private void Update()
    {
        if (!hunger)
            return;
        slider.value = hunger.CurrentHungerNormalized;
    }
}