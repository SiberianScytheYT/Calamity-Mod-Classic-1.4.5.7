using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class TrueCausticEdge : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("True Caustic Edge");
/*
            Tooltip.SetDefault("Fires a bouncing caustic beam\n" +
                "Inflicts on fire, poison, and venom");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.damage = 100;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 28;
            Item.useTurn = true;
            Item.knockBack = 5.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 74;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<TrueCausticEdgeProjectile>();
            Item.shootSpeed = 16f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, (int)(damage * 0.75), Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CausticEdge>());
            recipe.AddRecipeGroup("AnyEvilFlask", 5);
            recipe.AddIngredient(ItemID.FlaskofPoison, 5);
            recipe.AddIngredient(ItemID.Deathweed, 3);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 74);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 600);
            target.AddBuff(BuffID.OnFire, 300);
            target.AddBuff(BuffID.Venom, 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Poisoned, 600);
            target.AddBuff(BuffID.OnFire, 300);
            target.AddBuff(BuffID.Venom, 300);
        }
    }
}
