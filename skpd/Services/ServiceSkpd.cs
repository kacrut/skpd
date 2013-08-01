using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using skpd.DTO;
using skpd.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.Transactions;
using System.Data;
using System.Configuration;
using skpd.Services;
using System.Web.Routing;

namespace skpd.Services
{
    public class ServiceSkpd : IServiceSkpd
    {
        //private ESKAPEDEContext db = new ESKAPEDEContext();

        public bool isAlreadyApproveOrReject(int RequestID, int ApprovalPosistionID, int FlagID)
        {
            bool isValid = false;
            using (var db = new ESKAPEDEContext())
            {
                var x = db.RequestLogs.Where(a => a.RequestID == RequestID && a.ApprovalPositionID == ApprovalPosistionID && a.FlagID == FlagID).Count();
                if (x > 0) return isValid = true;
            }
            
            return isValid;
        }


        public bool isRequestIDExist(int RequestID)
        {
            bool isValid = false;
            using (var db = new ESKAPEDEContext())
            {
                var x = db.RequestLogs.Where(a => a.RequestID == RequestID).Count();
                if (x > 0) return isValid = true;
            }

            return isValid;
        }


        public string ParentPositionName(int PositionID)
        {
            var ParentPositionName = string.Empty;
            using (var db = new ESKAPEDEContext())
            {
                var ParentPositionID = db.Positions.Where(a => a.PositionID == PositionID).Select(a => a.AssignID).Single();
                ParentPositionName = db.Positions.Where(a => a.PositionID == ParentPositionID).Select(a => a.PositionName).Single();
            }
            return ParentPositionName.ToString();
        }


        public string QueryStringEncrypt(string param)
        {
            var query = QueryStringModule.Encrypt(param);
            return query;
        }


        public string QueryStringDecrypt(string param)
        {
            var query = QueryStringModule.Decrypt(param);
            return query;
        }


        public int ParentPositionID(int PositionID)
        {
            using (var db = new ESKAPEDEContext())
            {
                var ParentPosisitionID = db.Positions.Where(a => a.PositionID == PositionID).Select(a => a.AssignID).Single();
                return ParentPosisitionID;
            }
        }


