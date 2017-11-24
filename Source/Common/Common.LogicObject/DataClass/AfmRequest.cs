using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    /// <summary>
    /// angular-FileManager request data
    /// </summary>
    public class AfmRequest
    {
        public string action;
        public string path;
        public string item;
        public string newItemPath;
        public string[] items;
        public string newPath;
        public string singleFilename;
        public string content;
        public string perms;
        public string permsCode;
        public bool recursive;
        public string destination;
        public string compressedFilename;
        public string folderName;
        public string toFilename;
    }
}
