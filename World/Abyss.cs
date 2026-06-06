using CalRD.Tiles.Abyss;
using CalRD.Tiles.FurnitureVoid;
using CalRD.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace CalRD.World
{
	public class Abyss : ModSystem
	{
		public static void PlaceAbyss()
		{
			int x = Main.maxTilesX;
			int y = Main.maxTilesY;
			int genLimit = x / 2;
			int rockLayer = (int)Main.rockLayer;

			int abyssChasmY = y - 250; //Underworld = y - 200
			CalamityWorld.abyssChasmBottom = abyssChasmY - 100; //850 small 1450 medium 2050 large
			int abyssChasmX = CalamityWorld.abyssSide ? genLimit - (genLimit - 135) : genLimit + (genLimit - 135); //2100 - 1965 = 135 : 2100 + 1965 = 4065

			bool tenebrisSide = true;
			if (abyssChasmX < genLimit)
				tenebrisSide = false;

			if (CalamityWorld.abyssSide)
			{
				for (int abyssIndex = 0; abyssIndex < abyssChasmX + 80; abyssIndex++) //235
				{
					for (int abyssIndex2 = 0; abyssIndex2 < abyssChasmY; abyssIndex2++)
					{
						Tile tile = Framing.GetTileSafely(abyssIndex, abyssIndex2);
						bool canConvert = tile.HasTile &&
							tile.TileType < TileID.Count &&
							tile.TileType != TileID.DyePlants &&
							tile.TileType != TileID.Trees &&
							tile.TileType != TileID.PalmTree &&
							tile.TileType != TileID.Sand &&
							tile.TileType != TileID.Containers &&
							tile.TileType != TileID.Coral &&
							tile.TileType != TileID.BeachPiles &&
							tile.TileType != ModContent.TileType<SulphurousSand>() &&
							tile.TileType != ModContent.TileType<SulphurousSandstone>();
						if (abyssIndex2 > rockLayer - WorldGen.genRand.Next(30))
						{
							if (abyssIndex > abyssChasmX + 75 - WorldGen.genRand.Next(30))
							{
								if (WorldGen.genRand.Next(4) == 0)
								{
									if (canConvert)
									{
										tile.WallType = (ushort)ModContent.WallType<AbyssGravelWall>();
										tile.TileType = (ushort)ModContent.TileType<AbyssGravel>();
									}
									else if (!tile.HasTile)
									{
										tile.Get<TileWallWireStateData>().HasTile = true;
										tile.WallType = (ushort)ModContent.WallType<AbyssGravelWall>();
										tile.TileType = (ushort)ModContent.TileType<AbyssGravel>();
									}
								}
							}
							else if (abyssIndex > abyssChasmX + 70)
							{
								if (WorldGen.genRand.Next(2) == 0)
								{
									if (canConvert)
									{
										tile.WallType = (ushort)ModContent.WallType<AbyssGravelWall>();
										tile.TileType = (ushort)ModContent.TileType<AbyssGravel>();
									}
									else if (!tile.HasTile)
									{
										tile.Get<TileWallWireStateData>().HasTile = true;
										tile.WallType = (ushort)ModContent.WallType<AbyssGravelWall>();
										tile.TileType = (ushort)ModContent.TileType<AbyssGravel>();
									}
								}
							}
							else
							{
								if (canConvert)
								{
									if (abyssIndex2 > (rockLayer + y * 0.262))
									{
										tile.TileType = (ushort)ModContent.TileType<Voidstone>();
										tile.WallType = (ushort)ModContent.WallType<VoidstoneWallUnsafe>();
									}
									else if (abyssIndex2 > (rockLayer + y * 0.143) && WorldGen.genRand.Next(3) == 0)
									{
										tile.TileType = (ushort)ModContent.TileType<Voidstone>();
										tile.WallType = (ushort)ModContent.WallType<VoidstoneWallUnsafe>();
									}
									else
									{
										tile.TileType = (ushort)ModContent.TileType<AbyssGravel>();
										tile.WallType = (ushort)ModContent.WallType<AbyssGravelWall>();
									}
								}
								else if (!tile.HasTile)
								{
									tile.Get<TileWallWireStateData>().HasTile = true;
									if (abyssIndex2 > (rockLayer + y * 0.262))
									{
										tile.TileType = (ushort)ModContent.TileType<Voidstone>();
										tile.WallType = (ushort)ModContent.WallType<VoidstoneWallUnsafe>();
									}
									else if (abyssIndex2 > (rockLayer + y * 0.143) && WorldGen.genRand.Next(3) == 0)
									{
										tile.TileType = (ushort)ModContent.TileType<Voidstone>();
										tile.WallType = (ushort)ModContent.WallType<VoidstoneWallUnsafe>();
									}
									else
									{
										tile.WallType = (ushort)ModContent.WallType<AbyssGravelWall>();
										tile.TileType = (ushort)ModContent.TileType<AbyssGravel>();
									}
								}
							}
						}
					}
				}
			}
			else
			{
				for (int abyssIndex = abyssChasmX - 80; abyssIndex < x; abyssIndex++) //3965
				{
					for (int abyssIndex2 = 0; abyssIndex2 < abyssChasmY; abyssIndex2++)
					{
						Tile tile = Framing.GetTileSafely(abyssIndex, abyssIndex2);
						bool canConvert = tile.HasTile &&
							tile.TileType < TileID.Count &&
							tile.TileType != TileID.DyePlants &&
							tile.TileType != TileID.Trees &&
							tile.TileType != TileID.PalmTree &&
							tile.TileType != TileID.Sand &&
							tile.TileType != TileID.Containers &&
							tile.TileType != TileID.Coral &&
							tile.TileType != TileID.BeachPiles;
						if (abyssIndex2 > rockLayer - WorldGen.genRand.Next(30))
						{
							if (abyssIndex < abyssChasmX - 75)
							{
								if (WorldGen.genRand.Next(4) == 0)
								{
									if (canConvert)
									{
										tile.WallType = (ushort)ModContent.WallType<AbyssGravelWall>();
										tile.TileType = (ushort)ModContent.TileType<AbyssGravel>();
									}
									else if (!tile.HasTile)
									{
										tile.Get<TileWallWireStateData>().HasTile = true;
										tile.WallType = (ushort)ModContent.WallType<AbyssGravelWall>();
										tile.TileType = (ushort)ModContent.TileType<AbyssGravel>();
									}
								}
							}
							else if (abyssIndex < abyssChasmX - 70)
							{
								if (WorldGen.genRand.Next(2) == 0)
								{
									if (canConvert)
									{
										tile.WallType = (ushort)ModContent.WallType<AbyssGravelWall>();
										tile.TileType = (ushort)ModContent.TileType<AbyssGravel>();
									}
									else if (!tile.HasTile)
									{
										tile.Get<TileWallWireStateData>().HasTile = true;
										tile.WallType = (ushort)ModContent.WallType<AbyssGravelWall>();
										tile.TileType = (ushort)ModContent.TileType<AbyssGravel>();
									}
								}
							}
							else
							{
								if (canConvert)
								{
									if (abyssIndex2 > (rockLayer + y * 0.262))
									{
										tile.TileType = (ushort)ModContent.TileType<Voidstone>();
										tile.WallType = (ushort)ModContent.WallType<VoidstoneWallUnsafe>();
									}
									else if (abyssIndex2 > (rockLayer + y * 0.143) && WorldGen.genRand.Next(3) == 0)
									{
										tile.TileType = (ushort)ModContent.TileType<Voidstone>();
										tile.WallType = (ushort)ModContent.WallType<VoidstoneWallUnsafe>();
									}
									else
									{
										tile.TileType = (ushort)ModContent.TileType<AbyssGravel>();
										tile.WallType = (ushort)ModContent.WallType<AbyssGravelWall>();
									}
								}
								else if (!tile.HasTile)
								{
									tile.Get<TileWallWireStateData>().HasTile = true;
									if (abyssIndex2 > (rockLayer + y * 0.262))
									{
										tile.TileType = (ushort)ModContent.TileType<Voidstone>();
										tile.WallType = (ushort)ModContent.WallType<VoidstoneWallUnsafe>();
									}
									else if (abyssIndex2 > (rockLayer + y * 0.143) && WorldGen.genRand.Next(3) == 0)
									{
										tile.TileType = (ushort)ModContent.TileType<Voidstone>();
										tile.WallType = (ushort)ModContent.WallType<VoidstoneWallUnsafe>();
									}
									else
									{
										tile.WallType = (ushort)ModContent.WallType<AbyssGravelWall>();
										tile.TileType = (ushort)ModContent.TileType<AbyssGravel>();
									}
								}
							}
						}
					}
				}
			}

			WorldGenerationMethods.ChasmGenerator(abyssChasmX, (int)GenVars.worldSurfaceLow, CalamityWorld.abyssChasmBottom, true);

			int maxAbyssIslands = 11; //Small World
			if (y > 2100)
				maxAbyssIslands = 20; //Large World
			else if (y > 1500)
				maxAbyssIslands = 16; //Medium World

			WorldGenerationMethods.SpecialHut((ushort)ModContent.TileType<SmoothVoidstone>(), (ushort)ModContent.TileType<Voidstone>(),
				(ushort)ModContent.WallType<VoidstoneWallUnsafe>(), 8, abyssChasmX, CalamityWorld.abyssChasmBottom);

			int islandLocationOffset = 30;
			int islandLocationY = rockLayer;
			for (int islands = 0; islands < maxAbyssIslands; islands++)
			{
				int islandLocationX = abyssChasmX;
				int randomIsland = WorldGen.genRand.Next(5); //0 1 2 3 4
				bool hasVoidstone = islandLocationY > (rockLayer + y * 0.143);
				CalamityWorld.AbyssIslandY[CalamityWorld.numAbyssIslands] = islandLocationY;
				switch (randomIsland)
				{
					case 0:
						WorldGenerationMethods.AbyssIsland(islandLocationX, islandLocationY, 60, 66, 30, 36, true, false, hasVoidstone);
						WorldGenerationMethods.AbyssIsland(islandLocationX + 40, islandLocationY + 15, 60, 66, 30, 36, false, tenebrisSide, hasVoidstone);
						WorldGenerationMethods.AbyssIsland(islandLocationX - 40, islandLocationY + 15, 60, 66, 30, 36, false, !tenebrisSide, hasVoidstone);
						break;
					case 1:
						WorldGenerationMethods.AbyssIsland(islandLocationX + 30, islandLocationY, 60, 66, 30, 36, true, false, hasVoidstone);
						WorldGenerationMethods.AbyssIsland(islandLocationX, islandLocationY + 15, 60, 66, 30, 36, false, tenebrisSide, hasVoidstone);
						WorldGenerationMethods.AbyssIsland(islandLocationX - 40, islandLocationY + 10, 60, 66, 30, 36, false, !tenebrisSide, hasVoidstone);
						islandLocationX += 30;
						break;
					case 2:
						WorldGenerationMethods.AbyssIsland(islandLocationX - 30, islandLocationY, 60, 66, 30, 36, true, false, hasVoidstone);
						WorldGenerationMethods.AbyssIsland(islandLocationX + 30, islandLocationY + 10, 60, 66, 30, 36, false, tenebrisSide, hasVoidstone);
						WorldGenerationMethods.AbyssIsland(islandLocationX, islandLocationY + 15, 60, 66, 30, 36, false, !tenebrisSide, hasVoidstone);
						islandLocationX -= 30;
						break;
					case 3:
						WorldGenerationMethods.AbyssIsland(islandLocationX + 25, islandLocationY, 60, 66, 30, 36, true, false, hasVoidstone);
						WorldGenerationMethods.AbyssIsland(islandLocationX - 5, islandLocationY + 5, 60, 66, 30, 36, false, tenebrisSide, hasVoidstone);
						WorldGenerationMethods.AbyssIsland(islandLocationX - 35, islandLocationY + 15, 60, 66, 30, 36, false, !tenebrisSide, hasVoidstone);
						islandLocationX += 25;
						break;
					case 4:
						WorldGenerationMethods.AbyssIsland(islandLocationX - 25, islandLocationY, 60, 66, 30, 36, true, false, hasVoidstone);
						WorldGenerationMethods.AbyssIsland(islandLocationX + 5, islandLocationY + 15, 60, 66, 30, 36, false, tenebrisSide, hasVoidstone);
						WorldGenerationMethods.AbyssIsland(islandLocationX + 35, islandLocationY + 5, 60, 66, 30, 36, false, !tenebrisSide, hasVoidstone);
						islandLocationX -= 25;
						break;
				}

				CalamityWorld.AbyssIslandX[CalamityWorld.numAbyssIslands] = islandLocationX;
				CalamityWorld.numAbyssIslands++;

				islandLocationY += islandLocationOffset;
				if (islandLocationY >= CalamityWorld.abyssChasmBottom - 50)
					break;
			}

			CalamityWorld.AbyssItemArray = CalamityUtils.ShuffleArray(CalamityWorld.AbyssItemArray);
			for (int abyssHouse = 0; abyssHouse < CalamityWorld.numAbyssIslands; abyssHouse++) //11 15 19
			{
				if (abyssHouse != 20)
					WorldGenerationMethods.AbyssIslandHouse(CalamityWorld.AbyssIslandX[abyssHouse],
						CalamityWorld.AbyssIslandY[abyssHouse],
						CalamityWorld.AbyssItemArray[abyssHouse > 9 ? (abyssHouse - 10) : abyssHouse], //10 choices 0 to 9
						CalamityWorld.AbyssIslandY[abyssHouse] > (rockLayer + y * 0.143));
			}

			if (CalamityWorld.abyssSide)
			{
				for (int abyssIndex = 0; abyssIndex < abyssChasmX + 80; abyssIndex++) //235
				{
					for (int abyssIndex2 = 0; abyssIndex2 < abyssChasmY; abyssIndex2++)
					{
						if (!Main.tile[abyssIndex, abyssIndex2].HasTile)
						{
							if (WorldGen.SolidTile(abyssIndex, abyssIndex2 + 1) &&
								abyssIndex2 > rockLayer)
							{
								if (WorldGen.genRand.Next(5) == 0)
								{
									WorldGen.PlacePot(abyssIndex, abyssIndex2, (ushort)ModContent.TileType<AbyssalPots>());
									CalamityUtils.SafeSquareTileFrame(abyssIndex, abyssIndex2, true);
								}
							}
							else if (WorldGen.SolidTile(abyssIndex, abyssIndex2 + 1) &&
									 abyssIndex2 < (int)Main.worldSurface)
							{
								if (WorldGen.genRand.Next(3) == 0)
								{
									WorldGen.PlacePot(abyssIndex, abyssIndex2, (ushort)ModContent.TileType<SulphurousPots>());
									CalamityUtils.SafeSquareTileFrame(abyssIndex, abyssIndex2, true);
								}
							}
						}
					}
				}
			}
			else
			{
				for (int abyssIndex = abyssChasmX - 80; abyssIndex < x; abyssIndex++) //3965
				{
					for (int abyssIndex2 = 0; abyssIndex2 < abyssChasmY; abyssIndex2++)
					{
						if (!Main.tile[abyssIndex, abyssIndex2].HasTile)
						{
							if (WorldGen.SolidTile(abyssIndex, abyssIndex2 + 1) &&
								abyssIndex2 > rockLayer)
							{
								if (WorldGen.genRand.Next(5) == 0)
								{
									WorldGen.PlacePot(abyssIndex, abyssIndex2, (ushort)ModContent.TileType<AbyssalPots>());
									CalamityUtils.SafeSquareTileFrame(abyssIndex, abyssIndex2, true);
								}
							}
							else if (WorldGen.SolidTile(abyssIndex, abyssIndex2 + 1) &&
									 abyssIndex2 < (int)Main.worldSurface)
							{
								if (WorldGen.genRand.Next(3) == 0)
								{
									WorldGen.PlacePot(abyssIndex, abyssIndex2, (ushort)ModContent.TileType<SulphurousPots>());
									CalamityUtils.SafeSquareTileFrame(abyssIndex, abyssIndex2, true);
								}
							}
						}
					}
				}
			}
		}
	}
}
