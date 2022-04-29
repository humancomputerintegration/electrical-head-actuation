using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRehamove;

public class ehaController : MonoBehaviour
{
    Rehamove r = null;

    [SerializeField] GameObject camera;
    [SerializeField] [Range(0.5f, 10.0f)] float coefficient;
    [SerializeField] [Range(0, 20)] public int Left;
    [SerializeField] [Range(0, 20)] public int Right;
    [SerializeField] [Range(0, 20)] public int Up;
    [SerializeField] [Range(0, 20)] public int Down;
    [SerializeField] [Range(0, 500)] public int PW;
    [SerializeField] float staticThreshold = 5f;
    [SerializeField] PIDManager pidManager;

    [SerializeField] pidNeck controlledObject;

    private int maxPulseWidth;

    public int iPulseWidthX;
    public int iPulseWidthY;

    private Vector2 previousPOV;
    private float stopCounter;
    private bool stopEMS;
    private float errorTolerance = 25f;

    private Thread _updateThread;
    private string channel1;
    private string channel2;
    private int current1;
    private int current2;
    private int pulseWidth1;
    private int pulseWidth2;
    private Quaternion destination;
    private Quaternion prevDestination;
    private Vector2 distance;

    void Start()
    {
        r = new UnityRehamove.Rehamove("COM6"); // change the port num according to your setting
        _updateThread = new Thread(SuperFastLoop);
        _updateThread.Start();
        maxPulseWidth = PW;
        stopCounter = 0f;
        stopEMS = false;
    }

    private void OnDisable()
    {
        r.close();
    }

    void Update()
    {
        Vector2 PID = controlledObject.neckPID;
        PID = Vector2.Scale(PID, new Vector2(-1, -1));
        iPulseWidthX = Convert.ToInt32(PID.x);
        iPulseWidthY = Convert.ToInt32(PID.y);
        destination = controlledObject.DesiredOrientation;

        if (destination != prevDestination)
        {
            distance = controlledObject.error;
        }

        //reset all output to be 0
        pulseWidth1 = 0;
        pulseWidth2 = 0;

        //for PID neck stopper
        Vector2 difference = new Vector2
        {
            x = Map360To180(camera.transform.eulerAngles.x) - previousPOV.x,
            y = Map360To180(camera.transform.eulerAngles.y) - previousPOV.y
        };

        if (stopEMS == true)
        {
            suspendEMS();
            StartCoroutine(resumeEMS());
        }

        if (pidManager.horizontalTrigger == true || pidManager.verticalTrigger == true || pidManager.sineTrigger == true)
        {
            dynamicEMS();
        }
        else
        {
            staticEMS();
        }

        // for calibration
        if (Input.GetKey("d") == true)
        {
            channel2 = "white"; current2 = Left; pulseWidth2 = PW;
        }
        else if (Input.GetKey("a") == true)
        {
            channel2 = "black"; current2 = Right; pulseWidth2 = PW;
        }
        else if (Input.GetKey("w") == true)
        {
            channel1 = "red"; current1 = Up; pulseWidth1 = PW;
        }
        else if (Input.GetKey("s") == true)
        {
            channel1 = "blue"; current1 = Down; pulseWidth1 = PW;
        }
        else if (Input.GetKey("q") == true)
        {
            StartCoroutine(Measurement());
        }

        //for PID neck stopper
        if (pidManager.trigger == true) staticThreshold = 0f;
        if (Mathf.Sqrt(Mathf.Pow(difference.x, 2f) + Mathf.Pow(difference.y, 2f)) < staticThreshold && controlledObject.totalError >= errorTolerance)
        {
            stopCounter += Time.deltaTime;
        }

        if (stopCounter >= 0.5f)
        {
            stopEMS = true;
            stopCounter = 0f;
        }

        previousPOV = new Vector2
        {
            x = Map360To180(camera.transform.eulerAngles.x),
            y = Map360To180(camera.transform.eulerAngles.y)
        };

        prevDestination = destination;
        //Debug.Log(distance);
    }

    float Map360To180(float degree)
    {
        if (degree > 180)
        {
            return degree - 360;
        }

        return degree;
    }

    void SuperFastLoop()
    {
        // This begins our Update loop
        while (true)
        {
            r.pulse(channel1, current1, pulseWidth1);
            r.pulse(channel2, current2, pulseWidth2);
            //Thread.Sleep(1);

        }
    }

