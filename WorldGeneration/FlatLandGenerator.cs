using System;
using System.Collections.Generic;

namespace SharpMC
{
	public class FlatLandGenerator
	{
		List<Chunk> chunks = new List<Chunk>();

		public void GenerateChunk(Vector2 Location)
		{
			Chunk i = new Chunk (Location);
			int xDone = 0;
			int yDone = 0;

			while (xDone < 16) 
			{
				while (yDone < 16) 
				{
					i.SetBlock (new Vector3 (xDone, yDone, 0), new Blocks.Dirt ());
					yDone++;
				}
				xDone++;
				yDone = 0;
			}
			chunks.Add (i);
		}

	}
}

