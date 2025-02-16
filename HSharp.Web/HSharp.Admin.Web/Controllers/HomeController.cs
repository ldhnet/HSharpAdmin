﻿using HSharp.Business.OrganizationManage;
using HSharp.Business.SystemManage;
using HSharp.Entity.OrganizationManage;
using HSharp.Entity.SystemManage;
using HSharp.Enum;
using HSharp.Model.Result;
using HSharp.Util;
using HSharp.Util.Extension;
using HSharp.Util.Model;
using HSharp.Web.Code;
using HSharp.Web.Code.State;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HSharp.Admin.Web.Controllers
{
    public class HomeController : BaseController
    {
        private MenuBLL menuBLL = new MenuBLL();
        private UserBLL userBLL = new UserBLL();
        private LogLoginBLL logLoginBLL = new LogLoginBLL();
        private MenuAuthorizeBLL menuAuthorizeBLL = new MenuAuthorizeBLL();
        private SysMessageBLL sysMessageBLL = new SysMessageBLL();

        #region 视图功能

        [HttpGet]
        [AuthorizeFilter]
        public async Task<IActionResult> Index()
        {
            OperatorInfo operatorInfo = await Operator.Instance.Current();

            TData<List<MenuEntity>> objMenu = await menuBLL.GetList(null);
            List<MenuEntity> menuList = objMenu.Data;
            menuList = menuList.Where(p => p.MenuStatus == StatusEnum.Yes.ParseToInt()).ToList();

            if (operatorInfo.IsSystem != 1)
            {
                TData<List<MenuAuthorizeInfo>> objMenuAuthorize = await menuAuthorizeBLL.GetAuthorizeList(operatorInfo);
                List<long?> authorizeMenuIdList = objMenuAuthorize.Data.Select(p => p.MenuId).ToList();
                menuList = menuList.Where(p => authorizeMenuIdList.Contains(p.Id)).ToList();
            }

            ViewBag.MenuList = menuList;
            ViewBag.OperatorInfo = operatorInfo;
            return View();
        }

        [HttpGet]
        public IActionResult Welcome()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (GlobalContext.SystemConfig.Demo)
            {
                ViewBag.UserName = "admin";
                ViewBag.Password = "123456";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginOffJson()
        {
            OperatorInfo user = await Operator.Instance.Current();
            if (user != null)
            {
                #region 退出系统

                // 如果不允许同一个用户多次登录，当用户登出的时候，就不在线了
                if (!GlobalContext.SystemConfig.LoginMultiple)
                {
                    await userBLL.UpdateUser(new UserEntity { Id = user.UserId, IsOnline = 0 });
                }

                // 登出日志
                await logLoginBLL.SaveForm(new LogLoginEntity
                {
                    LogStatus = OperateStatusEnum.Success.ParseToInt(),
                    Remark = "退出系统",
                    IpAddress = NetHelper.Ip,
                    IpLocation = string.Empty,
                    Browser = NetHelper.Browser,
                    OS = NetHelper.GetOSVersion(),
                    ExtraRemark = NetHelper.UserAgent,
                    BaseCreatorId = user.UserId
                });

                Operator.Instance.RemoveCurrent();
                new CookieHelper().RemoveCookie("RememberMe");

                return Json(new TData { Tag = 1 });

                #endregion 退出系统
            }
            else
            {
                throw new Exception("非法请求");
            }
        }

        [HttpGet]
        public IActionResult NoPermission()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpGet]
        public IActionResult Skin()
        {
            return View();
        }

        #endregion 视图功能

        #region 获取数据

        public IActionResult GetCaptchaImage()
        {
            string sessionId = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>().HttpContext.Session.Id;

            //Tuple<string, int> captchaCode = CaptchaHelper.GetCaptchaCode();
            //byte[] bytes = CaptchaHelper.CreateCaptchaImage(captchaCode.Item1);
            //new SessionHelper().WriteSession("CaptchaCode", captchaCode.Item2);

            byte[] bytes = new byte[10];
            return File(bytes, @"image/jpeg");
        }

        public async Task<ActionResult> GetMessageCount(long userId)
        {
            var obj = await sysMessageBLL.GetUnreadCount(userId);
            return Json(obj);
        }

        #endregion 获取数据

        #region 提交数据

        [HttpPost]
        public async Task<IActionResult> LoginJson(string userName, string password, string captchaCode = "")
        {
            TData obj = new TData();
            //if (string.IsNullOrEmpty(captchaCode))
            //{
            //    obj.Message = "验证码不能为空";
            //    return Json(obj);
            //}
            //if (captchaCode != new SessionHelper().GetSession("CaptchaCode").ParseToString())
            //{
            //    obj.Message = "验证码错误，请重新输入";
            //    return Json(obj);
            //}
            TData<UserEntity> userObj = await userBLL.CheckLogin(userName, password, (int)PlatformEnum.Web);
            if (userObj.Tag == 1)
            {
                await new UserBLL().UpdateUser(userObj.Data);
                await Operator.Instance.AddCurrent(userObj.Data.WebToken);
            } 
            if (userObj.Tag == 1)
            {
                Action taskAddUnreadMsg = async () =>
                { 
                    await sysMessageBLL.SendUnreadMessageToUser(userObj.Data.Id.ParseToLong());
                };
                AsyncTaskHelper.StartTask(taskAddUnreadMsg);
            }
            await logLoginBLL.SaveLoginLog(userObj.Data?.Id, userObj.Tag, userObj.Message);
            obj.Tag = userObj.Tag;
            obj.Message = userObj.Message;
            return Json(obj);
        }

        #endregion 提交数据
    }
}