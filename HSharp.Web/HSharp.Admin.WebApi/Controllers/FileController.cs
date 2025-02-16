﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HSharp.Util;
using HSharp.Util.Extension;
using HSharp.Util.Model;
using HSharp.Enum;

namespace HSharp.Admin.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [AuthorizeFilter]
    public class FileController : ControllerBase
    {
        #region 上传单个文件
        /// <summary>
        /// 工作流上传文件专用
        /// 支持格式 .jpg|.jpeg|.gif|.png|.xls|.xlsx|.doc|.docx|.pdf|.txt
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TData<string>> Upload(IFormFile file)
        { 
            TData<string> obj = await FileHelper.HSharpFileUpload(file);
            obj.Data = "http://117.72.70.166:9001" + obj.Data;
            return obj;
        }
        
        [HttpPost]
        public async Task<TData<string>> UploadFile(int fileModule, IFormCollection fileList)
        {
            TData<string> obj = await FileHelper.UploadFile(fileModule, fileList.Files);
            return obj;
        }
        #endregion

        #region 删除单个文件
        [HttpPost]
        public TData<string> DeleteFile(int fileModule, string filePath)
        {
            TData<string> obj = FileHelper.DeleteFile(fileModule, filePath);
            return obj;
        }
        #endregion

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
        #endregion
    }
}