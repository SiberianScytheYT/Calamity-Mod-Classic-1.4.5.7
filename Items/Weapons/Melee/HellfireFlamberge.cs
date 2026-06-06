using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class HellfireFlamberge : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hellfire Flamberge");
/*
            Tooltip.SetDefault("Fires a spread of volcanic fireballs");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 58;
            Item.damage = 90;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useTurn = true;
            Item.knockBack = 7.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 60;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<ChaosFlameSmall>();
            Item.shootSpeed = 20f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item20, player.position);
            for (int index = 0; index < 3; ++index)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-40, 41) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-40, 41) * 0.05f;
				float damageMult = 0.5f;
                switch (index)
                {
                    case 0:
                        type = ModContent.ProjectileType<ChaosFlameSmall>();
                        break;
                    case 1:
                        type = ModContent.ProjectileType<ChaosFlameSmall>();
                        break;
                    case 2:
                        type = ModContent.ProjectileType<ChaosFlameLarge>();
						damageMult = 0.75f;
                        break;
                    default:
                        break;
                }
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, (int)(damage * damageMult), Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>(), 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, Main.rand.NextBool(3) ? 16 : 174);
            }
            if (Main.rand.NextBool(5))
            {
				int smoke = Gore.NewGore(Entity.GetSource_FromThis(), new Vector2(hitbox.X, hitbox.Y), default, Main.rand.Next(375, 378), 0.75f);
				Main.gore[smoke].behindTiles = true;
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
