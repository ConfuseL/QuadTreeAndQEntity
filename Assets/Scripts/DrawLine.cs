using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine: MonoBehaviour {

    public static DrawLine Instance;
    List<Vector3[]> DrawRect;
    Dictionary<int,Vector3[]> qObjs;
    Dictionary<int,int> colors;
    List<Color> colorList;
    Color temp;
    // Use this for initialization
    private void Awake()
    {
        DrawRect = new List<Vector3[]>();
        qObjs = new Dictionary<int, Vector3[]>();
        colors = new Dictionary<int, int>();
        colorList = new List<Color>();
        for (int i = 0; i < 10; i++)
        {
            colorList.Add(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
        }

        Instance = this;
        QEntityManager.Instance.Init(new QuadTree(new Rect(0,0,2000,2000), 0));
    }

	
	// Update is called once per frame
	void Update () {
		foreach(Vector3[] rects in DrawRect)
        {
            Debug.DrawLine(rects[0], rects[1]);
            Debug.DrawLine(rects[1], rects[2]);
            Debug.DrawLine(rects[2], rects[3]);
            Debug.DrawLine(rects[3], rects[0]);
        }
        foreach (KeyValuePair<int, Vector3[]> rects in qObjs)
        {
            temp = colorList[colors[rects.Key]];
            Debug.DrawLine(rects.Value[0], rects.Value[1], temp);
            Debug.DrawLine(rects.Value[1], rects.Value[2], temp);
            Debug.DrawLine(rects.Value[2], rects.Value[3], temp);
            Debug.DrawLine(rects.Value[3], rects.Value[0], temp);
        }
    }

    public void AddDrawObj(Rect rect)
    {
        Vector3[] temp = new Vector3[4];
        temp[0] = new Vector3((float)(rect.o.x-rect.Rwidth), 1, (float)(rect.o.y - rect.Rheight));
        temp[1] = new Vector3((float)(rect.o.x - rect.Rwidth), 1, (float)(rect.o.y + rect.Rheight));
        temp[2] = new Vector3((float)(rect.o.x + rect.Rwidth), 1, (float)(rect.o.y + rect.Rheight));
        temp[3] = new Vector3((float)(rect.o.x + rect.Rwidth), 1, (float)(rect.o.y - rect.Rheight));
        DrawRect.Add(temp);
    }

    public void AddDrawObj(int id,Point point,ref double w,ref double h,int deep)
    {
        if (colors.ContainsKey(id))
            colors[id] = deep;
        else
            colors.Add(id, deep);
        if (qObjs.ContainsKey(id))
            return;
        Vector3[] temp = new Vector3[4];
        temp[0] = new Vector3((float)(point.x- w), 1, (float)(point.y - h));
        temp[1] = new Vector3((float)(point.x - w), 1, (float)(point.y + h));
        temp[2] = new Vector3((float)(point.x + w), 1, (float)(point.y + h));
        temp[3] = new Vector3((float)(point.x + w), 1, (float)(point.y - h ));
        qObjs.Add(id,temp);
    }
    public void ChangeADrawObj(ref int id)
    {
        if (!qObjs.ContainsKey(id))
            return;
        QObj obj = QEntityManager.Instance.GetPiont(ref id);
        Vector3[] temp;
        qObjs.TryGetValue(id, out temp);
        temp[0].x = (float)(obj.rect.o.x - obj.rect.Rwidth);
        temp[0].z = (float)(obj.rect.o.y - obj.rect.Rheight);

        temp[1].x = (float)(obj.rect.o.x - obj.rect.Rwidth);
        temp[1].z = (float)(obj.rect.o.y + obj.rect.Rheight);

        temp[2].x = (float)(obj.rect.o.x + obj.rect.Rwidth);
        temp[2].z = (float)(obj.rect.o.y + obj.rect.Rheight );

        temp[3].x = (float)(obj.rect.o.x + obj.rect.Rwidth);
        temp[3].z = (float)(obj.rect.o.y - obj.rect.Rheight);
    }
}
