using NetData;
using UnityEngine;

namespace H4ck.h4ckFuncs
{
    //一键举报
    class Report : MonoBehaviour
    {
        void Update()
        {
            if (!Check.CheckIsLoadOver) { return; }
            if (Input.GetKey(KeyCode.W)) //按住W时触发
            {
                Report_();
            }
        }
        void Report_()
        {
            BattleServerContext _battleServerContext = Contexts.sharedInstance.battleServer;
            ReportRequest reportRequest = new ReportRequest();

            foreach (PlayerEntity entity in GetData.players)
            {
                //外挂上报
                reportRequest.CidList.Add(entity.basicInfo.Cid);
                //去掉自己，否则将会连自己一起举报
                reportRequest.CidList.Remove(GetData.me.basicInfo.Cid);
                //原因0 = 外挂上报
                reportRequest.Reason = 0;
                //发送举报
                _battleServerContext.battleServer.Server.SendTcpMessage(20, reportRequest);

                //原理相同

                //言语辱骂
                reportRequest.CidList.Add(entity.basicInfo.Cid);
                reportRequest.CidList.Remove(GetData.me.basicInfo.Cid);
                reportRequest.Reason = 4;
                _battleServerContext.battleServer.Server.SendTcpMessage(20, reportRequest);

            }
        }
    }
}
