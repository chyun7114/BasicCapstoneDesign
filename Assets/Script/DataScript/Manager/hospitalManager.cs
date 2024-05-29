using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hospitalManager : MonoBehaviour
{
    public GameObject player;
    public GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 startPosition = new Vector3(2.577f, 0.18f, -4.371f);
        player.transform.position = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
