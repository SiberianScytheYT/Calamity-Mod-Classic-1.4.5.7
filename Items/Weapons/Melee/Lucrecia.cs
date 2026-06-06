using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Lucrecia : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lucrecia");
/*
            Tooltip.SetDefault("Finesse\n" +
                "Striking an enemy makes you immune for a short time\n" +
                "Using this weapon drains your life\n" +
                "Fires a DNA chain");
*/
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.useTurn = false;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.width = 58;
            Item.height = 58;
            Item.damage = 90;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.knockBack = 8.25f;
            Item.UseSound = SoundID.Item1;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DNA>();
            Item.shootSpeed = 32f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.statLife -= 5;
            if (player.lifeRegen > 0)
            {
                player.lifeRegen = 0;
            }
            player.lifeRegenTime = 0;
            if (Main.myPlayer == player.whoAmI)
            {
                player.HealEffect(-5, true);
            }
            if (player.statLife <= 0)
            {
                player.KillMe(PlayerDeathReason.ByCustomReason(player.name + "'s DNA was destroyed."), 1000.0, 0, false);
            }
            Projectile.NewProjectile(source, position.X, position.Y, Item.shootSpeed * player.direction, 0f, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CoreofCalamity>());
            recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 5);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 234);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!player.immune)
            {
                player.immune = true;
                player.immuneTime = 5;
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (!player.immune)
            {
                player.immune = true;
                player.immuneTime = 5;
            }
        }
    }
}
