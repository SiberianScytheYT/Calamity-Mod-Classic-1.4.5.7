using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Magic
{
    public class ArchAmaryllis : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Arch Amaryllis");
/*
            Tooltip.SetDefault("Casts a beaming flower that explodes into homing petals");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 75;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 66;
            Item.height = 68;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7.5f;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.UseSound = SoundID.Item109;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BeamingBolt>();
            Item.shootSpeed = 20f;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(15, 15);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<GleamingMagnolia>());
            recipe.AddIngredient(ItemID.FragmentNebula, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
