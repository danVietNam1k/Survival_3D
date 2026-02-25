using Mono.Reflection;
using Palmmedia.ReportGenerator.Core.Reporting.Builders.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public Canvas canvasLoadingScene;
    private bool isLoading;
    private void Awake()
    {
        if(Instance != null && Instance != this  ) Destroy(this.gameObject);
        else Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (Input.anyKeyDown && isLoading)
        {
            Time.timeScale = 1.0f;
            canvasLoadingScene.gameObject.SetActive(false);
            isLoading = false;
        }
    }

    // Json project save path
    string jsonPathProject;
    // json External/Real save path
    string jsonPathPersistent;

    string fileName = "SaveGame";
    public void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar ;
        jsonPathPersistent = Application.persistentDataPath + Path.AltDirectorySeparatorChar ;
        

    }
   
    #region --------Save And Load Section----------------
    public void SaveGame(int slotNumber)
    {
        AllGameData data = new AllGameData();
        data.playerData = GetPlayerData();

        data.enviromentData = GetEnviromentData();

        SaveGameDataToJsonFile(data, slotNumber);
    }

  
    #region -----------Player Data------------
    private PlayerData GetPlayerData()
    {
        // player status 
        float[] playerStates = new float[3];
        playerStates[0] = PlayerState.Instance.currentHeal;
        playerStates[1] = PlayerState.Instance.currentCalories;
        playerStates[2] = PlayerState.Instance.currentHydration;
        //player position 
        float[] playerPosAndRot = new float[7]; 
        playerPosAndRot[0] = PlayerState.Instance.playerBody.position.x;
        playerPosAndRot[1] = PlayerState.Instance.playerBody.position.y;
        playerPosAndRot[2] = PlayerState.Instance.playerBody.position.z;
        //player rotation 
        playerPosAndRot[3] = PlayerState.Instance.playerBody.rotation.x;
        playerPosAndRot[4] = PlayerState.Instance.playerBody.rotation.y;
        playerPosAndRot[5] = PlayerState.Instance.playerBody.rotation.z;
        playerPosAndRot[6] = PlayerState.Instance.playerBody.rotation.w;
        //Inventory
        string[] inventory = InventorySystem.Instance.itemList.ToArray();
        string[] inQuickSlots = InventorySystem.Instance.itemInQuickSlotList.ToArray();
        return new PlayerData(playerStates, playerPosAndRot, inventory, inQuickSlots);  
    }
   
    void SetPlayerData(PlayerData playerData)
    {
        Transform player = PlayerState.Instance.playerBody;
        player.GetComponent<CharacterController>().enabled = false;
        #region ------SetPlayerData----
        // status 
        PlayerState.Instance.currentHeal = playerData.playerStates[0];
        PlayerState.Instance.currentCalories = playerData.playerStates[1];
        PlayerState.Instance.currentHydration = playerData.playerStates[2];
        // position
        Vector3 loadPos;
        loadPos.x = playerData.playerPositionAndRotation[0];
        loadPos.y = playerData.playerPositionAndRotation[1];
        loadPos.z = playerData.playerPositionAndRotation[2];
        player.position = loadPos;
        //rotation
        
        Quaternion loadRot;
        loadRot.x = playerData.playerPositionAndRotation[3];
        loadRot.y = playerData.playerPositionAndRotation[4];
        loadRot.z = playerData.playerPositionAndRotation[5];
        loadRot.w = playerData.playerPositionAndRotation[6];
        player.rotation = loadRot;
        // inventory
        foreach (string item in playerData.inventoryContent)
        {
            InventorySystem.Instance.AddItemToInventoryAndPopup(item, false);
        }
        //in quick slots
        foreach (string item in playerData.inQuickSlotContent)
        {
            InventorySystem.Instance.AddItemtoQuickSlot(item);
        }
        #endregion
        player.GetComponent<CharacterController>().enabled = true;


    }
    #endregion

    #region -----------Enviroment Data---------------
    private EnvironmentData GetEnviromentData()
    {
        List<string> constructedName = new();
        List<float> constructedPos = new();
        List<float> constructedRot = new();
        Transform constructedArea = ConstructionManager.Instance.constructedArea;
       
        foreach (Transform child in constructedArea)
        {
            constructedName.Add(child.name);
            constructedPos.Add(child.position.x);
            constructedPos.Add(child.position.y);
            constructedPos.Add(child.position.z);
            constructedRot.Add(child.eulerAngles.x);
            constructedRot.Add(child.eulerAngles.y);
            constructedRot.Add(child.eulerAngles.z);
        }
        
        return new EnvironmentData(constructedName, constructedPos, constructedRot);
    }

    private void SetEnviromentData(EnvironmentData environmentData)
    {
        int i = 1;
        Vector3 oldPos = new();
        Vector3 oldRot = new();
        
        foreach (string name in environmentData.constructedName)
        {
            oldPos.x = environmentData.constructedpos[i - 1];
            oldPos.y = environmentData.constructedpos[i];
            oldPos.z = environmentData.constructedpos[i + 1];
            oldRot.x = environmentData.constructedRot[i - 1];
            oldRot.y = environmentData.constructedRot[i];
            oldRot.z = environmentData.constructedRot[i + 1];
            ConstructionManager.Instance.StartConstruction(name);
            ConstructionManager.Instance.ghost.transform.SetPositionAndRotation(oldPos, Quaternion.Euler(oldRot));
            ConstructionManager.Instance.Construction();
            i += 3;
        }
    }
    #endregion

    public void LoadGame(int slotNumber)
    {
        StartCoroutine(WaitLoadSceneToLoadData( slotNumber));
    }
    IEnumerator WaitLoadSceneToLoadData(int slotNumber)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("MainGamePlayScene");
        EnableLoadingScene();
        while (!op.isDone)
            yield return null;

        yield return null;
        LoadSaveData(slotNumber);
        DisableLoadingScene();
        //StartLoadedGame();
        //yield return new WaitForEndOfFrame();

        //LoadSaveData(slotNumber);
    }
    void LoadSaveData(int slotNumber)
    {   if (slotNumber == -1) return;// new game
        AllGameData allGameData = LoadGameDataFromJsonFile(slotNumber);
        // Player Data
        SetPlayerData(allGameData.playerData);
        SetEnviromentData(allGameData.enviromentData);
    }
    #endregion

    #region  -------------Setting Section--------------------
    [System.Serializable]
    public class VolumeSettings
    {
        public float music;
        public float effects;
        public float master;
    }
    public void SaveVolumeSettings(float _music, float _effects, float _master)
    {
        VolumeSettings volumeSettings = new VolumeSettings()
        {
            music = _music,
            effects = _effects,
            master = _master
        };
        PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSettings));
        PlayerPrefs.Save();
    }
    public VolumeSettings LoadVolumeSettings()
    {
        var settings = JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
        return settings;
    }
    #endregion

    #region ----------- Save To Json Section---------
    public void SaveGameDataToJsonFile(AllGameData gameData, int slotNumber)
    {
        string json = JsonUtility.ToJson(gameData);
        //print("ToJson: " + json);

        string encrypted = EncryptionDecryption(json);
        //print("encrypted: " + encrypted);

        using (StreamWriter writer = new StreamWriter(jsonPathProject+ fileName+ slotNumber+".json"))
        {
            writer.Write(json);
            //print("Saved Game to json file at: "+ jsonPathProject + fileName + slotNumber + ".json");
        }
    }

    public AllGameData LoadGameDataFromJsonFile(int slotNumber)
    {
        using (StreamReader reader = new StreamReader(jsonPathProject + fileName + slotNumber + ".json"))
        {
            string json = reader.ReadToEnd();
            //print("reader: " + json);

            string decrypted = EncryptionDecryption(json);
            //print("decrypted: " +decrypted);
            AllGameData gameData = JsonUtility.FromJson<AllGameData>(json);
            return gameData;
        }
    }
    #endregion

    #region ------------Encryption---------
    public string EncryptionDecryption(string data)
    {
        string keyWord = "1234567";
        string result = "";
        for(int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ keyWord[i % keyWord.Length]);

        }
        return result;
    }
    #endregion

    #region ----------Utility-----------
    public bool DoesFileExists(int slotNumber)
    {
        if (System.IO.File.Exists(jsonPathProject + fileName + slotNumber + ".json"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
    #region ----------Scene Loading section-----------
    public void EnableLoadingScene()
    {
       canvasLoadingScene.gameObject.SetActive(true);
        TextMeshProUGUI newTxt = canvasLoadingScene.GetComponentInChildren<TextMeshProUGUI>();
        newTxt.text = "Loading...";
        isLoading =true;
        print("Load start");


    }
    public void DisableLoadingScene()
    {
        if (!isLoading) return;
        TextMeshProUGUI newTxt = canvasLoadingScene.GetComponentInChildren<TextMeshProUGUI>();
        newTxt.text = "Press any button...";
        Time.timeScale = 0;

        print("Load done");
        
       
    }

    #endregion
}
