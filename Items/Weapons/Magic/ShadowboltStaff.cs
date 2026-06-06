using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Magic
{
    public class ShadowboltStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shadowbolt Staff");
/*
            Tooltip.SetDefault("The more tiles and enemies the beam bounces off of or travels through the more damage the beam does");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 200;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.width = 58;
            Item.height = 56;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 8f;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item72;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Shadowbolt>();
            Item.shootSpeed = 6f;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(15, 15);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ArmoredShell>(), 3);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 2);
            recipe.AddIngredient(ItemID.ShadowbeamStaff);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
