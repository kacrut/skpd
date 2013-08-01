using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using skpd.Models;

namespace skpd.Services
{
    public interface IServiceSkpd
    {
        bool isAlreadyApproveOrReject(int RequestID, int ApprovalPosistionID, int FlagID);
        bool isRequestIDExist(int RequestID);
        string ParentPositionName(int PositionID);
        int ParentPositionID(int PositionID);
        string QueryStringEncrypt(string param);
        string QueryStringDecrypt(string param);
        bool SendEmailResultToRequesterCcApprover(Request request, ESKAPEDEContext db, IMessengerService MessengerService);
        bool SendEmailToApproverFromRequester(string hostUrl, Request request, ESKAPEDEContext db, IMessengerService MessengerService);
        bool SendEmailRejectToRequesterCcApprover(Request request, ESKAPEDEContext db, IMessengerService MessengerService);
        bool SendEmailRequestBudgetToDivisionFromRequester(string hostUrl, Request request, ESKAPEDEContext db, IMessengerService MessengerService);
        bool isApproverExclusive(int id);
        string[] ccEmail(int requestID);
    }
}
