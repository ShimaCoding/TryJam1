using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mg_gameManager : MonoBehaviour
{
    public void Start () {
        Time.timeScale = 0;
    }

    public void StartGame () {
        Time.timeScale = 1;
    }
}
