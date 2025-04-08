using UnityEngine;

public class PlatformExplode : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject shatterEffectPrefab;
    public float velocity;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.relativeVelocity.magnitude);
        if (collision.relativeVelocity.magnitude > velocity && collision.gameObject.CompareTag("Pullable"))
        {
            GameObject effect = Instantiate(shatterEffectPrefab, transform.position, Quaternion.identity);
            effect.GetComponent<ParticleSystem>().Play();
            Destroy(effect, 3f);
            //Destroy(gameObject);

            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            for(int i = 0; i < gameObject.transform.childCount; ++i)
            {
                GameObject g = gameObject.transform.GetChild(i).gameObject;
                g.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    private void ResetObject()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            GameObject g = gameObject.transform.GetChild(i).gameObject;
            g.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
