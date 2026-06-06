using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class FlarefrostBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flarefrost Blade");
/*
            Tooltip.SetDefault("Fires a homing flarefrost orb");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.damage = 65;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 24;
            Item.useTime = 24;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 66;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<Flarefrost>();
            Item.shootSpeed = 11f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CryoBar>(), 8);
            recipe.AddIngredient(ItemID.HellstoneBar, 8);
            recipe.AddIngredient(ItemID.HallowedBar, 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            int dustChoice = Main.rand.Next(2);
            if (dustChoice == 0)
            {
                dustChoice = 67;
            }
            else
            {
                dustChoice = 6;
            }
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, dustChoice);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
            target.AddBuff(BuffID.Frostburn, 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.OnFire, 300);
            target.AddBuff(BuffID.Frostburn, 300);
        }
    }
}
