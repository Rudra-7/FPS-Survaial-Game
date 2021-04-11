using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weaponManager;

    public float fireRate = 15f;
    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomCameraAnime;
    private bool zoomed;

    private Camera mainCam;

    private GameObject crosshair;

    private bool isAiming;

    [SerializeField]
    private GameObject arrowPrefab, spearPrefab;

    [SerializeField]
    private Transform arrowBowStartPosition;



    private void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();

        zoomCameraAnime = transform.Find(Tags.LOOK_ROOT).transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();

        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);

        mainCam = Camera.main;

    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WeaponShoot();
        ZoomedInAndOut();
    }

    void WeaponShoot()
    {
        // assault rifle
        if(weaponManager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE)
        {
            // press and hold AND
            // TIme is greater than the next Time to fire
            if(Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;

                weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                //bullet being fired
                BulletFired();

            }
        }
        // shoots once
        else
        {
            if (Input.GetMouseButtonDown(0))
            {

                // handle axe
                if(weaponManager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG)
                {
                    weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                }
                
                //  handle shoot
                if(weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET)
                {
                    weaponManager.GetCurrentSelectedWeapon().ShootAnimation();

                    BulletFired();
                }
                else
                {
                    // bow or spear

                    if (isAiming)
                    {
                        weaponManager.GetCurrentSelectedWeapon().ShootAnimation();

                        if(weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW)
                        {
                            ThrowArrowOrSpear(true);
                        }

                        else if(weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.SPEAR)
                        {
                            ThrowArrowOrSpear(false);
                        }
                    }

                }
                //else
       


            }// mouse 0
        }// else
    }// end of weapon shoot method

    void ZoomedInAndOut()
    {
        // we are going to aim
        if(weaponManager.GetCurrentSelectedWeapon().weaponAim == WeaponAim.AIM)
        {
            // zoom in by clicking RMB
            if (Input.GetMouseButtonDown(1))
            {
                zoomCameraAnime.Play(AnimationTags.ZOOM_IN_ANIME);

                crosshair.SetActive(false);
            }

            // zoom out by clicking LMB
            if (Input.GetMouseButtonUp(1))
            {
                zoomCameraAnime.Play(AnimationTags.ZOOM_OUT_ANIME);

                crosshair.SetActive(true);
            }

        }// if we need to zoom weapon

        if(weaponManager.GetCurrentSelectedWeapon().weaponAim == WeaponAim.SELF_AIM)
        {

            if (Input.GetMouseButtonDown(1))
            {
                weaponManager.GetCurrentSelectedWeapon().Aim(true);
                isAiming = true;

            }

            if (Input.GetMouseButtonUp(1))
            {
                weaponManager.GetCurrentSelectedWeapon().Aim(false);
                isAiming = false;

            }

        }// weapon self aim (bow or aim)

    }// end of zoom in and out method

    void ThrowArrowOrSpear(bool throwArrow)
    {
        if (throwArrow)
        {
            GameObject arrow = Instantiate(arrowPrefab);
            arrow.transform.position = arrowBowStartPosition.position;

            arrow.GetComponent<ArrowBowScript>().Launch(mainCam);
        }

        else
        {
            GameObject spear = Instantiate(spearPrefab);
            spear.transform.position = arrowBowStartPosition.position;

            spear.GetComponent<ArrowBowScript>().Launch(mainCam);
        }
    }// throw arrow or spear

    void BulletFired()
    {
        RaycastHit hit;

        if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit))
        {
            //print("WE HIT: " + hit.transform.gameObject.name);
            if(hit.transform.tag == Tags.ENEMY_TAG)
            {
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }
        }
    }

}// end of class
















