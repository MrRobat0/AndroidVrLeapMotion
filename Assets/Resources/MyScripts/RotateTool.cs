using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTool : MonoBehaviour
{

    private float xRot, yRot;
    private float xRotPercent, yRotPercent;
    [SerializeField]
    private SculpTool sculpt;
    private Transform selectedTransform;
    private Transform prevTransform;
    private Quaternion initRotation;


    #region PARAMETER_ACCESS

    public void SetHorizontalValue(float value)
    {
        xRotPercent = value;
    }

    public void SetVerticalValue(float value)
    {
        yRotPercent = value;
    }

    #endregion

    #region ROTATION_LOGIC


    private void Start()
    {
        selectedTransform = transform;
    }

    private void Update()
    {
        if (sculpt != null)
        {
            try
            {
                selectedTransform = sculpt.SelectedTransform();
            }
            catch (NullReferenceException e)
            {
                print("Not focusing on any object");
                throw;
            }


        }

        Quaternion rot = new Quaternion(xRotPercent, yRotPercent, 0, 0);

        selectedTransform.Rotate(Vector3.down, yRotPercent);
        selectedTransform.Rotate(Vector3.right, xRotPercent);

    }


    #endregion


}
