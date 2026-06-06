using CalRD.Items.Materials;
using CalRD.Projectiles.Healing;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class GrandGuardian : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Grand Guardian");
/*
            Tooltip.SetDefault("Has a chance to lower enemy defense by 15 when striking them\n" +
                       "If enemy defense is 0 or below your attacks will heal you\n" +
                       "Striking enemies causes a large explosion\n" +
                       "Striking enemies that have under half life will make you release rainbow bolts\n" +
                       "Enemies spawn healing orbs on death");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 124;
            Item.height = 124;
            Item.damage = 150;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 22;
            Item.useTurn = true;
            Item.knockBack = 8.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(1, 0, 0, 0);
            Item.rare = 10;
            Item.shootSpeed = 12f;
        }

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			player.itemLocation += new Vector2(-12f * player.direction, 12f * player.gravDir).RotatedBy(player.itemRotation);
		}

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(5))
            {
                target.defense -= 15;
            }
            if (target.defense <= 0 && target.canGhostHeal && !player.moonLeech)
            {
                player.statLife += 4;
                player.HealEffect(4);
            }
			OnHitEffects(player, target.Center, target.life, target.lifeMax, Item.knockBack);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
			OnHitEffects(player, target.Center, target.statLife, target.statLifeMax2, Item.knockBack);
        }

		private void OnHitEffects(Player player, Vector2 targetPos, int targetLife, int targetMaxLife, float knockback)
		{
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), targetPos, Vector2.Zero, ModContent.ProjectileType<RainbowBoom>(), (int)(Item.damage * player.MeleeDamage() * 0.5f), 0f, player.whoAmI);
            if (targetLife <= (targetMaxLife * 0.5f))
            {
                float randomSpeedX = (float)Main.rand.Next(9);
                float randomSpeedY = (float)Main.rand.Next(6, 15);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center.X, player.Center.Y, -randomSpeedX, -randomSpeedY, ModContent.ProjectileType<RainBolt>(), (int)(Item.damage * player.MeleeDamage() * 0.75f), knockback, player.whoAmI);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center.X, player.Center.Y, randomSpeedX, -randomSpeedY, ModContent.ProjectileType<RainBolt>(), (int)(Item.damage * player.MeleeDamage() * 0.75f), knockback, player.whoAmI);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center.X, player.Center.Y, 0f, -randomSpeedY, ModContent.ProjectileType<RainBolt>(), (int)(Item.damage * player.MeleeDamage() * 0.75f), knockback, player.whoAmI);
            }
            if (targetLife <= 0 && !player.moonLeech)
            {
                float randomSpeedX = (float)Main.rand.Next(9);
                float randomSpeedY = (float)Main.rand.Next(6, 15);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), targetPos.X, targetPos.Y, -randomSpeedX, -randomSpeedY, ModContent.ProjectileType<RainHeal>(), 0, 0f, player.whoAmI);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), targetPos.X, targetPos.Y, randomSpeedX, -randomSpeedY, ModContent.ProjectileType<RainHeal>(), 0, 0f, player.whoAmI);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), targetPos.X, targetPos.Y, 0f, -randomSpeedY, ModContent.ProjectileType<RainHeal>(), 0, 0f, player.whoAmI);
            }
		}

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 66, 0f, 0f, 100, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MajesticGuard>());
            recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 10);
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 3);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
