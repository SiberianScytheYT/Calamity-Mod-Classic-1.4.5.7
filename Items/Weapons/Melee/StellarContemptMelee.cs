using CalRD.Projectiles.Hybrid;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class StellarContemptMelee : ModItem
    {
        public static int BaseDamage = 350;
        public static float Speed = 18f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stellar Contempt");
/*
            Tooltip.SetDefault("Lunar flares rain down on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 64;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.damage = BaseDamage;
            Item.knockBack = 9f;
            Item.useTime = 13;
            Item.useAnimation = 13;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
            Item.value = Item.buyPrice(1, 20, 0, 0);

            Item.shoot = ModContent.ProjectileType<StellarContemptHammer>();
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
            r.AddIngredient(ModContent.ItemType<TruePaladinsHammerMelee>());
            r.AddIngredient(ItemID.LunarBar, 5);
            r.AddIngredient(ItemID.FragmentSolar, 10);
            r.AddIngredient(ItemID.FragmentNebula, 10);
            r.AddTile(TileID.LunarCraftingStation);
            r.Register();
        }
    }
}
