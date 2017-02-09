using System;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using RdcMan;

namespace Plugin.ServerTreeNodeEx
{

    [Export(typeof(RdcMan.IPlugin))]
    public class ServerTreeNodeEx : RdcMan.IPlugin
    {
        public void PostLoad(RdcMan.IPluginContext context)
        {
            var nodeDClickEvent = context.Tree.GetType().GetEvent("NodeMouseDoubleClick");
            nodeDClickEvent.AddEventHandler(context.Tree, new TreeNodeMouseClickEventHandler((s, ee) =>
            {
                var server = (ee.Node as RdcMan.Server);
                if (server != null && server.IsConnected)
                {
                    server.GetType().InvokeMember("GoFullScreen", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, server, null);
                }
            }));

            var nodeClickEvent = context.Tree.GetType().GetEvent("NodeMouseClick");
            nodeClickEvent.AddEventHandler(context.Tree, new TreeNodeMouseClickEventHandler((s, ee) =>
            {
                var server = (ee.Node as RdcMan.Server);
                if (server != null)
                {
                    var hit = (s as TreeView).HitTest(ee.Location);
                    if (ee.Button == MouseButtons.Left && hit.Location == TreeViewHitTestLocations.Image)
                    {
                        if (server.IsConnected) server.Disconnect();
                        else server.Reconnect();
                    }
                }
            }));
        }

        public void OnContextMenu(ContextMenuStrip contextMenuStrip, RdcMan.RdcTreeNode node)
        {
            //if (node != null)
            //{
            //    contextMenuStrip.Items.Insert(0, new DelegateMenuItem("Disconnect", MenuNames.SessionDisconnect, new Action(node.Disconnect)));
            //}
        }
        public void OnDockServer(RdcMan.ServerBase server)
        {
        }

        public void OnUndockServer(RdcMan.IUndockedServerForm form)
        {
        }

        public void PreLoad(RdcMan.IPluginContext context, System.Xml.XmlNode xmlNode)
        {
        }

        public XmlNode SaveSettings()
        {
            //var x = new System.Xml.XmlDocument();
            //return x.CreateNode(XmlNodeType.Attribute, "path", "Remote");
            return null;
        }

        public void Shutdown()
        {
        }
    }
}
