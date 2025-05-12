using Dummiesman;
using System;
using System.Collections.Generic;
using System.Text;
using LocoMeshSplitter;
using UnityEngine;
using System.IO;
using System.Linq.Expressions;

namespace LocoMeshSplitter
{
	internal class MeshFinder
	{
		private static readonly string dvFolderPath = Directory.GetParent(Path.GetDirectoryName(Main.ModPath)).Parent.FullName;
		private static readonly string assetStudioPath = Path.Combine(Main.ModPath, @"AssetStudioModCLI_net472_win64_contained\AssetStudioModCLI.exe");
		private static readonly string assetStudioPathFull = System.IO.Path.GetFullPath(assetStudioPath);
		private static readonly string importPathFull = System.IO.Path.Combine(dvFolderPath, @"DerailValley_Data\resources.assets");
		private static readonly string exportPathFull = System.IO.Path.Combine(Main.ModPath, @"assets");
		private static readonly string gameVersionFileName = "_generated_in_game_version.dat";
		private static readonly string gameVersionFilePath = Path.Combine(exportPathFull, gameVersionFileName);
		
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

		//We want to get the necessary meshes from the  if we haven't generated them before,
		//or if the game has updated.
		private static bool? isMeshFindingNecessary;
		private static bool IsMeshFindingNecessary()
		{
			//we only need to run this method once when loading the game
			if (isMeshFindingNecessary is not null)
			{
				return (bool)isMeshFindingNecessary;
			}

			if (!File.Exists(gameVersionFilePath)) {
				//create file, store version
				Directory.CreateDirectory(exportPathFull);
				File.WriteAllText(gameVersionFilePath, UnityEngine.Application.version);
				isMeshFindingNecessary = true;
				return true;
			}
			else
			{
				//get game version, check if it's the same as what's in the file
				//if not, delete all current mesh files and regenerate them
				string gameVersion = UnityEngine.Application.version;
				string storedGameVersion = File.ReadAllText(gameVersionFilePath);
				if (gameVersion != storedGameVersion)
				{
					//delete all current mesh files, they're out of date
					//Directory.Delete(exportPathFull, true);
					DirectoryInfo exportDirectoryInfo = new DirectoryInfo(exportPathFull);
					foreach (FileInfo file in exportDirectoryInfo.GetFiles())
					{
						if (file.Name == gameVersionFileName)
						{
							continue;
						}
						file.Delete();
					}
					isMeshFindingNecessary = true;

					//create new game version file
					File.WriteAllText(gameVersionFilePath, UnityEngine.Application.version);

					return true;
				}
				else
				{
					isMeshFindingNecessary = false;
					return false;
				}
			}
		}

		internal static Mesh FindMesh(string meshName)
		{
			if (IsMeshFindingNecessary())
			{
				//Main.Logger.Log($"finding mesh '{meshName}'");
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
			}

			Mesh mesh = new OBJLoader().Load(exportPathFull + $"\\{meshName}.obj");
			return mesh;
		}

		internal static Mesh FindMesh(string meshName, int meshPathID)
		{
			string meshPath = Path.Combine(exportPathFull,	$"{meshName}.obj");
			//theoretically, we don't need to check if the file exists.
			//but I can imagine a few edge cases where the file doesn't exist even though
			//we thought it should... so we'll check anyway. It won't take up too much CPU
			if (IsMeshFindingNecessary() || !File.Exists(meshPath))
			{
				Main.Logger.Log($"finding mesh '{meshName}'");
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
			}

			Mesh mesh = new OBJLoader().Load(meshPath);
			return mesh;
		}
	}
}
