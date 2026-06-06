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
    public class AtaxiaHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hydrothermic Helm");
/*
            Tooltip.SetDefault("12% increased melee damage and 10% increased melee critical strike chance\n" +
                "12% increased melee and movement speed\n" +
                "Melee attacks and melee projectiles inflict on fire\n" +
                "Temporary immunity to lava and immunity to fire damage");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = 8;
            Item.defense = 33; //67
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip3")
					{
						line2.Text = "Temporary immunity to lava and immunity to fire damage\n" +
						"Provides heat protection in Death Mode";
					}
				}
			}
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AtaxiaArmor>() && legs.type == ModContent.ItemType<AtaxiaSubligar>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
			player.Calamity().hydrothermalSmoke = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "5% increased melee damage\n" +
                "Inferno effect when below 50% life\n" +
                "Melee attacks and projectiles cause chaos flames to erupt on enemy hits\n" +
                "You have a 20% chance to emit a blazing explosion when you are hit";
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.ataxiaBlaze = true;
            modPlayer.ataxiaGeyser = true;
            player.GetDamage(DamageClass.Melee) += 0.05f;
        }

        public override void UpdateEquip(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.ataxiaFire = true;
            player.GetAttackSpeed(DamageClass.Melee) += 0.12f;
            player.moveSpeed += 0.12f;
            player.GetDamage(DamageClass.Melee) += 0.12f;
            player.GetCritChance(DamageClass.Melee) += 10;
            player.lavaMax += 240;
            player.buffImmune[BuffID.OnFire] = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>(), 7);
			recipe.AddIngredient(ItemID.HellstoneBar, 4);
			recipe.AddIngredient(ModContent.ItemType<CoreofChaos>());
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
