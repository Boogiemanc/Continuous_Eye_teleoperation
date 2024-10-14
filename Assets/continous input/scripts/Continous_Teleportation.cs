using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;


public class Continous_Teleportation : MonoBehaviour
{  //ROS CONTENT////
    ROSConnection m_Ros;
    string m_TopicName = "/stretch_diff_drive_controller/cmd_vel";
    public GameObject teleop_mode;
    //////////////////////////////
    [Header("Basic Values")]

    [SerializeField] float speed = 0.5f;
    [SerializeField] float TurningRate = 0.2f;
    [SerializeField] bool ROS_Connection;
    [SerializeField] GameObject children;


    Vector2 XYTurning;
    public static Vector3 newPos, TurnPos, PostRosPos;
    private float tantheta, MovingRate;

    private Rigidbody _childrenRigidbody;

    //The below Ros realted Content are similar to eye_teleoperation.cs
    bool Navigation;


    void Start()
    {

        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.RegisterPublisher<TwistMsg>(m_TopicName);










    }

    // Update is called once per frame
    void Update()
    {
        //bool Is_turnning;

        Navigation = teleop_mode.GetComponent<robot_follower>().nav_;

        if (!Navigation)
        {

            MovingRate = Continous_input_gaze.ExportAcc;
            XYTurning = Continous_input_gaze.ExportV2;

            tantheta = Mathf.Atan2(XYTurning.x, XYTurning.y); //angle calculation here
            Debug.Log(XYTurning);

            if (MovingRate > 0.1f)
            {
                //check if it's moving, the closer to the center ,lesser the velocity


                if (XYTurning.y >= -0.5) //only turns when the robot is moving towards
                {
                    float targetAngle = tantheta;
                    TurnPos = new Vector3(0, targetAngle * TurningRate, 0);

                    if (!ROS_Connection)
                    {
                        _childrenRigidbody.angularVelocity = TurnPos;
                        //DEBUG USE:turn gameobject "children" 
                    }

                }

                newPos = new Vector3(0, XYTurning.y, 0);
                newPos += newPos * speed;

            }
            else
            {
                newPos = Vector3.zero;
                TurnPos = Vector3.zero;

            }



            if (!ROS_Connection)
            {
                children.transform.position += newPos * speed;
            }
            else
            {


                PublishDataToRos(newPos, TurnPos);

            }


        }


    }

    private void PublishDataToRos(Vector3 position, Vector3 rotation)
    {
        Vector3<FLU> x = new Vector3<FLU>(position.y, 0, 0);
        Vector3<FLU> y = new Vector3<FLU>(0, 0, -1f * rotation.y);


        var end_pos = new TwistMsg
        {
            linear = x,
            angular = y

        };
        m_Ros.Publish(m_TopicName, end_pos);


    }

}

















