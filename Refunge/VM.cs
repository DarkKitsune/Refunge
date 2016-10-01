using System;
using System.Collections.Generic;

namespace Refunge
{
	public class VM
	{
		public int X = 0;
		public int Y = 0;
		public byte Direction = 0;
		public Grid Program;
		public Stack<string> Stack;

		public VM(Grid program)
		{
			Program = program;
			Stack = new Stack<string>();
		}

		public VM(int x, int y, byte direction, Grid program)
		{
			Program = program;
			X = x;
			Y = y;
			Direction = direction;
			Stack = new Stack<string>();
		}

		public VM(int x, int y, byte direction, Grid program, Stack<string> stack)
		{
			Program = program;
			X = x;
			Y = y;
			Direction = direction;
			Stack = stack;
		}

		public void Move()
		{
			switch (Direction)
			{
				case (0):
					X++;
					if (X >= Program.Width)
						X = 0;
					break;
				case (1):
					Y--;
					if (Y < 0)
						Y = Program.Height - 1;
					break;
				case (2):
					X--;
					if (X < 0)
						X = Program.Width - 1;
					break;
				case (3):
					Y++;
					if (Y >= Program.Height)
						Y = 0;
					break;
			}
		}

		public void Run()
		{
			bool exit = false;
			bool stringMode = false;
			Random randomizer = new Random();

			while (!exit)
			{
				bool cancelMove = false;
				string inst = Program.Content[X, Y];

				if (inst != null && inst.Length > 0)
				{
					if (stringMode)
					{
						if (inst == "\"")
							stringMode = false;
						else
							Stack.Push(inst);
					}
					else
					{
						double aD, bD;
						int aI, bI, cI;
						string aS, bS, cS;
						Location location;

						//Console.WriteLine("Inst=" + inst);

						switch (inst)
						{
							case ("."):
								Console.Write(Double.Parse(Stack.Pop()).ToString());
								break;
							
							case (","):
								Console.Write(Stack.Pop().Replace(@"\n", "\n"));
								break;
							
							case ("?"):
								Direction = (byte)randomizer.Next(4);
								break;
							
							case ("@"):
								exit = true;
								break;

							case (">"):
								Direction = 0;
								break;
							
							case ("^"):
								Direction = 1;
								break;
							
							case ("<"):
								Direction = 2;
								break;

							case ("v"):
								Direction = 3;
								break;

							case ("+"):
								bD = double.Parse(Stack.Pop());
								aD = double.Parse(Stack.Pop());
								Stack.Push((aD + bD).ToString());
								break;
							
							case ("-"):
								bD = double.Parse(Stack.Pop());
								aD = double.Parse(Stack.Pop());
								Stack.Push((aD - bD).ToString());
								break;
							
							case ("*"):
								bD = double.Parse(Stack.Pop());
								aD = double.Parse(Stack.Pop());
								Stack.Push((aD * bD).ToString());
								break;
							
							case ("/"):
								bD = double.Parse(Stack.Pop());
								aD = double.Parse(Stack.Pop());
								Stack.Push((aD / bD).ToString());
								break;
							
							case ("%"):
								bD = double.Parse(Stack.Pop());
								aD = double.Parse(Stack.Pop());
								Stack.Push((aD % bD).ToString());
								break;
							
							case ("!"):
								aD = double.Parse(Stack.Pop());
								if (aD == 0.0)
									Stack.Push("1");
								else
									Stack.Push("0");
								break;

							case ("`"):
								bD = double.Parse(Stack.Pop());
								aD = double.Parse(Stack.Pop());
								if (aD > bD)
									Stack.Push("1");
								else
									Stack.Push("0");
								break;

							case ("#"):
								Move();
								break;
							
							case ("_"):
								aD = double.Parse(Stack.Pop());
								if (aD == 0.0)
									Direction = 0;
								else
									Direction = 2;
								break;

							case ("|"):
								aD = double.Parse(Stack.Pop());
								if (aD == 0.0)
									Direction = 3;
								else
									Direction = 1;
								break;

							case ("i"):
								aD = double.Parse(Stack.Pop());
								if (aD == 0.0)
									Move();
								break;

							case ("\""):
								stringMode = true;
								break;

							case (":"):
								aS = Stack.Pop();
								Stack.Push(aS);
								Stack.Push(aS);
								break;

							case (@"\"):
								bS = Stack.Pop();
								aS = Stack.Pop();
								Stack.Push(bS);
								Stack.Push(aS);
								break;

							case ("$"):
								Stack.Pop();
								break;

							case ("g"):
								bI = (int)double.Parse(Stack.Pop());
								aI = (int)double.Parse(Stack.Pop());
								Stack.Push(Program.Content[aI, bI]);
								break;

							case ("p"):
								cI = (int)double.Parse(Stack.Pop());
								bI = (int)double.Parse(Stack.Pop());
								aS = Stack.Pop();
								Program.Content[bI, cI] = aS;
								break;

							case ("&"):
								Stack.Push(Console.ReadLine());
								break;

							case ("~"):
								Stack.Push(((char)Console.Read()).ToString());
								break;

							case ("b"):
								Stack.Push((X).ToString());
								Stack.Push((Y - 1).ToString());
								break;

							case ("n"):
								X = 0;
								Y++;
								cancelMove = true;
								if (Y >= Program.Height)
									Y = 0;
								break;

							case ("c"):
								aS = Stack.Pop();

								location = Program.FindLabel(aS);

								VM callVM = new VM(location.X, location.Y, 0, Program, Stack);
								callVM.Run();
								break;

							case ("j"):
								aS = Stack.Pop();

								location = Program.FindLabel(aS);

								X = location.X;
								Y = location.Y;
								cancelMove = true;
								break;

							case ("r"):
								aS = Stack.Pop();

								location = Program.FindLabel(aS);

								Stack.Push(Program.Content[location.X, location.Y]);
								break;

							case ("w"):
								bS = Stack.Pop();
								aS = Stack.Pop();

								location = Program.FindLabel(bS);

								Program.Content[location.X, location.Y] = aS;
								break;

							case ("l"):
								aS = Stack.Pop();

								location = Program.FindLabel(aS);

								Stack.Push(location.X.ToString());
								Stack.Push(location.Y.ToString());
								break;

							case ("'"):
								bS = Stack.Pop();
								aS = Stack.Pop();

								Stack.Push(aS + bS);
								break;

							case ("="):
								bS = Stack.Pop();
								aS = Stack.Pop();

								if (aS == bS)
									Stack.Push("1");
								else
									Stack.Push("0");
								break;

							case ("["):
								Direction = (byte)(((int)Direction + 1) % 4);
								break;

							case ("]"):
								Direction = (byte)(((int)Direction + 4 - 1) % 4);
								break;

							default:
								Stack.Push(inst);
								break;
						}
					}
					if (exit)
						break;
				}

				if (!cancelMove)
					Move();
			}
		}
	}
}

