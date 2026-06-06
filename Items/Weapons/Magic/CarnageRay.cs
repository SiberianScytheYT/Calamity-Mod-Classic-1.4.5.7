using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Magic
{
    public class CarnageRay : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Carnage Ray");
/*
            Tooltip.SetDefault("Fires a blood ray that splits if enemies are near it");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 46;
            Item.height = 46;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.25f;
            Item.value = Item.buyPrice(0, 12, 0, 0);
            Item.rare = 4;
            Item.UseSound = SoundID.Item72;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BloodRay>();
            Item.shootSpeed = 6f;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(12, 12);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.WandofSparking);
            recipe.AddIngredient(ItemID.CrimsonRod);
            recipe.AddIngredient(ItemID.AmberStaff);
            recipe.AddIngredient(ItemID.MagicMissile);
            recipe.AddIngredient(ModContent.ItemType<BloodSample>(), 15);
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
