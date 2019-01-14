using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.IO;

public class CarsMasterDemonstrate : MonoBehaviour 
{
	
	public GameObject prefab;
	public int[] layers;
	// private
	private int num_successful = 0;
	private int total_num_successful = 0;
	private int current_gen = 0;
	private GameObject car; 
	private List<int> elite_seeds = new List<int>();
	
	void Start() 
	{
		NeuralNetwork net = new NeuralNetwork(layers);
		string[] lines = File.ReadAllLines("saved_seeds/elite_seeds.txt");
		for(int i = 0; i < lines.Length; i++)
		{
			string[] values = lines[i].Split(new[]{","}, StringSplitOptions.None);
			net.mutate(int.Parse(values[0]),double.Parse(values[1]));
		}
  
		car = Instantiate(prefab) as GameObject;
		car.GetComponent<CarController>().assignNet(net);
	}

	public void Quit()
	{
		SceneManager.LoadScene("Menu");
	}

	void FixedUpdate()
	{
		if(car.GetComponent<CarController>().getAlive() == false)
		{
			car.GetComponent<CarController>().resetFitness();
			car.GetComponent<CarController>().resetPosition();
		}
	}
}
