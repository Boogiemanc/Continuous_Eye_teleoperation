using JetBrains.Annotations;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Continous_input_gaze : MonoBehaviour
{
    [Header("Vector 3 debugging")]
    [SerializeField] private GameObject leftEye;
    [SerializeField] private GameObject eyeGaze;
    [SerializeField] private GameObject centerEyeCamera;
    // Debug Canvas [SerializeField] private GameObject canvas;

    [Header("Canvas joystick testing")]
    [SerializeField] private GameObject joypadOrigin;
    [SerializeField] private GameObject joypadTarget;
    [SerializeField] private GameObject edgeCanvas;
    [SerializeField] private GameObject joystickStop;
    [SerializeField] private GameObject joypadCanvas;




    private TextMeshProUGUI debugText;
    private Camera _centerEyeCamera;
    private Ray rayLeft;
    private float canvasWidth, canvasHeight;
    private RectTransform originCanvasRect, targetCanvasRect, canvasRect, stopRect, edgeRect;
    private Image stoparea_Render;
    public static Vector2 ExportV2;
    public static float ExportAcc;


    private void Start()
    {

        originCanvasRect = joypadOrigin.GetComponent<RectTransform>();
        targetCanvasRect = joypadTarget.GetComponent<RectTransform>();
        stopRect = joystickStop.GetComponent<RectTransform>();
        edgeRect = edgeCanvas.GetComponent<RectTransform>();
        canvasRect = joypadCanvas.GetComponent<RectTransform>();
        /////Rect TRansform 
        stoparea_Render = joystickStop.GetComponent<Image>();
    
        _centerEyeCamera = centerEyeCamera.GetComponent<Camera>();
        canvasWidth = canvasRect.rect.width;
        canvasHeight = canvasRect.rect.height;
        //DEBUG CANVAS
        //    debugText = canvas.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        rayLeft = leftEye.GetComponent<RayInteractor>().Ray;
        ExportV2 = exportXYDirection(new Vector2(0, 0));


        if (Physics.Raycast(rayLeft, out RaycastHit hitDataLeft))
        {
            Vector2 point_Testing;
            Vector3 point = hitDataLeft.point;
            Vector2 currentPt = targetCanvasRect.anchoredPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
              canvasRect, _centerEyeCamera.WorldToScreenPoint(point), _centerEyeCamera, out point_Testing);
            targetCanvasRect.anchoredPosition = new(point_Testing.x, point_Testing.y);
            if (isInAreaX(currentPt,canvasRect)&& isInAreaY(currentPt, canvasRect))
            {
                

                DisplayData(currentPt);
            };
           
            
        }

        //to DO : Make the area stop



    }

    private Vector2 GetClosestPointOnEdge(Vector2 _curentPoint)
    {
        Vector2 currentPoint = _curentPoint;
        Vector2 closest_point;
        float closest_point_x, closest_point_y;
        float halfWidth = canvasWidth / 2;
        float halfHeight = canvasHeight / 2;

        Vector2 normalizedPoint = new Vector2(
            currentPoint.x / halfWidth,
            currentPoint.y / halfHeight
        );
        //check which edge is the closest
        if (Mathf.Abs(normalizedPoint.x) > Mathf.Abs(normalizedPoint.y))//checks if its close to horizontal or vertical
        {
            closest_point_x = Mathf.Sign(normalizedPoint.x) * halfWidth;
            closest_point_y = halfHeight * normalizedPoint.y / Mathf.Abs(normalizedPoint.x);


        }
        else
        {
            closest_point_x = halfWidth * normalizedPoint.x / Mathf.Abs(normalizedPoint.y);
            closest_point_y = Mathf.Sign(normalizedPoint.y) * halfHeight;

        }
        closest_point = new Vector2(
                closest_point_x, closest_point_y

            );
        return closest_point;
    }


    private void DisplayData(Vector2 _curentPoint)
    {
        Vector2 currentPoint = _curentPoint;
        Vector2 originPoint = originCanvasRect.anchoredPosition;
        Vector2 stopArea = GetWidthHeight(stopRect, canvasWidth, canvasHeight);
        Vector2 heading = currentPoint - originPoint;
        float gazeMagnitude = heading.magnitude;
        Vector2 direction = heading / gazeMagnitude;
        stoparea_Render.color = Color.white;
        Vector2 closest_point = GetClosestPointOnEdge(currentPoint);
        float accerlation_scale = (gazeMagnitude / (closest_point - originPoint).magnitude); // current location / the total length of the edge and origin point


        if (isInAreaX(currentPoint, stopRect) && isInAreaY(currentPoint, stopRect))
        {
            accerlation_scale = (1 - (gazeMagnitude / (closest_point - originPoint).magnitude)) * -1;
            
            if (accerlation_scale <= -0.6)
            {
                direction = new Vector2(0, 0);
                stoparea_Render.color = Color.yellow;
            }

        }

        ExportAcc = exportaccerlation(accerlation_scale);
        ExportV2 = exportXYDirection(direction);




        edgeRect.anchoredPosition = new Vector2(closest_point.x, closest_point.y); //map the edgepoint for better debuging
      /*  debugText.text =
                         $"eyeGaze_Coords: {currentPoint}\n" +
                         $"accerlation: {accerlation_scale}\n" +
                         $"Direction: {direction}\n" +
                          $"stop area{stopArea}\n"
                       ; */
    }

        








    //export values to car


    Vector2 exportXYDirection(Vector2 _direction)
    {
        return _direction;
    }
    float exportaccerlation(float _accerlation)
    {
        return _accerlation;
    }
    //////Boolean check//////
    private static bool isInAreaX(Vector2 _currentPT, RectTransform _stopArea)
    {
        bool isInSquareX = false;
        if (Mathf.Abs(_currentPT.x) < _stopArea.rect.width / 2)
        {

            isInSquareX = true;
        }



        return isInSquareX;
    }
    private static bool isInAreaY(Vector2 _currentPT, RectTransform _stopArea)
    {
        bool isInSquareY = false;
        if (Mathf.Abs(_currentPT.y) < _stopArea.rect.height / 2)
        {

            isInSquareY = true;
        }



        return isInSquareY;
    }

    ///getwidth , getheight
    private static Vector2 GetWidthHeight(RectTransform rt, float Width, float height)
    {
        Debug.Log(rt.rect.size);

        var w = (rt.anchorMax.x - rt.anchorMin.x) * Width + rt.sizeDelta.x;
        var h = (rt.anchorMax.y - rt.anchorMin.y) * height + rt.sizeDelta.y;
        return new Vector2(w, h);

    }
}