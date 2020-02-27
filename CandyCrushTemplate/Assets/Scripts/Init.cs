using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform LevelParent;
    public int CellSize;
    public GameObject CandyPreFab;

    public Sprite[] BaseCandies;

    private Vector2 parentSize;

    void Start()
    {
        SetParams();

        fillLevel();
    }

    private void SetParams()
    {
        parentSize = LevelParent.sizeDelta;
    }

    private void fillLevel()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
