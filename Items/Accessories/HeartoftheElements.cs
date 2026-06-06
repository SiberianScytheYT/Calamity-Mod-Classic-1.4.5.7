using CalRD.Buffs.Summon;
using CalRD.CalPlayer;
using CalRD.Projectiles.Summon;
using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalRD.Items.Accessories
{
    public class HeartoftheElements : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heart of the Elements");
/*
            Tooltip.SetDefault("The heart of the world\n" +
                "Increases max life by 20, life regen by 1, and all damage by 5%\n" +
                "Increases movement speed by 10% and jump speed by 20%\n" +
                "Increases damage reduction by 5%\n" +
                "Increases max mana by 50 and reduces mana usage by 5%\n" +
                "You grow flowers on the grass beneath you, chance to grow very random dye plants on grassless dirt\n" +
                "Summons all elementals to protect you\n" +
                "Toggling the visibility of this accessory also toggles the elementals on and off\n" +
                "Stat increases are slightly higher if the elementals are turned off");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 8));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.Rarity10BuyPrice;
            Item.rare = 10;
            Item.defense = 9;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.Rainbow;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)/* tModPorter Suggestion: Consider using new hook CanAccessoryBeEquippedWith */
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (modPlayer.brimstoneWaifu || modPlayer.sandWaifu || modPlayer.sandBoobWaifu || modPlayer.cloudWaifu || modPlayer.sirenWaifu)
            {
                return false;
            }
            return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			if (player != null && !player.dead)
			{
				Lighting.AddLight((int)player.Center.X / 16, (int)player.Center.Y / 16, Main.DiscoR / 255f, Main.DiscoG / 255f, Main.DiscoB / 255f);
			}
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.allWaifus = !hideVisual;
            modPlayer.elementalHeart = true;

			player.lifeRegen += hideVisual ? 2 : 1;
			player.statLifeMax2 += hideVisual ? 25 : 20;
			player.moveSpeed += hideVisual ? 0.12f : 0.1f;
			player.jumpSpeedBoost += hideVisual ? (player.autoJump ? 0.3f : 1.1f) : (player.autoJump ? 0.25f : 1f);
			player.endurance += hideVisual ? 0.06f : 0.05f;
			player.statManaMax2 += hideVisual ? 60 : 50;
			player.manaCost *= hideVisual ? 0.93f : 0.95f;
			player.GetDamage(DamageClass.Generic) += hideVisual ? 0.07f : 0.05f;

			int brimmy = ProjectileType<BrimstoneElementalMinion>();
			int siren = ProjectileType<WaterElementalMinion>();
			int healer = ProjectileType<SandElementalHealer>();
			int sandy = ProjectileType<SandElementalMinion>();
			int cloudy = ProjectileType<CloudElementalMinion>();

            if (!hideVisual)
            {
				Vector2 velocity = new Vector2(0f, -1f);
				int damage = NPC.downedMoonlord ? 150 : 90;
				float damageMult = CalamityWorld.downedDoG ? 2f : 1f;
				int elementalDmg = (int)(damage * damageMult * player.MinionDamage());
				float kBack = 2f + player.GetKnockback(DamageClass.Summon).Base;

				if (player.ownedProjectileCounts[brimmy] > 1 || player.ownedProjectileCounts[siren] > 1 ||
					player.ownedProjectileCounts[healer] > 1 || player.ownedProjectileCounts[sandy] > 1 ||
					player.ownedProjectileCounts[cloudy] > 1)
				{
					player.ClearBuff(BuffType<HotE>());
				}
				if (player != null && player.whoAmI == Main.myPlayer && !player.dead)
				{
					if (player.FindBuffIndex(BuffType<HotE>()) == -1)
					{
						player.AddBuff(BuffType<HotE>(), 3600, true);
					}
					if (player.ownedProjectileCounts[brimmy] < 1)
					{
						Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, velocity, brimmy, elementalDmg, kBack, player.whoAmI);
					}
					if (player.ownedProjectileCounts[siren] < 1)
					{
						Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, velocity, siren, elementalDmg, kBack, player.whoAmI);
					}
					if (player.ownedProjectileCounts[healer] < 1)
					{
						Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, velocity, healer, elementalDmg, kBack, player.whoAmI);
					}
					if (player.ownedProjectileCounts[sandy] < 1)
					{
						Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, velocity, sandy, elementalDmg, kBack, player.whoAmI);
					}
					if (player.ownedProjectileCounts[cloudy] < 1)
					{
						Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, velocity, cloudy, elementalDmg, kBack, player.whoAmI);
					}
				}
            }
            else
            {
                if (player.ownedProjectileCounts[brimmy] > 0 || player.ownedProjectileCounts[siren] > 0 ||
                    player.ownedProjectileCounts[healer] > 0 || player.ownedProjectileCounts[sandy] > 0 ||
                    player.ownedProjectileCounts[cloudy] > 0)
                {
                    player.ClearBuff(BuffType<HotE>());
                }
            }

			//Flower Boots code
            if (player != null && !player.dead && player.velocity.Y == 0f && player.grappling[0] == -1)
            {
                int x = (int)player.Center.X / 16;
                int y = (int)(player.position.Y + player.height - 1f) / 16;
				Tile tile = Main.tile[x, y];
                if (tile == null)
                {
                    tile = new Tile();
                }
                if (!tile.HasTile && tile.LiquidAmount == 0 && Main.tile[x, y + 1] != null && WorldGen.SolidTile(x, y + 1))
                {
                    tile.TileFrameY = 0;
                    tile.Get<TileWallWireStateData>().Slope = 0;
                    tile.Get<TileWallWireStateData>().IsHalfBlock = false;
					//On dirt blocks, there's a small chance to grow a dye plant
                    if (Main.tile[x, y + 1].TileType == TileID.Dirt)
                    {
                        if (Main.rand.NextBool(1000))
                        {
                            tile.Get<TileWallWireStateData>().HasTile = true;
                            tile.TileType = TileID.DyePlants;
                            tile.TileFrameX = (short)(34 * Main.rand.Next(1, 13));
                            while (tile.TileFrameX == 144)
                            {
                                tile.TileFrameX = (short)(34 * Main.rand.Next(1, 13));
                            }
                        }
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendTileSquare(-1, x, y, 1, TileChangeType.None);
                        }
                    }
					//On grass, grow flowers
                    if (Main.tile[x, y + 1].TileType == TileID.Grass)
                    {
                        if (Main.rand.NextBool(2))
                        {
                            tile.Get<TileWallWireStateData>().HasTile = true;
                            tile.TileType = TileID.Plants;
                            tile.TileFrameX = (short)(18 * Main.rand.Next(6, 11));
                            while (tile.TileFrameX == 144)
                            {
                                tile.TileFrameX = (short)(18 * Main.rand.Next(6, 11));
                            }
                        }
                        else
                        {
                            tile.Get<TileWallWireStateData>().HasTile = true;
                            tile.TileType = TileID.Plants2;
                            tile.TileFrameX = (short)(18 * Main.rand.Next(6, 21));
                            while (tile.TileFrameX == 144)
                            {
                                tile.TileFrameX = (short)(18 * Main.rand.Next(6, 21));
                            }
                        }
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendTileSquare(-1, x, y, 1, TileChangeType.None);
                        }
                    }
					//On hallowed grass, grow hallowed flowers
                    else if (Main.tile[x, y + 1].TileType == TileID.HallowedGrass)
                    {
                        if (Main.rand.NextBool(2))
                        {
                            tile.Get<TileWallWireStateData>().HasTile = true;
                            tile.TileType = TileID.HallowedPlants;
                            tile.TileFrameX = (short)(18 * Main.rand.Next(4, 7));
                            while (tile.TileFrameX == 90)
                            {
                                tile.TileFrameX = (short)(18 * Main.rand.Next(4, 7));
                            }
                        }
                        else
                        {
                            tile.Get<TileWallWireStateData>().HasTile = true;
                            tile.TileType = TileID.HallowedPlants2;
                            tile.TileFrameX = (short)(18 * Main.rand.Next(2, 8));
                            while (tile.TileFrameX == 90)
                            {
                                tile.TileFrameX = (short)(18 * Main.rand.Next(2, 8));
                            }
                        }
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendTileSquare(-1, x, y, 1, TileChangeType.None);
                        }
                    }
					//On jungle grass, grow jungle flowers
                    else if (Main.tile[x, y + 1].TileType == TileID.JungleGrass)
                    {
                        tile.Get<TileWallWireStateData>().HasTile = true;
                        tile.TileType = TileID.JunglePlants2;
                        tile.TileFrameX = (short)(18 * Main.rand.Next(9, 17));
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendTileSquare(-1, x, y, 1, TileChangeType.None);
                        }
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<WifeinaBottle>());
            recipe.AddIngredient(ItemType<WifeinaBottlewithBoobs>());
            recipe.AddIngredient(ItemType<LureofEnthrallment>());
            recipe.AddIngredient(ItemType<EyeoftheStorm>());
            recipe.AddIngredient(ItemType<RoseStone>());
            recipe.AddIngredient(ItemType<AeroStone>());
            recipe.AddIngredient(ItemType<CryoStone>());
            recipe.AddIngredient(ItemType<ChaosStone>());
            recipe.AddIngredient(ItemType<BloomStone>());
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
