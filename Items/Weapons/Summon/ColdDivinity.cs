using CalRD.Buffs.Summon;
using CalRD.Projectiles.Summon;
using CalRD.World;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class ColdDivinity : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cold Divinity");
/*
            Tooltip.SetDefault("Legendary Drop\n" +
							   "Summons the power of the ancient ice castle\n" +
                               "For each minion slot used, you will gain an additional orbiting shield spike\n" +
                               "These spikes accelerate rapidly towards a nearby enemy to inflict heavy damage\n" +
                               "They take some time to regenerate after launching themselves at the target, however\n" +
                               "On right click, summons a duplicate ring around the targeted enemy, which slowly converges before exploding\n" +
                               "Revengeance Drop");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.mana = 20;
            Item.width = 52;
            Item.height = 50;
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.knockBack = 4.5f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
            Item.UseSound = SoundID.Item30;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ColdDivinityPointyThing>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip7")
					{
						line2.Text = "Provides heat and cold protection in Death Mode when in use\n" +
						"Revengeance Drop";
					}
				}
			}
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float totalMinionSlots = 0f;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].minion && Main.projectile[i].owner == player.whoAmI)
                {
                    totalMinionSlots += Main.projectile[i].minionSlots;
                }
            }
            if (player.altFunctionUse != 2 && totalMinionSlots < player.maxMinions)
            {
                player.AddBuff(ModContent.BuffType<ColdDivinityBuff>(), 120, true);
                position = Main.MouseWorld;
                Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI);
                int pointyThingCount = 0;
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == type && Main.projectile[i].owner == player.whoAmI)
                    {
                        if (!(Main.projectile[i].ModProjectile as ColdDivinityPointyThing).circlingPlayer)
                            continue;
                        pointyThingCount++;
                    }
                }
                float angleVariance = MathHelper.TwoPi / pointyThingCount;
                float angle = 0f;
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == type && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].ai[1] == 0f)
                    {
                        if (!(Main.projectile[i].ModProjectile as ColdDivinityPointyThing).circlingPlayer)
                            continue;
                        Main.projectile[i].ai[0] = angle;
                        Main.projectile[i].netUpdate = true;
                        angle += angleVariance;
                        for (int j = 0; j < 22; j++)
                        {
                            Dust dust = Dust.NewDustDirect(Main.projectile[i].position, Main.projectile[i].width, Main.projectile[i].height, DustID.Ice);
                            dust.velocity = Vector2.UnitY * Main.rand.NextFloat(3f, 5.5f) * Main.rand.NextBool(2).ToDirectionInt();
                            dust.noGravity = true;
                        }
                    }
                }
            }
            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return base.AltFunctionUse(player);
        }
    }

}
