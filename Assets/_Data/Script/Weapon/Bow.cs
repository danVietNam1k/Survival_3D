using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class Bow : MonoBehaviour
{
    private Animator animator;
    float forceShoot = 0;
    public float maxForceShoot = 2f;
    bool canShoot = false;
    public GameObject arow;
    public Transform posArow;
    ItemSlot slot = null;
    bool arrowReady = false;
    public AudioClip drawSound, shootSound;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        arow = Resources.Load<GameObject>("Item_obj/Arrow");
        transform.parent.transform.localPosition = Vector3.zero;
        transform.parent.transform.localRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        
        Debug.DrawLine(posArow.position, posArow.position + Vector3.up, Color.red);
        if (slot?.amount > 0 && arrowReady == false)
        {
            arrowReady = true;
        }
        else if (slot?.amount == 0)
        {
            arrowReady = false;
        }
        Shoot();
    }
    private void FixedUpdate()
    {
        
        {
            
        }
        foreach (string itemName in InventorySystem.Instance.itemList)
        {
            if (itemName == "Arrow")
            {
                foreach (GameObject slot in InventorySystem.Instance.slotList)
                {

                    if (slot.transform.childCount > 0 && slot.transform.GetChild(0).name == "Arrow")
                    {
                        this.slot = slot.GetComponent<ItemSlot>();
                        break;

                    }
                    this.slot = null;
                }
            }
        }
      
    }
    void Shoot()
    {
        if (arrowReady) {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                SoundManager.Instance.PlaySFX(drawSound);
                animator.SetBool("Draw", true);
                canShoot = true;

            }
            if (forceShoot < maxForceShoot && canShoot)
            {
                forceShoot += Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                animator.SetBool("Draw", false);
                canShoot = false;
                forceShoot = 0;
            }
            if (Input.GetMouseButtonDown(0) && canShoot)
            {
                animator.SetTrigger("Shoot");
                InventorySystem.Instance.RemoveItem("Arrow", 1);

            }
        }
        else
        {
            print("no have arrow");
        }
        

    }
    
    public void ShootAnim()
    {
        InstantieArow(forceShoot);
        SoundManager.Instance.PlaySFX(shootSound);
        animator.SetBool("Draw", false);
        canShoot = false;
        forceShoot = 0;
    }
    public void InstantieArow(float force)
    {
        Vector3 target = CalculateDIraction();
        Vector3 diraction = target - posArow.position;

        GameObject arowNew = Instantiate(arow,posArow.position, Quaternion.LookRotation(diraction));
        arowNew.transform.SetParent(null);
        
        print(posArow.position);
        Debug.DrawLine(posArow.position, target, Color.red,3f);


        arowNew.GetComponent<Rigidbody>().AddForce(diraction.normalized * force*20f,ForceMode.Impulse);
    }
    Vector3 CalculateDIraction()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3 (0.5f,0.5f,0));
        RaycastHit hit;
        Vector3 targetPoint;
        
        if(Physics.Raycast(ray,out hit)){
            targetPoint = hit.point;
          
        }
        else
        {
            targetPoint = ray.GetPoint(50f);

        }
        return targetPoint;
    }
    }
