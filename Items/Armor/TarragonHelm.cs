using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class TarragonHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Tarragon Helm");
/*
            Tooltip.SetDefault("Helm of the disciple of ancients\n" +
				"Temporary immunity to lava\n" +
                "Can move freely through liquids\n" +
                "5% increased damage reduction\n" +
                "10% increased melee damage and critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.defense = 33; //98
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip4")
					{
						line2.Text = "10% increased melee damage and critical strike chance\n" +
						"Provides heat protection in Death Mode";
					}
				}
			}
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TarragonBreastplate>() && legs.type == ModContent.ItemType<TarragonLeggings>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.tarraSet = true;
            modPlayer.tarraMelee = true;
            string hotkey = CalRD.TarraHotKey.TooltipHotkeyString();
            player.setBonus = "Increased heart pickup range\n" +
                "Enemies have a chance to drop extra hearts on death\n" +
                "You have a 25% chance to gain a life regen buff when you take damage\n" +
                "Press " + hotkey + " to cloak yourself in life energy that heavily reduces enemy contact damage for 10 seconds\n" +
                "This has a 30 second cooldown";
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.1f;
            player.GetCritChance(DamageClass.Melee) += 10;
            player.endurance += 0.05f;
            player.lavaMax += 240;
            player.ignoreWater = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 7);
            recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 6);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
