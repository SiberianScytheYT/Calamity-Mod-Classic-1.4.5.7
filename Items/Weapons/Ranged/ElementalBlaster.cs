using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class ElementalBlaster : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Elemental Blaster");
/*
            Tooltip.SetDefault("Does not consume ammo\n" +
                "Fires a storm of rainbow blasts");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 66;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 104;
            Item.height = 42;
            Item.useTime = 2;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.75f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/PlasmaBolt");
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<RainbowBlast>();
            Item.shootSpeed = 18f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SpectralstormCannon>());
            recipe.AddIngredient(ModContent.ItemType<ClockGatlignum>());
            recipe.AddIngredient(ModContent.ItemType<PaintballBlaster>());
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
