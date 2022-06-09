using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class KinectHandController : MonoBehaviour
{
    // [Header("Object/VFX References")]
    // public Transform cube;
    //
    // public VisualEffect absorbVFX;
    
    [Header("Kinect Avatar References")]
    public AvatarController _avatarController;
    
    public CubeWave cubeWaveManager;

    
    public Transform leftHandBone;
    public Transform rightHandBone;

    [Header("Hand Parameters")]
    public Quaternion handRotation = Quaternion.identity;
    public float handScale;
    
    [Header("Hand Calculations")]
    public Vector3 leftHandPos;
    public Vector3 rightHandPos;

    public Vector3 handVector;
    public Vector3 handMidpoint;
    public float handDist;

    public float rotSpeed;

    
    [Header("Debug Texts")]
    public TextMeshProUGUI leftHandText;
    public TextMeshProUGUI rightHandText;
    public TextMeshProUGUI handVectorText;
    public TextMeshProUGUI handMidpointText;
    public TextMeshProUGUI handDistText;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!leftHandBone)
        {
            _avatarController.FetchBone(KinectWrapper.NuiSkeletonPositionIndex.HandLeft, out leftHandBone, ref leftHandBone);
        } 
        if (!rightHandBone)
        {
            _avatarController.FetchBone(KinectWrapper.NuiSkeletonPositionIndex.HandRight + 1, out rightHandBone, ref rightHandBone);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        leftHandPos = leftHandBone.transform.position;
        rightHandPos = rightHandBone.transform.position;
        
        handVector = (leftHandPos - rightHandPos);
        //handVector.x *= -1f;
        handDist = Mathf.Clamp((handVector.magnitude * handScale), 0.2f, 10f);

        handMidpoint = Midpoint(leftHandPos, leftHandPos);
        
        handRotation = Quaternion.LookRotation(handVector, transform.up);
        

        
        // if (cube)
        // {
        //     cube.transform.rotation = handRotation;
        //     cube.localScale = Vector3.one * handDist;
        //     //cube.transform.position = handMidpoint;
        // }

        // if (absorbVFX)
        // {
            //absorbVFX.SetVector3("Scale", handScale);
            //absorbVFX.transform.rotation = handRotation;
            //cube.transform.position = handMidpoint;
        //}

        OutputLogs();
    }

    public void GrabValues(ref Vector3 handVector, ref Vector3 handMidpoint, ref float handDist)
    {
        handVector = this.handVector;
        handMidpoint = this.handMidpoint;
        handDist = this.handDist;
    }
    
    void OutputLogs()
    {
        if (leftHandText)
            leftHandText.text = "Left Hand - " + leftHandBone.transform.position;
        
        if (rightHandText)
            rightHandText.text = "Right Hand - " + rightHandBone.transform.position;
        
        if (handVectorText)
            handVectorText.text = "Hand Vector - " + handVector;

        if (handMidpointText)
            handMidpointText.text = "Hand Midpoint " + handMidpoint;
        
        if (handDistText)
            handDistText.text = "Hand Distance - " + handDist;
    }

    Vector3 Midpoint(Vector3 point1, Vector3 point2)
    {
        return new Vector3((point1.x + point2.x) / 2,
                            (point1.y + point2.y) / 2,
                            (point1.z + point2.z) / 2);
    }
}
