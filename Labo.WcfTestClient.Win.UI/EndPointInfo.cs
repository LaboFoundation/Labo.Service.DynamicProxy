namespace Labo.WcfTestClient.Win.UI
{
    using System;

    [Serializable]
    public sealed class EndPointInfo
    {
        public ContractInfo ContractInfo { get; set; }

        public string BindingName { get; set; }
    }
}