using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CampfireCtrl : MonoBehaviour
{
    public float maxFuel = 100f;
    public float currentFuel;
    bool thisOn = true;
    public bool thisBeConstructed;
    void Start()
    {
        currentFuel = maxFuel+50f;
    }

    // Update is called once per frame
    void Update()
    {
        CampfireOn();
    }
    void CampfireOn()
    {
        if (!thisOn) return;
        if (currentFuel > 30f)
        {
            currentFuel -= Time.deltaTime;
        }
        else
        {
            currentFuel = 30f;
            thisOn = false;
            this.transform.GetChild(0).gameObject.SetActive(false);
            return;
        }
        float ratio = currentFuel / maxFuel;
        this.transform.GetChild(0).localScale = new Vector3(ratio, ratio, ratio);  

    }
    void AddMoreFuel(Transform other)
    {
        eCanPickupItemType item = other.GetComponent<PickUp>().type;
        if (item == eCanPickupItemType.stick)
        {
            currentFuel += 10f;
            this.transform.GetChild(0).gameObject.SetActive(true);

            thisOn = true;
            Destroy(other.gameObject);
        }
        else if (item == eCanPickupItemType.LogWooden)
        {
            currentFuel += 30f;
            this.transform.GetChild(0).gameObject.SetActive(true);

            thisOn = true;
            Destroy(other.gameObject);
        }
        if (currentFuel > 150f) currentFuel = 150f;
        
    }
    private void OnTriggerEnter(Collider other)
    {
        

    }
    private void OnCollisionEnter(Collision collision)
    {
        Transform col = collision.transform ;
        if (col.GetComponent<PickUp>() != null && thisBeConstructed)
        {

            AddMoreFuel(col);

        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PickUp>() != null && thisBeConstructed)
        {
            if (other.GetComponent<PickUp>().thisCanCooking == true)
            {
                eCanPickupItemType type = other.GetComponent<PickUp>().type;

                CookingSystem.Instance.StartCoking( type, other.transform);
            }
        }
    }

}
