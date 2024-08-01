using RosMessageTypes.Actionlib;
using System.Linq;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using UnityEngine.AI;

// Created on September 2023
// @author: samn (Samuel Millan-Norman, millan-normans@cardiff.ac.uk)
// Purpose: ROS-to-Unity-social-navigation

public class robot_follower : MonoBehaviour
{
    public NavMeshAgent Player;
    public Transform Robot;

    // ROS Connector
    ROSConnection m_Ros;

    bool cam_;
    public bool nav_;
    private float journeyTime = 0.025f;

    // Start is called before the first frame update
    void Start()
    {
        cam_ = false;
        nav_ = false;

        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.Subscribe<GoalStatusArrayMsg>("/smf_goto_action/status", ReceiveROScheckCmd);
    }

    void Update()
    {
        if (cam_ == false)
        {
            if (nav_ == true)
            {
                Player.SetDestination(Robot.position);
            }
            else
            {
                transform.position = Vector3.Slerp(transform.position, Robot.position, journeyTime);
                transform.rotation = Robot.rotation;
            }
        }
    }

    void ReceiveROScheckCmd(GoalStatusArrayMsg statusdata)
    {

        if (cam_ == false)
        {
            if (statusdata.status_list.Count() != 0)
            {
                int planner_running = statusdata.status_list[0].status;
                if (planner_running == 3)
                {
                    switch_camera();
                }
            }
        }
    }

    public void teleoperationmode()
    {
        nav_ = false;
    }
    public void navigationmode()
    {
        nav_ = true;
    }
    public void switch_camera()
    {
        Player.enabled = false;
        Vector3 new_position = new Vector3(-47.0f, 0.0f, -47.0f);
        transform.position = new_position;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        cam_ = true;
    }
    public void switch_VR()
    {
        transform.position = Robot.position;
        transform.rotation = Robot.rotation;
        cam_ = false;
        Player.enabled = true;
    }
}
