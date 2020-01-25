using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Utility;
using static AutoTool.JsonPath;

namespace AutoTool
{
    public class WebRequestManager
    {
        public bool LOG = false;

        public ToolSetting Setting;

        public WebRequestManager(ToolSetting setting)
        {
            this.Setting = setting;
        }

        public const string PRESIGN = "/data/upload/presign_urls";

        public const string PRESIGN_SAVE = "/data/upload/presign_url_save";

        public const string UPLOAD = "/data/avatars/{0}/upload";

        public const string DATA_AVATAR = "/data/avatars/{0}";

        public const string CREATE_AVATAR = "/data/create_avatar";

        private Create.Request CreateRequest(FilesItem file)
        {
            var req = new Create.Request
            {
                filename = file.name_file.Split('.')[0],

                path = file.path,
                target = file.GetTarget(),
                type = file.GetFileType(),
                version = Setting.Version,

                md5 = GetMd5(file)
            };
            return req;
        }

        private PMd5 GetMd5(FilesItem item)
        {
            var md5 = new PMd5
            {
                android = GetMd5ByFile(item.android),
                ios = GetMd5ByFile(item.ios),
                standalonewindows = GetMd5ByFile(item.standalonewindows),
                webgl = GetMd5ByFile(item.webgl)
            };
            return md5;
        }

        private string GetMd5ByFile(string path)
        {
            return Md5Tool.GetMd5Hash(File.ReadAllBytes(Path.Combine(Setting.Path, path)));
        }

        public async Task UploadFile(FilesItem file)
        {
            ("Begin:" + file.path).Print(ConsoleColor.White);
            var crm = CreateRequest(file);
            var cReq = await BeginRequest<Create.Response, Create.Request>(CREATE_AVATAR, crm);

            //asset_icon/ab_ios_file/ab_android_file/ab_webgl_file/ab_windows_file/ab_osx_file/fbx_file


            if (crm.md5.ios != cReq.md5.ios || Setting.Force)
            {
                await UploadStep(cReq.id, file.name_file, file.ios, "ab_ios_file");
            }
            else
            {
                "ab_ios_file File is same".Print();
            }
            if (crm.md5.android != cReq.md5.android || Setting.Force)
            {
                await UploadStep(cReq.id, file.name_file, file.android, "ab_android_file");
            }
            else
            {
                "ab_android_file File is same".Print();
            }

            if (crm.md5.webgl != cReq.md5.webgl || Setting.Force)
            {
                await UploadStep(cReq.id, file.name_file, file.webgl, "ab_webgl_file");
            }
            else
            {
                "ab_webgl_file File is same".Print();
            }

            if (crm.md5.standalonewindows != cReq.md5.standalonewindows || Setting.Force)
            {
                await UploadStep(cReq.id, file.name_file, file.standalonewindows, "ab_windows_file");
            }
            else
            {
                "ab_windows_file File is same".Print();
            }
        }

        private async Task<Upload.Response> UploadStep(string asset_id, string fileName, string filePath, string fileType)
        {
            ("FileType:" + fileType + " filePath:" + filePath).Print(ConsoleColor.Yellow);
            var req = new Sign.Request { category = "3d_asset", keep_name = true, filename = fileName, given_fold = null };
            var res = await BeginRequest<Sign.Response, Sign.Request>(PRESIGN, req);
            await UploadFileRequest(res.url, filePath, res.content_type, res.method);

            var s_req = new Sign_Save.Request { save_key = res.save_key, url = res.visit_url, acl_private = false };
            var save_res = await BeginRequest<Sign_Save.Response, Sign_Save.Request>(PRESIGN_SAVE, s_req);
            var upload = await BeginRequest<Upload.Response, Upload.Request>(
                string.Format(UPLOAD, asset_id),
                new Upload.Request
                {
                    filetype = fileType,
                    key = save_res.key
                });
            return upload;
        }

        private async Task<string> UploadFileRequest(string url, string filePath, string contType, string method)
        {
            //("Upload file :" + url).Print( ConsoleColor.Green); 
            using (var http = new HttpClient())
            {
                using (var file = new FileStream(Path.Combine(Setting.Path, filePath), FileMode.Open))
                {
                    HttpResponseMessage message = null;
                    var content = new StreamContent(file);
                    content.Headers.ContentType = new MediaTypeHeaderValue(contType);
                    switch (method.ToLower())
                    {
                        case "put":
                            message = await http.PutAsync(url, content);
                            break;

                    }

                    if (message == null)
                        throw new Exception("unsupport method:" + method);
                    if (!message.IsSuccessStatusCode)
                    {
                        throw new Exception("Error Code:" + message.StatusCode);
                    }
                    var res = await message.Content.ReadAsStringAsync();
                    //("Upload res :" + res).Print(ConsoleColor.Green); ;
                    return res;
                }

            }

        }

        private async Task<Response> BeginRequest<Response, Request>(string url, Request req)
            where Response : class
            where Request : class

        {
            var json = Tool.ToJson(req);
            url = Setting.HttpHost + url;
            if (LOG)
                ("Request:" + url + "->" + json).Print(ConsoleColor.Green);
            using (var http = new HttpClient(new HttpClientHandler { UseCookies = false }))
            {
                HttpResponseMessage message;
                var content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                content.Headers.Add("Cookie", Setting.User);
                message = await http.PostAsync(url, content);

                if (message.IsSuccessStatusCode)
                {
                    var res = await message.Content.ReadAsStringAsync();
                    if (LOG)
                        ("Result:" + res).Print(ConsoleColor.Green);
                    var error = Tool.ToObject<Error>(res);
                    if (!string.IsNullOrEmpty(error.login_url))
                    {
                        throw new Exception("Login failure!");
                    }
                    var resJson = Tool.ToObject<Response>(res);
                    return resJson;
                }
                else
                {
                    var res = await message.Content.ReadAsStringAsync();
                    ("Error:" + res).Print(ConsoleColor.Red); ;
                    throw new Exception("ErrorCode:" + message.StatusCode);
                }
            }
        }

    }
}

