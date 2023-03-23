using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(9, 0);
        Physics.IgnoreLayerCollision(9, 7);
        Physics.IgnoreLayerCollision(9, 8);
        Physics.IgnoreLayerCollision(9, 6);
        Physics.IgnoreLayerCollision(8, 6);
        Physics.IgnoreLayerCollision(8, 7);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
