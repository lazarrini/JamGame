using System;
using UnityEngine;

public class ResourcesSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject testObject;


    private void Start()
    {
        DropResource();

    }
    private void DropResource()
    {
        GameObject test = Instantiate(testObject, spawnPoint.position, spawnPoint.rotation);
        test.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 5f, ForceMode2D.Impulse);
    }
}
