using System;

namespace SharpMC
{
	//Height: 256
	//Width: 16
	//Length: 16

	public class Chunk
	{
		private Vector2 location { get; set;}
		public Block[] Blocks = new Block[65536];

		public Chunk(Vector2 Location)
		{
			foreach (Block i in Blocks)
			{
				i.block = new Air();
			}
			location = Location;
		}

		public void SetBlock(Vector3 Location, BlockClass Block)
		{
			foreach(Block i in Blocks)
			{
				if (i.Location == Location) 
				{
					i.block = Block;
					break;
				}
			}
		}
	}
}

