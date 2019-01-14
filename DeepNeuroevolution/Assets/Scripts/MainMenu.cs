using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour 
{

	public void StartEvolve() 
	{
		SceneManager.LoadScene("NeuroEvolution");
	}

	public void StartDemonstrate()
	{
		if(File.Exists("saved_seeds/elite_seeds.txt"))
		{
			SceneManager.LoadScene("Demonstrate");
		}
	}

	public void Exit()
	{
		Application.Quit();
	}
}
