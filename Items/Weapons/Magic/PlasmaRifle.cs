using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class PlasmaRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Plasma Rifle");
/*
            Tooltip.SetDefault("Fires a plasma blast that explodes\n" +
                "Right click to fire plasma bolts");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 183;
            Item.mana = 40;
            Item.DamageType = DamageClass.Magic;
            Item.width = 48;
            Item.height = 22;
            Item.useTime = Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/PlasmaBlast");
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<PlasmaShot>();
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.UseSound = new SoundStyle("CalRD/Sounds/Item/PlasmaBolt");
            }
            else
            {
                Item.UseSound = new SoundStyle("CalRD/Sounds/Item/PlasmaBlast");
            }
            return base.CanUseItem(player);
        }

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (player.altFunctionUse == 2)
				mult *= 0.25f;
		}

		public override float UseSpeedMultiplier(Player player)
		{
			if (player.altFunctionUse == 2)
				return 1f;
			return 0.2f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<PlasmaBolt>(), damage, Item.knockBack, player.whoAmI);
            }
            else
            {
                Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, (int)(damage * 0.8571), Item.knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 7);
            recipe.AddIngredient(ItemID.Musket);
            recipe.AddIngredient(ItemID.ToxicFlask);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 7);
            recipe.AddIngredient(ItemID.TheUndertaker);
            recipe.AddIngredient(ItemID.ToxicFlask);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
