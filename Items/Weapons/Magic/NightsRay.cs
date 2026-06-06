using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Magic
{
    public class NightsRay : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Night's Ray");
/*
            Tooltip.SetDefault("Fires a dark ray that splits if enemies are near it");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.25f;
            Item.value = Item.buyPrice(0, 12, 0, 0);
            Item.rare = 4;
            Item.UseSound = SoundID.Item72;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<NightsRayBeam>();
            Item.shootSpeed = 6f;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(15, 15);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.WandofSparking);
            recipe.AddIngredient(ItemID.Vilethorn);
            recipe.AddIngredient(ItemID.AmberStaff);
            recipe.AddIngredient(ItemID.MagicMissile);
            recipe.AddIngredient(ModContent.ItemType<TrueShadowScale>(), 15);
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
