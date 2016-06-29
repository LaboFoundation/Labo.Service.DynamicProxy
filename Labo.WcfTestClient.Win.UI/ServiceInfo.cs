using System;
using System.Collections.Generic;

namespace Labo.WcfTestClient.Win.UI
{
    [Serializable]
    public sealed class ServiceInfo
    {
        public string Wsdl { get; set; }

        private IList<EndPointInfo> m_EndPoints;
        public IList<EndPointInfo> EndPoints
        {
            get
            {
                return m_EndPoints ?? (m_EndPoints = new List<EndPointInfo>());
            }
        }

        public string Config { get; set; }
    }
}