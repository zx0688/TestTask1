using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Button add;
    [SerializeField] private Button remove;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform listContainer;
    [SerializeField] private Transform modelContainer;
    [SerializeField] private float rotationSpeed = 30f;

    public Action OnListUpdated;

    private ModelInfo[] models;
    private List<Item> items;
    private Item current = null;

    private GameObject preview;
    private Dictionary<string, GameObject> pool;

    void Awake()
    {
        items = new List<Item>();
        pool = new Dictionary<string, GameObject>();
    }

    void Start()
    {
        models = Resources.LoadAll<ModelInfo>("");

        add.onClick.AddListener(OnAdd);
        remove.onClick.AddListener(OnRemove);

        OnListUpdated += OnUpdateSelected;

    }

    private void OnUpdateSelected()
    {
        items.ForEach(i => i.Select(false));

        if (current != null)
            current.Select(true);
    }

    void Update()
    {
        if (preview != null && preview.activeSelf)
            preview.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnRemove()
    {
        Item item = null;
        for (int i = items.Count - 1; i >= 0; i--)
            if (items[i].Active == true)
            {
                item = items[i];
                break;
            }

        if (item == null)
            return;

        item.Active = false;

        if (current == item)
            UpdatePreview(item);

        OnListUpdated?.Invoke();
    }

    private void OnAdd()
    {
        int randomIndex = UnityEngine.Random.Range(0, models.Length);
        ModelInfo randomModel = models[randomIndex];

        Item cached = items.Find(i => i.gameObject.activeInHierarchy == false);
        if (cached == null)
        {
            GameObject newItem = Instantiate(itemPrefab, listContainer);
            cached = newItem.GetComponent<Item>();
            items.Add(cached);
            cached.gameObject.GetComponent<Button>().onClick.AddListener(() => OnSelect(cached));
        }

        cached.UpdateData(randomModel);
        cached.Active = true;

        OnListUpdated?.Invoke();
    }

    private void OnSelect(Item item)
    {
        current = item;
        item.Select(true);
        UpdatePreview(item);

        OnUpdateSelected();
    }

    private void UpdatePreview(Item item)
    {
        preview?.SetActive(false);

        if (!item.gameObject.activeSelf)
            return;

        GameObject current = null;
        if (!pool.TryGetValue(item.Prefab, out current))
        {
            GameObject prefab = Resources.Load<GameObject>(item.Prefab);
            current = Instantiate(prefab, modelContainer);
            pool.Add(item.Prefab, current);
        }

        preview = current;
        preview.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        preview.SetActive(true);
    }
}
