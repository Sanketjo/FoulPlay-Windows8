﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foulplay_Windows8.Core.Entities
{
    public class TrophyDetailEntity
    {
        public class FromUser
        {
            public string OnlineId { get; set; }
            public bool Earned { get; set; }
            public string EarnedDate { get; set; }
        }

        public class ComparedUser
        {
            public string OnlineId { get; set; }
            public bool Earned { get; set; }
            public string EarnedDate { get; set; }
        }

        public class Trophy
        {
            public int TrophyId { get; set; }
            public bool TrophyHidden { get; set; }
            public string TrophyType { get; set; }
            public string TrophyName { get; set; }
            public string TrophyDetail { get; set; }
            public string TrophyIconUrl { get; set; }
            public int TrophyRare { get; set; }
            public string TrophyEarnedRate { get; set; }
            public FromUser FromUser { get; set; }
            public ComparedUser ComparedUser { get; set; }
        }
    }
}
