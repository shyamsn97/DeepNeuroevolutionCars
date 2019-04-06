using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour 
{
	string m_Path;

	public void StartEvolve() 
	{
		SceneManager.LoadScene("NeuroEvolution");
	}

	public void StartDemonstrate()
	{
		m_Path = Application.dataPath;
		if(File.Exists(m_Path + "/saved_seeds/elite_seeds.txt"))
		{
			SceneManager.LoadScene("Demonstrate");
		}
	}

	public void Exit()
	{
		Application.Quit();
	}
}
