using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CredtisMenuUI : MonoBehaviour
{
    public GameObject QrAgustin;
    public GameObject QrJuan;
    public GameObject QrTomas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenQRAgustin()
    {
        QrAgustin.SetActive(true);
        QrJuan.SetActive(false);
        QrTomas.SetActive(false);    
    }
    public void OpenQRJuan()
    {
        QrJuan.SetActive(true);
        QrAgustin.SetActive(false);
        QrTomas.SetActive(false);
    }
    public void OpenQRTomas()
    {
        QrTomas.SetActive(true);
        QrAgustin.SetActive(false);
        QrJuan.SetActive(false);
    }
}
