using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Common.LogicObject;

namespace Att
{
    public class AttCombination
    {
        protected List<AttInfo> attList = new List<AttInfo>();

        public AttCombination(DataTable dt)
        {
            Initialize(dt);
        }

        public List<AttInfo> GetList()
        {
            return attList;
        }

        protected void Initialize(DataTable dt)
        {
            foreach (DataRowView drvTemp in dt.DefaultView)
            {
                string attSubject = drvTemp.ToSafeStr("AttSubject");
                int sortNo = Convert.ToInt32(drvTemp["SortNo"]);
                string fileSavedName = drvTemp.ToSafeStr("FileSavedName");

                //找出同名AttInfo
                AttInfo curAttInfo = attList.Find(x => x.AttSubject == attSubject);

                if (curAttInfo == null)
                {
                    curAttInfo = new AttInfo();
                    curAttInfo.AttSubject = attSubject;
                    curAttInfo.SortNo = sortNo;

                    attList.Add(curAttInfo);
                }

                string ext = Path.GetExtension(fileSavedName);
                DateTime mdfDate = drvTemp.To<DateTime>("MdfDate", DateTime.MinValue);

                if (mdfDate == DateTime.MinValue)
                {
                    mdfDate = Convert.ToDateTime(drvTemp["PostDate"]);
                }

                // add file data
                FileData curFile = new FileData()
                {
                    AttId = (Guid)drvTemp["AttId"],
                    SortNo = Convert.ToInt32(drvTemp["SortNo"]),
                    FileName = fileSavedName,
                    FileExt = ext,
                    FileSize = Convert.ToInt32(drvTemp["FileSize"]),
                    ReadCount = Convert.ToInt32(drvTemp["ReadCount"]),
                    MdfDate = mdfDate
                };

                if (curFile.FileSize > 1024)
                {
                    curFile.FileSizeDesc = string.Format("{0:#,0.##} MB", curFile.FileSize / 1024f);
                }
                else
                {
                    curFile.FileSizeDesc = string.Format("{0:#,0} KB", curFile.FileSize);
                }

                if (curAttInfo.SortNo > curFile.SortNo)
                {
                    curAttInfo.SortNo = curFile.SortNo;
                }

                curAttInfo.Files.Add(curFile);
            }

            // attList.Sort((x, y) => x.SortNo.CompareTo(y.SortNo));
        }
    }

    public class AttInfo
    {
        public string AttSubject;
        public int SortNo = -1;
        public List<FileData> Files = new List<FileData>();
    }

    public class FileData
    {
        public Guid AttId;
        public int SortNo;
        public string FileName;
        public string FileExt;
        public int FileSize;
        public string FileSizeDesc;
        public int ReadCount;
        public DateTime MdfDate;
    }

}