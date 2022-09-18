using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public GameObject gameCamera;
    public float charSpeed;

    private CharacterController characterController;
    private float gravity = 9.8f;
    private float vSpeed = 0;
    private float hInput = 0;
    private float hSpeed = 0;

    //to move through ob
    private float forwardSpeed = 50f;

    private bool isHit = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {

        if (gameObject.transform.position.y < -1)
        {
            endCharacter();
        }
        //stop movement if it is hit
        if (!isHit)
        {
            hInput = Input.GetAxis("Horizontal");

            hSpeed = hInput * charSpeed;
            
            if (hSpeed < 0)
                gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.Euler(0, -15, 0), .03f);
            else if (hSpeed != 0)
                gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.Euler(0, 15, 0), .03f);
            else
                gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.Euler(0, 0, 0), 0.03f);
            


            if (!characterController.isGrounded)
                vSpeed -= gravity * Time.deltaTime;
            else
                vSpeed = 0;
            characterController.Move(new Vector3(hSpeed, vSpeed, 0) * Time.deltaTime);
        }
        else
        {
            //so character doesn't travel over the broken pieces
            characterController.slopeLimit = 0;
            //only travels forward and down if off ground
            if (!characterController.isGrounded)
            {
                vSpeed -= gravity * Time.deltaTime;
                if (gravity > 0)
                    gravity -= 0.02f;
                else
                    gravity = 0;
            }
            else
            {
                vSpeed = 0;
            }
            characterController.Move(new Vector3(0, vSpeed, forwardSpeed) * Time.deltaTime);
            //slow down the player
            if (forwardSpeed > 0)
                forwardSpeed -= 0.1f;
            else
                forwardSpeed = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            endCharacter();
        }
    }

    private void endCharacter()
    {
        gameCamera.GetComponent<CameraFollow>().enabled = false;
       // FindObjectOfType<GroundManager>().shouldMove = false;
        //move the character forward to make collision bigger
        isHit = true;
        //FindObjectOfType<CameraFollow>().enabled = false;
        Invoke("beginGameEnd", 2f);
    }

    private void beginGameEnd()
    {
        FindObjectOfType<GameManager>().endGame();
    }
}
