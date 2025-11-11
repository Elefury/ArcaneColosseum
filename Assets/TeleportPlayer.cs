using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportPlayer : MonoBehaviour
{
    public Transform player;
    //public Transform startLoc;
    public Transform loc1;
    public XRRayInteractor m_RayInteractor_left;
    public XRRayInteractor m_RayInteractor_right;
    
        public void teleLoc1 () {
               player.position = loc1.position;
               m_RayInteractor_left.gameObject.SetActive(false);
               m_RayInteractor_right.gameObject.SetActive(false);

        }

}
