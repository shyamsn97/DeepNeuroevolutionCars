using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float velocity;
	public Text score;
	private Rigidbody rb;
	private int count;
	public Text winText;
	private float[][] weights;
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		count = 0;
		winText.text = "";
		setCountText ();
	}

	void FixedUpdate () 
	{ 
		//after physics
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		// Random.seed = 0;
		// Debug.Log(rb.position);
        // Vector3 movement = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
		rb.AddForce (movement * velocity);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Pick Up"))
		{
			other.gameObject.SetActive(false);
			count = count + 1;
			setCountText ();
		}
	}

	void setCountText () 
	{
		score.text = "Count: " + count.ToString ();
		if (count >= 3)
		{
			winText.text = "You Win!";
		}
	}
}