        public bool SendEmailResultToRequesterCcApprover(Request request, ESKAPEDEContext Db, IMessengerService MessengerService)
        {
            var isSuccess = false;
            try
            {
                //SEND EMAIL TO REQUESTER Cc APPROVER
                var RequestID = request.RequestID.ToString();
                var fromFirstName = Db.Users.Where(a => a.PositionID == request.ApprovalPositionID).Select(a => a.FirstName).SingleOrDefault();
                var fromLastName = Db.Users.Where(a => a.PositionID == request.ApprovalPositionID).Select(a => a.LastName).SingleOrDefault();
                if (fromLastName == null) fromLastName = "";
                var fromfullName = string.Format("{0}.{1}", fromFirstName.ToUpper(), fromLastName.ToUpper());
                var toFirstName = Db.Users.Where(a => a.Username == request.CreatedBy).Select(a => a.FirstName).SingleOrDefault();
                var toLastName = Db.Users.Where(a => a.Username == request.CreatedBy).Select(a => a.LastName).SingleOrDefault();
                var tofullName = string.Format("{0}.{1}", toFirstName.ToUpper(), toLastName.ToUpper());
                var fromAddress = ConfigurationManager.AppSettings["Sender"];
                var toAddress = Db.Users.Where(a => a.ID == request.UserID).Select(a => a.Email).SingleOrDefault();
                var ccAddress = ccEmail(request.RequestID);
                var subject = string.Format("Hasil Pengajuan Perjalanan Dinas Dari {0}", tofullName);
                var appSkpdUrl = ConfigurationManager.AppSettings["hostUrl"];
                var body = string.Format("<p>Kepada Yth,<br /> Bapak/Ibu {0}<br /><br />" +
                                        "<p>No. Perjadin : {1}</p> <br />" +
                                        "<p>Pengajuan perjalanan dinas anda telah DISETUJUI oleh Bapak/Ibu {2}</p><br />" +
                                        "<p>Untuk informasi lengkap <a href=\"{3}\">login</a></p>",
                                        tofullName.ToUpper(), RequestID, fromfullName, appSkpdUrl);

                if (MessengerService.Send(fromAddress, toAddress, subject, body, true, ccAddress))
                {
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return isSuccess;
        }

        public bool SendEmailToApproverFromRequester(string hostUrl,Request request, ESKAPEDEContext Db,IMessengerService MessengerService)
        {
            var isSuccess = false;
            try
            {
                var RequestID = request.RequestID ;
                var fromFirstName = Db.Users.Where(a => a.Username == request.CreatedBy).Select(a => a.FirstName).SingleOrDefault();
                var fromLastName = Db.Users.Where(a => a.Username == request.CreatedBy).Select(a => a.LastName).SingleOrDefault();
                if (fromLastName == null) fromLastName = "";
                var fromfullName = string.Format("{0}.{1}", fromFirstName.ToUpper(), fromLastName.ToUpper());
                var fromAddress = ConfigurationManager.AppSettings["Sender"];
                var fromPositionName = Db.Positions.Where(a => a.PositionID == request.PositionID).Select(a => a.PositionName).SingleOrDefault();
                var fromDivisionName = Db.Positions.Where(a => a.PositionID  == request.PositionID).Select(a => a.Unit.Division.DivisionName).SingleOrDefault();
                var fromCountryName = Db.Positions.Where(a => a.PositionID  == request.PositionID).Select(a => a.Country.CountryName).SingleOrDefault();
                var fromDesti = Db.Countries.Where(a => a.CountryID == request.FromCountryID).Select(a => a.CountryName).SingleOrDefault();
                var toDesti = Db.Countries.Where(a => a.CountryID == request.ToCountryID).Select(a => a.CountryName).SingleOrDefault();
                var fromDate = request.StartDate.Date.ToShortDateString();
                var toDate = request.EndDate.Date.ToShortDateString();
                var skpdEventName = request.EventName;
                var skpdEndDate = request.EndDate.Date.ToShortDateString();
                var toAddress = Db.Users.Where(a => a.PositionID == request.ApprovalPositionID).Select(a => a.Email).SingleOrDefault();
                var toName = Db.Users.Where(a => a.PositionID == request.ApprovalPositionID).Select(a => a.Username).SingleOrDefault();
                var skpdRequestID = request.RequestID.ToString();
                var QueryParam = string.Format("UserName={0}&RequestID={1}", toName, skpdRequestID);
                var QueryParamEncrypt = QueryStringEncrypt(QueryParam);
                var approveUrl = hostUrl + VirtualPathUtility.ToAbsolute(string.Format("~/FromEmail/Approve{0}", QueryParamEncrypt));
                var rejectUrl = hostUrl + VirtualPathUtility.ToAbsolute(string.Format("~/FromEmail/Reject{0}", QueryParamEncrypt));
                var subject = string.Format("Pengajuan Permohonan Perjalanan Dinas Dari Bapak/Ibu {0}", fromfullName);
                IEnumerable<RequestInTransport> ListTrans = Db.RequestInTransports.Include("Transport").Where(a => a.RequestID == request.RequestID);
                var anggaran = Db.RequestInPrograms.Where(a => a.RequestID == request.RequestID).Select(a => a.Program.ProgramName).FirstOrDefault();

                //IEnumerable<Transport> transport = db.Transports;
                var StrListTrans = "<ul>";
                foreach (var item in ListTrans)
                {
                    StrListTrans += string.Format("<li>{0}</li>", item.Transport.TransportName);
                }
                StrListTrans = string.Format("{0}</ul>", StrListTrans);

                var body = string.Format("Kepada Yth,<br />" +
                                        "Bapak/Ibu. {0} <br /><br/>" +
                                        "Permohonan Persetujuan Perjalanan Dinas : " +
                                        "<br/><br/>No. Perjadin: {1} " +
                                        "<br/>Nama: {2} " +
                                        "<br/>Posisi: {3} " +
                                        "<br/>Divisi : {4} " +
                                        "<br/>Kantor : {5} " +
                                        "<br/>Kegiatan : {6} " +
                                        "<br/>Perjalanan : {7} - {8} " +
                                        "<br/>Tanggal: {9} - {10}" +
                                        "<br/>Transportasi : {11} " +
                                        "<br/>Beban Anggaran : {12} " +
                                        "<br/><br /> Apakah Permohonan perjalanan dinas ini di SETUJUI atau TIDAK DISETUJUI ? " +
                                        "<br/><br/><p><strong><a href=\"{13}\">DISETUJUI</a></strong></p> " +
                                        "<br/><br/><p><strong><a href=\"{14}\">TIDAK DISETUJUI</a></strong></p>",
                                        toName.ToUpper(), skpdRequestID, fromfullName, fromPositionName, fromDivisionName,
                                        fromCountryName, skpdEventName, fromDesti, toDesti, fromDate, toDate,
                                        StrListTrans, anggaran, approveUrl, rejectUrl);

                if (MessengerService.Send(fromAddress, toAddress, subject, body, true))
                {
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                //Log exception
                ex.ToString();
            }

            return isSuccess;
        }


        //public bool ExclusiveApprover(int ApprovalPositionID)
        //{
        //    var ApprovalPositionName = "";
        //    bool PositionName = false;
        //    string[] Exclusive;
        //    Exclusive = new string[2];
        //    Exclusive[0] = "KADIV";
        //    Exclusive[1] = "KA. KPM";
        //    List<string> _Exclusive = new List<string>(Exclusive);
        //    using (var db = new ESKAPEDEContext())
        //    {
        //        ApprovalPositionName = db.Positions.Where(a => a.PositionID == ApprovalPositionID).Select(a => a.PositionName).First();
        //    }
        //    if (_Exclusive.Any(s => ApprovalPositionName.Contains(s)))
        //    {
        //        PositionName = true;
        //    }
        //    return PositionName;
            
        //}


        public bool SendEmailRejectToRequesterCcApprover(Request request, ESKAPEDEContext Db, IMessengerService MessengerService)
        {
            var isSuccess = false;
            try
            {
                var RequestID = request.RequestID;
                var fromUsername = Db.Users.Where(a => a.PositionID == request.ApprovalPositionID).Select(a => a.Username).SingleOrDefault();
                var fromFirstName = Db.Users.Where(a => a.Username == fromUsername).Select(a => a.FirstName).SingleOrDefault();
                var fromLastName = Db.Users.Where(a => a.Username == fromUsername).Select(a => a.LastName).SingleOrDefault();
                if (fromLastName == null) fromLastName = "";
                var fromfullName = string.Format("{0}.{1}", fromFirstName.ToUpper(), fromLastName.ToUpper());
                var fromAddress = ConfigurationManager.AppSettings["Sender"];
                var toAddress = Db.Users.Where(a => a.PositionID == request.ApprovalPositionID).Select(a => a.Email).SingleOrDefault();
                var toFirstName = Db.Users.Where(a => a.Username == request.CreatedBy).Select(a => a.FirstName).SingleOrDefault();
                var toLastName = Db.Users.Where(a => a.Username == request.CreatedBy).Select(a => a.LastName).SingleOrDefault();
                var tofullName = string.Format("{0}.{1}", toFirstName.ToUpper(), toLastName.ToUpper());
                //var ccAddress = Db.Users.Where(a => a.PositionID == request.ApprovalPositionID).Select(a => a.Email).Single();
                var ccAddress = ccEmail(request.RequestID);
                var subject = string.Format("Hasil Permohonan Pengajuan Perjalan Dinas Dari {0}", fromfullName);
                var appSkpdUrl = ConfigurationManager.AppSettings["hostUrl"];
                var fromReason = request.RejectedReason;
                var body = string.Format("<p>Kepada Yth,<br />" +
                                        "Bapak/Ibu {0}<br /><br />" +
                                        "No. Perjadin : {1} <br />" +
                                        "TIDAK DISETUJUI Oleh Bapak/Ibu {2}</p><br />" +
                                        "Dengan Alasan : {3}" +
                                        "<p>Untuk informasi lengkap <a href='{4}'>login</a></p>",
                                        tofullName.ToUpper(), RequestID, fromfullName, request.RejectedReason, appSkpdUrl);
                if (MessengerService.Send(fromAddress, toAddress, subject, body, true, ccAddress))
                {
                    isSuccess = true;
                }
                
            }
            catch (Exception ex)
            {
                //Log exception
                ex.ToString();
            }

            return isSuccess;
        }


        public bool SendEmailRequestBudgetToDivisionFromRequester(string hostUrl, Request request, ESKAPEDEContext Db, IMessengerService MessengerService)
        {
            var isSuccess = false;
            try
            {
                var fromUserprofile = Db.vwUserProfiles.Where(a => a.Username == request.CreatedBy).FirstOrDefault();
                var toUserprofile = Db.vwUserProfiles.Where(a => a.PositionID == request.ApprovalPositionID).FirstOrDefault();
                var RequestID = request.RequestID;
                //var fromFirstName = Db.Users.Where(a => a.Username == request.CreatedBy).Select(a => a.FirstName).SingleOrDefault();
                //var fromLastName = Db.Users.Where(a => a.Username == request.CreatedBy).Select(a => a.LastName).SingleOrDefault();
                //if (fromLastName == null) fromLastName = "";
                var fromfullName = fromUserprofile.fullname;
                var fromAddress = ConfigurationManager.AppSettings["Sender"];
                var fromPositionName = fromUserprofile.PositionName;
                var fromDivisionName = fromUserprofile.DivisionName;
                var fromCountryName = fromUserprofile.CountryName;
                var fromDesti = Db.Countries.Where(a => a.CountryID == request.FromCountryID).Select(a => a.CountryName).SingleOrDefault();
                var toDesti = Db.Countries.Where(a => a.CountryID == request.ToCountryID).Select(a => a.CountryName).SingleOrDefault();
                var fromDate = request.StartDate.Date.ToShortDateString();
                var toDate = request.EndDate.Date.ToShortDateString();
                var skpdEventName = request.EventName;
                var skpdEndDate = request.EndDate.Date.ToShortDateString();
                var toAddress = Db.Users.Where(a => a.PositionID == request.ApprovalPositionID).Select(a => a.Email).SingleOrDefault();
                var toName = Db.Users.Where(a => a.PositionID == request.ApprovalPositionID).Select(a => a.Username).SingleOrDefault();
                var skpdRequestID = request.RequestID.ToString();
                var QueryParam = string.Format("UserName={0}&RequestID={1}", toName, skpdRequestID);
                var QueryParamEncrypt = QueryStringEncrypt(QueryParam);
                var approveUrl = hostUrl + VirtualPathUtility.ToAbsolute(string.Format("~/FromEmail/Approve{0}", QueryParamEncrypt));
                var rejectUrl = hostUrl + VirtualPathUtility.ToAbsolute(string.Format("~/FromEmail/Reject{0}", QueryParamEncrypt));
                var subject = string.Format("Pengajuan Permohonan Perjalanan Dinas Dari Bapak/Ibu {0}", fromfullName);
                IEnumerable<RequestInTransport> ListTrans = Db.RequestInTransports.Include("Transport").Where(a => a.RequestID == request.RequestID);
                var anggaran = Db.RequestInPrograms.Where(a => a.RequestID == request.RequestID).Select(a => a.Program.ProgramName).FirstOrDefault();
                var StrListTrans = "<ul>";
                foreach (var item in ListTrans)
                {
                    StrListTrans += string.Format("<li>{0}</li>", item.Transport.TransportName);
                }
                StrListTrans = string.Format("{0}</ul>", StrListTrans);

                //var budgetDivisionName = Db.Positions.Include("Division").Where(a=>a.PositionID == request.ApprovalPositionID).Select(a=>a.Division.DivisionName).SingleOrDefault();
                var budgetDivisionName = Db.vwUserProfiles.Where(a => a.PositionID == request.ApprovalPositionID).Select(a => a.DivisionName).SingleOrDefault();
                //var BudgetID = Db.RequestInPrograms.Where(a => a.RequestID == request.RequestID).Select(a=>a.ProgramID).First();
                //var BudgetName = Db.RequestInPrograms.Include("RefBudget").Where(a => a.RequestID == request.RequestID).Select(a => a.RefBudget.BudgetName).First().ToUpper();
                //var body = string.Format("Dear, Mr./Mrs. {0} <br /><br/>" +
                //                        "Request for approval of official travel : " +
                //                        "<br/><br/>RequestID: {1} " +
                //                        "<br/>Name: {2} " +
                //                        "<br/>Position: {3} " +
                //                        "<br/>Destination : {4} " +
                //                        "<br/>EventName : {5} " +
                //                        "<br/>Transport : {6} " +
                //                        "<br/>Star Date : {7} " +
                //                        "<br/>End Date : {8} " +
                //                        "<br/>Budget Divison : {9} " +
                //                        "<br/><br /> whether you would approve or reject this request ? " +
                //                        "<br/><br/><p><strong><a href=\"{10}\">APPROVE</a></strong></p> " +
                //                        "<br/><br/><p><strong><a href=\"{11}\">REJECT</a></strong></p>",
                //                        toName.ToUpper(), skpdRequestID, fromfullName, fromPositionName, skpdDestination,
                //                        skpdEventName, StrListTrans, skpdStarDate, skpdEndDate, budgetDivisionName, approveUrl, rejectUrl);
                var body = string.Format("Kepada Yth,<br />" +
                                        "Bapak/Ibu. {0} <br /><br/>" +
                                        "Permohonan Persetujuan Penggunaan Beban Anggaran Perjalanan Dinas : " +
                                        "<br/><br/>No. Perjadin: {1} " +
                                        "<br/>Nama: {2} " +
                                        "<br/>Posisi: {3} " +
                                        "<br/>Divisi : {4} " +
                                        "<br/>Kantor : {5} " +
                                        "<br/>Kegiatan : {6} " +
                                        "<br/>Perjalanan : {7} - {8} " +
                                        "<br/>Tanggal: {9} - {10}" +
                                        "<br/>Transportasi : {11} " +
                                        "<br/>Beban Anggaran : {12} " +
                                        "<br/><br /> Apakah Permohonan Penggunaan Beban Anggaran perjalanan dinas ini di SETUJUI atau TIDAK DISETUJUI ? " +
                                        "<br/><br/><p><strong><a href=\"{13}\">DISETUJUI</a></strong></p> " +
                                        "<br/><br/><p><strong><a href=\"{14}\">TIDAK DISETUJUI</a></strong></p>",
                                        toName.ToUpper(), skpdRequestID, fromfullName, fromPositionName, fromDivisionName,
                                        fromCountryName, skpdEventName, fromDesti, toDesti, fromDate, toDate,
                                        StrListTrans, anggaran, approveUrl, rejectUrl);

                if (MessengerService.Send(fromAddress, toAddress, subject, body, true))
                {
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                //Log exception
                ex.ToString();
            }

            return isSuccess;
        }


        public bool isApproverExclusive(int ApprovalPositionID)
        {
            var isSuccess = false;
            try
            {

                var ApprovalPositionName = "";
                string[] Exclusive;
                Exclusive = new string[1];
                Exclusive[0] = "KADIV";
                List<string> _Exclusive = new List<string>(Exclusive);
                using (var db = new ESKAPEDEContext())
                {
                    ApprovalPositionName = db.vwUserProfiles.Where(a => a.PositionID == ApprovalPositionID).Select(a => a.PositionTypeName).SingleOrDefault();
                }
                if (_Exclusive.Any(s => ApprovalPositionName.Contains(s)))
                {
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                //Log exception
                ex.ToString();
            }

            return isSuccess;
        }

        public string[] ccEmail(int requestID)
        {
            using (var db = new ESKAPEDEContext())
            {
                string[] _ccEmail = (from reglog in db.RequestLogs
                                     join user in db.Users on reglog.ApprovalPositionID equals user.PositionID
                                     where reglog.RequestID == requestID
                                     select user.Email).Distinct().ToArray();
                return _ccEmail;
            }
        }
    }
}