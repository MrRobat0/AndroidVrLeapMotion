using Leap;
using Leap.Unity;
using UnityEngine;

public class MeshDeformerInput : MonoBehaviour
{

    public float force = 10f;
    public float forceOffset = 0.1f;
    public HandModel handModel;
    public Hand leapHand;
    public LayerMask lm;
    private MeshDeformer deformer;
    GameObject _target;
    Vector3 deformPoint;
    [SerializeField]
    Transform tracker;
    public float radius = 2f;
    bool pickedUp = false;
    bool inTransport = false;

    Transform initTrackParent;



    void Start()
    {
        if (handModel != null)
            leapHand = handModel.GetLeapHand();

        initTrackParent = tracker.transform.parent;

    }


    void Update()
    {
        if (handModel == null) return;
        Vector3 palmPos = handModel.GetPalmPosition();

        if (!pickedUp)
        {
            Debug.Log("Picked up !");
            RaycastHit hit;

            
            Vector3 fwd = handModel.GetPalmNormal();
            Vector3 destination = tracker.position - palmPos;

            if (Physics.Raycast(palmPos, destination, out hit, Mathf.Infinity))
            {
                float distanceToGround = hit.distance;
                Debug.Log("Hitting " + hit);
                Debug.DrawRay(palmPos, destination, Color.red);

                 deformer = hit.collider.GetComponent<MeshDeformer>();
                if (deformer)
                {

                    tracker.SetParent(null);


                    deformPoint = hit.point;
                    deformPoint += hit.normal * forceOffset;
                    pickedUp = true;
                    inTransport = true;

                }
            }

        }

        if(inTransport)
        {
          
            deformer.AddDeformingForce(deformPoint, force * Vector3.Distance(tracker.transform.position, palmPos));
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(tracker.position, radius);
        Gizmos.color = Color.cyan;
    }

    public void setTarget(GameObject target)
    {
    
    }

    public void pickupTarget()
    {

        inTransport = true;

    }

    ////Avoids object jumping when passing from hand to hand.
    //IEnumerator changeParent()
    //{
    //    yield return null;
    //    if (_target != null)
    //        _target.transform.parent = transform;
    //}

    public void releaseTarget()
    {
        pickedUp = false;
        tracker.transform.parent = initTrackParent;
        tracker.transform.localPosition = Vector3.zero;
        inTransport = false;

    }

    public void clearTarget()
    {
        pickedUp = false;
        tracker.transform.parent = initTrackParent;
        tracker.transform.localPosition = Vector3.zero;
        inTransport = false;
    }



}