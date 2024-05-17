using UnityEngine;
using ZXing;
using UnityEngine.UI;
using System;

public class QRCodeScanner : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private AspectRatioFitter aspectRatioFitter;

    private bool isCamAvailable;
    private WebCamTexture cameraTexture;

    public static Action<string> QRCodeRead;

    private void OnEnable()
    {
        SetupCamera();
    }

    private void SetupCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length == 0)
        {
            isCamAvailable = false;
            return;
        }
        
        for(int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                cameraTexture = new WebCamTexture(devices[i].name);
            }
        }

        cameraTexture.Play();
        rawImage.texture = cameraTexture;
        isCamAvailable = true;
    }

    private void UpdateCameraRender()
    {
        if (!isCamAvailable) return;

        float ratio = (float)cameraTexture.width/(float)cameraTexture.height;
        aspectRatioFitter.aspectRatio = ratio;

        int orientation = -cameraTexture.videoRotationAngle;
        rawImage.rectTransform.localEulerAngles = new Vector3(0f,0f,orientation);
    }

    private void Update()
    {
        UpdateCameraRender();
        Scan();
    }

    private void Scan()
    {
        IBarcodeReader reader = new BarcodeReader();
        Result result = reader.Decode(cameraTexture.GetPixels32(), cameraTexture.width, cameraTexture.height);
        if (result != null)
        {
            Debug.Log("Read QR code: " + result.Text);
            QRCodeRead?.Invoke(result.Text);
        }
    }
}
