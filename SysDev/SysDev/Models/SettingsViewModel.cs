using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SysDev.Models
{
    public class SettingsViewModel : ViewModelBase
    {
        public List<MasterData> MasterDatas { get; set; }
        public List<MasterDetail> MasterDetails { get; set; }

        public MasterData MasterData { get; set; }
        public MasterDetail MasterDetail { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public int MasterDataId { get; set; }
        public int MasterDetailId { get; set; }
    }
    public class DataDetailsViewModel
    {
        public List<MasterData> MasterDatas { get; set; }
        public List<MasterDetail> MasterDetails { get; set; }
    }

    public class MasterDetailsViewModel : ViewModelBase
    {
        public MasterData MasterData { get; set; }
        public List<MasterDetail> MasterDetails { get; set; }
    }

    public class NewMasterDetailsViewModel : ViewModelBase
    {
        public MasterData MasterData { get; set; }
        public MasterDetail MasterDetail { get; set; }
    }
}
