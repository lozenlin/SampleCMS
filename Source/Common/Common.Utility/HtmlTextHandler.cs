using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utility
{
    /// <summary>
    /// Html 文字處理
    /// </summary>
    public class HtmlTextHandler
    {
        private List<char> tempTagName = new List<char>();
        private string tagName = "";
        private string blockTagName = "";
        private bool insideBlock = false;
        private bool isAttributeArea = false;
        private string[] blockTags = new string[] { "script", "style" };    // which needs to remove whole block content.

        /// <summary>
        /// Remove HTML tags from string using char array.
        /// </summary>
        /// <remarks>
        /// reference: https://www.dotnetperls.com/remove-html-tags StripTagsCharArray(string source)
        /// </remarks>
        public string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];

                if (let == '<')
                {
                    inside = true;
                    continue;
                }

                if (let == '>')
                {
                    BriefSumUpTempTagName();
                    isAttributeArea = false;
                    inside = false;
                    continue;
                }

                if (inside)
                {
                    if (!isAttributeArea)
                    {
                        tempTagName.Add(let);
                    }

                    if (let == ' ')
                    {
                        BriefSumUpTempTagName();
                        isAttributeArea = true;
                        continue;
                    }
                }
                else if (!insideBlock)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }

            string result = new string(array, 0, arrayIndex);

            return result;
        }

        private void BriefSumUpTempTagName()
        {
            tagName = new string(tempTagName.ToArray()).Trim();
            tempTagName.Clear();

            if (insideBlock)
            {
                if (tagName.StartsWith("/"))
                {
                    if (IsBlockTag(tagName.Remove(0, 1)))
                    {
                        insideBlock = false;
                        blockTagName = "";
                    }
                }
            }
            else if (IsBlockTag(tagName))
            {
                blockTagName = tagName;
                insideBlock = true;
            }
        }

        private bool IsBlockTag(string value)
        {
            bool result = blockTags.Any<string>(x => { return 0 == string.Compare(x, value, true); });

            return result;
        }
    }
}
