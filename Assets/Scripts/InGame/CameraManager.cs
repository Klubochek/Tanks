using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public List<GameObject> cameraObjs = new List<GameObject>();
    public GameObject CurrentCamera;

    public void SwitchCamera()
    {
        Debug.Log("SwitchCamera");
        CurrentCamera.SetActive(false);
        int cameraIndex = cameraObjs.FindIndex(x => x == CurrentCamera);
        Debug.Log(cameraIndex);
        if (cameraIndex != cameraObjs.Count - 1)
        {
            Debug.Log("SwitchCameraCondition1");
            CurrentCamera = cameraObjs[cameraIndex++];
            CurrentCamera.SetActive(true);
        }
        else
        {
            Debug.Log("SwitchCameraCondition2");
            CurrentCamera = cameraObjs[0];
            CurrentCamera.SetActive(true);
        }
    }
}
