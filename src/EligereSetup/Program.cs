using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.CommandLineUtils;
using System.Security.Cryptography;

namespace EligereSetup
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmd = new CommandLineApplication(false);

            cmd.Command("ticket",
                ticketCmd =>
                {
                    var arg = ticketCmd.Argument("", "ticket related commands", false);
                    ticketCmd.HelpOption("-? | -h | --help");
                    var kgen = ticketCmd.Command("keygen",
                        keygenCmd => {
                            var arg = keygenCmd.Argument("keydirectory", "Directory where to generate the new crypto key", false);
                            var exp = keygenCmd.Option("-e | --KeyExpiration", "Expiration date for the generated key", CommandOptionType.SingleValue);
                            keygenCmd.OnExecute(() =>
                            {
                                if (System.IO.Directory.Exists(arg.Value))
                                {
                                    Console.WriteLine($"Directory {arg.Value} exists, please specify a new directory for the generated key.");
                                    return 1;
                                }

                                var serviceCollection = new ServiceCollection();

                                // Creates the secrets needed to Election System and Voting System to interact
                                var serv = serviceCollection.AddDataProtection()
                                    .SetApplicationName("Eligere");
                                if (exp.HasValue())
                                {
                                    var keyspan = DateTime.Parse(exp.Value()) - DateTime.Today;
                                    serv = serv.SetDefaultKeyLifetime(keyspan);
                                }
                                serv.PersistKeysToFileSystem(new System.IO.DirectoryInfo(arg.Value));
                                var services = serviceCollection.BuildServiceProvider();

                                // create an instance of MyClass using the service provider
                                var instance = ActivatorUtilities.CreateInstance<Setup>(services);
                                instance.RunConfig();
                                return 0;
                            });
                        });

                }
                );
            cmd.HelpOption("-x");
            cmd.Execute(args);
        }
    }

    public class Setup
    {
        IDataProtector _protector;

        // the 'provider' parameter is provided by DI
        public Setup(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("Eligere.Ticket.v1");
        }

        public void RunConfig()
        {
            var test = Guid.NewGuid().ToString();
            Console.WriteLine("Generating ticket keys...");
            var tk = _protector.Protect(test);
            Console.Write("Testing generated ticket keys...");
            if (_protector.Unprotect(tk) == test) Console.WriteLine(" Success!");
            else Console.WriteLine(" Fail :(");
        }

    }
}
