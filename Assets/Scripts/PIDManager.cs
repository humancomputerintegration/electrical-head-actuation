using UnityEngine;

public class PIDManager : MonoBehaviour
{
    public Transform Center;

    //static targets
    public Transform Target1;
    public Transform Target2;
    public Transform Target3;
    public Transform Target4;
    public Transform Target5;
    public Transform Target6;
    public Transform Target7;
    public Transform Target8;
    public Transform Target9;
    public Transform Target10;
    public Transform Target11;
    public Transform Target12;
    public Transform Target13;
    public Transform Target14;
    public Transform Target15;
    public Transform Target16;

    //dynamic targets (i.e., trajectories)
    public Transform Trajectory1;
    public Transform Trajectory2;
    public Transform Trajectory3;

    public bool trigger; // for quitting the application after a fixed period of time.
    private float counter; // for quitting the application after a fixed period of time.
    [SerializeField] private int limit; // for quitting the application after a fixed period of time.

    public bool horizontalTrigger;
    public bool verticalTrigger;
    public bool sineTrigger;

    public pidNeck controlledObject;
    private Vector3 _cameraPosition;

    private void Awake()
    {
        this.controlledObject.GetComponent<Rigidbody>().centerOfMass = Vector3.zero;
        this._cameraPosition = Camera.main.transform.position;
        trigger = false;
        counter = 0;
    }

    private void OnGUI()
    {
        GUI.BeginGroup(new Rect(10, 10, 175, 1000));

        // Make a background box
        //GUI.Box(new Rect(10, 10, 150, 400), "Choose Action");

        if (GUI.Button(new Rect(20, 40, 150, 20), "Center"))
        {
            if (this.controlledObject != null)
            {
                /* this.controlledObject.transform.position = Vector3.zero;
                this.controlledObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                this.controlledObject.DesiredOrientation = this.controlledObject.transform.rotation;
                this.controlledObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                Camera.main.transform.position = this._cameraPosition; */
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Center.position, Vector3.up);
            }
        }

        if (GUI.Button(new Rect(20, 70, 150, 20), "1 Target"))
        {
            if (this.controlledObject != null)
            {
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Target1.position, Vector3.up);
                trigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 100, 150, 20), "2 Target"))
        {
            if (this.controlledObject != null)
            {
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Target2.position, Vector3.up);
                trigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 130, 150, 20), "3 Target"))
        {
            if (this.controlledObject != null)
            {
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Target3.position, Vector3.up);
                trigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 160, 150, 20), "4 Target"))
        {
            if (this.controlledObject != null)
            {
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Target4.position, Vector3.up);
                trigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 190, 150, 20), "5 Target"))
        {
            if (this.controlledObject != null)
            {
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Target5.position, Vector3.up);
                trigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 220, 150, 20), "6 Target"))
        {
            if (this.controlledObject != null)
            {
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Target6.position, Vector3.up);
                trigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 250, 150, 20), "7 Target"))
        {
            if (this.controlledObject != null)
            {
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Target7.position, Vector3.up);
                trigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 280, 150, 20), "8 Target"))
        {
            if (this.controlledObject != null)
            {
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Target8.position, Vector3.up);
                trigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 310, 150, 20), "9 Target"))
        {
            if (this.controlledObject != null)
            {
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Target9.position, Vector3.up);
                trigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 340, 150, 20), "10 Target"))
        {
            if (this.controlledObject != null)
            {
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Target10.position, Vector3.up);
                trigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 370, 150, 20), "11 Target"))
        {
            if (this.controlledObject != null)
            {
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Target11.position, Vector3.up);
                trigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 400, 150, 20), "12 Target"))
        {
            if (this.controlledObject != null)
            {
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Target12.position, Vector3.up);
                trigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 430, 150, 20), "13 Target"))
        {
            if (this.controlledObject != null)
            {
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Target13.position, Vector3.up);
                trigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 460, 150, 20), "14 Target"))
        {
            if (this.controlledObject != null)
            {
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Target14.position, Vector3.up);
                trigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 490, 150, 20), "15 Target"))
        {
            if (this.controlledObject != null)
            {
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Target15.position, Vector3.up);
                trigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 520, 150, 20), "16 Target"))
        {
            if (this.controlledObject != null)
            {
                this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Target16.position, Vector3.up);
                trigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 570, 150, 20), "1 Trajectory"))
        {
            if (this.controlledObject != null)
            {
                //this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Horizontal.position, Vector3.up);
                //trigger = true;
                horizontalTrigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 600, 150, 20), "2 Trajectory"))
        {
            if (this.controlledObject != null)
            {
                //this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Vertical.position, Vector3.up);
                //trigger = true;
                verticalTrigger = true;
            }
        }

        if (GUI.Button(new Rect(20, 630, 150, 20), "3 Trajectory"))
        {
            if (this.controlledObject != null)
            {
                //this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.HZigZag.position, Vector3.up);
                //trigger = true;
                sineTrigger = true;
            }
        }

        GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.UpperCenter;

        //GUI.Label(new Rect(20, 250, 125, 60), "Use scrollwheel to zoom camera.", centeredStyle);

        GUI.EndGroup();
    }

    private void Update()
    {
        Vector3 delta = Input.mouseScrollDelta.y * Vector3.forward;
        Vector3 localizedDelta = Camera.main.transform.TransformDirection(delta);

        Camera.main.transform.position += localizedDelta;

        if (trigger == true)
        {
            counter += Time.deltaTime;
        }

        if (counter >= limit) UnityEditor.EditorApplication.isPlaying = false;

        if (Input.GetKey(KeyCode.JoystickButton1) == true || Input.GetKey("q") == true) UnityEditor.EditorApplication.isPlaying = false;

        if (horizontalTrigger == true)
        {
            this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Trajectory1.position, Vector3.up);
        }

        if (verticalTrigger == true)
        {
            this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Trajectory2.position, Vector3.up);
        }

        if (sineTrigger == true)
        {
            this.controlledObject.DesiredOrientation = Quaternion.LookRotation(this.Trajectory3.position, Vector3.up);
        }
    }
}