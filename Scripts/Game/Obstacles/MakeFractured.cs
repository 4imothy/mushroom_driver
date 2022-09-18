using UnityEngine;

public class MakeFractured : MonoBehaviour
{
    public GameObject fracturedOb;
    //change to fractured on collison
    private void OnCollisionEnter(Collision collision)
    {
        //if collides with character
        if (collision.gameObject.tag == "Character" || collision.gameObject.tag == "Obstacle")
        {
            GameObject currentGround = GameObject.Find("LevelGenerator").GetComponent<GroundManager>().getCurrentGround();
            //spawn new the y is the heigh of the obstacle
            GameObject fractured = Instantiate(fracturedOb);
            fractured.transform.parent = currentGround.transform;
            Vector3 newPos = transform.position;
            //height of a obstacle
            newPos.y = -3f;
            fractured.transform.position = newPos;
            //destroy new
            Destroy(gameObject);
        }
    }
}
