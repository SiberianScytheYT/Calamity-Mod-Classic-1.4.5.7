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
    public class GalaxySmasherRogue : RogueWeapon
    {
        public static int BaseDamage = 390;
        public static float Speed = 18f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Galaxy Smasher");
/*
            Tooltip.SetDefault("Explodes and summons death lasers on enemy hits");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 86;
            Item.height = 72;
            Item.damage = BaseDamage;
            Item.knockBack = 9f;
            Item.useAnimation = 13;
            Item.useTime = 13;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
            Item.value = Item.buyPrice(1, 80, 0, 0);

            Item.Calamity().rogue = true;
            Item.shoot = ModContent.ProjectileType<GalaxySmasherHammer>();
            Item.shootSpeed = Speed;
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
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<StellarContemptRogue>());
            r.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            r.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>());
            r.AddTile(ModContent.TileType<DraedonsForge>());
            r.Register();
        }
    }
}
