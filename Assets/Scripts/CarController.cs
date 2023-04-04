using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    private CarData carData;
    [SerializeField]
    private GameObject EndGameScreen;


    void Start()
    {
        carData = gameObject.GetComponent<CarData>();
        carData.carVariables.speedData.carRigidBody = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (carData.carVariables.speedData.canMoveTheCar == true)
        {
            moveTheCar(carData.carVariables.speedData.carRigidBody);
        }
        checkBottomPosition();
    }

    private void moveTheCar(Rigidbody carRigidBody)
    {
        carRigidBody.AddRelativeForce(Vector3.right * carData.carVariables.speedData.moveTheCarForce);
    }

    private void checkBottomPosition()
    {
        if (transform.position.y < -5)
        {
            transform.position = new Vector3(transform.position.x, 3, transform.position.z);
            //reset velocity to not fall quick
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("BonusSpeed"))
        {
            carData.carVariables.speedData.carMoveSpeed *= 2;
            Debug.Log("Speedd X2");
            other.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            carData.carVariables.speedData.canMoveTheCar = true;
        }
        else if (other.gameObject.CompareTag("EndGame"))
        {
            Camera.main.transform.rotation = Quaternion.AngleAxis(25f, Vector3.up);
            EndGameScreen.gameObject.SetActive(true);
        }
    }

    public void replayTheGame()
    {
        SceneManager.LoadScene(0);
    }

}
