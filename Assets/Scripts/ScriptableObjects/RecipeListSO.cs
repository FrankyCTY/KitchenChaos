using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The reason we have this RecipeListSO with predefined list of recipeSO is for future extensibility.
/// Instead of configuring different recipeSO into the `DeliveryManager` serializableField (list of..)
/// Having a dedicated Scriptable Object allows us to easily reuse the same data in the future if any scrips somehow need to reference it.
/// </summary>

// Preventing it to show up on the asset menu, because we only want to have a single copy of the recipeListSO
// [CreateAssetMenu()]
public class RecipeListSO : ScriptableObject
{
    public List<RecipeSO> recipeSOList;
}
