using Labo.ServiceModel.Core.Utils.Reflection;

namespace Labo.WcfTestClient.Win.UI
{
    using System;

    [Serializable]
    public sealed class OperationInfo
    {
        public ContractInfo Contract { get; set; }

        public Method Method { get; set; }
    }
}
