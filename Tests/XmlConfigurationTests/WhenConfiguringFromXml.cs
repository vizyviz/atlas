using System;
using System.Configuration;
using System.ServiceProcess;
using System.Xml;
using Atlas.Configuration;
using Atlas.Runners;
using NUnit.Framework;

namespace Tests.XmlConfigurationTests
{
    public partial class XmlConfigurationTests
    {
        [TestFixture]
        public class WhenConfiguringFromXml
        {
            private XmlConfiguration _configuration;

            private const string CONFIG_IS_EMPTY =
@"<atlas>
</atlas>";

            private const string MISSING_NAME_ATTRIBUTE =
@"<atlas>
    <host  allowMultipleInstances=""true"">
    </host>
</atlas>";
            private const string MISSING_ALLOWMULITINSTANCE_ATTRIBUTE =
@"<atlas>
    <host name=""MyService"" >
    </host>
</atlas>";

            private const string HOST_ONLY_CONFIGURATION =
@"<atlas>
    <host name=""MyService""  allowMultipleInstances=""true"">
    </host>
</atlas>";

            private const string WITH_DEPDENDENCY =
@"<atlas>
    <host name=""MyService""  allowMultipleInstances=""true"">
        <dependencies>
            <dependency name=""MSMQ"" secondsToWait=""35"" />
        </dependencies>
    </host>
</atlas>";

            private const string WITH_DEPDENDENCY_MISSING_NAME =
@"<atlas>
    <host name=""MyService""  allowMultipleInstances=""true"">
        <dependencies>
            <dependency secondsToWait=""35"" />
        </dependencies>
    </host>
</atlas>";
            private const string WITH_DEPDENDENCY_MISSING_SECONDSTOWAIT =
@"<atlas>
    <host name=""MyService""  allowMultipleInstances=""true"">
        <dependencies>
            <dependency name=""MSMQ""  />
        </dependencies>
    </host>
</atlas>";

            private const string WITH_DEPDENDENCIES =
@"<atlas>
    <host name=""MyService""  allowMultipleInstances=""true"">
        <dependencies>
            <dependency name=""MSMQ"" secondsToWait=""35"" />
            <dependency name=""AnotherDependency"" secondsToWait=""65"" />
        </dependencies>
    </host>
</atlas>";
            private const string WITHOUT_RUNMODE =
@"<atlas>
    <host name=""MyService""  allowMultipleInstances=""true"">
        <dependencies>
            <dependency name=""MSMQ"" secondsToWait=""35"" />
            <dependency name=""AnotherDependency"" secondsToWait=""65"" />
        </dependencies>
    </host>
</atlas>";

            private const string WITH_CONSOLE_RUNMODE =
@"<atlas>
    <host name=""MyService""  allowMultipleInstances=""true"" runMode=""Console"">
        <dependencies>
            <dependency name=""MSMQ"" secondsToWait=""35"" />
            <dependency name=""AnotherDependency"" secondsToWait=""65"" />
        </dependencies>
    </host>
</atlas>";
            private const string WITH_SERVICE_RUNMODE =
@"<atlas>
    <host name=""MyService""  allowMultipleInstances=""true"" runMode=""service"">
        <dependencies>
            <dependency name=""MSMQ"" secondsToWait=""35"" />
            <dependency name=""AnotherDependency"" secondsToWait=""65"" />
        </dependencies>
    </host>
</atlas>";
            private const string WITH_INVALID_RUNMODE =
@"<atlas>
    <host name=""MyService""  allowMultipleInstances=""true"" runMode=""wrongmode"">
        <dependencies>
            <dependency name=""MSMQ"" secondsToWait=""35"" />
            <dependency name=""AnotherDependency"" secondsToWait=""65"" />
        </dependencies>
    </host>
</atlas>";

