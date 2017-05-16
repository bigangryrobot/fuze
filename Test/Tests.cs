using Xunit;
using Fuze;
using System.Collections.Generic;

namespace Tests
{
	public class Tests
	{
		[Fact]
		public void IndentCheck()
		{
			SharedMethods access = new SharedMethods();
			string testIndent = access.FixIndention(0, "hello!");
			string testIndentOne = access.FixIndention(1, "hello!");
			string testIndentTwo = access.FixIndention(2, "hello!");

			Assert.Equal("hello!", testIndent);
			Assert.Equal("  hello!", testIndentOne);
			Assert.Equal("    hello!", testIndentTwo);
		}

		[Fact]
		public void IngressCreation()
		{
			IngressGenerator access = new IngressGenerator();
			List<string> ingress = access.CreateIngress("test");
			Assert.Equal(ingress[0], "apiVersion: extensions/v1beta1");
			Assert.Equal(ingress[1], "kind: Ingress");
			Assert.Equal(ingress[2], "metadata:");
			Assert.Equal(ingress[3], "  name: test-ing");
			Assert.Equal(ingress[4], "spec:");
			Assert.Equal(ingress[5], "  rules:");
			Assert.Equal(ingress[6], "  - host: test.local");
			Assert.Equal(ingress[7], "    http:");
			Assert.Equal(ingress[8], "      paths:");
			Assert.Equal(ingress[9], "      - path: /");
			Assert.Equal(ingress[10], "        backend:");
			Assert.Equal(ingress[11], "          serviceName: test-svc");
			Assert.Equal(ingress[12], "          servicePort: 80");
		}

		[Fact]
		public void ServiceCreation()
		{
			ServiceGenerator access = new ServiceGenerator();
			List<string> ingress = access.CreateService("test", "5000");
			Assert.Equal(ingress[0], "apiVersion: v1");
			Assert.Equal(ingress[1], "kind: Service");
			Assert.Equal(ingress[2], "metadata:");
			Assert.Equal(ingress[3], "  name: test-svc");
			Assert.Equal(ingress[4], "spec:");
			Assert.Equal(ingress[5], "  ports:");
			Assert.Equal(ingress[6], "  - port: 80");
			Assert.Equal(ingress[7], "    targetPort: 5000");
			Assert.Equal(ingress[8], "  selector:");
			Assert.Equal(ingress[9], "    run: test-deploy");
			Assert.Equal(ingress[10], "  type: ClusterIP");
		}

		[Fact]
		public void DeploymentCreation()
		{
			DeploymentGenerator access = new DeploymentGenerator();
			List<string> deployment = access.CreateDeployment("test", "5000", "3", "test/test:1.0.0");
			Assert.Equal(deployment[0], "apiVersion: extensions/v1beta1");
			Assert.Equal(deployment[1], "kind: Deployment");
			Assert.Equal(deployment[2], "metadata:");
			Assert.Equal(deployment[3], "  name: test-deploy");
			Assert.Equal(deployment[4], "spec:");
			Assert.Equal(deployment[5], "  replicas: 3");
			Assert.Equal(deployment[6], "  template:");
			Assert.Equal(deployment[7], "    metadata:");
			Assert.Equal(deployment[8], "      labels:");
			Assert.Equal(deployment[9], "        run: test-deploy");
			Assert.Equal(deployment[10], "    spec:");
			Assert.Equal(deployment[11], "      containers:");
			Assert.Equal(deployment[12], "      - name: test");
			Assert.Equal(deployment[13], "        image: test/test:1.0.0");
			Assert.Equal(deployment[14], "        ports:");
			Assert.Equal(deployment[15], "        - containerPort: 5000");				
		}
	}
}






