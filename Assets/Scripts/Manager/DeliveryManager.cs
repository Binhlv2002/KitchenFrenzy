using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DeliveryManager : NetworkBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeWithTimer> waitingRecipesWithTimers;
    private float spawnRecipeTimer = 4f;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successfulRecipesAmount;

    private void Awake()
    {
        Instance = this;
        waitingRecipesWithTimers = new List<RecipeWithTimer>();
    }

    private void Update()
    {
        if (!IsServer) return;

        if (!GameManager.Instance.IsGamePlaying()) return;

        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (GameManager.Instance.IsGamePlaying() && waitingRecipesWithTimers.Count < waitingRecipeMax)
            {
                int waitingRecipeSOIndex = UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count);
                SpawnNewWaitingRecipeClientRpc(waitingRecipeSOIndex);
            }
        }

        for (int i = waitingRecipesWithTimers.Count - 1; i >= 0; i--)
        {
            RecipeWithTimer recipeWithTimer = waitingRecipesWithTimers[i];
            recipeWithTimer.remainingTime -= Time.deltaTime;

            if (recipeWithTimer.remainingTime <= 0f)
            {
                GameManager.Instance.GameOver();
                waitingRecipesWithTimers.RemoveAt(i);
                return;
            }
            UpdateRemainingTimeClientRpc(i, recipeWithTimer.remainingTime);
        }
    }

    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int waitingRecipeSOIndex)
    {
        RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[waitingRecipeSOIndex];
        waitingRecipesWithTimers.Add(new RecipeWithTimer(waitingRecipeSO));

        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }

    [ClientRpc]
    private void UpdateRemainingTimeClientRpc(int index, float remainingTime)
    {
        if (index >= 0 && index < waitingRecipesWithTimers.Count)
        {
            waitingRecipesWithTimers[index].remainingTime = remainingTime;
        }
        else
        {
            Debug.LogError("Index out of range: " + index);
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        for (int i = 0; i < waitingRecipesWithTimers.Count; i++)
        {
            RecipeWithTimer recipeWithTimer = waitingRecipesWithTimers[i];
            RecipeSO waitingRecipeSO = recipeWithTimer.RecipeSO;

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentsMatchesRecipe = true;

                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;

                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        plateContentsMatchesRecipe = false;
                        break;
                    }
                }

                if (plateContentsMatchesRecipe)
                {
                    DeliverCorrectRecipeServerRpc(i);
                    return;
                }
            }
        }

        DeliverIncorrectRecipeServerRpc();
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        List<RecipeSO> recipeSOList = new List<RecipeSO>();
        foreach (RecipeWithTimer recipeWithTimer in waitingRecipesWithTimers)
        {
            recipeSOList.Add(recipeWithTimer.RecipeSO);
        }
        return recipeSOList;
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverIncorrectRecipeServerRpc()
    {
        DeliverIncorrectRecipeClientRpc();
    }

    [ClientRpc]
    private void DeliverIncorrectRecipeClientRpc()
    {
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverCorrectRecipeServerRpc(int waitingRecipesWithTimersIndex)
    {
        DeliverCorrectRecipeClientRpc(waitingRecipesWithTimersIndex);
    }

    [ClientRpc]
    private void DeliverCorrectRecipeClientRpc(int waitingRecipesWithTimersIndex)
    {
        if (waitingRecipesWithTimersIndex >= 0 && waitingRecipesWithTimersIndex < waitingRecipesWithTimers.Count)
        {
            successfulRecipesAmount++;
            waitingRecipesWithTimers.RemoveAt(waitingRecipesWithTimersIndex);

            OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
            OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Debug.LogError("Index out of range: " + waitingRecipesWithTimersIndex);
        }
    }

    public List<RecipeWithTimer> GetWaitingRecipesWithTimers()
    {
        return waitingRecipesWithTimers;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return GameManager.Instance.IsGameOver() ? successfulRecipesAmount : successfulRecipesAmount++;
    }
}
