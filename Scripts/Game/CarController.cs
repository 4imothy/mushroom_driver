using UnityEngine;
using TMPro;

public class CarController : MonoBehaviour
{
    private float horizontalInput;
    private float currentSteerAngle;
    public static int coinsThisGame = 0;
    public static bool isHover = false;

    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float speed = 0;
    private float turnConst;
    private float speedConst;

    [SerializeField] DataForPlayer dataInteractor;
    [SerializeField] TextMeshProUGUI coinText;

    [SerializeField] private Camera gameCamera;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private GameObject visualComponent;

    private Transform frontLeftWheelTransform;
    private Transform frontRightWheelTransform;
    private Transform rearLeftWheelTransform;
    private Transform rearRightWheelTransform;
    private WheelCollider[] wheelColliders = new WheelCollider[4];

    private bool isHit = false;
    private float timeForControl = 0;
    private int loadIndex;

    private static bool isZeroIndex = true;
    private bool waitingToStart = false;
    private LastPlayData[] lastPlayStorage = new LastPlayData[2];
    private float timeSinceLastSave = 0;

    private void Start()
    {
        if (!isHover)
        {
            frontLeftWheelTransform = GameObject.Find("/Car/Visual/Wheels(Clone)/FrontLeft").transform;
            frontRightWheelTransform = GameObject.Find("/Car/Visual/Wheels(Clone)/FrontRight").transform;
            rearLeftWheelTransform = GameObject.Find("/Car/Visual/Wheels(Clone)/BackLeft").transform;
            rearRightWheelTransform = GameObject.Find("/Car/Visual/Wheels(Clone)/BackRight").transform;
        }

        coinText.text = "<sprite=\"coinSprite\" index=0> " + coinsThisGame;

        turnConst = dataInteractor.getTurnConst();
        speedConst = dataInteractor.getSpeedConst();
        //fill it with origin so that if they die within first 10 seconds still have save to go to
        lastPlayStorage[1] = new LastPlayData(horizontalInput, gameObject.transform.position, gameObject.transform.rotation, GetComponent<Rigidbody>().velocity, currentSteerAngle);
        InvokeRepeating("fillLastPlayData", 0f, 10f);
    }

    //variables for if continue is called
    private class LastPlayData
    {
        public float x;
        public float z;
        public float y;
        public float rotX;
        public float rotY;
        public float rotZ;
        public float rotW;
        public float velX;
        public float velY;
        public float velZ;
        public float savedSteerAngle;

        public Quaternion flcWheelRot;
        public Quaternion frcWheelRot;
        public Quaternion blcWheelRot;
        public Quaternion brcWheelRot;

        public Quaternion fltWheelRot;
        public Quaternion frtWheelRot;
        public Quaternion bltWheelRot;
        public Quaternion brtWheelRot;

        public float horizInput;

        public LastPlayData(float horizInput, Vector3 position, Quaternion rotation, Vector3 velocity, float currentSteerAngle)
        {
            this.x = position.x;
            this.z = position.z;
            this.y = position.y;

            this.rotX = rotation.x;
            this.rotY = rotation.y;
            this.rotZ = rotation.z;
            this.rotW = rotation.w;

            this.velX = velocity.x;
            this.velY = velocity.y;
            this.velZ = velocity.z;

            this.horizInput = horizInput;

            savedSteerAngle = currentSteerAngle;
        }
    }

    private void fillLastPlayData()
    {
        int index = isZeroIndex ? 0 : 1;
        lastPlayStorage[index] = new LastPlayData(horizontalInput, gameObject.transform.position, gameObject.transform.rotation, GetComponent<Rigidbody>().velocity, currentSteerAngle);
        isZeroIndex = !isZeroIndex;
        timeSinceLastSave = 0;
    }

