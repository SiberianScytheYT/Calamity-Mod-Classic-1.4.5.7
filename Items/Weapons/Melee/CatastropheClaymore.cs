using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class CatastropheClaymore : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Catastrophe Claymore");
/*
            Tooltip.SetDefault("Fires explosive energy bolts");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 56;
            Item.damage = 67;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 23;
            Item.useTime = 23;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 56;
            Item.value = Item.buyPrice(0, 48, 0, 0);
            Item.rare = 6;
            Item.shoot = ModContent.ProjectileType<CalamityAura>();
            Item.shootSpeed = 11f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			type = Utils.SelectRandom(Main.rand, new int[]
			{
				ModContent.ProjectileType<CalamityAura>(),
				ModContent.ProjectileType<CalamityAuraType2>(),
				ModContent.ProjectileType<CalamityAuraType3>()
			});

            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, Main.myPlayer);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ItemID.CrystalShard, 15);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddRecipeGroup("CursedFlameIchor", 5);
            recipe.AddIngredient(ItemID.SoulofMight, 3);
            recipe.AddIngredient(ItemID.SoulofSight, 3);
            recipe.AddIngredient(ItemID.SoulofFright, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 73);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.Ichor, 60);
                target.AddBuff(BuffID.OnFire, 300);
                target.AddBuff(BuffID.Frostburn, 150);
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.Ichor, 60);
                target.AddBuff(BuffID.OnFire, 300);
                target.AddBuff(BuffID.Frostburn, 150);
            }
        }
    }
}
