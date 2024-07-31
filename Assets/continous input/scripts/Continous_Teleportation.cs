using RosMessageTypes.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using UnityEngine.UIElements;

public class Continous_Teleportation : MonoBehaviour
{
    [Header("Basic Values")]

    [SerializeField] float speed = 0.5f;
    [SerializeField] GameObject children;

    // Start is called before the first frame update
    // [SerializeField]  GameObject teleop_mode;
    Vector2 XYTurning;
   public static Vector3 newPos;
    private float tantheta ,accerlationRate, Targetrotation;
    private Quaternion previous_quaternion, targetRotation;
    private Transform _children;
    private Rigidbody _childrenRigidbody;
    private bool InstopArea;
    
    Transform Transform;



    //The below Ros Content are similar to eye_teleoperation
    ROSConnection m_Ros;
    string m_TopicName = "/stretch_diff_drive_controller/cmd_vel";
    bool navigation;
    void Start()
    {

        /*  m_Ros = ROSConnection.GetOrCreateInstance();
          m_Ros.RegisterPublisher<TwistMsg>(m_TopicName);*/
        _children = children.transform;
        _childrenRigidbody = children.GetComponent<Rigidbody>();
        
        


    }

    // Update is called once per frame
    void Update()
    {
        //bool Is_turnning;
        InstopArea = Continous_input_gaze.ISinarea;
        accerlationRate = Continous_input_gaze.ExportAcc;
        XYTurning = Continous_input_gaze.ExportV2;
        //newPos = transform.forward * XYTurning.y + transform.right * XYTurning.x;


        tantheta = Mathf.Atan2(XYTurning.x, XYTurning.y);


        //  Vector3 direction = new Vector3 (XYTurning.x,0,XYTurning.y) ;
        if (accerlationRate>0.15f) {


            
            if (XYTurning.y >= 0)
            {
                float targetAngle =  tantheta;
                targetRotation = Quaternion.Euler(0, targetAngle, 0);
                _childrenRigidbody.angularVelocity = new Vector3 (0,targetAngle*0.1f,0);
            }

            newPos = children.transform.forward * XYTurning.y  ;
        }
        else
        {
            newPos = Vector3.zero;

        }
             
            


           children.transform.position += newPos * speed;
           
           

           
        }

    }
       

            
           
           

        


  






    /* if (!navigation) {
     Vector3<FLU> x = new Vector3<FLU>(XYTurning.x + movementspeed,0,0);
     Vector3<FLU> y = new Vector3<FLU>(0,0,XYTurning.x + movementspeed);



         var end_pos = new TwistMsg
         {
             linear = x,
             angular = y
         };
         m_Ros.Publish(m_TopicName,end_pos);
     }*/








