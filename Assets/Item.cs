using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private Text id;
    [SerializeField] private Text modelName;

    public string Prefab { get; private set; }
    public bool Active { get => gameObject.activeInHierarchy; set => gameObject.SetActive(value); }

    public void Select(bool value)
    {
        GetComponent<Image>().color = value ? Color.magenta : Color.white;
    }

    public void UpdateData(ModelInfo data)
    {
        id.text = data.Id;
        modelName.text = data.Name;

        Select(false);

        Prefab = data.PrefabName;
    }
}
