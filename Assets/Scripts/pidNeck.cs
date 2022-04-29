using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pidNeck : MonoBehaviour
{
    private readonly pidNeckController _pidController = new pidNeckController(8.0f, 0.0f, 0.05f);

    private Transform _currentTransform;
    public Rigidbody _objectRigidbody;

    public float Kp;
    public float Ki;
    public float Kd;

    private Vector2 previousError;
    public Vector2 neckPID;
    public Vector2 error;
    public float totalError;
    private float offset;

    [SerializeField] PIDManager pidManager;

    public Quaternion DesiredOrientation { get; set; }

    void Awake()
    {
        this._currentTransform = transform;
        this._objectRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        offset = Map360To180(this._currentTransform.eulerAngles.x);
    }

    void Update()
    {
        if (DesiredOrientation == null || this._currentTransform == null || this._objectRigidbody == null)
        {
            return;
        }

        this._pidController.Kp = this.Kp;
        this._pidController.Ki = this.Ki;
        this._pidController.Kd = this.Kd;


        var errorQuaternion = DesiredOrientation * Quaternion.Inverse(this._currentTransform.rotation);
        error = new Vector2
        {
            x = Map360To180(Map360To180(errorQuaternion.eulerAngles.x) + offset),
            y = Map360To180(errorQuaternion.eulerAngles.y)
        };

        if (pidManager.trigger == false && pidManager.horizontalTrigger == false && pidManager.verticalTrigger == false && pidManager.sineTrigger == false)
        {
            error = new Vector2(0, 0);
        }

        Vector2 delta = new Vector2
        {
            x = error.x - previousError.x,
            y = error.y - previousError.y
        };

        neckPID = this._pidController.ComputeOutput(error, delta, Time.fixedDeltaTime);



        previousError = error;

        totalError = Mathf.Sqrt(Mathf.Pow(error.x, 2f) + Mathf.Pow(error.y, 2f));

        //Debug.Log(error);
        //Debug.Log(offset);
    }

    float Map360To180(float degree)
    {
        if (degree > 180)
        {
            return degree - 360;
        }

        return degree;
    }
}
