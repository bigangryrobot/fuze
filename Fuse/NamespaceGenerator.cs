using System.Collections.Generic;
using System;

namespace Fuze
{
	/// <summary>
	/// This class is responsible for creating a structure which represents an namespace in a træfik cluster. 
	/// This is one of multiple services, designed for a pipeline, so that a kubernetes cluster namespace can be spun up for testing with services/namespace/pods or replicationcontroller.
	/// </summary>
	public class NameSpaceGenerator
	{
		/// <summary>
		/// Creates an namespace rule for a given pipeline.
		/// Choices that have been made implicitly, are that træfik is running, and that the service is tested in 
		/// context of its correct placement (ie access goes through root on / )
		/// </summary>
		/// <param name="path">Path to save file</param>
		/// <param name="name">Namespace name</param>
		/// <param name="namespaceName">Namespace name</param>
		public List<string> CreateNamespace(string name, string env)
		{
			#region Conventions for namespace
			string namespaceName = string.Format("{0}-{1}", name, env);
			#endregion

			List<string> file = new List<string>();
			file = WriteNamespaceMetadata(file, namespaceName);
			return file;
		}

		/// <summary>
		/// Writes in this format to file: 
		///   		
		/// apiVersion: v1
		///   kind: Namespace
		///   metadata:
		///     name: foo-env
		///   
		/// </summary>
		/// <param name="file">The structure of the file to write to, in this context its a namespace yml.</param>
		/// <param name="namespaceName">Name for the namespace in the kubernetes cluster</param>
		List<string> WriteNamespaceMetadata(List<string> file, string namespaceName)
		{
			SharedMethods indent = new SharedMethods(); 
			file.Add("apiVersion: v1");
			file.Add("kind: Namespace");
			file.Add("metadata:");
			file.Add(indent.Padding(1, string.Format("name: {0}", namespaceName)));			
			return file;
		}
	}
}