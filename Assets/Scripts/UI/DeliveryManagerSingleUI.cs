using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;
    [SerializeField] private Image remainingTimeBar; 

    private RecipeWithTimer currentRecipeWithTimer;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeWithTimer(RecipeWithTimer recipeWithTimer)
    {
        currentRecipeWithTimer = recipeWithTimer; 
        RecipeSO recipeSO = recipeWithTimer.RecipeSO;

        recipeNameText.text = recipeSO.recipeName;

        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }


        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }

    private void Update()
    {
        if (currentRecipeWithTimer != null)
        {
           
            float normalizedTime = Mathf.Clamp01(currentRecipeWithTimer.remainingTime / currentRecipeWithTimer.maxTime);
            remainingTimeBar.fillAmount = normalizedTime;
        }
    }
}
