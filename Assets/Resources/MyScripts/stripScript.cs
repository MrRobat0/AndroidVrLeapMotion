using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stripScript : MonoBehaviour
{
    private bool available;
    private WebCamTexture backCam;
    private WebCamTexture frontCam;
    private Texture defaultBG;

    public RawImage BG;
    public AspectRatioFitter fitter;

    private void Start()
    {
        defaultBG = BG.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length == 0)
        {
            Debug.Log("no camera detected");
            available = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
           if(!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);

            }
        }

        if(backCam == null)
        {
            Debug.Log("unable to find back camera");
            return;
        }

        backCam.Play();
        BG.texture = backCam;
        available = true;

    }

    private void Update()
    {
        if (!available)
        {
            return;
        }

        float ratio = (float)backCam.width / (float)backCam.height;
        fitter.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        BG.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCam.videoRotationAngle;
        BG.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

    }
}