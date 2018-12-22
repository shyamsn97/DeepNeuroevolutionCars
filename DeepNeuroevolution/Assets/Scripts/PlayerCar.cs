using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour 
{

	public Transform[] wheels;
  	public float acceleration = 1.4f;
    public float motorPower = 150.0f;
    public float maxTurn = 25.0f;
    public GameObject goal;

    bool alive = true;
    Vector3 originalPos;
    Vector3 originalForward;
    float distance = 10.0f;
    float instantePower = 25.0f;
    float brake = 0.0f;
    float wheelTurn = 0.0f;
    Rigidbody myRigidbody;

	public Vector3 getPosition()
	{
		return transform.position;
	}

	public void resetPosition()
	{
		transform.position = originalPos;
		transform.forward = originalForward;
		alive = true;
	}


	// Use this for initialization
	void Start () 
	{
		originalPos = new Vector3(-1.0f, -0.4f, 0.0f);
		originalForward = transform.forward;
        myRigidbody = this.gameObject.GetComponent<Rigidbody>();
        myRigidbody.centerOfMass = new Vector3(0, 0.0f, 0.0f);
        float[,] inp = {{1.0f,2.0f,3.0f}};
	}
    

    // Update is called once per frame
    void FixedUpdate() 
    {
    	float velocity = 1500f * motorPower * Time.deltaTime;
    	RaycastHit hit;
		Vector3 position = transform.position + new Vector3(0f,1f,0f);
		Ray LeftSensor = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.0f,0.0f).normalized)*distance);
		Ray FrontSensor = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (0.0f,0.0f,1.0f).normalized)*distance);
		Ray RightSensor = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.0f,0.0f).normalized)*distance);
		Ray FrontLeftSensor = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.0f,0.5f).normalized)*distance);
		Ray FrontRightSensor = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.0f,0.5f).normalized)*distance);
		
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,0.0f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (0.0f,0.1f,1.0f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,0.0f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,1.75f).normalized)*distance,Color.green);

		if(Physics.Raycast(FrontSensor,out hit,distance))
		{
			if(hit.collider.tag == "Car")
			{
				Debug.Log("Car Detected");
			}
		}

        instantePower = Input.GetAxis("Vertical") * velocity;
        wheelTurn = Input.GetAxis("Horizontal") * maxTurn;
        brake = Input.GetKey("space") ? myRigidbody.mass * 0.1f : 0.0f;

        //turn collider
        getCollider(0).steerAngle = wheelTurn;
        getCollider(1).steerAngle = wheelTurn;

        //turn wheels
        wheels[0].localEulerAngles = new Vector3(wheels[0].localEulerAngles.x,
            getCollider(0).steerAngle - wheels[0].localEulerAngles.z + 90,
            wheels[0].localEulerAngles.z);
        wheels[1].localEulerAngles = new Vector3(wheels[1].localEulerAngles.x,
            getCollider(1).steerAngle - wheels[1].localEulerAngles.z + 90,
            wheels[1].localEulerAngles.z);

        // spin wheels
        wheels[0].Rotate(0, -getCollider(0).rpm / 60 * 360 * Time.deltaTime, 0);
        wheels[1].Rotate(0, -getCollider(1).rpm / 60 * 360 * Time.deltaTime, 0);
        wheels[2].Rotate(0, -getCollider(2).rpm / 60 * 360 * Time.deltaTime, 0);
        wheels[3].Rotate(0, -getCollider(3).rpm / 60 * 360 * Time.deltaTime, 0);

        // //brakes
        if (brake > 0.0f)
        {
            getCollider(0).brakeTorque = brake;
            getCollider(1).brakeTorque = brake;
            getCollider(2).brakeTorque = brake;
            getCollider(3).brakeTorque = brake;
            getCollider(2).motorTorque = 0.0f;
            getCollider(3).motorTorque = 0.0f;
        }
        else
        {
	        getCollider(0).brakeTorque = 0.0f;
	        getCollider(1).brakeTorque = 0.0f;
	        getCollider(2).brakeTorque = 0.0f;
	        getCollider(3).brakeTorque = 0.0f;
	        getCollider(2).motorTorque = instantePower;
	        getCollider(3).motorTorque = instantePower;
    	}
    }

    WheelCollider getCollider(int n)
    {
        return wheels[n].gameObject.GetComponent<WheelCollider>();
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Wall"))
		{
			gameObject.SetActive(false);
			resetPosition();
			gameObject.SetActive(true);
		}
	}
}


