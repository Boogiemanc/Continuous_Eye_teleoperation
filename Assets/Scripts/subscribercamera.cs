using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using CompressedImage = RosMessageTypes.Sensor.CompressedImageMsg;

// Created on October 2022
// @author: samn (Samuel Millan-Norman, millan-normans@cardiff.ac.uk)
// Purpose: ROS-to-Unity-social-navigation

public class subscribercamera : MonoBehaviour
{
    ROSConnection m_Ros;

    public GameObject dome;
    private Texture2D texture2D;
    private byte[] imageData;
    private bool isMessageReceived;
    // Start is called before the first frame update
    void Start()
    {
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.Subscribe<CompressedImage>("/camera/color/image_raw/compressed", ShowImage);
        texture2D = new Texture2D(1, 1);
        dome.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
    }

    private void Update()
    {
        if (isMessageReceived)
            ProcessImage();
        Debug.Log(isMessageReceived);
    }

    void ShowImage(CompressedImage ImgMsg)
    {
        imageData = ImgMsg.data;
        isMessageReceived = true;
        Debug.Log(ImgMsg.format);
    }

    void ProcessImage()
    {
        texture2D.LoadImage(imageData);
        texture2D.Apply();
        dome.GetComponent<Renderer>().material.SetTexture("_MainTex", texture2D);
        isMessageReceived = false;
    }
}
