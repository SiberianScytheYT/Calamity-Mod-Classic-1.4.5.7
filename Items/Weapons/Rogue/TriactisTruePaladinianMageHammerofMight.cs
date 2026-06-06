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
    public class TriactisTruePaladinianMageHammerofMight : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Triactis' True Paladinian Mage-Hammer of Might");
/*
            Tooltip.SetDefault("Explodes on enemy hits");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 160;
            Item.damage = 10000;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.knockBack = 50f;
            Item.UseSound = SoundID.Item1;
            Item.height = 160;
            Item.value = Item.buyPrice(5, 0, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<TriactisOPHammer>();
            Item.shootSpeed = 25f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI);
            Main.projectile[proj].Calamity().forceRogue = true;
			Main.projectile[proj].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<GalaxySmasherRogue>());
            recipe.AddIngredient(ItemID.SoulofMight, 30);
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
