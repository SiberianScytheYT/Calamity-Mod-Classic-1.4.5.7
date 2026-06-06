using CalRD.Items.Materials;
using CalRD.Projectiles.Hybrid;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class GalaxySmasherMelee : ModItem
    {
        public static int BaseDamage = 480;
        public static float Speed = 18f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Galaxy Smasher");
/*
            Tooltip.SetDefault("Explodes and summons death lasers on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 86;
            Item.height = 72;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
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

            Item.shoot = ModContent.ProjectileType<GalaxySmasherHammer>();
            Item.shootSpeed = Speed;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI);
            Main.projectile[proj].Calamity().forceMelee = true;
            return false;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<StellarContemptMelee>());
            r.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            r.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>());
            r.AddTile(ModContent.TileType<DraedonsForge>());
            r.Register();
        }
    }
}