            private const string WITH_RUNTIME =
@"<atlas>
    <host name=""MyService""  allowMultipleInstances=""true"">
        <dependencies>
            <dependency name=""MSMQ"" secondsToWait=""35"" />
            <dependency name=""AnotherDependency"" secondsToWait=""65"" />
        </dependencies>
        <runtime username=""myusername"" password=""mypassword"" accounttype=""networkservice"" startup=""automatic"" />
    </host>
</atlas>";
            private const string RUNTIME_WITHOUT_USERNAME =
@"<atlas>
    <host name=""MyService""  allowMultipleInstances=""true"">
        <dependencies>
            <dependency name=""MSMQ"" secondsToWait=""35"" />
            <dependency name=""AnotherDependency"" secondsToWait=""65"" />
        </dependencies>
        <runtime password=""mypassword"" accounttype=""networkservice"" startup=""automatic"" />
    </host>
</atlas>";
            private const string RUNTIME_WITHOUT_PASSWORD =
@"<atlas>
    <host name=""MyService""  allowMultipleInstances=""true"">
        <dependencies>
            <dependency name=""MSMQ"" secondsToWait=""35"" />
            <dependency name=""AnotherDependency"" secondsToWait=""65"" />
        </dependencies>
        <runtime username=""myusername"" accounttype=""networkservice"" startup=""automatic"" />
    </host>
</atlas>";
            private const string RUNTIME_WITHOUT_ACCOUNTTYPE =
@"<atlas>
    <host name=""MyService""  allowMultipleInstances=""true"">
        <dependencies>
            <dependency name=""MSMQ"" secondsToWait=""35"" />
            <dependency name=""AnotherDependency"" secondsToWait=""65"" />
        </dependencies>
        <runtime username=""myusername"" password=""mypassword""  startup=""automatic"" />
    </host>
</atlas>";
            private const string RUNTIME_WITHOUT_STARTUP =
@"<atlas>
    <host name=""MyService"" allowMultipleInstances=""true"">
        <dependencies>
            <dependency name=""MSMQ"" secondsToWait=""35"" />
            <dependency name=""AnotherDependency"" secondsToWait=""65"" />
        </dependencies>
        <runtime username=""myusername"" password=""mypassword""  accounttype=""networkservice"" />
    </host>
</atlas>";

            private const string FULL_CONFIG =
@"<atlas>
    <host name=""MyService"" displayName=""My Serivce Display Name"" description=""A Description"" allowMultipleInstances=""true"">
        <dependencies>
            <dependency name=""MSMQ"" secondsToWait=""35"" />
            <dependency name=""AnotherDependency"" secondsToWait=""65"" />
        </dependencies>
        <runtime username=""myusername"" password=""mypassword""  accounttype=""networkservice"" startup=""automatic"" />
    </host>
</atlas>";

            [SetUp]
            public void SetUp()
            {
                _configuration = new XmlConfiguration();
            }

            [Test]
            public void ConfigIsEmpty()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(CONFIG_IS_EMPTY);

                var exception = Assert.Throws<ConfigurationErrorsException>(() =>
                {
                    var section = CreateConfigSection(doc);
                });
                Assert.AreEqual("Missing configuration", exception.Message);
            }

            [Test]
            public void ConfiguringOnlyTheHost()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(HOST_ONLY_CONFIGURATION);
                var section = CreateConfigSection(doc);

                Assert.AreEqual("MyService", section.Name);
                Assert.IsTrue(section.AllowsMultipleInstances);
                Assert.AreEqual(0, section.Dependencies.Count);
            }

            [Test]
            public void ConfiguringWithSingleDependency()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(WITH_DEPDENDENCY);
                var section = CreateConfigSection(doc);

