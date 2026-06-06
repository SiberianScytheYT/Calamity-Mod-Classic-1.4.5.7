using CalRD.Items.Placeables;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class SeashineSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Seashine Sword");
/*
            Tooltip.SetDefault("Shoots an aqua sword beam");
*/
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.EnchantedSword);
            Item.damage = 26;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.width = 38;
            Item.height = 38;
            Item.knockBack = 2;
            Item.shootSpeed = 11;
            Item.rare = 2;
            Item.shoot = Mod.Find<ModProjectile>("SeashineSwordProj").Type;
            Item.UseSound = SoundID.Item1;
        }

        //why was this done this way at all anyway lmao
        /*
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = ModContent.ProjectileType<SeashineSwordProj>();
            return base.Shoot(player, source, position, velocity, type, damage, Item.knockBack);
        }
        */

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SeaPrism>(), 7);
            recipe.AddIngredient(ModContent.ItemType<Navystone>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
