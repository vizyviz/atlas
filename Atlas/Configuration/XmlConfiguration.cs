using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using System.Xml;
using System.Xml.Linq;
using Atlas.Runners;

namespace Atlas.Configuration
{
    internal class XmlConfiguration : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var xml = section.InnerXml;

            if (string.IsNullOrEmpty(xml))
            {
                throw new ConfigurationErrorsException("Missing configuration");
            }

            XElement root = XElement.Parse(xml);

            var config = new Configuration<object>();

            var nameAttribute = root.Attribute("name");
            if (nameAttribute == null)
            {
                throw new ConfigurationErrorsException("Must specify a name for the hosted service");
            }

            config.Name = nameAttribute.Value;

            var displayNameAttribute = root.Attribute("displayName");
            if(displayNameAttribute != null)
            {
                config.DisplayName = displayNameAttribute.Value;
            }
            
            var descriptionAttribute = root.Attribute("description");
            if (descriptionAttribute != null)
            {
                config.Description = descriptionAttribute.Value;
            }

            var allowMultipleInstancesAttribute = root.Attribute("allowMultipleInstances");
            if (allowMultipleInstancesAttribute != null)
            {
                config.AllowsMultipleInstances = Boolean.Parse(allowMultipleInstancesAttribute.Value);
            }

            var runModeAttribute = root.Attribute("runMode");
            if (runModeAttribute != null)
            {
                RunMode mode;
                if (!Enum.TryParse(runModeAttribute.Value, true, out mode))
                {
                    mode = RunMode.NotSet;
                }

                config.RunMode = mode;
            }

            var dependenciesElement = root.Element("dependencies");
            if (dependenciesElement != null)
            {

                var dependenciesElements = dependenciesElement.Elements();
                IList<Dependency> dependencies = new List<Dependency>();
                foreach (var element in dependenciesElements)
                {
                    var depdencyNameAttribute = element.Attribute("name");
                    if(depdencyNameAttribute != null)
                    {
                        var timeoutAttribute = element.Attribute("secondsToWait");
                        if (timeoutAttribute != null)
                        {
                            dependencies.Add(new Dependency(depdencyNameAttribute.Value,
                                                            TimeSpan.FromSeconds(Double.Parse(timeoutAttribute.Value))));
                        }
                        else
                        {
                            dependencies.Add(new Dependency(depdencyNameAttribute.Value));
                        }    
                    }
                    else
                    {
                        throw new ConfigurationErrorsException("A name must be specified for dependencies");
                    }
                }
                config.Dependencies = dependencies;
            }

            var runtimeElement = root.Element("runtime");
            if (runtimeElement != null)
            {
                var userNameAttribute = runtimeElement.Attribute("username");

                if(userNameAttribute != null)
                {
                    config.UserName = userNameAttribute.Value;
                }

                var passwordAttribute = runtimeElement.Attribute("password");
                if(passwordAttribute != null)
                {
                    config.Password = passwordAttribute.Value;
                }

                var accountTypeAttribute = runtimeElement.Attribute("accounttype");
                ServiceAccount account;
                if (accountTypeAttribute != null && Enum.TryParse(accountTypeAttribute.Value, true, out account))
                {
                    config.Account = account;
                }

                var startupAttribute = runtimeElement.Attribute("startup");
                ServiceStartMode startMode;
                if (startupAttribute != null && Enum.TryParse(startupAttribute.Value, true, out startMode))
                {
                    config.StartMode = startMode;
                }
            }

            return config;
        }
    }
}