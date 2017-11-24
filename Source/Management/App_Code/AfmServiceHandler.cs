using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// get list for angular-FileManager
/// </summary>
public class AfmGetList : AfmServiceHandlerAbstract
{
    public AfmGetList(HttpContext context, AfmRequest afmRequest)
        : base(context, afmRequest)
    {
    }

    public override AfmResult ProcessRequest()
    {
        AfmResult result = null;
        string listDir = GetListDir();

        if (listDir == "")
        {
            result = BuildResultOfError("listType is invalid");
            return result;
        }

        DirectoryInfo diList = new DirectoryInfo(listDir);
        FileSystemInfo[] fsInfos = diList.GetFileSystemInfos();
        List<AfmFileInfo> afmFiles = new List<AfmFileInfo>(fsInfos.Length);

        foreach (FileSystemInfo fsi in fsInfos)
        {
            AfmFileInfo afmFile = null;

            if (fsi is FileInfo)
            {
                FileInfo fi = (FileInfo)fsi;

                afmFile = new AfmFileInfo()
                {
                    name = fi.Name,
                    rights = null,
                    size = fi.Length.ToString(),
                    date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", fi.LastWriteTime),
                    type = AfmFileType.file
                };
            }
            else if (fsi is DirectoryInfo)
            {
                DirectoryInfo di = (DirectoryInfo)fsi;

                afmFile = new AfmFileInfo()
                {
                    name = di.Name,
                    rights = null,
                    size = null,
                    date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", di.LastWriteTime),
                    type = AfmFileType.dir
                };
            }

            afmFiles.Add(afmFile);
        }

        result = new AfmResult() { result = afmFiles };

        return result;
    }

    private string GetListDir()
    {
        string listDir = "";

        string appDir = Server.MapPath("~/");

        if (string.Compare(c.qsListType, AfmListType.icon) == 0)
        {
            listDir = appDir + @"BPimages\icon\";
        }
        else if (string.Compare(c.qsListType, AfmListType.images) == 0)
        {
            listDir = appDir + @"images\";
        }
        else if (string.Compare(c.qsListType, AfmListType.UserFiles) == 0)
        {
            listDir = appDir + @"UserFiles\";
        }

        if (afmRequest.path.Length > 1)
        {
            string subPath = afmRequest.path.Substring(1).Replace('/', '\\');
            listDir += subPath + @"\";
        }

        return listDir;
    }
}