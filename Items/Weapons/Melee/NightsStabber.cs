using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class NightsStabber : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Night's Stabber");
/*
            Tooltip.SetDefault("Don't underestimate the power of stabby knives\n" +
                "Enemies release homing dark energy on death");
*/
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.useTurn = false;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.width = 28;
            Item.height = 34;
            Item.damage = 52;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.knockBack = 6.5f;
            Item.UseSound = SoundID.Item1;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AncientShiv>());
            recipe.AddIngredient(ModContent.ItemType<SporeKnife>());
            recipe.AddIngredient(ModContent.ItemType<FlameburstShortsword>());
            recipe.AddIngredient(ModContent.ItemType<LeechingDagger>());
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AncientShiv>());
            recipe.AddIngredient(ModContent.ItemType<SporeKnife>());
            recipe.AddIngredient(ModContent.ItemType<FlameburstShortsword>());
            recipe.AddIngredient(ModContent.ItemType<BloodyRupture>());
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 14);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.life <= 0)
            {
                for (int i = 0; i <= 2; i++)
                {
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<NightStabber>(), (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f)), Item.knockBack, Main.myPlayer);
                }
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (target.statLife <= 0)
            {
                for (int i = 0; i <= 2; i++)
                {
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<NightStabber>(), (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f)), Item.knockBack, Main.myPlayer);
                }
            }
        }
    }
}
