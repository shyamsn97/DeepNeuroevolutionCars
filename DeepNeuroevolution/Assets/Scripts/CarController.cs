using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour 
{
  	/*
  		Car Controller Object
  	*/
  	public Transform[] wheels;
  	public float acceleration = 1.4f;
    public float motorPower = 150.0f;
    public float maxTurn = 25.0f;
    public int life = 1000;
    
    int original_life;
    Vector3 originalPos;
    Vector3 originalForward;
    bool reached = false;
    bool alive = true;
    float distance = 10.0f;
    float instantePower = 25.0f;
    float brake = 0.0f;
    float wheelTurn = 0.0f;
    Rigidbody myRigidbody;
    NeuralNetwork net;



	// Use this for initialization
	public void copyParameters(NeuralNetwork target)
	{
		net.copyParameters(target);
	}

	public void mutate(int scaling)
	{
		net.mutate((double)scaling);
	}

	public void resetFitness()
	{
		net.resetFitness();
	}

	public bool getAlive()
	{
		return alive;
	}

	public NeuralNetwork getNet()
	{
		return net;
	}

	public bool getReached()
	{
		return reached;
	}

	public float[,] forward(float[,] inputarr)
	{
		return net.forward(inputarr);
	}

	public void assignNet(NeuralNetwork target)
	{
		net = target;
	}

	public Vector3 getPosition()
	{
		return transform.position;
	}

	public void loseLife()
	{
		life--;
	}

	public void resetPosition()
	{
		transform.position = originalPos;
		transform.forward = originalForward;
		alive = true;
		reached = false;
		life = original_life;
		gameObject.SetActive(true);
	}

	void Start () 
	{
		original_life = life;
		originalPos = new Vector3(-1.0f, -0.4f, 0.0f);
		originalForward = transform.forward;
        myRigidbody = this.gameObject.GetComponent<Rigidbody>();
        myRigidbody.centerOfMass = new Vector3(0, 0.0f, 0.0f);
	}


	float GetRandomNumber(System.Random random, double minimum, double maximum)
	{ 
    	return (float)(random.NextDouble() * (maximum - minimum) + minimum);
	}

	float getDistance(Ray ray, float distance)
	{
		RaycastHit[] hits;
		hits =  Physics.RaycastAll(ray,distance);
		float outdistance = 0.0f;
		for(int i = 0; i < hits.Length; i++)
		{
			RaycastHit hit = hits[i];
			if(hit.collider.tag == "Wall")
			{
				outdistance = (hit.distance / distance);
				return outdistance;
			}
		}
		return outdistance;
	}

    // Update is called once per frame
    void FixedUpdate() 
    {
    	float fitness = (float)transform.position.z*1000f;
    	net.setFitness(fitness);
    	float[,] inputarr = new float[1,9];
    	float velocity = 1500f * motorPower * Time.deltaTime;
		Vector3 rowX = new Vector3 (-1.0f,0.1f,0.0f);
		Vector3 position = transform.position + new Vector3(0f,1f,0f);
		//9 sensors
		Ray LeftSensor = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,0.0f).normalized)*distance);
		Ray FrontSensor = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (0.0f,0.1f,1.0f).normalized)*distance);
		Ray RightSensor = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,0.0f).normalized)*distance);
		Ray FrontLeftSensor75 = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,0.75f).normalized)*distance);
		Ray FrontRightSensor75 = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,0.75f).normalized)*distance);
		Ray FrontLeftSensor125 = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,1.25f).normalized)*distance);
		Ray FrontRightSensor125 = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,1.25f).normalized)*distance);
		Ray FrontLeftSensor175 = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,1.75f).normalized)*distance);
		Ray FrontRightSensor175 = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,1.75f).normalized)*distance);
		
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,0.0f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (0.0f,0.1f,1.0f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,0.0f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,0.75f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,0.75f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,1.25f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,1.25f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,1.75f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,1.75f).normalized)*distance,Color.green);

		inputarr[0,0] = getDistance(LeftSensor,distance);
		inputarr[0,1] = getDistance(FrontLeftSensor75,distance);
		inputarr[0,2] = getDistance(FrontLeftSensor125,distance);
		inputarr[0,3] = getDistance(FrontLeftSensor175,distance);
		inputarr[0,4] = getDistance(FrontSensor,distance);
		inputarr[0,5] = getDistance(FrontRightSensor175,distance);
		inputarr[0,6] = getDistance(FrontRightSensor125,distance);
		inputarr[0,7] = getDistance(FrontRightSensor75,distance);
		inputarr[0,8] = getDistance(RightSensor,distance);


    	float[,] prediction = forward(inputarr);

        instantePower = prediction[0,0] * (1500 * motorPower * 1.25f);
        net.addFitness(instantePower);
        wheelTurn =  prediction[0,1] * maxTurn;

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

    	loseLife();
		if(life <= 0)
		{
			gameObject.SetActive(false);
			alive = false;
			net.addFitness(-10000.0f);
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
			alive = false;
			net.addFitness(-10000.0f);

		}

		if (other.gameObject.CompareTag("Goal"))
		{
			gameObject.SetActive(false);
			alive = false;
			reached = true;
			net.addFitness(10000.0f);

		}
	}

}








