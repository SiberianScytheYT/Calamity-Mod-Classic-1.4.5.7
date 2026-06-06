using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Magic
{
    public class VenusianTrident : ModItem
    {
        public static int BaseDamage = 180;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Venusian Trident");
/*
            Tooltip.SetDefault("Casts an infernal trident that erupts into a gigantic explosion of fire and magma shards");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = BaseDamage;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 9f;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item45;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<VenusianBolt>();
            Item.shootSpeed = 19f;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(15, 15);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.InfernoFork);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 2);
            recipe.AddIngredient(ModContent.ItemType<TwistingNether>());
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
