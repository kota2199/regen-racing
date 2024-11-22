using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarColorChanger : MonoBehaviour
{
    [SerializeField]
    private Material[] bodyMaterials;
    [SerializeField]
    private Material[] TyreMaterials;
    [SerializeField]
    private Material[] WingMaterials;

    [SerializeField]
    private MeshRenderer bodyObj, WingObj;
    [SerializeField]
    private MeshRenderer[] Wheels;

    public enum CarInfo
    {
        Human,AI
    }

    public CarInfo carInfo;

    [SerializeField]
    private RaceData raceData;

    public int carColorIndex;

    // Start is called before the first frame update
    void Start()
    {
        if(carInfo == CarInfo.Human)
        {
            SetCarColor(raceData.playerColorIndex);
        }
        else
        {
            SetCarColor(Random.Range(0, bodyMaterials.Length));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCarColor(int index)
    {
        bodyObj.material = bodyMaterials[index];
        WingObj.material = WingMaterials[index];
        for(int i = 0; i < Wheels.Length; i++)
        {
            Wheels[i].material = TyreMaterials[index];
        }

        carColorIndex = index;
    }
}
