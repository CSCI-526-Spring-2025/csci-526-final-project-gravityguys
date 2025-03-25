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
        if (collision.relativeVelocity.magnitude > velocity)
        {
            GameObject effect = Instantiate(shatterEffectPrefab, transform.position, Quaternion.identity);
            effect.GetComponent<ParticleSystem>().Play();
            Destroy(effect, 3f);
            Destroy(gameObject);
        }
    }
}
