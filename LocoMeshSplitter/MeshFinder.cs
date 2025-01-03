using Dummiesman;
using System;
using System.Collections.Generic;
using System.Text;
using LocoMeshSplitter;
using UnityEngine;
using System.IO;

namespace LocoMeshSplitter
{
	internal class MeshFinder
	{
		private static readonly String dvFolderPath = Directory.GetParent(Path.GetDirectoryName(Main.ModPath)).Parent.FullName;
		private static readonly String assetStudioPath = Path.Combine(Main.ModPath, @"AssetStudioModCLI_net472_win64_contained\AssetStudioModCLI.exe");
		private static readonly String assetStudioPathFull = System.IO.Path.GetFullPath(assetStudioPath);
		private static readonly String importPathFull = System.IO.Path.Combine(dvFolderPath, @"DerailValley_Data\resources.assets");
		private static readonly String exportPathFull = System.IO.Path.Combine(Main.ModPath, @"assets");

		/*internal static Mesh FindMesh(string meshName)
		{
			//https://stackoverflow.com/questions/1469764/run-command-prompt-commands
			var process = new System.Diagnostics.Process();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.Start();
			String assetStudioCmd =
				$"{assetStudioPath} \"{importPathFull}\" " +
				$"-o \"{exportPathFull}\" " +
				$"-t mesh --filter-by-name \"{meshName}\" " +
				$"--log-output file";
			process.StandardInput.WriteLine(assetStudioCmd);
			process.StandardInput.Flush();
			process.StandardInput.Close();
			process.WaitForExit();
			Main.Logger.Log(process.StandardOutput.ReadToEnd());

			Mesh mesh = new OBJLoader().Load(exportPathFull + $"\\{meshName}.obj");
			return mesh;
		}*/

		internal static Mesh FindMesh(string meshName)
		{
			//https://stackoverflow.com/questions/1469764/run-command-prompt-commands
			var process = new System.Diagnostics.Process();
			process.StartInfo.FileName = assetStudioPathFull;
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.Arguments =
				$"\"{importPathFull}\" " +
				$"-o \"{exportPathFull}\" " +
				$"-t mesh " +
				$"--filter-by-name \"{meshName}\" ";
				//+ $"--log-level verbose --log-output both";

			process.Start();
			process.WaitForExit();
			//I might regret turning this off...
			//Main.Logger.Log(process.StandardOutput.ReadToEnd());

			Mesh mesh = new OBJLoader().Load(exportPathFull + $"\\{meshName}.obj");
			return mesh;
		}

		internal static Mesh FindMesh(string meshName, int meshPathID)
		{
			//https://stackoverflow.com/questions/1469764/run-command-prompt-commands
			var process = new System.Diagnostics.Process();
			process.StartInfo.FileName = assetStudioPathFull;
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.Arguments =
				$"\"{importPathFull}\" " +
				$"-o \"{exportPathFull}\" " +
				$"-t mesh " +
				$"--filter-by-pathid \"{meshPathID}\" " +
				$"--filter-by-name \"{meshName}\" ";
			//+ $"--log-level verbose --log-output both";

			process.Start();
			process.WaitForExit();
			//I might regret turning this off...
			//Main.Logger.Log(process.StandardOutput.ReadToEnd());

			Mesh mesh = new OBJLoader().Load(exportPathFull + $"\\{meshName}.obj");
			return mesh;
		}
	}
}
