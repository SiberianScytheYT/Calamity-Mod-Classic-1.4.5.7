using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class ExoGladius : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Exo Gladius");
/*
            Tooltip.SetDefault("Do not underestimate the power of Exoblade's younger brother\n" +
                "Striking an enemy with the blade makes you immune for a short time and summons comets from the sky\n" +
                "Fires a rainbow orb that summons sword beams on hit");
*/
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.useTurn = false;
            Item.useAnimation = 12;
            Item.useTime = 12;
            Item.width = 56;
            Item.height = 56;
            Item.damage = 2700;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.knockBack = 9.9f;
            Item.UseSound = SoundID.Item1;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<ExoGladProj>();
            Item.shootSpeed = 19f;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, Item.shootSpeed * player.direction, 0f, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<GalileoGladius>());
            recipe.AddIngredient(ModContent.ItemType<CosmicShiv>());
            recipe.AddIngredient(ModContent.ItemType<Lucrecia>());
			recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
			recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 107);
            }
        }

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.ExoDebuffs();
			OnHitEffects(player);
		}

		public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
		{
			target.ExoDebuffs();
			OnHitEffects(player);
		}

        private void OnHitEffects(Player player)
        {
            if (!player.immune)
            {
                player.immune = true;
                if (player.immuneTime < 10)
                    player.immuneTime = 10;
                if (player.hurtCooldowns[0] < 10)
                    player.hurtCooldowns[0] = 10;
                if (player.hurtCooldowns[1] < 10)
                    player.hurtCooldowns[1] = 10;
            }

            if (player.whoAmI == Main.myPlayer)
            {
                int damage = player.GetWeaponDamage(player.ActiveItem());
                CalamityUtils.ProjectileRain(player.GetSource_FromThis(), player.Center, 400f, 100f, 500f, 800f, 25f, ModContent.ProjectileType<ExoGladComet>(), damage, 15f, player.whoAmI);
            }
        }
	}
}
