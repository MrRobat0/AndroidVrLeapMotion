using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CameraToTexture : MonoBehaviour
{
    static WebCamTexture cam;
    public GameObject quad;

    private bool isPlaying = false;

    public void PlayVideo()
    {

        isPlaying = !isPlaying;

        if (!isPlaying)
        {

            quad.SetActive(true);
            cam = new WebCamTexture();
            quad.GetComponent<Renderer>().material.mainTexture = cam;
            cam.Play();
        }
        else
        {
            cam.Stop();
            quad.SetActive(false);


        }


    }



}