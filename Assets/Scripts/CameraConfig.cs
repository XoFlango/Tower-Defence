using UnityEngine;

public class CameraFit : MonoBehaviour
{
    public float referenceWidth = 1080f;   // largura de referência
    public float referenceHeight = 1920f;  // altura de referência

    void Start()
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float referenceRatio = referenceWidth / referenceHeight;

        Camera camera = GetComponent<Camera>();

        if (screenRatio > referenceRatio)
        {
            // Tela mais larga que a referência
            camera.orthographicSize = referenceHeight / 200f;
        }
        else
        {
            // Tela mais estreita que a referência
            float diff = referenceRatio / screenRatio;
            camera.orthographicSize = referenceHeight / 200f * diff;
        }
    }
}