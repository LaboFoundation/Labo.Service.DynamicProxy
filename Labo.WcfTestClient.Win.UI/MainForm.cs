namespace Labo.WcfTestClient.Win.UI
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Forms;

    using Labo.ServiceModel.Core;

    public partial class MainForm : Form
    {
        private readonly IReflectionHelper m_ReflectionHelper;

        private List<ServiceInfo> m_ServiceInfos;
        private List<ServiceInfo> ServiceInfos
        {
            get
            {
                return m_ServiceInfos ?? (m_ServiceInfos = new List<ServiceInfo>());
            }
        }

        public MainForm()
        {
            InitializeComponent();

            m_ReflectionHelper = new DefaultReflectionHelper();
        }

        private void AddServiceToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddServiceForm addServiceForm = new AddServiceForm(this);
            DialogResult dialogResult = addServiceForm.ShowDialog(this);
            if(dialogResult == DialogResult.OK)
            {
                ServiceInfos.AddRange(addServiceForm.Services);

                FillTreeView();
            }
        }

        private void FillTreeView()
        {
            tvwServices.BeginUpdate();

            tvwServices.Nodes.Clear();

            for (int i = 0; i < ServiceInfos.Count; i++)
            {
                ServiceInfo serviceInfo = ServiceInfos[i];
                TreeNode serviceNode = new TreeNode(serviceInfo.Wsdl);

                IList<EndPointInfo> endpointInfos = serviceInfo.EndPoints;
                for (int j = 0; j < endpointInfos.Count; j++)
                {
                    EndPointInfo endPointInfo = endpointInfos[j];
                    ContractInfo contractInfo = endPointInfo.ContractInfo;
                    TreeNode contractNode = new TreeNode(string.Format(CultureInfo.CurrentCulture, "{0} ({1})", contractInfo.ContractName, endPointInfo.BindingName));

                    List<OperationInfo> operationInfos = contractInfo.Operations;
                    for (int k = 0; k < operationInfos.Count; k++)
                    {
                        OperationInfo operationInfo = operationInfos[k];
                        TreeNode operationNode = new TreeNode(operationInfo.Method.Name)
                                                     {
                                                         Tag = operationInfo
                                                     };
                        contractNode.Nodes.Add(operationNode);
                    }

                    serviceNode.Nodes.Add(contractNode);
                }

                TreeNode configNode = new TreeNode("Config")
                                          {
                                              Name = "ConfigNode",
                                              Tag = serviceInfo.Config,
                                              ToolTipText = serviceInfo.Wsdl + " Config"
                                          };
                serviceNode.Nodes.Add(configNode);

                tvwServices.Nodes.Add(serviceNode);
            }

            tvwServices.ExpandAll();
            tvwServices.EndUpdate();
        }

        private void tvwServices_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeNode selectedNode = tvwServices.SelectedNode;
            if (selectedNode.Name == "ConfigNode")
            {
                TabPage tabPage = new TabPage(selectedNode.ToolTipText);
                ServiceConfigUserControl serviceConfigUserControl = new ServiceConfigUserControl(selectedNode.Tag.ToString())
                {
                    Dock = DockStyle.Fill
                };
                tabPage.Controls.Add(serviceConfigUserControl);
                tbOperations.TabPages.Add(tabPage);
            }
            else
            {
                OperationInfo operationInfo = selectedNode.Tag as OperationInfo;
                if (operationInfo != null)
                {
                    TabPage tabPage = new TabPage(operationInfo.Method.Name);
                    OperationInvokerUserControl operationInvokerUserControl = new OperationInvokerUserControl(operationInfo, m_ReflectionHelper)
                    {
                        Dock = DockStyle.Fill
                    };
                    tabPage.Controls.Add(operationInvokerUserControl);
                    tbOperations.TabPages.Add(tabPage);
                }
            }
        }
    }
}
