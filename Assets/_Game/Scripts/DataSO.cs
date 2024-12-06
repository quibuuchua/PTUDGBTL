using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommonEnum;

[CreateAssetMenu(menuName = "Data/Color Data", fileName = "Color Data")]

public class DataSO : ScriptableObject
{
    [SerializeField] private List<Material> materials = new List<Material>();

    public CommonEnum.ColorType color;

    public Material GetMaterial(CommonEnum.ColorType color) => materials[(int)color];

}


