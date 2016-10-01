using System;
using System.IO;

namespace Refunge
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			if (args.Length < 1 || !File.Exists(args[0]))
			{
				Console.WriteLine("Error: Argument should be an existing file!");
				return;
			}
			Grid test = new Grid(args[0]);

			VM vm = new VM(test);
			vm.Run();
		}
	}
}
