using System;

namespace SharpMC
{
	/*
	 * I guess we need something like a block class if we develop the world generator further.
	 * For now though, we will only have Grass, Dirt, Stone and Bedrock.
	 */

	public class Block
	{
		public Vector3 Location {get; set;}
		public BlockClass block {get; set;}
	}

	public class BlockClass
	{
		public BlockClass ()
		{
		}

		public int BlockID { get; set; }
		public string Name { get; set; }
		public BlockType Type { get; set; }
	}

	public class BlockTypeClass
	{
		public int Difficulty { get; set; }
		public string Name { get; set; }
		public BlockType Type { get; set;}
	}

	public enum BlockType
	{
		Dirt,
		Stone,
		Wood,
		Bedrock,
		Air
	}

	public class StoneType : BlockTypeClass
	{
		public int Difficulty = 1500;
		public string Name = "Stone";
		public BlockType Type = BlockType.Stone;
	}

	public class DirtType : BlockTypeClass
	{
		public int Difficulty = 500;
		public string Name = "Dirt";
		public BlockType Type = BlockType.Dirt;
	}

	public class WoodType : BlockTypeClass
	{
		public int Difficulty = 1200;
		public string Name = "Wood";
		public BlockType Type = BlockType.Wood;
	}

	public class BedrockType : BlockTypeClass
	{
		//For bedrock we set it to -1, as (in my code) that means Undestroyable.
		public int Difficulty = -1;
		public string Name = "Bedrock";
		public BlockType Type = BlockType.Bedrock;
	}

	public class BlockHelper
	{
		public static int GetDifficulty(BlockType type)
		{
			switch (type) 
			{
			case BlockType.Stone:
				return new StoneType ().Difficulty;
			case BlockType.Dirt:
				return new DirtType ().Difficulty;
			case BlockType.Wood:
				return new WoodType ().Difficulty;
			case BlockType.Bedrock:
				return new BedrockType ().Difficulty;
			case BlockType.Air:
				return -1;
			default:
				return 0;
			}
		}
	}
}

