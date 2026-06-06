using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class Sponge : ModItem
    {

		public override string Texture => (DateTime.Now.Month == 4 && DateTime.Now.Day == 1) ? "CalRD/Items/Accessories/SpongeReal" : "CalRD/Items/Accessories/Sponge";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Sponge");
/*
            Tooltip.SetDefault("50% increased mining speed and you emit light\n" +
                "10% increased damage reduction and increased life regen\n" +
                "Poison, Freeze, Chill, Frostburn, and Venom immunity\n" +
                "Honey-like life regen with no speed penalty, +20 max life and mana\n" +
                "Most bee/hornet enemies and projectiles do 75% damage to you\n" +
                "24% increased jump speed and 12% increased movement speed\n" +
                "Standing still boosts life and mana regen\n" +
                "Increased defense and damage reduction when submerged in liquid\n" +
                "Increased movement speed when submerged in liquid\n" +
                "Enemies take damage when they hit you\n" +
                "Taking a hit will make you move very fast for a short time\n" +
                "You emit a mushroom spore and spark explosion when you are hit\n" +
                "Enemy attacks will have part of their damage absorbed and used to heal you");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 30));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.defense = 10;
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip12")
					{
						line2.Text = "Enemy attacks will have part of their damage absorbed and used to heal you\n" +
						"Provides cold protection in Death Mode";
					}
				}
			}
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.beeResist = true;
            modPlayer.aSpark = true;
            modPlayer.gShell = true;
            modPlayer.fCarapace = true;
            modPlayer.seaShell = true;
            modPlayer.absorber = true;
            modPlayer.aAmpoule = true;
            player.statManaMax2 += 20;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            if (Texture == "CalRD/Items/Accessories/Sponge")
            {
                Texture2D tex = ModContent.Request<Texture2D>("CalRD/Items/Accessories/SpongeShield").Value;
                spriteBatch.Draw(tex, Item.Center - Main.screenPosition + new Vector2(0f, 0f), Main.itemAnimations[Item.type].GetFrame(tex), Color.Cyan * 0.5f, 0f, new Vector2(tex.Width / 2f, (tex.Height / 30f ) * 0.8f), 1f, SpriteEffects.None, 0);
            }
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (Texture == "CalRD/Items/Accessories/Sponge")
            {
                Texture2D tex = ModContent.Request<Texture2D>("CalRD/Items/Accessories/SpongeShield").Value;
                spriteBatch.Draw(tex, position, Main.itemAnimations[Item.type].GetFrame(tex), Color.Cyan * 0.4f, 0f, origin, scale, SpriteEffects.None, 0);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TheAbsorber>());
            recipe.AddIngredient(ModContent.ItemType<AmbrosialAmpoule>());
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
