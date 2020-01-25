
import requests
import json
import hashlib
import time
import sys
import datetime


ASSET_DIR = '/Users/xiexiongping/workdir/gogokidz/AvaterUnity/Assetbundles'
FILE_NAME = 'Mapping.json'

TOKEN_API = "/openapi/auth/build_cookie"
PRESIGN = "/data/upload/presign_urls"
PRESIGN_SAVE = "/data/upload/presign_url_save"
UPLOAD = "/data/avatars/%s/upload"
DATA_AVATAR = "/data/avatars/%s"
CREATE_AVATAR = "/data/create_avatar"
HTTP_HOST = "https://data-api-testfulllink.gkid.com"
COOKIE = "user=2|1:0|10:1562582603|4:user|20:SG9uZ0NoZW5QaWFvS2U=|0e35673e31fdc8bca62d79bf4815549507b055e47961bc743c42bb9791232a16"
VERSION = time.strftime("%Y%m%d-%H%M%S",time.localtime())
FORCE = False
IGNORE=""

upload_list = []

i = 1
while i < len(sys.argv):
    if sys.argv[i] == '--json':
        i += 1
        FILE_NAME = sys.argv[i]
    elif sys.argv[i] == '--path':
        i += 1
        ASSET_DIR = sys.argv[i]
    elif sys.argv[i] == '--host':
        i += 1
        HTTP_HOST = sys.argv[i]
    elif sys.argv[i] == '--force':
        i += 1
        FORCE = sys.argv[i] == 'true'
    elif sys.argv[i] == '--ignore':
        i += 1
        IGNORE = sys.argv[i]
    pass
        
    i += 1
    pass

print("--path:%s" % ASSET_DIR)
print("--json:%s" % FILE_NAME)
print("--host:%s" % HTTP_HOST)
print("--force:%s" % FORCE)
print("--ignore:%s" % IGNORE)


ignore_args = IGNORE.split(',')



def build_hashed_token(timestamp_str):
    token_key =  'c41d6e8b-K4(2#pUJ+@10@-841e-dfa589e72912' + timestamp_str + 'littlelights'
    m = hashlib.md5()
    m.update(token_key)
    return m.hexdigest() 


# get file md5
def file_as_bytes(path):
    fp = "%s/%s" % (ASSET_DIR, path)
    file = open(fp, "rb")
    with file:
        return hashlib.md5(file.read()).hexdigest()
        # .upper()
    pass


# get all md5
def get_md5(item):
    md5 = {'ios': file_as_bytes(item["ios"]), 'android': file_as_bytes(item["android"]),
           'standalonewindows': file_as_bytes(item["standalonewindows"]), 
           'webgl': file_as_bytes(item["webgl"])}
    return md5


# get target
def get_target(item):
    path = item["path"].split('/')
    #print(path)
    if path[0]=="export" or path[0]=="exportitems":
        if path[1] == "andrew": 
            return "Andrew"
        if path[1] == "kathy":
            return  "Kathy"
    elif path[0]=="exportpet":
        return "Gilly"
    else:
        raise Exception("No found target")
    pass


# get file type
def get_file_type(item):
    path = item["path"].split('/')
    if path[0] == 'export' or path[0] == "exportpet":
        if path[2] == "animation" or path[2] == "animator":
            return "action"
        elif path[2] == "scene":
            return "scene"
        elif path[2] == "skin":
            return "model"
        else:
            raise  Exception("unknow type:"+item["path"])
        pass
    elif path[0] == "exportitems":
        return "prop"
    else:
        raise Exception("unknow type:"+item["path"])
    pass


# upload to cdn
def upload_file_request(url, filepath, content_type, mothod) :
    if mothod == 'put':
        headers = {"Content-Type": content_type}
        res = requests.put(url, headers=headers, data=open("%s/%s" % (ASSET_DIR, filepath), 'rb'))
        
        #print("Response:%s %s" % (res, res.headers))
        res.raise_for_status()
        pass
    pass        

      
# request 
def begin_request(url, req):
    headers = {'Cookie': COOKIE}
    #print("request:%s" % req)
    r = requests.post(HTTP_HOST + url, data=req, headers=headers)
    #print(r.url)
    #print("response:%s" % r.text)
    r.raise_for_status()
    return json.loads(r.text)


# upload file
def upload_step(id, file_name, f_path, p_type, p_md5):
    res = begin_request(PRESIGN, json.dumps({'category': '3d_asset',
                                            'keep_name': True, 'filename': file_name})) 
    upload_file_request(res["url"], f_path, res["content_type"], res["method"])
    save_res = begin_request(PRESIGN_SAVE, json.dumps({'save_key': res["save_key"], 
                                                       'url': res['visit_url'], 'acl_private': False, 'md5':p_md5}))
    upload = begin_request(UPLOAD % id, json.dumps({'filetype': p_type, 'key': save_res["key"]}))
    return upload
    pass


def upload_file(item):
    if len(ignore_args)>0 :
        for index in range(0,len(ignore_args)):
            ig = ignore_args[index]
            if(ig and i['path'].startswith(ig)):
                print("Ignore:%s"%i['path'])
                return
            pass
    print("begin:%s" % item["name_file"])         
    req = {'filename': item["name_file"].split('.')[0],
           'path': item['path'], 'target': get_target(item),
           'type': get_file_type(item), 'version': VERSION, 'md5': get_md5(item)}
    crm = begin_request(CREATE_AVATAR, json.dumps(req))
    change = False
    if (not 'md5' in crm) or crm['md5'].get('ios') != req["md5"]['ios'] or FORCE:
        upload_step(crm['id'], item["name_file"], item["ios"], 'ab_ios_file',req["md5"]['ios'])
        change = True
    pass
        #print("ab_ios_file is same")
        
    if (not 'md5' in crm) or crm['md5'].get('android') != req["md5"]['android'] or FORCE:
        upload_step(crm['id'], item["name_file"], item["android"], 'ab_android_file',req["md5"]['android'])
        change = True
    pass
        #print("ab_android_file is same")
        
    if (not 'md5' in crm) or crm['md5'].get('webgl') != req["md5"]['webgl'] or FORCE:
        upload_step(crm['id'], item["name_file"], item["webgl"], 'ab_webgl_file',req["md5"]['webgl'])
        change = True
    pass
        #print("ab_webgl_file is same")  
    
    if (not 'md5' in crm) or crm['md5'].get('standalonewindows') != req["md5"]['standalonewindows'] or FORCE:
        upload_step(crm['id'], item["name_file"], item["standalonewindows"], 'ab_windows_file',req["md5"]['standalonewindows'])
        change = True
    pass
        #print("ab_windows_file is same")
    pass

    if change:
        upload_list.append("%s" % (i['path']))

def get_cookie(api):
    timestamp_str = datetime.datetime.utcnow().isoformat() + 'Z'
    token_key = 'c41d6e8b-K4(2#pUJ+@10@-841e-dfa589e72912%slittlelights' % timestamp_str
    m = hashlib.md5(token_key.encode('utf-8'))
    token = m.hexdigest() 
    url = '%s?token=%s&timestamp=%s'%(api,token, timestamp_str)
    print(url)
    res = requests.get(url)
    json_res = json.loads(res.text)
    return json_res['cookie']
    pass

f = open("%s/%s" % (ASSET_DIR, FILE_NAME), "r")
mapping = json.loads(f.read())
f.close()

COOKIE ='user=%s'%get_cookie(HTTP_HOST+TOKEN_API)

print("cookie="+COOKIE)

for i in mapping["files"]:
    upload_file(i)
    time.sleep(0)
    pass


for i in upload_list:
    print(i)
    pass

