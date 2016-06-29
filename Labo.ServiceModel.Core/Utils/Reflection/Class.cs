using System.Collections.Generic;

namespace Labo.ServiceModel.Core.Utils.Reflection
{
    using System;

    [Serializable]
    public sealed class Class : Instance
    {
        private IList<Member> m_Properties;
        public IList<Member> Properties
        {
            get
            {
                return m_Properties ?? (m_Properties = new List<Member>());
            }
        }
    }
}