﻿using System.Collections.Generic;
using System;

namespace Fuze
{
	/// <summary>
	/// This class is responsible for creating a structure which represents a service. 
	/// This is one of multiple services, designed for a pipeline, so that a kubernetes cluster namespace can be spun up for testing with services/ingress/pods or replicationcontroller.
	/// </summary>
	public class AutoscalerGenerator
	{
		/// <summary>
		/// Creates an horizontalpodautoscaler rule for a given pipeline.
		/// </summary>
		/// <param name="path">Path to save file</param>
		/// <param name="name">Ingress name</param>
		public List<string> CreateAutoscaler(string appName, string envName)
		{
			#region Conventions for hpa - this has a dependency in kubernetes to the ingress!
			string serviceName = string.Format("{0}-svc", appName);
			string nameSpace = string.Format("{0}-{1}", appName, envName);
			#endregion

			List<string> file = new List<string>();
			file = WriteAutoscalerMetadata(file, serviceName, nameSpace);
			file = WriteAutoscalerSpec(file, serviceName, nameSpace);

			return file;
		}

		/// <summary>
		/// Writes in this format to file: 
		/// 
		/// apiVersion: v1
		/// kind: HorizontalPodAutoscaler
		/// metadata:
		///   name: service-name
		///   
		/// </summary>
		/// <param name="file">The structure of the file to write to, in this context its an service yml.</param>
		/// <param name="nameSpace">Name for the service in the kubernetes cluster</param>
		/// <param name="serviceName">Name for the service in the kubernetes cluster</param>
		List<string> WriteAutoscalerMetadata(List<string> file, string serviceName, string nameSpace)
		{
			SharedMethods indent = new SharedMethods();
			file.Add("apiVersion: v1");
			file.Add("kind: HorizontalPodAutoscaler");
			file.Add("metadata:");
			file.Add(indent.Padding(1, string.Format("name: {0}", serviceName)));
			file.Add(indent.Padding(1, string.Format("namespace: {0}", nameSpace)));		
			file.Add(indent.Padding(1, "labels:"));
			file.Add(indent.Padding(2, string.Format("app: {0}", serviceName)));
			file.Add(indent.Padding(2, "autoGenerated: true"));
			file.Add(indent.Padding(2, string.Format("generated: {0}", DateTime.Now)));

			return file;
		}

		/// <summary>
		/// Writes in this format to file: 
		/// 
		///  spec:
		///    scaleRef:
		///      kind: Deployment
		///      name: service-name
		///      namespace: service-name
		///    minReplicas: 2
		///    maxReplicas: 10
		///    cpuUtilization:
		///      targetPercentage: 80
		///  
		/// </summary>
		/// <param name="file">The structure of the file to write to, in this context its an service yml.</param>
		/// <param name="targetPort">The pod exposed on the deployed pods.</param>
		/// <param name="serviceName">Name for the service in the kubernetes cluster</param>
		List<string> WriteAutoscalerSpec(List<string> file, string serviceName, string nameSpace)
		{
			SharedMethods indent = new SharedMethods();
			file.Add("spec:");
			file.Add(indent.Padding(1, "scaleRef:"));
			file.Add(indent.Padding(2, "kind: Deployment"));
			file.Add(indent.Padding(2, string.Format("name: {0}", serviceName)));
			file.Add(indent.Padding(2, string.Format("namespace: {0}", nameSpace)));
			file.Add(indent.Padding(1, "minReplicas: 2"));
			file.Add(indent.Padding(1, "maxReplicas: 15"));			
			file.Add(indent.Padding(2, "cpuUtilization:"));			
			file.Add(indent.Padding(3, "targetPercentage: 80"));
	
			return file;
		}
	}
}
