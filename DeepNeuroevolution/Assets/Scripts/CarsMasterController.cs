﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CarsMasterController : MonoBehaviour 
{
	
	public GameObject prefab;
	public int num_cars;
	public int num_generations;
	public int num_elites;
	public int[] layers;
	public Text generation;
	public Text maxFitness;
	public Text carsSuccess;
	public Text totalSuccess;
	public Text median;
	// private
	private int num_successful = 0;
	private int total_num_successful = 0;
	private int current_gen = 0;
	private GameObject[] carList; 
	void Start () 
	{
		generation.text = "Generation: " + current_gen.ToString() + "\n";
		carList = new GameObject[num_cars];
		System.Random random = new System.Random();
		for(int i = 0; i < num_cars; i++)
		{
			carList[i] = Instantiate(prefab) as GameObject;
			int rand = random.Next();
			carList[i].GetComponent<CarController>().assignNet(new NeuralNetwork(layers,rand));
		}
	}

	void FixedUpdate()
	{
		if(checkAlive() == false)
		{
			resetCars();
			updateNets();
			current_gen++;
			generation.text = "Generation: " + current_gen.ToString() + "\n";
		}
	}

	void resetCars()
	{
		num_successful = 0;
		for(int i = 0; i < num_cars; i++)
		{
			if(carList[i].GetComponent<CarController>().getReached())
			{
				num_successful++;
				total_num_successful++;
			}
			// carList[i].GetComponent<CarController>().resetPosition();
		}
		carsSuccess.text = "Cars Success: " + num_successful.ToString() + "\n";
		totalSuccess.text = "Total Success: " + total_num_successful.ToString() + "\n";
	}

	List<NeuralNetwork> getNets()
	{
		List<NeuralNetwork> nets = new List<NeuralNetwork>();
		for(int i = 0; i < num_cars; i++)
		{
			NeuralNetwork net = carList[i].GetComponent<CarController>().getNet();
			NeuralNetwork copynet = new NeuralNetwork(layers);
			copynet.copyParameters(net);
			nets.Add(copynet);
		}
		return nets;
	}

	void updateNets()
	{
		int scaling = 0;
		if(num_successful > 0)
		{
			scaling = current_gen;
		}
		List<NeuralNetwork> nets = getNets();
		nets.Sort();
		maxFitness.text = "Top Fitness:\n0: " + nets[0].getFitness().ToString() + "\n";
		float median_fitness = nets[nets.Count / 2].getFitness();
		median.text = "Median Fitness: " + median_fitness.ToString() + "\n";
		for(int i = 1; i < num_elites; i++)
		{
			maxFitness.text += i.ToString() + ": " + (nets[i].getFitness().ToString() + "\n");
		}
		System.Random random = new System.Random();
		for(int i = 1; i < num_cars; i++)
		{
			int index = random.Next(0,num_elites);
			carList[i].GetComponent<CarController>().copyParameters(nets[index]);
			carList[i].GetComponent<CarController>().mutate((scaling+1));
			carList[i].GetComponent<CarController>().resetFitness();
			carList[i].GetComponent<CarController>().resetPosition();
		}
		carList[0].GetComponent<CarController>().copyParameters(nets[0]);
		carList[0].GetComponent<CarController>().resetFitness();
		carList[0].GetComponent<CarController>().resetPosition();
	}

	bool checkAlive()
	{
		for(int i = 0; i < num_cars; i++)
		{
			if(carList[i].GetComponent<CarController>().getAlive() == true) 
			{
				return true;
			}
		}
		return false;
	}
}
