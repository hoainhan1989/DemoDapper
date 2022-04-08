using System;
using System.Collections.Generic;
using System.Text;

namespace DemoDapper.Dtos
{
    public class Contract
    {
        public int Id { get; set; }
        public ContractType ContractType { get; set; }
        public string Name { get; set; }
    }

    public class MobileContract : Contract
    {
        public MobileContract() => ContractType = ContractType.Mobile;
        public string MobileNumber { get; set; }
    }
    public class TvContract : Contract
    {
        public TvContract() => ContractType = ContractType.TV;
        public string TVSeries { get; set; }
    }


    public enum ContractType
    {
        Mobile = 1, TV
    }
}
