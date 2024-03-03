using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "modelInfo", menuName = "3dModels")]
public class ModelInfo : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private string modelName;
    [SerializeField] private string prefabName;

    public string Id => id;
    public string Name => modelName;
    public string PrefabName => prefabName;
}
