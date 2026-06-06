using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class MolecularManipulator : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Molecular Manipulator");
/*
            Tooltip.SetDefault("Is it nullable or not? Let's find out!\n" +
                "Fires a fast null bullet that distorts NPC stats\n" +
                "Uses your life as ammo");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 890;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 56;
            Item.height = 34;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 8f;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item33;
            Item.autoReuse = true;
            Item.shootSpeed = 25f;
            Item.shoot = ModContent.ProjectileType<NullShot2>();
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.statLife -= 5;
            if (player.statLife <= 0)
            {
                player.KillMe(PlayerDeathReason.ByCustomReason(player.Male ? player.name + " was vaporized by the imbuement of his life." : player.name + " was vaporized by the imbuement of her life."), 1000.0, 0, false);
            }
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<NullShot2>(), damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<NullificationRifle>());
            recipe.AddIngredient(ModContent.ItemType<DarkPlasma>(), 2);
            recipe.AddIngredient(ModContent.ItemType<CoreofCalamity>(), 3);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
