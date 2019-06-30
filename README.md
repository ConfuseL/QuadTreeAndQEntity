# QuadTreeAndQEntity
# 简单实现Unity2D游戏中的四叉树空间管理和矩形碰撞检测

![](https://github.com/ConfuseL/QuadTreeAndQEntity/blob/master/Gif/ConfuseL%E5%9B%9B%E5%8F%89%E6%A0%91%E4%B8%8E%E7%A2%B0%E6%92%9E%E6%A3%80%E6%B5%8B.gif?raw=true)

- 采用的坐标轴是Unity中的X轴(水平)与Z轴(垂直)
- 为物体添加QEnity脚本，初始化时可自动识别精灵图的宽高设置碰撞盒或者在监视面板设定
- 需要一个载体添加DrawLine脚本，在上面设置你的四叉树根节点的坐标和范围以及最大容量
- 模仿实现Unity中的Collision和Trigger方法，但需要用委托注册这些方法

[相关思路的讲解在这篇博客上](http://confusel.tech/2019/MyLearn-QTree1/)