    void staticEMS()
    {
        if (iPulseWidthX > 0)
        {
            if (distance.x <= 0f)
            {
                if (iPulseWidthX >= maxPulseWidth) iPulseWidthX = maxPulseWidth; // safety limit
                //r.pulse("red", Up, iPulseWidthX);
                channel1 = "red"; current1 = Up; pulseWidth1 = iPulseWidthX;
            } //else {
              //  if (iPulseWidthX >= maxPulseWidth) iPulseWidthX = maxPulseWidth; // safety limit
              //r.pulse("red", Up, iPulseWidthX);
              // channel1 = "red"; current1 = Up/2; pulseWidth1 = iPulseWidthX;
              //}
        }
        else if (iPulseWidthX < 0)
        {
            if (distance.x >= 0f)
            {
                if (iPulseWidthX <= -maxPulseWidth) iPulseWidthX = -maxPulseWidth; // safety limit
                                                                                   //r.pulse("blue", Down, -iPulseWidthX);
                channel1 = "blue"; current1 = Down; pulseWidth1 = -iPulseWidthX;
            } //else
              //{
              //  if (iPulseWidthX <= -maxPulseWidth) iPulseWidthX = -maxPulseWidth; // safety limit
              //r.pulse("blue", Down, -iPulseWidthX);
              //   channel1 = "blue"; current1 = Down/2; pulseWidth1 = -iPulseWidthX;
              //}
        }

        if (iPulseWidthY > 0)
        {
            if (distance.y <= -5f)
            {
                if (iPulseWidthY >= maxPulseWidth) iPulseWidthY = maxPulseWidth; // safety limit
                                                                                 //r.pulse("white", Left, iPulseWidthY);
                channel2 = "white"; current2 = Left; pulseWidth2 = iPulseWidthY;
            } //else
              //{
              // if (iPulseWidthY >= maxPulseWidth) iPulseWidthY = maxPulseWidth; // safety limit
              //r.pulse("white", Left, iPulseWidthY);
              //  channel2 = "white"; current2 = Left/2; pulseWidth2 = iPulseWidthY;
              // }
        }
        else if (iPulseWidthY < 0)
        {
            if (distance.y >= 5f)
            {
                if (iPulseWidthY <= -maxPulseWidth) iPulseWidthY = -maxPulseWidth; // safety limit
                                                                                   //r.pulse("black", Right, -iPulseWidthY);
                channel2 = "black"; current2 = Right; pulseWidth2 = -iPulseWidthY;
            } //else
              //{
              // if (iPulseWidthY <= -maxPulseWidth) iPulseWidthY = -maxPulseWidth; // safety limit
              //r.pulse("black", Right, -iPulseWidthY);
              //  channel2 = "black"; current2 = Right/2; pulseWidth2 = -iPulseWidthY;
              //}

        }
    }

    void dynamicEMS()
    {
        if (iPulseWidthX > 0 && distance.x <= -1f)// && pidManager.horizontalTrigger == false)
        {
            if (iPulseWidthX >= maxPulseWidth) iPulseWidthX = maxPulseWidth; // safety limit
            //r.pulse("red", Up, iPulseWidthX);
            channel1 = "red"; current1 = Up; pulseWidth1 = iPulseWidthX;

        }
        else if (iPulseWidthX < 0 && distance.x >= 1f)// && pidManager.horizontalTrigger == false)
        {
            if (iPulseWidthX <= -maxPulseWidth) iPulseWidthX = -maxPulseWidth; // safety limit
            //r.pulse("blue", Down, -iPulseWidthX);
            channel1 = "blue"; current1 = Down; pulseWidth1 = -iPulseWidthX;
        }

        if (iPulseWidthY > 0 && distance.y <= -1f && pidManager.verticalTrigger == false)
        {
            if (iPulseWidthY >= maxPulseWidth) iPulseWidthY = maxPulseWidth; // safety limit
            //r.pulse("white", Left, iPulseWidthY);
            channel2 = "white"; current2 = Left; pulseWidth2 = iPulseWidthY;
        }
        else if (iPulseWidthY < 0 && distance.y >= 1f && pidManager.verticalTrigger == false)
        {
            if (iPulseWidthY <= -maxPulseWidth) iPulseWidthY = -maxPulseWidth; // safety limit
            //r.pulse("black", Right, -iPulseWidthY);
            channel2 = "black"; current2 = Right; pulseWidth2 = -iPulseWidthY;
        }
    }

    void suspendEMS()
    {
        iPulseWidthX = 0;
        iPulseWidthY = 0;
    }

    IEnumerator resumeEMS()
    {
        yield return new WaitForSeconds(0.5f);
        stopEMS = false;
        stopCounter = 0f;
    }

    IEnumerator Measurement()
    {
        for (int i = 0; i < 30; i++)
        {
            r.pulse("black", Down, PW);
            r.pulse("white", Down, PW);
            //r.pulse("blue", Down, PW);
        }
        yield return null;
    }
}