                Assert.AreEqual(1, section.Dependencies.Count);
                Assert.AreEqual("MSMQ", section.Dependencies[0].Name);
                Assert.AreEqual(TimeSpan.FromSeconds(35), section.Dependencies[0].Timeout);
            }

            [Test]
            public void ConfiguringDependencyWithMissingSecondsToWaitAttribue()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(WITH_DEPDENDENCY_MISSING_SECONDSTOWAIT);
                var section = CreateConfigSection(doc);

                Assert.AreEqual(1, section.Dependencies.Count);
                Assert.AreEqual("MSMQ", section.Dependencies[0].Name);
                Assert.AreEqual(TimeSpan.FromSeconds(15), section.Dependencies[0].Timeout);
            }

            [Test]
            public void ConfiguringDependencyWithMisingNameAttribute()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(WITH_DEPDENDENCY_MISSING_NAME);
                var exception = Assert.Throws<ConfigurationErrorsException>(() =>
                {
                    var section = CreateConfigSection(doc);
                });
                Assert.AreEqual("A name must be specified for dependencies", exception.Message);
            }

            [Test]
            public void ConfiguringWithMultipleDependencies()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(WITH_DEPDENDENCIES);
                var section = CreateConfigSection(doc);

                Assert.AreEqual(2, section.Dependencies.Count);
                Assert.AreEqual("MSMQ", section.Dependencies[0].Name);
                Assert.AreEqual(TimeSpan.FromSeconds(35), section.Dependencies[0].Timeout);

                Assert.AreEqual("AnotherDependency", section.Dependencies[1].Name);
                Assert.AreEqual(TimeSpan.FromSeconds(65), section.Dependencies[1].Timeout);
            }

            [Test]
            public void ConfiguringWithoutSettingRunMode()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(WITHOUT_RUNMODE);
                var section = CreateConfigSection(doc);
                Assert.AreEqual(RunMode.NotSet, section.RunMode);
            }

            [Test]
            public void ConfiguringWithSettingConsoleRunMode()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(WITH_CONSOLE_RUNMODE);
                var section = CreateConfigSection(doc);
                Assert.AreEqual(RunMode.Console, section.RunMode);
            }

            [Test]
            public void ConfiguringWithSettingServiceRunMode()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(WITH_SERVICE_RUNMODE);
                var section = CreateConfigSection(doc);
                Assert.AreEqual(RunMode.Service, section.RunMode);
            }

            [Test]
            public void ConfiguringWithInvalidRunMode()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(WITH_INVALID_RUNMODE);
                var section = CreateConfigSection(doc);
                Assert.AreEqual(RunMode.NotSet, section.RunMode);
            }

            [Test]
            public void MissingNameAttribute()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(MISSING_NAME_ATTRIBUTE);
                var exception = Assert.Throws<ConfigurationErrorsException>(() =>
                {
                    var section = CreateConfigSection(doc);
                });
                Assert.AreEqual("Must specify a name for the hosted service", exception.Message);
            }

            [Test]
            public void MissingAllowsMultipleTypesAttribute()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(MISSING_ALLOWMULITINSTANCE_ATTRIBUTE);
                var section = CreateConfigSection(doc);
                Assert.IsFalse(section.AllowsMultipleInstances);
            }

            [Test]
            public void RuntimeNodeProvided()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(WITH_RUNTIME);
                var section = CreateConfigSection(doc);
                Assert.AreEqual("myusername", section.UserName);
                Assert.AreEqual("mypassword", section.Password);
                Assert.AreEqual(ServiceAccount.NetworkService, section.Account);
                Assert.AreEqual(ServiceStartMode.Automatic, section.StartMode);
            }

            [Test]
            public void UsernameNotSetInRuntimeNode()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(RUNTIME_WITHOUT_USERNAME);
                var section = CreateConfigSection(doc);
                Assert.IsTrue(string.IsNullOrWhiteSpace(section.UserName));
                Assert.AreEqual("mypassword", section.Password);
                Assert.AreEqual(ServiceAccount.NetworkService, section.Account);
                Assert.AreEqual(ServiceStartMode.Automatic, section.StartMode);
            }

            [Test]
            public void PasswordNotSetInRuntimeNode()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(RUNTIME_WITHOUT_PASSWORD);
                var section = CreateConfigSection(doc);
                Assert.AreEqual("myusername", section.UserName);
                Assert.IsTrue(string.IsNullOrWhiteSpace(section.Password));
                Assert.AreEqual(ServiceAccount.NetworkService, section.Account);
                Assert.AreEqual(ServiceStartMode.Automatic, section.StartMode);
            }

            [Test]
            public void AccountTypeNotSetInRuntimeNode()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(RUNTIME_WITHOUT_ACCOUNTTYPE);
                var section = CreateConfigSection(doc);
                Assert.AreEqual("myusername", section.UserName);
                Assert.AreEqual("mypassword", section.Password);
                Assert.AreEqual(ServiceAccount.LocalSystem, section.Account);
                Assert.AreEqual(ServiceStartMode.Automatic, section.StartMode);
            }

            [Test]
            public void StartupNotSetInRuntimeNode()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(RUNTIME_WITHOUT_STARTUP);
                var section = CreateConfigSection(doc);
                Assert.AreEqual("myusername", section.UserName);
                Assert.AreEqual("mypassword", section.Password);
                Assert.AreEqual(ServiceAccount.NetworkService, section.Account);
                Assert.AreEqual(ServiceStartMode.Manual, section.StartMode);
            }

            [Test]
            public void FullConfigOptionsProvided()
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(FULL_CONFIG);
                var section = CreateConfigSection(doc);

                Assert.AreEqual("MyService", section.Name);
                Assert.AreEqual("My Serivce Display Name", section.DisplayName);
                Assert.AreEqual("A Description", section.Description);

                Assert.IsTrue(section.AllowsMultipleInstances);

                Assert.AreEqual(2, section.Dependencies.Count);
                Assert.AreEqual("MSMQ", section.Dependencies[0].Name);
                Assert.AreEqual(TimeSpan.FromSeconds(35), section.Dependencies[0].Timeout);

                Assert.AreEqual("AnotherDependency", section.Dependencies[1].Name);
                Assert.AreEqual(TimeSpan.FromSeconds(65), section.Dependencies[1].Timeout);
                Assert.AreEqual("myusername", section.UserName);
                Assert.AreEqual("mypassword", section.Password);
                Assert.AreEqual(ServiceAccount.NetworkService, section.Account);
                Assert.AreEqual(ServiceStartMode.Automatic, section.StartMode);


            }

            private Configuration<object> CreateConfigSection(XmlDocument doc)
            {
                return (Configuration<object>)_configuration.Create(null, null, doc.ChildNodes[0]);
            }
        }
    }
}