using CalRD.Items.Materials;
using CalRD.Projectiles.Melee.Spears;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class SausageMaker : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sausage Maker");
/*
            Tooltip.SetDefault("Sprays homing blood on hit");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.damage = 29;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.knockBack = 6.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 42;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.shoot = ModContent.ProjectileType<SausageMakerSpear>();
            Item.shootSpeed = 6f;
        }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodSample>(), 8);
            recipe.AddIngredient(ItemID.Vertebrae, 4);
            recipe.AddIngredient(ItemID.CrimtaneBar, 5);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
