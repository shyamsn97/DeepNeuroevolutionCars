using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class NeuralNetwork :  IComparable<NeuralNetwork>
{
	private List<float[,]> weights = new List<float[,]>();
	private List<int> seeds = new List<int>();
	private float fitness;
	private int variance = 0;

	public NeuralNetwork(int[] layer_dims,int seed)
	{
		variance = layer_dims[0];
		for(int i = 1; i < layer_dims.Length; i++)
		{
			variance += layer_dims[i];
			float[,] weight_arr = new float[layer_dims[i-1],layer_dims[i]];
			weights.Add(weight_arr);
		}
		variance -= layer_dims[layer_dims.Length - 1];
		addSeed(seed);
		initializeWeights(seed);
	}

	public NeuralNetwork(int[] layer_dims)
	{
		variance = layer_dims[0];
		for(int i = 1; i < layer_dims.Length; i++)
		{
			variance += layer_dims[i];
			float[,] weight_arr = new float[layer_dims[i-1],layer_dims[i]];
			weights.Add(weight_arr);
		}
		variance -= layer_dims[layer_dims.Length - 1];
	}

	public NeuralNetwork copyNetwork(int[] layer_dims, int seed)
	{
		return new NeuralNetwork(layer_dims,seed);
	}

	public void initializeWeights(int seed)
	{
		System.Random random = new System.Random(seed);
		for(int i = 0; i < weights.Count; i++)
    	{
    		for(int j = 0; j < weights[i].GetLength(0); j++)
    		{
    			for(int k = 0; k < weights[i].GetLength(1); k++)
    			{
    				weights[i][j,k] = normSample(random,1.0d);
    			}
    		}
    	}
	}

	public void resetFitness()
	{
		fitness = 0;
	}

	public List<float[,]> getWeights()
	{
		return weights;
	}

	public List<int> getSeeds()
	{
		return seeds;
	}

	public void addSeed(int seed)
	{
		seeds.Add(seed);
	}

	public float getFitness()
	{
		return fitness;
	}
 	
 	public void addFitness(float fit)
    {
        fitness += fit;
    }

    public void setFitness(float fit)
    {
        fitness = fit;
    }

	public void copyParameters(NeuralNetwork net)
	{
		copyWeights(net.getWeights());
		copySeeds(net.getSeeds());
		fitness = net.getFitness();
	}

	public void copySeeds(List<int> new_seeds)
	{
		seeds = new List<int>();
		for(int i = 0; i < new_seeds.Count; i++)
		{
			seeds.Add(new_seeds[i]);
		}
	}

	public void copyWeights(List<float[,]> new_weights)
	{
		weights = new List<float[,]>();
		for(int i = 0; i < new_weights.Count; i++)
		{
			float[,] new_arr = new float[new_weights[i].GetLength(0),new_weights[i].GetLength(1)];
			weights.Add(copy(new_arr,new_weights[i]));
		}

	}

	public float[,] copy(float[,] new_arr, float[,] b)
	{
		for(int i = 0; i < b.GetLength(0); i++)
		{
			for(int j = 0; j < b.GetLength(1); j++)
			{
				new_arr[i,j] = b[i,j];
			}
		}
		return new_arr;
	}

	public int getMaxIndex(float[,] a)
	{
		int max = 0;
		float max_val = a[0,0];
		for(int i = 0; i < a.GetLength(1); i++)
		{
			if(a[0,i] > max_val)
			{
				max_val = a[0,i];
				max = i;
			}
		}
		return max;
	}

	public float[,] dot(float[,] a, float[,] b)
	{
		int a_rows = a.GetLength(0);
		int b_rows = b.GetLength(0);
		int b_columns = b.GetLength(1);
		float[,] ab = new float[a_rows,b_columns];
		for(int i = 0; i < a_rows; i++)
		{
			for(int j = 0; j < b_columns; j++)
			{
				for(int k = 0; k < b_rows; k++)
				{
					ab[i,j] += a[i,k] * b[k,j];
				}
			}
		}
		return ab;
	}

	public float clamp( float value, float min, float max )
	{
    	return (value < min) ? min : (value > max) ? max : value;
	}

	public float[,] Sigmoid(float[,] arr) {
		for(int i = 0; i < arr.GetLength(0); i++)
		{
			for(int j = 0; j < arr.GetLength(1); j++)
			{
				arr[i,j] = 1.0f / (1.0f + (float) Math.Exp(-arr[i,j]));
			}
		}
		return arr;
	}

	public float[,] ReLU(float[,] arr)
	{
		for(int i = 0; i < arr.GetLength(0); i++)
		{
			for(int j = 0; j < arr.GetLength(1); j++)
			{
				arr[i,j] = clamp(arr[i,j],0.0f,float.MaxValue);
			}
		}
		return arr;
	}

	public float[,] TanH(float[,] arr)
	{
		for(int i = 0; i < arr.GetLength(0); i++)
		{
			for(int j = 0; j < arr.GetLength(1); j++)
			{
				arr[i,j] = (float)Math.Tanh(arr[i,j]);
			}
		}
		return arr;
	}

	public float[,] forward(float[,] input)
	{
		float[,] output = input;
		for(int i = 0; i < (this.weights.Count - 1); i++)
		{
			output = ReLU(dot(output,this.weights[i]));
		}

		return TanH(dot(output,this.weights[this.weights.Count - 1])); //[-1,1];
	}

	double GetRandomNumber(System.Random random, double minimum, double maximum)
	{ 
    	return (random.NextDouble() * (maximum - minimum) + minimum);
	}

    public float normSample(System.Random random, double stdev)
    {
    	double u1 = 1.0-random.NextDouble();
    	double u2 = 1.0-random.NextDouble();
		double randStdNormal = Math.Sqrt(-2.0f * Math.Log(u1)) * Math.Sin(2.0f * (Math.PI) * u2); //random normal(0,1)
		// Debug.Log(randStdNormal);
		return  (float)(0.0d + stdev * randStdNormal);
    }

    public void mutate(double scaling)
    {
    	System.Random random_seeder = new System.Random();
    	int seed = random_seeder.Next();
    	addSeed(seed);
    	System.Random random = new System.Random(seed);
    	double std = scaling*Math.Sqrt((double)variance);
    	for(int i = 0; i < this.weights.Count; i++)
    	{
    		for(int j = 0; j < this.weights[i].GetLength(0); j++)
    		{
    			for(int k = 0; k < this.weights[i].GetLength(1); k++)
    			{
    				this.weights[i][j,k] += normSample(random,(1.0/std));
    			}
    		}
    	}
    }

	public int CompareTo(NeuralNetwork other)
    {
        if (other == null) return 1;

        if (fitness > other.fitness)
            return -1;
        else if (fitness < other.fitness)
            return 1;
        else
            return 0;
    }

}


