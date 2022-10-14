using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField] float obsSpeed;

    void Start()
    {
        Destroy(gameObject, 6f);
    }

    void Update()
    {
        transform.Translate(Vector3.left * obsSpeed * Time.deltaTime, Space.World);
    }
}
