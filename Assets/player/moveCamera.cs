using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class moveCamera : MonoBehaviour
{

    public Transform player;

    void Update()
    {
        transform.position = player.transform.position;
    }
}

