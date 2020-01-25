using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AutoTool
{
    [DataContract]
    public class Error
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string is_ok { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string login_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int error_code { get; set; }
    }

    [DataContract]
    public class PMd5
    {
        [DataMember]
        public string ios;
        [DataMember]
        public string android;
        [DataMember]
        public string webgl;
        [DataMember]
        public string standalonewindows;
    }

    public class JsonPath
    {
        [DataContract]
        public class FilesItem
        {
            /// <summary>
            /// 
            /// </summary>
            [DataMember(Name = "webgl")]
            public string webgl { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ios { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string android { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string standalonewindows { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string standaloneosx { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string path { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string name_file { get; set; }
        }

        [DataContract]
        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public List<FilesItem> files { get; set; }
        }
    }

    public class Sign
    {
        [DataContract]
        public class Request
        {
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string filename { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public bool keep_name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string given_fold { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string category { get; set; }
        }

        [DataContract]
        public class Response
        {
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string content_type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string method { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string visit_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string save_key { get; set; }
        }
    }


    public class Sign_Save
    {
        [DataContract]
        public class Request
        {
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string save_key { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public bool acl_private { get; set; }
        }
        [DataContract]
        public class Response
        {
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string basename { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string deleted { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string created_at { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string bucket { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string updated_at { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string visit_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string acl { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public int filesize { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string key { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string id { get; set; }
        }
    }


    public class Upload
    {
        [DataContract]
        public class Request
        {
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string filetype { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string key { get; set; }
        }

        [DataContract]
        public class Response
        {

            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string category { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_osx_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string asset_icon { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public List<string> tags { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string deleted { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_android_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string created_at { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_windows_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string updated_at { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_ios_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_webgl_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string version { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string path { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string model_species { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public PMd5 md5 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string fbx_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string desc { get; set; }
        }
    }


    public class Create
    {
        [DataContract]
        public class Request
        {
            [DataMember]
            public string path;
            [DataMember]
            public string version;
            [DataMember]
            public string target;
            [DataMember]
            public string type;
            [DataMember]
            public PMd5 md5;
            [DataMember]
            public string filename;
        }
        [DataContract]
        public class Response 
        {
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string category { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_osx_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string asset_icon { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public List<string> tags { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string deleted { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_android_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string created_at { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_windows_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string updated_at { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_ios_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_webgl_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string version { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string path { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string model_species { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public PMd5 md5 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string fbx_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string desc { get; set; }
        }
    }


    public class Avatar
    {
        [DataContract]
        public class Request
        {
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string category { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public List<string> tags { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string model_species { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_ios_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_webgl_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_android_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_windows_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_osx_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string fbx_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string desc { get; set; }
        }
        [DataContract]
        public class Response
        {
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string category { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_osx_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string asset_icon { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public List<string> tags { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string deleted { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_android_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string created_at { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_windows_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string updated_at { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_ios_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string ab_webgl_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string version { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string path { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string model_species { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public PMd5 md5 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string fbx_file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            public string desc { get; set; }
        }
    }
}
