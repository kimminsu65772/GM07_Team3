using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Build.Content;

public class FloatingDmgTextPoolManager : Singleton<FloatingDmgTextPoolManager>
{
    [SerializeField] private FloatingDmgText prefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private string rootName = "FloatingDmgTextPool_Root";

    private readonly Queue<FloatingDmgText> pool = new();
    private Transform root;

    protected override void Awake()
    {
        base.Awake();
        CreateRoot();
        Preload();
    }

    private void CreateRoot()
    {
        GameObject rootObj = new GameObject(rootName);
        rootObj.transform.SetParent(transform);
        root = rootObj.transform;
    }

    private void Preload()
    {
        for (int i = 0; i < poolSize; i++)
        {
            FloatingDmgText dmgText = CreateNew();
            Return(dmgText);
        }
    }

    public FloatingDmgText GetText()
    {
        // 풀이 비어있다면 새롭게 생성
        if (pool.Count == 0)
        {
            return CreateNew();
        }

        FloatingDmgText dmgText = pool.Dequeue();
        dmgText.MarkOutOfPool();
        dmgText.ResetState();
        dmgText.gameObject.SetActive(true);

        return dmgText;
    }

    public void Return(FloatingDmgText dmgText)
    {
        if (dmgText == null) return;

        if (dmgText.IsInPool)
        {
            Debug.LogWarning($"{dmgText.name}은 이미 풀에 반환되었습니다.");
            return;
        }

        dmgText.ResetState();
        dmgText.MarkInPool();
        dmgText.gameObject.SetActive(false);
        dmgText.transform.SetParent(root);
        pool.Enqueue(dmgText);
    }

    private FloatingDmgText CreateNew()
    {
        FloatingDmgText dmgText = Instantiate(prefab, root);
        dmgText.Init();
        dmgText.gameObject.SetActive(false);

        return dmgText;
    }
}
