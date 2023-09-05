using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Domain.Infrastructure
{
    public static class ServersAD
    {
        private static AdConfigManager _Config = ConfigurationManager.GetSection("ServersAD") as AdConfigManager;
        public static ServersADElementCollection GetServersAD()
        {
            return _Config.ServersAD;
        }
    }
    //Extend the ConfigurationSection class "Section".
    public class AdConfigManager : ConfigurationSection
    {
        [ConfigurationProperty("servers")]
        public ServersADElementCollection ServersAD
        {
            get { return (ServersADElementCollection)this["servers"]; }
        }
    }

    //single element in the collection.
    [ConfigurationCollection(typeof(ServerElement))]
    public class ServersADElementCollection : ConfigurationElementCollection
    {
        public ServerElement this[int index]
        {
            get { return (ServerElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServerElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServerElement)element).Key;
        }
    }

    //single element in the collection. Create a property for each xml attribute in your element.
    public class ServerElement : ConfigurationElement
    {
        public string _User { get; set; }
        public string _Pass { get; set; }

        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("hostname", IsRequired = true)]
        public string Hostname
        {
            get { return (string)this["hostname"]; }
            set { this["hostname"] = value; }
        }

        [ConfigurationProperty("username", IsRequired = true)]
        public string Username
        {
            get { return (string)this["username"]; }
            set { this["username"] = value; }
        }
        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }

        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }

        [ConfigurationProperty("filtergroup", IsRequired = false)]
        public string Filtergroup
        {
            get { return (string)this["filtergroup"]; }
            set { this["filtergroup"] = value; }
        }
    }

}
