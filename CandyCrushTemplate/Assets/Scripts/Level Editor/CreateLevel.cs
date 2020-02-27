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
    [SerializeField]
    public Hashtable Cells;


    public void Addcell(Vector2 deltaPos)
    {
        if (Cells == null) { Cells = new Hashtable(); }
        deltaPos.x = (int)Mathf.Floor(deltaPos.x / CellSize);
        deltaPos.y = (int)Mathf.Floor(-deltaPos.y / CellSize);
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
            Cells.Add(deltaPos, g);
        }
    }

    public void Removecell(Vector2 deltaPos)
    {
        if (Cells == null) { Cells = new Hashtable(); return; }
        deltaPos.x = (int)Mathf.Floor(deltaPos.x / CellSize);
        deltaPos.y = (int)Mathf.Floor(-deltaPos.y / CellSize);
        if (Cells.Contains(deltaPos))
        {
            DestroyImmediate((GameObject)Cells[deltaPos]);
            Cells.Remove(deltaPos);
        }
    }

    public void ClearLevel()
    {
        if (Cells == null) { Cells = new Hashtable(); return; }
        Cells.Clear();
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
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
        
    }

    public void OnSceneGUI()
    {
        CreateLevel creator = (CreateLevel)target;
        if(creator.Editing)
        {
            Event e = Event.current;
            if (e.type == EventType.MouseDown)
            {
                if (e.button == 0)
                {
                    RaycastHit hit;
                    if(Physics.Raycast(Camera.current.ScreenPointToRay(e.mousePosition), out hit))
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
                    if (Physics.Raycast(Camera.current.ScreenPointToRay(e.mousePosition), out hit))
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