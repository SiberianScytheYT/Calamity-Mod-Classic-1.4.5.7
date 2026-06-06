using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Magic
{
    public class Viscera : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Viscera");
/*
            Tooltip.SetDefault("Fires a blood beam that heals you on enemy hits\n" +
                "The more tiles and enemies the beam bounces off of or travels through the more healing the beam does");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 153;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.width = 50;
            Item.height = 52;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<VisceraBeam>();
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
            recipe.AddIngredient(ModContent.ItemType<BloodstoneCore>(), 4);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
