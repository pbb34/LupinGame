using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FOV2DEyes : MonoBehaviour
{
    public bool raysGizmosEnabled;
    //public float updateRate = 0.02f;
    public int quality = 4;
    public int fovAngle = 90;
    public float fovMaxDistance = 15;
    public LayerMask cullingMask;
    public List<RaycastHit> hits = new List<RaycastHit>();
    public List<RaycastHit2D> hits2D = new List<RaycastHit2D>(); // mine

    int numRays;
    float currentAngle;
    Vector3 direction;
    RaycastHit hit;

    void Update()
    {
        Cast2DRays();
    }



    void Cast2DRays()
    {
        numRays = fovAngle * quality;
        currentAngle = fovAngle / -2;

        hits2D.Clear();

        for (int i = 0; i < numRays; i++)
        {
            direction = Quaternion.AngleAxis(currentAngle, transform.up) * transform.forward;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, fovMaxDistance, cullingMask);
            if (hit == false)
            {
                hit.point = transform.position + (direction * fovMaxDistance);
            }

          /*if (hit.collider.tag=="Player")
            {
                print("HIT PLAYER");
            }
            */
            hits2D.Add(hit);

            currentAngle += 1f / quality;
        }


    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        if (raysGizmosEnabled && hits.Count() > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                Gizmos.DrawSphere(hit.point, 0.04f);
                Gizmos.DrawLine(transform.position, hit.point);
            }
        }
    }

}

