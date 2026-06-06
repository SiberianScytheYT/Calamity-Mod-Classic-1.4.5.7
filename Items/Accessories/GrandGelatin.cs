using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class GrandGelatin : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Grand Gelatin");
/*
            Tooltip.SetDefault("10% increased movement speed\n" +
                "40% increased jump speed\n" +
                "+20 max life and mana\n" +
                "Standing still boosts life and mana regen");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 24;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
            Item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            bool autoJump = Main.player[Main.myPlayer].autoJump;
			string jumpAmt = autoJump ? "10" : "40";
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
                {
                    line2.Text = jumpAmt + "% increased jump speed";
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed += 0.1f;
            player.jumpSpeedBoost += player.autoJump ? 0.5f : 2f;
            player.statLifeMax2 += 20;
            player.statManaMax2 += 20;
            if (Math.Abs(player.velocity.X) < 0.05f && Math.Abs(player.velocity.Y) < 0.05f && player.itemAnimation == 0)
            {
                player.lifeRegen += 2;
                player.manaRegenBonus += 2;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ManaJelly>());
            recipe.AddIngredient(ModContent.ItemType<LifeJelly>());
            recipe.AddIngredient(ModContent.ItemType<VitalJelly>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
