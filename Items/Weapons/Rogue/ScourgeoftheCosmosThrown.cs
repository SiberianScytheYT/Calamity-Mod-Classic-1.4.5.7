using CalRD.Items.Materials;
using CalRD.Projectiles.Hybrid;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class ScourgeoftheCosmosThrown : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scourge of the Cosmos");
/*
            Tooltip.SetDefault("Throws a bouncing cosmic scourge that emits tiny homing cosmic scourges on death and tile hits");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 18;
            Item.damage = 1100;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item109;
            Item.autoReuse = true;
            Item.height = 20;
            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<ScourgeoftheCosmosProj>();
            Item.shootSpeed = 15f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int javelin = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 1f);
			Main.projectile[javelin].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ScourgeoftheCorruptor);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 10);
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 10);
            recipe.AddIngredient(ModContent.ItemType<XerocPitchfork>(), 200);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
