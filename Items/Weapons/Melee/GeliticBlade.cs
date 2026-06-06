using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class GeliticBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gelitic Blade");
/*
            Tooltip.SetDefault("Fires a gel wave that slows down on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.damage = 38;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 62;
            Item.value = Item.buyPrice(0, 12, 0, 0);
            Item.rare = 4;
            Item.shoot = ModContent.ProjectileType<GelWave>();
            Item.shootSpeed = 9f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 30);
            recipe.AddIngredient(ItemID.Gel, 35);
            recipe.AddIngredient(ItemID.HellstoneBar, 10);
            recipe.AddTile(ModContent.TileType<StaticRefiner>());
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 20);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Slimed, 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Slimed, 300);
        }
    }
}
