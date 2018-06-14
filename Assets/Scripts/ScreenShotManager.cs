using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotManager : MonoBehaviour {

    public Camera Cam;
    private int cullMask;
    private bool isFinish = false;

    private void Start() {
        cullMask = Cam.cullingMask;
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("hallo");
            Cam.cullingMask = 5 << 0;
            ScreenCapture.CaptureScreenshot("Screenshot.png");
            Debug.Log("ScreenShot");
            StartCoroutine(WaitForIt());
        }
	}

    IEnumerator WaitForIt(){

        yield return new WaitForSeconds(1);
        Cam.cullingMask = cullMask;
        Debug.Log("CullMask: " + cullMask);
    }
}

