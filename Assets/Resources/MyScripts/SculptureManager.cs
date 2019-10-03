using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SculptureManager : MonoBehaviour
{
    //Refresh new sculpture 

    [SerializeField]
    private Mesh cleanSculpture;
    [SerializeField]
    private MeshFilter currentSculpture;


    public void Start()
    {
        CreateSculpture();
    }

    public void CreateSculpture()
    {
        currentSculpture.mesh = cleanSculpture;
    }


}
