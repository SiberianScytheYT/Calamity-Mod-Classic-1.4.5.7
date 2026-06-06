using CalRD.Buffs.Cooldowns;
using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class PlagueReaperMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Plague Reaper Mask");
/*
            Tooltip.SetDefault("10% increased ranged damage and 8% increased ranged critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = 8;
            Item.defense = 9; //35
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PlagueReaperVest>() && legs.type == ModContent.ItemType<PlagueReaperStriders>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            string hotkey = CalRD.TarraHotKey.TooltipHotkeyString();
            player.setBonus = "25% reduced ammo usage and 5% increased flight time\n" +
                "Enemies receive 10% more damage from ranged projectiles when afflicted by the Plague\n" +
				"Getting hit causes the plague cinders to rain from above\n" +
                "Press " + hotkey + " to blind yourself for 5 seconds but massively boost your ranged damage\n" +
				"This has a 25 second cooldown.";
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.plagueReaper = true;
            player.ammoCost75 = true;

			if (modPlayer.plagueReaperCooldown > 1500)
			{
				player.blind = true;
				player.headcovered = true;
				player.blackout = true;
				player.GetDamage(DamageClass.Ranged) += 1f; //100% ranged dmg and 30% crit
				player.GetCritChance(DamageClass.Ranged) += 30;
			}
			if (modPlayer.plagueReaperCooldown == 1) //dust when ready to use again
			{
				for (int i = 0; i < 66; i++)
				{
					int d = Dust.NewDust(player.position, player.width, player.height, 89, 0, 0, 100, default, 1.5f);
					Main.dust[d].noGravity = true;
					Main.dust[d].velocity *= 6.6f;
				}
				SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/PlagueReaperRecharge"), player.Center);
			}
            if (modPlayer.plagueReaperCooldown == 1500)
            {
				player.AddBuff(ModContent.BuffType<PlagueBlackoutCooldown>(), 1500, false);
			}

            if (player.whoAmI == Main.myPlayer)
            {
                if (player.immune)
                {
                    if (Main.rand.NextBool(10))
                    {
						CalamityUtils.ProjectileRain(player.GetSource_FromThis(), player.Center, 400f, 100f, 500f, 800f, 22f, ModContent.ProjectileType<TheSyringeCinder>(), (int)(40 * player.RangedDamage()), 4f, player.whoAmI, 6);
                    }
                }
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.1f;
            player.GetCritChance(DamageClass.Ranged) += 8;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PlagueCellCluster>(), 15);
			recipe.AddIngredient(ItemID.NecroHelmet);
			recipe.AddIngredient(ItemID.Nanites, 11);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
			recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PlagueCellCluster>(), 15);
			recipe.AddIngredient(ItemID.AncientNecroHelmet);
			recipe.AddIngredient(ItemID.Nanites, 11);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
