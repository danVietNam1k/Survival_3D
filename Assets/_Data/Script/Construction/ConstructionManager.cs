using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager Instance { get; private set; }

    [SerializeField] Transform player;
    [SerializeField] float buildDistance = 3f;

    public GameObject ghost, itemConstructionOnHand;
    string currentItem;
    public bool canBuild =true, isBuilding =false;
    public LayerMask layerMask;
    public Material ghostMateriakRed, ghostMateriakGreen, nomalMaterial;
    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (itemConstructionOnHand.IsDestroyed())
        {
            itemConstructionOnHand = null;
            StopConstruction();
        }
        UpdateGhostPosition();
       

    }
    public void ReferenceItemOnHand(GameObject item)
    {
        itemConstructionOnHand = item;
    }
    public void StartConstruction(string itemName)
    {
        if (currentItem == itemName && ghost != null)
            return;

        StopConstruction();

        currentItem = itemName;
        var prefab = Resources.Load<GameObject>("Construction_item/" + itemName);
        ghost = Instantiate(prefab);
        ghost.name = itemName;
        CheckLayerMaskConstruction();
    }

    public void StopConstruction()
    {
        if (ghost != null)
            Destroy(ghost);

        ghost = null;
        currentItem = null;
    }

    void UpdateGhostPosition()
    {
        if (InventorySystem.Instance.isOpenInventory) return;
        if (ghost == null || isBuilding) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(
            ray, out RaycastHit hit, buildDistance, layerMask
           ))
        {
            if (!ghost.activeSelf) ghost.SetActive(true);

            //ghost.transform.position = CalculateSnapPosition(hit);
            ghost.transform.position = CalculateSnapPosition(hit);

            ghost.transform.rotation = CaculateRotate( hit);
            //ghost.transform.rotation = player.rotation;

        }
        else
        {
            ghost.SetActive(false);
            canBuild = false;
            //ghost.transform.position = player.position + Vector3.forward * 3f;
            //ghost.transform.rotation = player.rotation;


        }
        ConstructionStatus();
    }
    Vector3 CalculateSnapPosition(RaycastHit hit)
    {
        if (hit.transform.CompareTag("GhostConstructed"))
        {
            if (hit.transform.name == ghost.transform.name)
                return hit.transform.position;
            else
            {
                ghost.SetActive(false);
                return hit.point;

            }
        }
        Vector3 newPos = hit.point;
        switch (ghost.name) {
                case NameStatic.WoodenFloor:
                      newPos.y = newPos.y +0.1f;
                    break;
                case NameStatic.WoodenWall:
                        newPos.y = newPos.y + 1f;
                  break;
            }
        
                return newPos;
    }
    Quaternion CaculateRotate(RaycastHit hit)
    {

        if (hit.transform.CompareTag("GhostConstructed") && hit.transform.name == ghost.transform.name)
        {
            return hit.transform.rotation;
        }
        
            return player.rotation;

    }
    //public Vector3 CalculateSnapPosition(RaycastHit hit)
    //{
    //    float gridSize = 2.001f;
    //    if (hit.transform.CompareTag("Constructed"))
    //    {

    //        Vector3 center = hit.transform.position;
    //        Vector3 dir = hit.point - center;
    //        if (hit.transform.name == ghost.transform.name)
    //        {
    //            switch (ghost.transform.name)
    //            {
    //                case NameStatic.WoodenFloor:
    //                    if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
    //                    {
    //                        // Snap left / right
    //                        center.x += Mathf.Sign(dir.x) * gridSize;
    //                    }
    //                    else
    //                    {
    //                        // Snap forward / back
    //                        center.z += Mathf.Sign(dir.z) * gridSize;
    //                    }
    //                    return center;
    //                case NameStatic.WoodenWall:
    //                    if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
    //                    {
    //                        // Snap left / right
    //                        center.x += Mathf.Sign(dir.x) * gridSize;
    //                    }
    //                    else 
    //                    {
    //                        // Snap forward / back
    //                        center.y += Mathf.Sign(dir.y) * gridSize;
    //                    }
    //                    return center;
    //            }
    //        }
    //        else
    //        {
    //            switch (ghost.transform.name)
    //            {
    //                case NameStatic.WoodenFloor:
    //                    center.y += Mathf.Sign(dir.y) * gridSize;
    //                    if (Mathf.Abs(dir.z)>0)
    //                    {
    //                        // Snap left / right
    //                        center.z += Mathf.Sign(dir.z) * 1f;
    //                    }
    //                    return center;
    //                case NameStatic.WoodenWall:
    //                    if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
    //                    {
    //                        // Snap left / right
    //                        center.x += Mathf.Sign(dir.x) * 1.101f;

    //                    }
    //                    else
    //                    {
    //                        // Snap forward / back
    //                        center.z += Mathf.Sign(dir.z) * 1.101f;

    //                    }
    //                    return center;
    //            }

    //        }
    //    }
    //    return hit.point;
    //}
    //Quaternion CaculateRotate(RaycastHit hit)
    //{
    //    if (!hit.transform.CompareTag("Constructed"))
    //        return player.rotation;

    //    // wall on floor
    //    if (hit.transform.name == NameStatic.WoodenFloor &&
    //        ghost.transform.name == NameStatic.WoodenWall)
    //    {
    //        Vector3 dir = hit.point - hit.transform.position;

    //        float yRot = Mathf.Abs(dir.x) > Mathf.Abs(dir.z) ? 90f : 0f;
    //        return hit.transform.rotation * Quaternion.Euler(0, yRot, 0);
    //    }

    //    return hit.transform.rotation;
    //}
    
    void CheckLayerMaskConstruction()
    {
        if (ghost == null) return;
        Debug.Log("changeLayer");
        if (ghost.transform.name == NameStatic.WoodenFloor)
        {
            layerMask |= (1 << LayerMask.NameToLayer("GhostConstructedFloor"));
            layerMask &= ~(1 << LayerMask.NameToLayer("GhostConstructedWall"));
        }
        else if (ghost.transform.name == NameStatic.WoodenWall)
        {
            layerMask |= (1 << LayerMask.NameToLayer("GhostConstructedWall"));
            layerMask &= ~(1 << LayerMask.NameToLayer("GhostConstructedFloor"));
        }

    }
    void ConstructionStatus()
    {
        canBuild = ghost.transform.GetComponent<ConstructionCheck>().IsTriggerEmpty();
        if (canBuild)
        {
            ghost.transform.GetComponentInChildren<MeshRenderer>().material = ghostMateriakGreen;
        }
        else
        {
            ghost.transform.GetComponentInChildren<MeshRenderer>().material = ghostMateriakRed;

        }
    }
    public void Construction()
    {
        ghost.transform.GetComponentInChildren<MeshRenderer>().material = nomalMaterial;
        Destroy(ghost.GetComponent<ConstructionCheck>());
        ghost.GetComponent<Collider>().isTrigger = false;
        ghost.layer = 10;
        ghost.tag = "Constructed";
        foreach (Transform child in ghost.transform)
        { child.gameObject.SetActive(true); }
        ghost = null;
        isBuilding = false;
       
    }
}
