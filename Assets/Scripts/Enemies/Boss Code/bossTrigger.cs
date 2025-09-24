using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
// Code Written By Nathaniel King <3

public class bossTrigger : MonoBehaviour
{
    [SerializeField] List<GameObject> bosses;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(bosses[gamemanager.instance.currLevel], transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
