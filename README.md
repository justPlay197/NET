# NET
### **1、课程项目要求**
    1. 系统至少有五个程序集，需要系统进行模块划分
    2. 需要其中一个是共享程序集
    3. 采用c++/CLI实现一个程序集，给c#使用
    4. 使用c++实现一部分功能，输出为一个Win32DLL，尽量采用多种方式调用其中的函数（互操作）
    5. 实现一个简单的COM组件，然后进行使用
    6. 要求使用多线程技术和线程池技术
    7. 利用ASP.NET、ADO.NET完成基本的处理
    8. 最好利用.NET其他技术进行实践
### **2、项目概述**
#### **项目简介：**  
    本项目是一个二手书交易平台，用户可自由发布商品或购买商品
#### **核心功能**
    用户模块：
        1. 注册：邮箱、手机必须符合格式
        2、登陆：账号密码必须已经注册过
        3、查看个人信息：在个人账户板块可以看到自己的个人信息
        4、修改个人信息：在个人账户板块可以修改自己的个人信息，包括头像
        5、修改默认收货地址：在个人账户板块可以修改默认的收货地址
    浏览商品模块
        1、浏览商品：用户在首页可以看到一些推荐的书本
        2、分类查看商品：用户可以在导航栏选择类别来分类查看书本
        3、搜索商品：用户可以在搜索框输入关键字进行模糊搜索
        4、查看商品详情：用户点击书本可以进入书本详情页，包含库存、介绍等信息
    购买商品模块
        1、添加购物车：用户在商品详情页可以添加书本至购物车
        2、提交订单：用户可以在购物车界面进行书本结算
        3、支付订单：用户可以在我的订单购买板块进行订单支付或取消
        4、评价：订单成交后用户可以进行书本评价
    出售商品模块
        1、发布商品：用户可以在发布商品板块发布自己的商品
        2、查看已发布商品：用户可以在已发布商品板块查看所有发布过的商品
        3、接受订单：用户可以在我的订单出售板块查看自己的出售订单
### **3、主体框架说明**
    1. 主体框架：ASP.NET，前端模板为cshtml，除了基本的controllers和models，我们还在两层中间加了一层业务逻辑层  
    2. 数据库访问：EF5.0 + Linq  
### **4、所有程序集：位于lib文件夹中**   
  
|ID|功能|文件名|命名空间|编写语言|程序集类型|备注|
|---|---|---|----|---|---|---|
|1|邮箱验证|EmailService.dll|DLL.EmailService|c#|私有程序集|要在web.config配置|
|2|验证码生成|RandomCode.dll|DLL.RandomCode|c#|共享程序集|-|
|3|加密|EntryptAndDetrypt.dll|DLL.EntryptAndDetrypt|c#|私有程序集|-|
|4|上传图片|UploadFile.dll|DLL.UploadFile|c#|私有程序集|-|
|5|格式验证|Verify.dll|DLL.Verify|c#|私有程序集|-|
|6|格式验证|CLRDLL.dll|CLRDLL|c++/CLI|私有程序集|-|
|7|格式验证|CppDLL.dll|-|c++win32DLL|动态链接库|DLL要放到运行环境|
|8|硬编码|MSGBUS.dll|-|ATLCOM|COM组件|js和c#都有调用|

