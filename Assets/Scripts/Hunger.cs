using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hunger : MonoBehaviour
{
    public float CurrentHungerNormalized { get { return currentHunger / maxHunger; } }

    [SerializeField] private float maxHunger = 10f;
    [SerializeField] private float hungerDepletionSpeed = 1f;

    private float currentHunger;

    private void Start()
    {
        currentHunger = maxHunger / 2;
    }

    private void Update()
    {
        DepleteHunger();
    }

    public void DepleteHunger()
    {
        var nextIncrement = hungerDepletionSpeed * Time.deltaTime;
        AffectHunger(-nextIncrement);
    }

    public void AffectHunger(float foodAmount)
    {
        if (currentHunger + foodAmount > maxHunger || currentHunger - foodAmount < 0)
            Die();
        currentHunger += foodAmount;
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public float GetCurrentHunger()
    {
        return currentHunger / maxHunger;
    }


}