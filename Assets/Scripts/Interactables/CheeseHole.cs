using System;
using UnityEngine;

public class CheeseHole : Interactable
{

    [SerializeField] private Transform nextHoleTransform = null;
    [SerializeField] private float foodAmount = 10f;
    [SerializeField] private float depletePerUse = 2f;
    [SerializeField] private float minFoodAmount = 2f;

    private float currentFoodAmount;

    private void Start()
    {
        currentFoodAmount = foodAmount;
    }

    public override void Interact(Player player)
    {
        player.Teleport(nextHoleTransform.position);
        player.EatCheese(currentFoodAmount);
        DecreaseCheeseAmount();
    }

    private void DecreaseCheeseAmount()
    {
        if (currentFoodAmount - depletePerUse < minFoodAmount)
        {
            currentFoodAmount = minFoodAmount;
            return;
        }
        currentFoodAmount -= depletePerUse;
        transform.localScale *= foodAmount / currentFoodAmount;
    }

    private void OnDrawGizmosSelected()
    {
        if (nextHoleTransform != null)
            Gizmos.DrawLine(transform.position, nextHoleTransform.position);
    }

}