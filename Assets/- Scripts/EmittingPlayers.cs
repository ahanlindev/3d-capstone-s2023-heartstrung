using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmittingPlayers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(1).SetParent(null);
        transform.GetChild(0).SetParent(null);
    }


}
