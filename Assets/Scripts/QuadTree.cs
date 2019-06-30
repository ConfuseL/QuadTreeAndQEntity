using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 四叉树节点
/// </summary>
public class QuadTree
{
    //当自身存储数量达到多少的时候 开始划分
    public int maxSum;
    //四个子区域
    public QuadTree[] childs;
    //存储的数量
    public List<QObj> objs;
    //当前该节点代表的的深度
    public int deep;
    //自己的位置信息
    public Rect rect;

    private QuadTree father;

    public QuadTree(Rect rect,int deep, QuadTree fa=null)
    {
        father = fa;
        this.rect = rect;  
        this.deep = deep + 1;
        objs = new List<QObj>();
        maxSum = 4;
        DrawLine.Instance.AddDrawObj(this.rect);
    }

    public void AddObj(QObj obj)
    {
        //这个是调用绘制类在scene窗口绘制矩形 可以无视 后面会给实现
        DrawLine.Instance.AddDrawObj(obj.id, obj.rect.o, ref obj.rect.Rwidth, ref obj.rect.Rheight, deep);
        obj.locate = this;//设置这个碰撞盒的所在节点 为当前四叉树节点
        if (childs == null)//如果子节点不存在 说明还没有分割
        {
            objs.Add(obj);//添加到list中
            if (deep <= 5 && objs.Count > maxSum)//如果深度到达某个阈值(这里是5)，并且管理的物体对象数量超过最大值
                Split();//调用分割函数 下面给出实现
        }
        else //如果子节点存在，说明这个四叉树节点不是叶子节点
        {

            int index = GetIndex(obj);//获取所在对应子节点的想象
            if (index >= 0)
            {
                childs[index].AddObj(obj);//调用该子节点的添加方法
            }
            else//如果象限小于 那就是这个物体在分界线上，因此属于该节点管理
                objs.Add(obj);
        }

    }

    public void Split()
    {
        childs = new QuadTree[4];
        //第一象限
        childs[0] = new QuadTree(new Rect(rect.o.x + rect.Rwidth / 2, rect.o.y + rect.Rheight / 2, rect.Rwidth, rect.Rheight), deep, this);
        //第二象限
        childs[1] = new QuadTree(new Rect(rect.o.x - rect.Rwidth / 2, rect.o.y + rect.Rheight / 2, rect.Rwidth, rect.Rheight), deep, this);
        //第三象限
        childs[2] = new QuadTree(new Rect(rect.o.x - rect.Rwidth / 2, rect.o.y - rect.Rheight / 2, rect.Rwidth, rect.Rheight), deep, this);
        //第四象限
        childs[3] = new QuadTree(new Rect(rect.o.x + rect.Rwidth / 2, rect.o.y - rect.Rheight / 2, rect.Rwidth, rect.Rheight), deep, this);

        //开始将当前管理的物体交给子节点
        for (int i = objs.Count - 1; i >= 0; i--)
        {
            int index = GetIndex(objs[i]);
            if (index >= 0)//只有象限大于的才给子节点，否则还是自己保管
            {
                childs[index].AddObj(objs[i]);
                objs.Remove(objs[i]);
            }
        }
    }

    public int GetIndex(QObj target)
    {
        //如果在分界轴上，返回-1
        if (Mathf.Abs((float)(target.rect.o.x - rect.o.x)) < target.rect.Rwidth || Mathf.Abs((float)(target.rect.o.y - rect.o.y)) < target.rect.Rheight)
            return -1;
        for (int i = 0; i < 4; i++)//遍历四个子节点，调用isInclude去判断
        {
            if (childs[i].rect.IsInclude(target.rect))
                return i;//返回对应的象限号
        }
        return -1;
    }

    public void IsHere(QObj target)
    {
        //判断是否包含在矩形范围内
        if (rect.IsInclude(target.rect))
        {
            //如果包含，就查出可不可以添加到子节点里
            if (childs == null)
                return;
            int index = GetIndex(target);
            if (index >= 0)
            {
                if (objs.Count > 0)
                    for (int i = objs.Count - 1; i >= 0; i--)
                    {
                        if (target.id == objs[i].id)
                        {
                            childs[index].AddObj(objs[i]);//添加到子节点
                            objs.Remove(objs[i]);//从自身移除
                            break;
                        }
                    }
            }
        }
        else//不在矩形范围内 说明物体可能移动出去了
        {

            if (objs.Count > 0)
                for (int i = objs.Count - 1; i >= 0; i--)
                {
                    if (target.id == objs[i].id)//尝试找到是不是在自身的管理物体列表里 如果存在 那么的确是从自身范围移动出去的
                    {
                        objs.Remove(objs[i]);//移除
                        if (father != null)
                            father.AddObj(target);//添加到父物体中 调用AddObj的时候 会自动添加到父物体对应的子节点里
                        break;
                    }
                }
        }
    }




    public void Collide(QObj obj,ref Point rem)
    {
            for (int i = objs.Count - 1; i >= 0; i--)
            {
            if (obj.id!= objs[i].id)
            {
                obj.Collide(objs[i], ref rem);
            }
            }
            if(childs!=null)
            foreach(QuadTree child in childs)
            {
                for (int i = child.objs.Count - 1; i >= 0; i--)
                {
                    if (obj.id != child.objs[i].id )
                    {
                        obj.Collide(child.objs[i], ref rem);
                    }
                }
            }
            if(father!=null)
            {
            for (int i = father.objs.Count - 1; i >= 0; i--)
            {
                if (obj.id != father.objs[i].id )
                {
                    obj.Collide(father.objs[i], ref rem);

                }
            }
        }
    }
}
