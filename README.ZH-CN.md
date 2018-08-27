# Saga基本使用指南

## 使用前置条件说明

> 如果还有同学对Saga还不甚了解的同学,可以参考Saga官方中文地址[https://github.com/apache/incubator-servicecomb-saga/blob/master/README_ZH.md](https://github.com/apache/incubator-servicecomb-saga/blob/master/README_ZH.md),同时可以参考此项目贡献者之一的[@ithLin](https://github.com/WithLin)的一篇中文说明文章,该地址如下:[https://www.jianshu.com/p/dfe7a4c05992](https://www.jianshu.com/p/dfe7a4c05992),文章由浅入深的讲述了分布式事务在微服务场景下的重要性,以及Saga对分布式事务的大致实现方式和后续的思考

* **必须** 你需要可用的一个本地或者远程的数据库(mysql或者postpresql)作为Saga持久化分布式事务事件的持久化存储,当然只要官方支持的Database Provider即可,具体idea数据库配置如下图,注意数据库的名字与您真实数据库名一致

![alpha-server-database-setting.png](imgs/alpha-server-database-setting.png)

* **必须** 成功启动alpha-server,考虑到目前官方并没有给出docker image,导致环境搭建以及部署颇为麻烦,后期官方将会提供image上传docker hub提供给大家使用,启动成功参考下图

![alpha-server.png](imgs/alpha-server.png)

* **可选** 同时saga提供了UI可视化界面,直接idea中启动saga-web即可

## 开始玩转分布式事务Saga

克隆当前项目,然后请使用VS2017打开解决方案定位到sample目录,你会看到如下所示的三个实例应用程序,这里app都是基于TargetFramework=netcoreapp2.0的,所以需要相应的启动环境

![sample-app-test.png](imgs/sample-app-test.png)

下面对上图做一个基本介绍,假定现在我们有三个微服务,分别是 Booking-预定服务,Car-订车服务,Hotel-酒店服务,相信大家一看便知,三者的从属关系以及在现实社会中的关联关系,下面我们的分布式事务一致性测试将在这几个app中完成,现在我们分别对项目做一定的初始化工作

```csharp
services.AddOmegaCore(option =>
{
    // your alpha-server address
    option.GrpcServerAddress = "localhost:8080";
    // your app identification
    option.InstanceId = "Booking123";
    // your app name
    option.ServiceName = "Booking";
});
```

这里需要对三个项目都做如上所示的基本配置即可,现在一直都配置就绪了,下面开始我们的分布式事务的测试吧...

## 分布式事务场景测试

下面将会针对正常以及异常情况分别测试

### 正常情况测试

> 由 Booking 发起预定汽车和预定酒店的服务,且假设三个服务均可以正常访问的情况,正常启动如下图所示:
![apps.png](imgs/apps.png)

```csharp
        [HttpGet, SagaStart]
        [Route("book")]
        public ActionResult Book()
        {
            // init basic httpclient
            var httpClient = new HttpClient();
            // mark a reservation of car
            httpClient.GetAsync("http://localhost:5002/api/values");
            // book a hotel
            httpClient.GetAsync("http://localhost:5003/api/values");
            // your busniess code
            // for example save the order to your database
            return Ok("ok");
        }
```