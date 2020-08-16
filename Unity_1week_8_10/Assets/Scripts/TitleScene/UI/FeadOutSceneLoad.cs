using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FeadOutSceneLoad : MonoBehaviour
{
    public void feadOut(){
        SceneManager.LoadScene("MainScene");
    }
}
