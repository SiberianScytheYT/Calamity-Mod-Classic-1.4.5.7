using CalRD.Buffs.DamageOverTime;
using CalRD.Items.Materials;
using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Tools
{
    public class InfernaCutter : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Inferna Cutter");
/*
            Tooltip.SetDefault("Critical hits with the blade cause small explosions\n" +
                "Generates a number of small sparks when swung");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 110;
			Item.crit += 10;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 62;
            Item.height = 46;
            Item.useTime = 8;
            Item.useAnimation = 12;
            Item.useTurn = true;
            Item.axe = 27;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            hitbox = CalamityUtils.FixSwingHitbox(54, 54);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PurityAxe>());
            recipe.AddIngredient(ItemID.SoulofFright, 8);
            recipe.AddIngredient(ModContent.ItemType<EssenceofChaos>(), 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.itemAnimation == (int)(player.itemAnimationMax * 0.1) ||
                    player.itemAnimation == (int)(player.itemAnimationMax * 0.3) ||
                    player.itemAnimation == (int)(player.itemAnimationMax * 0.5) ||
                    player.itemAnimation == (int)(player.itemAnimationMax * 0.7) ||
                    player.itemAnimation == (int)(player.itemAnimationMax * 0.9))
                {
                    float num339 = 0f;
                    float num340 = 0f;
                    float num341 = 0f;
                    float num342 = 0f;
                    if (player.itemAnimation == (int)(player.itemAnimationMax * 0.9))
                    {
                        num339 = -7f;
                    }
                    if (player.itemAnimation == (int)(player.itemAnimationMax * 0.7))
                    {
                        num339 = -6f;
                        num340 = 2f;
                    }
                    if (player.itemAnimation == (int)(player.itemAnimationMax * 0.5))
                    {
                        num339 = -4f;
                        num340 = 4f;
                    }
                    if (player.itemAnimation == (int)(player.itemAnimationMax * 0.3))
                    {
                        num339 = -2f;
                        num340 = 6f;
                    }
                    if (player.itemAnimation == (int)(player.itemAnimationMax * 0.1))
                    {
                        num340 = 7f;
                    }
                    if (player.itemAnimation == (int)(player.itemAnimationMax * 0.7))
                    {
                        num342 = 26f;
                    }
                    if (player.itemAnimation == (int)(player.itemAnimationMax * 0.3))
                    {
                        num342 -= 4f;
                        num341 -= 20f;
                    }
                    if (player.itemAnimation == (int)(player.itemAnimationMax * 0.1))
                    {
                        num341 += 6f;
                    }
                    if (player.direction == -1)
                    {
                        if (player.itemAnimation == (int)(player.itemAnimationMax * 0.9))
                        {
                            num342 -= 8f;
                        }
                        if (player.itemAnimation == (int)(player.itemAnimationMax * 0.7))
                        {
                            num342 -= 6f;
                        }
                    }
                    num339 *= 1.5f;
                    num340 *= 1.5f;
                    num342 *= (float)player.direction;
                    num341 *= player.gravDir;
                    int spark = Projectile.NewProjectile(player.GetSource_ItemUse(Item), (float)(hitbox.X + hitbox.Width / 2) + num342, (float)(hitbox.Y + hitbox.Height / 2) + num341, (float)player.direction * num340, num339 * player.gravDir, ProjectileID.Spark, (int)(Item.damage * 0.2f * player.MeleeDamage()), 0f, player.whoAmI, 0f, 0f);
					Main.projectile[spark].Calamity().forceMelee = true;
                }
            }
            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 6);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                int boom = Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<FuckYou>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack, player.whoAmI, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
                Main.projectile[boom].Calamity().forceMelee = true;
            }
            target.AddBuff(BuffID.OnFire, 300);
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
        }
    }
}
