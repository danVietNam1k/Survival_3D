using System.Collections.Generic;
using UnityEngine;

public class TreeReplacer_Optimized : MonoBehaviour
{
    [Header("References")]
    public Terrain terrain;
    public Camera playerCamera;

    [Header("Interaction")]
    public KeyCode replaceKey = KeyCode.G;
    public float interactionDistance = 5f;
    public float findRadius = 3f;

    [Header("Performance")]
    public float cellSize = 10f;
    public float despawnDistance = 50f;

    [System.Serializable]
    public class TreeReplacement
    {
        public string treeName;
        public GameObject replacementPrefab;
    }

    public TreeReplacement[] replacements;

    TerrainData tData;
    Transform camTf;

    Dictionary<string, GameObject> replaceDict;
    Dictionary<Vector3Int, List<TreeData>> treeGrid = new();
    HashSet<int> choppedTreeIDs = new();
    List<TreeData> activeTrees = new();

    class TreeData
    {
        public int id;
        public Vector3 worldPos;
        public TreeInstance original;
        public GameObject spawned;
    }

    void Start()
    {
        camTf = playerCamera.transform;
        tData = terrain.terrainData;

        replaceDict = new();
        foreach (var r in replacements)
            replaceDict[r.treeName] = r.replacementPrefab;

        BuildGrid();
    }

    void Update()
    {
        
            TryChopTree();

        CheckDespawn();
    }

    // ---------------- GRID ----------------

    void BuildGrid()
    {
        var trees = tData.treeInstances;

        for (int i = 0; i < trees.Length; i++)
        {
            Vector3 worldPos = Vector3.Scale(trees[i].position, tData.size) + terrain.transform.position;
            var cell = WorldToCell(worldPos);

            if (!treeGrid.TryGetValue(cell, out var list))
            {
                list = new();
                treeGrid[cell] = list;
            }

            list.Add(new TreeData
            {
                id = i,
                worldPos = worldPos,
                original = trees[i]
            });
        }
    }

    Vector3Int WorldToCell(Vector3 pos) =>
        new(Mathf.FloorToInt(pos.x / cellSize), 0, Mathf.FloorToInt(pos.z / cellSize));

    // ---------------- CHOP ----------------

    void TryChopTree()
    {
        Ray ray = new(camTf.position, camTf.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance)) return;

        TreeData tree = FindNearestTree(hit.point);
        if (tree == null) return;
        if (choppedTreeIDs.Contains(tree.id)) return;

        string name = tData.treePrototypes[tree.original.prototypeIndex].prefab.name;
        if (!replaceDict.TryGetValue(name, out var prefab)) return;

        SpawnTree(tree, prefab);
    }

    TreeData FindNearestTree(Vector3 point)
    {
        float minDist = findRadius;
        TreeData closest = null;

        var center = WorldToCell(point);

        for (int x = -1; x <= 1; x++)
            for (int z = -1; z <= 1; z++)
            {
                var cell = new Vector3Int(center.x + x, 0, center.z + z);
                if (!treeGrid.TryGetValue(cell, out var list)) continue;

                foreach (var t in list)
                {
                    if (choppedTreeIDs.Contains(t.id)) continue;

                    float d = Vector2.Distance(
                        new(point.x, point.z),
                        new(t.worldPos.x, t.worldPos.z));

                    if (d < minDist)
                    {
                        minDist = d;
                        closest = t;
                    }
                }
            }
        return closest;
    }

    void SpawnTree(TreeData tree, GameObject prefab)
    {
        Quaternion rot = Quaternion.Euler(0, tree.original.rotation * Mathf.Rad2Deg, 0);
        Vector3 scale = new(tree.original.widthScale, tree.original.heightScale, tree.original.widthScale);

        var go = Instantiate(prefab, tree.worldPos, rot);
        go.transform.localScale = Vector3.Scale(go.transform.localScale, scale);

        tree.spawned = go;
        choppedTreeIDs.Add(tree.id);
        activeTrees.Add(tree);
    }

    // ---------------- DESPAWN ----------------

    void CheckDespawn()
    {
        for (int i = activeTrees.Count - 1; i >= 0; i--)
        {
            var t = activeTrees[i];
            if (Vector3.Distance(camTf.position, t.worldPos) > despawnDistance)
            {
                Destroy(t.spawned);
                activeTrees.RemoveAt(i);
                choppedTreeIDs.Remove(t.id);
            }
        }
    }
}
