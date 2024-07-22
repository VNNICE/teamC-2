using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class ClearRecords : MonoBehaviour
{
    /*
     `�V���O���g�[���p�^�[���ŃN���A���Ԃ��L�^����
     */
    public static ClearRecords inst;
    private Dictionary<string, float> clearRecords = new Dictionary<string, float>();

    private string jsonPath;
    private void Awake()
    {//�d��������邽�߁E
        if (ClearRecords.inst == null)
        {
            ClearRecords.inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else 
        {
            Destroy(this.gameObject);
        }
        //����json�ŋL�^�����邱�Ƃł��ł������悤�ɂ��邽��
        jsonPath = Application.dataPath + "/Data";
        if (File.Exists(jsonPath + "/ClearRecord.json")) 
        {
            System.IO.File.ReadAllText(jsonPath + "/ClearRecord.json");
        }
    }

    public void AddClearTime(float time) 
    {
        string nowStage = SceneManager.GetActiveScene().name;
        if (clearRecords.ContainsKey(nowStage))
        {
            if (clearRecords[nowStage] > time) 
            {
                clearRecords[nowStage] = time;
            }
        }
        else
        {
            clearRecords.Add(nowStage, time);
        }
        foreach (var record in clearRecords) 
        {
            Debug.Log("Saved time: " + record);
        }
    }

    public void RecordClearTime()
    {
        string jsonData = DictionaryJsonUtility.ToJson(clearRecords, true);
        if (!Directory.Exists(jsonPath))
        {
            Directory.CreateDirectory(jsonPath);
        }
        System.IO.File.WriteAllText(jsonPath + "/ClearRecord.json", jsonData);
    }
}
