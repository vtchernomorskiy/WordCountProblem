using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using WordCountLibrary;
using WordCountLibrary.Interfaces;
using WordCountUtilities;

namespace WordCountWcfServiceRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new Logger(typeof(WordCounter));
            var svcHost = new ServiceHost(typeof(WordCounter), new Uri("http://localhost:50000/WordCount"));
            var smb = svcHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
            // If not, add one
            if (smb == null)
                smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            svcHost.Description.Behaviors.Add(smb);
            // Add MEX endpoint
            svcHost.AddServiceEndpoint(
              ServiceMetadataBehavior.MexContractName,
              MetadataExchangeBindings.CreateMexHttpBinding(),
              "mex"
            );

            var ep = svcHost.AddServiceEndpoint(typeof(IWordCount), new BasicHttpBinding(), "");

            svcHost.Open();
            Console.WriteLine("Service is running on: ");
            foreach (var endPoint in  svcHost.Description.Endpoints)
            {
                Console.WriteLine(endPoint.Address);
            }
            Console.WriteLine("Press enter to quit...");
            Console.ReadLine();
            svcHost.Close();
        }
    }
}
