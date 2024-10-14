using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Continous_input_gaze : MonoBehaviour
{//This script will first translate world point from eyeGaze's Raycast to the canvas on sight,
    //then get the normalised point and export ACC
    [Header("Camera")]
    [SerializeField] private GameObject centerEyeCamera;


    [Header("Canvas joystick testing")]
    [SerializeField] private GameObject joypadOrigin;
    [SerializeField] private GameObject joypadTarget;

    [SerializeField] private GameObject joystickStop;
    [SerializeField] private GameObject joypadCanvas;



    // Debug Canvas
    //[SerializeField] private GameObject canvas;
    //Debug edge point 
    // [SerializeField] private GameObject edgeCanvas;
    // DEBUG USE ONLY
    private TextMeshProUGUI debugText;
    private Camera _centerEyeCamera;
    private Ray rayLeft;
    private float canvasWidth, canvasHeight;
    private RectTransform originCanvasRect, targetCanvasRect, canvasRect, stopRect, edgeRect;
    private Image stoparea_Render;
    public static Vector2 ExportV2, originPoint;
    public static bool ISinarea = false;
    public static bool ISTracticking = false;
    public static float ExportAcc;


    private void Start()
    {
        /////Rect TRansform Part
        originCanvasRect = joypadOrigin.GetComponent<RectTransform>();
        targetCanvasRect = joypadTarget.GetComponent<RectTransform>();
        stopRect = joystickStop.GetComponent<RectTransform>();

        canvasRect = joypadCanvas.GetComponent<RectTransform>();

        stoparea_Render = joystickStop.GetComponent<Image>();

        _centerEyeCamera = centerEyeCamera.GetComponent<Camera>();
        canvasWidth = canvasRect.rect.width;
        canvasHeight = canvasRect.rect.height;



    }

    private void Update()
    {
        rayLeft = Tiltchecknotimed.GetLeftTar_XRray;
        ExportV2 = ExportXYDirection(new Vector2(0, 0));


        if (Physics.Raycast(rayLeft, out RaycastHit hitDataLeft))
        {
            Vector3 point = hitDataLeft.point;
            Vector2 currentPt = targetCanvasRect.anchoredPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
              canvasRect, _centerEyeCamera.WorldToScreenPoint(point), _centerEyeCamera, out Vector2 point_Testing);
            targetCanvasRect.anchoredPosition = new(point_Testing.x, point_Testing.y);
            if (IsInAreaX(currentPt, canvasRect) && IsInAreaY(currentPt, canvasRect))
            {

                ISTracticking = true;

                ExportData2bot(currentPt);
                Debug.Log("Tracking started");


            }
            else
            {
                ISTracticking = false;
                ExportV2 = Vector2.zero;
                ExportAcc = 0f;
            }


        }


        else
        {
            ISTracticking = false;
            ExportV2 = Vector2.zero;
            ExportAcc = 0f;
        }



    }

    private Vector2 GetClosestPointOnEdge(Vector2 _curentPoint)
    {
        Vector2 currentPoint = _curentPoint;
        Vector2 closest_point;
        float closest_point_x, closest_point_y;
        float halfWidth = canvasWidth / 2;
        float halfHeight = canvasHeight / 2;

        Vector2 normalizedPoint = new(
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


    private void ExportData2bot(Vector2 _curentPoint)
    {
        Vector2 currentPoint = _curentPoint;
        originPoint = originCanvasRect.anchoredPosition;
        Vector2 heading = currentPoint - originPoint;
        float gazeMagnitude = heading.magnitude;
        //get the length of the vector
        Vector2 direction = heading / gazeMagnitude;
        //normalise it 

        //stoparea_Render.color = Color.white;
        Vector2 closest_point = GetClosestPointOnEdge(currentPoint);
        float accerlation_scale = (gazeMagnitude / (closest_point - originPoint).magnitude); // current location / the total length of the edge and origin point


        if (IsInAreaX(currentPoint, stopRect) && IsInAreaY(currentPoint, stopRect))
        {
            ISinarea = true;



            ISTracticking = false;

        }
        else
        {
            ISinarea = false;
        }

        ExportAcc = Exportaccerlation(accerlation_scale);
        ExportV2 = ExportXYDirection(direction);







    }










    //export values to Continuous_Teleportation

    Vector2 ExportXYDirection(Vector2 _direction)
    {
        return _direction;
    }
    float Exportaccerlation(float _accerlation)
    {
        return _accerlation;
    }
    //////Boolean check//////
    private static bool IsInAreaX(Vector2 _currentPT, RectTransform _stopArea)
    {
        bool isInSquareX = false;
        if (Mathf.Abs(_currentPT.x) < _stopArea.rect.width / 2)
        {

            isInSquareX = true;
        }



        return isInSquareX;
    }
    private static bool IsInAreaY(Vector2 _currentPT, RectTransform _stopArea)
    {
        bool isInSquareY = false;
        if (Mathf.Abs(_currentPT.y) < _stopArea.rect.height / 2)
        {

            isInSquareY = true;
        }



        return isInSquareY;
    }


}