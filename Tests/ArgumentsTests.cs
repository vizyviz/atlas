using System.Collections.Generic;
using System.ServiceProcess;
using Atlas;
using Atlas.Installation;
using Atlas.Runners;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ArgumentsTests
    {
        [Test]
        public void ZeroArgsReturnsServiceMode()
        {
            var actual = new Arguments(new string[] { });
            Assert.AreEqual(RunMode.Service, actual.RunMode);
        }

        [TestCase("/console")]
        [TestCase("-console")]
        [TestCase("-CONSOLE")]
        [TestCase("-cOnsole")]
        [TestCase("-c")]
        [TestCase("/c")]
        [TestCase("/C")]
        public void OneArgEqualToConsoleReturnsConsole(string arg)
        {
            var actual = new Arguments(new[] { arg });
            Assert.AreEqual(RunMode.Console, actual.RunMode);
        }

        [Test]
        public void OneArgNotEqualToConsoleReturnsService()
        {
            var actual = new Arguments(new[] { "/something" });
            Assert.AreEqual(RunMode.Service, actual.RunMode);
        }

        [TestCase("/install")]
        [TestCase("-install")]
        [TestCase("-INStall")]
        [TestCase("-i")]
        [TestCase("-I")]
        [TestCase("/i")]
        public void OneArgEqualToInstallReturnsInstallAndIsRunningAsService(string arg)
        {
            var actual = new Arguments(new[] { arg });
            Assert.AreEqual(InstallMode.Install, actual.InstallMode);
            Assert.AreEqual(RunMode.Service, actual.RunMode);
        }

        [TestCase("/install", "-start")]
        [TestCase("/install", "-START")]
        [TestCase("-install", "-start")]
        [TestCase("/i", "-start")]
        [TestCase("-i", "-start")]
        [TestCase("/install", "/start")]
        [TestCase("-install", "/start")]
        [TestCase("/i", "/start")]
        [TestCase("/I", "/stArt")]
        [TestCase("-i", "/start")]
        public void OneArgEqualToInstallStartReturnsInstallAndStartAndIsRunningAsService(string arg1, string arg2)
        {
            var actual = new Arguments(new[] { arg1, arg2 });
            Assert.AreEqual(InstallMode.InstallAndStart, actual.InstallMode);
            Assert.AreEqual(RunMode.Service, actual.RunMode);

            actual = new Arguments(new[] { arg2, arg1 });
            Assert.AreEqual(InstallMode.InstallAndStart, actual.InstallMode);
            Assert.AreEqual(RunMode.Service, actual.RunMode);
        }

        [TestCase("/uninstall")]
        [TestCase("/unInStaLL")]
        [TestCase("-uninstall")]
        [TestCase("-u")]
        [TestCase("/u")]
        [TestCase("/U")]
        public void OneArgEqualToUnInstallReturnsUnInstall(string arg)
        {
            var actual = new Arguments(new[] { arg });
            Assert.AreEqual(InstallMode.Uninstall, actual.InstallMode);
        }

        [Test]
        public void BothInstallAndUnintallWillBeNotSet()
        {
            var actual = new Arguments(new[] { "/uninstall /install" });
            Assert.AreEqual(InstallMode.NotSet, actual.InstallMode);
        }

        [Test]
        public void NotSettingAccountReturnsLocalSystem()
        {
            var actual = new Arguments(new string[] { });
            Assert.AreEqual(ServiceAccount.LocalSystem, actual.Account);
        }

        [TestCase("networkservice", ServiceAccount.NetworkService)]
        [TestCase("LocAlsyStem", ServiceAccount.LocalSystem)]
        [TestCase("localsystem", ServiceAccount.LocalSystem)]
        [TestCase("localservice", ServiceAccount.LocalService)]
        [TestCase("user", ServiceAccount.User)]
        public void SettingAccountParsesCorreclty(string accountype, ServiceAccount expected)
        {
            var actual = new Arguments(new[] { "/accounttype=" + accountype });
            Assert.AreEqual(expected, actual.Account);
        }

        [Test]
        public void NotSettingStartModeReturnsManual()
        {
            var actual = new Arguments(new string[] { });
            Assert.AreEqual(ServiceStartMode.Manual, actual.StartMode);
        }

        [TestCase("manual", ServiceStartMode.Manual)]
        [TestCase("disABled", ServiceStartMode.Disabled)]
        [TestCase("disabled", ServiceStartMode.Disabled)]
        [TestCase("automatic", ServiceStartMode.Automatic)]
        public void SettingStartModeParsesCorreclty(string startup, ServiceStartMode expected)
        {
            var actual = new Arguments(new[] { "/startup=" + startup });
            Assert.AreEqual(expected, actual.StartMode);
        }

        [Test]
        public void NotSettingUserNameIsEmptyString()
        {
            var actual = new Arguments(new[] { "" });
            Assert.IsEmpty(actual.UserName);
        }

        [Test]
        public void SettingUserNameIsEqualToUserNameProvided()
        {
            var actual = new Arguments(new[] { "/username=myuser" });
            Assert.AreEqual("myuser", actual.UserName);
        }

        [Test]
        public void SettingUserNameIgnoresCaseOfArgumentKeyIsEqualToUserNameProvided()
        {
            var actual = new Arguments(new[] { "/useRnaMe=myuser" });
            Assert.AreEqual("myuser", actual.UserName);
        }

        [Test]
        public void NotSettinPasswordIsEmptyString()
        {
            var actual = new Arguments(new[] { "" });
            Assert.IsEmpty(actual.Password);
        }

        [Test]
        public void SettingPasswordIsEqualToPasswordProvided()
        {
            var actual = new Arguments(new[] { "/password=mypassword" });
            Assert.AreEqual("mypassword", actual.Password);
        }

        [Test]
        public void SettingPasswordIgnoresCaseOfArgumentKeyIsEqualToPasswordProvided()
        {
            var actual = new Arguments(new[] { "/passWorD=mypassword" });
            Assert.AreEqual("mypassword", actual.Password);
        }


        [Test]
        public void ConsolesInstallModeIsIgnored()
        {
            var actual = new Arguments(new[] {"/console /install"});
            Assert.AreEqual(InstallMode.NotSet, actual.InstallMode);
        }

        [Test]
        public void ArgumentsTogether()
        {
            var actual =
                new Arguments(new[]
                                  {
                                      "/install -start /username=myuser /password=mypassword -startup=automatic /accounttype=user"
                                  });

            Assert.AreEqual(RunMode.Service, actual.RunMode);
            Assert.AreEqual(InstallMode.InstallAndStart, actual.InstallMode);
            Assert.AreEqual("myuser", actual.UserName);
            Assert.AreEqual("mypassword", actual.Password);
            Assert.AreEqual(ServiceStartMode.Automatic, actual.StartMode);
            Assert.AreEqual(ServiceAccount.User, actual.Account);
        }

        [Test]
        public void Split()
        {
            string tosplit =
                "/install -start /username=myuser /password=mypassword -startup=automatic /accounttype=user";

            var result = tosplit.Split(new[] {' '});

            Assert.AreEqual(6, result.Length);

            IList<KeyValuePair<string, string>> subs = new List<KeyValuePair<string, string>>();

            foreach (var r in result)
            {
                var subss = r.Split(new[] {'='});
                subs.Add(new KeyValuePair<string, string>(subss[0].TrimStart(new []{'/', '-'}), subss.Length > 1? subss[1] : string.Empty));
            }

            Assert.AreEqual(6, subs.Count);
            Assert.AreEqual("install", subs[0].Key);
            Assert.AreEqual(string.Empty, subs[0].Value);

            Assert.AreEqual("start", subs[1].Key);
            Assert.AreEqual(string.Empty, subs[1].Value);

            Assert.AreEqual("username", subs[2].Key);
            Assert.AreEqual("myuser", subs[2].Value);

            Assert.AreEqual("password", subs[3].Key);
            Assert.AreEqual("mypassword", subs[3].Value);

            Assert.AreEqual("startup", subs[4].Key);
            Assert.AreEqual("automatic", subs[4].Value);

            Assert.AreEqual("accounttype", subs[5].Key);
            Assert.AreEqual("user", subs[5].Value);
        }
    }
}