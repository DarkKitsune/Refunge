using System;
using System.Collections.Generic;

namespace Refunge
{
	public class Grid
	{
		public string[,] Content;
		public int Width = 0;
		public int Height = 0;

		private Dictionary<string, Location> LabelLoc = new Dictionary<string, Location>();

		public Grid(string filename)
		{
			string[] lines = System.IO.File.ReadAllLines(filename);

			Height = lines.Length;

			foreach (string line in lines)
			{
				if (line.Length > Width)
					Width = line.Length;
			}

			Content = new string[Width, Height];

			int y = 0;
			foreach (string line in lines)
			{
				for (int x = 0; x < line.Length; x++)
				{
					char character = line[x];

					if (!IsWhitespace(character))
					{

						if (character == '{')
						{
							int oldX = x;

							x++;

							string label = "";
							while (x < line.Length)
							{
								char last = line[x];

								if (last == '}')
									break;
								else
									label += last;

								x++;
							}

							Content[oldX, y] = label;
						}
						else if (character == '(')
						{
							//int oldX = x;

							x++;

							string label = "";
							while (x < line.Length)
							{
								char last = line[x];

								if (last == ')')
									break;
								else
									label += last;

								x++;
							}

							LabelLoc[label] = new Location(x + 1, y);
						}
						else
						{
							Content[x, y] = "" + line[x];
						}

					}
				}
				y++;
			}
		}

		public Location FindLabel(string label)
		{
			return LabelLoc[label];
		}



		private static bool IsWhitespace(char character)
		{
			return (character <= 32);
		}
	}
}
