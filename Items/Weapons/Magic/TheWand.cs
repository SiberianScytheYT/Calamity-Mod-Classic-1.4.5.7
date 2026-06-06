using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class TheWand : ModItem
    {
        public static int BaseDamage = 998;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Wand");
/*
            Tooltip.SetDefault("The ultimate wand");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.damage = 1;
            Item.mana = 150;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.useAnimation = 250;
            Item.useTime = 250;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0.5f;
            Item.UseSound = SoundID.Item102;
            Item.autoReuse = true;
            Item.height = 36;
            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<SparkInfernal>();
            Item.shootSpeed = 24f;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(10, 10);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.WandofSparking);
            recipe.AddIngredient(ModContent.ItemType<HellcasterFragment>(), 5);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 6);
            }
        }
    }
}
