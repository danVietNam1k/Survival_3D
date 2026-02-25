using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager Instance;

    public MonoBehaviour inputController;
    public GameObject hudRoot;
    public Transform cutsceneGroup;

    [Header("Cutscene Database")]
    public CutSceneData cutSceneData;
    public List<GameObject> ListCutcsenes = new();
    public List<string> cutcseneWaitPlay = new();
    public List<string> cutscenePlayed = new();
    private bool isPlayingCutScene;
    float holdTime;
    private PlayableDirector currentCutscene;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
       foreach(Transform c in cutsceneGroup.transform)
        {
            ListCutcsenes.Add(c.gameObject);
        }
    }
    void Update()
    {
        SkipCutscene();
    }
    void SkipCutscene()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            holdTime += Time.deltaTime;
            if (holdTime > 1.5f)
                Skip();
        }
        else
        {
            holdTime = 0;
        }
    }
    public void TurnOnCutscene(string nameCutscene)
    {
        foreach (GameObject c in ListCutcsenes) {
        if(c.name == nameCutscene)
            {
                c.SetActive(true);  
            }
        }

    }
    public void OnCutsceneStart()
    {
        inputController.enabled = false;
        hudRoot.SetActive(false);
    }

    public void OnCutsceneEnd()
    {
        inputController.enabled = true;
        hudRoot.SetActive(true);

    }
    public void PlayCutscene(string cutsceneID)
    {
        if (isPlayingCutScene || ThisIsPlayed(cutsceneID)) return;

        PlayableDirector data = ListCutcsenes.Find(c => c.name == cutsceneID).GetComponent<PlayableDirector>();
        cutscenePlayed.Add(cutsceneID);
        if (data == null)
        {
            Debug.LogWarning($"? Cutscene not found: {cutsceneID}");
            return;
        }
        else
        {
            currentCutscene = data;
            StartCutscene();
        }
    }
    bool ThisIsPlayed(string cutsceneID)
    {
        foreach (string c in cutscenePlayed)
        {
            if (cutsceneID == c)
            {
                return true;
            }
        }
        return false;   
    }
    private void StartCutscene()
    {
        isPlayingCutScene = true;
        currentCutscene.stopped += OnCutsceneFinished;

        currentCutscene.Play();

        OnCutsceneStart();
    }
    public void Skip()
    {
        if (!isPlayingCutScene || currentCutscene == null) return;

        currentCutscene.time = currentCutscene.duration;
        currentCutscene.Evaluate();
        EndCutscene();
    }
    private void EndCutscene()
    {
        currentCutscene.stopped -= OnCutsceneFinished;


        isPlayingCutScene = false;
        currentCutscene = null;
        OnCutsceneEnd();
    }
    private void OnCutsceneFinished(PlayableDirector director)
    {
        EndCutscene();
    }

}
