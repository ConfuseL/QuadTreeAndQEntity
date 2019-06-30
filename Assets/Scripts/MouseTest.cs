using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTest : MonoBehaviour {
    Ray ray;
     RaycastHit hit;
    public GameObject Stone;
    public GameObject Floor;
    int cnt = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            //从摄像机发出射线的点
             ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
               if(hit.collider.tag.CompareTo("BackGround")==0)
                {
                   if(cnt++%2==0)
                    {
                        GameObject instance = Instantiate(Stone, new Vector3(hit.point.x, 10, hit.point.z), new Quaternion());
                        instance.transform.eulerAngles = new Vector3(90, 0, 0);
                        instance.AddComponent<QEntity>();
                        instance.GetComponent<QEntity>().self.OnCollision += TestCollision;
                    }
                   else
                    {
                        GameObject instance = Instantiate(Floor, new Vector3(hit.point.x, 10, hit.point.z), new Quaternion());
                        instance.transform.eulerAngles = new Vector3(90, 0, 0);
                        instance.AddComponent<QEntity>();
                        instance.GetComponent<QEntity>().isTrigger = true;
                        instance.GetComponent<QEntity>().self.OnTrigger += TestTrigger;
                    }

                }
            }
        }
	}

    public void TestCollision(QEntity self,QEntity target)
    {
        Debug.Log(self.name+ "与" + target.name + "发生碰撞");
    }

    public void TestTrigger(QEntity self, QEntity target)
    {
        Debug.Log(self.name + "被" + target.name + "穿过");
    }
}
