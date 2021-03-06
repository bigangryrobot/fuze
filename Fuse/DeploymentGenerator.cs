﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fuze
{
	/// <summary>
	/// This class is responsible for creating a structure which represents a deployment. 
	/// This is one of multiple services, designed for a pipeline, so that a kubernetes cluster namespace can be spun up for testing with services/ingress/pods or replicationcontroller.
	/// </summary>
	public class DeploymentGenerator
	{
		/// <summary>
		/// Creates an ingress rule for a given pipeline.
		/// Choices that have been made implicitly, are that træfik is running, and that the service is tested in 
		/// context of its correct placement (ie access goes through root on / )
		/// </summary>
		/// <param name="path">Path to save file</param>
		/// <param name="name">Ingress name</param>
		public List<string> CreateDeployment(string appName, string image, List<Dictionary<string,string>> configMaps, string envName)
		{
			#region Conventions for service - this has a dependency in kubernetes to the ingress!
			string deploymentName = string.Format("{0}-deploy", appName);
			string nameSpace = string.Format("{0}-{1}", appName, envName);
			string appSeviceName = string.Format("{0}-svc", appName);			
			#endregion

			List<string> file = new List<string>();
			file = WriteDeploymentMetadata(file, deploymentName, appName,nameSpace);
			file = WriteDeploymentSpec(file, appName, image, deploymentName);
			foreach (var configMap in configMaps) {
				foreach (var config in configMap) {
					file = WriteConfigMapMetadata(file, config.Value);
				}
			}

			return file;
		}

		/// <summary>
		/// Writes in this format to file: 
		/// 
		/// - name: env
        /// valueFrom:
        ///   configMapKeyRef:
        ///     name: env
        ///     key: env-value
		///   
		/// </summary>
		/// <param name="file">The structure of the file to write to, in this context its an service yml.</param>
		/// <param name="name">Name for the deployment in the kubernetes cluster</param>
		List<string> WriteConfigMapMetadata(List<string> file, string key)
		{
			#region Conventions for configMap, this must match conventions found in the ConfigMapGenerator
			string keyName = string.Format("{0}-value", key);	
			#endregion

			SharedMethods indent = new SharedMethods();
			file.Add(indent.Padding(1, string.Format("name: {0}", key)));
			file.Add(indent.Padding(1, "valueFrom:"));			
			file.Add(indent.Padding(2, "configMapKeyRef:"));			
			file.Add(indent.Padding(3, string.Format("name: {0}", key)));
			file.Add(indent.Padding(3, string.Format("key: {0}", keyName)));

			return file;
		}

		/// <summary>
		/// Writes in this format to file: 
		/// 
		/// apiVersion: extensions/v1beta1
		/// kind: Deployment
		/// metadata:
		///   name: deployment-name
		///   
		/// </summary>
		/// <param name="file">The structure of the file to write to, in this context its an service yml.</param>
		/// <param name="name">Name for the deployment in the kubernetes cluster</param>
		List<string> WriteDeploymentMetadata(List<string> file, string deploymentName, string appName,string nameSpace)
		{
			SharedMethods indent = new SharedMethods();
			file.Add("apiVersion: extensions/v1beta1");
			file.Add("kind: Deployment");
			file.Add("metadata:");
			file.Add(indent.Padding(1, string.Format("name: {0}", deploymentName)));
			file.Add(indent.Padding(1, string.Format("namespace: {0}", nameSpace)));
			file.Add(indent.Padding(1, "labels:"));			
			file.Add(indent.Padding(2, string.Format("app: {0}", appName)));
			file.Add(indent.Padding(2, "autoGenerated: true"));
			file.Add(indent.Padding(2, string.Format("generated: {0}", DateTime.Now)));
			return file;
		}

		/// <summary>
		/// Writes in this format to file: 
		/// 
		/// spec:
		///   replicas: 3
		///   template:
		///     metadata:
		///       labels:
		///         run: nginx
		///    	spec:
		///    	  containers:
		///    	  - name: nginx
		///    	    image: user/image:tag
		///    	    ports:
		///    		- containerPort: 80 
		///     		
		/// </summary>
		/// <param name="file">The structure of the file to write to, in this context its an service yml.</param>
		/// <param name="targetPort">The pod exposed on the deployed pods.</param>
		/// <param name="deploymentName">Name for the deployment in kubernetes.</param>
		/// <param name="image">Image from hub, in the format of user/image:tag </param>
		/// <param name="deploymentName">Name for deployment, typically name appended with something (ex name-deploy) </param>
		List<string> WriteDeploymentSpec(List<string> file, string appName, string image, string deploymentName)
		{
			SharedMethods indent = new SharedMethods();
			file.Add("spec:");
			file.Add(indent.Padding(1, "containers:"));
			file.Add(indent.Padding(1, string.Format("- name: {0}", appName)));
			file.Add(indent.Padding(2, string.Format("image: {0}", image)));
			file.Add(indent.Padding(2, "ports:"));
			file.Add(indent.Padding(2, "- name: http"));
			file.Add(indent.Padding(3, "containerPort: 1337"));
			file.Add(indent.Padding(2, "livenessProbe:"));
			file.Add(indent.Padding(2, "httpGet:"));
			file.Add(indent.Padding(3, "path: /version"));
			file.Add(indent.Padding(3, "port: default-port"));
			file.Add(indent.Padding(2, "initialDelaySeconds: 3"));
			file.Add(indent.Padding(2, "periodSeconds: 3"));
			file.Add(indent.Padding(2, "failureThreshold: 1"));
			file.Add(indent.Padding(2, "readinessProbe:"));
			file.Add(indent.Padding(2, "httpGet:"));
			file.Add(indent.Padding(3, "path: /version"));
			file.Add(indent.Padding(3, "port: default-port"));
			file.Add(indent.Padding(2, "initialDelaySeconds: 3"));
			file.Add(indent.Padding(2, "periodSeconds: 3"));
			file.Add(indent.Padding(2, "failureThreshold: 1"));
			file.Add(indent.Padding(2, "volumeMounts:"));
			file.Add(indent.Padding(2, "- mountPath: /var/run/secrets/kubernetes.io/serviceaccount"));
			file.Add(indent.Padding(3, "name: no-api-access-please"));
			file.Add(indent.Padding(3, "readOnly: true"));
			file.Add("volumes:");
			file.Add(indent.Padding(1, "- name: no-api-access-please"));
			file.Add(indent.Padding(2, "emptyDir: {}	"));
			return file;
		}
	}
}
