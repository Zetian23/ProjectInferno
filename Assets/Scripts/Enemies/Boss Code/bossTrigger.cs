using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
// Code Written By Nathaniel King <3

public class bossTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(gamemanager.instance.bosses[gamemanager.instance.currLevel - 1], transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
