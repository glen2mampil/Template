using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SysDev.Models
{
    public class SettingsViewModel
    {
        public List<MasterData> MasterDatas { get; set; }
        public List<MasterDetail> MasterDetails { get; set; }
    }
    public class DataDetailsViewModel
    {
        public List<MasterData> MasterDatas { get; set; }
        public List<MasterDetail> MasterDetails { get; set; }
    }
}
