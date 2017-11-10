// ===============================================================================
// DataPagerLogic of SampleCMS
// https://github.com/lozenlin/SampleCMS
//
// DataPagerLogic.cs
//
// ===============================================================================
// Copyright (c) 2017 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    /// <summary>
    /// 分頁控制
    /// </summary>
    public class DataPagerLogic
    {
        public DataPagerLogic()
        {
        }

        #region Public properties

        /// <summary>
        /// 每頁最大筆數
        /// </summary>
        public int MaxItemCountOfPage
        {
            get { return maxItemCountOfPage; }
            set { maxItemCountOfPage = value; }
        }
        private int maxItemCountOfPage = 20;

        /// <summary>
        /// 分頁區最多顯示頁碼數
        /// </summary>
        public int MaxDisplayCountInPageCodeArea
        {
            get { return maxDisplayCountInPageCodeArea; }
            set { maxDisplayCountInPageCodeArea = value; }
        }
        private int maxDisplayCountInPageCodeArea = 5;

        /// <summary>
        /// 目前頁碼 (to set value, call SetCurrentPageCodeAndRecalc())
        /// </summary>
        public int CurrentPageCode
        {
            get { return currentPageCode; }
        }
        private int currentPageCode = 1;

        /// <summary>
        /// 總筆數
        /// </summary>
        public int ItemTotalCount
        {
            get { return itemTotalCount; }
            set { itemTotalCount = value; }
        }
        private int itemTotalCount = 0;

        /// <summary>
        /// 總頁數
        /// </summary>
        public int PageTotalCount
        {
            get { return pageTotalCount; }
        }
        private int pageTotalCount = 0;

        /// <summary>
        /// 顯示的第一個頁碼
        /// </summary>
        public int FirstDisplayPageCode
        {
            get { return firstDisplayPageCode; }
        }
        private int firstDisplayPageCode = 1;

        /// <summary>
        /// 顯示的最後一個頁碼
        /// </summary>
        public int LastDisplayPageCode
        {
            get { return lastDisplayPageCode; }
        }
        private int lastDisplayPageCode = 1;

        /// <summary>
        /// 頁面中的起始項目編號
        /// </summary>
        public int BeginItemNumberOfPage
        {
            get { return beginItemNumberOfPage; }
        }
        private int beginItemNumberOfPage = 1;

        /// <summary>
        /// 頁面中的結束項目編號
        /// </summary>
        public int EndItemNumberOfPage
        {
            get { return endItemNumberOfPage; }
        }
        private int endItemNumberOfPage;

        /// <summary>
        /// 按鈕「上一頁」可顯示
        /// </summary>
        public bool CanShowPreviousButton
        {
            get { return canShowPreviousButton; }
        }
        private bool canShowPreviousButton = true;

        /// <summary>
        /// 按鈕「下一頁」可顯示
        /// </summary>
        public bool CanShowNextButton
        {
            get { return canShowNextButton; }
        }
        private bool canShowNextButton = true;

        /// <summary>
        /// 按鈕「第一頁」可顯示
        /// </summary>
        public bool CanShowFirstButton
        {
            get { return canShowFirstButton; }
        }
        private bool canShowFirstButton = true;

        /// <summary>
        /// 按鈕「最後一頁」可顯示
        /// </summary>
        public bool CanShowLastButton
        {
            get { return canShowLastButton; }
        }
        private bool canShowLastButton = true;

        #endregion

        public void SetCurrentPageCodeAndRecalc(int pageCode)
        {
            currentPageCode = pageCode;
            //計算頁碼範圍
            CalculatePageCodeRange();
        }

        /// <summary>
        /// 計算頁碼範圍
        /// </summary>
        public void CalculatePageCodeRange()
        {
            pageTotalCount = itemTotalCount / maxItemCountOfPage;
            //有餘數,多一頁
            if (itemTotalCount % maxItemCountOfPage > 0)
                pageTotalCount++;

            //算出分頁區的頭尾頁碼
            if (pageTotalCount > maxDisplayCountInPageCodeArea)
            {
                if (currentPageCode < maxDisplayCountInPageCodeArea)
                {
                    //移至最前段
                    firstDisplayPageCode = 1;
                    lastDisplayPageCode = maxDisplayCountInPageCodeArea;
                }
                else if (currentPageCode > pageTotalCount - maxDisplayCountInPageCodeArea + 1)
                {
                    //目前頁碼大於最後段的開頭頁碼時
                    //移至最後段
                    firstDisplayPageCode = pageTotalCount - maxDisplayCountInPageCodeArea + 1;
                    lastDisplayPageCode = pageTotalCount;
                }
                else if (maxDisplayCountInPageCodeArea % 2 == 0)
                {
                    //分頁區最多顯示頁碼數為偶數時,currentPageCode 排中間偏左
                    firstDisplayPageCode = currentPageCode - (maxDisplayCountInPageCodeArea / 2 - 1);
                    lastDisplayPageCode = currentPageCode + maxDisplayCountInPageCodeArea / 2;
                }
                else
                {
                    //分頁區最多顯示頁碼數為偶數時,currentPageCode 排中間
                    firstDisplayPageCode = currentPageCode - maxDisplayCountInPageCodeArea / 2;
                    lastDisplayPageCode = currentPageCode + maxDisplayCountInPageCodeArea / 2;
                }

                if (firstDisplayPageCode < 1)
                    firstDisplayPageCode = 1;

                if (lastDisplayPageCode > pageTotalCount)
                    lastDisplayPageCode = pageTotalCount;
            }
            else
            {
                //總頁數不足時
                firstDisplayPageCode = 1;
                lastDisplayPageCode = pageTotalCount;
            }

            //更新按鈕狀態
            //第一頁
            canShowFirstButton = (currentPageCode > 1 && itemTotalCount > 0);
            //上一頁
            canShowPreviousButton = canShowFirstButton;

            //最後一頁
            canShowLastButton = (currentPageCode < pageTotalCount && itemTotalCount > 0);
            //下一頁
            canShowNextButton = canShowLastButton;

            //計算頁面中的起迄編號
            beginItemNumberOfPage = (currentPageCode - 1) * maxItemCountOfPage + 1;
            endItemNumberOfPage = beginItemNumberOfPage + maxItemCountOfPage - 1;
        }
    }
}
