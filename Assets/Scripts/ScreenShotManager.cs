using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotManager : MonoBehaviour {

    public Camera Cam;
    private MainUIElements mainUIElements;
    private string lastScreenshotPath;
    public RenderTexture renderTexture;
    public GameObject bg;

    private void Start() {
        mainUIElements = FindObjectOfType<MainUIElements>();
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetKeyDown(KeyCode.Space)){
            TakeAShot();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            GetScreenshotImage();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            // choose the margin randomly
            float margin = Random.Range(0.0f, 0.3f);
            // setup the rectangle
            Cam.rect = new Rect(margin, 0.0f, 1.0f - margin * 2.0f, 1.0f);
        }
    }

    public void TakeAShot()
    {
        HideUI(true);
        StartCoroutine("CaptureIt");
    }

    IEnumerator CaptureIt()
    {
        string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        string fileName = "Screenshot" + timeStamp + ".png";
        lastScreenshotPath = fileName;
        ScreenCapture.CaptureScreenshot(lastScreenshotPath);
        yield return new WaitForEndOfFrame();
        HideUI(false);

    }

    /// <summary>
    /// Hides all UI elements to get the clean camera input
    /// </summary>
    /// <param name="on">True when the UI should be hidden; false to show it again</param>
    private void HideUI(bool hide)
    {
        mainUIElements.gameObject.SetActive(!hide);
    }



    public Texture2D GetScreenshotImage()
    {
        string filePath = Application.dataPath + "/../" + lastScreenshotPath;
        Texture2D texture = null;
        byte[] fileBytes;
      
            fileBytes = File.ReadAllBytes(filePath);
            texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
            texture.LoadImage(fileBytes);
        
        Debug.Log(filePath);
        //bg.transform.localScale += new Vector3(Cam.rect.y, 0, Cam.rect.x);
        //Graphics.Blit(texture, renderTexture);
        //bg.GetComponent<MeshRenderer>().material.mainTexture = texture;
        //Cam.targetTexture = renderTexture;
        return texture;
    }






}

