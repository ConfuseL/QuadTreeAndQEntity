using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QEntityManager  {
    //单例模式
    public static QEntityManager Instance { get { if (qEntityManager == null) qEntityManager = new QEntityManager();return qEntityManager; } }
    private static QEntityManager  qEntityManager;
    //用于自增的id计数
    private int cnt = 0;
    //映射的字典
    Dictionary<int, QObj> EntityMap;
    Dictionary<int, Transform> TransfromMap;
    //四叉树的根节点
    public QuadTree quadTree;

    //用于临时引用 减少内存消耗
    private QObj tempQObj;
    private Transform tempTransform;

    public QEntityManager()
    {
        EntityMap = new Dictionary<int, QObj>();
        TransfromMap = new Dictionary<int, Transform>();
    }
    public void Init(QuadTree quadTree)
    {

       this.quadTree = quadTree;
    }

    //注册一个游戏实体 
    public void Register(QObj qObj,Transform transform)
    {
        qObj.SetID(++cnt);//设置ID
        EntityMap.Add(qObj.id, qObj);//添加到字典中
        TransfromMap.Add(qObj.id, transform);
        quadTree.AddObj(qObj);//添加到树中
    }
    //在实体的循环中调用，是物体主动移动，这时候需要更改碰撞盒的位置
    public void ChangePoint( int id, Vector3 point)//改变点的信息 但不移动
    {
        tempQObj = EntityMap[id];
        tempQObj.rect.o.x = point.x;
        tempQObj.rect.o.y = point.z;
        //这个是绘制在Scene窗口的一个类
        DrawLine.Instance.ChangeADrawObj(ref id);
    }
    //在实体的碰撞修复时调用，是物体被动移动，这时候需要更改物体的位置
    public void ChangePoint(int id, Point point)//改变点的信息 移动
    {
        tempQObj = EntityMap[id];
        tempTransform = TransfromMap[id];
        tempQObj.rect.o.x = point.x;
        tempQObj.rect.o.y = point.y;
        tempTransform.position = new Vector3((float)point.x, tempTransform.position.y, (float)point.y);
        //这个是绘制在Scene窗口的一个类
        DrawLine.Instance.ChangeADrawObj(ref id);
    }
    //获得碰撞盒信息
    public QObj GetPiont(ref int id)
    {
        return EntityMap[id];
    }
}
