using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;


public class Continous_Teleportation : MonoBehaviour
{
    [Header("Basic Values")]

    [SerializeField] float speed = 0.5f;
    [SerializeField] float TurningRate = 0.2f;
    [SerializeField] bool ROS_Connection;
    [SerializeField] GameObject children;


    Vector2 XYTurning;
    public static Vector3 newPos, TurnPos;
    private float tantheta, MovingRate;

    private Rigidbody _childrenRigidbody;

    //The below Ros realted Content are similar to eye_teleoperation.cs
    bool Navigation;
    private GameObject teleop_mode;
    ROSConnection m_Ros;
    string m_TopicName = "/stretch_diff_drive_controller/cmd_vel";
    void Start()
    {
        if (ROS_Connection)
        {
            m_Ros = ROSConnection.GetOrCreateInstance();
            m_Ros.RegisterPublisher<TwistMsg>(m_TopicName);

        }
        else
        {


            _childrenRigidbody = children.GetComponent<Rigidbody>();

        }







    }

    // Update is called once per frame
    void Update()
    {
        //bool Is_turnning;



        if (!Navigation)
        {

            MovingRate = Continous_input_gaze.ExportAcc;
            XYTurning = Continous_input_gaze.ExportV2;
            Navigation = teleop_mode.GetComponent<robot_follower>().nav_;
            tantheta = Mathf.Atan2(XYTurning.x, XYTurning.y); //angle calculation here


            if (MovingRate > 0.15f)
            {
                //check if it's moving, the closer to the center ,lesser the velocity


                if (XYTurning.y >= -0.5) //only turns when the robot is moving towards
                {
                    float targetAngle = tantheta;
                    TurnPos = new Vector3(0, targetAngle * TurningRate, 0);
                    if (!ROS_Connection)
                    {
                        _childrenRigidbody.angularVelocity = TurnPos;
                    }

                }

                newPos = children.transform.forward * XYTurning.y;
            }
            else
            {
                newPos = Vector3.zero;

            }



            if (!ROS_Connection)
            {
                children.transform.position += newPos * speed;
            }
            else
            {
                PublishDataToRos(TurnPos, newPos);

            }


        }


    }

    private void PublishDataToRos(Vector3 position, Vector3 rotation)
    {
        Vector3<FLU> x = new Vector3<FLU>(position);
        Vector3<FLU> y = new Vector3<FLU>(rotation);

        var end_pos = new TwistMsg
        {
            linear = x,
            angular = y

        };
        m_Ros.Publish(m_TopicName, end_pos);


    }

}





















