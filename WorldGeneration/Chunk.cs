using System;
using System.Collections.Generic;

namespace SharpMC
{
	//Height: 256
	//Width: 16
	//Length: 16

	public class Chunk
	{
		private Vector2 location { get; set;}
		public List<Block> _Blocks = new List<Block>();

		public Chunk(Vector2 Location)
		{
			for (int i = 0; i < 65536; i++) {
				Block b = new Block ();
				b.block = new Blocks.Air ();
				_Blocks.Add (b);
			}
			location = Location;
		}

		public void SetBlock(Vector3 Location, BlockClass Block)
		{
			foreach(Block i in _Blocks)
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

