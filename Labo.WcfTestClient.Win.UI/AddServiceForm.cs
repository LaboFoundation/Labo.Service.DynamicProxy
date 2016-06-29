using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Description;
using System.Windows.Forms;
using Labo.ServiceModel.Core.Utils.Reflection;
using Labo.ServiceModel.DynamicProxy;

namespace Labo.WcfTestClient.Win.UI
{
    using System.ServiceModel.Channels;

    public partial class AddServiceForm : Form
    {
        private string Wsdl
        {
            get
            {
                return txtServiceWsdl.Text.Trim();
            }
        }

        private IList<ServiceInfo> m_Services;
        public IList<ServiceInfo> Services
        {
            get
            {
                return m_Services ?? (m_Services = new ReadOnlyCollection<ServiceInfo>(new List<ServiceInfo>(0)));
            }
        }

        public AddServiceForm(Form owner)
        {
            InitializeComponent();

            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            Owner = owner;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceClientProxyFactoryGenerator proxyFactoryGenerator = new ServiceClientProxyFactoryGenerator(new ServiceMetadataDownloader(), new ServiceMetadataImporter(new CSharpCodeDomProviderFactory()), new ServiceClientProxyCompiler());
                ServiceClientProxyFactory proxyFactory = proxyFactoryGenerator.GenerateProxyFactory(Wsdl);
                List<ServiceInfo> serviceInfos = new List<ServiceInfo>();
                ServiceInfo serviceInfo = new ServiceInfo { Wsdl = Wsdl, Config = proxyFactory.Config };
                Collection<ServiceEndpoint> serviceEndpoints = proxyFactory.Endpoints;

                for (int i = 0; i < serviceEndpoints.Count; i++)
                {
                    ServiceEndpoint serviceEndpoint = serviceEndpoints[i];
                    ContractDescription contractDescription = serviceEndpoint.Contract;
                    string contractName = contractDescription.Name;

                    ServiceClientProxy proxy = proxyFactory.CreateProxy(serviceEndpoint);

                    string[] operationNames = contractDescription.Operations.Select(x => x.Name).ToArray();
                    ContractInfo contractInfo = new ContractInfo { Proxy = proxy, ContractName = contractName };
                    EndPointInfo endPointInfo = new EndPointInfo { BindingName = serviceEndpoint.Binding.Name, ContractInfo = contractInfo };

                    for (int j = 0; j < operationNames.Length; j++)
                    {
                        string operationName = operationNames[j];
                        object instance = proxy.CreateInstance();
                        using (instance as IDisposable)
                        {
                            Method method = ReflectionUtils.GetMethodDefinition(instance, operationName);
                            contractInfo.Operations.Add(new OperationInfo { Contract = contractInfo, Method = method });
                        }
                    }
                    serviceInfo.EndPoints.Add(endPointInfo);
                }

                serviceInfos.Add(serviceInfo);

                m_Services = serviceInfos.AsReadOnly();
            }
            catch (Exception exception)
            {
                ShowErrorForm.ShowDialog(this, exception);
            }
        }
    }
}
