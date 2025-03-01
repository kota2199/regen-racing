using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{

    public enum CarOwner
    {
        Human, AI, Debug
    }

    public CarOwner carOwner;
}
