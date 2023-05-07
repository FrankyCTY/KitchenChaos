using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
// Custom data object (We can treat it readonly)
// Like a data object that group data and can be used in other script
/**
 * Creating game assets that can be reused across multiple game objects, such as item or weapon templates.
 * Creating game data that can be modified during runtime, such as player stats or game settings.
 * Creating data-driven systems that can be updated without requiring code changes, such as enemy behavior or dialogue systems.
 */
{
    public Transform prefab;
    public Sprite sprite;
    public string objectName;
}
