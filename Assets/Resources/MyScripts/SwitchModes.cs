using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchModes : MonoBehaviour
{
    [SerializeField]
    private GameObject sculptingMode;
    [SerializeField]
    private GameObject drawingMode;
    private bool switchMode = false;
    [SerializeField]
    private Material sculpMat;
    [SerializeField]
    private Material drawMat;
    [SerializeField]
    private MeshRenderer switchButtonRenderer;

    public void Switch()
    {
        switchMode = !switchMode;

        if (!switchMode)
        {
            sculptingMode.SetActive(true);
            drawingMode.SetActive(false);
            switchButtonRenderer.material = sculpMat;
        }
        else
        {
            sculptingMode.SetActive(false);
            drawingMode.SetActive(true);
            switchButtonRenderer.material = drawMat;
        }
    }

}
