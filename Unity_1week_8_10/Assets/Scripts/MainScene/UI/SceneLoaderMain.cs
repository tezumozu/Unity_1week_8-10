using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderMain : MonoBehaviour
{
    public void feadOut(){
        SceneManager.LoadScene("TitleScene");
    }
}
