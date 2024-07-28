using Oculus.Interaction;
using System.Collections;
using UnityEngine;




public class Tilt_detector : MonoBehaviour
{
    
    // Start is called before the first frame update
    [SerializeField]
    GameObject ArrowGroup_L, ArrowGroup_R, L_EYE, R_EYE, eyeGaze;
    [Header ("Timed Gaze Check")]

    [SerializeField]
    float Time_threshold;
    private Ray GetLeftTar_XRray, GetRightTar_XRray;
    private RayInteractor GetLeftTar_RayInspector, GetRightTar_RayInspector;
    private Vector3 PlayPos;
    
    
    void Awake()
    {
        ArrowGroup_R.SetActive(false);  
        ArrowGroup_L.SetActive(false);
        //hide the arrows when starts
    }




    private void Start()
    {
        if (L_EYE && R_EYE != null)
        {
            GetLeftTar_RayInspector = L_EYE.GetComponent<RayInteractor>();
            GetRightTar_RayInspector = R_EYE.GetComponent<RayInteractor>();
            


        }
        
        StartCoroutine(Sight_Countdown());

    }
    // Update is called once per frame

   static Vector3 returnRC_Pos(Vector3 RC_pos)
    { Vector3 pos = RC_pos;

        return pos;
    }
    void Direction_Detect(RaycastHit RCH_L, Vector3 PlayPos)
    {
        Vector3 heading = RCH_L.point - PlayPos;





        float dir = heading.normalized.x;
        //check head postiton 
        //if it's negative, it's look on the left vice versea 

       // Debug.Log($"heading {heading.normalized}\n");
        if (dir < -0.0f)
        {


        //    Debug.Log("You are looking at LEFT WALL");
            ArrowGroup_L.SetActive(true);
        }


        else if (dir > 0.0f)
        {
        //    Debug.Log("You are looking at RIGHT WALL");
            ArrowGroup_R.SetActive(true);
        }
        else
        {
         //   Debug.Log("You are looking at back/front");
                ArrowGroup_R.SetActive(true);
            ArrowGroup_L.SetActive(true);

        }
    }

    void Tag_Check()
    {
        if (GetLeftTar_RayInspector && GetRightTar_RayInspector != null)
        {
            Physics.Raycast(GetLeftTar_XRray, out RaycastHit hitdata_L);
            Physics.Raycast(GetRightTar_XRray, out RaycastHit hitdata_R);
            // Debug.Log($"hitdata point:{hitdata_L.point}\n");

            //Debug.Log(  $"RAY Cast Point {hitdata_L.point.ToVector3f()}\n");

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
      //  Debug.Log(PlayPos);
        

    }





}

        
    

        
      
       
           
           
         
        

        
  

