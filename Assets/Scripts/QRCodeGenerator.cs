using UnityEngine;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;
using System.Threading.Tasks;

public class QRCodeGenerator : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    private Texture2D storeEncodedTexture;

    private void Start()
    {
        storeEncodedTexture = new Texture2D(256, 256);
        EncodeToQR();
    }

    private Color32[] Encode(string dataToEncode, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width,
            }
        };

        return writer.Write(dataToEncode);
    }

    private void EncodeToQR()
    {

        Color32[] convertPixelToTexture = Encode(NetworkUtilities.GetPublicIPv4AddressAsync().Result.ToString(), storeEncodedTexture.width, storeEncodedTexture.height);
        storeEncodedTexture.SetPixels32(convertPixelToTexture);
        storeEncodedTexture.Apply();

        rawImage.texture = storeEncodedTexture;
    }
}
