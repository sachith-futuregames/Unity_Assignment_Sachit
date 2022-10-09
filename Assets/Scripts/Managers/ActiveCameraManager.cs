using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public enum ECurrentCam
{
    TPP,
    FPP,
    Air
}
public class ActiveCameraManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerLayer;
    [SerializeField] Image _crosshair;
    [SerializeField] CinemachineBrain CMBrain;
    [SerializeField] CinemachineVirtualCamera TPPCam;
    [SerializeField] CinemachineVirtualCamera FPPCam;
    [SerializeField] CinemachineVirtualCamera AirCam;

    public ECurrentCam CurrentCamera;

    

    private void Awake()
    {
        if(!CMBrain)
        {
            CMBrain = GetComponent<CinemachineBrain>();
        }
        
        TPPCam.enabled = true;
        FPPCam.enabled = false;
        _crosshair.enabled = false;
        AirCam.enabled = false;
        CurrentCamera = ECurrentCam.TPP;
    }

    
    ////Get the Active Player From the GameManager and Set the Camera Accordingly
    public void SetActiveTPPCamera(Transform InTransform)
    {
        TPPCam.Follow = InTransform;
        TPPCam.LookAt = InTransform;
    }

    public void SetActiveAirCamera(Transform InTransform)
    {
        AirCam.Follow = InTransform;
        AirCam.LookAt = InTransform;

    }

    public void SetActiveFPPCamera(Transform InTransform)
    {
        FPPCam.Follow = InTransform;
    }

    public void SetActiveCamera(ECurrentCam InCurrentCam)
    {
        CurrentCamera = InCurrentCam;
        if (InCurrentCam == ECurrentCam.TPP)
        {
            TPPCam.enabled = true;
            FPPCam.enabled = false;
            AirCam.enabled = false;
            _crosshair.enabled = false;
        }
        if(InCurrentCam == ECurrentCam.Air)
        {
            AirCam.enabled = true;
            TPPCam.enabled = false;
            FPPCam.enabled = false;
            _crosshair.enabled = false;
        }
        if (InCurrentCam == ECurrentCam.FPP)
        {
            FPPCam.enabled = true;
            AirCam.enabled = false;
            TPPCam.enabled = false;
            _crosshair.enabled = true;
        }
    }
}
