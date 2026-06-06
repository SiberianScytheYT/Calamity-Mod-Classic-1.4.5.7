using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class TemporalFloeSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Temporal Floe Sword");
/*
            Tooltip.SetDefault("The iceman cometh...\n" +
                "Fires a frozen sword beam that splits into two more beams\n" +
				"The direction of the beams depends on in game time");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 52;
            Item.damage = 95;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 16;
            Item.useTurn = true;
            Item.knockBack = 6;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 60;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<TemporalFloeSwordProjectile>();
            Item.shootSpeed = 16f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, (int)(damage * 0.65), Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CryoBar>(), 15);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddTile(TileID.IceMachine);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 34);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
            }
            target.AddBuff(BuffID.Frostburn, 600);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
            }
            target.AddBuff(BuffID.Frostburn, 600);
        }
    }
}
