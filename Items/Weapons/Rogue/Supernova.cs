using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Rogue
{
    public class Supernova : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Supernova");
/*
            Tooltip.SetDefault("Creates a massive explosion on impact\n"
                               +"Explodes into spikes and homing energy\n"
                               +"Stealth strikes release energy as they fly");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 34;
            Item.damage = 2250;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 24;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.height = 36;
            Item.value = Item.buyPrice(platinum: 2, gold: 50);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<SupernovaBomb>();
            Item.shootSpeed = 16f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<TotalityBreakers>());
            recipe.AddIngredient(ModContent.ItemType<BallisticPoisonBomb>());
            recipe.AddIngredient(ModContent.ItemType<ShockGrenade>(), 200);
            recipe.AddIngredient(ModContent.ItemType<Penumbra>());
            recipe.AddIngredient(ModContent.ItemType<StarofDestruction>());
			recipe.AddIngredient(ModContent.ItemType<SealedSingularity>());

			recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
			recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
