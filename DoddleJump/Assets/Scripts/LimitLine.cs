using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Through")
            collision.gameObject.SetActive(false);
    }
}
