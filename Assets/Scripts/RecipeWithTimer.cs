using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeWithTimer
{
    public RecipeSO RecipeSO { get; private set; } 
    public float remainingTime; 
    public float maxTime; 

  
    public RecipeWithTimer(RecipeSO recipeSO)
    {
        RecipeSO = recipeSO;
        maxTime = recipeSO.timeLimit; 
        remainingTime = maxTime; 
    }
}

