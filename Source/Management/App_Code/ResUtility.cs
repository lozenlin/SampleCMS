using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Resources Utility
/// </summary>
public static class ResUtility
{
	static ResUtility()
	{
	}

    public static string GetExtIconFileName(string fileName)
    {
        string result = "generic";
        string fileExt = Path.GetExtension(fileName);

        switch (fileExt.ToLower())
        {
            case ".doc":
            case ".docx":
                result = "doc";
                break;
            case ".xls":
            case ".xlsx":
                result = "xls";
                break;
            case ".ppt":
            case ".pptx":
                result = "ppt";
                break;
            case ".odt":
                result = "odt";
                break;
            case ".ods":
                result = "ods";
                break;
            case ".odp":
                result = "odp";
                break;
            case ".pdf":
                result = "pdf";
                break;
            case ".txt":
                result = "txt";
                break;
            case ".gif":
                result = "gif";
                break;
            case ".jpg":
                result = "jpg";
                break;
            case ".png":
                result = "png";
                break;
            case ".svg":
                result = "svg";
                break;
            case ".tif":
                result = "tif";
                break;
            case ".mp3":
                result = "mp3";
                break;
            case ".wav":
            case ".wma":
                result = "music";
                break;
            case ".avi":
                result = "avi";
                break;
            case ".mp4":
            case ".wmv":
            case ".mov":
                result = "video";
                break;
            case ".zip":
                result = "zip";
                break;
            case ".rar":
                result = "rar";
                break;
        }

        result += ".png";

        return result;
    }

    public static string GetExtIconText(string fileName)
    {
        string result = Resources.Lang.FileExtIconText_generic;
        string fileExt = Path.GetExtension(fileName);

        switch (fileExt.ToLower())
        {
            case ".doc":
            case ".docx":
                result = Resources.Lang.FileExtIconText_doc;
                break;
            case ".xls":
            case ".xlsx":
                result = Resources.Lang.FileExtIconText_xls;
                break;
            case ".ppt":
            case ".pptx":
                result = Resources.Lang.FileExtIconText_ppt;
                break;
            case ".odt":
                result = Resources.Lang.FileExtIconText_odt;
                break;
            case ".ods":
                result = Resources.Lang.FileExtIconText_ods;
                break;
            case ".odp":
                result = Resources.Lang.FileExtIconText_odp;
                break;
            case ".pdf":
                result = Resources.Lang.FileExtIconText_pdf;
                break;
            case ".txt":
                result = Resources.Lang.FileExtIconText_txt;
                break;
            case ".gif":
                result = Resources.Lang.FileExtIconText_gif;
                break;
            case ".jpg":
                result = Resources.Lang.FileExtIconText_jpg;
                break;
            case ".png":
                result = Resources.Lang.FileExtIconText_png;
                break;
            case ".svg":
                result = Resources.Lang.FileExtIconText_svg;
                break;
            case ".tif":
                result = Resources.Lang.FileExtIconText_tif;
                break;
            case ".mp3":
                result = Resources.Lang.FileExtIconText_mp3;
                break;
            case ".wav":
                result = Resources.Lang.FileExtIconText_wav;
                break;
            case ".wma":
                result = Resources.Lang.FileExtIconText_wma;
                break;
            case ".avi":
                result = Resources.Lang.FileExtIconText_avi;
                break;
            case ".mp4":
                result = Resources.Lang.FileExtIconText_mp4;
                break;
            case ".wmv":
                result = Resources.Lang.FileExtIconText_wmv;
                break;
            case ".mov":
                result = Resources.Lang.FileExtIconText_mov;
                break;
            case ".zip":
                result = Resources.Lang.FileExtIconText_zip;
                break;
            case ".rar":
                result = Resources.Lang.FileExtIconText_rar;
                break;
        }

        return result;
    }

    public static string GetErrMsgOfAttFileErrState(AttFileErrState errState)
    {
        string errMsg = "";

        switch (errState)
        {
            case AttFileErrState.LoadDataFailed:
                errMsg = Resources.Lang.ErrMsg_LoadDataFailed;
                break;
            case AttFileErrState.LoadMultiLangDataFailed:
                errMsg = Resources.Lang.ErrMsg_LoadMultiLangDataFailed;
                break;
            case AttFileErrState.AttachFileIsRequired:
                errMsg = Resources.Lang.ErrMsg_AttachFileIsRequired;
                break;
            case AttFileErrState.InvalidFileExt:
                errMsg = Resources.Lang.ErrMsg_InvalidFileExt;
                break;
            case AttFileErrState.NoInitialize:
                errMsg = Resources.Lang.ErrMsg_NoInitialize;
                break;
            case AttFileErrState.DeleteDataFailed:
                errMsg = Resources.Lang.ErrMsg_DeleteAttachmentFailed;
                break;
            case AttFileErrState.DeletePhysicalFileFailed:
                errMsg = Resources.Lang.ErrMsg_DeletePhysicalFileFailed;
                break;
            case AttFileErrState.SavePhysicalFileFailed:
                errMsg = Resources.Lang.ErrMsg_SavePhysicalFileFailed;
                break;
            case AttFileErrState.InsertDataFailed:
                errMsg = Resources.Lang.ErrMsg_InsertAttachmentFailed;
                break;
            case AttFileErrState.InsertMultiLangDataFailed:
                errMsg = Resources.Lang.ErrMsg_InsertMultiLangAttachmentFailed;
                break;
            case AttFileErrState.UpdateDataFailed:
                errMsg = Resources.Lang.ErrMsg_UpdateAttachmentFailed;
                break;
            case AttFileErrState.UpdateMultiLangDataFailed:
                errMsg = Resources.Lang.ErrMsg_UpdateMultiLangAttachmentFailed;
                break;
            case AttFileErrState.AttIdIsRequired:
                errMsg = Resources.Lang.ErrMsg_AttIdIsRequired;
                break;
        }

        return errMsg;
    }

}