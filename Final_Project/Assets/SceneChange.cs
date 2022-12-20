using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
	[SerializeField] private GameObject player;
	void Update()
    {
    	if (Vector3.Distance(player.transform.position, transform.position) < 1f)
	    {
	         SceneManager.LoadScene(2);
	    }
    }
}
