﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    /// <summary>
    /// Action of config-form
    /// </summary>
    public static class ConfigFormAction
    {
        public const string edit = "edit";
        public const string add = "add";
    }

    /// <summary>
    /// angular-FileManager file type
    /// </summary>
    public static class AfmFileType
    {
        public const string dir = "dir";
        public const string file = "file";
    }

    /// <summary>
    /// angular-FileManager list type
    /// </summary>
    public static class AfmListType
    {
        public const string icon = "icon";
        public const string images = "images";
        public const string UserFiles = "UserFiles";
    }

    /// <summary>
    /// error state of AttachFileManager
    /// </summary>
    public enum AttFileErrState
    {
        None = 0,
        LoadDataFailed,
        LoadMultiLangDataFailed,
        AttachFileIsRequired,
        InvalidFileExt
    }
}
