using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public List<GameObject> cameraObjs = new List<GameObject>();
    public GameObject currentCamera;

    public void SwitchCamera()
    {
        Debug.Log("SwitchCamera");
        currentCamera.SetActive(false);
        int cameraIndex = cameraObjs.FindIndex(x => x == currentCamera);
        Debug.Log(cameraIndex);
        if (cameraIndex != cameraObjs.Count - 1)
        {
            Debug.Log("SwitchCameraCondition1");
            currentCamera = cameraObjs[cameraIndex++];
            currentCamera.SetActive(true);

        }
        else
        {
            Debug.Log("SwitchCameraCondition1");
            currentCamera = cameraObjs[0];
            currentCamera.SetActive(true);
        }
    }
}
