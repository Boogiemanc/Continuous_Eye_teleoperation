using Oculus.Interaction;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tiltchecknotimed : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject ArrowGroup_L, ArrowGroup_R, L_EYE, R_EYE, eyeGaze;
   // [Header("Timed Gaze Check")]

    //[SerializeField]
    float Time_threshold;
    private Ray  GetRightTar_XRray;
    public static Ray GetLeftTar_XRray;
    private RayInteractor GetLeftTar_RayInspector, GetRightTar_RayInspector;
    private Vector3 PlayPos;
    void Awake()
    {
        ArrowGroup_R.gameObject.SetActive(false);
        ArrowGroup_L.gameObject.SetActive(false);

    }




    private void Start()
    {
        if (L_EYE && R_EYE != null)
        {
            GetLeftTar_RayInspector = L_EYE.GetComponent<RayInteractor>();
            GetRightTar_RayInspector = L_EYE.GetComponent<RayInteractor>();


        }
        

    }
    // Update is called once per frame


    void Direction_Detect(RaycastHit RCH_L, Vector3 PlayPos)
    {
        Vector3 heading = RCH_L.point - PlayPos;





        float dir = heading.normalized.x;

        // Debug.Log($"heading {heading.normalized}\n");
        if (dir < -0.0f)
        {


            //    Debug.Log("You are looking at LEFT WALL");
            ArrowGroup_L.gameObject.SetActive(true);
        }


        else if (dir > 0.0f)
        {
            //    Debug.Log("You are looking at RIGHT WALL");
            ArrowGroup_R.gameObject.SetActive(true);
        }
        else
        {
            //   Debug.Log("You are looking at back/front");
            ArrowGroup_R.gameObject.SetActive(true);
            ArrowGroup_L.gameObject.SetActive(true);

        }
    }

    void Tag_Check()
    {
        if (GetLeftTar_RayInspector && GetRightTar_RayInspector != null)
        {
            Physics.Raycast(GetLeftTar_XRray, out RaycastHit hitdata_L);
            Physics.Raycast(GetRightTar_XRray, out RaycastHit hitdata_R);
            // Debug.Log($"hitdata point:{hitdata_L.point}\n");

            Debug.Log($"RAY Cast Point {hitdata_L.point.ToVector3f()}\n");

            if (hitdata_L.collider)
            {
                if (hitdata_L.collider.CompareTag("sight_boarders"))

                {
                    Direction_Detect(hitdata_L, PlayPos);

                }
                else if (hitdata_L.collider.CompareTag("UI_screens"))
                {

                    ArrowGroup_R.SetActive(false);

                    ArrowGroup_L.SetActive(false);
                }


            }
        }

    }
    IEnumerator Sight_Countdown()
    {

        while (true)
        {
            yield return new WaitForSeconds(Time_threshold);
            Tag_Check();

        }
    }



    void Update()

    {


        GetLeftTar_XRray = L_EYE.GetComponent<RayInteractor>().Ray;
        GetRightTar_XRray = R_EYE.GetComponent<RayInteractor>().Ray;
        PlayPos = eyeGaze.transform.position;
       
        Tag_Check();

    }



}
