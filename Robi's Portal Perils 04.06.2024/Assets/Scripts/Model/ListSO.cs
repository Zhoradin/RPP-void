using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ListSO", menuName = "ScriptableObjects/ListSO")]
public class ListSO : ScriptableObject
{
    public List<ScriptableObject> scriptableObjects = new List<ScriptableObject>();
}
