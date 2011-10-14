using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using Atlas.Installation;
using Atlas.Runners;

namespace Atlas
{
    internal class Arguments
    {
        private readonly IDictionary<string, string> _args = new Dictionary<string, string>();
        private const string ACCOUNT_TYPE_KEY = "accounttype";
        private const string STARTMODE_KEY = "startup";
        private const string USERNAME_KEY = "username";
        private const string PASSWORD_KEY = "password";
        private const string CONSOLE_KEY = "console";
        private const string SHORT_CONSOLE_KEY = "c";
        private const string INSTALLMODE_KEY = "install";
        private const string SHORT_INSTALLMODE_KEY = "i";
        private const string UNINSTALLMODE_KEY = "uninstall";
        private const string SHORT_UNINSTALLMODE_KEY = "u";
        private const string START_KEY = "start";
        private const string SHORT_START_KEY = "s";

        private ServiceAccount _account = ServiceAccount.LocalSystem;
        private ServiceStartMode _startMode = ServiceStartMode.Manual;

        internal Arguments(IEnumerable<string> args)
        {
            foreach (var kp in string.Join(" ", args).Split(' ').Select(s => s.Split(new[] { '=' })))
            {
                _args.Add(kp[0].TrimStart(new[] { '/', '-' }).ToLower(), kp.Length > 1 ? kp[1] : string.Empty);
            }
        }

        internal RunMode RunMode
        {
            get
            {
                if (_args.ContainsKey(CONSOLE_KEY) || _args.ContainsKey(SHORT_CONSOLE_KEY))
                {
                    return RunMode.Console;
                }
                return RunMode.Service;
            }
        }

        internal InstallMode InstallMode
        {
            get
            {
                if (RunMode == RunMode.Console ||
                    (_args.ContainsKey(INSTALLMODE_KEY) || _args.ContainsKey(SHORT_INSTALLMODE_KEY)) &&
                    (_args.ContainsKey(UNINSTALLMODE_KEY) || _args.ContainsKey(SHORT_UNINSTALLMODE_KEY)))
                {
                    return InstallMode.NotSet;
                }

                if (_args.ContainsKey(INSTALLMODE_KEY) || _args.ContainsKey(SHORT_INSTALLMODE_KEY))
                {
                    return ParseInstallMode();
                }

                if (_args.ContainsKey(UNINSTALLMODE_KEY) || _args.ContainsKey(SHORT_UNINSTALLMODE_KEY))
                {
                    return InstallMode.Uninstall;
                }

                return InstallMode.NotSet;
            }
        }

        internal ServiceAccount Account
        {
            get
            {
                if (!_args.ContainsKey(ACCOUNT_TYPE_KEY))
                {
                    return ServiceAccount.LocalSystem;
                }

                if (_account == ServiceAccount.LocalSystem)
                {
                    _account = ParseAccount();
                }

                return _account;
            }
        }

        internal ServiceStartMode StartMode
        {
            get
            {
                if (!_args.ContainsKey(STARTMODE_KEY))
                {
                    return ServiceStartMode.Manual;
                }
                if (_startMode == ServiceStartMode.Manual)
                {
                    _startMode = ParseStartMode();
                }
                return _startMode;
            }
        }

        internal string UserName
        {
            get
            {
                if (_args.ContainsKey(USERNAME_KEY))
                {
                    return _args[USERNAME_KEY];
                }
                return string.Empty;
            }
        }

        internal string Password
        {
            get
            {
                if (_args.ContainsKey(PASSWORD_KEY))
                {
                    return _args[PASSWORD_KEY];
                }
                return string.Empty;
            }
        }

        private ServiceStartMode ParseStartMode()
        {
            switch (_args[STARTMODE_KEY].ToLower())
            {
                case "disabled":
                    return ServiceStartMode.Disabled;
                case "automatic":
                    return ServiceStartMode.Automatic;
                case "manual":
                default:
                    return ServiceStartMode.Manual;
            }
        }

        private ServiceAccount ParseAccount()
        {
            switch (_args[ACCOUNT_TYPE_KEY].ToLower())
            {
                case "user":
                    return ServiceAccount.User;
                case "networkservice":
                    return ServiceAccount.NetworkService;
                case "localservice":
                    return ServiceAccount.LocalService;
                case "localsystem":
                default:
                    return ServiceAccount.LocalSystem;
            }
        }

        private InstallMode ParseInstallMode()
        {
            if (_args.ContainsKey(START_KEY) || _args.ContainsKey(SHORT_START_KEY))
            {
                return InstallMode.InstallAndStart;
            }

            return InstallMode.Install;
        }
    }
}