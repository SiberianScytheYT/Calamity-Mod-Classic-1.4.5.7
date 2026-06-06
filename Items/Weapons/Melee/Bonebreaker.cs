using CalRD.Projectiles.Melee;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
	public class Bonebreaker : ModItem
    {
		public const int BaseDamage = 60;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bonebreaker");
/*
            Tooltip.SetDefault("Fires javelins that stick to enemies before bursting into shrapnel");
*/
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.width = 32;
            Item.height = 32;
            Item.damage = BaseDamage;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<BonebreakerProjectile>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shootSpeed = 12f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BoneJavelin, 150);
            recipe.AddIngredient(ModContent.ItemType<CorrodedFossil>(), 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
