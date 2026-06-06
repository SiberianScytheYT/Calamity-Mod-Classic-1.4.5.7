using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
	public class AbandonedSlimeStaff : ModItem
    {
		int slimeSlots;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Abandoned Slime Staff");
/*
            Tooltip.SetDefault("Cast down from the heavens in disgust, this relic sings a song of quiet tragedy...\n" +
                               "Consumes all of the remaining minion slots on use\n" +
							   "Must be used from the hotbar\n" +
                               "Increased power and size based on the number of minion slots used\n" +
							   "Holding this weapon grants 20% increased jump speed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 62;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item44;

            Item.DamageType = DamageClass.Summon;
            Item.mana = 40;
            Item.damage = 56;
            Item.knockBack = 3f;
            Item.useTime = Item.useAnimation = 20;
            Item.shoot = ModContent.ProjectileType<AstrageldonSummon>();
            Item.shootSpeed = 10f;

            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.Calamity().customRarity = CalamityRarity.Dedicated; //rarity 21
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            bool autoJump = Main.player[Main.myPlayer].autoJump;
			string jumpAmt = autoJump ? "5" : "20";
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "Tooltip4")
                {
                    line2.Text = "Holding this weapon grants " + jumpAmt + "% increased jump speed";
                }
            }
        }

		public override void HoldItem(Player player)
        {
			//same boost as Aero Stone
			player.jumpSpeedBoost += player.autoJump ? 0.25f : 1f;

			double minionCount = 0;
			for (int j = 0; j < Main.projectile.Length; j++)
			{
                Projectile projectile = Main.projectile[j];
				if (projectile.active && projectile.owner == player.whoAmI && projectile.minion && projectile.type != ModContent.ProjectileType<AstrageldonSummon>())
				{
					minionCount += projectile.minionSlots;
				}
			}
			slimeSlots = (int)(player.maxMinions - minionCount);
		}

        public override bool CanUseItem(Player player)
		{
			return slimeSlots >= 1;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			CalamityUtils.KillShootProjectiles(true, type, player);
			float damageMult = ((float)Math.Log(slimeSlots, 8f)) + 1f;
			float size = ((float)Math.Log(slimeSlots, 10f)) + 1f;
            position = Main.MouseWorld;
            velocity.X = 0;
            velocity.Y = 0;
            int slime = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, (int)(damage * damageMult), Item.knockBack, player.whoAmI);
			Main.projectile[slime].Calamity().lineColor = slimeSlots;
			Main.projectile[slime].scale = size;
            return false;
        }
    }
}
