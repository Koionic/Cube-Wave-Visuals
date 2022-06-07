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

    #region SDF
    
    [Header("Signed Distance Fields")]

    public int sdfIndex;

    public Texture3D defaultSDF;
    
    public List<Texture3D> signedDistanceFields;

    #endregion
    
    #region LiveSDF
    
    [Header("Live SDF")]

    
    public bool liveSDF = false;

    private MeshToSDFBaker sdfBaker;
    
    public List<MeshFilter> liveSDFFilters = new List<MeshFilter>();

    public Mesh liveSDFMesh;
    
    public Mesh bakedSDFMesh;
    
    public int maxResolution = 64;
    public Vector3 center;
    public Vector3 sizeBox;
    public int signPassCount = 1;
    public float threshold = 0.5f;
    
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
        
        if (!defaultSDF)
        {
            defaultSDF = signedDistanceFields[sdfIndex];
        }
        
        if (liveSDF)
        {
            SetUpLiveBake();
            BakeLiveMesh();
        }

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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PrevSDF();
        }
            
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextSDF();
        }

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

        if (orbVFX)
        {
            //orbVFX.SetFloat(sphereDiameterID, sphereDiameter);
            
            if (liveSDF)
            {
                BakeLiveMesh();
            }
            //orbVFX.transform.rotation = objRotation;
            //cube.transform.position = handMidpoint;
        }
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
        
        ResetSDFToDefault();
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

    #region Live SDF Baking

    public void FeedMeshFilters(List<MeshFilter> filters)
    {
        liveSDFFilters.Clear();

        liveSDFFilters = filters;
    }
    
    void SetUpLiveBake()
    {
        bakedSDFMesh = new Mesh();
        sdfBaker = new MeshToSDFBaker(sizeBox, center, maxResolution, bakedSDFMesh, signPassCount, threshold);
        //orbVFX.SetBool("LiveSDFBake", true);
    }

    public void BakeLiveMesh()
    {
        if (liveSDFFilters.Count > 0)
        {

            CombineInstance[] combine = new CombineInstance[liveSDFFilters.Count];

            for (int i = 0; i < liveSDFFilters.Count; i++)
            {
                combine[i].mesh = liveSDFFilters[i].sharedMesh;
                combine[i].transform = liveSDFFilters[i].transform.localToWorldMatrix;
            }

            bakedSDFMesh.CombineMeshes(combine);
            sdfBaker.BakeSDF();
            SetNewSDF(sdfBaker.SdfTexture);
        }
    }

    #endregion
    
    #region Signed Distance Field Handling
    
    public void NextSDF()
    {
        sdfIndex++;
        if (sdfIndex > signedDistanceFields.Count - 1)
        {
            sdfIndex = 0;
        }
        
        SetNewSDF(signedDistanceFields[sdfIndex]);
    }
    
    public void PrevSDF()
    {
        sdfIndex--;
        if (sdfIndex < 0)
        {
            sdfIndex = signedDistanceFields.Count - 1;
        }
        
        SetNewSDF(signedDistanceFields[sdfIndex]);
    }

    public void ResetSDFToDefault()
    {
        SetNewSDF(defaultSDF ? defaultSDF : signedDistanceFields[0]);
    }
    
    
    public void SetNewSDF(RenderTexture newRenderTex)
    {
        if (orbVFX)
        {
            orbVFX.SetTexture("ConformSDF", newRenderTex);
        }
    }
    
    public void SetNewSDF(Texture3D newTex3D)
    {
        if (orbVFX)
        {
            orbVFX.SetTexture("ConformSDF", newTex3D);
        }
    }

    #endregion

}
