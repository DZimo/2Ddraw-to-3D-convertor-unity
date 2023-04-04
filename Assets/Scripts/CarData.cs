using UnityEngine;

[System.Serializable]
public class CarData : MonoBehaviour
{
    [System.Serializable]
    public struct carDataStruct
    {
       public SpeedData speedData;
    }
    public carDataStruct carVariables;

    [System.Serializable]
    public class SpeedData
    {
        public float carMoveSpeed = 500f;
        public Rigidbody carRigidBody;
        public float moveTheCarForce = 1000f;
        public bool canMoveTheCar=false;
    }
}
