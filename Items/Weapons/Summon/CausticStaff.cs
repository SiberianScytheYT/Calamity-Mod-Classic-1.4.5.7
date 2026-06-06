using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Weapons.Summon
{
    public class CausticStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Caustic Staff");
/*
            Tooltip.SetDefault("Summons a mini dragon to fight for you\n" +
                               "The dragon can inflict several debilitating debuffs if you hold a summon weapon or tool");
*/
        }

        public override void SetDefaults()
        {
			Item.mana = 10;
			Item.damage = 15;
			Item.useStyle = 1;
			Item.shootSpeed = 10f;
			Item.shoot = ModContent.ProjectileType<CausticStaffSummon>();
			Item.width = 26;
			Item.height = 28;
			Item.UseSound = SoundID.Item77;
			Item.useAnimation = Item.useTime = 25;
			Item.rare = 4;
			Item.noMelee = true;
			Item.knockBack = 2f;
			Item.DamageType = DamageClass.Summon;
            Item.value = Item.buyPrice(0, 12, 0, 0);
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
			Item.autoReuse = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI, 0f, 1f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("AnyEvilFlask", 5);
            recipe.AddIngredient(ItemID.Deathweed, 2);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddRecipeGroup("AnyEvilBar", 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
