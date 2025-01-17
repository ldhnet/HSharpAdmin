﻿using HSharp.Util;
using HSharp.Util.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HSharp.Admin.Web.Controllers
{
    public class FileController : BaseController
    {
        #region 上传单个文件

        [HttpPost]
        [AuthorizeFilter]
        public async Task<TData<string>> UploadFile(int fileModule, IFormCollection fileList)
        {
            TData<string> obj = await FileHelper.UploadFile(fileModule, fileList.Files);
            return obj;
        }

        #endregion 上传单个文件

        #region 删除单个文件

        [HttpPost]
        [AuthorizeFilter]
        public TData<string> DeleteFile(int fileModule, string filePath)
        {
            TData<string> obj = FileHelper.DeleteFile(fileModule, filePath);
            return obj;
        }

        #endregion 删除单个文件

        #region 下载文件

        [HttpGet]
        public FileContentResult DownloadFile(string filePath, int delete = 1)
        {
            TData<FileContentResult> obj = FileHelper.DownloadFile(filePath, delete);
            if (obj.Tag == 1)
            {
                return obj.Data;
            }
            else
            {
                throw new Exception("下载失败：" + obj.Message);
            }
        }

        #endregion 下载文件
    }
}