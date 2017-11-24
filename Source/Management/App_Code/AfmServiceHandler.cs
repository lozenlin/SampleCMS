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
            result = BuildResultOfError("list type is invalid");
            return result;
        }

        DirectoryInfo diList = new DirectoryInfo(listDir);

        if (!diList.Exists)
        {
            result = BuildResultOfError("directory does not exist");
            return result;
        }

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
}

/// <summary>
/// upload files for angular-FileManager
/// </summary>
public class AfmUploadFiles : AfmServiceHandlerAbstract
{
    public AfmUploadFiles(HttpContext context, AfmRequest afmRequest)
        : base(context, afmRequest)
    {
    }

    public override AfmResult ProcessRequest()
    {
        AfmResult result = null;
        string listDir = GetListDir();

        if (listDir == "")
        {
            result = BuildResultOfError("list type is invalid");
            return result;
        }

        DirectoryInfo diList = new DirectoryInfo(listDir);

        if (!diList.Exists)
        {
            result = BuildResultOfError("directory does not exist");
            return result;
        }

        if (Request.Files.Count == 0)
        {
            result = BuildResultOfError("no file data");
            return result;
        }

        for (int fileIndex = 0; fileIndex < Request.Files.Count; fileIndex++)
        {
            HttpPostedFile postedFile = Request.Files[fileIndex];

            string fileName = Path.GetFileName(postedFile.FileName);
            int duplicateCount = 0;

            // check same file
            while (File.Exists(listDir + fileName))
            {
                if (++duplicateCount > 99)
                {
                    result = BuildResultOfError(string.Format("too many duplication of {0}, not allowed to save more", Path.GetFileName(postedFile.FileName)));
                    return result;
                }

                //change name
                string fileNameWoExt = string.Format("{0} ({1})", Path.GetFileNameWithoutExtension(postedFile.FileName), duplicateCount);
                string ext = Path.GetExtension(postedFile.FileName);

                fileName = fileNameWoExt + ext;
            }

            try
            {
                postedFile.SaveAs(listDir + fileName);
            }
            catch (Exception ex)
            {
                c.LoggerOfUI.Error("", ex);

                result = BuildResultOfError(string.Format("save {0} failed", fileName));
            }
        }

        result = BuildResultOfSuccess();

        return result;
    }
}