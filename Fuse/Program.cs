using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Fuze;

namespace ConsoleApplication
{
	/// <summary>
	/// 
	/// </summary>	
	public class Program
	{
		public static void Main(string[] args)
		{
			#region Welcome
			Console.WriteLine();
			Console.WriteLine(@"__/\\\\\\\\\\\\\\\__/\\\________/\\\__/\\\\\\\\\\\\\\\__/\\\\\\\\\\\\\\\_        ");
			Console.WriteLine(@" _\/\\\///////////__\/\\\_______\/\\\_\////////////\\\__\/\\\///////////__       ");
			Console.WriteLine(@"  _\/\\\_____________\/\\\_______\/\\\___________/\\\/___\/\\\_____________      ");
			Console.WriteLine(@"   _\/\\\\\\\\\\\_____\/\\\_______\/\\\_________/\\\/_____\/\\\\\\\\\\\_____     ");
			Console.WriteLine(@"    _\/\\\///////______\/\\\_______\/\\\_______/\\\/_______\/\\\///////______    ");
			Console.WriteLine(@"     _\/\\\_____________\/\\\_______\/\\\_____/\\\/_________\/\\\_____________   ");
			Console.WriteLine(@"      _\/\\\_____________\//\\\______/\\\____/\\\/___________\/\\\_____________  ");
			Console.WriteLine(@"       _\/\\\______________\///\\\\\\\\\/____/\\\\\\\\\\\\\\\_\/\\\\\\\\\\\\\\\_ ");
			Console.WriteLine(@"        _\///_________________\/////////_____\///////////////__\///////////////__");
			Console.WriteLine();
			Console.WriteLine(@" Joining your app and kubernetes with reduced effort and centralized scaffolding ");
			Console.WriteLine(@"                                                        created with <3 by Clark ");
			Console.WriteLine();
			#endregion

			string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);
			SharedMethods access = new SharedMethods();
			var deserializerBuilder = new DeserializerBuilder().WithNamingConvention(new CamelCaseNamingConvention());
			var deserializer = deserializerBuilder.Build();
			var result = deserializer.Deserialize<FuzeDataModel.K8SettingObject>(File.OpenText(string.Format("{0}/{1}", path, "k8settings.yml")));

				foreach (var thisconfig in result.configs) {
					Console.WriteLine(string.Format("Working on config element {0}", thisconfig.name));

					#region Namespace
					string nameSpace = string.Format("{0}-{1}-ns.yml", thisconfig.appName, thisconfig.name);
					Console.WriteLine(string.Format("Generating nameSpace - {0}", nameSpace));
					NameSpaceGenerator namespaceconfig = new NameSpaceGenerator();
					access.WriteOut(namespaceconfig.CreateNamespace(thisconfig.appName,thisconfig.name), string.Format("{0}/{1}", path, nameSpace));
					#endregion

					#region ConfigMap
					string config = string.Format("{0}-{1}-cm.yml", thisconfig.appName, thisconfig.name);
					Console.WriteLine(string.Format("Generating configmap - {0}", config));
					ConfigMapGenerator configmap = new ConfigMapGenerator();
					access.WriteOut(configmap.CreateConfigMap(thisconfig.appName, thisconfig.name, thisconfig.env), string.Format("{0}/{1}", path, config));
					#endregion

					#region Secret
					string secret = string.Format("{0}-{1}-secret.yml", thisconfig.appName, thisconfig.name);
					Console.WriteLine(string.Format("Generating secretmap - {0}", secret));
					SecretMapGenerator secretmap = new SecretMapGenerator();
					access.WriteOut(secretmap.CreateSecretMap(thisconfig.appName, thisconfig.name, thisconfig.secret), string.Format("{0}/{1}", path, secret));
					#endregion					

					#region Deployment
					string deploy = string.Format("{0}-{1}-deploy.yml", thisconfig.appName, thisconfig.name);
					Console.WriteLine(string.Format("Generating deployment - {0}", deploy));
					DeploymentGenerator deployment = new DeploymentGenerator();
					access.WriteOut(deployment.CreateDeployment(thisconfig.appName, thisconfig.image, thisconfig.env, thisconfig.name), string.Format("{0}/{1}", path, deploy));
					#endregion

					#region Service
					string svc = string.Format("{0}-{1}-svc.yml", thisconfig.appName, thisconfig.name);
					Console.WriteLine(string.Format("Generating service - {0}", svc));
					ServiceGenerator service = new ServiceGenerator();
					access.WriteOut(service.CreateService(thisconfig.appName), string.Format("{0}/{1}", path, svc));
					#endregion

					#region Ingress
					foreach (var domain in thisconfig.domain) {
						string ing = string.Format("{0}-{1}-{2}-ing.yml", domain, thisconfig.appName, thisconfig.name);;
						Console.WriteLine(string.Format("Generating ingress - {0}", ing));
						IngressGenerator ingress = new IngressGenerator();
						access.WriteOut(ingress.CreateIngress(thisconfig.appName, domain), string.Format("{0}/{1}", path, ing));
					}
					#endregion

					#region Autoscaler
					string hpa = string.Format("{0}-{1}-hpa.yml", thisconfig.appName, thisconfig.name);
					Console.WriteLine(string.Format("Generating autoscaler - {0}", hpa));
					AutoscalerGenerator autoscaler = new AutoscalerGenerator();
					access.WriteOut(autoscaler.CreateAutoscaler(thisconfig.appName, thisconfig.name), string.Format("{0}/{1}", path, hpa));
					#endregion		
					
					Console.WriteLine();		
				}
			Console.WriteLine("Finished");
		}
	}
}
