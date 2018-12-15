using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour 
{
  	
  	public Transform[] wheels;
  	public float acceleration = 1.4f;
    public float motorPower = 150.0f;
    public float maxTurn = 25.0f;
    public int life = 1000;
    
    // Renderer rend;
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
		// rend = GetComponent<Renderer>();
  //       rend.enabled = false;
		original_life = life;
		originalPos = new Vector3(-1.0f, -0.4f, 0.0f);
		originalForward = transform.forward;
        myRigidbody = this.gameObject.GetComponent<Rigidbody>();
        myRigidbody.centerOfMass = new Vector3(0, 0.0f, 0.0f);
	}

	void Update()
	{		
		

		// if(Physics.Raycast(LeftSensor,out hit, distance))
		// {
		// 	if(hit.collider.tag == "Wall")
		// 	{
		// 		// Debug.Log(hit.collider.transform.position.x);
		// 	}
		// }

		// if(Physics.Raycast(RightSensor,out hit, distance))
		// {
		// 	if(hit.collider.tag == "Wall")
		// 	{
		// 		Debug.Log(hit.collider.transform.position.x);
		// 	}
		// }

	}

	float GetRandomNumber(System.Random random, double minimum, double maximum)
	{ 
    	return (float)(random.NextDouble() * (maximum - minimum) + minimum);
	}

    // Update is called once per frame
    void FixedUpdate() 
    {
    	// float fitness = 2000.0f*(1.0f/(Vector3.Distance(transform.position,GameObject.FindGameObjectWithTag("Pick Up").transform.position)));
    	// float fitness = 500000.0f / (((float)GameObject.FindGameObjectWithTag("Goal").transform.position.z - (float)transform.position.z));
    	float fitness = (float)transform.position.z*1000;
    	net.setFitness(fitness);
    	float[,] inputarr = new float[1,9];
    	float velocity = 1500f * motorPower * Time.deltaTime;
    	// inputarr[0,0] = velocity;
    	RaycastHit hit;
		Vector3 rowX = new Vector3 (-1.0f,0.1f,0.0f);
		Vector3 position = transform.position + new Vector3(0f,1f,0f);
		Ray LeftSensor = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,0.0f).normalized)*distance);
		Ray FrontSensor = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (0.0f,0.1f,1.0f).normalized)*distance);
		Ray RightSensor = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,0.0f).normalized)*distance);
		Ray FrontLeftSensor25 = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,0.75f).normalized)*distance);
		Ray FrontRightSensor25 = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,0.75f).normalized)*distance);
		Ray FrontLeftSensor50 = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,1.25f).normalized)*distance);
		Ray FrontRightSensor50 = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,1.25f).normalized)*distance);
		Ray FrontLeftSensor75 = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,1.75f).normalized)*distance);
		Ray FrontRightSensor75 = new Ray(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,1.75f).normalized)*distance);
		// Ray frontlight = new Ray(transform.position, rowX);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,0.0f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (0.0f,0.1f,1.0f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,0.0f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,0.75f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,0.75f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,1.25f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,1.25f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (1.0f,0.1f,1.75f).normalized)*distance,Color.green);
		Debug.DrawRay(position + transform.forward * 1.1f, transform.TransformDirection(new Vector3 (-1.0f,0.1f,1.75f).normalized)*distance,Color.green);
		// Debug.Log(transform.forward);
		if(Physics.Raycast(LeftSensor,out hit,distance))
		{
			if(hit.collider.tag == "Wall")
			{
				inputarr[0,0] = (float)(hit.distance);
			}
		}

		if(Physics.Raycast(FrontLeftSensor25,out hit,distance))
		{
			if(hit.collider.tag == "Wall")
			{
				inputarr[0,1] = (float)(hit.distance);
			}
		}

		if(Physics.Raycast(FrontSensor,out hit,distance))
		{
			if(hit.collider.tag == "Wall")
			{
				inputarr[0,2] = (float)(hit.distance);
			}
		}

		if(Physics.Raycast(FrontRightSensor25,out hit,distance))
		{
			if(hit.collider.tag == "Wall")
			{
				inputarr[0,3] = (float)(hit.distance);
			}
		}

		if(Physics.Raycast(RightSensor,out hit,distance))
		{
			if(hit.collider.tag == "Wall")
			{
				inputarr[0,4] = (float)(hit.distance);
			}
		}

		if(Physics.Raycast(FrontLeftSensor50,out hit,distance))
		{
			if(hit.collider.tag == "Wall")
			{
				inputarr[0,5] = (float)(hit.distance);
			}
		}

		if(Physics.Raycast(FrontRightSensor50,out hit,distance))
		{
			if(hit.collider.tag == "Wall")
			{
				inputarr[0,6] = (float)(hit.distance);
			}
		}

		if(Physics.Raycast(FrontLeftSensor75,out hit,distance))
		{
			if(hit.collider.tag == "Wall")
			{
				inputarr[0,7] = (float)(hit.distance);
			}
		}

		if(Physics.Raycast(FrontRightSensor75,out hit,distance))
		{
			if(hit.collider.tag == "Wall")
			{
				inputarr[0,8] = (float)(hit.distance);
			}
		}

    	float[,] prediction = forward(inputarr);
    	// Debug.Log(prediction[0,0].ToString() + " " + prediction[0,1].ToString());
        // instantePower = Input.GetAxis("Vertical") * (1500 * motorPower * acceleration);
        instantePower = prediction[0,0] * (1500 * motorPower * 1.25f);
        net.addFitness(instantePower);
        // instantePower = Clamp(instantePower,0,50000);
        // Debug.Log(instantePower);
        // wheelTurn = Input.GetAxis("Horizontal") * maxTurn;
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
        // getCollider(0).motorTorque = instantePower;
        // getCollider(1).motorTorque = instantePower;
    	}

    	loseLife();
		if(life <= 0)
		{
			gameObject.SetActive(false);
			alive = false;
			net.addFitness(-1000.0f);
			// net.addFitness((float)(original_life - life));
		}

    }

    WheelCollider getCollider(int n)
    {
        return wheels[n].gameObject.GetComponent<WheelCollider>();
    }


    // void OnCollisionEnter(Collision collision)
    // {
    // 	if (collision.gameObject.CompareTag("Car"))
    // 	{
    // 		Physics.IgnoreCollision(Car.collider, collider);
    // 	}
    // }
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Wall"))
		{
			gameObject.SetActive(false);
			alive = false;
			net.addFitness(-1000.0f);
			// net.addFitness(-life*10.0f);
			// count = count + 1;
			// setCountText ();
		}

		if (other.gameObject.CompareTag("Goal"))
		{
			gameObject.SetActive(false);
			alive = false;
			reached = true;
			net.addFitness(10000.0f);
			// net.addFitness(life*100f);
			// count = count + 1;
			// setCountText ();
		}
	}
	// public Transform[] wheels;
	// public float motorpower = 150.0f;
	// public float maxTurn = 25.0f;
	// // Use this for initialization
	// void Start () {
		
	// }
	
	// void FixedUpdate () 
	// { 
	// 	//after physics
	// 	float moveHorizontal = Input.GetAxis("Horizontal");
	// 	float moveVertical = Input.GetAxis("Vertical");
	// 	// Random.seed = 0;
	// 	// Debug.Log(rb.position);
 //        // Vector3 movement = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
 //        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
		
	// 	gameObject.transform.Translate(movement.normalized*Time.deltaTime*motorpower);
	// }
	// // Update is called once per frame
	// // void Update () {
		
	// // }
}








