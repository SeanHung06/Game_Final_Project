using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChoose : MonoBehaviour
{
    public void EasyLevel()
    {
        Debug.Log("Load Easy");
        //SceneManager.LoadScene(3);
    }
    public void HardLevel()
    {
    	Debug.Log("Load Hard");
        //SceneManager.LoadScene(4);
    }
    public void Return(){
    	SceneManager.LoadScene(1);
    }
}
