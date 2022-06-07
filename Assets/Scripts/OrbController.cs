using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.SDF;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class OrbController : MonoBehaviour
{
    private VisualEffect orbVFX;

    
    //private KinectManager _kinectManager;

    //public KinectHandController handController;

    private uint User1Id;
    private uint User2Id;
    
    private bool User1Active;
    private bool User2Active;
    
    #region Transform Modifications
    
    [Header("Transform Modifications")]
    
    public Vector3 leftHandPos;
    public Vector3 rightHandPos;

    public Vector3 rotateVector;
    public Vector3 handMidpoint;
    public float sphereDiameter;

    private int sphereDiameterID;
    private int orbColourID;
    
    private Quaternion objRotation;

    #endregion

    #region Colour Modifications

    [Header("Colours")]

    public Vector3 previousColour;
    public Vector3 currentColour;
    public Vector3 targetColour;

    public Color colourToSetOrb;
    
    public List<Color> orbColours;

    private int colourIndex;

    public float timeToLerpColour;
    public float colourLerpTimer;

    private bool colourLerping;
    
    #endregion
    
    #region Auto Pilot

    public bool autoPilotOn;

    public float autoPilotDelay;
    
    public float autoRotateAngle;
    public float autoRotateChange;

    public Vector3 autoPilotPivotChange;
    
    #endregion
    

    // Start is called before the first frame update
    void Start()
    {
        orbVFX = GetComponent<VisualEffect>();
        //_kinectManager = FindObjectOfType<KinectManager>();

        // if (_kinectManager)
        // {
        //     _kinectManager.OnUserAdded.AddListener(OnKinectUserAdd);
        //     _kinectManager.OnUserRemoved.AddListener(OnKinectUserRemove);
        // }
        

        //sphereDiameterID = Shader.PropertyToID("SphereSize");
        //orbColourID = Shader.PropertyToID("OrbColour");
        

        if (orbVFX)
        {
            //sphereDiameter = orbVFX.GetFloat(sphereDiameterID);
        }
        
        
        //Color.RGBToHSV(orbColours[colourIndex], out currentColour.x, out currentColour.y, out currentColour.z);
        //previousColour = currentColour;
    }

    private void Update()
    {
        // if (orbVFX)
        // {
        //     if (colourLerping)
        //     {
        //         colourLerpTimer += Time.deltaTime;
        //
        //         var lerp = colourLerpTimer / timeToLerpColour;
        //
        //         currentColour = Vector3.Lerp(previousColour, targetColour, lerp);
        //
        //         if (colourLerpTimer >= timeToLerpColour)
        //         {
        //             currentColour = targetColour;
        //             previousColour = currentColour;
        //             colourLerping = false;
        //             colourLerpTimer = 0f;
        //         }
        //
        //         colourToSetOrb = Color.HSVToRGB(currentColour.x, currentColour.y, currentColour.z);
        //             
        //         orbVFX.SetVector4(orbColourID, colourToSetOrb);
        //     }
        // }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if (User1Active)
        // { 
        //     // if (handController)
        //     // {
        //     //     handController.GrabValues(ref rotateVector, ref handMidpoint, ref sphereDiameter);
        //     // }
        //     
        //     SetRotation(Quaternion.LookRotation(rotateVector, transform.up));
        // }
        // else if (autoPilotOn)
        // {
        //     autoRotateAngle = (autoRotateAngle + (autoRotateChange * Time.deltaTime)) % 360f;
        //     SetRotation(Quaternion.AngleAxis(autoRotateAngle, rotateVector));
        //
        //     rotateVector.x = Mathf.Sin(Time.time * autoPilotPivotChange.x);
        //     rotateVector.y = Mathf.Sin(Time.time * autoPilotPivotChange.y);
        //     rotateVector.z = Mathf.Sin(Time.time * autoPilotPivotChange.z);
        // }
        //
        // transform.rotation = objRotation;
        
    }

    public void StartLerpingNextColour()
    {
        colourIndex++;

        if (colourIndex >= orbColours.Count)
        {
            colourIndex = 0;
        }

        previousColour = currentColour;        
        Color.RGBToHSV(orbColours[colourIndex], out targetColour.x, out targetColour.y, out targetColour.z);
        
        colourLerpTimer = 0f;
        
        colourLerping = true;
    }
    
    void StartAutoPilot()
    {
        autoPilotOn = true;
    }

    void StopAutoPilot()
    {
        autoPilotOn = false;
    }

    public void SetRotation(Quaternion newQuart)
    {
        objRotation = newQuart;
    }
    
    void OnKinectUserAdd(uint userID)
    {
        if (!User1Active)
        {
            User1Id = userID;
            User1Active = true;
            
            CancelInvoke(nameof(StartAutoPilot));
            StopAutoPilot();
        }
        else if (!User2Active)
        {
            User2Id = userID;
            User2Active = true;
        }
    }

    void OnKinectUserRemove(uint userID)
    {
        if (userID == User2Id)
        {
            //Remove User 2
            User2Active = false;
            User2Id = 0;
        }
        else if (userID == User1Id)
        {
            if (User2Active)
            {
                //User 2 becomes User 1
                User2Active = false;
                User1Id = User2Id;
                User2Id = 0;
            }
            else
            {
                //Remove User 2
                User1Active = false;
                User1Id = 0;

                Invoke(nameof(StartAutoPilot), autoPilotDelay);
            }
        }
    }
}
