using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
	public class BelladonnaSpiritStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Belladonna Spirit Staff");
/*
            Tooltip.SetDefault("Summons a cute forest spirit that scatters toxic petals");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.mana = 9;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 1;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BelladonnaSpirit>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Vine, 4);
            recipe.AddIngredient(ItemID.JungleSpores, 5);
            recipe.AddIngredient(ItemID.Stinger, 8);
            recipe.AddIngredient(ItemID.RichMahogany, 25);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
