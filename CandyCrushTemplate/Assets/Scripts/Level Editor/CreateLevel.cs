using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CreateLevel : MonoBehaviour
{
    public Transform Canvas;
    public float CellSize;
    public bool Editing;

    [HideInInspector]
    public List<Vector2> Cells;

    [HideInInspector]
    public List<GameObject> Spots;

    public void Addcell(Vector2 deltaPos)
    {
        if (Cells == null) { Cells = new List<Vector2>(); Spots = new List<GameObject>();  }
        deltaPos.x = (int)Mathf.Floor(deltaPos.x / CellSize);
        deltaPos.y = (int)Mathf.Floor(deltaPos.y / CellSize);
        if (!Cells.Contains(deltaPos))
        {
            
            GameObject g = new GameObject("Spot");
            g.transform.parent = transform;
            Image img = g.AddComponent<Image>();
            img.color = new Color(0.8207547f, 0.4837f, 0f, 0.5333334f);
            RectTransform rt = g.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;
            rt.sizeDelta = Vector2.one * CellSize;
            rt.localPosition = deltaPos * CellSize + (Vector2.one * CellSize / 2f);
            Cells.Add(deltaPos);
            Spots.Add(g);
        }
    }

    public void Removecell(Vector2 deltaPos)
    {
        if (Cells == null) { Cells = new List<Vector2>(); Spots = new List<GameObject>(); return; }
        deltaPos.x = (int)Mathf.Floor(deltaPos.x / CellSize);
        deltaPos.y = (int)Mathf.Floor(deltaPos.y / CellSize);
        if (Cells.Contains(deltaPos))
        {
            int i = Cells.FindIndex((x) => x == deltaPos);
            DestroyImmediate(Spots[i]);
            Cells.Remove(deltaPos);
            Spots.RemoveAt(i);
        }
    }

    public void ClearLevel()
    {
        if (Cells == null) { Cells = new List<Vector2>(); Spots = new List<GameObject>(); return; }
        Cells.Clear();
        Spots.Clear();
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    public void SaveLevel()
    {
        Debug.Log(JsonUtility.ToJson(Cells));
        TextAsset tA = new TextAsset(JsonUtility.ToJson(Cells));
        AssetDatabase.CreateAsset(tA, AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Levels/Level"));
    }

}

[CustomEditor(typeof(CreateLevel))]
public class E_CreateLevel : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        if (GUILayout.Button("Clean"))
        {
            ((CreateLevel)target).ClearLevel();
        }
        if (GUILayout.Button("Save"))
        {
            ((CreateLevel)target).SaveLevel();
        }

    }

    public void OnSceneGUI()
    {
        CreateLevel creator = (CreateLevel)target;
        if(creator.Editing)
        {
            Event e = Event.current;
            if (e.type == EventType.MouseDown && (!e.control && !e.alt))
            {
                if (e.button == 0)
                {
                    RaycastHit hit;
                    if(Physics.Raycast(HandleUtility.GUIPointToWorldRay(e.mousePosition), out hit))
                    {
                        if (hit.transform.GetComponent<CreateLevel>() != null)
                        {
                            creator.Addcell(creator.transform.InverseTransformPoint(hit.point));
                            EditorUtility.SetDirty(creator.gameObject);
                        }
                    }
                }
                else if (e.button == 1)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(e.mousePosition), out hit))
                    {
                        if (hit.transform.GetComponent<CreateLevel>() != null)
                        {
                            creator.Removecell(creator.transform.InverseTransformPoint(hit.point));
                            EditorUtility.SetDirty(creator.gameObject);
                        }
                    }
                }
            }
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            
        }
        

    }

}