    public void loadNewCar()
    {
        //if isZeroIndex is true then 0 was the most recently filled index
        if (timeSinceLastSave <= 5)
        {
            //load from older save, if isZeroIndex is true, then 0 is the next one to be filled so 0 is the oldest
            loadIndex = isZeroIndex ? 0: 1;
        }
        else
        {
            //if isZeroIndex is true then 0 is the next one to be filled so one is the newest
            loadIndex = isZeroIndex ? 1 : 0;
        }
        Destroy(GameObject.Find("Car/Fractured(Clone)"));
        gameCamera.GetComponent<CameraFollow>().enabled = true;
        gameCamera.GetComponent<CameraFollow>().setCameraToCar();
        visualComponent.SetActive(true);
        isHit = false;
        LastPlayData lookAt = lastPlayStorage[loadIndex];
        gameObject.transform.position = new Vector3(lookAt.x, lookAt.y, lookAt.z);
        gameObject.transform.rotation = new Quaternion(lookAt.rotX, lookAt.rotY, lookAt.rotZ, lookAt.rotW);

        currentSteerAngle = lookAt.savedSteerAngle;


        waitingToStart = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void startNewCar()
    {
        LastPlayData lookAt = lastPlayStorage[loadIndex];
        GetComponent<Rigidbody>().velocity = new Vector3(lookAt.velX, lookAt.velY, lookAt.velZ);
        //cut time in half to start the car off slower but get faster quicker
        timeForControl = -10;
        speedConst++;
        turnConst++;
        waitingToStart = false;
    }

    private void FixedUpdate()
    {
        if (!isHit && !waitingToStart)
        {
            timeForControl += Time.deltaTime;
            timeSinceLastSave += Time.deltaTime;
            timeToAngleAndSpeed();
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
            if (transform.position.y < -1)
                endCharacter();
        }
    }


    private void timeToAngleAndSpeed()
    {
        speed = Mathf.Log(timeForControl + 15, 3) * speedConst;
        maxSteerAngle = Mathf.Log(timeForControl + 40) * turnConst;
    }

    private void GetInput()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                horizontalInput = (touch.position.x - Screen.width / 2) / (Screen.width);
            }
        }
        else
            horizontalInput = 0f;
    }

    private void HandleMotor()
    {

        frontLeftWheelCollider.motorTorque = 1;
        frontRightWheelCollider.motorTorque = 1;

        if (isHit)
        {

            if (speed > 0)
                speed -= 0.2f;
            else
                speed = 0;
        }
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        vel.y = 0;
        vel = vel.normalized * speed;
        vel.y = -1;
        GetComponent<Rigidbody>().velocity = vel;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        if (!isHover)
        {
            wheelTransform.rotation = rot;
            //wheelTransform.position = pos;
            wheelTransform.position = new Vector3(wheelTransform.position.x, pos.y, wheelTransform.position.z);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Obstacle")
        {
            if (!isHit)
            {
                isHit = true;
                endCharacter();
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            dataInteractor.addCoin();
            coinText.text = "<sprite=\"coinSprite\" index=0> " + ++coinsThisGame;
        }
    }

    public void endCharacter()
    {
        visualComponent.SetActive(false);
        Instantiate(Resources.Load("Cars/" + dataInteractor.getSelectedCar() + "/Fractured") as GameObject, gameObject.transform);
        dataInteractor.updateHighScore();
        dataInteractor.saveCurrentData();
        gameCamera.GetComponent<CameraFollow>().enabled = false;
        isHit = true;
        //time here is given for some game over animation car crash or smth
        FindObjectOfType<GameManager>().endGameAfterDelay(2);
    }

    /*
    private bool isGrounded()
    {
        //this method changes even back tires to normal stiffness which is wrong needs to be multipled by ten
        wheelColliders = new WheelCollider[] { frontLeftWheelCollider, frontRightWheelCollider, rearLeftWheelCollider, rearRightWheelCollider };

        bool check = false;
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            WheelHit hit;
            if (wheelColliders[i].GetGroundHit(out hit))
            {
                check = true;
                WheelFrictionCurve curve = wheelColliders[i].forwardFriction;
                curve.stiffness = hit.collider.material.staticFriction;
                wheelColliders[i].forwardFriction = curve;
                curve = wheelColliders[i].sidewaysFriction;
                curve.stiffness = hit.collider.material.staticFriction;
                wheelColliders[i].sidewaysFriction = curve;
            }
        }
        return check;
    }
    */
}
