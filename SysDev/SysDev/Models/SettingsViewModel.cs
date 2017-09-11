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

    public class MasterDetailsViewModel
    {
        public MasterData MasterData { get; set; }
        public List<MasterDetail> MasterDetails { get; set; }
    }

    public class NewMasterDetailsViewModel
    {
        public MasterData MasterData { get; set; }
        public MasterDetail MasterDetail { get; set; }
    }
}
