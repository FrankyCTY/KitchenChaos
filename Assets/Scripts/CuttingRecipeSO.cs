using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{
  public KitchenObjectSO fromObject;
  public KitchenObjectSO toObject;
  public int cuttingProgressMax;
}
