using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeType
{
    private float cost;
    private float valueToAdd;
    private string destination;
    private int numberOfUpgradesAllowed;
  
    public UpgradeType(float Cost, float Value, string Dest, int num)
    {
        cost = Cost;
        valueToAdd = Value;
        destination = Dest;
        numberOfUpgradesAllowed = num;
    }
    public float GetCost()
    {
        return cost;
    }

    public float GetValueToAdd()
    {
        return valueToAdd;
    }

    public string GetDestination()
    {
        return destination;
    }

    public int GetNumberOfUpgradesAllowed()
    {
        return numberOfUpgradesAllowed;
    }

    public float UseUpgrade() // Prior to using this please check if currency is enough to cover cost.
    {
        --numberOfUpgradesAllowed;
        cost *= 2;
        return valueToAdd;
    }

}
