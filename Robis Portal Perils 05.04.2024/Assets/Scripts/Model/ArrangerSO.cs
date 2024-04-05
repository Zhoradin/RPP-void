using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ArrangerSO : ScriptableObject
{
    public bool fromNewGameInventory = false;
    public bool fromNewGameDestroyer = false;
    
    public bool fromLoadGame = false;
    public bool fromContinueGame = false;

    public bool inGameNewGameInventory = false;
    public bool inGameNewGameDestroyer = false;

    public bool menuClicked = false;
}
