using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using TMPro;
using UnityEngine.UI;

public class QrCodeScanner : MonoBehaviour
{

    [SerializeField] private RawImage _rawImageBackground;

    [SerializeField] private AspectRatioFitter _aspectRatioFitter;

    [SerializeField] private TextMeshProUGUI _texOut;

    [SerializeField] private RectTransform _scanZone;

    private bool _isCamAvaible;

    private WebCamTexture _cameraTexture;

    void Start()
    {
        SetUpCamera();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraRender();
    }

    private void UpdateCameraRender()
    {
        if (_isCamAvaible == false)
        {
            return;
        }

        float ratio = (float) _cameraTexture.width / (float) _cameraTexture.height;
        _aspectRatioFitter.aspectRatio = ratio;
        int orientation = _cameraTexture.videoRotationAngle;
        _rawImageBackground.rectTransform.localEulerAngles = new Vector3(180, 180, orientation);
    }

    private void SetUpCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        
        if (devices.Length == 0)
        {
            _isCamAvaible = false;
            Debug.Log("No Device");
            return;
            ;
        }

        for (int i = 0; i < devices.Length;  i++)
        {
            if (devices[i].isFrontFacing == false)
            {
                _cameraTexture = new WebCamTexture(devices[i].name, (int)_scanZone.rect.width,(int)_scanZone.rect.height);
            }
        }

        if (_cameraTexture == null)
        {
            Debug.Log("Null");
        }
        
        _cameraTexture.Play();
        _rawImageBackground.texture = _cameraTexture;
        _isCamAvaible = true;
    }
    
    public void OnClickScan()
    {
        Scan();
    }

    void Scan()
    {
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            Result result =
                barcodeReader.Decode(_cameraTexture.GetPixels32(), _cameraTexture.width, _cameraTexture.height);
            if (result != null)
            {
                _texOut.text = result.Text;
                Application.OpenURL(result.Text);
            }
            else
            {
                _texOut.text = "Failed to read Qr code";


            }
        }
        catch
        {
            _texOut.text = "Faied in try";
        }
    }
    
    
}