#### **1、共享程序集RandomCode说明**
1. 生成命令  
![Alt text](https://github.com/justPlay197/NET/blob/master/images/%E6%B7%BB%E5%8A%A0%E5%85%B1%E4%BA%AB%E7%A8%8B%E5%BA%8F%E9%9B%86.png?raw=true)  

2. c#中调用情况：可以看出这是一个共享程序集，且拥有公钥  
![Alt text](https://github.com/justPlay197/NET/blob/master/images/%E5%85%B1%E4%BA%AB%E7%A8%8B%E5%BA%8F%E9%9B%86RandomCode.png?raw=true)

#### **2、c#调用c++/cli托管c++时遇到的问题**
    问题一  ：当c++/cli中参数为string时，c#处显示类型为问号，即未知类型  
    解决办法：修改参数类型为char\*传入后再转为string  
  
    问题二  ：c++的char\*对应c#中的sbyte\*，而sbyte\*无法直接获取  
    解决办法：代码如下  
```
unsafe
    {
        string str = "15311111111";
        byte[] bb = Encoding.Default.GetBytes(str);
        sbyte[] sbb = new sbyte[bb.Length];
        Buffer.BlockCopy(bb, 0, sbb, 0, bb.Length);
        fixed (sbyte* sb = sbb)
        {
            Console.WriteLine(verify.IsHandset(sb));
        }
    }
```
#### **3、c++动态链接库遇到的问题**
    note:c++动态链接库所放位置：C:\\Program Files (x86)\\IIS Express —— 运行时的位置  
    
    错误类型1:运行的时候报错，错误为Debug Assertion Fail
    问题原因1:查阅资料后为指针非法使用问题，但我并没有用指针，最后锁定为字符串传入问题
    解决办法1:c++参数类型改为char*，csharp传入为byte[]

    错误类型2:运行崩溃显示堆栈不对称
    问题原因2:堆栈清理者设定出错
    解决办法2:在dllimport的时候指定堆栈清理方为Cdecl

    错误类型3:运行正常，但返回的结果永远是true，明显应该返回false的情况也返回true
    问题原因3:非托管代码数据传输问题
    解决办法3:在dllimport和函数声明中间加一句[return:MarshalAs(UnmanagedType.I1)

#### **5、COM组件注册和调用问题**
    错误类型1：ATL项目直接运行报错，报错为unable to start pargram...
    解决办法1：以管理员的身份打开cmd，利用regsvr32命令注册COM组件
    
    错误类型2：csharp创建COM实例时报错 “无法嵌入互操作类型，请改用适当的接口”
    解决办法2：右键添加的COM组件，选择属性，修改嵌入式操作类型为false
    
    错误类型3：JS调用COM组件没有反应
    尝试解决3：改用IE打开项目，仍然没有反应
    解决办法3：修改IE安全级别，允许ActiveX控件
### **4、项目启动问题**
    问题描述：无法启动计算机'.'上的服务W3SVC（IIS无法开启）
      
      尝试1 ：启动服务W3SCV，报错依赖服务或组无法启动
      尝试2 ：查看依赖服务，发现HTTP服务没有开启，开启HTTP服务报错无法启动HTTP
      尝试3 ：直接修改注册表，然后重启，成功
        
      3.1：win + R，运行命令regedit打开注册表编辑器
      3.2：进入路径： &\SYSTEM\CurrentControlSet\Services\HTTP]:
      3.3：修改start的状态从4变为3
      3.4：重启，重新加载注册表
![Alt text](https://github.com/justPlay197/NET/blob/master/images/%E5%BC%80%E5%90%AFHTTP%E6%9C%8D%E5%8A%A1.png?raw=true)  

### **5、多线程和线程池**
```
考虑发送邮件可能需要消耗一定的时间，且用户点击注册后无需等待直接跳到邮箱验证部分，所以可以将发送邮件的功能放入线程池进行运算
//代码如下
ThreadPool.QueueUserWorkItem(sendEmail,"");
```
### **6、发布时遇到的问题**
    1.1 访问报错1：500.19，原因是项目访问权限不足
    1.2 解决报错1:500.19，右键发布的物理路径->属性->安全->编辑添加一个新组->给他所有的权限
    
    2.1 访问报错2：HTTP500，内部服务器错误，原因是64位的机器调用的程序集是32位的，不兼容
    2.2 解决报错2：在IIS中选择应用程序池->右键网站->高级设置->把启用32位应用程序设为true
    
    3.1 访问报错3：COM权限问题，net应用程序无法访问COM组件
    3.2 解决报错3：找到注册的COM组件，右键属性添加一个everyone的组（参考报错1的解决方案）
