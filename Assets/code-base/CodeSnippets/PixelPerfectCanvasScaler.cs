using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class PixelPerfectCanvasScaler : MonoBehaviour
{
    PixelPerfectCamera pixelPerfectCamera;
    public CanvasScaler canvasScaler;

    void Start()
    {
        pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
    }

    void LateUpdate()
    {
        canvasScaler.scaleFactor = pixelPerfectCamera.pixelRatio;
    }
}
