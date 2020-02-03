using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public float mFireRate = 0.5f;
    public float mFireRange = 50f;
    public float mHitForce = 100f;
    public int mLaserDamage = 100;

    private LineRenderer mLaserLine;

    private bool mLaserLineEnabled;

    private WaitForSeconds mLaserDuration = new WaitForSeconds(0.05f);

    private float mNextFire;

    // Start is called before the first frame update
    void Start()
    {
        mLaserLine = GetComponent<LineRenderer>();
    }

    private void Fire()
    {
        Transform cam = Camera.main.transform;

        mNextFire = Time.time + mFireRate;

        Vector3 rayOrigin = cam.position;

        mLaserLine.SetPosition(0, transform.up * -10f);

        RaycastHit hit;

        if(Physics.Raycast( rayOrigin, cam.forward, out hit, mFireRange ))
        {
            mLaserLine.SetPosition(1, hit.point);
        }
        else
        {
            mLaserLine.SetPosition(1, cam.forward * mFireRange);
        }

        StartCoroutine(LaserFx());

        if(Physics.Raycast(rayOrigin, cam.forward, out hit, mFireRange))
        {
            mLaserLine.SetPosition(1, hit.point);

            CubeBehaviorScript cubeCtr = hit.collider.GetComponent<CubeBehaviorScript>();
            if( cubeCtr != null)
            {
                if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * mHitForce);

                    cubeCtr.Hit(mLaserDamage);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1") && Time.time > mNextFire)
        {
            Fire();
        }
    }

    private IEnumerator LaserFx()
    {
        mLaserLine.enabled = true;

        yield return mLaserDuration;
        mLaserLine.enabled = false;
    }




}

