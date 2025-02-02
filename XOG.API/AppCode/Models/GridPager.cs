﻿using System.Collections.Generic;

namespace XOG.Models
{
    public class GridPager
    {
        /// <summary>
        /// The Current Page Number
        /// </summary>
        public int CurrentPage { get; set; }

        public int EndPage { get; set; }

        public string FirstLinkCssClass { get; set; } = "First";

        public string FirstLinkText { get; set; } = "&Lang;";

        public string LastLinkCssClass { get; set; } = "Last";

        public string LastLinkText { get; set; } = "&Rang;";

        public string LinkSelectedCssClass { get; set; } = "Selected";

        public string NextLinkCssClass { get; set; } = "Next";

        public string NextLinkText { get; set; } = "&rang;";

        /// <summary>
        /// Number of Pager buttons to show
        /// </summary>
        public int PagerCount { get; set; }

        public List<GridPage> Pages { get; set; }

        /// <summary>
        /// Number of Records to Display in a single page of grid view
        /// </summary>
        public int PageSize { get; set; }

        public string PreviousLinkCssClass { get; set; } = "Previous";

        public string PreviousLinkText { get; set; } = "&lang;";

        public int StartPage { get; set; }

        /// <summary>
        /// Total Number of Pages calculated from Page Size
        /// </summary>
        public int TotalPages { get; set; }
    }
}
