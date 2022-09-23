using System.Collections;
using UnityEngine;

public class FlimsyScript : MonoBehaviour
{
    private float shrinkSpeed = 10f;
    private float riseSpeed = 2f;
    private float scaleX;
    private float scaleZ;
    private GameObject car;
    public void Start()
    {
        gameObject.tag = "Flimsy";
        scaleX = gameObject.transform.localScale.x;
        scaleZ = gameObject.transform.localScale.z;
        //set it beneath ground
        transform.position = new Vector3(transform.position.x, -2, transform.position.z);
        car = GameObject.Find("/Car");
        LODGroup lodGroup = GetComponent<LODGroup>();
        LOD[] lods = lodGroup.GetLODs();
        Renderer[] renderers = new Renderer[1];
        renderers[0] = GetComponent<MeshRenderer>();
        lods[lods.Length - 1] = new LOD(1f / 40f, renderers);
        lodGroup.SetLODs(lods);
        lodGroup.RecalculateBounds();
        lodGroup.size = 2;
    }

    public void Update()
    {
        if (gameObject.transform.position.y < 0)
        {
            if (GetComponent<Renderer>().isVisible)
            {
                //start moving them up to y value of 0
                Vector3 newPos = new Vector3(transform.position.x, transform.position.y + riseSpeed * Time.deltaTime, transform.position.z);
                transform.position = newPos;
            }
        }
        /*
        if(transform.position.z + 10 < car.transform.position.z)
        {
            Destroy(gameObject);
        }
        */
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject.GetComponent<Collider>());
        StartCoroutine(shrinkToNothing());
    }

    private IEnumerator shrinkToNothing()
    {
        float time = 0;
        float timeToShrink = .5f;
        float newScale;
        while (time < timeToShrink)
        {
            time += Time.deltaTime;
            newScale = gameObject.transform.localScale.y - (shrinkSpeed * Time.deltaTime);
            if (newScale > .1)
                gameObject.transform.localScale = new Vector3(scaleX, newScale, scaleZ);
        }
        yield return null;
    }
}
