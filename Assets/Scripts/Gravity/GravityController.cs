using UnityEngine;
using System.Collections;

public class GravityController : MonoBehaviour
{
    [SerializeField] float GravityStrength;
    [SerializeField] float RotationTime;
    private bool useGravity;
    private Vector3 gravityDir;
    private ConstantForce CF;
    private bool currentlyShifting = false;

    public ChangeGravity activeGravitySource;
    Coroutine activeShift;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CF = this.gameObject.GetComponent<ConstantForce>();
        gravityDir = new Vector3(0, -1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (useGravity)
        {
            CF.force = gravityDir * GravityStrength;
        }
        else
        {
            CF.force = Vector3.zero;
        }
    }

    public void SetUseGravity(bool b)
    {
        useGravity = b;
    }

    private void ShiftGravity(ChangeGravity cg)
    {
        Transform t = this.gameObject.transform;

        Vector3 newDirection = -cg.transform.up.normalized;
        //if (Mathf.Abs(180 * Mathf.Acos(Vector3.Dot(t.up, -newDirection)) / Mathf.PI) > 45) 
        {
            if (!currentlyShifting)
            {
                gravityDir = newDirection;
                if (activeGravitySource)
                {
                    activeGravitySource.checkForEnter = true;
                }
                activeGravitySource = cg;
                cg.checkForEnter = false;
                activeShift = StartCoroutine(RotatePlayer(t));
            }
        }

    }
    private void ShiftGravity(Vector3 newDirection)
    {
        Transform t = this.gameObject.transform;
        if (activeGravitySource)
        {
            //StopAllCoroutines();
            activeGravitySource.checkForEnter = true;
            activeGravitySource = null;
                
            gravityDir = newDirection;

            StartCoroutine(RotatePlayer(t));
            
        }

    }

    private IEnumerator RotatePlayer(Transform t)
    {
        currentlyShifting = true;

        PlayerCamera pc = Camera.main.gameObject.GetComponent<PlayerCamera>();

        Vector3 axis = Vector3.Cross(t.up.normalized, -gravityDir).normalized;
        float angle = 180 * Mathf.Acos(Vector3.Dot(t.up.normalized, -gravityDir)) / Mathf.PI;

        if (angle > 180 - 0.01)
        {
            axis = t.right.normalized;
        }

        GameObject camera = Camera.main.transform.parent.parent.gameObject;
        Quaternion camQ1 = camera.transform.localRotation;
        Transform camT2 = camera.transform;
        camT2.Rotate(axis, angle);
        Quaternion camQ2 = camT2.localRotation;

        Quaternion pQ1 = this.gameObject.transform.localRotation;
        Transform pT2 = this.gameObject.transform;
        pT2.Rotate(axis, angle);
        Quaternion pQ2 = pT2.localRotation;
        Vector3 pq2_euler = pQ2.eulerAngles;

        pQ2.Normalize();
        camQ2.Normalize();

        for (float x = 0; x < RotationTime; x += Time.deltaTime)
        {
            float u = x / RotationTime;
            this.transform.localRotation = Quaternion.Slerp(pQ1, pQ2, u);
            camera.transform.localRotation = Quaternion.Slerp(camQ1, camQ2, u);

            yield return null;
        }

        for (int i = 0; i < 3; ++i)
        {
            this.transform.localRotation = pQ2;
            camera.transform.localRotation = camQ2;
            yield return null;
        }

        currentlyShifting = false;
        yield return null;
    }

    private void checkAngle()
    {
        //this.transform.localRotation
    }
}
