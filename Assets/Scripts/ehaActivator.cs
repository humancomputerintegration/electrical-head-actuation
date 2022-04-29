using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ehaActivator : MonoBehaviour
{
    int counter;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        counter += 1;
        if (counter >= 10) GetComponent<ehaController>().enabled = true;
    }
}
