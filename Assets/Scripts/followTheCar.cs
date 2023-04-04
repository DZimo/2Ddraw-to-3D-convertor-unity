using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followTheCar : MonoBehaviour
{
    [SerializeField]
    private GameObject theCar;
    void Update()
    {
        gameObject.transform.position = new Vector3(theCar.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
