using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
	public class SubsumingVortex : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Subsuming Vortex");
/*
            Tooltip.SetDefault("Releases a gigantic, slow-moving vortex\n" +
                               "The vortex releases exo tentacles that thrash at nearby enemies\n" +
                               "After a few seconds the vortex slows down, becomes unstable, and explodes");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 1000;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 78;
            Item.width = 38;
            Item.height = 48;
            Item.UseSound = SoundID.Item84;
            Item.useTime = Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.rare = ItemRarityID.Red;
            Item.Calamity().customRarity = CalamityRarity.Violet;
            Item.value = Item.buyPrice(platinum: 2, gold: 50);
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EnormousConsumingVortex>();
            Item.shootSpeed = 7f;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Weapons/Magic/SubsumingVortexGlow").Value);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AuguroftheElements>());
            recipe.AddIngredient(ModContent.ItemType<EventHorizon>());
            recipe.AddIngredient(ModContent.ItemType<TearsofHeaven>());
			recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
			recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
