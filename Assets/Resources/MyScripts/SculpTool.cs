using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using Unity;

public class SculpTool : MonoBehaviour
{
    [SerializeField]
    private float radius = 1.0f;
    [SerializeField]
    private float pull = 10.0f;
    private MeshFilter unappliedMesh;
    public FallOff fallOff = FallOff.Gauss;
    public HandModel handModel;
    public Hand leapHand;
    [SerializeField]
    LayerMask lm;
    [SerializeField]
    Transform tracker;
    bool brushActive = false;
    private Transform selectedTransform;
    [SerializeField]
    private GameObject paintBrush;


    [Tooltip("Head Shoulder Offset")]
    [SerializeField]
    [Range(-1, 1f)]
    private float headOffset;

    Vector3 fwd;
    Vector3 headPos;
    Vector3 destination;
    Vector3 palmPos;
    MeshFilter filter;
    private List<MeshFilter> historyFilters = new List<MeshFilter>();

    public float Pull
    {
        get
        {
            return pull;
        }

        set
        {
            pull = value;
        }
    }

    public Transform SelectedTransform()
    {
        return selectedTransform;
    }

    void Start()
    {
        historyFilters.Add(filter);

    }

    public void Update()
    {
        // if (handModel == null) return;

        RaycastHit hit;

        if (brushActive)
        {
            paintBrush.SetActive(true);
            palmPos = handModel.GetPalmPosition();
            fwd = handModel.GetPalmNormal();
            headPos = new Vector3(Camera.main.transform.position.x + headOffset, Camera.main.transform.position.y, Camera.main.transform.position.z);
            destination = tracker.position - headPos;

            Debug.Log("BRUSH IS ACTIVE!");
            if (Physics.Raycast(Camera.main.transform.position, destination, out hit, Mathf.Infinity, lm))
            {
                Debug.DrawRay(Camera.main.transform.position, destination, Color.red);

                filter = hit.collider.GetComponent<MeshFilter>();
                if (filter)
                {
                    selectedTransform = hit.collider.transform;
                    Debug.Log("Current object: " + selectedTransform.gameObject.name);

                    if (filter != unappliedMesh)
                    {
                        ApplyMeshCollider();
                        unappliedMesh = filter;
                        //historyFilters.Add(filter);
                    }


                    Vector3 relativePoint = filter.transform.InverseTransformPoint(hit.point);
                    DeformMesh(filter.mesh, relativePoint, pull * Time.deltaTime, radius);

                    paintBrush.transform.position = hit.point;
                }
            }
        }
        else
        {
            paintBrush.SetActive(false);
            ApplyMeshCollider();
            return;
        }
    }

    //public void GoBack()
    //{
    //    if (historyFilters.Count > 1)
    //    {
    //        historyFilters.RemoveAt(historyFilters.Count);
    //        unappliedMesh = historyFilters[historyFilters.Count - 1];

    //    }
    //}

    public void ResetAll()
    {

    }

    public void pickupTarget()
    {
        brushActive = true;
        Debug.Log("pick up works");
    }

    public void releaseTarget()
    {
        brushActive = false;
    }

    private void ApplyMeshCollider()
    {
        if (unappliedMesh && unappliedMesh.GetComponent<MeshCollider>())
        {
            unappliedMesh.GetComponent<MeshCollider>().sharedMesh = unappliedMesh.sharedMesh;
        }

        unappliedMesh = null;
    }

    private float NeedleFalloff(float dist, float inRadius)
    {
        return -(dist * dist) / (inRadius * inRadius) + 1.0f;
    }

    private void DeformMesh(Mesh mesh, Vector3 position, float power, float inRadius)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        float sqrRadius = inRadius * inRadius;

        // Calculate averaged normal of all surrounding vertices	
        Vector3 averageNormal = Vector3.zero;
        for (int i = 0; i < vertices.Length; i++)
        {
            float sqrMagnitude = (vertices[i] - position).sqrMagnitude;
            // Early out if too far away
            if (sqrMagnitude > sqrRadius)
            {
                continue;
            }

            float distance = Mathf.Sqrt(sqrMagnitude);
            float falloff = LinearFalloff(distance, inRadius);
            averageNormal += falloff * normals[i];
        }

        averageNormal = averageNormal.normalized;

        // Deform vertices along averaged normal
        for (int i = 0; i < vertices.Length; i++)
        {
            float sqrMagnitude = (vertices[i] - position).sqrMagnitude;
            // Early out if too far away
            if (sqrMagnitude > sqrRadius)
            {
                continue;
            }

            float distance = Mathf.Sqrt(sqrMagnitude);
            float falloff;
            switch (fallOff)
            {
                case FallOff.Gauss:
                    falloff = GaussFalloff(distance, inRadius);
                    break;
                case FallOff.Needle:
                    falloff = NeedleFalloff(distance, inRadius);
                    break;
                default:
                    falloff = LinearFalloff(distance, inRadius);
                    break;
            }

            vertices[i] += averageNormal * falloff * power;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }



    private static float LinearFalloff(float distance, float inRadius)
    {
        return Mathf.Clamp01(1.0f - distance / inRadius);
    }

    private static float GaussFalloff(float distance, float inRadius)
    {
        return Mathf.Clamp01(Mathf.Pow(360.0f, -Mathf.Pow(distance / inRadius, 2.5f) - 0.01f));
    }
}



public enum FallOff
{
    Gauss,
    Linear,
    Needle
}