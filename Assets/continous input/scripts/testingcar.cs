using RosMessageTypes.Geometry;
using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

public class testingcar : MonoBehaviour
{

    [Header("Basic Values")]
    
    [SerializeField] private float accerlation = 0.2f;
    // Start is called before the first frame update
    [SerializeField]  GameObject teleop_mode;
    Vector2 XYTurning;
    private float movementspeed, accerlationRate;
    Continous_input_gaze controlInput;
    Transform Transform;

    //The below Ros Content are similar to eye_teleoperation
    ROSConnection m_Ros;
    string m_TopicName = "/stretch_diff_drive_controller/cmd_vel";
    bool navigation;
    void Start()
    {

        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.RegisterPublisher<TwistMsg>(m_TopicName);

    }

    // Update is called once per frame
    void Update()
    {  
        accerlationRate = Continous_input_gaze.ExportAcc;
        XYTurning = Continous_input_gaze.ExportV2;
        movementspeed = accerlation;
        //transform.position = transform.position + new Vector3(XYTurning.x+ movementspeed * Time.deltaTime,0, XYTurning.y+ movementspeed * Time.deltaTime);

        if (!navigation) {
        Vector3<FLU> x = new Vector3<FLU>(XYTurning.x + movementspeed,0,0);
        Vector3<FLU> y = new Vector3<FLU>(0,0,XYTurning.x + movementspeed);



            var end_pos = new TwistMsg
            {
                linear = x,
                angular = y
            };
            m_Ros.Publish(m_TopicName,end_pos);
        }

    }

    bool GetNavigationState()
    {
       return navigation = teleop_mode.GetComponent<robot_follower>().nav_;
    }
}
