using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class QObj
{
    //碰撞矩阵 
    public Rect rect;
    //id
    public int id { private set; get; }
    //当前所在的四叉树节点
    public QuadTree locate;
    public Action<QEntity,QEntity> OnCollision;
    public Action<QEntity, QEntity> OnTrigger;
    public bool isTrigger = false;
    public QEntity qEntity=null;

    public QObj(int id,Point o, double w, double h)
    {
        this.id = id;
        rect = new Rect(o, w, h);
    }

    public QObj(Point o, double w, double h)
    {
        rect = new Rect(o, w, h);
    }
    public void SetID(int id)
    {
        this.id = id;
    }

    public void ISOutOfLocate()
    {
        if(locate!=null)
        locate.IsHere(this);
    }

    public bool Collide(QObj obj,ref Point rem)
    {

             float offsetX = (float)((rect.o.x - obj.rect.o.x)/ (obj.rect.Rwidth + rect.Rwidth)), offsetY = (float)((rect.o.y - obj.rect.o.y)/ (obj.rect.Rheight + rect.Rheight));
            if (Mathf.Abs(offsetX) < 1 && Mathf.Abs(offsetY) < 1)
            {

            if (!isTrigger &&!obj.isTrigger)
            {
                rem = rect.o;
                if (Mathf.Abs(offsetX) >= Mathf.Abs(offsetY))
                {
                    rem.x = obj.rect.o.x;
                    if(offsetX>=0)
                        rem.x+=(obj.rect.Rwidth + rect.Rwidth);
                    else
                        rem.x -= (obj.rect.Rwidth + rect.Rwidth);
                }
                else
                {
                    rem.y = obj.rect.o.y;
                    if (offsetY >= 0)
                        rem.y+=(obj.rect.Rheight + rect.Rheight);
                    else
                        rem.y -= (obj.rect.Rheight + rect.Rheight);
                }

                QEntityManager.Instance.ChangePoint(id, rem);
                if(OnCollision!=null)
                {
                    OnCollision(qEntity,obj.qEntity);
                }
                if (obj.OnCollision != null)
                {
                    obj.OnCollision(qEntity, obj.qEntity);
                }
            }
            if(obj.OnTrigger !=null)
            {
                obj.OnTrigger(qEntity,obj.qEntity);
            }
            return true;
        }
            return false;
    }
}



public class QEntity : MonoBehaviour {

    public QObj self;
    Point remain;//用来恢复的位置
    [Range(1, 1000)]
    public double weight;
    [Range(1, 1000)]
    public double hight;
    public bool isTrigger { set { self.isTrigger = value; } get { return self.isTrigger; } }
    private void Awake()
    {
        //优先读取图片的宽高
        if(GetComponent<SpriteRenderer>()!=null)
        {
            self = new QObj(new Point(transform.position.x, transform.position.z), GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.z );
        }
        else
        self = new QObj(new Point(transform.position.x, transform.position.z), weight, hight);
        self.isTrigger = isTrigger;
        self.qEntity = this;
        //初始化的时候，记录位置
        remain = new Point(self.rect.o.x, self.rect.o.y);
    }
    private void Start()
    {
        //向管理脚本注册
        QEntityManager.Instance.Register(self,transform); ;
    }

    private void FixedUpdate()
    {
        //主动更改位置信息
        QEntityManager.Instance.ChangePoint(self.id, transform.position);
        //判断是否超出当前节点位置
        self.ISOutOfLocate();
        //碰撞检测
        self.locate.Collide(self,ref remain);
    }
    private void OnDestroy()
    {
        self.qEntity = null;//解除循环引用
        self = null;
    }
}
