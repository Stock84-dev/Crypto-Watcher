using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace CryptoWatcher.Launcher
{
	public enum InstructionType { copy, delete, create, move, extract, createDir }
	// Instruction.txt contains serialized list of instructions
	// this class is used for creating, deleting, moving, copying and exrtacting files
	public class Instruction
	{
		public static List<Instruction> instructions = new List<Instruction>();

		public InstructionType iType;
		public string src;
		public string dest;

		public Instruction() { }

		Instruction(InstructionType instructionType, string source, string destination = "")
		{
			iType = instructionType;
			src = source;
			dest = destination;
		}

		public static bool Load(string file_path)
		{
			bool ret = true;
			FileStream fs = null;
			try
			{
				// creating reading filestream
				fs = new FileStream(file_path, FileMode.OpenOrCreate, FileAccess.Read);
				// if we don't have permission to read we exit
				if (!fs.CanRead)
					ret = false;

				byte[] buffer = new byte[4194304];// 4MB of space
				int bytesRead;
				bytesRead = fs.Read(buffer, 0, buffer.Length);
				// creating list of instruction by decoding bytes to string and then deserializing JSON format to object
				instructions = (List<Instruction>)new JavaScriptSerializer().Deserialize(Encoding.ASCII.GetString(buffer, 0, bytesRead), typeof(List<Instruction>));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				ret = false;
			}
			finally
			{
				if (fs != null)
				{
					// closing filestream
					fs.Flush();
					fs.Close();
				}
			}
			return ret;
		}

		public static void DoWork()
		{
			foreach (var instruction in instructions)
			{
				switch (instruction.iType)
				{
					case InstructionType.copy:
						File.Copy(instruction.src, instruction.dest == "" ? AppDomain.CurrentDomain.BaseDirectory : instruction.dest);
						break;
					case InstructionType.delete:
						File.Delete(instruction.src);
						break;
					case InstructionType.create:
						File.Create(instruction.src);
						break;
					case InstructionType.move:
						File.Move(instruction.src, AppDomain.CurrentDomain.BaseDirectory + instruction.dest);
						break;
					case InstructionType.extract:
						ZipFile.ExtractToDirectory(instruction.src, instruction.dest == "" ? AppDomain.CurrentDomain.BaseDirectory : instruction.dest);
						break;
					case InstructionType.createDir:
						Directory.CreateDirectory(instruction.src);
						break;
				}
			}
		}

		// this is used to create instruction file
		public static void SaveInstructions()
		{
			instructions.Add(new Instruction(InstructionType.delete, "CryptoWatcher.exe"));
			instructions.Add(new Instruction(InstructionType.move, "tmp/CryptoWatcher.exe", "CryptoWatcher.exe"));
			instructions.Add(new Instruction(InstructionType.move, "tmp/Newtonsoft.Json.dll", "Newtonsoft.Json.dll"));
			instructions.Add(new Instruction(InstructionType.move, "tmp/Tulpep.NotificationWindow.dll", "Tulpep.NotificationWindow.dll"));
			instructions.Add(new Instruction(InstructionType.delete, "CryptoWatcher.exe.config"));
			instructions.Add(new Instruction(InstructionType.move, "tmp/CryptoWatcher.exe.config", "CryptoWatcher.exe.config"));
			Save();
		}

		private static void Save()
		{
			// creating filestream that can write a file
			FileStream fs = new FileStream("Instructions.txt", FileMode.Create, FileAccess.Write);
			// if we don't have permission to write we exit function
			if (!fs.CanWrite)
				return;
			// creating JSON format from alerts list
			string str_alerts = new JavaScriptSerializer().Serialize(instructions);
			// converting string to byte array
			byte[] buffer = Encoding.ASCII.GetBytes(str_alerts);
			// writing whole buffer array
			fs.Write(buffer, 0, buffer.Length);
			// closing filestream
			fs.Flush();
			fs.Close();
		}
	}
}
